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
        private Timer timer;

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
            this.timer = new Timer(2000);
            timer.AutoReset = true;
            timer.Elapsed += (sender, e) => OnTimedEvent(sender, e, this);

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

        /*The method registrates a new user to the system. The method first checks if nickname input is legal.
         * If it is, the method creats new User instance and adds him to the users list. 
         * If nickname is already been used by the same groupId, the function throws an exception
         */
        public void Registration(string groupId, string nickname)
        {
            if (!CheckIfInputIsEmpty(groupId) || !CheckIfInputIsEmpty(nickname))
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
        //return false if input is empty 
        private bool CheckIfInputIsEmpty(string str)
        {
            if (str == "")
            {
                return false;
            }
            return true;
        }

        //Check if the user is registered. If he is, returns true. Otherwise, returns false.
        public bool Login(string groupId, string nickname)
        {
            if (!CheckIfInputIsEmpty(groupId) || !CheckIfInputIsEmpty(nickname))
            {
                throw new Exception(EMPTY_INPUT);
            }
            foreach (IUser user in this.users)
            {
                if (user.GroupID() == groupId && user.Nickname() == nickname)
                {
                    this.currentUser = user;
                    timer.Start(); // BEGIN TIMER 
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
            timer.Stop();//STOP TIMER
        }

        /*The function retrieves 10 last messages from server. The function adds only the new messages to the messages list
         * sorted by their timestamp. The head of the list points to the oldest message.
         */
        public void RetrieveNMessages(int number)
        {
            int new_messages = 0;
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
                    new_messages++; // add 1 to the count of new messages which added to the list
                }
            }
            this.count_of_new_message = new_messages; //update the count of messages which added to the list
            this.messages = this.messages.OrderBy(m => m.Date).ToList();
        }

        //The function returns list of the *** last messages to display without retrieving new messages from server
        public List<IMessage> DisplayNMessages(int number)
        {
            log.Info("The system is displaying the messeges");
            List<IMessage> display = new List<IMessage>();
            for (int i = this.messages.Count - 1; i >= 0 & number > 0; i = i - 1)
            {
                display.Add(this.messages[i]);
                number = number - 1;
            }
            //We need to reverse the list so the list will be sorted from the oldest message to the newest 
            display.Reverse();
            return display;
        }

        /*The function gets groupId and nickname and returns list of all messages that have been sent from this certain user 
        *sorted by their timestamp.
        */
        public List<IMessage> DisplayAllMessagesFromCertainUser(string groupId, string nickname)
        {
            log.Info("The system is displaying all messeges from " + groupId + " " + nickname);
            List<IMessage> display = new List<IMessage>();
            var users = from msg in this.messages
                        where msg.GroupID.Equals(groupId) && msg.UserName.Equals(nickname)
                        select msg;
            display = users.ToList();
            return display;
        }

        /*The function gets a string, checks if it is legal. If it is, sends the message content to the server and saves it in 
         * messages list. If it isn't, throws an exception
         */
        public void Send(string messageContent)
        {
            if ((Message.CheckValidity(messageContent)))
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
            else
            {
                log.Error("The user wrote an illegal message");
                throw new Exception(ILLEGAL_LENGTH_MESSAGE);
            }
        }
        public static void OnTimedEvent(object source, ElapsedEventArgs e, ChatRoom chat)
        {
            chat.RetrieveNMessages(10);
            List<IMessage> msg = chat.DisplayNMessages(chat.getCount_of_new_message()); // update the data
            chat.setCount_of_new_message(0);
            //PresentationLayer.Gui.Display2(msg);
        }

        public int getCount_of_new_message()
        {
            return this.count_of_new_message;
        }

        public void setCount_of_new_message(int num)
        {
            this.count_of_new_message = num;
        }

        public List<IMessage> SortTimestampAscending()
        {
            return this.messages;
        }
        public List<IMessage> SortByTimestampDescending()
        {
            List<IMessage> order_list = this.messages;
            order_list.Reverse();
            return (order_list);
        }
        public List<IMessage> SortByNicknameDescending()
        {
            List<IMessage> order_list = this.messages;
            order_list = order_list.OrderByDescending(o => o.UserName).ToList();
            return order_list;
        }
        public List<IMessage> SortByNicknameAscending()
        {
            List<IMessage> order_list = this.messages;
            order_list = order_list.OrderBy(o => o.UserName).ToList();
            return order_list;
        }
        public List<IMessage> SortByIdNicknameTimestamp()
        {
            List<IMessage> order_list = this.messages;
            order_list.OrderBy(x => x.Id).ThenBy(x => x.UserName).ThenBy(x => x.Date);

            return order_list;
        }
        public List<IMessage> SortByIdNicknameTimestampDescending()
        {
            List<IMessage> order_list = this.messages;
            order_list.OrderByDescending(x => x.Id).ThenByDescending(x => x.UserName).ThenByDescending(x => x.Date);
            return order_list;
        }

        public List<IMessage> FilterByGroupId(String groupId)
        {
            List<IMessage> filter_list = this.messages;
            filter_list.Where (x => x.GroupID.Equals(groupId)).ToList();
            return filter_list;
        }
        // filtering by a specific groupId and nickname
        public List<IMessage> FilterByUser(String groupId, String nickname)
        {
            List<IMessage> filter_list = this.messages;
            filter_list.Where(x => (x.GroupID.Equals(groupId))&&x.UserName.Equals(nickname)).ToList();
            return filter_list;
        }






    }
}



