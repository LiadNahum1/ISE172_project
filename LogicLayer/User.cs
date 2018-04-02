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
        private string groupID;
        private string nickname;
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
        public void Save()
        {
            log.Info("the system is saving the user");
            UserHandler.SaveToFile(Nickname(), GroupID());
        }
        //returns message that recieved from server. The function build a Message instance that save itself in the constructor
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
