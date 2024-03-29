﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IoT.Windows10.ws2812b
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            var ws2812bDriver = new WS2812bDriver();

            Task.Run(async () =>
            {
                await ws2812bDriver.InitializeAsync(64);
                Random random = new Random();
                while (true)
                {
                    ws2812bDriver.Clean();
                    ws2812bDriver.Write(random.Next(0, 63), (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                    ws2812bDriver.Write(random.Next(0, 63) , (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                    ws2812bDriver.Write(random.Next(0, 63), (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                    ws2812bDriver.Write(random.Next(0, 63) , (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
                    ws2812bDriver.RefreshLeds();
                    Thread.Sleep(1);
                }
            });
        }
    }
}
