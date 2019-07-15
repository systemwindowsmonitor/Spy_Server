using BrowserHistory_Server.Data;
using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media.Animation;

namespace BrowserHistory_Server
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class ClientObject
        {
            public TcpClient client;
            public ClientObject(TcpClient tcpClient)
            {
                client = tcpClient;
            }

            public void Process()
            {
                NetworkStream stream = null;
                try
                {
                    stream = client.GetStream();
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    while (true)
                    {
                        // получаем сообщение
                        StringBuilder builder = new StringBuilder();
                        int bytes = 0;
                        do
                        {
                            bytes = stream.Read(data, 0, data.Length);
                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        }
                        while (stream.DataAvailable);

                        string message = builder.ToString();

                        Console.WriteLine(message);
                        // отправляем обратно сообщение в верхнем регистре
                        message = message.Substring(message.IndexOf(':') + 1).Trim().ToUpper();
                        data = Encoding.Unicode.GetBytes(message);
                        stream.Write(data, 0, data.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                    if (client != null)
                        client.Close();
                }
            }
        }

        const int port = 8888;
        static TcpListener listener;



        string databaseName = (System.IO.Directory.GetCurrentDirectory() + "\\DB.db");
        Storyboard animation;
        DoubleAnimation a;
        public MainWindow()
        {
            InitializeComponent();

        }
        private void showError()
        {
            Dispatcher.BeginInvoke(new Action(() =>
           {
               a = new DoubleAnimation();
               a.From = 0;
               a.To = 1;
               a.Duration = TimeSpan.FromSeconds(3);
               Storyboard.SetTarget(a, Error_Label);
               Storyboard.SetTargetProperty(a, new PropertyPath(UIElement.OpacityProperty));
               animation = new Storyboard();
               animation.Children.Add(a);
               animation.Completed += hideError;
               animation.Begin();

           }));
        }


        private void hideError(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
           {
               a = new DoubleAnimation();
               a.From = 1;
               a.To = 0;
               a.Duration = TimeSpan.FromSeconds(3);
               Storyboard.SetTarget(a, Error_Label);
               Storyboard.SetTargetProperty(a, new PropertyPath(UIElement.OpacityProperty));
               animation = new Storyboard();
               animation.Children.Add(a);
               animation.Begin();
           }));
        }


        // Открывает рабочее окно
        private async void Sign_in_Button_Input(object sender, RoutedEventArgs e)
        {
            DbManager db = new DbManager(databaseName);
            db.Connect();
            //MessageBox.Show(db.CheckLogin(Login_TextBox.Text).ToString());
            //MessageBox.Show(db.CheckPassword(Password_PasswordBox.Password).ToString());
            foreach (var item in db.GetPasswords())
            {
                MessageBox.Show(item);
            }
        }
        Thread t;
        TcpClient client;
        bool isActive = true;
        ThreadStart ts;
        //Закрытие окна входа
        private void Closed_Button_Input(object sender, RoutedEventArgs e)
        {
            
            if(client != null)client.Dispose();

            //t.Abort();
            //this.Close();

            Process progress = Process.GetCurrentProcess();
            progress.Kill();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
             ts = new ThreadStart(() =>
            {
                try
                {
                    listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                    listener.Start();


                    while (true)
                    {
                        client = listener.AcceptTcpClient();
                        ClientObject clientObject = new ClientObject(client);

                        // создаем новый поток для обслуживания нового клиента
                        Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                        clientThread.Start();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (listener != null)
                        listener.Stop();
                }
            });
            
            t = new Thread(ts);
            t.Start();
            
        }
    }
}
