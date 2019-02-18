﻿using System;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Demo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RunPage : Page
    {
        public RunPage()
        {
            this.InitializeComponent();
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            if(Run.Content.ToString()=="开始" || Run.Content.ToString() == "继续")
            {
                Run.Content = "暂停";
            }
            else
            {
                Run.Content = "继续";
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Data_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
