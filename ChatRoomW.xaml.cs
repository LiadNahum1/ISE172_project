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
        private string nickName;
        private string groupId;
        private string sort;
        private string filter;
        private bool ascending;
        private ChatRoom chat;
        private DispatcherTimer dispatcherTimer;
        public ChatRoomW(ChatRoom chat)
        {
            InitializeComponent();
           // messageVieu.ItemsSource = chat.DisplayNMessages(5);
            this.chat = chat;
            nickName=null;
            groupId=null;
            sort= "SortByTimestamp";
            filter=null;
            ascending=false;
        hellowUserId.Content = ("hii" + chat.getCorrantUser().Nickname());
            inisializeFilterandSorter();
            this.dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            dispatcherTimer.Start();

        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            List<IMessage> msg = chat.MessageManager(this.ascending, this.filter, this.sort, this.groupId, this.nickName);
            messageVieu.ItemsSource = msg;
        }
       
        private void inisializeFilterandSorter()
        {
            ComboBoxItem sortOpAss = new ComboBoxItem();
            sortOpAss.Content = "assending";
            sortOrder.Items.Add(sortOpAss);
            ComboBoxItem sortOpDess = new ComboBoxItem();
            sortOpDess.Content = "dessending";
            sortOrder.Items.Add(sortOpDess);
            ComboBoxItem filterOpUser = new ComboBoxItem();
            filterOpUser.Content = "user";
            filterOptions.Items.Add(filterOpUser);
            ComboBoxItem filterOpName = new ComboBoxItem();
            filterOpName.Content = "user";
            filterOptions.Items.Add(filterOpName);
        }
        private void CheckBox_Unchecked_time(object sender, RoutedEventArgs e)
        {
            
        }
        private void CheckBox_checked_time(object sender, RoutedEventArgs e)
        {
            this.sort = "SortByTimestamp";
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
                    chat.SortByIdNicknameTimestamp(isAssending);
                else
                {
                    if (sortChoses[0])
                        chat.SortTimestamp(isAssending);
                    else if (sortChoses[1])
                        chat.SortByNickname(isAssending);
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
            this.dispatcherTimer.Stop();// user logged out
            MainWindow main = new MainWindow(this.chat);
            main.Show();
            this.Close();
        }
        
        private void Button_Click_send(object sender, RoutedEventArgs e)
        {
            try
            {
                chat.Send(messageContent.Text);
                messageContent.Text = "";
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
                        chat.FilterByUser(userData[0], userData[1]);
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
                chat.FilterByGroupId(IdFilter.Text);
            }
            catch(Exception error) { MessageBox.Show(error.Message); }
        }
    }
}
