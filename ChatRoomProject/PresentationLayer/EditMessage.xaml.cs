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
        private string lastMessage;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ObservableObjectChatRoom _main = new ObservableObjectChatRoom();
        public EditMessage(ChatRoom chat , string lastMessage)
        {
            InitializeComponent();
            this.chat = chat;
            this.lastMessage = lastMessage;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                chat.EditMessage(_main.NewMessageContent, this.lastMessage);
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
        }

        private void btnDialogCENcel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
