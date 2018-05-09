using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ChatRoomProject.CommunicationLayer;

namespace ChatRoomProject.PresentationLayer
{
    class ObservableObjectChatRoom : INotifyPropertyChanged
    {
        private List<IMessage> messageVieu;
        public event PropertyChangedEventHandler PropertyChanged;

        public List<IMessage> MessageVieu
        {
            get
            {
                return messageVieu;
            }
            set
            {
                messageVieu =ChatRoomW.
            }
        }

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
