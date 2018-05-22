using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChatRoomProject.LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomProject.LogicLayer.Tests
{
    [TestClass()]
    public class MessageTests
    {
        [TestMethod()]
        public void CheckValidityTest()
        {
            //arrange
            string messageContent;
            bool isValidity;
            bool expected;
            //act
            //check empty message - false
            messageContent = "";
            expected = false;
            isValidity = Message.CheckValidity(messageContent);
            //assert
            Assert.AreEqual(expected, isValidity);

            //check message with 4 characters - true
            messageContent = messageContent + "test";
            expected = true;
            isValidity = Message.CheckValidity(messageContent);
            Assert.AreEqual(expected, isValidity);

            //check message with 150 characters - true
            messageContent = "";
            for (int i = 0; i < 150; i = i + 1)
            {
                messageContent = messageContent + "t";
            }
            expected = true;
            isValidity = Message.CheckValidity(messageContent);
            Assert.AreEqual(expected, isValidity);

            //check message with more than 150 characters - false
            messageContent = messageContent + "m";
            expected = false;
            isValidity = Message.CheckValidity(messageContent);
            Assert.AreEqual(expected, isValidity);

        }
    }
}