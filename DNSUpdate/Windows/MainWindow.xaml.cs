using DNSUpdate.Class;
using DNSUpdate.Controller;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace DNSUpdate.Windows
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer Timer;
        RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public MainWindow()
        {
            InitializeComponent();
            LoggerController.SetupLog();
            if (SettingsController.CreateSettings())
                LoggerController.LogEvent("Settings DB setup ok");
            if (SettingsController.GetSettings().Domain != "" && SettingsController.GetSettings().Token != "" && SettingsController.GetSettings().Interval != 0)
                Hide();
            PopulateToolTip();
            if (rk.GetValue("DNSUpdate") == null)
                OnStartup.IsChecked = false;
            else
                OnStartup.IsChecked = true;
            PopulateFields();
            if (StartUpdater())
                ToggleUpdater.Content = "Stop updater";
        }

        private void PopulateToolTip()
        {
            string tipInt = "Not setted";
            if (SettingsController.GetSettings().Interval != 0)
                tipInt = SettingsController.GetSettings().Interval + " m";
            ToolTipInfo.Text = "DNSUpdate\n" + "Update interval: " + tipInt;
        }

        private bool StartUpdater()
        {
            Settings settings = SettingsController.GetSettings();
            if (settings.Domain != "" && settings.Token != "" && settings.Interval != 0)
            {
                Timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMinutes(settings.Interval)
                };
                Timer.Tick += Timer_Tick;
                Timer.Start();
                LoggerController.LogEvent("Updater started");
                PopulateToolTip();
                if (!ConnectionController.Update(settings.Domain, settings.Token))
                    LoggerController.LogEvent("No connection or invalid input data when trying to update");
                return true;
            }
            else
            {
                LoggerController.LogEvent("Can't start updater, invalid input");
                return false;
            }
        }

        private void StopUpdater()
        {
            if (Timer != null) { LoggerController.LogEvent("Updater stopped"); Timer.Stop(); }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Settings settings = SettingsController.GetSettings();
            if (!ConnectionController.Update(settings.Domain, settings.Token))
                LoggerController.LogEvent("No connection when trying to update");
            else
                LoggerController.LogEvent("Automatic update");
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

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if ((Domain.Text != "") && (Token.Text != "") && (Interval.Text != ""))
            {
                if (SettingsController.SetSettings(Domain.Text, Token.Text, byte.Parse(Interval.Text)) && ConnectionController.Update(Domain.Text, Token.Text))
                {
                    LoggerController.LogEvent("Manual update");
                    MessageBox.Show("Update succesful");
                }
                else
                {
                    MessageBox.Show("Check input data or connection", "Error");
                }
            }
            else
            {
                MessageBox.Show("Check input data or connection", "Error");
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsController.ResetSettings())
            {
                PopulateFields();
                LoggerController.LogEvent("Reset updater settings");
                StopUpdater();
                LoggerController.LogEvent("Updater stopped");
                MessageBox.Show("Reset succesful, updater stopped");
            }
            else
            {
                MessageBox.Show("Reset failed", "Error");
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void UpdateNow_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = SettingsController.GetSettings();
            if (ConnectionController.Update(settings.Domain, settings.Token))
                LoggerController.LogEvent("Manual update");
            else
                LoggerController.LogEvent("Manual update failed");
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

        private void OnStartup_Click(object sender, RoutedEventArgs e)
        {
            if (OnStartup.IsChecked == true)
                rk.SetValue("DNSUpdate", System.IO.Path.GetFullPath(System.Reflection.Assembly.GetExecutingAssembly().Location));
            else
                rk.DeleteValue("DNSUpdate", false);
        }

        private void ToggleUpdater_Click(object sender, RoutedEventArgs e)
        {
            if (Timer.IsEnabled)
            {
                StopUpdater();
                ToggleUpdater.Content = "Start updater";
            }
            else
            {
                if (StartUpdater())
                    ToggleUpdater.Content = "Stop updater";
            }
        }
    }
}
