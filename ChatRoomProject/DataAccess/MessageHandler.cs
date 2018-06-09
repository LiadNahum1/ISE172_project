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
        private static IList<IQueryAction> filters = new List<IQueryAction>();
       
        public static void ClearFilters() { filters.Clear(); }
        public static void AddGroupFilter(int groupId)
        {
            filters.Add(new GroupFilter(groupId));
        }
        public static void AddNicknameFilter(string nickName)
        {

            filters.Add(new NicknameFilter(nickName));
        }
        public static void InsertNewMessage(Guid userId, IMessage msg)
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
        public static IMessage CreateNewMessage(SqlDataReader data_reader)
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
            //get Nickname and GroupId
            String sql_query1 = "SELECT [Group_Id], [Nickname] FROM [dbo].[Users] WHERE [Id] =" + userId;
            SqlCommand command1 = new SqlCommand(sql_query1, connection);
            SqlDataReader data_reader1 = command1.ExecuteReader();
            int groupId = data_reader1.GetInt16(0);
            String nickname = data_reader1.GetString(1);
            IMessage message = new Message(guid, nickname, groupId, date, data_reader.GetString(3));
            return message;
        }
        public static List<IMessage> RetrieveMessages()
        {
            List<IMessage> newMessages = new List<IMessage>();
            try
            {
                connection.Open();
                log.Info("connected to: " + server_address);
                if (isStart)
                {
                    sql_query = "SELECT * FROM [dbo].[Messages];";
                    command = new SqlCommand(sql_query, connection);
                    isStart = false;
                }
                else
                {
                    sql_query = "SELECT * FROM [dbo].[Messages] WHERE [SendTime];";
                    command = new SqlCommand(sql_query, connection);
                }
                data_reader = command.ExecuteReader();
                while (data_reader.Read())
                {
                    IMessage message = CreateNewMessage(data_reader);
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
                return null;
            }
        }
    }
}
