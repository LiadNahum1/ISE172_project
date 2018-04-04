using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRoomProject.LogicLayer;
using ChatRoomProject.CommunicationLayer;
using log4net;
namespace ChatRoomProject.PresentationLayer
{
    class Gui
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("ChatRoom.cs");
        private ChatRoom chatroom;

        public Gui(ChatRoom chat)
        {
            chatroom = chat;
        }
        public void Start()
        {

            Console.WriteLine("Welcome to our Chat Room");
            while (true)
            {
                Console.WriteLine("Press 1 - to registrate \n" +
                      "Press 2- to login \n" +
                      "Press 0 -to exit");
                Console.WriteLine("-----------------------------");
                try
                {
                    int request = int.Parse(Console.ReadLine());
                    switch (request)
                    {
                        case 0:
                            {
                                log.Info("the user had left");
                                return; // exit the function and close the cmd
                            }
                        case 1:
                            {
                                Console.WriteLine("Registeration Window");
                                Register();
                                break;
                            }
                        case 2:
                            {
                                Console.WriteLine("Login Window");
                                Login();
                                break;
                            }

                        default:
                            {
                                Console.WriteLine("You entered illegall value, please insert a correct value");
                                break;
                            }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Enter something");
                }

            }

        }
        public void Register()
        {
            string groupId;
            string nickname;
            Console.WriteLine("Insert GroupId:");
            groupId = Console.ReadLine();
            Console.WriteLine("Insert nickname: ");
            nickname = Console.ReadLine();
            try
            {
                chatroom.Registration(groupId, nickname);
                log.Info("the user had registered");
                Console.WriteLine("you had been registered");
            }
            catch (Exception e)
            {
                log.Info("the user had failed to register");
                string exception = e.Message;
                Console.WriteLine(exception);
                Console.WriteLine("We'll take you back to the main chat page, so you can choose which way to proceed.");
            }

        }
        public void Login()
        {
            Console.WriteLine("Please enter your groupId");
            string groupId = Console.ReadLine();
            Console.WriteLine("Please enter your nickname");
            string nickname = Console.ReadLine();
            try
            {
                chatroom.Login(groupId, nickname);
                log.Info("the user loged in");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                log.Info("the user faild to log in");
                Console.WriteLine("if you want to try to login again-press 1 \n" + "if you want to go back to the Main Page press-2");
                int choose = int.Parse(Console.ReadLine());
                switch (choose)
                {
                    case 1:
                        {
                            Console.WriteLine("We will repeat the login process,please insert the correct values");
                            Login();
                            break;
                        }
                    case 2:
                        {
                            Start();
                            break;
                        }
                }
            }
            try // If the connection succeeded
            {
                    AfterLogin();
                }
                catch (Exception)
                {
                    log.Error("the system had failed");
                    Console.WriteLine("Something wrong");
                }
            }
            
        public void AfterLogin()
        {
            Boolean LogedIn = true;
            while (LogedIn)
            {
                Console.WriteLine("If you want to send a message press 1" + "\n" +
                    "If you want to retrieve 10 messages press 2" + "\n" +
                    "If you want to Display 20 last messages press 3" + "\n" +
                    "If you want to Logout press 4");
                Console.WriteLine("--------------------------------------------------");
                try
                {
                    int choose = int.Parse(Console.ReadLine());
                    switch (choose)
                    {
                        case 1:
                            Send();
                            break;
                        case 2:
                            chatroom.RetrieveNMessages(10);
                            log.Info("the user retrieved messages");
                            break;
                        case 3:
                            Display(20);
                            break;
                        case 4:
                            Logout();
                            Console.WriteLine("you are now loged out");
                            log.Info("the user loged out");
                            LogedIn = false;
                            break;
                        default:
                            Console.WriteLine("you press ilegal number, we will repeat our questions again");
                            log.Info("the user pressed an ilegal value");
                            break;
                    }
                }
                catch (Exception)
                {
                    log.Info("the user pressed an ilegal value");
                    Console.WriteLine("Enter Something");
                }
            }

        }
        public void Display(int number)
        {

            List<IMessage> messages = chatroom.DisplayNMessages(number);
            foreach (Message msg in messages)
            {
                Console.WriteLine(msg.ToString());
            }
        }
        public void Logout()
        {
            chatroom.Logout();
        }
        public void Send()
        {
            Console.WriteLine("Please enter your message, it can be only under 150 words");
            string messagetosend = Console.ReadLine();
            try
            {
                chatroom.Send(messagetosend);
                log.Info("send a message");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                log.Info("the user failed to send a message");
                Console.WriteLine("We will repeat our questions, please enter legal message");
                Send();

            }

        }

    }
}
