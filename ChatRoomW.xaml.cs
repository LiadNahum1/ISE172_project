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
namespace ChatRoomProject
{
    /// <summary>
    /// Interaction logic for ChatRoomW.xaml
    /// </summary>
    public partial class ChatRoomW : Window
    {
        public ChatRoomW(ChatRoom chat)
        {
            InitializeComponent();
            hellowUserId.Content = ("hii");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {

        }
        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
