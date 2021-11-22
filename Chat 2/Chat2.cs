using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Chat2
{
    public partial class Form1 : Form
    {
        Socket socketenviar = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
        IPEndPoint endereco = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9061);
        Socket socketreceber = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
        EndPoint enderecoFinal = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9060);
        byte[] data = new byte[1024];
        int qtdbytes;
        Thread mythread;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            socketenviar.Close();
        }
        private void meuProcesso()
        {
            socketreceber.Bind(enderecoFinal);
            while (true)
            {
                qtdbytes = socketreceber.ReceiveFrom(data, ref enderecoFinal);
                listBox1.Invoke((Action)delegate ()
                {
                    listBox1.Items.Add(Encoding.ASCII.GetString(data, 0, qtdbytes));
                });
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add("Thiago: " + textBox1.Text);
            socketenviar.SendTo(Encoding.ASCII.GetBytes("Thiago: " + textBox1.Text), endereco);
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            if (button2.Text == "Iniciar")
            {
                button2.Text = "Desativar";
                mythread = new Thread(new ThreadStart(this.meuProcesso));
                mythread.Start();
            }
            else
            {
                button2.Text = "Iniciar";
                mythread.Abort();
            }
        }
    }
}
