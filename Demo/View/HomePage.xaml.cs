using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using System.Diagnostics;
using Windows.Storage;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Demo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SerialReflush();
            ParmasLoad();
        }

        private void ParmasLoad()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            PicXLen.Text = localSettings.Values["PicXLen"].ToString();
            PicYLen.Text = localSettings.Values["PicYLen"].ToString();
            PicXCount.Text = localSettings.Values["PicXCount"].ToString();
            PicYCount.Text = localSettings.Values["PicYCount"].ToString();
        }

        private void ParmasSave()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["PicXLen"] = PicXLen.Text;
            localSettings.Values["PicYLen"] = PicYLen.Text;
            localSettings.Values["PicXCount"] = PicXCount.Text;
            localSettings.Values["PicYCount"] = PicYCount.Text;
            localSettings.Values["isContinue"] = "false";
            localSettings.Values["ExcelName"] = "";
            localSettings.Values["RowN"] = "0";
            localSettings.Values["ColN"] = "0";
            localSettings.Values["PortName"] = Port.SelectedItem.ToString();
        }

        private void ToRun_Click(object sender, RoutedEventArgs e)
        {
            ParmasSave();
            if (!(Port.SelectedItem == null))
            {
                if (!(Window.Current.Content is Frame RootFrame))
                    return;

                RootFrame.Navigate(typeof(Run));
            }
            else
            {
                PortFailed.Show("未找到串口，请选中或刷新后尝试！", 2000);
            }

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

            if(Port.Items.Count != 0)
                Port.SelectedIndex = 0;

        }
    }
}
