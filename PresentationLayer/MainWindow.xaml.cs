using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatRoomProject.LogicLayer;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace ChatRoomProject.PresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("MianWinsow.xaml.cs");
        private ChatRoom chat;

        //Main window which includes the main menu. This constructor is called once in the program
        public MainWindow()
        {
            InitializeComponent();
            this.chat = new ChatRoom();
            this.chat.Start();
        }

        //The constructor is called when the client returns from other windows to the main windo
        public MainWindow(ChatRoom chat)
        {
            InitializeComponent();
            this.chat = chat;
        }

        //Event of registrate button. Open the registration window 
        private void Registrate_Click(object sender, RoutedEventArgs e)
        {
            Registration regWindow = new Registration(this.chat);
            regWindow.Show();
            this.Close();
        }

        //Event of login button. Open the login window 
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login loWindow = new Login(this.chat);
            loWindow.Show();
            this.Close();
        }

        //Event of exit button. Close the program if asked 
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if(result == MessageBoxResult.Yes)
            {
                this.Close();
            }

        }
    }
}
