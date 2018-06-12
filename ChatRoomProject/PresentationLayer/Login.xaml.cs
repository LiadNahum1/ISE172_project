﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ChatRoomProject.LogicLayer;


namespace ChatRoomProject.PresentationLayer
{
    /// <summary>
    /// Login Window - this class will ask the client  for the login details,
    /// and send them for the logic layer for validation 
    /// </summary>
    public partial class Login : Window
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Login.cs");
        private ChatRoom chat;
        private string password;
        ObservableObjectChatRoom _main = new ObservableObjectChatRoom();

        public Login(ChatRoom chat)
        {
            InitializeComponent();
            this.chat = chat;
            this.password = "";
            this.DataContext = _main;
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)

        {
            PasswordBox pb = sender as PasswordBox;
            this.password = pb.Password;
        }

            //Call to Login function in ChatRoom. If there are no problems, open the ChatRoom window 
            private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.chat.Login(_main.GroupId, _main.Nickname, this.password);
                log.Info("The user " + _main.GroupId + ":" + _main.Nickname + " logged in");
                ChatRoomW chatRoom = new ChatRoomW(this.chat);
                chatRoom.Show();
                this.Close();
            }
            catch (Exception err)
            {
                log.Info("Login failed. The user isn't registered");
                log.Info(err.ToString());
                MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Open the main window and close this one
        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            log.Info("The user returned to menu");
            MainWindow main = new MainWindow(this.chat);
            main.Show();
            this.Close();

        }
    }
}
