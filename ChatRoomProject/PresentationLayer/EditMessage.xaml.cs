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
using log4net;
namespace ChatRoomProject.PresentationLayer
{
    /// <summary>
    /// Interaction logic for EditMessage.xaml
    /// </summary>
    public partial class EditMessage : Window
    {
        private ChatRoom chat;
        private IMessage lastMessage;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ObservableObjectChatRoom _main;
        public EditMessage(ChatRoom chat , IMessage lastMessage , ObservableObjectChatRoom _main)
        {
            InitializeComponent();
            this.chat = chat;
            this.lastMessage = lastMessage;
            this.DataContext = _main;
            this._main = _main;
        }
        //this will send the chat room request for editing
        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            string newContent = _main.NewMessageContent;
            try
            {
                chat.EditMessage(newContent, this.lastMessage);
                _main.NewMessageContent = "";
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
        }
      //this wiil cencel the users choise and close the window
        private void btnDialogCENcel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
