using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Demo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(typeof(HomePage));
        }

        private void NvMenu_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItem == null)
                return;
            Debug.WriteLine(args.InvokedItem);
            switch(args.InvokedItem)
            {
                case "开始":
                    mainFrame.Navigate(typeof(HomePage));
                    break;
                case "传感器设置":
                    mainFrame.Navigate(typeof(SensorPage));
                    break;
                case "步进电机设置":
                    mainFrame.Navigate(typeof(MotorPage));
                    break;
                case "关于":
                    mainFrame.Navigate(typeof(AboutPage));
                    break;
            }

        }

        private void toRunning()
        {
            Frame.Navigate(typeof(RunPage));
        }

    }
}
