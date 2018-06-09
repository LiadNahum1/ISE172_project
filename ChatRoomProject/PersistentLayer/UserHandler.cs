using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ChatRoomProject.PersistentLayer
{
    public class UserHandler
    {
        /*public static void SaveToFile(string nickname, string groupId)
        {
            string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            startupPath += "\\PersistentLayer\\DataUsers.txt";
            using (StreamWriter sw = File.AppendText(startupPath))
            {
                sw.WriteLine(groupId + "," + nickname);

            }
        }

        public static List<string> RestoreUsers()
        {
            List<string> userList = new List<string>();
            string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            startupPath += "\\PersistentLayer\\DataUsers.txt";
            var lines = System.IO.File.ReadAllLines(startupPath);
            foreach (string item in lines)
            {
                userList.Add(item);
            }
            return userList;
        }


        public static List<string> groupIDs()
        {
            throw new NotImplementedException();
        }*/

    }
}
