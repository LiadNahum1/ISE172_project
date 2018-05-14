using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatRoomProject.PresentationLayer
{
    public class ObservableObjectChatRoom : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableObjectChatRoom()
        {
            Messages.CollectionChanged += Messages_CollectionChanged;
        }

        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();

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