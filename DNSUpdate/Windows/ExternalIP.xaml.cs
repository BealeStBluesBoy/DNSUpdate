using System;
using System.Windows;

namespace DNSUpdate
{
    /// <summary>
    /// Lógica de interacción para ExternalIP.xaml
    /// </summary>
    public partial class ExternalIP : Window
    {
        public ExternalIP()
        {
            InitializeComponent();
            DisplayIP();
        }

        private async void DisplayIP()
        {
            ConnectionController ctrlUpdater = new ConnectionController();
            IP.Text = await ctrlUpdater.GetExternalIP();
            if (IP.Text == "No connection!")
            {
                Copy.IsEnabled = false;
            }
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(IP.Text);
            MessageBox.Show("Copied!");
            Close();
        }
    }
}
