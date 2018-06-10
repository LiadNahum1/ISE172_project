﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRoomProject.DataAccess;

using System.Timers;
namespace ChatRoomProject.LogicLayer
{
    /// <summary>
    /// this class will get the data from the presentation layer make it ordered,
    /// validate it and sending it to the persistent layer for saving and restoring
    /// </summary>
    public class ChatRoom : IChatRoom
    {
        //fields
        private MessageHandler message_handler;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("ChatRoom.cs");
        private List<IUser> users;
        private List<IMessage> messages;
        private IUser currentUser;

        //useful error messages
        const string INVALID_NICKNAME = "Invalid nickname. \nYou insert a nickname that is already used in your group";
        const string EMPTY_INPUT = "Please insert data";
        const string INVALID_LOGIN = "Must register first";
        const string INVALID_GROUPID = "Group Id must be an integer";
        const string INVALID_PASSWORD = "Letters and numbers only, 4 <= length <= 16";
        const string WRONG_PASSWORD = "Wrong password";
        const string ILLEGAL_LENGTH_MESSAGE = "Illegal length message. Must be under 100 characters";

        //constructor
        public ChatRoom()
        {
            this.message_handler = new MessageHandler();
            this.users = new List<IUser>(); //users list
            this.messages = new List<IMessage>(); //messages list
            this.currentUser = null; //user that is connected now
        }

        /*The method restores the users and the messages that had been saved in the system files from previous use */
        public void Start()
        {
            log.Info("The system starts now");
            messages = message_handler.RetrieveMessages();
        }
        //This is done every two seconds by reading from the timer.
        //This returns an updated list of messages that are organized according to
        //the data that the operation receives: sort type, sort order and filter 
        //if the user is interested.
        public List<String> MessageManager(bool ascending, string filter,string sort, string groupId,string nickName)
        {
            List<IMessage> updateList = this.messages;
            if (filter != null) // if the user chose to filter the messages 
            {
                if (filter.Equals("filterByUser"))
                {
                    message_handler.AddNicknameFilter(nickName);
                    message_handler.AddGroupFilter(Int32.Parse(groupId));
                }
                else // FilterByGroupID
                    message_handler.AddGroupFilter(Int32.Parse(groupId));
                updateList = message_handler.RetrieveMessages(); // filter list
            }
            else // no filter so update the list of messages
            {
                message_handler.ClearFilters(); // delete all the filters
                this.messages.AddRange(message_handler.RetrieveMessages()); // update the new messages from the data base
                updateList = this.messages;
            }
            
            if (sort.Equals("SortByNickName"))
                updateList =SortByNickname(updateList,ascending);
            if (sort.Equals("SortByIdNicknameTimestamp"))
                updateList= SortByIdNicknameTimestamp(updateList, ascending);
            if (sort.Equals("SortByTimestamp"))
                updateList= SortTimestamp(updateList,ascending);
           
            return ConvertToString(updateList); //convert the update list of Imessages to string
        }

        /*this function convert the Imessage messages to string in order to send them 
        *rightfully to the presentation*/
        public static List<String> ConvertToString(List<IMessage> updateList)
        {
            List<String> newList =new List<String>();
            for(int i=0;i<updateList.Count();i++)
            {
                newList.Add(updateList[i].ToString());
            }
            return newList;
        }

        /*The method registrates a new user to the system. The method first checks if nickname and groupId input is legal.
         * If it is, the method creats new User instance and adds him to the users list. 
         * If nickname is already been used by the same groupId, the function throws an exception
         */
       public void Registration(string groupId, string nickname, string password)
        {
            if (CheckIfInputIsEmpty(groupId) || CheckIfInputIsEmpty(nickname)|| CheckIfInputIsEmpty(password))
            { 
                throw new Exception(EMPTY_INPUT);
            }
            if (!IsValidGroupID(groupId))
            {
                throw new Exception(INVALID_GROUPID);
            }
            if (!IsValidNickname(groupId, nickname))
            {
                throw new Exception(INVALID_NICKNAME);
            }
          
            if (!IsValidPassword(password))
            {
                throw new Exception(INVALID_PASSWORD);
            }
            else
            {
                this.users.Add(new User(groupId, nickname ,password,message_handler));
            }
        }

        public List<IUser> getUsers()
        {
            return this.users;
        }
        public IUser getCurrentUser()
        {
            return this.currentUser;
        }
        /*Check if nickname is already been used in the same group. If it is, returns false because the nickname is invalid. 
         * Nickname can only be used by one member of the same group
         */
        private bool IsValidNickname(string groupId, string nickname)
        {
            return UserHandler.IsValidNickname(groupId, nickname);
          /*  foreach (IUser user in this.users)
            {
                if (user.GroupID().Equals(groupId) && user.Nickname().Equals(nickname))
                    return false;
            }
            return true;*/
        }

        /*Check if password is legal. Need to be consisted only from letters and numbers and
         *  4 <= length <= 16"*/
        private bool IsValidPassword(string password)
        {
            if (password.Length < 4 | password.Length > 16)
                return false;
            password = password.ToLower();
           for(int i=0; i<password.Length; i = i + 1)
            {
                if (!(password[i]>= '0' & password[i]<='9') |(password[i]>='a' & password[i]<='z'))
                    return false;
            }
            return true; 
        }


