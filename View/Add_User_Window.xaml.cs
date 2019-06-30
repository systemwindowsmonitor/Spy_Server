using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ValidateDataChange;

namespace BrowserHistory_Server
{
    /// <summary>
    /// Логика взаимодействия для Add_User_Window.xaml
    /// </summary>
    public partial class Add_User_Window : Window, IDisposable
    {
        int hash;
        bool discardChanges;

        public Add_User_Window()
        {
            InitializeComponent();
            
            discardChanges = false;
            //var contact = new Contact("Vlad", "IP", "region");
            //hash = contact.GetHashCode();

            //this.DataContext = contact;
        }

        private void Closed_Click(object sender, RoutedEventArgs e)
        {
            //if (this.DataContext.GetHashCode() != hash && !discardChanges)
            //{
                
            //}
            SnackbarUnsavedChanges.IsActive = true;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Save Data
        }

        private void SnackbarMessage_ActionClick(object sender, RoutedEventArgs e)
        {
            SnackbarUnsavedChanges.IsActive = false;
            discardChanges = true;
            this.Close();
        }

        public void Dispose()
        {
            //GC.Collect();
        }
    }
}
