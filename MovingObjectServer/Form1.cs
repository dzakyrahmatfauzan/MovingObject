using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace MovingObjectServer
{
    public partial class Form1 : Form
    {
        Pen red = new Pen(Color.Red);
        Rectangle rect = new Rectangle(20, 20, 30, 30);
        SolidBrush fillBlue = new SolidBrush(Color.Blue);
        int slide = 10;
        TcpListener server;
        List<TcpClient> clients = new List<TcpClient>();

        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 50;
            timer1.Enabled = true;

            // server jalan
            server = new TcpListener(IPAddress.Any, 5000);
            server.Start();
            server.BeginAcceptTcpClient(OnClientConnect, null);
        }

        private void OnClientConnect(IAsyncResult ar)
        {
            TcpClient client = server.EndAcceptTcpClient(ar);
            clients.Add(client);

            // siap terima client baru berikutnya
            server.BeginAcceptTcpClient(OnClientConnect, null);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            back();
            rect.X += slide;

            // kirim posisi ke semua client
            string pos = rect.X + "," + rect.Y;
            byte[] data = Encoding.UTF8.GetBytes(pos);

            foreach (var c in clients.ToList())
            {
                try
                {
                    c.GetStream().Write(data, 0, data.Length);
                }
                catch
                {
                    clients.Remove(c); // kalau client disconnect
                }
            }

            Invalidate();
        }

        private void back()
        {
            if (rect.X >= this.Width - rect.Width * 2)
                slide = -10;
            else if (rect.X <= rect.Width / 2)
                slide = 10;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(red, rect);
            g.FillRectangle(fillBlue, rect);
        }
    }
}
