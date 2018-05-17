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
using log4net;
namespace ChatRoomProject.PresentationLayer
{
    /// <summary>
    /// Interaction logic for ChatRoomW.xaml
    /// </summary>
    public partial class ChatRoomW : Window
    {
        private string nickName;
        private string groupId;
        private string currChose;
        private string sort;
        private string filter;
        private bool ascending;
        private ChatRoom chat;
        private DispatcherTimer dispatcherTimer;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ObservableObjectChatRoom _main = new ObservableObjectChatRoom();
        public ChatRoomW(ChatRoom chat)
        {
            log.Info("user entered chat room");
            InitializeComponent();
            this.DataContext = _main;
            this.chat = chat;
            nickName=null;
            groupId=null;
            sort= "SortByTimestamp"; //default
            currChose = "SortByTimestamp";//default
            filter =null; //default
            ascending = true;//default
            inisializeFilterandSorter();
            List<string> messagesFromLogic = chat.MessageManager(this.ascending, this.filter, this.sort, this.groupId, this.nickName);
            foreach (string msg in messagesFromLogic)
            {
                _main.Messages.Add(msg);
            }
            _main.Nickname = chat.getCurrentUser().Nickname();
            this.dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            dispatcherTimer.Start(); //TimerBegin

        }
        //this is a timer that risponsilble for updating  the new messages from the server every two soconds
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            _main.Messages.Clear();
            // update list of messages sorted and filtered according to the requirements of the user
            List<string> messagesFromLogic = chat.MessageManager(this.ascending, this.filter, this.sort, this.groupId, this.nickName); 
            foreach (string msg in messagesFromLogic){ 
                _main.Messages.Add(msg);
            }
        }
       // a help function that inisialize the choose boxeses that will appear on the application with the sorting and filter options
        private void inisializeFilterandSorter()
        {
            ComboBoxItem sortOpAsc = new ComboBoxItem();
            sortOpAsc.Content = "ascending";
            sortOrder.Items.Add(sortOpAsc);
            ComboBoxItem sortOpDes = new ComboBoxItem();
            sortOpDes.Content = "descending";
            sortOrder.Items.Add(sortOpDes);
            ComboBoxItem filterOpNone = new ComboBoxItem();
            filterOpNone.Content = "None";
            filterOptions.Items.Add(filterOpNone);
            ComboBoxItem filterOpUser = new ComboBoxItem();
            filterOpUser.Content = "filterByUser";
            filterOptions.Items.Add(filterOpUser);
            ComboBoxItem filterOpId = new ComboBoxItem();
            filterOpId.Content = "filterById";
            filterOptions.Items.Add(filterOpId);
        }
      //the function tells the chat room that the user logged out and close the chet room page
        private void Button_Click_LogOut(object sender, RoutedEventArgs e)
        {
            log.Info("user exit chat room");
            chat.Logout();
            this.dispatcherTimer.Stop();// user logged out and timer Stop
            MainWindow main = new MainWindow(this.chat);
            main.Show();
            this.Close();
        }
     
        // a functions that occur when the user try to send a message and sends the chatroom the send request 
        private void Button_Click_send(object sender, RoutedEventArgs e)
        {
            try
            {
                log.Info("user try to send a message");
                chat.Send(_main.MessageContent);
                _main.MessageContent = "";
            }
            catch(Exception error)
            {
                log.Error("user send an illigal message");
                MessageBox.Show(error.Message,"Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

   //this function accur when the user press the filter and sort buttons 
        private void Button_Click_FAS(object sender, RoutedEventArgs e)
        {
            //this will check the sort options
            try
            {
                this.ascending = _main.IsAscending.Equals("ascending");
                this.sort = currChose;
                log.Info("the yser choose" + _main.IsAscending +"and" + this.sort +"order to sort by");
            }
            catch (Exception error)
            {
                log.Error("the system faild to get user choise");
                MessageBox.Show(error.Message);
            }
            //the will check the user choise of filter
            try
            {
                if (_main.Filter == "filterByUser")
                {
                    if (_main.FNickName.Equals(""))
                    {
                        log.Error("the user didnt choose a filter name for the user");
                        throw new Exception("Please choose user nickname to filter by");
                    }
                    if (_main.FId.Equals(""))
                    {
                        log.Error("the user didnt choose a filter Id for the user");
                        throw new Exception("Please choose user Id to filter by");
                    }
                    else
                    {
                        log.Info("the user filtered secsesfully by user ");
                        this.filter = "filterByUser";
                        this.nickName = _main.FNickName;
                        this.groupId = _main.FId;
                    }   
                }
                else if (_main.Filter.Equals("filterById"))
                {

                    if (_main.FId.Equals(""))
                    {
                        log.Error("the user didnt choose a filter id");
                        throw new Exception("Please choose user Id to filter by");
                    }
                    else
                    {
                        log.Info("the user filtered secsesfully by user id");
                        this.filter = "filterByGroupId";
                        this.groupId = _main.FId;

                    }
                }
                else if (_main.Filter == "None")
                {
                    log.Info("the user chose not to filter");
                    _main.FId = "";
                    _main.FNickName = "";
                    this.filter = null; 
                }


            }
            catch (Exception error)
            {
                MessageBox.Show(_main.Filter);
                MessageBox.Show(error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /*the next three functions will handel the user choise of sorting order
         * and will save the current choise until he/she will press filter and sort
        */
        private void RadioButton_checked_name(object sender, RoutedEventArgs e)
        {
            currChose = "SortByNickName";
        }

        private void RadioButton_checked_time(object sender, RoutedEventArgs e)
        {
            currChose = "SortByTimestamp";
        }

        private void RadioButton_checked_allSort(object sender, RoutedEventArgs e)
        {
            currChose = "SortByIdNicknameTimestamp";
        }


        private void filterOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show(_main.Filter);
            if (_main.Filter.Equals("filterByUser"))
            {
                _main.IDVisibility = Visibility.Visible;
                _main.TextIDVisibility = Visibility.Visible;
                _main.NameVisibility = Visibility.Visible;
                _main.TextNameVisibility = Visibility.Visible;
            }
            else if (_main.Filter.Equals("filterById"))
            {
                _main.IDVisibility = Visibility.Visible;
                _main.TextIDVisibility = Visibility.Visible;
                _main.NameVisibility = Visibility.Hidden;
                _main.TextNameVisibility = Visibility.Hidden;
            }
            else
            {
                _main.IDVisibility = Visibility.Hidden;
                _main.TextIDVisibility = Visibility.Hidden;
                _main.NameVisibility = Visibility.Hidden;
                _main.TextNameVisibility = Visibility.Hidden;
            }
        }

      
    }
}
