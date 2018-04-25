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
        private ChatRoom chat;
        private Boolean[] sortChoses;
        private Boolean[] filterChoses;
        const int sortOptionNumber = 3;
        const int filterOptionNumber = 2;
        public ChatRoomW(ChatRoom chat)
        {
            InitializeComponent();
            this.chat = chat;
            hellowUserId.Content = ("hii" + chat.getCorrantUser().Nickname());
            inisializeFilterAndSorter();
        }
        private void inisializeFilterAndSorter()
        {
            ComboBoxItem comboBoxItem1 = new ComboBoxItem();
            comboBoxItem1.Content = "assending";
            sortOrder.Items.Add(comboBoxItem1);
            ComboBoxItem comboBoxItem2 = new ComboBoxItem();
            comboBoxItem2.Content = "dessending";
            sortOrder.Items.Add(comboBoxItem2);
            sortChoses = new Boolean[sortOptionNumber];
            filterChoses = new Boolean[filterOptionNumber];
            for (int i = 0; i< sortOptionNumber; i++)
            {
                sortChoses[i] = false;
            }
            for (int i = 0; i < filterOptionNumber; i++)
            {
                filterChoses[i] = false;
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
        
        private void CheckBox_Unchecked_FID(object sender, RoutedEventArgs e)
        {
            filterChoses[0] = false;
        }
        private void CheckBox_Checked_FID(object sender, RoutedEventArgs e)
        {
            filterChoses[0] = true;
        }
        private void CheckBox_Unchecked_FName(object sender, RoutedEventArgs e)
        {
            filterChoses[1] = false;
        }
        private void CheckBox_Checked_FName(object sender, RoutedEventArgs e)
        {
            filterChoses[1] = true;
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
                        throw new Exception("didnt chose options to soort by");
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("plese chose sorting options");
            }

        }

        private void Button_Click_Filter(object sender, RoutedEventArgs e)
        {
            try
            {
              

            }
            catch (Exception error)
            {
                MessageBox.Show("plese chose filter options");
            }

        }
        private void Button_Click_LogOut(object sender, RoutedEventArgs e)
        {

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
    }
}
