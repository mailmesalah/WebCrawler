using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace WebCrawler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    static class AsyncDownload
    {
        public static async Task<string> DownloadPageAsync(string url)
        {
            // ... Target page.
            string page = url;

            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(page))
            using (HttpContent content = response.Content)
            {
                // ... Read the string.
                string result = await content.ReadAsStringAsync();
                return result;
            }
        }

    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string domain = txtUrl.Text.ToString().Trim();
            string[] paths = txtPaths.Text.ToString().Trim().Split(',');
            HashSet<string> links = new HashSet<string>();

            foreach (var path in paths)
            {
                try
                {
                    string url = domain + '/' + path + '/';
                    string retData = await AsyncDownload.DownloadPageAsync(url);
                    string exp = @"/(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})/gi";
                    Match match = Regex.Match(retData, exp);
                    while (match.Success)
                    {

                        if (!links.Contains(match.Value))
                        {
                            links.Add(match.Value);
                            Console.WriteLine(match.Value);
                        }
                        match = match.NextMatch();
                    }
                }
                catch (Exception) { }
                finally
                {

                }
            }

            MessageBox.Show("Download Completed! If any ;)");
        }
    }
}
