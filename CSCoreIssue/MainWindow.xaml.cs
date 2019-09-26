using CSCore.Codecs;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace CSCoreIssue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _mp3FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.mp3");
        private string _url = "http://localhost:52342/";
        private bool _closing = true;

        public MainWindow()
        {
            InitializeComponent();
            Task.Factory.StartNew(SimpleListenerExample);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(_url);
            var codec = CodecFactory.Instance.GetCodec(uri);
        }

        public void SimpleListenerExample()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add(_url);
            listener.Start();

            while (true) // This is terrible, I know - but it works for the purpose of the issue!
            {
                var context = listener.GetContext();
                var request = context.Request;
                var response = context.Response;

                byte[] buffer = File.ReadAllBytes(_mp3FilePath);
                response.ContentType = "audio/mpeg";
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
        }
    }
}
