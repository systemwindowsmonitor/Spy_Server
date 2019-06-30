﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace BrowserHistory_Server
{
    /// <summary>
    /// Логика взаимодействия для Admin_Window.xaml
    /// </summary>
    public partial class Admin_Window : Window
    {
        public Admin_Window()
        {
            InitializeComponent();
        }

        class MyBrush
        {
            public string Name;
            public Brush Brush;
        }

     
        private void SuperClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            Work_Grid.Background = (Brush)button?.Tag;

        }
      
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Process progress = Process.GetCurrentProcess();
            progress.Kill();

        }

        private void EditUser_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            switch ((sender as ListView).Name)
            {
                case "AddAdmin":
                    new View.Add_Admin_Window().ShowDialog();
                    break;
                case "AddUser":
                    new Add_User_Window().ShowDialog();
                    break;
                case "DeleteUser":
                    new View.Window_Delete().ShowDialog();
                    break;
                case "EditUser":
                    new View.Window_Edit().ShowDialog();
                    break;

                default:
                    break;
            }
        }

    }
}
