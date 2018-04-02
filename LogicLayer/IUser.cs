using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRoomProject.CommunicationLayer;

namespace ChatRoomProject.LogicLayer
{
    public interface IUser
    {
        string GroupID();
        string Nickname();
        IMessage Send(string messageContent);

    }
}
