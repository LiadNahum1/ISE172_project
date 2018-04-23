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


namespace ChatRoomProject
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private ChatRoom chat;
        private const int NUM_GROUPS = 33;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("ChatRoom.cs");

        public Login(ChatRoom chat)
        {
            InitializeComponent();
            this.chat = chat;
            for (int i = 1; i < NUM_GROUPS; i = i + 1)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = i.ToString();
                groups.Items.Add(comboBoxItem);
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.chat.Login(groups.SelectedValue.ToString().Split(' ')[1], nicknameContent.Text);
                log.Info("The user logged in");
                MessageBox.Show("Hi " + nicknameContent.Text);
                ChatRoomW chatRoom = new ChatRoomW(this.chat);
                chatRoom.Show();
                this.Close();
            }
            catch (Exception err)
            {
                log.Info("The user faild to log in");
                MessageBox.Show(err.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow(this.chat);
            main.Show();
            this.Close();

        }

        private void NicknameContent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Login_Click(sender, e);
        }
    }
}
