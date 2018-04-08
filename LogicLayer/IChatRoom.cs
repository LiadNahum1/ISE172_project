using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRoomProject.CommunicationLayer;

namespace ChatRoomProject.LogicLayer
{
    public interface IChatRoom
    {
        void Start();
        bool Registration(string groupId, string nickname);
        bool IsValidRegistration(string groupId, string nickname);
        bool IsValidLogin(string groupId, string nickname);
        bool Login(string groupId, string nickname);
        void Logout();
        void RetrieveNMessages(int number);
        List<IMessage> DisplayNMessages(int number); 
        List<IMessage> DisplayAllMessagesFromUser(string groupId, string nickname);
        void Send(string messageContent);

    }
}
