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
        /*The method restores the users and the messages that had been saved in the system files from previous use */
        void Start();
        /*The method registrates a new user to the system. The method first checks if nickname input is legal.
         * If it is, the method creats new User instance and adds him to the users list. 
         * If nickname is already been used by the same groupId, the function throws an exception
         */
        void Registration(string groupId, string nickname);
        /*Check if the user is registered. If he is, returns true. Otherwise, returns false.*/
        bool Login(string groupId, string nickname);
        /*Logout the current user which is connected now*/
        void Logout();
        /*The function retrieves 10 last messages from server. The function adds only the new messages to the messages list
        * The head of the list points to the oldest message.
        */
        void RetrieveNMessages(int number);
        /*Send messages. If empty or more than 150 characters throw an exception.
        Otherwise, send it and save into messages list*/
        void Send(string messageContent);

    }
}
