using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MovingObjectClient
{
    public partial class Form1 : Form
    {
        private Rectangle rect = new Rectangle(20, 20, 30, 30);
        private Pen red = new Pen(Color.Red);
        private SolidBrush fillBlue = new SolidBrush(Color.Blue);

        private TcpClient client;
        private NetworkStream stream;
        private Thread receiveThread;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            receiveThread = new Thread(ConnectAndReceive) { IsBackground = true };
            receiveThread.Start();
        }

        private void ConnectAndReceive()
        {
            try
            {
                client = new TcpClient("127.0.0.1", 5000);
                stream = client.GetStream();

                var buffer = new byte[1024];
                while (true)
                {
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    if (bytes <= 0) break;
                    var msg = Encoding.UTF8.GetString(buffer, 0, bytes);
                    var parts = msg.Split(',');
                    if (parts.Length >= 1 && int.TryParse(parts[0], out int x))
                    {
                        rect.X = x;
                        this.Invoke((Action)(() => Invalidate()));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Invoke((Action)(() => MessageBox.Show("Connection error: " + ex.Message)));
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(fillBlue, rect);
            e.Graphics.DrawRectangle(red, rect);
        }
    }
}
