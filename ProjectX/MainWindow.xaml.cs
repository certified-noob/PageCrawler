using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ProjectX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                var rawdata = GetData();
                rawdata = rawdata.Substring(rawdata.IndexOf("stream")+8);
                rawdata = rawdata.Remove(rawdata.LastIndexOf("tradesOnly")-2);
                tradesView.Items.Add(JSONOperation.Json2Tree(JToken.Parse(rawdata), "Trades"));  

            }
            catch (JsonReaderException jre)
            {
                MessageBox.Show($"Invalid Json {jre.Message}");
            }
        }

        private  TreeViewItem GetChild(string key, string value)
        {
            var outputValue = string.IsNullOrEmpty(value) ? "null" : value;
            return new TreeViewItem() { Header = $" \x22{key}\x22 : \x22{value}\x22" };
        }
        string GetData()
        {
            var client = new RestClient("https://profit.ly/pusher/api");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("authority", " profit.ly");
            request.AddHeader("method", " POST");
            request.AddHeader("path", " /pusher/api");
            request.AddHeader("scheme", " https");
            request.AddHeader("accept", " */*");
            request.AddHeader("accept-encoding", " gzip, deflate, br");
            request.AddHeader("accept-language", " en-US,en;q=0.9");
            request.AddHeader("content-length", " 158");
            request.AddHeader("content-type", " application/json; charset=UTF-8");
            request.AddHeader("cookie", " affiliate=6; fs=1619620750249; hit=b0870839-b28e-4857-bdb2-c75f1bef0495; _ga=GA1.2.51737911.1619620756; _gid=GA1.2.2097326417.1619620756; __qca=P0-666129733-1619620757519; _admrla=2.0-4f2de6b4-6766-d54d-2b31-e4a9783147e2; viewed-disclaimer=true; _awl=2.1619632999.0.4-a61063ab-4f2de6b46766d54d2b31e4a9783147e2-6763652d6575726f70652d7765737431-6089a367-3; SPRING_SECURITY_REMEMBER_ME_COOKIE=QW5hamFmaToxNjIwODQyNjg1MTAyOjQzMjVjYzI2ZjU0ZGY0NzFkMTQ2ODA3MmM3Mzk5ODE1; SESSION=5ba4dc33-a946-41e8-8fe4-51cd378f237c; _gat=1");
            request.AddHeader("origin", " https://profit.ly");
            request.AddHeader("referer", " https://profit.ly/profiding");
            request.AddHeader("sec-ch-ua", " \" Not A;Brand\";v=\"99\", \"Chromium\";v=\"90\", \"Google Chrome\";v=\"90\"");
            request.AddHeader("sec-ch-ua-mobile", " ?0");
            request.AddHeader("sec-fetch-dest", " empty");
            request.AddHeader("sec-fetch-mode", " cors");
            request.AddHeader("sec-fetch-site", " same-origin");
            client.UserAgent = " Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36";
            request.AddHeader("x-newrelic-id", " UQYDU1dACQoGU1da");
            request.AddHeader("Authorization", "auth fda9b40f5dc022d34e74:3e60db6961a7cc14f6119c5ce81af708ba16352ac93252e26b00bfbb6ab2156");
            request.AddParameter(" application/json; charset=UTF-8", "{\"command\":\"StreamRequest\",\"offset\":0,\"tradesOnly\":true,\"openAggregatedPositionsOnly\":false,\"ukey\":\"ny6CO\",\"ignoresUnknownProperties\":true,\"dingVersion\":null}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
