using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ChatRoomProject.PersistentLayer
{
    class MessageHandler
    {
        public static void SaveToFile(Guid id, string nickname, string groupID, DateTime date, string messageContent)
        {
            string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            startupPath += "\\PersistentLayer\\DataMessages.txt";
            using (StreamWriter sw = File.AppendText(startupPath))
            {
                sw.WriteLine(id.ToString() + "," + nickname + "," + groupID + "," + date.ToString() + "," + messageContent );
              
            }
        }
        public static List<String> RestoreMessages()
        {
            List<string> messageList = new List<string>();
            string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            startupPath += "\\PersistentLayer\\DataMessages.txt";
            var lines = System.IO.File.ReadAllLines(startupPath);
            foreach (string item in lines)
            {
                messageList.Add(item);
            }
            return messageList;

        }
    }
}
