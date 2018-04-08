using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRoomProject.CommunicationLayer;
using ChatRoomProject.PersistentLayer;


namespace ChatRoomProject.LogicLayer
{
    class Message : IMessage
    {
        //fields
        private Guid id;
        private string nickname;
        private string groupId;
        private DateTime date;
        private string messageContent;
        const int MAX_LENGTH = 150;

        /* constructor 
          * gets strings: id, nickname, groupId, date and message content and a boolean value isRestored which indicates whether the current
          * message details are restored from dataBase or the message is a new one. 
          * If the message is restored there is no need to save it in persistent layer.
          * If it isn't, we will save its details in files. 
          */
        public Message(string id, string nickname, string groupId, string date, string messageContent, bool isRestored)
        {
            this.id = new Guid(id);
            this.nickname = nickname;
            this.groupId = groupId;
            //this.date = DateTime.Parse(date);
            string[] time = date.Split(' ');
            string[] day = time[0].Split('/');
            string[] hour = time[1].Split(':');
            this.date = new DateTime(Int32.Parse(day[2]), Int32.Parse(day[1]), Int32.Parse(day[0]), Int32.Parse(hour[0]), Int32.Parse(hour[1]), Int32.Parse(hour[2]));
            this.messageContent = messageContent;
            if (!isRestored)
                Save();
        }

        /*constructor
         * gets IMessage and a boolean value. Build the Message instance from the fields of the IMessage.
         * If the message is not restored, saves it in files.
         */
        public Message(IMessage message, bool isRestored)
        {
            this.id = message.Id;
            this.nickname = message.UserName;
            this.groupId = message.GroupID;
            this.date = message.Date;
            this.messageContent = message.MessageContent;
            if (!isRestored)
                Save();
        }
       
        //implements IMessage
        Guid IMessage.Id { get { return this.id; } }

        string IMessage.UserName { get { return this.nickname; } }

        DateTime IMessage.Date { get { return this.date; } }

        string IMessage.MessageContent { get { return this.messageContent; } }

        public string GroupID { get { return this.groupId; } }

        //Save message's details in the system files 
        public void Save()
        {
            MessageHandler.SaveToFile(this.id, this.nickname, this.groupId, this.date, this.messageContent);
        }

        //Static function checks the validity of a message content. If the string is above 150 characters, returns false. 
        public static bool CheckValidity(string content)
        {
            if (content.Length > MAX_LENGTH)
                return false;
            return true;
        }

        public override string ToString()
        {
            return this.messageContent + " send by " + this.groupId + ":" + this.nickname + " " + this.date;
        }
    }
}
