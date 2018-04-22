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
            Copy.IsEnabled = false;
            DisplayIP();
        }

        private async void DisplayIP()
        {
            ConnectionController ctrlUpdater = new ConnectionController();
            var tarea = ctrlUpdater.GetExternalIP();
            if ("No Connection!" == await tarea)
            {
                IP.Text = await tarea;
            }
            else
            {
                IP.Text = await tarea;
                Copy.IsEnabled = true;
            }
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(IP.Text);
            MessageBox.Show("Copied!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
