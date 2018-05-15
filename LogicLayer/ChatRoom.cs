using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRoomProject.PersistentLayer;
using ChatRoomProject.CommunicationLayer;
using System.Timers;
namespace ChatRoomProject.LogicLayer
{
    public class ChatRoom : IChatRoom
    {
        //fields
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("ChatRoom.cs");
        private List<IUser> users;
        private List<IMessage> messages;
        private IUser currentUser;
        public const string URL = " http://ise172.ise.bgu.ac.il:80";
        private int count_of_new_message;
        //useful error messages
        const string INVALID_NICKNAME = "Invalid nickname. \nYou insert a nickname that is already used in your group";
        const string EMPTY_INPUT = "Please insert data";
        const string INVALID_LOGIN = "Must register first";
        const string ILLEGAL_LENGTH_MESSAGE = "Illegal length message. Must be under 150 characters";

        //constructor
        public ChatRoom()
        {
            this.users = new List<IUser>(); //users list
            this.messages = new List<IMessage>(); //messages list
            this.currentUser = null; //user that is connected now
            this.count_of_new_message = 0;
        }

        /*The method restores the users and the messages that had been saved in the system files from previous use */
        public void Start()
        {
            log.Info("The system starts now");
            log.Info("The system is restorig the users ");
            //restoring the users list
            List<String> usersData = UserHandler.RestoreUsers();
            foreach (string data in usersData)
            {
                string[] details = data.Split(',');
                this.users.Add(new User(details[0], details[1], true));
            }

            log.Info("The system is restorig the messeges");
            //restoring the messages list
            List<String> messagesData = MessageHandler.RestoreMessages();
            foreach (string data in messagesData)
            {
                string[] details = data.Split(',');
                this.messages.Add(new Message(details[0], details[1], details[2], details[3], details[4], true));
            }
        }
        //need to update fields sort and filter with setSort every time they change the sort
        //return update list sorted by or filter by
        
        public List<String> MessageManager(bool ascending, string filter,string sort, string groupId,string nickName)
        {
            RetrieveNMessages(10);
            List<IMessage> updateList =this.messages;
          
                if (sort.Equals("SortByNickName"))
                    updateList =SortByNickname(updateList,ascending);
                if (sort.Equals("SortByIdNicknameTimestamp"))
                    updateList= SortByIdNicknameTimestamp(updateList, ascending);
                if (sort.Equals("SortByTimestamp"))
                    updateList= SortTimestamp(updateList,ascending);
            if(filter!=null)
            {
                if (filter.Equals("filterByUser"))
                    updateList= FilterByUser(updateList, groupId, nickName); 
                else
                    updateList= FilterByGroupId(updateList, groupId);
            }
            return ConvertToString(updateList);
        }

        private List<String> ConvertToString(List<IMessage> updateList)
        {
            List<String> newList =new List<String>();
            for(int i=0;i<updateList.Count();i++)
            {
                newList.Add(updateList[i].ToString());
            }

            return newList;
        }

