using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
using System.Windows.Shapes;

namespace AsyncAwait
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private List<string> GetSiteData()
        {
            List<string> sites = new List<string>();

            sites.Add("https://www.google.com/");
            sites.Add("https://www.youtube.com/");
            sites.Add("https://www.microsoft.com/es-mx/");
            sites.Add("https://www.herbalife.com/");
            sites.Add("https://stackoverflow.com/");

            return sites;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var timer = Stopwatch.StartNew();
            txtResult.Text = string.Empty;
            
            GetSitesInformation();

            timer.Stop();

            txtResult.Text += $"The query took {timer.ElapsedMilliseconds}\n";
        }

        private void GetSitesInformation()
        {
            List<string> sites = GetSiteData();

            foreach (var site in sites)
            {
                string content = GetSiteContent(site);
                txtResult.Text += $"Content for site {site} with length {content.Length}\n";
            }
        }
        
        private string GetSiteContent(string site)
        {
            string content = string.Empty;
            using (WebClient client = new WebClient())
            {
                content = client.DownloadString(site);
            }

            return content;
        }
        
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var timer = Stopwatch.StartNew();
            txtResult.Text = string.Empty;
            
            await GetSitesInformationAsync();

            timer.Stop();

            txtResult.Text += $"The query took {timer.ElapsedMilliseconds}\n";
        }

        private async Task GetSitesInformationAsync()
        {
            List<string> sites = GetSiteData();

            foreach (var site in sites)
            {
                string content = await Task.Run(() => GetSiteContent(site));
                txtResult.Text += $"Content for site {site} with length {content.Length}\n";
            }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var timer = Stopwatch.StartNew();
            txtResult.Text = string.Empty;

            await GetSitesInformationAsyncParallel();

            timer.Stop();

            txtResult.Text += $"The query took {timer.ElapsedMilliseconds}\n";
        }

        private async Task GetSitesInformationAsyncParallel()
        {
            List<string> sites = GetSiteData();
            List<Task<string>> tasks = new List<Task<string>>();

            foreach (var site in sites)
            {
                tasks.Add(Task.Run(()=>GetSiteContent(site)));
            }

            var results = await Task.WhenAll(tasks);
            foreach (var item in results)
            {
                txtResult.Text += $"Content for site {item.Length}\n";
            }
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var timer = Stopwatch.StartNew();
            txtResult.Text = string.Empty;

            await GetSitesInformationAsyncParallelFull();

            timer.Stop();

            txtResult.Text += $"The query took {timer.ElapsedMilliseconds}\n";
        }

        private async Task GetSitesInformationAsyncParallelFull()
        {
            List<string> sites = GetSiteData();
            List<Task<string>> tasks = new List<Task<string>>();

            foreach (var site in sites)
            {
                tasks.Add(GetSiteContentAsync(site));
            }

            var results = await Task.WhenAll(tasks);
            foreach (var item in results)
            {
                txtResult.Text += $"Content for site {item.Length}\n";
            }
        }

        private async Task<string> GetSiteContentAsync(string site)
        {
            string content = string.Empty;
            using (WebClient client = new WebClient())
            {
                content = await client.DownloadStringTaskAsync(site);
            }

            return content;
        }
    }
}
