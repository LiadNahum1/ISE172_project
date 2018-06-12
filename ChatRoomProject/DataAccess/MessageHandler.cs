﻿using System;
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
        public  void InsertNewMessage(IMessage msg)
        {
            try
            {
                connection.Open();
                command = new SqlCommand(null, connection);
                // Create and prepare an SQL statement.
                command.CommandText =
                    "INSERT INTO Messages ([Guid],[SendTime],[Body]) " +
                    "VALUES (@guid, @time,@body)";
                SqlParameter guid_param = new SqlParameter(@"guid", SqlDbType.Text, 20);
                SqlParameter user_Id_param = new SqlParameter(@"user_Id", SqlDbType.Text, 20);
                SqlParameter time_param = new SqlParameter(@"time", SqlDbType.Text, 20);
                SqlParameter body_param = new SqlParameter(@"body", SqlDbType.Text, 20);
                guid_param.Value = msg.Id;
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
        }

        public void DeleteByGuid(string messageGuid)
        {
            connection.Open();
            log.Info("connected to: " + server_address);
            SqlCommand Command = new SqlCommand(
              "DELETE FROM [dbo].[Messages] WHERE Guid='@word'" +connection);
            Command.Parameters.AddWithValue("@word", messageGuid);
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

                        sql_query = "SELECT TOP " + MAX_MESSAGES + " [Guid], [SendTime], [Body], [Group_id], [Nickname] From [dbo].[Messages] JOIN [dbo].[Users]" +
                            "on [Messages].[User_Id]=[Users].[Id]  order by [SendTime];";
                        // todo check ascending or descending
                        command = new SqlCommand(sql_query, connection);
                        isStart = false;
                    }
                    else// not the first retrieve
                    {
                        sql_query = "SELECT TOP " + MAX_MESSAGES + " [Guid], [SendTime], [Body], [Group_id], [Nickname] From [dbo].[Messages] JOIN [dbo].[Users]" +
                            "on [Messages].[User_Id]=[Users].[Id]  WHERE [SendTime] >= '" + lastDate + "' order by [SendTime];"; //TODO no more than 200
                       // SqlParameter last_date = new SqlParameter(@"last_date", SqlDbType.DateTime, 20);
                       // last_date.Value = lastDate;
                       // command.Parameters.Add(last_date); // todo check
                        command = new SqlCommand(sql_query, connection);
                    }

                }
                else // there are filters
                {
                    if (isStart)
                    {
                        String sql = "SELECT TOP " + MAX_MESSAGES + " * FROM[dbo].[Messages] WHERE";
                        for (int i = 0; i < filters.Count; i++)
                        {
                            sql = filters.ElementAt(i).execute(sql) + " AND";
                        }
                        sql = sql.Substring(0, sql.Length - 4); // delete the last " AND"
                        sql += " order by [SendTime] "; //todo check the name in the table
                        command = new SqlCommand(sql_query, connection);
                    }
                    else // return only the new filter messages
                    {
                        String sql = "SELECT TOP " + MAX_MESSAGES + " * FROM[dbo].[Messages] WHERE";
                        for (int i = 0; i < filters.Count; i++)
                        {
                            sql = filters.ElementAt(i).execute(sql) + " AND";
                        }
                        sql += " [SendTime]>'" + lastDate + "' order by [SendTime];";
                        //SqlParameter last_date = new SqlParameter(@"last_date", SqlDbType.DateTime, 20);
                        //last_date.Value = lastDate;
                      //  command.Parameters.AddWithValue()
                        //command.Parameters.Add(last_date); // todo check
                        command = new SqlCommand(sql_query, connection);
                    }
                }
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
                    string msgContent = data_reader.GetString(2);
                    int groupId = (int)data_reader.GetValue(3);
                    string nickname = data_reader.GetString(4);
                    IMessage message = new Message(guid, nickname, groupId, date, msgContent);
                    newMessages.Add(message);
                }
                if (newMessages.Count > 1) { 
                lastDate = newMessages.ElementAt(0).Date;  //save the last date
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
