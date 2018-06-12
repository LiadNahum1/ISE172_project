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
        private string sql_query;
        private string server_address;
        private string database_name;
        private string user_name;
        private string password;
        private string connetion_string;
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader data_reader;
        private static string SALT = "1337";

        public UserHandler()
        {
            this.sql_query = null;
            this.server_address = "ise172.ise.bgu.ac.il,1433\\DB_LAB";
            this.database_name = "MS3";
            this.user_name = "publicUser";
            this.password = "isANerd";
            this.connetion_string = $"Data Source={server_address};Initial Catalog={database_name };User ID={user_name};Password={password}";
            this.connection = new SqlConnection(connetion_string);
        }
        public void InsertNewUser(IUser user)
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
  
                group_Id_param.Value = user.GroupID();
                nickname_param.Value = user.Nickname();
                password_param.Value = user.Password(); //To check!!!
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
        public List<IUser> RetrieveUsers()
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

        public IUser RetrieveUser(int groupId, string nickname, string password)
        {
            IUser user = null;
            try
            {
                connection.Open();
                log.Info("connected to: " + server_address);
                sql_query = "SELECT * FROM [dbo].[Users] WHERE [Group_Id]="+ groupId + " AND [Nickname]='"+nickname+"' AND [Password]='" + hashing.GetHashString(password + SALT)+"';";
                command = new SqlCommand(sql_query, connection);

                data_reader = command.ExecuteReader();
                while (data_reader.Read())
                {
                    user = CreateUserInstance(data_reader);
                }
                data_reader.Close();
                command.Dispose();
                connection.Close();
                return user;
            }
            catch (Exception ex)
            {
                log.Error("Reading from Data Base failed");
                log.Error(ex.ToString());
                return null;
            }
        }

        public IUser CreateUserInstance(SqlDataReader data_reader)
        {
            return new User((int)data_reader.GetValue(1), data_reader.GetString(2), data_reader.GetString(3));
        }
        /*registration*/
        public bool IsValidNickname(string groupId, string nickname)
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
        public bool IsValidPassword(string groupId, string nickname, string password)
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
