using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Diagnostics;
using System.Windows.Shapes;

namespace FirstEncounterWithAsync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        
        private void buttonBlockingCall_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Before blocking call");
            // If we awaited directly client.GetStringAsync here,
            // application would run normally.
            // From my understanting it is simply because HttpClient does not use
            // contexts to run async operations. Just as we do
            // in below method with ConfigureAwait(false).
            var task = DownloadForBlockingCallAsync();
            responseText.Text = task.Result;
            Debug.WriteLine("After blocking call - should never be printed.");
        }

        private async Task<string> DownloadForBlockingCallAsync()
        {
            // ConfigureAwait (false) solves issue of deadlock. It can be used,
            // as we do not have any continuation in this method that may require
            // context.
            var response = await client.GetStringAsync("https://meczyki.pl/").ConfigureAwait(false);
            return response;
        }

        private async void button_Click(object sender, RoutedEventArgs e)
            => await OnClick(sender);
        private async Task OnClick(object s) => await DownloadAsync();
        private async Task DownloadAsync()
        {
            // Breakpoint before await includes in stack trace OnClick(object) method, while
            // after await, if awaited operation takes long enough (so there will be asynchronous execution),
            // the stack trace will only contain current method, rest of the stack trace comes from
            // windows message loop and asynchronous calls.
            label.Content = "Fetching...";

            Debug.WriteLine(Environment.StackTrace);
            var text = await client.GetStringAsync("https://meczyki.pl/").ConfigureAwait(true);
            label.Content = $"Fetched {text.Length} characters in response.";
            responseText.Text = text;

            Debug.WriteLine("After awaiting");
            Debug.WriteLine(Environment.StackTrace);
        }

        private void buttonExperiment_Click(object sender, RoutedEventArgs e)
        {
            localAsync().GetAwaiter().GetResult();

            async Task localAsync()
            {
                buttonExperiment.Content = "Starting experiment...";
                var task = Task.Delay(5 * 1000);
                var configuredTask = task.ConfigureAwait(false);
                await configuredTask;

                buttonExperiment.Content = "After experiment!";
            };
        }
    }
}
