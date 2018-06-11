using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRoomProject.DataAccess;
namespace ChatRoomProject.LogicLayer
{
    public interface IUser
    {
        string Password();
        /*return the groupId of the user*/
        string GroupID();
        /*return the nickname of the user*/
        string Nickname();
        /*Call Persistenet Layer's function in order to save user's details in the system files*/
        //void Save();
        /*The function gets a string of a message content and sends it to the server. Recieves an IMessage from server and returns it. */
        void Send(string messageContent);

    }
}
