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
                Console.WriteLine("To registrate press 1\n" +
                                  "To login press 2 \n" +
                                  "To exit press 0 ");
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
                                Console.Clear();
                                Console.WriteLine("Registeration Window");
                                Register();
                                break;
                            }
                        case 2:
                            {
                                Console.Clear();
                                Console.WriteLine("Login Window");
                                Login();
                                break;
                            }

                        default:
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("You entered illegall value, please insert a correct value");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                            }
                    }
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Enter some number");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }

        }
        private bool CheckIfInsertSomething(string str)
        {
            if (str == "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please insert some data");
                Console.ForegroundColor = ConsoleColor.Gray;
                return false;
            }
            return true;
        }
        public void Register()
        {
            string groupId;
            string nickname;
            Console.WriteLine("Please insert groupId: ");
            groupId = Console.ReadLine();
            Console.WriteLine("Please insert nickname: ");
            nickname = Console.ReadLine();
            if (!CheckIfInsertSomething(groupId) |!CheckIfInsertSomething(nickname))
            {
                Register();
            }
            try
            {
                chatroom.Registration(groupId, nickname);
                log.Info("The user registered");
                Console.WriteLine("You had been registered");
            }
            catch (Exception e)
            {
                log.Info("The user had failed to register");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("We'll take you back to the main chat page:");
            }

        }
        public void Login()
        {
            Console.WriteLine("Please enter your groupId");
            string groupId = Console.ReadLine();
            Console.WriteLine("Please enter your nickname");
            string nickname = Console.ReadLine();
            if(!CheckIfInsertSomething(groupId) | !CheckIfInsertSomething(nickname))
            {
                Login();
            }
            try
            {
                chatroom.Login(groupId, nickname);
                log.Info("The user logged in");
                try 
                {
                   AfterLogin();
                }
               
                catch (Exception)
                {
                    log.Error("The system had failed");
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
                log.Info("The user faild to log in");
                Console.WriteLine("Your login has been failed. Try again or register in the main Menu: ");
                Start();
            }

        }
        public void AfterLogin()
        {
            bool LogedIn = true;
            Console.WriteLine("You are logged in");
            while (LogedIn)
            {
                Console.WriteLine();
                Console.WriteLine("If you want to send a message press 1" + "\n" +
                    "If you want to retrieve 10 messages press 2" + "\n" +
                    "If you want to Display 20 last messages press 3" + "\n" +
                     "If you want to Display messages from specific user press 4" + "\n" +
                    "If you want to Logout press 5");
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
                            log.Info("The user retrieved 10 messages");
                            Console.WriteLine("The messages has been retrieved");
                            break;

                        case 3:
                            Display(20);
                            break;

                        case 4:
                            Console.WriteLine("Please enter the groupId of the specific user");
                            String groupId = Console.ReadLine();
                            Console.WriteLine("Please enter the nickname of the specific user");
                            String nickname = Console.ReadLine();
                            List<IMessage> ListToDisplay = chatroom.DisplayAllMessagesFromCertainUser(groupId, nickname);
                            DisplayFromSpecificUser(ListToDisplay);
                            break;

                        case 5:
                            Logout();
                            Console.WriteLine("You are now logged out");
                            log.Info("The user logged out");
                            LogedIn = false;
                            break;
                        default:
                            Console.WriteLine("You press illegal number, we will repeat our questions again");
                            log.Info("The user pressed an ilegal value");
                            break;
                    }
                }
                catch (Exception)
                {
                    log.Error("The user pressed an ilegal value");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Enter valid input");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }

        }
        public void Display(int number)// display 20 last messages
        {
            List<IMessage> messages = chatroom.DisplayNMessages(number);
            if (messages.Count() == 0) // if we get an empty list it means that the client didnt retrieve any message 
                Console.WriteLine("You have not retrieve messages, so we will can't show you any message.\nPlease retrieve and then press Display.");
            else
            {
                foreach (Message msg in messages)
                {
                    Console.WriteLine(msg.ToString());
                }
                log.Info("Display last messages");
            }
           
        }
        public void DisplayFromSpecificUser(List<IMessage> messages) // display from specific user
        {
            if (messages.Count() == 0)
            {
                Console.WriteLine("The specific user didn't send any message from the the messages you retrieved.");
                log.Info("There are no messages to display");
            }
                
            else
            {
                foreach (Message msg in messages)
                {
                    Console.WriteLine(msg.ToString());
                }
                log.Info("The function displays messages from specific user");
            }
        }
        public void Logout()
        {
            chatroom.Logout();
        }
        public void Send()
        {
            Console.WriteLine("Please enter your message, it can be only under 150 characters");
            string messagetosend = Console.ReadLine();
            try
            {
                chatroom.Send(messagetosend);
                log.Info("User sends a message");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
                log.Error(e.Message);
                Console.WriteLine("We will repeat our questions, please enter legal message");
                Send();

            }

        }

    }
}
