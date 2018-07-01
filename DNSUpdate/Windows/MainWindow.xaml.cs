using DNSUpdate.Class;
using DNSUpdate.Controller;
using System;
using System.ComponentModel;
using System.Windows;

namespace DNSUpdate.Windows
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetupWindow();
        }

        private async void SetupWindow()
        {
            Settings settings = SettingsController.GetSettings();
            if (settings.Domain != "" && settings.Token != "" && settings.Interval != 0)
            {
                Hide();
                PopulateFields(settings.Domain, settings.Token, settings.Interval);
                DisableEdition();
                PopulateToolTip(settings.Interval);
            }
            if (SettingsController.IsSettedOnStartup())
                OnStartup.IsChecked = true;
            else
                OnStartup.IsChecked = false;
            if (await UpdaterController.StartUpdater())
                ToggleUpdater.Content = "Stop";
        }

        private void PopulateToolTip(byte interval)
        {
            string tipInt = "Not setted";
            if (interval != 0)
                tipInt = interval + " m";
            ToolTipInfo.Text = "DNSUpdate\n" + "Update interval: " + tipInt;
        }

        private void PopulateFields(string domain, string token, byte interval)
        {
            Domain.Text = domain;
            Token.Text = token;
            Interval.Text = "";
            if (interval != 0)
                Interval.Text = interval.ToString();
        }

        private void EnableEdition(bool initialSetup)
        {
            Domain.IsEnabled = true;
            Token.IsEnabled = true;
            Interval.IsEnabled = true;
            Save.Visibility = Visibility.Visible;
            Update.Visibility = Visibility.Collapsed;
            ToggleUpdater.Visibility = Visibility.Collapsed;
            Edit.Visibility = Visibility.Collapsed;
            if (initialSetup)
            {
                Wipe.Visibility = Visibility.Collapsed;
                Cancel.Visibility = Visibility.Collapsed;
            }
            else
            {
                Wipe.Visibility = Visibility.Visible;
                Cancel.Visibility = Visibility.Visible;
            }
        }

        private void DisableEdition()
        {
            Domain.IsEnabled = false;
            Token.IsEnabled = false;
            Interval.IsEnabled = false;

            Wipe.Visibility = Visibility.Collapsed;
            Save.Visibility = Visibility.Collapsed;
            Cancel.Visibility = Visibility.Collapsed;

            Update.Visibility = Visibility.Visible;
            ToggleUpdater.Visibility = Visibility.Visible;
            Edit.Visibility = Visibility.Visible;
        }

        private async void UpdateNow_Click(object sender, RoutedEventArgs e)
        {
            if (await UpdaterController.UpdateNow(Domain.Text, Token.Text))
                MessageBox.Show("Update succesful");
            else
                MessageBox.Show("Update unsuccesful");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = SettingsController.GetSettings();
            PopulateFields(settings.Domain, settings.Token, settings.Interval);
            DisableEdition();
            if (UpdaterController.IsRunning())
                ToggleUpdater.Content = "Stop";
            else
                ToggleUpdater.Content = "Start";
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            EnableEdition(false);
        }

        private void Wipe_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Wipe settings",
                MessageBoxButton.YesNo, MessageBoxImage.Question,
                MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                if (SettingsController.ResetSettings())
                {
                    EnableEdition(true);
                    PopulateFields("", "", 0);
                    LoggerController.LogEvent("Wiped updater settings");
                    UpdaterController.StopUpdater();
                    LoggerController.LogEvent("Updater stopped");
                    MessageBox.Show("Wipe succesful, updater stopped");
                }
                else
                {
                    MessageBox.Show("Wipe failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                } 
            }
        }

        private async void ToggleUpdater_Click(object sender, RoutedEventArgs e)
        {
            if (UpdaterController.IsRunning())
            {
                UpdaterController.StopUpdater();
                ToggleUpdater.Content = "Start";
            }
            else
            {
                await UpdaterController.StartUpdater();
                ToggleUpdater.Content = "Stop";
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Interval.Text != "" && Domain.Text != "" && Token.Text != "" &&
                SettingsController.SetSettings(Domain.Text, Token.Text, byte.Parse(Interval.Text)) &&
                await UpdaterController.StartUpdater())
            {
                ToggleUpdater.Content = "Stop";
                PopulateToolTip(byte.Parse(Interval.Text));
                DisableEdition();
            }
            else
            {
                MessageBox.Show("Complete all fields with valid data!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                SettingsController.ResetSettings();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            Show();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void ShowIP_Click(object sender, RoutedEventArgs e)
        {
            ExternalIP ShowIP = new ExternalIP();
            ShowIP.Show();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            About AboutInfo = new About();
            AboutInfo.Show();
        }

        private void OnStartup_Checked(object sender, RoutedEventArgs e)
        {
            SettingsController.SetOnStartup();
        }

        private void OnStartup_Unchecked(object sender, RoutedEventArgs e)
        {
            SettingsController.UnsetOnStartup();
        }

        private void TaskbarIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Show();
        }
    }
}
