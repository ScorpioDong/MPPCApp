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
            SerialReflush();
        }

        private void ToRun_Click(object sender, RoutedEventArgs e)
        {
            if (!(Port.SelectedItem == null))
            {
                if (!(Window.Current.Content is Frame RootFrame))
                    return;

                RootFrame.Navigate(typeof(RunPage));
            }
            else
            {
                PortFailed.Show("未找到串口，请选中或刷新后尝试！");
            }

        }

        private void PortReflush_Click(object sender, RoutedEventArgs e)
        {
            SerialReflush();
        }

        private async void SerialReflush()
        {
            string filter = SerialDevice.GetDeviceSelector();
            var dis = await DeviceInformation.FindAllAsync(filter);

            foreach (var id in dis)
                Port.Items.Add(id.Name.ToString());

            Port.SelectedIndex = 0;

        }
    }
}
