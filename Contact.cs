using System.ComponentModel;

namespace ValidateDataChange
{
    internal class Contact : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; NotifyPropertyChanged("Name"); }
        }
        private string ip;
        public string Ip
        {
            get { return ip; }
            set { ip = value; NotifyPropertyChanged("Ip"); }
        }
        private string region;
        public string Region
        {
            get { return region; }
            set { region = value; NotifyPropertyChanged("Region"); }
        }

        public Contact(string name, string ip, string region)
        {
            this.name = name;
            this.ip = ip;
            this.region = region;
        }

        public override int GetHashCode()
        {
            return (name + ip + region).GetHashCode();
        }

    }
}