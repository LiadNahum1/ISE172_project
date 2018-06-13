using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ChatRoomProject.LogicLayer;

namespace ChatRoomProject.DataAccess
{
    public class MessageHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("MessageHandler.cs");
        private string sql_query;
        private string server_address;
        private string database_name;
        private string user_name;
        private string password;
        private string connetion_string;
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader data_reader;
        private IList<IQueryAction> filters;
        private DateTime lastDate;
        private const int MAX_MESSAGES = 200;
        public MessageHandler()
        {
            this.sql_query = null;
            this.server_address = "ise172.ise.bgu.ac.il,1433\\DB_LAB";
            this.database_name = "MS3";
            this.user_name = "publicUser";
            this.password = "isANerd";
            this.connetion_string = $"Data Source={server_address};Initial Catalog={database_name };User ID={user_name};Password={password}";
            this.connection = new SqlConnection(connetion_string);
            this.filters = new List<IQueryAction>();
            this.lastDate = new DateTime();
        }
        public void ClearFilters() { filters.Clear(); }
        public void AddGroupFilter(int groupId)
        {
            filters.Add(new GroupFilter(groupId));
        }
        public void AddNicknameFilter(string nickName)
        {
            filters.Add(new NicknameFilter(nickName));
        }
        public int GetUserId(IMessage msg)
        {
            try
            {
                connection.Open();
                sql_query = "SELECT [Id] From [dbo].[Users] WHERE [Users].[Nickname]='" + msg.UserName + "' AND [Users].[Group_Id]=" + msg.GroupID + ";";

                command = new SqlCommand(sql_query, connection);
                data_reader = command.ExecuteReader();
                int userId = 0;
                while (data_reader.Read())
                {
                    userId = (int)data_reader.GetValue(0);
                }
                data_reader.Close();
                command.Dispose();
                connection.Close();
                return userId;
            }
            catch (Exception e)
            {
                throw new Exception();   
            }
        }
            
        public  void InsertNewMessage(IMessage msg)
        {
            try
            {
                int userId = GetUserId(msg);
                connection.Open();
                command = new SqlCommand(null, connection);
                // Create and prepare an SQL statement.
                command.CommandText =
                    "INSERT INTO Messages ([Guid],[User_Id],[SendTime],[Body]) " +
                    "VALUES (@guid, @userId, @time,@body)";
                SqlParameter guid_param = new SqlParameter(@"guid", SqlDbType.Text, 68);
                SqlParameter user_id_param = new SqlParameter(@"userId", SqlDbType.Int);
                SqlParameter time_param = new SqlParameter(@"time", SqlDbType.DateTime);
                SqlParameter body_param = new SqlParameter(@"body", SqlDbType.Text, 100);
                guid_param.Value = msg.Id.ToString();
                user_id_param.Value = userId;
                time_param.Value = msg.Date;
                body_param.Value = msg.MessageContent;
                command.Parameters.Add(guid_param);
                command.Parameters.Add(user_id_param);
                command.Parameters.Add(time_param);
                command.Parameters.Add(body_param);

                // Call Prepare after setting the Commandtext and Parameters.
                command.Prepare();
                int num_rows_changed = command.ExecuteNonQuery();
                // data_reader.Close();
                command.Dispose();
                connection.Close();
                Console.WriteLine($"ExecuteNonQuery in SqlCommand executed!! {num_rows_changed.ToString()} row was changes\\inserted");
            }
            catch (Exception ex)
            {
                log.Error("Writing into Data Base failed");
                log.Error(ex.ToString());
            }
        }

        public void EditByGuid( string lastMessageContent,string messageGuid)
        {
            connection.Open();
            log.Info("connected to: " + server_address);//to continu
            SqlCommand Command = new SqlCommand(
              "UPDATE [dbo].[Messages] Body ='@content',Date= '@date' WHERE Guid='@id'" + connection);
            Command.Parameters.AddWithValue("@id", messageGuid);
            Command.Parameters.AddWithValue("@content", messageGuid);
            Command.Parameters.AddWithValue("@date", DateTime.Now.ToUniversalTime());
            Command.ExecuteNonQuery();
        }

