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
using System.Timers;
using ChatRoomProject.LogicLayer;
using ChatRoomProject.CommunicationLayer;
using System.Windows.Threading;
namespace ChatRoomProject
{
    /// <summary>
    /// Interaction logic for ChatRoomW.xaml
    /// </summary>
    public partial class ChatRoomW : Window
    {
        private ChatRoom chat;
        private Boolean[] sortChoses;
        const int sortOptionNumber = 3;
        //  private Timer timer;
        private DispatcherTimer dispatcherTimer;
        public ChatRoomW(ChatRoom chat)
        {
            InitializeComponent();
            this.chat = chat;
            hellowUserId.Content = ("hii" + chat.getCorrantUser().Nickname());
            inisializeSorter();
            this.dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            dispatcherTimer.Start();

            //this.timer = new Timer(2000);
            //timer.AutoReset = true;
            //timer.Elapsed += (sender, e) => OnTimedEvent(sender, e,chat,this);
            //timer.Start();// timer begin because the user logged in
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            chat.RetrieveNMessages(10); 
            List<IMessage> msg = chat.DisplayNMessages(chat.getCount_of_new_message()); // update the data
            chat.setCount_of_new_message(0);
            messageVieu.Items.Add(msg);
           // messageVieu.ItemsSource = msg;
           

        }
        /*
        public static void OnTimedEvent(object source, ElapsedEventArgs e,ChatRoom chat,ChatRoomW chat_gui)
        {
           chat.RetrieveNMessages(10);
           List<IMessage> msg = chat.DisplayNMessages(chat.getCount_of_new_message()); // update the data
           chat.setCount_of_new_message(0);
           chat_gui.messageVieu.ItemsSource = msg;
            MessageBoxResult result = MessageBox.Show("HEY MAN");

        }
        */

        private void inisializeSorter()
        {
            ComboBoxItem comboBoxItem1 = new ComboBoxItem();
            comboBoxItem1.Content = "assending";
            sortOrder.Items.Add(comboBoxItem1);
            ComboBoxItem comboBoxItem2 = new ComboBoxItem();
            comboBoxItem2.Content = "dessending";
            sortOrder.Items.Add(comboBoxItem2);
            sortChoses = new Boolean[sortOptionNumber];
            for (int i = 0; i< sortOptionNumber; i++)
            {
                sortChoses[i] = false;
            }
        }
        private void CheckBox_Unchecked_time(object sender, RoutedEventArgs e)
        {
            sortChoses[0] = false;
        }
        private void CheckBox_checked_time(object sender, RoutedEventArgs e)
        {
            sortChoses[0] = true;
        }
        private void CheckBox_Unchecked_name(object sender, RoutedEventArgs e)
        {
            sortChoses[1] = false;
        }
        private void CheckBox_checked_name(object sender, RoutedEventArgs e)
        {
            sortChoses[1] = true;
        }
        private void CheckBox_Unchecked_Id(object sender, RoutedEventArgs e)
        {
            sortChoses[2] = false;
        }
        private void CheckBox_checked_Id(object sender, RoutedEventArgs e)
        {
            sortChoses[2] = true;
        }
        
        
        private void Button_Click_sort(object sender, RoutedEventArgs e)
        {
            try{
                bool isAssending = (sortOptions.DataContext.ToString().Equals("assending"));
                if (sortChoses[0] & sortChoses[1] & sortChoses[2])
                    messageVieu.ItemsSource = chat.SortByIdNicknameTimestamp(isAssending);
                else
                {
                    if (sortChoses[0])
                        messageVieu.ItemsSource = chat.SortTimestamp(isAssending);
                    else if (sortChoses[1])
                        messageVieu.ItemsSource = chat.SortByNickname(isAssending);
                    else
                        throw new Exception("didnt choose options to sort by");
                }

            }
            catch (Exception error) // TODO add error 
            {
                MessageBox.Show("please choose sorting options");
            }

        }

       
        private void Button_Click_LogOut(object sender, RoutedEventArgs e)
        {
            chat.Logout();
            //  timer.Stop();// user logged out
            this.dispatcherTimer.Stop();
            MainWindow main = new MainWindow(this.chat);
            main.Show();
            this.Close();
        }
        
        private void Button_Click_send(object sender, RoutedEventArgs e)
        {
            try
            {
                chat.Send(messageContent.Text);
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void Button_Click_FUser(object sender, RoutedEventArgs e)
        {
            try {
                if(!userFilter.Text.Contains(","))
                    throw new Exception("please choose user to filter by write id,nickname");
                else
                {
                    string[] userData = userFilter.Text.Split(',');
                    if (!(userData.Length == 2))
                        throw new Exception("please choose user to filter by write id,nickname");
                    else
                        messageVieu.ItemsSource = chat.FilterByUser(userData[0], userData[1]);
                }
            }
            catch(Exception error) {
                MessageBox.Show(error.Message);
            }
        }

        private void Button_Click_FId(object sender, RoutedEventArgs e)
        {
            try
            {
                messageVieu.ItemsSource = chat.FilterByGroupId(IdFilter.Text);
            }
            catch(Exception error) { MessageBox.Show(error.Message); }
        }
    }
}
