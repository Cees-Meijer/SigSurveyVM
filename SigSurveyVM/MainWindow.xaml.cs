using System;
using System.Windows;
using System.Threading.Tasks.Dataflow;
using System.Net.Sockets;
using System.IO;
using AD2CPData;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF.Themes;
using System.Windows.Media;

namespace SurveyVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static SpectrogramViewModel spectrogramViewModel = new SpectrogramViewModel();
        static SpectrogramViewModel spectrogramViewModel2 = new SpectrogramViewModel();
        static SpectrogramViewModel spectrogramViewModel3 = new SpectrogramViewModel();
        static SpectrogramViewModel spectrogramViewModel4 = new SpectrogramViewModel();
        static int SelectedChartView;
        TrackPlotViewModel trackPlot = new TrackPlotViewModel();

        public MainWindow()
        {
            InitializeComponent();
            Image1.DataContext = spectrogramViewModel;
            Image2.DataContext = spectrogramViewModel2;
            Image3.DataContext = spectrogramViewModel3;
            Image4.DataContext = spectrogramViewModel4;
            TrackPlot.DataContext = trackPlot;
            GreenLight.Fill =  new SolidColorBrush(Colors.LawnGreen);
            AD2CP_StatusText.Text = "AD2CP Status messages\r\nwill appear here.";
            GPS_StatusText.Text = "GPS Status messages\r\nwill appear here.";
        }

        static ActionBlock<UdpReceiveResult> ProcessAD2CPData = new ActionBlock<UdpReceiveResult>(udp_received =>
        {
            AD2CP_Header Header = new AD2CP_Header();
            AD2CP_DataFormat3 ADCP_Burst = new AD2CP_DataFormat3();
            MemoryStream ms = new MemoryStream(udp_received.Buffer);
            BinaryReader b = new BinaryReader(ms);
            Header.Read(b);
            if (Header.ID == 0x15)
            {
                ADCP_Burst.Read(b);
                switch (SelectedChartView)
                {
                    case 0:
                        spectrogramViewModel.AddTrace(ADCP_Burst.velocityData[0]);
                        spectrogramViewModel2.AddTrace(ADCP_Burst.velocityData[1]);
                        spectrogramViewModel3.AddTrace(ADCP_Burst.velocityData[2]);
                        spectrogramViewModel4.AddTrace(ADCP_Burst.velocityData[3]);
                        break;
                    case 1:
                        spectrogramViewModel.AddTrace(ADCP_Burst.amplitudeData[0]);
                        spectrogramViewModel2.AddTrace(ADCP_Burst.amplitudeData[1]);
                        spectrogramViewModel3.AddTrace(ADCP_Burst.amplitudeData[2]);
                        spectrogramViewModel4.AddTrace(ADCP_Burst.amplitudeData[3]);
                        break;
                    case 2:
                        spectrogramViewModel.AddTrace(ADCP_Burst.correlationData[0]);
                        spectrogramViewModel2.AddTrace(ADCP_Burst.correlationData[1]);
                        spectrogramViewModel3.AddTrace(ADCP_Burst.correlationData[2]);
                        spectrogramViewModel4.AddTrace(ADCP_Burst.correlationData[3]);
                        break;
                }

                //Console.WriteLine("V{0},{1},{2}", ADCP_Burst.velocityData[0][0], ADCP_Burst.velocityData[0][1], ADCP_Burst.velocityData[0][2]);

            }
            //Console.WriteLine("Sync:{0:X} ID:{1:X} Size{2} Year:{3}", Header.Sync, Header.ID, Header.DataSize, ADCP_Burst.year + 1900);

        },
            // Specify a task scheduler from the current synchronization context
            // so that the action runs on the UI thread.
        new ExecutionDataflowBlockOptions
        {
           TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext()
        });
        

        private static void UDPListener()
        {
            Task.Run(async () =>
            {
                using (var udpClient = new UdpClient(9002))
                {

                    while (true)
                    {
                        //IPEndPoint object will allow us to read datagrams sent from any source.
                        var receivedResults = await udpClient.ReceiveAsync();

                        using (Stream DestinationWriter = File.Open("DATA_LOG.BIN", FileMode.Append))
                        {
                            await DestinationWriter.WriteAsync(receivedResults.Buffer, 0, receivedResults.Buffer.Length);
                        }
                        ProcessAD2CPData.Post(receivedResults);
                    }
                }
            });
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            UDPListener();
        }

        private void ViewTabs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var item = sender as TabControl;
            SelectedChartView= item.SelectedIndex;
            Console.WriteLine("Selected:{0}", SelectedChartView);

            switch (SelectedChartView)
            {
                case 0: // Velocity 
                    spectrogramViewModel.SetMinMax(-500, 500);
                    spectrogramViewModel2.SetMinMax(-500, 500);
                    spectrogramViewModel3.SetMinMax(-500, 500);
                    spectrogramViewModel4.SetMinMax(-500, 500);

                    break;
                case 1: // Amplitude
                case 2: // Correlation
                    spectrogramViewModel.SetMinMax(0, 256);
                    spectrogramViewModel2.SetMinMax(0, 256);
                    spectrogramViewModel3.SetMinMax(0, 256);
                    spectrogramViewModel4.SetMinMax(0, 256);

                    break;
            }

        }

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            /// Image does not have a size before the image is rendered, so we use the size of the surrounding Border Control.
            /// 
            Console.WriteLine("Width:{0},Height:{1},AWidth:{2},AHeight:{3}", Image1.Width, Image1.Height,Image1.ActualWidth,Image1.ActualHeight);
            spectrogramViewModel.Init(ref Image1);
            spectrogramViewModel.SetMinMax(-500, 500);
            spectrogramViewModel2.Init(ref Image2);
            spectrogramViewModel2.SetMinMax(-500, 500);
            spectrogramViewModel3.Init(ref Image3);
            spectrogramViewModel3.SetMinMax(-500, 500);
            spectrogramViewModel4.Init(ref Image4);
            spectrogramViewModel4.SetMinMax(-500, 500);
            ThemeManager.ApplyTheme(this, "ExpressionDark");
        }

  

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           Console.WriteLine("Width:{0},Height:{1},AWidth:{2},AHeight:{3}", Image1.Width, Image1.Height, Image1.ActualWidth, Image1.ActualHeight);
            spectrogramViewModel.Init(ref Image1);
            spectrogramViewModel2.Init(ref Image2);
            spectrogramViewModel3.Init(ref Image3);
            spectrogramViewModel4.Init(ref Image4);
        }

        
    }
}
