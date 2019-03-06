using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Demo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ParmasLoad();
        }

        private void ParmasLoad()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            XSpeed.Text = localSettings.Values["XSpeed"].ToString();
            YSpeed.Text = localSettings.Values["YSpeed"].ToString();
            GateTimeCmb.SelectedIndex = Convert.ToInt32(localSettings.Values["GateTime"].ToString());
            ThresholdCmb.SelectedIndex = Convert.ToInt32(localSettings.Values["Threshold"].ToString());
        }

        private void ParmasSave()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["XSpeed"] = XSpeed.Text;
            localSettings.Values["YSpeed"] = YSpeed.Text;
            localSettings.Values["GateTime"] = GateTimeCmb.SelectedIndex.ToString();
            localSettings.Values["Threshold"] = ThresholdCmb.SelectedIndex.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ParmasSave();
            SaveFlyout.Show("保存成功！", 2000);
        }
    }
}
