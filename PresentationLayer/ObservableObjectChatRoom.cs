using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ChatRoomProject.PresentationLayer
{
    public class ObservableObjectChatRoom : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string groupId = "";
        public string GroupId
        {
            get
            {
                return groupId;
            }
            set
            {
                groupId = value;
                OnPropertyChanged("GroupId");
            }
        }
        private string nickname = "";
        public string Nickname
        {
            get
            {
                return nickname;
            }
            set
            {
                nickname = value;
                OnPropertyChanged("Nickname");
            }
        }
        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();
        public ObservableObjectChatRoom()
        {
            Messages.CollectionChanged += Messages_CollectionChanged;
        }
     

        private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Messages");
        }

        private string messageContent = "";
        public string MessageContent
        {
            get
            {
                return messageContent;
            }
            set
            {
                messageContent = value;
                OnPropertyChanged("MessageContent");
            }
        }

        private string fId = "";
        public string FId
        {
            get
            {
                return fId;
            }
            set
            {
                fId = value;
                OnPropertyChanged("FId");
            }
        }

        private string fNickName = "";
        public string FNickName
        {
            get
            {
                return fNickName;
            }
            set
            {
                fNickName = value;
                OnPropertyChanged("FNickName");
            }
        }


        private string isAscending = "ascending";
        public string IsAscending
        {
            get
            {
                return isAscending;
            }
            set
            {
                isAscending = value;
                OnPropertyChanged("IsAscending");
            }
        }

        private string filter = "";
        public string Filter
        {
            get
            {
                return filter;
            }
            set
            {
                filter = value;
                OnPropertyChanged("Filter");
            }
        }

        private Visibility textNameVisibility = Visibility.Collapsed;
        public Visibility TextNameVisibility
        {
            get
            {
                return textNameVisibility;
            }
            set
            {
                textNameVisibility = value;

                OnPropertyChanged("TextNameVisibility");
            }
        }
        private Visibility nameVisibility = Visibility.Collapsed;
        public Visibility NameVisibility
        {
            get
            {
                return nameVisibility;
            }
            set
            {
                nameVisibility = value;
                OnPropertyChanged("NameVisibility");
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}