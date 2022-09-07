using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace İstemci
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static void IstemciBaslat()
        {
            byte[] bytes = new byte[1024];

            try
            {
                // Uzak sunucuya bağlan
                // Bağlantı kurmak için kullanılan Host IP Adresini al
                // Bu durumda, localhost'un bir IP adresi olan IP'yi alıyoruz: 127.0.0.1
                // Bir ana bilgisayarın birden fazla adresi varsa, bir adres listesi alırsınız
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAdres = host.AddressList[0];
                IPEndPoint uzakEP = new IPEndPoint(ipAdres, 11000);

                // Bir TCP/IP soketi oluşturun.
                Socket sender = new Socket(ipAdres.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Soketi uzak uç noktaya bağlayın. Herhangi bir hatayı yakalayın.
                try
                {
                    // Uzak Uç Noktaya Bağlan
                    sender.Connect(uzakEP);

                    MessageBox.Show("Soket Bağlandı {0}",
                        sender.RemoteEndPoint.ToString());

                    // Veri dizesini bir bayt dizi olarak kodlayın.
                    byte[] mesaj = Encoding.ASCII.GetBytes("Bu mesaj <EOF>");

                    // Verileri soket üzerinden gönderin.
                    int bytesSent = sender.Send(mesaj);

                    // Uzak cihazdan yanıtı alın.
                    int bytesRec = sender.Receive(bytes);
                    MessageBox.Show("Test = {0}",
                        Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // Soket bağlantısını bırak
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    MessageBox.Show("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    MessageBox.Show("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    MessageBox.Show("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            IstemciBaslat();
        }

    }
}