        public List<IMessage> RetrieveMessages(bool isStart)
        {
            List<IMessage> newMessages = new List<IMessage>();
            try
            {
                connection.Open();
                log.Info("connected to: " + server_address);
                if (filters.Count == 0) // no filters 
                {
                    if (isStart) // first retrieve
                    {
                        sql_query = "SELECT TOP " + MAX_MESSAGES + " [Guid], [SendTime], [Body], [Group_id], [Nickname] From [dbo].[Messages] JOIN [dbo].[Users] " +
                            "on [Messages].[User_Id]=[Users].[Id]  order by [SendTime] DESC;";
                        // todo check ascending or descending
                        command = new SqlCommand(sql_query, connection);
                    }
                    else// not the first retrieve
                    {
                        sql_query = "SELECT TOP " + MAX_MESSAGES + " [Guid], [SendTime], [Body], [Group_id], [Nickname] From [dbo].[Messages] JOIN [dbo].[Users] " +
                            "on [Messages].[User_Id]=[Users].[Id]  WHERE [SendTime] > '" + lastDate + "' order by [SendTime] DESC;"; //TODO no more than 200                                                                                                      
                        command = new SqlCommand(sql_query, connection);
                    }
                }
                else // there are filters
                {
                    if (isStart)
                    {
                        // build sql to get the id element of all the useres with the spesifc nickname or groupid
                        List<int> auto_Id = new List<int>();
                        string sql_id = "SELECT [Id] FROM [dbo].[Users] WHERE";
                        for (int i = 0; i < filters.Count; i++)
                        {
                            sql_id = filters.ElementAt(i).execute(sql_id) + " AND";
                        }
                        sql_id = sql_id.Substring(0, sql_id.Length - 4); // delete the last " AND"
                        command = new SqlCommand(sql_id, connection);
                        data_reader = command.ExecuteReader();
                        while (data_reader.Read())
                        {
                            auto_Id.Add((int)(data_reader.GetValue(0)));
                        }
                        data_reader.Close();
                        command.Dispose();
                        connection.Close();
                        log.Info("close connection " + server_address);

                        connection.Open(); // open again
                        log.Info("connected to: " + server_address);

                        //sql of all the messages with the auto_id
                        String sql = "SELECT TOP " + MAX_MESSAGES + " [Messages].[Guid], [Messages].[SendTime], [Messages].[Body], [Users].[Group_Id], [Users].[Nickname] FROM [dbo].[Messages] JOIN [dbo].[Users] on ";
                        for (int i = 0; i < auto_Id.Count; i++)
                        {
                            sql += "[Users].[Id]=" + auto_Id[i] + " OR ";
                        }
                        sql=sql.Substring(0, sql.Length - 4); // delete the last OR
                        command = new SqlCommand(sql, connection);
                    }
                    else // return only the new filter messages
                    {
                        // build sql to get the id element of all the useres with the spesifc nickname or groupid
                        List<int> auto_Id = new List<int>();
                        string sql_id = "SELECT [Id] FROM [dbo].[Users] WHERE";
                        for (int i = 0; i < filters.Count; i++)
                        {
                            sql_id = filters.ElementAt(i).execute(sql_id) + " AND ";
                        }
                        sql_id = sql_id.Substring(0, sql_id.Length - 5); // delete the last " AND"
                        command = new SqlCommand(sql_id, connection);
                        data_reader = command.ExecuteReader();
                        while (data_reader.Read())
                        {
                            auto_Id.Add((int)(data_reader.GetValue(0)));
                        }
                        data_reader.Close();
                        command.Dispose();
                        connection.Close();
                        log.Info("close connection " + server_address);

                        connection.Open(); // open again
                        log.Info("connected to: " + server_address);

                        //sql of all the messages with the auto_id
                        String sql = "SELECT TOP " + MAX_MESSAGES + " [Messages].[Guid], [Messages].[SendTime], [Messages].[Body], [Users].[Group_Id], [Users].[Nickname] FROM [dbo].[Messages] JOIN [dbo].[Users] on ";
                        for (int i = 0; i < auto_Id.Count; i++)
                        {
                            sql += "[Users].[Id]=" + auto_Id[i] + " OR ";
                        }
                        sql=sql.Substring(0, sql.Length - 4); // delete the last OR
                        sql += " AND [SendTime]>'" + lastDate +"' order by [SendTime] DESC;";
                        //SqlParameter last_date = new SqlParameter(@"last_date", SqlDbType.DateTime, 20);
                        //last_date.Value = lastDate;
                        //command.Parameters.Add(last_date); // todo check
                        command = new SqlCommand(sql, connection);
                    }
                }
                /*
                    else // return only the new filter messages
                    {
                        String sql = "SELECT TOP " + MAX_MESSAGES + " * FROM[dbo].[Messages] WHERE";
                        for (int i = 0; i < filters.Count; i++)
                        {
                            sql = filters.ElementAt(i).execute(sql) + " AND";
                        }
                        sql += " [SendTime]>'" + lastDate + "' order by [SendTime] DESC;";
                        //SqlParameter last_date = new SqlParameter(@"last_date", SqlDbType.DateTime, 20);
                        //last_date.Value = lastDate;
                        //command.Parameters.Add(last_date); // todo check
                        command = new SqlCommand(sql_query, connection);
                    }
                }
                */
                data_reader = command.ExecuteReader();
                while (data_reader.Read())
                {
                    //date AND guid
                    DateTime date = new DateTime();
                    Guid guid = new Guid();
                    if (!data_reader.IsDBNull(0) & !data_reader.IsDBNull(1))
                    {
                        date = data_reader.GetDateTime(1); //2 is the coloumn index of the date. There are such               
                        guid = Guid.Parse(data_reader.GetString(0));
                    }
                    string msgContent = data_reader.GetString(2).Trim();
                    int groupId = (int)data_reader.GetValue(3);
                    string nickname = data_reader.GetString(4).Trim();
                    IMessage message = new Message(guid, nickname, groupId, date, msgContent);
                    newMessages.Add(message);
                }
                this.lastDate = DateTime.Now.ToUniversalTime();
               // if (newMessages.Count > 1) { 
               // lastDate = newMessages.ElementAt(newMessages.Count-1).Date;  //save the last date
           // }
                data_reader.Close();
                command.Dispose();
                connection.Close();
                return newMessages;
            }
            catch (Exception ex)
            {
                log.Error("Error" + ex.ToString());
                return null;
            }
        }
    }
}
