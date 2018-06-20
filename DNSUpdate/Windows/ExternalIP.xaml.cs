using DNSUpdate.Controller;
using System.Windows;

namespace DNSUpdate.Windows
{
    /// <summary>
    /// Lógica de interacción para ExternalIP.xaml
    /// </summary>
    public partial class ExternalIP : Window
    {
        public ExternalIP()
        {
            InitializeComponent();
            Copy.IsEnabled = false;
            DisplayIP();
        }

        private async void DisplayIP()
        {
            IP.Text = await ConnectionController.GetExternalIP();
            if ("No Connection!" != IP.Text)
                Copy.IsEnabled = true;            
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(IP.Text);
            MessageBox.Show("Copied!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        private void Retry_Click(object sender, RoutedEventArgs e)
        {
            IP.Text = "Waiting connection...";
            DisplayIP();
        }
    }
}
