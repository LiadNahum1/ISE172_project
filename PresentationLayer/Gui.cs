using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRoomProject.LogicLayer;
using ChatRoomProject.CommunicationLayer;

namespace ChatRoomProject.PresentationLayer
{
    class Gui
    {
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
                          "Press 2- to login +\n" +
                          "Press 0 -to exit");
                    Console.WriteLine("-----------------------------");
                try
                {
                    int request = int.Parse(Console.ReadLine());
                    switch (request)
                    {
                        case 0:
                            {
                                return; // exit the function and close the cmd
                            }
                        case 1:
                            {
                                Console.WriteLine("Registeration Window");
                                Register();
                                Console.WriteLine("You have been registered. Please login");
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
                catch(Exception e)
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

            }
            catch (Exception e)
            {
                string exception = e.Message;
                Console.WriteLine(exception);
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("We will repeat the login process,please insert the correct values");
                Register();
            }

            try
            {
                AfterLogin();
            }
            catch(Exception e)
            {
                Console.WriteLine("Something wrong");
            }
        }
        public void AfterLogin()
        {
            while (true)
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
                            chatroom.RetrieveNMessages(10); // TODO - decide if to make it generic
                            break;
                        case 3:
                            Display(20);
                            break;
                        case 4:
                            Logout();
                            break;
                        default:
                            Console.WriteLine("you press illegal number, we will repeat our questions again");
                            break;
                    }
                }
                catch(Exception e)
                {
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("We will repeat our questions, please enter legal message");
                Send();

            }

        }
       
    }
}