        /*The method registrates a new user to the system. The method first checks if nickname input is legal.
         * If it is, the method creats new User instance and adds him to the users list. 
         * If nickname is already been used by the same groupId, the function throws an exception
         */
        public void Registration(string groupId, string nickname)
        {
            if (CheckIfInputIsEmpty(groupId) || CheckIfInputIsEmpty(nickname))
            {
                throw new Exception(EMPTY_INPUT);
            }
            if (!IsValidNickname(groupId, nickname))
            {
                log.Error("Registration failed. The user inserted an invalid nickname");
                throw new Exception(INVALID_NICKNAME);
            }
            else
            {
                this.users.Add(new User(groupId, nickname, false));
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
            foreach (IUser user in this.users)
            {
                if (user.GroupID().Equals(groupId) && user.Nickname().Equals(nickname))
                    return false;
            }
            return true;
        }
        //return true if input is empty 
        private bool CheckIfInputIsEmpty(string str)
        {
            if (str == null)
            {
                return true;
            }
            return false;
        }

        //Check if the user is registered. If he is, returns true. Otherwise, returns false.
        public bool Login(string groupId, string nickname)
        {
            if (CheckIfInputIsEmpty(groupId) || CheckIfInputIsEmpty(nickname))
            {
                throw new Exception(EMPTY_INPUT);
            }
            foreach (IUser user in this.users)
            {
                if (user.GroupID() == groupId && user.Nickname() == nickname)
                {
                    this.currentUser = user;
                    return true;
                }
            }
            log.Error("Login failed. The user is not registered");
            throw new Exception(INVALID_LOGIN);
        }

        //Logout the current user
        public void Logout()
        {
            log.Info("User: " + this.currentUser + " logout");
            this.currentUser = null;
        }

        /*The function retrieves 10 last messages from server. The function adds only the new messages to the messages list
         * sorted by their timestamp. The head of the list points to the oldest message.
         */
        public void RetrieveNMessages(int number)
        {
            List<IMessage> retrievedMessages = Communication.Instance.GetTenMessages(URL);
            foreach (IMessage msg in retrievedMessages)
            {
                bool isAlreadySaved = false;
                foreach (IMessage savedMsg in this.messages)
                {
                    if (savedMsg.Id.Equals(msg.Id))
                    {
                        isAlreadySaved = true;
                        break;
                    }
                }
                if (!isAlreadySaved)
                {
                    Message newMessage = new Message(msg, false);
                    this.messages.Add(newMessage);
                }
            }
            this.messages = this.messages.OrderBy(m => m.Date).ToList();
        }
        public void Send(string messageContent)
        {
            if(messageContent == "")
            {
                log.Error("The user wrote an illegal message");
                throw new Exception(EMPTY_INPUT);
            }
            if (!Message.CheckValidity(messageContent))
            {

                log.Error("The user wrote an illegal message");
                throw new Exception(ILLEGAL_LENGTH_MESSAGE);
            }
            else
            {
                IMessage msg = this.currentUser.Send(messageContent);
                if (msg == null)
                {
                    throw new Exception("The user couldn't send the message");
                }
                else
                {
                    Message message = new Message(msg, false);
                    this.messages.Add(message);
                }
            }
        }
      

        private List<IMessage> SortTimestamp(List<IMessage> updatelist,Boolean ascending)
        {
            if (ascending)
            {
                return updatelist;
            }
            else
            {
                updatelist.Reverse();
                return (updatelist);
            }
        }

        private List<IMessage> SortByNickname(List<IMessage>updatelist, Boolean ascending)
        {
            if (ascending)
            {
                updatelist = updatelist.OrderBy(o => o.UserName).ToList();
                return updatelist;
            }
            else
            {
                updatelist =updatelist.OrderByDescending(o => o.UserName).ToList();
                return updatelist;
            }
        }
        private List<IMessage> SortByIdNicknameTimestamp(List<IMessage> updatelist, Boolean ascending)
        {
            if (ascending)
            {
                return updatelist.OrderBy(x => x.GroupID).ThenBy(x => x.UserName).ThenBy(x => x.Date).ToList();
              //  return updatelist;
            }
            else {
                updatelist.OrderByDescending(x => x.GroupID).ThenByDescending(x => x.UserName).ThenByDescending(x => x.Date);
                return updatelist;
            }
        }
        private List<IMessage> FilterByGroupId(List<IMessage> list, String groupId)
        {
            list=list.Where (x => x.GroupID.Equals(groupId)).ToList();
            return list;
        }
        // filtering by a specific groupId and nickname
        private List<IMessage> FilterByUser(List<IMessage> list,String groupId, String nickname)
        {
            list=list.Where(x => (x.GroupID.Equals(groupId))&&x.UserName.Equals(nickname)).ToList();
            return list;
        }


    }
}



