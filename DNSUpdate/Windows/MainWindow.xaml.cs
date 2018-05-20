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
            if (SettingsController.GetSettings().Domain != "" && SettingsController.GetSettings().Token != "" && SettingsController.GetSettings().Interval != 0)
            {
                Hide();
                PopulateFields();
                DisableEdition();
            }
            PopulateToolTip();
            if (SettingsController.IsSettedOnStartup())
                OnStartup.IsChecked = true;
            else
                OnStartup.IsChecked = false;
            if (UpdaterController.StartUpdater())
                ToggleUpdater.Content = "Stop";
        }

        private void PopulateToolTip()
        {
            string tipInt = "Not setted";
            if (SettingsController.GetSettings().Interval != 0)
                tipInt = SettingsController.GetSettings().Interval + " m";
            ToolTipInfo.Text = "DNSUpdate\n" + "Update interval: " + tipInt;
        }

        private void PopulateFields()
        {
            Settings settings = SettingsController.GetSettings();
            Domain.Text = settings.Domain;
            Token.Text = settings.Token;
            if (settings.Interval != 0)
                Interval.Text = settings.Interval.ToString();
            else
                Interval.Text = "";
        }

        private void EnableEdition()
        {
            Domain.IsEnabled = true;
            Token.IsEnabled = true;
            Interval.IsEnabled = true;
            Edit.Content = "Wipe settings";
            Update.Content = "Cancel";
        }

        private void DisableEdition()
        {
            Domain.IsEnabled = false;
            Token.IsEnabled = false;
            Interval.IsEnabled = false;
            Edit.Content = "Edit settings";
            Update.Content = "Update now";
        }

        private void UpdateNow_Click(object sender, RoutedEventArgs e)
        {
            if (Update.Content.ToString() == "Update now")
            {
                if (UpdaterController.UpdateNow(Domain.Text, Token.Text))
                    MessageBox.Show("Update succesful");
                else
                    MessageBox.Show("Update unsuccesful");
            }
            else
            {
                PopulateFields();
                DisableEdition();
                if (UpdaterController.StartUpdater())
                    ToggleUpdater.Content = "Stop";
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (Edit.Content.ToString() == "Edit settings")
            {
                UpdaterController.StopUpdater();
                ToggleUpdater.Content = "Save and start";
                EnableEdition();
            }
            else if (SettingsController.ResetSettings())
            {
                PopulateFields();
                LoggerController.LogEvent("Wiped updater settings");
                UpdaterController.StopUpdater();
                ToggleUpdater.Content = "Save and start";
                LoggerController.LogEvent("Updater stopped");
                MessageBox.Show("Wipe succesful, updater stopped");
            }
            else
            {
                MessageBox.Show("Wipe failed", "Error");
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

        private void ToggleUpdater_Click(object sender, RoutedEventArgs e)
        {
            if (UpdaterController.IsRunning())
            {
                UpdaterController.StopUpdater();
                ToggleUpdater.Content = "Start";
            }
            else if (Interval.Text != "" && Domain.Text != "" && Token.Text != "" && SettingsController.SetSettings(Domain.Text, Token.Text, byte.Parse(Interval.Text)) && UpdaterController.StartUpdater())
            {
                ToggleUpdater.Content = "Stop";
                PopulateToolTip();
                DisableEdition();
            }
        }

        private void TaskbarIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Show();
        }
    }
}
