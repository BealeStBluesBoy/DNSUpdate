using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Threading;

namespace DNSUpdate
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer Timer;
        SettingsController ctrlSettings = new SettingsController();
        LoggerController logger = new LoggerController();
        ConnectionController ctrlConnection = new ConnectionController();
        RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public MainWindow()
        {
            InitializeComponent();
            logger.SetupLog();
            if (ctrlSettings.CreateSettings()) { logger.LogEvent("Settings DB setup ok"); }
            if (ctrlSettings.GetSettings().Domain != "" || ctrlSettings.GetSettings().Token != "" || ctrlSettings.GetSettings().Interval != 0)
            {
                Hide();
            }
            PopulateToolTip();
            if (rk.GetValue("DNSUpdate") == null)
            {
                OnStartup.IsChecked = false;
            }
            else
            {
                OnStartup.IsChecked = true;
            }
            PopulateFields();
            if (StartUpdater())
            {
                ToggleUpdater.Content = "Stop updater";
            }
        }

        private void PopulateToolTip()
        {
            string tipInt = "Not setted";
            if (ctrlSettings.GetSettings().Interval != 0) { tipInt = ctrlSettings.GetSettings().Interval + " m"; }
            ToolTipInfo.Text = "DNSUpdate\n" + "Update interval: " + tipInt;
        }

        private bool StartUpdater()
        {
            Settings settings = ctrlSettings.GetSettings();
            if (settings.Domain != "" && settings.Token != "" && settings.Interval != 0)
            {
                Timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMinutes(settings.Interval)
                };
                Timer.Tick += Timer_Tick;
                Timer.Start();
                logger.LogEvent("Updater started");
                PopulateToolTip();
                if (!ctrlConnection.Update(settings.Domain, settings.Token))
                {
                    logger.LogEvent("No connection or invalid input data when trying to update");
                }
                return true;
            }
            else
            {
                logger.LogEvent("Can't start updater, invalid input");
                return false;
            }
        }

        private void StopUpdater()
        {
            if (Timer != null) { logger.LogEvent("Updater stopped"); Timer.Stop(); }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Settings settings = ctrlSettings.GetSettings();
            if (!ctrlConnection.Update(settings.Domain, settings.Token))
            {
                logger.LogEvent("No connection when trying to update");
            }
            else
            {
                logger.LogEvent("Automatic update");
            }
        }

        private void PopulateFields()
        {
            Settings settings = ctrlSettings.GetSettings();
            Domain.Text = settings.Domain;
            Token.Text = settings.Token;
            if (settings.Interval != 0)
            {
                Interval.Text = settings.Interval.ToString();
            }
            else
            {
                Interval.Text = "";
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if ((Domain.Text != "") && (Token.Text != "") && (Interval.Text != ""))
            {
                if (ctrlSettings.SetSettings(Domain.Text, Token.Text, byte.Parse(Interval.Text)) && ctrlConnection.Update(Domain.Text, Token.Text))
                {
                    logger.LogEvent("Manual update");
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
            if (ctrlSettings.ResetSettings())
            {
                PopulateFields();
                logger.LogEvent("Reset updater settings");
                StopUpdater();
                logger.LogEvent("Updater stopped");
                MessageBox.Show("Reset succesful, updater stopped");
            }
            else
            {
                MessageBox.Show("Reset failed", "Error");
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized) Hide();
            base.OnStateChanged(e);
        }

        private void UpdateNow_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = ctrlSettings.GetSettings();
            if (ctrlConnection.Update(settings.Domain, settings.Token))
            {
                logger.LogEvent("Manual update");
            }
            else
            {
                logger.LogEvent("Manual update failed");
            }
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
            {
                rk.SetValue("DNSUpdate", System.IO.Path.GetFullPath(System.Reflection.Assembly.GetExecutingAssembly().Location));
            }
            else
            {
                rk.DeleteValue("DNSUpdate", false);
            }
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
                {
                    ToggleUpdater.Content = "Stop updater";
                }
            }
        }
    }
}
