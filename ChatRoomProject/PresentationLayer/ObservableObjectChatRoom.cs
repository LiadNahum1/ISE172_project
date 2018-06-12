using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ChatRoomProject.LogicLayer;

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
        public ObservableCollection<IMessage> Messages { get; } = new ObservableCollection<IMessage>();
        public ObservableCollection<string> SortOp { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> FilterOp { get; } = new ObservableCollection<string>();
        public ObservableObjectChatRoom()
        {
            Messages.CollectionChanged += Messages_CollectionChanged;
            SortOp.CollectionChanged += SortOp_CollectionChanged;
            FilterOp.CollectionChanged += FilterOp_CollectionChanged;
        }
     
        private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Messages");
        }
        private void SortOp_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("SortOp");
        }

        private void FilterOp_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("FilterOp");
        }
        private string newMessageContent = "";
        public string NewMessageContent
        {
            get
            {
                return newMessageContent;
            }
            set
            {
                newMessageContent = value;
                OnPropertyChanged("NewMessageContent");
            }
        }
        private IMessage lastMessage = new Message("1",1,"");
        public IMessage LastMessage
        {
            get
            {
                return lastMessage;
            }
            set
            {
                lastMessage = value;
                OnPropertyChanged("LastMessage");
            }
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
                filter = value.Split(' ')[value.Split(' ').Length -1];
                OnPropertyChanged("Filter");
            }
        }

        private Visibility textIDVisibility = Visibility.Hidden;
        public Visibility TextIDVisibility
        {
            get
            {
                return textIDVisibility;
            }
            set
            {
                textIDVisibility = value;
                OnPropertyChanged("TextIDVisibility");
            }
        }
        private Visibility idVisibility = Visibility.Hidden;
        public Visibility IDVisibility
        {
            get
            {
                return idVisibility;
            }
            set
            {
                idVisibility = value;
                OnPropertyChanged("IDVisibility");
            }
        }


        private Visibility textNameVisibility = Visibility.Hidden;
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
        private Visibility nameVisibility = Visibility.Hidden;
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