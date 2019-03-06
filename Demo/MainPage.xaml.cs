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
using Windows.Storage;

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
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(typeof(HomePage));
            ParamsInit();
        }

        private void ParamsInit()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if(null == localSettings.Values["isInit"])
            {
                localSettings.Values["isInit"] = "true";
                localSettings.Values["PicXLen"] = "5";
                localSettings.Values["PicYLen"] = "5";
                localSettings.Values["PicXCount"] = "50";
                localSettings.Values["PicYCount"] = "50";
                localSettings.Values["XSpeed"] = "5000";
                localSettings.Values["YSpeed"] = "5000";
                localSettings.Values["GateTime"] = "1";
                localSettings.Values["Threshold"] = "1";
                localSettings.Values["isContinue"] = "false";
                localSettings.Values["ExcelName"] = "";
                localSettings.Values["RowN"] = "0";
                localSettings.Values["ColN"] = "0";
                localSettings.Values["PortName"] = "";
            }
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
                case "历史":
                    mainFrame.Navigate(typeof(HistoryPage));
                    break;
                case "设置":
                    mainFrame.Navigate(typeof(SettingPage));
                    break;
                case "关于":
                    mainFrame.Navigate(typeof(AboutPage));
                    break;
            }

        }

    }
}
