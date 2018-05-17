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
using System.Windows.Shapes;
using ChatRoomProject.LogicLayer;

namespace ChatRoomProject.PresentationLayer
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("ChatRoom.cs");
        private ChatRoom chat;
        ObservableObjectChatRoom _main = new ObservableObjectChatRoom();

        public Registration(ChatRoom chat)
        {
            InitializeComponent();
            this.chat = chat;
            this.DataContext = _main; 
        }

        //Call to Registration function in ChatRoom. If there are no problems, open the main window again  
        private void Registrate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.chat.Registration(_main.GroupId, _main.Nickname);
                log.Info("The user " + _main.GroupId + ":" + _main.Nickname + "registered");
                MessageBox.Show("You had been registered", "Reagistration", MessageBoxButton.OK, MessageBoxImage.None);
                MainWindow window = new MainWindow(this.chat);
                window.Show();
                this.Close();

            }
            catch (Exception err)
            {
                log.Info("Registration has failed." + err.Message);
                MessageBox.Show(err.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //Open the main window and close this one
        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow(this.chat);
            main.Show();
            this.Close();
        }

    
    }
}
