using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRoomProject.LogicLayer;

namespace ChatRoomProject.PresentationLayer
{
    interface IGui
    {
        void BuildGui(ChatRoom chat);
        void Start();
        void Registration(string nickname, string groupId);
        void Login(User user);
    }
}
