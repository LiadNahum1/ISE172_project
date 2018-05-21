using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChatRoomProject.LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRoomProject.CommunicationLayer;

namespace ChatRoomProject.LogicLayer.Tests
{
    [TestClass()]
    public class ChatRoomTests
    {

        [TestMethod()]
        public void SortByNicknameTest() // Ascending and Descending
        {
            //arrange
            List<IMessage> listToCheck = new List<IMessage>();
            Message a = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "e", "1", "5/16/2018 10:29:02 AM", "hello", true));
            Message b = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "f", "2", "5/17/2018 10:29:02 AM", "it's a", true));
            Message c = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "c", "3", "5/16/2018 10:29:02 AM", "test only", true));
            Message d = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "b", "4", "5/15/2018 10:29:02 AM", "only", true));
            Message e = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "a", "5", "5/14/2018 10:29:02 AM", "for", true));
            Message f = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "z", "6", "5/16/2018 10:29:02 AM", "nickname", true));
            listToCheck.Add(a);
            listToCheck.Add(b);
            listToCheck.Add(c);
            listToCheck.Add(d);
            listToCheck.Add(e);
            listToCheck.Add(f);
            List<IMessage> resultlistAsc = new List<IMessage>();
            List<IMessage> expectedlistAsc = new List<IMessage>();
            expectedlistAsc.Add(e);
            expectedlistAsc.Add(d);
            expectedlistAsc.Add(c);
            expectedlistAsc.Add(a);
            expectedlistAsc.Add(b);
            expectedlistAsc.Add(f);
          
            List<IMessage> resultlistDesc = new List<IMessage>();
            List<IMessage> expectedlistDesc = new List<IMessage>();
            expectedlistDesc.Add(f);
            expectedlistDesc.Add(b);
            expectedlistDesc.Add(a);
            expectedlistDesc.Add(c);
            expectedlistDesc.Add(d);
            expectedlistDesc.Add(e);

            //act
            resultlistAsc = ChatRoom.SortByNickname(listToCheck, true); //Ascending sort of nicknames
            resultlistDesc = ChatRoom.SortByNickname(listToCheck, false); //Descending sort of nicknames

            //assert
            for (int i = 0; i < expectedlistAsc.Count(); i++)
                Assert.AreEqual(expectedlistAsc.ElementAt(i), resultlistAsc.ElementAt(i));
            for (int i = 0; i < expectedlistDesc.Count(); i++)
                Assert.AreEqual(expectedlistDesc.ElementAt(i), resultlistDesc.ElementAt(i));
        }

        [TestMethod()]
        public void SortByTimeStampTest() 
        {
            List<IMessage> listToCheck = new List<IMessage>();

            Message a = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "e", "1", "6/16/2018 10:32:04 AM", "hii", true));
            Message b = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "f", "2", "6/16/2018 10:30:02 AM", "hii", true));
            Message c = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "c", "3", "5/16/2018 10:29:07 AM", "hii", true));
            Message d = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "b", "4", "5/18/2018 10:29:03 AM", "hii", true));
            Message e = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "a", "5", "5/16/2017 10:29:01 AM", "hii", true));
            Message f = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "z", "6", "5/16/2018 10:29:08 AM", "hii", true));  
            listToCheck.Add(a);
            listToCheck.Add(b);
            listToCheck.Add(c);
            listToCheck.Add(d);
            listToCheck.Add(e);
            listToCheck.Add(f);

            //descending sort
            List<IMessage> resultlistDesc = new List<IMessage>();
            List<IMessage> expectedlistDesc = new List<IMessage>();
            expectedlistDesc.Add(a);
            expectedlistDesc.Add(b);
            expectedlistDesc.Add(d);
            expectedlistDesc.Add(f);
            expectedlistDesc.Add(c);
            expectedlistDesc.Add(e);
            resultlistDesc = ChatRoom.SortTimestamp(listToCheck, false); //Descending sort of timeStamp

            //ascending sort
            List<IMessage> resultlistAcs = new List<IMessage>();
            List<IMessage> expectedlistAcs = new List<IMessage>();
            expectedlistAcs.Add(e);
            expectedlistAcs.Add(c);
            expectedlistAcs.Add(f);
            expectedlistAcs.Add(d);
            expectedlistAcs.Add(b);
            expectedlistAcs.Add(a);
            //act
            resultlistAcs = ChatRoom.SortTimestamp(listToCheck, true); //Ascending sort of timeStamp
            //assert
            for (int i = 0; i < expectedlistAcs.Count(); i++)
                Assert.AreEqual(expectedlistAcs.ElementAt(i), resultlistAcs.ElementAt(i));
            for (int i = 0; i < expectedlistDesc.Count(); i++)
                Assert.AreEqual(expectedlistDesc.ElementAt(i), resultlistDesc.ElementAt(i));
        }
        [TestMethod()]
        public void SortByIdNicknameTimestamp() 
        {              
            //arrange
            List<IMessage> listToCheck = new List<IMessage>();
            Message a = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "e", "1", "5/16/2018 10:29:02 AM", "hii", true));
            Message b = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "e", "1", "5/16/2018 10:30:45 AM", "hii", true));
            Message c = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "c", "1", "5/16/2018 11:29:02 AM", "hii", true));
            Message d = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "b", "2", "5/18/2018 10:29:02 AM", "hii", true));
            Message e = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "a", "2", "5/16/2018 10:29:02 AM", "hii", true));
            Message f = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "a", "2", "5/16/2017 10:29:02 AM", "hii", true));
            listToCheck.Add(a);
            listToCheck.Add(b);
            listToCheck.Add(c);
            listToCheck.Add(d);
            listToCheck.Add(e);
            listToCheck.Add(f);

            //ascending
            List<IMessage> resultlistAsc = new List<IMessage>();
            List<IMessage> expectedlistAsc = new List<IMessage>();
            expectedlistAsc.Add(c);
            expectedlistAsc.Add(a);
            expectedlistAsc.Add(b);
            expectedlistAsc.Add(f);
            expectedlistAsc.Add(e);
            expectedlistAsc.Add(d);

            //descending
            List<IMessage> resultlistDesc = new List<IMessage>();
            List<IMessage> expectedlistDesc = new List<IMessage>();
            expectedlistDesc.Add(d);
            expectedlistDesc.Add(e);
            expectedlistDesc.Add(f);
            expectedlistDesc.Add(b);
            expectedlistDesc.Add(a);
            expectedlistDesc.Add(c);

            //act
            resultlistAsc = ChatRoom.SortByIdNicknameTimestamp(listToCheck, true); 
            resultlistDesc = ChatRoom.SortByIdNicknameTimestamp(listToCheck, false);

            //assert
            for (int i = 0; i < expectedlistAsc.Count(); i++)
                Assert.AreEqual(expectedlistAsc.ElementAt(i), resultlistAsc.ElementAt(i));
            for (int i = 0; i < expectedlistDesc.Count(); i++)
                Assert.AreEqual(expectedlistDesc.ElementAt(i), resultlistDesc.ElementAt(i));
        }

        [TestMethod()]
        public void FilterByGroupIdTest()
        {
            //arrange
            List<IMessage> listToCheck = new List<IMessage>();
            Message a = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "e", "1", "5/16/2018 10:29:02 AM", "hii", true));
            Message b = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "f", "2", "5/16/2018 10:29:02 AM", "hii", true));
            Message c = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "c", "1", "5/16/2018 10:29:02 AM", "hii", true));
            Message d = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "b", "1", "5/16/2018 10:29:02 AM", "hii", true));
            Message e = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "a", "2", "5/16/2018 10:29:02 AM", "hii", true));
            Message f = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "z", "1", "5/16/2018 10:29:02 AM", "hii", true));
            listToCheck.Add(a);
            listToCheck.Add(b);
            listToCheck.Add(c);
            listToCheck.Add(d);
            listToCheck.Add(e);
            listToCheck.Add(f);
            List<IMessage> expectedlist = new List<IMessage>();
            expectedlist.Add(b);
            expectedlist.Add(e);
            //act
            List<IMessage> resultlist = new List<IMessage>();
            resultlist = ChatRoom.FilterByGroupId(listToCheck, "2"); // should contain only b and e messages
            //assert
            for (int i = 0; i < expectedlist.Count(); i++)
                Assert.AreEqual(expectedlist.ElementAt(i), resultlist.ElementAt(i));

            resultlist = ChatRoom.FilterByGroupId(listToCheck, "9999"); //should be an empty list because there is no message with this groupId 
            List<IMessage> empty_expected = new List<IMessage>();
            CollectionAssert.AreEqual(empty_expected, resultlist);
        }
        [TestMethod()]
        public void FilterByUserTest()
        {
            //arrange
            List<IMessage> listToCheck = new List<IMessage>();
            Message a = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "e", "1", "5/16/2018 10:29:02 AM", "hii", true));
            Message b = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "f", "2", "5/16/2018 10:29:02 AM", "hii", true));
            Message c = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "c", "1", "5/16/2018 10:29:02 AM", "hii", true));
            Message d = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "e", "1", "5/16/2018 10:29:02 AM", "hii", true));
            Message e = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "a", "2", "5/16/2018 10:29:02 AM", "hii", true));
            Message f = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "z", "1", "5/16/2018 10:29:02 AM", "hii", true));
            listToCheck.Add(a);
            listToCheck.Add(b);
            listToCheck.Add(c);
            listToCheck.Add(d);
            listToCheck.Add(e);
            listToCheck.Add(f);

            List<IMessage> expectedlist = new List<IMessage>();
            expectedlist.Add(a);
            expectedlist.Add(d);
            //act
            List<IMessage> resultlist = new List<IMessage>();
            resultlist = ChatRoom.FilterByUser(listToCheck, "1", "e");
            //assert
            for (int i = 0; i < expectedlist.Count(); i++)
                Assert.AreEqual(expectedlist.ElementAt(i), resultlist.ElementAt(i));

            resultlist = ChatRoom.FilterByUser(listToCheck, "9", "thf"); //should be an empty list because there is no message of this user
            List<IMessage> empty_expected = new List<IMessage>();
            CollectionAssert.AreEqual(empty_expected, resultlist);
        }

        [TestMethod()]
        public void ConvertToStringTest()
        {
            List<IMessage> listToCheck = new List<IMessage>();

           Message a = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "e", "1", "5/16/2018 10:29:02 AM", "hii", true));
           Message b = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "f", "2", "5/16/2018 10:29:02 AM", "hii", true));
           Message c = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "c", "1", "5/16/2018 10:29:02 AM", "hii", true));
            
           listToCheck.Add(a);
           listToCheck.Add(b);
           listToCheck.Add(c);
           List<string> resultlist = new List<string>();
           resultlist = ChatRoom.ConvertToString(listToCheck);
           
           List<string> expectedlist = new List<string>();
           string ao = "Group:1,Nickname:e (5/16/2018 1:29:02 PM): hii (Message GUID:8c1f2400-8fee-423d-a7e4-7fe06b775f52)";
           string bo = "Group:2,Nickname:f (5/16/2018 1:29:02 PM): hii (Message GUID:8c1f2400-8fee-423d-a7e4-7fe06b775f52)";
           string co = "Group:1,Nickname:c (5/16/2018 1:29:02 PM): hii (Message GUID:8c1f2400-8fee-423d-a7e4-7fe06b775f52)";
           expectedlist.Add(ao);
           expectedlist.Add(bo);
           expectedlist.Add(co);
        
           for (int i = 0; i < expectedlist.Count(); i++)
               Assert.AreEqual(expectedlist.ElementAt(i), resultlist.ElementAt(i));
        }
    }
}