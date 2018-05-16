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
        public void SortByNicknameAscTest() // Ascending
        {
            List<IMessage> listToCheck = new List<IMessage>();

            Message a = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "e", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:02 AM", "hii", true));
            Message b = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "f", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:02 AM", "hii", true));
            Message c = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "c", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:02 AM", "hii", true));
            Message d = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "b", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:02 AM", "hii", true));
            Message e = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "a", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:02 AM", "hii", true));
            Message f = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "z", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:02 AM", "hii", true));
            listToCheck.Add(a);
            listToCheck.Add(b);
            listToCheck.Add(c);
            listToCheck.Add(d);
            listToCheck.Add(e);
            listToCheck.Add(f);
            List<IMessage> resultlist = new List<IMessage>();
            resultlist = ChatRoom.SortByNickname(listToCheck, true); //Ascending sort of nicknames

            List<IMessage> expectedlist = new List<IMessage>();
            expectedlist.Add(e);
            expectedlist.Add(d);
            expectedlist.Add(c);
            expectedlist.Add(a);
            expectedlist.Add(b);
            expectedlist.Add(f);
            for (int i = 0; i < expectedlist.Count(); i++)
                Assert.AreEqual(expectedlist.ElementAt(i), resultlist.ElementAt(i));

            Assert.AreNotEqual(ChatRoom.SortByNickname(new List<IMessage>(), true),resultlist);
        }

        [TestMethod()]
        public void SortByTimeStampAscTest() // Ascending
        {
            List<IMessage> listToCheck = new List<IMessage>();

            Message a = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "e", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:04 AM", "hii", true));//a
            Message b = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "f", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:02 AM", "hii", true));//b
            Message c = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "c", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:07 AM", "hii", true));//c
            Message d = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "b", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:03 AM", "hii", true));//d
            Message e = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "a", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:01 AM", "hii", true));//e
            Message f = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "z", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:08 AM", "hii", true));//f  
            listToCheck.Add(a);
            listToCheck.Add(b);
            listToCheck.Add(c);
            listToCheck.Add(d);
            listToCheck.Add(e);
            listToCheck.Add(f);
            List<IMessage> resultlist = new List<IMessage>();
            resultlist = ChatRoom.SortTimestamp(listToCheck, true); //Ascending

            List<IMessage> expectedlist = new List<IMessage>();
            expectedlist.Add(e); 
            expectedlist.Add(b);
            expectedlist.Add(d);
            expectedlist.Add(a);
            expectedlist.Add(c);
            expectedlist.Add(f);
            for (int i = 0; i < expectedlist.Count(); i++)
                Assert.AreEqual(expectedlist.ElementAt(i), resultlist.ElementAt(i));



        }
        [TestMethod()]
        public void SortByTimeStampDscTest() // Ascending
        {
            List<IMessage> listToCheck = new List<IMessage>();

            Message a = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "e", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:04 AM", "hii", true));//a
            Message b = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "f", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:02 AM", "hii", true));//b
            Message c = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "c", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:07 AM", "hii", true));//c
            Message d = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "b", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:03 AM", "hii", true));//d
            Message e = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "a", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:01 AM", "hii", true));//e
            Message f = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "z", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:08 AM", "hii", true));//f  
            listToCheck.Add(a);
            listToCheck.Add(b);
            listToCheck.Add(c);
            listToCheck.Add(d);
            listToCheck.Add(e);
            listToCheck.Add(f);
            List<IMessage> resultlist = new List<IMessage>();
            resultlist = ChatRoom.SortTimestamp(listToCheck, false); //Descending sort of timeStamp

            List<IMessage> expectedlist = new List<IMessage>();
            expectedlist.Add(f);
            expectedlist.Add(c);
            expectedlist.Add(a);
            expectedlist.Add(d);
            expectedlist.Add(b);
            expectedlist.Add(e);
            for (int i = 0; i < expectedlist.Count(); i++)
                Assert.AreEqual(expectedlist.ElementAt(i), resultlist.ElementAt(i));

              
        }




        [TestMethod()]
        public void SortByNickNameTestDes()
        {
            List<IMessage> listToCheck = new List<IMessage>();

            Message a = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "e", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:02 AM", "hii", true));
            Message b = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "f", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:02 AM", "hii", true));
            Message c = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "c", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:02 AM", "hii", true));
            Message d = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "b", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:02 AM", "hii", true));
            Message e = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "a", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:02 AM", "hii", true));
            Message f = (new Message("8c1f2400-8fee-423d-a7e4-7fe06b775f52", "z", "8c1f2400-8fee-423d-a7e4-7fe06b775f52", "5/16/2018 10:29:02 AM", "hii", true));
            listToCheck.Add(a);
            listToCheck.Add(b);
            listToCheck.Add(c);
            listToCheck.Add(d);
            listToCheck.Add(e);
            listToCheck.Add(f);
            List<IMessage> resultlist = new List<IMessage>();
            resultlist = ChatRoom.SortByNickname(listToCheck, false); //Descending Sort of nicknames

            List<IMessage> expectedlist = new List<IMessage>();
            expectedlist.Add(f);
            expectedlist.Add(b);
            expectedlist.Add(a);
            expectedlist.Add(c);
            expectedlist.Add(d);
            expectedlist.Add(e);

            for (int i = 0; i < expectedlist.Count(); i++)
                Assert.AreEqual(expectedlist.ElementAt(i), resultlist.ElementAt(i));
        }

        [TestMethod()]
        public void FilterByGroupIdTest()
        {
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
            List<IMessage> resultlist = new List<IMessage>();
            resultlist = ChatRoom.FilterByGroupId(listToCheck, "2"); // should contain only b and e messages
            List<IMessage> expectedlist = new List<IMessage>();
            expectedlist.Add(b);
            expectedlist.Add(e);
            for (int i = 0; i < expectedlist.Count(); i++)
                Assert.AreEqual(expectedlist.ElementAt(i), resultlist.ElementAt(i));
            resultlist = ChatRoom.FilterByGroupId(listToCheck, "9999"); //should be an empty list because there is no message with this groupId 
            List<IMessage> empty_expected = new List<IMessage>();
            CollectionAssert.AreEqual(empty_expected, resultlist);
        }
        [TestMethod()]
        public void FilterByUserTest()
        {
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
            List<IMessage> resultlist = new List<IMessage>();
            resultlist = ChatRoom.FilterByUser(listToCheck, "1", "e");
            List<IMessage> expectedlist = new List<IMessage>();
            expectedlist.Add(a);
            expectedlist.Add(d);
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
            Console.WriteLine(resultlist);
           
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