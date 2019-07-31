using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Spi;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IoT.Windows10.ws2812b
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private SpiDevice spi;

        public MainPage()
        {
            this.InitializeComponent();
            WS2812B();
        }

        public async Task WS2812B()
        {
            var settings = new SpiConnectionSettings(0);
            settings.ClockFrequency = 4000000;
            List<byte> dataBits = new List<byte>();

            string spiDevice = SpiDevice.GetDeviceSelector("SPI0");

            var deviceInformation = await DeviceInformation.FindAllAsync(spiDevice);

            using (spi = await SpiDevice.FromIdAsync(deviceInformation[0].Id, settings))
            {
                if (spi != null)
                {
                    spi.Write(new byte[] { 0x00 });
                    Thread.Sleep(1);

                    int value = 0b1100;
                    int encoding = 0;
                    int indexBitsOfLed = 0;
                    int indexBytesOfFrame = 0;

                    for (int index = 0; index < 8; index++)
                    {
                        indexBytesOfFrame = 0;
                        while (indexBytesOfFrame < 3)
                        {
                            indexBitsOfLed = 0;
                            encoding = 0;

                            if (indexBytesOfFrame == 2)
                                value = 0b1110;
                            else
                                value = 0b1100;

                            while (indexBitsOfLed < 8)
                            {
                                encoding <<= 4;
                                encoding |= value;

                                indexBitsOfLed++;
                            }

                            dataBits.Add((byte)((encoding >> 24) & 0xff));
                            dataBits.Add((byte)((encoding >> 16) & 0xff));
                            dataBits.Add((byte)((encoding >> 8) & 0xff));
                            dataBits.Add((byte)(encoding & 0xff));

                            indexBytesOfFrame++;
                        }
                    }

                    spi.Write(dataBits.ToArray());
                    Thread.Sleep(1);
                }
            }

        }
    }
}
