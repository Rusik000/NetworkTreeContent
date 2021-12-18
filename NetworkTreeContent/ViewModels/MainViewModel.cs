using HtmlAgilityPack;
using NetworkTreeContent.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NetworkTreeContent.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainWindow MainView { get; set; }
        public RelayCommand SearchCommand { get; set; }

        public string Adress { get; set; }

        public MainViewModel()
        {
            SearchCommand = new RelayCommand((sender) =>
            {
                Adress = MainView.SearchTxtBx.Text;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://{Adress}");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;
                    if (String.IsNullOrWhiteSpace(response.CharacterSet))
                    {

                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream,
                            Encoding.GetEncoding(response.CharacterSet));
                    }

                    string data = readStream.ReadToEnd();

                    //
                    var doc = new HtmlWeb().Load($"http://{Adress}");
                    var linkTags = doc.DocumentNode.Descendants("link");
                    var linkedPages = doc.DocumentNode.Descendants("a")
                                                      .Select(a => a.GetAttributeValue("href", null))
                                                      .Where(u => !String.IsNullOrEmpty(u));
                    string[] arr = linkedPages.ToArray();
                    MainView.FirstTree.Header = MainView.SearchTxtBx.Text;
                    MainView.SecondTree.Header = arr[0];
                    MainView.ThirdTree.Header = arr[1];
                    MainView.FourthTree.Header = arr[2];
                    MainView.FivethTree.Header = arr[3];
                    MainView.SixthTree.Header = arr[4];
                    MainView.SeventhTree.Header = arr[5];
                    MainView.EighthTree.Header = arr[6];
                    MainView.NinethTree.Header = arr[7];
                    //
                    response.Close();
                    readStream.Close();
                }
            });

        }
    }
}
