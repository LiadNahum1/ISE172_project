using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace ChatRoomProject.DataAccess
{
    class MessageHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("MessageHandler.cs");
        private bool isStart;
        private string connetion_string = null;
        private string sql_query = null;
        private string server_address = "ise172.ise.bgu.ac.il,1433\\DB_LAB";
        private string database_name = "MS3";
        private string user_name = "publicUser";
        private string password = "isANerd";
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader data_reader;
        private IList<IQueryAction> filters;
        public MessageHandler()
        {
            this.connetion_string = $"Data Source={server_address};Initial Catalog={database_name };User ID={user_name};Password={password}";
            this.connection = new SqlConnection(connetion_string);
            this.filters = new List<IQueryAction>();
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
            //get Nickname and GroupId
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
                if (isStart)
                {
                    sql_query = "SELECT * FROM [dbo].[Messages];";
                    command = new SqlCommand(sql_query, connection);
                    isStart = false;
                }
                else {
                    sql_query = "SELECT * FROM [dbo].[Messages] WHERE [SendTime];";
                    command = new SqlCommand(sql_query, connection);
                }
                this.data_reader = command.ExecuteReader();
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
                throw new Exception();
            }
        }
        /*
            try
            {
                connection.Open();
                command = new SqlCommand(null, connection);

                // Create and prepare an SQL statement.
                // Use should never use something like: query = "insert into table values(" + value + ");" 
                // Especially when selecting. More about it on the lab about security.
                command.CommandText =
                    "INSERT INTO Customer ([FirstName],[LastName],[City],[Country],[Phone]) " +
                    "VALUES (@first_name, @last_name,@city,@country,@phone)";
                SqlParameter first_name_param = new SqlParameter(@"first_name", SqlDbType.Text, 20);
                SqlParameter last_name_param = new SqlParameter(@"last_name", SqlDbType.Text, 20);
                SqlParameter city_param = new SqlParameter(@"city", SqlDbType.Text, 20);
                SqlParameter country_param = new SqlParameter(@"country", SqlDbType.Text, 20);
                SqlParameter phone_param = new SqlParameter(@"phone", SqlDbType.Text, 20);

                first_name_param.Value = "Edsger";
                last_name_param.Value = "Dijkstra";
                city_param.Value = "Amsterdam";
                country_param.Value = "Netherlands";
                phone_param.Value = "054-8965478";
                command.Parameters.Add(first_name_param);
                command.Parameters.Add(last_name_param);
                command.Parameters.Add(city_param);
                command.Parameters.Add(country_param);
                command.Parameters.Add(phone_param);

                // Call Prepare after setting the Commandtext and Parameters.
                command.Prepare();
                int num_rows_changed = command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                Console.WriteLine($"ExecuteNonQuery in SqlCommand executed!! {num_rows_changed.ToString()} row was changes\\inserted");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());

            }
            Console.ReadKey();
        }


    }*/
    }
}