        /*Check if groupID is an integer*/
        private bool IsValidGroupID(string groupId)
        {
            try
            {
                int group = int.Parse(groupId);
                return true;
            }
            catch(Exception e)
            {
                return false; 
            }
        }
        //return true if input is empty 
        private bool CheckIfInputIsEmpty(string str)
        {
            if (str == null | str=="")
            {
                return true;
            }
            return false;
        }

        //Check if the user is registered. If he is, returns true. Otherwise, returns false.
        public bool Login(string groupId, string nickname, string password)
        {
            if (CheckIfInputIsEmpty(groupId) || CheckIfInputIsEmpty(nickname))
            {
                throw new Exception(EMPTY_INPUT);
            }
            else if(!UserHandler.IsValidNickname(groupId, nickname))
            {
                throw new Exception(INVALID_LOGIN); //user wasnt found
            }
            else if (!UserHandler.IsValidPassword(groupId, nickname, password))
            {
                throw new Exception(WRONG_PASSWORD);
            }
            else
                return true; 
            /* foreach (IUser user in this.users)
             {
                 if (user.GroupID().Equals(groupId) && user.Nickname().Equals(nickname))
                 {
                     this.currentUser = user;
                     return true;
                 }
             }*/
        }

        //Logout the current user
        public void Logout()
        {
            log.Info("User: " + this.currentUser + " logout");
            this.currentUser = null;
        }

        /*The function retrieves the new messages from server.(up to 200) The function adds only the new messages to the messages list
         * sorted by their timestamp. The head of the list points to the oldest message.
         */
        public void RetrieveMessages()
        {
            this.messages.AddRange(message_handler.RetrieveMessages());
            this.messages = this.messages.OrderBy(m => m.Date).ToList();
            LegalSizeOfMessagesList();
        }
        /*Keep the size of the message list no more than 200*/
        public void LegalSizeOfMessagesList()
        {
            if(this.messages.Count > 200)
            {
                int gap = this.messages.Count - 200;
                for(int i=0; i <gap; i = i + 1)
                {
                    this.messages.RemoveAt(0);
                }
            }
        }
        //Send messages. If empty or more than 150 characters throw an exception.
        //Otherwise, send it and save into messages list
        public void Send(string messageContent)
        {
           
            if (!Message.CheckValidity(messageContent))
            {
                if (messageContent == "")//empty message
                {
                    log.Error("The user wrote an empty message");
                    throw new Exception(EMPTY_INPUT);
                }
                else
                {
                    log.Error("The user wrote an illegal message");
                    throw new Exception(ILLEGAL_LENGTH_MESSAGE);
                }
            }
            else
            {
                this.currentUser.Send(messageContent);
            }
        }

        //input: list of Imessages and true- if ascending, false-descending
        //output:list of Imessages sorted by timestamp according the ascending boolean variable
        public static List<IMessage> SortTimestamp(List<IMessage> updatelist,Boolean ascending)
        {
            if (ascending)
            {
                return updatelist.OrderBy(o => o.Date).ToList();
            }
            else//descending
            {
                return updatelist.OrderByDescending(o => o.Date).ToList();
            }
        }
        //input: list of Imessages and true- if ascending, false-descending
        //output:list of Imessages sorted by NickName according the ascending boolean variable
        public static List<IMessage> SortByNickname(List<IMessage>updatelist, Boolean ascending)
        {
            if (ascending)
            {
                updatelist = updatelist.OrderBy(o => o.UserName).ToList();
                return updatelist;
            }
            else //descending
            {
                updatelist =updatelist.OrderByDescending(o => o.UserName).ToList();
                return updatelist;
            }
        }
        //input: list of Imessages and true- if ascending, false-descending
        //output:list of Imessages sorted by id->nickname->TimeStamp according the ascending boolean variable
        public static List<IMessage> SortByIdNicknameTimestamp(List<IMessage> updatelist, Boolean ascending)
        {
            if (ascending)
            {
                return updatelist.OrderBy(x => x.GroupID).ThenBy(x => x.UserName).ThenBy(x => x.Date).ToList();
            }
            else { // descending
                return updatelist.OrderByDescending(x => x.GroupID).ThenByDescending(x => x.UserName).ThenByDescending(x => x.Date).ToList();
                }
        }
        //input: list of Imessages and groupID
        //output:list of Imessages which include only the messages of the specific groupID
        public static List<IMessage> FilterByGroupId(List<IMessage> list, String groupId)
        {
            list=list.Where (x => x.GroupID.Equals(groupId)).ToList();

            return list;
        }
        //input: list of Imessages, groupID and nickname
        //output:list of Imessages which include only the messages of the specific user
        public static List<IMessage> FilterByUser(List<IMessage> list,String groupId, String nickname)
        {
            list=list.Where(x => (x.GroupID.Equals(groupId))&&x.UserName.Equals(nickname)).ToList();
            return list;
        }

        
       

    }
}


