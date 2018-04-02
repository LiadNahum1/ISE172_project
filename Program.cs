using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRoomProject.LogicLayer;
using ChatRoomProject.PresentationLayer;
using System.IO;
using log4net;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace ChatRoomProject
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Program.cs");
        static void Main(string[] args)
        {
           
                ChatRoom cr = new ChatRoom();
                cr.Start();
                Gui gui = new Gui(cr);
                gui.Start();


        }
    }
}
