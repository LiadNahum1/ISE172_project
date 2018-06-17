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
        //the function gets  imessege and insert it to the data base
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
            }
        }
        //this function gets a message guid and new message content and eddit the message with this guid
        public void EditByGuid( string newMessageContent ,string messageGuid)
        {
            connection.Open();
            log.Info("user edit message");
            sql_query = "UPDATE [dbo].[Messages] SET [Body]='"+newMessageContent+"' ,[SendTime]='"+ DateTime.Now.ToUniversalTime() + "' WHERE Guid='"+messageGuid+"'";
            command = new SqlCommand(sql_query, connection);
        command.ExecuteNonQuery();
        command.Dispose();
        connection.Close();

    }
        //this function gets the message from the data base
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
                        sql_query = "SELECT TOP " + MAX_MESSAGES + " [Guid], [SendTime], [Body], [Group_id], [Nickname] " +
                            "From [dbo].[Messages] JOIN [dbo].[Users] on [Messages].[User_Id]=[Users].[Id] WHERE [SendTime] <='"+DateTime.Now.ToUniversalTime()+"' order by [SendTime] DESC;";
                        // todo check ascending or descending
                        command = new SqlCommand(sql_query, connection);
                    }
                    else// not the first retrieve
                    {
                        sql_query = "SELECT TOP " + MAX_MESSAGES + " [Guid], [SendTime], [Body], [Group_id], [Nickname] From [dbo].[Messages] " +
                            "JOIN [dbo].[Users] on [Messages].[User_Id]=[Users].[Id] WHERE [SendTime]>= @DatePar AND [SendTime] <='" + DateTime.Now.ToUniversalTime() + "' order by [SendTime] DESC;";                                                                                                       
                        command = new SqlCommand(sql_query, connection);
                        SqlParameter par = new SqlParameter("DatePar", DbType.DateTime) { Value = lastDate.ToUniversalTime() };
                        command.Parameters.Add(par);

                    }
                }
                else // there are filters
                {
                        String sql = "SELECT TOP " + MAX_MESSAGES + " [Messages].[Guid], [Messages].[SendTime], [Messages].[Body], [Users].[Group_Id], [Users].[Nickname] " +
                            "FROM [dbo].[Messages] JOIN [dbo].[Users] on [Users].[Id]=[Messages].[User_Id] WHERE";

                        for (int i = 0; i < filters.Count; i++)
                        {
                            sql = filters.ElementAt(i).execute(sql) + " AND ";
                        }
                    //     sql = sql.Substring(0, sql.Length - 5); // delete the last " AND"
                    sql = sql + "[SendTime] <= '" + DateTime.Now.ToUniversalTime() + "'";
                    command = new SqlCommand(sql, connection);

                    if (!isStart)
                    // return only the new filter messages
                    {
                        sql += " AND [SendTime]>=@DatePar";
                        command = new SqlCommand(sql, connection);
                        SqlParameter par = new SqlParameter("DatePar", DbType.DateTime) { Value = lastDate.ToUniversalTime() };
                        command.Parameters.Add(par);
                    }
                    sql += "order by[SendTime] DESC; ";
                }
                this.lastDate = DateTime.Now;
                data_reader = command.ExecuteReader();
                while (data_reader.Read())
                {
                    //date AND guid
                    DateTime date = new DateTime();
                    Guid guid = new Guid();
                    if (!data_reader.IsDBNull(0) & !data_reader.IsDBNull(1))
                    {
                        date = data_reader.GetDateTime(1); //2 is the coloumn index of the date. There are such               
                        try
                        {
                            guid = Guid.Parse(data_reader.GetString(0).Trim());
                        }
                        catch(Exception e)
                        {
                            log.Error("invalid parse to guid");
                        }
                    }

                    string msgContent = data_reader.GetString(2).Trim();
                    int groupId = (int)data_reader.GetValue(3);
                    string nickname = data_reader.GetString(4).Trim();
                    IMessage message = new Message(guid, nickname, groupId, date, msgContent);
                    newMessages.Add(message);
                }


                data_reader.Close();
                command.Dispose();
                connection.Close();
                return newMessages;
            }
            catch (Exception ex)
            {
                log.Error("Error" + ex.ToString());
                return newMessages;     
            }
        }
    }
}
