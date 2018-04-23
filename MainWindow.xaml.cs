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

namespace ChatRoomProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChatRoom chat;
        public MainWindow()
        {
            InitializeComponent();
            this.chat = new ChatRoom();
            this.chat.Start();
        }
        public MainWindow(ChatRoom chat)
        {
            InitializeComponent();
            this.chat = chat;
        }

        private void Registrate_Click(object sender, RoutedEventArgs e)
        {
            Registration regWindow = new Registration(this.chat);
            regWindow.Show();
            this.Close();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login loWindow = new Login(this.chat);
            loWindow.Show();
            this.Close();
        }
    }
}
