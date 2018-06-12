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
        int GroupID();
        /*return the nickname of the user*/
        string Nickname();
    }
}
