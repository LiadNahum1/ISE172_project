using System;

namespace ChatRoomProject.DataAccess
{
    public interface IMessage
    {
        Guid Id { get; }
        string UserName { get; }
        DateTime Date { get; }
        string MessageContent { get; }
        int GroupID { get; }
        string ToString();
    }
}