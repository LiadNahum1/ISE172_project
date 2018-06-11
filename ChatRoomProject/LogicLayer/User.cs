﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRoomProject.DataAccess;

namespace ChatRoomProject.LogicLayer
{
    public class User : IUser
    {
        //fields
        private string groupID;
        private string nickname;
        private string password; 
        private MessageHandler message_handler;

        /*constructor
         * gets groupId, nickname and boolean value isRestored which indicates whether the current user details are restored from dataBase or
         * the user is a new one. If the user is restored there is no need to save him in persistent layer because he is already saved.
         * If he isn't, we will save his details in Data Base. 
         */
        public User(string groupID, string nickname,string password, MessageHandler message_handler)
        {
            this.message_handler = message_handler;;
            this.groupID = groupID;
            this.nickname = nickname;
            this.password = password;
            //Check if he is already in?? or maybe dont need to
            SaveIntoDataBase();
        }
        /*restore from DataBase*/
        public User(string groupID, string nickname, string password)
        {
            this.groupID = groupID;
            this.nickname = nickname;
            this.password = password;
        }
       
        public string Password()
        {
            return this.password;
        }
        public string GroupID()
        {
            return this.groupID;
        }
        public string Nickname()
        {
            return this.nickname;
        }

        //Save user's details in the data base
        public void SaveIntoDataBase()
        {
            UserHandler.InsertNewUser(Nickname(), GroupID(), Password());
        }

        

        //The function gets a string of a message content and sends it to the server. Recieves an IMessage from server and returns it. 
        public void Send(string messageContent)
        {
            IMessage msg = new Message(Nickname(), int.Parse(GroupID()), messageContent);
            message_handler.InsertNewMessage(msg);          
        }

        public override string ToString()
        {
            return GroupID() + " "+ Nickname();
        }

      
    }
}
