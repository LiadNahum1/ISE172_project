using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRoomProject.PersistentLayer;
using ChatRoomProject.CommunicationLayer;

namespace ChatRoomProject.LogicLayer
{
    public class User : IUser
    {
        //fields
        private string groupID;
        private string nickname;

        /*constructor
         * gets groupId, nickname and boolean value isRestored which indicates whether the current user details are restored from dataBase or
         * the user is a new one. If the user is restored there is no need to save him in persistent layer because he is already saved.
         * If he isn't, we will save his details in files. 
         */
        public User(string groupID, string nickname, bool isRestored)
        {
            this.groupID = groupID;
            this.nickname = nickname;
            if (!isRestored)
                Save();
        }

        public string GroupID()
        {
            return this.groupID;
        }
        public string Nickname()
        {
            return this.nickname;
        }

        //Save user's details in the system files 
        public void Save()
        {
            UserHandler.SaveToFile(Nickname(), GroupID());
        }

        //The function gets a string of a message content and sends it to the server. Recieves an IMessage from server and returns it. 
        public IMessage Send(string messageContent)
        {
            IMessage message = Communication.Instance.Send(ChatRoom.URL, GroupID(), Nickname(), messageContent);
            return message;
        }

        public override string ToString()
        {
            return GroupID() + Nickname();
        }
    }
}
