using System.Windows;

namespace DNSUpdate.Windows
{
    /// <summary>
    /// Lógica de interacción para About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
