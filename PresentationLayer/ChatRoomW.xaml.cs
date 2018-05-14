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
namespace ChatRoomProject.PresentationLayer
{
    /// <summary>
    /// Interaction logic for ChatRoomW.xaml
    /// </summary>
    public partial class ChatRoomW : Window
    {
        List<IMessage> messages;
        private string nickName;
        private string groupId;
        private string sort;
        private string filter;
        private bool ascending;
        private bool[] sortChoses;
        const int sortOptionNumber = 3;
        private ChatRoom chat;
        private DispatcherTimer dispatcherTimer;
        ObservableObjectChatRoom _main = new ObservableObjectChatRoom();
        public ChatRoomW(ChatRoom chat)
        {

            InitializeComponent();
            this.DataContext = _main;
            this.chat = chat;
            nickName=null;
            groupId=null;
            sort= "SortByTimestamp";
            filter=null;
            ascending=false;
            inisializeFilterandSorter();
            this.dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            dispatcherTimer.Start();

        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            List<string> messagesFromLogic = chat.MessageManager(this.ascending, this.filter, this.sort, this.groupId, this.nickName);
            _main.Messages.Clear();
            foreach (string msg in messagesFromLogic){
                _main.Messages.Add(msg);
            }
        }
       
        private void inisializeFilterandSorter()
        {
            ComboBoxItem sortOpAsc = new ComboBoxItem();
            sortOpAsc.Content = "ascending";
            sortOrder.Items.Add(sortOpAsc);
            ComboBoxItem sortOpDes = new ComboBoxItem();
            sortOpDes.Content = "descending";
            sortOrder.Items.Add(sortOpDes);
            ComboBoxItem filterOpUser = new ComboBoxItem();
            filterOpUser.Content = "filterByUser";
            filterOptions.Items.Add(filterOpUser);
            ComboBoxItem filterOpName = new ComboBoxItem();
            filterOpName.Content = "filterByName";
            filterOptions.Items.Add(filterOpName);
            sortChoses = new Boolean[sortOptionNumber];
            for (int i = 0; i < sortOptionNumber; i++)
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
        
        
       
        private void Button_Click_LogOut(object sender, RoutedEventArgs e)
        {
            chat.Logout();
            this.dispatcherTimer.Stop();// user logged out
            MainWindow main = new MainWindow(this.chat);
            main.Show();
            this.Close();
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click_send(sender, e);
            }
        }
        private void Button_Click_send(object sender, RoutedEventArgs e)
        {
            try
            {
                chat.Send(_main.MessageContent);
                _main.MessageContent = "";
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

   
        private void Button_Click_FAS(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ascending = (sortOptions.DataContext.ToString().Equals("ascending"));
                if (sortChoses[0] & sortChoses[1] & sortChoses[2])
                    this.sort = "SortByIdNicknameTimestamp";
                else
                {
                    if (sortChoses[0])
                        this.sort = "SortByTimestamp";
                    else if (sortChoses[1])
                        this.sort = "SortByNickName";
                    else
                        throw new Exception("didnt choose options to sort by");
                }

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            try
            {
                if (filterOptions.DataContext.ToString().Equals("filterByUser"))
                {
                    if (FNickName.Text.Equals("")) 
                         throw new Exception("please choose user nickname to filter by");
                    if (FId.Text.Equals(""))
                        throw new Exception("please choose user Id to filter by");
                    this.filter = "filterByUser";
                }
                if (filterOptions.DataContext.ToString().Equals("filterByName"))
                {

                    if (FId.Text.Equals(""))
                        throw new Exception("please choose user Id to filter by");
                    this.filter = "FilterByGroupId";
                }
                else
                    throw new Exception("please choose filter options");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            

        }

        private void RadioButton_Unchecked_Id(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Unchecked_name(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Unchecked_time(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_checked_Id(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_checked_name(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_checked_time(object sender, RoutedEventArgs e)
        {

        }
    }
}
