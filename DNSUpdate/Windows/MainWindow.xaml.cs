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
            }
            PopulateToolTip(settings.Interval);
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

        private void EnableEdition()
        {
            Domain.IsEnabled = true;
            Token.IsEnabled = true;
            Interval.IsEnabled = true;
            Update.Content = "Cancel";
            Edit.Visibility = Visibility.Collapsed;
            Wipe.Visibility = Visibility.Visible;
        }

        private void DisableEdition()
        {
            Domain.IsEnabled = false;
            Token.IsEnabled = false;
            Interval.IsEnabled = false;
            Update.Content = "Update now";
            Edit.Visibility = Visibility.Visible;
            Wipe.Visibility = Visibility.Collapsed;
        }

        private async void UpdateNow_Click(object sender, RoutedEventArgs e)
        {
            if (Update.Content.ToString() == "Update now")
            {
                if (await UpdaterController.UpdateNow(Domain.Text, Token.Text))
                    MessageBox.Show("Update succesful");
                else
                    MessageBox.Show("Update unsuccesful");
            }
            else
            {
                Settings settings = SettingsController.GetSettings();
                PopulateFields(settings.Domain, settings.Token, settings.Interval);
                DisableEdition();
                if (UpdaterController.IsRunning())
                    ToggleUpdater.Content = "Stop";
                else
                    ToggleUpdater.Content = "Start";
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            ToggleUpdater.Content = "Save";
            EnableEdition();
        }

        private void Wipe_Click(object sender, RoutedEventArgs e)
        {
            if (true)
            {
                if (SettingsController.ResetSettings())
                {
                    PopulateFields("", "", 0);
                    LoggerController.LogEvent("Wiped updater settings");
                    UpdaterController.StopUpdater();
                    ToggleUpdater.Content = "Save and start";
                    LoggerController.LogEvent("Updater stopped");
                    MessageBox.Show("Wipe succesful, updater stopped");
                    DisableEdition();
                }
                else
                {
                    MessageBox.Show("Wipe failed", "Error");
                } 
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

        private async void ToggleUpdater_Click(object sender, RoutedEventArgs e)
        {
            if (UpdaterController.IsRunning() && ToggleUpdater.Content.ToString() != "Save")
            {
                UpdaterController.StopUpdater();
                ToggleUpdater.Content = "Start";
            }
            else if (Interval.Text != "" && Domain.Text != "" && Token.Text != "" && SettingsController.SetSettings(Domain.Text, Token.Text, byte.Parse(Interval.Text)) && await UpdaterController.StartUpdater())
            {
                ToggleUpdater.Content = "Stop";
                PopulateToolTip(byte.Parse(Interval.Text));
                DisableEdition();
            }
        }

        private void TaskbarIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Show();
        }
    }
}
