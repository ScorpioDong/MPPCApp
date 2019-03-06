using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
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
    public sealed partial class HistoryPage : Page
    {
        public HistoryPage()
        {
            this.InitializeComponent();
            SerialReflush();
        }

        private void ParamsLoad()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["isContinue"] = "true";
            localSettings.Values["ExcelName"] = HistoryList.SelectedItem.ToString();
            localSettings.Values["PortName"] = Port.SelectedItem.ToString();
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            if (HistoryList.SelectedItem == null)
            {
                NotSeleted.Show("未选中任何项目，请选中后再试！", 2000);
                return;
            }

            if (Port.SelectedItem == null)
            {
                NotSeleted.Show("未找到串口，请选中或刷新后尝试！", 2000);
                return;
            }

            if (!(Window.Current.Content is Frame RootFrame))
                return;

            ParamsLoad();
            RootFrame.Navigate(typeof(Run));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (HistoryList.SelectedItem == null)
            {
                NotSeleted.Show("未选中任何项目，请选中后再试！", 2000);
                return;
            }

            HistoryList.Items.Remove(HistoryList.SelectedItem);
        }

        private void PortReflush_Click(object sender, RoutedEventArgs e)
        {
            SerialReflush();
        }
        private async void SerialReflush()
        {
            Port.Items.Clear();

            string filter = SerialDevice.GetDeviceSelector();
            var dis = await DeviceInformation.FindAllAsync(filter);

            foreach (var id in dis)
                Port.Items.Add(id.Name.ToString());

            if (Port.Items.Count != 0)
                Port.SelectedIndex = 0;

        }
    }
}
