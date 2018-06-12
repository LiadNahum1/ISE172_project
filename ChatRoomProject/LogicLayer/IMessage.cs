using System;

namespace ChatRoomProject.LogicLayer
{
    public interface IMessage
    {
        Guid Id { get; set; }
        string UserName { get; set; }
        DateTime Date { get; set; }
        string MessageContent { get; set; }
        int GroupID { get; set; }
        string ToString();
    }
}