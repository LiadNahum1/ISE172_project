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
    class UserHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("UserHandler.cs");
       // private bool isStart;
        private static string sql_query = null;
        private static string server_address = "ise172.ise.bgu.ac.il,1433\\DB_LAB";
        private static string database_name = "MS3";
        private static string user_name = "publicUser";
        private static string password = "isANerd";
        private static string connetion_string = $"Data Source={server_address};Initial Catalog={database_name };User ID={user_name};Password={password}";
        private static SqlConnection connection = new SqlConnection(connetion_string);
        private static SqlCommand command;
        private static SqlDataReader data_reader;
        private static string SALT = "1337";

        public static void InsertNewUser(string nickname, string groupId, string password)
        {
            try
            {
                connection.Open();
                command = new SqlCommand(null, connection);
                // Create and prepare an SQL statement.
                command.CommandText =
                    "INSERT INTO [Users] ([Group_Id],[Nickname],[Password]) " +
                    "VALUES (@group_Id,@nickname,@password)";
                SqlParameter group_Id_param = new SqlParameter(@"group_Id", SqlDbType.Int, 32);
                SqlParameter nickname_param = new SqlParameter(@"nickname", SqlDbType.Char, 8);
                SqlParameter password_param = new SqlParameter(@"password", SqlDbType.Char, 64);
  
                group_Id_param.Value = int.Parse(groupId);
                nickname_param.Value = nickname;
                password_param.Value = hashing.GetHashString(password + SALT); //salt to password
                log.Info("password" + hashing.GetHashString(password + SALT));
                command.Parameters.Add(group_Id_param);
                command.Parameters.Add(nickname_param);
                command.Parameters.Add(password_param);

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
            //Console.ReadKey();
        }
        public static List<IUser> RetrieveUsers()
        {
            List<IUser> users = new List<IUser>();
            try
            {
                connection.Open();
                log.Info("connected to: " + server_address);
                sql_query = "SELECT * FROM [dbo].[Users];";
                command = new SqlCommand(sql_query, connection);
              
                data_reader = command.ExecuteReader();
                while (data_reader.Read())
                {
                    IUser user = CreateUserInstance(data_reader);
                    users.Add(user);
                }
                data_reader.Close();
                command.Dispose();
                connection.Close();
                return users;
            }
            catch (Exception ex)
            {
                log.Error("Reading from Data Base failed");
                log.Error(ex.ToString());
                return null;
            }
        }

        public static IUser CreateUserInstance(SqlDataReader data_reader)
        {
            return new User(data_reader.GetString(1), data_reader.GetString(2), data_reader.GetString(3));
        }
        /*registration*/
        public static bool IsValidNickname(string groupId, string nickname)
        {
            try
            {
                connection.Open();
                log.Info("connected to: " + server_address);
                sql_query = "SELECT [Group_Id], [Nickname] FROM [dbo].[Users] WHERE [Group_Id]=" + int.Parse(groupId) + " AND [Nickname]='" + nickname+ "';";
                command = new SqlCommand(sql_query, connection);
                data_reader = command.ExecuteReader();
                bool result = false;
                if (!data_reader.Read())
                {
                    result = true;  
                }
                data_reader.Close();
                command.Dispose();
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                log.Error("Reading from Data Base failed");
                log.Error(ex.ToString());
                return true;
            }
        }
        public static bool IsValidPassword(string groupId, string nickname, string password)
        {
            try
            {
                connection.Open();
                String hashedPassword = hashing.GetHashString(password + SALT);
                log.Info("connected to: " + server_address);
                sql_query = "SELECT * FROM [dbo].[Users] WHERE [Group_Id]=" + groupId + " AND [Nickname]='" + nickname+"'"
                    + "AND [Password]='" + hashedPassword + "';";
                command = new SqlCommand(sql_query, connection);
                data_reader = command.ExecuteReader();
                bool result = true; 
                if (!data_reader.Read())
                {
                    result = false;
                }
                data_reader.Close();
                command.Dispose();
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                log.Error("Reading from Data Base failed");
                return true;
            }
        }
    }
}
