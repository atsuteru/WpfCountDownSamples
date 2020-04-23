using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public CancellationTokenSource Canceller { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Canceller = new CancellationTokenSource();
            var cancelToken = Canceller.Token;
            // !ATTENTION! This timer has a time lag due to the processing time when it times out.
            Task
                .Run(async () =>
                {
                    while(!cancelToken.IsCancellationRequested)
                    {
                        await Task
                            .Delay(1000)
                            .ContinueWith(t =>
                            {
                                Log.Text = $"{Log.Text}{DateTime.Now.ToString("HH:mm:ss")}\r\n";
                            }, cancelToken, TaskContinuationOptions.None, scheduler);
                    }
                }, cancelToken);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Canceller?.Cancel();
        }
    }
}
