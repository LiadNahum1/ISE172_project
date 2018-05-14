using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatRoomProject.PresentationLayer
{
    public class ObservableObjectChatRoom : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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


        private string isAscending = "";
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

        private float sliderTwoWay = 0.0f;
        public float SliderTwoWay
        {
            get
            {
                return sliderTwoWay;
            }
            set
            {
                if (value >= 0.0 && value <= 100.0)
                {
                    sliderTwoWay = value;
                    OnPropertyChanged("SliderTwoWay");
                }
            }
        }

        

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}