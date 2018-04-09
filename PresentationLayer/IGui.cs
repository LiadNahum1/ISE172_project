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
        void Start();
        void Register();
        void Login();
        void AfterLogin();
        void Display(int number);
        void DisplayFromSpecificUser(string groupId, string nickname);
        void Logout();
        void Send();
    }
}
