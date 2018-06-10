using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ChatRoomProject.DataAccess
{
    public class MessageHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("MessageHandler.cs");
        private static bool isStart = true;
        private static string sql_query = null;
        private static string server_address = "ise172.ise.bgu.ac.il,1433\\DB_LAB";
        private static string database_name = "MS3";
        private static string user_name = "publicUser";
        private static string password = "isANerd";
        private static string connetion_string = $"Data Source={server_address};Initial Catalog={database_name };User ID={user_name};Password={password}";
        private static SqlConnection connection = new SqlConnection(connetion_string);
        private static SqlCommand command;
        private static SqlDataReader data_reader;
        private IList<IQueryAction> filters = new List<IQueryAction>();
        private DateTime last_date;
        
        public void ClearFilters() { filters.Clear(); }
        public void AddGroupFilter(int groupId)
        {
            filters.Add(new GroupFilter(groupId));
        }
        public void AddNicknameFilter(string nickName)
        {

            filters.Add(new NicknameFilter(nickName));
        }
        public  void InsertNewMessage(Guid userId, IMessage msg)
        {
            try
            {
                connection.Open();
                command = new SqlCommand(null, connection);
                // Create and prepare an SQL statement.
                command.CommandText =
                    "INSERT INTO Messages ([Guid],[User_Id],[SendTime],[Body]) " +
                    "VALUES (@guid, @user_Id,@time,@body)";
                SqlParameter guid_param = new SqlParameter(@"guid", SqlDbType.Text, 20);
                SqlParameter user_Id_param = new SqlParameter(@"user_Id", SqlDbType.Text, 20);
                SqlParameter time_param = new SqlParameter(@"time", SqlDbType.Text, 20);
                SqlParameter body_param = new SqlParameter(@"body", SqlDbType.Text, 20);
                guid_param.Value = msg.Id;
                user_Id_param.Value = userId;
                time_param.Value = msg.Date;
                body_param.Value = msg.MessageContent;
                command.Parameters.Add(guid_param);
                command.Parameters.Add(user_Id_param);
                command.Parameters.Add(time_param);
                command.Parameters.Add(body_param);

                // Call Prepare after setting the Commandtext and Parameters.
                command.Prepare();
                int num_rows_changed = command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                Console.WriteLine($"ExecuteNonQuery in SqlCommand executed!! {num_rows_changed.ToString()} row was changes\\inserted");
            }
            catch (Exception ex)
            {
                log.Error("Writing into Data Base failed");
            }
            Console.ReadKey();
        }
        public IMessage CreateNewMessage(SqlDataReader data_reader)
        {
            //date AND guid
            DateTime date = new DateTime();
            Guid guid = new Guid();
            if (!data_reader.IsDBNull(0) & !data_reader.IsDBNull(2))
            {
                date = data_reader.GetDateTime(2); //2 is the coloumn index of the date. There are such               
                guid = data_reader.GetGuid(0);
            }
            int userId = data_reader.GetInt16(1); //TODO: check 
            //get Nickname and GroupId //    
            String sql_query1 = "SELECT [Group_Id], [Nickname] FROM [dbo].[Users] WHERE [Id] =" + userId;
            SqlCommand command1 = new SqlCommand(sql_query1, connection);
            SqlDataReader data_reader1 = command1.ExecuteReader();
            int groupId = data_reader1.GetInt16(0);
            String nickname = data_reader1.GetString(1);
            IMessage message = new Message(guid, nickname, groupId, date, data_reader.GetString(3));
            return message;
        }
        public List<IMessage> RetrieveMessages()
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
                        sql_query = "SELECT TOP 200 * FROM [dbo].[Messages] order by DateTime;"; // todo check ascending or descending
                        command = new SqlCommand(sql_query, connection);
                        isStart = false;
                    }
                    else// not the first retrieve
                    {
                        sql_query = "SELECT TOP 200 * FROM [dbo].[Messages] WHERE [SendTime]>@last_date order by DateTime;"; //TODO no more than 200
                        command = new SqlCommand(sql_query, connection);
                        command.Parameters.Add(last_date); 
                    }
                
                }
                else // there are filters
                {
                    String sql = "SELECT TOP 200 * FROM[dbo].[Messages] WHERE";
                    for(int i = 0; i < filters.Count; i++)
                    {
                         sql=filters.ElementAt(i).execute(sql)+" AND";
                    }
                    sql = sql.Substring(0, sql.Length-4); // delete the last " AND"
                    sql += " order by time DateTime"; //todo check the name in the table
                    command = new SqlCommand(sql_query, connection);
                }
                data_reader = command.ExecuteReader();
                while (data_reader.Read())
                {
                    IMessage message = CreateNewMessage(data_reader);
                    newMessages.Add(message);
                }
                if(filters.Count!=0) // save the last date only if there are no filters
                    last_date = newMessages.ElementAt(newMessages.Count - 1).Date;  //save the last date

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
