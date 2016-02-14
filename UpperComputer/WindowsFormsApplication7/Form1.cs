using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace WindowsFormsApplication7
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private StringBuilder builder = new StringBuilder();//用来进行字符串的处理

        SerialPort port1 = new SerialPort();

        //private long received_count;        接收计数

        //private long send_count;            发送计数
        
       

        private void button1_Click(object sender, EventArgs e)
        {
            string zhengshu = this.textBox1.Text;
            string xiaoshu =  this.textBox2.Text;
            int geinikan1, geinikan2;
            //string jiugeinikan1, jiugeinikan2;
            //string heti1;
            //string heti2;
            //int geili1,geili2;
            
            geinikan1= int.Parse(zhengshu);
            geinikan2 = int.Parse(xiaoshu);
          

            string zheng1, zheng2, xiao1, xiao2;
            zheng1 = zhengshu.Substring(0, 1);
            zheng2 = zhengshu.Substring(1, 1);
            xiao1 = xiaoshu.Substring(0, 1);
            xiao2 = xiaoshu.Substring(1, 1);


            
            char[] tongbuzi = new char[3];
            tongbuzi[0] = 'C';
            tongbuzi[1] = 'O';
            tongbuzi[2] = 'M';

            byte[] caozuozi = new byte[1];
            caozuozi[0] = 0xA0;

            byte[] shujuwei = new byte[2];
            shujuwei[0] = (byte)geinikan1;
            shujuwei[1] = (byte)geinikan2;
 
            /*string[] caibugeinikan1=new string[2];
            string[] caibugeinikan2=new string[2];
           /*
           
            byte[] xiaoshuchuan  = new byte[1];
            xiaoshuchuan=strToToHexByte(xiaoshu);
            geinikan2 = xiaoshu.ToString("X");
           */
            //caibugeinikan1[0] = zheng1;
            //caibugeinikan1[1] = zheng2;
            //caibugeinikan2[0] = xiao1;
            //caibugeinikan2[1] = xiao2;
           // char[] hetile1 = new char[2];
           // char[] hetile2 = new char[2];

           //char[] hetile1 = jiugeinikan1.ToCharArray();
           //char[] hetile2 = jiugeinikan2.ToCharArray();

            port1.Write(tongbuzi, 0, 3);
            port1.Write(caozuozi, 0, 1);
            port1.Write(shujuwei, 0, 2);

            Image img1 = Image.FromFile(@"C:\数码管图片\" + zheng1 + ".jpg");
            pictureBox1.Image = img1;

            Image img2 = Image.FromFile(@"C:\数码管图片\" + zheng2 + ".jpg");
            pictureBox2.Image = img2;

            Image img3 = Image.FromFile(@"C:\数码管图片\" + xiao1 + ".jpg");
            pictureBox3.Image = img3;

            Image img4 = Image.FromFile(@"C:\数码管图片\" + xiao2 + ".jpg");
            pictureBox4.Image = img4;

            Image img5 = Image.FromFile(@"C:\数码管图片\dot.png");
            pictureBox5.Image = img5;

            label7.Text = "整数";
            label8.Text = "小数";
        
        
        
        }
        
        //  将字符串转化为16进制字符
        private string StringToHexString(string s, Encoding encode)
        {
            byte[] b = encode.GetBytes(s);
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)
            {
                result += Convert.ToString(b[i], 16);
            }
            return result;
        }
         //将16进制字符串转化为byte[]
         private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        
         //将byte[]转为16进制字符串
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /*private string DexStringToHexString(string a)
        {
            int step1 = Int32.Parse(a);
            string step2 = step1.ToString("X2");
            return step2;
        }*/



        //打开串口的方法
        public void OpenPort()
        {
            try
            {
                port1.Open();
            }
            catch { }
            if (port1.IsOpen)
            {
                MessageBox.Show("the port is opened!");
            }
            else
            {
                MessageBox.Show("failure to open the port!");
            }
        }

        public void ClosePort()
        {
            try
            {
                port1.Close();
            }

            catch { }
            if (!port1.IsOpen)
            {
                MessageBox.Show("the port is closed!"); 
            }
            else
            {
                MessageBox.Show("failure to close the port!");
            }


         
        }



        //com口发送数据
        public void SendCommand1()
        {
            
            char[] tongbuzi = new char[3];
            tongbuzi[0]='C';
            tongbuzi[1]='O';
            tongbuzi[2]='M';

            byte[] shuju = new byte[3];
            shuju[0]=0xA0;
            shuju[1]=0x00;
            shuju[2]=0x00;
          
            port1.Write(tongbuzi,0,3);
            port1.Write(shuju,0,3);
            
        }





        private void port1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //int state = 0;
            int i;
            char[] ch = new char[6];
            byte[] shujuhui = new byte[3];
            char[] caozuozi = new char[3];
            try
            {
                StringBuilder currentline = new StringBuilder();
                //循环接收数据
                while (port1.BytesToRead > 0)
                {



                    for (i = 0; i < 6; i++)
                    {
                        ch[i] = (char)port1.ReadByte();
                        MySS.data[i] = ch[i];
                    }
                    currentline.Append(ch);
                }
                //在这里对接收到的数据进行处理


                currentline = new StringBuilder();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

        }


        private class MySS
        {
            public static int[] data = new int[6];
        }
        
        
        
        public byte[] ReceiveCommand()
        {
            int chu = 0;
            byte[] shujuhui = new byte[3];
            char[] caozuozi = new char[3];
            //string bijiao;
            int state = 0;
            while(chu==0)
            {
                
                switch (state)
                {
                    case 0:
                        caozuozi[0]=(char)port1.ReadChar();
                        if (caozuozi[0] == 'C')
                            state = 1;
                        else state = 0;
                        break;
                    case 1:
                        caozuozi[1] =(char)port1.ReadChar();
                        if (caozuozi[1] == 'O')
                            state = 2;
                        else state = 0;
                        break;
                    case 2:
                        caozuozi[2] =(char)port1.ReadChar();
                        if (caozuozi[2] == 'M')
                            state = 3;
                        else state = 0;
                        break;
                    case 3: shujuhui[0] = (byte)port1.ReadByte();
                            shujuhui[1] = (byte)port1.ReadByte();
                            shujuhui[2] = (byte)port1.ReadByte();
                            
                            chu = 1;
                        break;   
                }

            }
            return shujuhui;

        }
     



        public void InitCOM()
        {
            //SerialPort port1 = new SerialPort(PortName); 
            port1.PortName = "COM1";
            port1.BaudRate = 9600;//波特率
            port1.ReceivedBytesThreshold = 8;//设置 DataReceived 事件发生前内部输入缓冲区中的字节数
            port1.Parity = Parity.None;//无奇偶校验位
            port1.StopBits = StopBits.One;//一个停止位
            port1.DataReceived += new SerialDataReceivedEventHandler(port1_DataReceived);//DataReceived事件委托
            
            //;
            //port1.Handshake = Handshake.RequestToSend;//控制协议
            // port1.ReceivedBytesThreshold = 4;//设置 DataReceived 事件发生前内部输入缓冲区中的字节数
            // port1.DataReceived += new SerialDataReceivedEventHandler(port1_DataReceived);//DataReceived事件委托

        }

        private void button2_Click(object sender, EventArgs e)
        {
            InitCOM();
            OpenPort();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            char[] tongbuzi = new char[3];
            tongbuzi[0] = 'C';
            tongbuzi[1] = 'O';
            tongbuzi[2] = 'M';

            byte[] shuju = new byte[3];
            shuju[0] = 0xA1;
            shuju[1] = 0xff;
            shuju[2] = 0xff;
         
            port1.Write(tongbuzi, 0, 3);
            port1.Write(shuju, 0, 3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClosePort();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            char[] tongbuzi = new char[3];
            tongbuzi[0] = 'C';
            tongbuzi[1] = 'O';
            tongbuzi[2] = 'M';

            byte[] shuju = new byte[3];
            shuju[0] = 0xA2;
            shuju[1] = 0xff;
            shuju[2] = 0xff;
         
            port1.Write(tongbuzi, 0, 3);
            port1.Write(shuju, 0, 3);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            byte[] hui = new byte[3];
            byte[] zheng = new byte[1];
            byte[] xiao  = new byte[1];
            char[] tongbuzi = new char[3];
            string[] huizhuan = new string[2];
            string zheng1, zheng2, xiao1, xiao2;
            
            
           // string xianshi;
            int geinikan;
            string geinikan2;
            int jiugeinikan;
            string jiugeinikan2;




            tongbuzi[0] = 'C';
            tongbuzi[1] = 'O';
            tongbuzi[2] = 'M';

            byte[] shuju = new byte[3];
            shuju[0] = 0xA8;
            shuju[1] = 0xff;
            shuju[2] = 0xff;
           
            port1.Write(tongbuzi, 0, 3);
            port1.Write(shuju, 0, 3);
             
            hui=ReceiveCommand();

            if (hui[0] == 0xA8)
            { 
               zheng[0]=hui[1];    //传入高位
               xiao[0]=hui[2];     //传入低位
                     
            }

            huizhuan[0]=byteToHexStr(zheng);
            //MessageBox.Show(huizhuan[0]);
            
            huizhuan[1]=byteToHexStr(xiao);
            //MessageBox.Show(huizhuan[1]);

            geinikan=Convert.ToInt16(huizhuan[0], 16);
            geinikan2 = geinikan.ToString();
            //MessageBox.Show(geinikan2);

            jiugeinikan = Convert.ToInt16(huizhuan[1], 16);
            jiugeinikan2 = jiugeinikan.ToString();
           // MessageBox.Show(jiugeinikan2);

            zheng1 = geinikan2.Substring(0, 1);
            //MessageBox.Show(zheng1);
            
            zheng2 = geinikan2.Substring(1,1);
            //MessageBox.Show(zheng2);

            xiao1 = jiugeinikan2.Substring(0, 1);
            //MessageBox.Show(xiao1);
           
            xiao2 = jiugeinikan2.Substring(1,1);
            //MessageBox.Show(xiao2);

            
            
            
            
            Image img1 = Image.FromFile(@"C:\数码管图片\"+zheng1+".jpg");
            pictureBox1.Image = img1;

            Image img2 = Image.FromFile(@"C:\数码管图片\" + zheng2 + ".jpg");
            pictureBox2.Image = img2;

            Image img3 = Image.FromFile(@"C:\数码管图片\" + xiao1 + ".jpg");
            pictureBox3.Image = img3;

            Image img4 = Image.FromFile(@"C:\数码管图片\" + xiao2 + ".jpg");
            pictureBox4.Image = img4;

            Image img5 = Image.FromFile(@"C:\数码管图片\dot.png");
            pictureBox5.Image = img5;

            label7.Text = "整数";
            label8.Text = "小数";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            char[] tongbuzi = new char[3];
            tongbuzi[0] = 'C';
            tongbuzi[1] = 'O';
            tongbuzi[2] = 'M';

            byte[] shuju = new byte[3];
            shuju[0] = 0xA3;
            shuju[1] = 0xff;
            shuju[2] = 0xff;
            
            port1.Write(tongbuzi, 0, 3);
            port1.Write(shuju, 0, 3);
        }



        private void button9_Click(object sender, EventArgs e)
        {
            string fen  = this.textBox3.Text;
            string miao = this.textBox4.Text;
            int geinikan1, geinikan2;
            //string jiugeinikan1, jiugeinikan2;
            //int geili1, geili2;

            geinikan1 = int.Parse(fen);
            geinikan2 = int.Parse(miao);

            string fen1, fen2, miao1, miao2;
            fen1 = fen.Substring(0, 1);
            fen2 = fen.Substring(1, 1);
            miao1 =miao.Substring(0, 1);
            miao2 =miao.Substring(1, 1);

            char[] tongbuzi = new char[3];
            tongbuzi[0] = 'C';
            tongbuzi[1] = 'O';
            tongbuzi[2] = 'M';

            byte[] caozuozi = new byte[1];
            caozuozi[0] = 0xA4;
           
            byte[] fenchuan = new byte[1];
            //fenchuan = strToToHexByte(fen);
            byte[] shujuwei = new byte[2];
            //miaochuan = strToToHexByte(miao);
            shujuwei[0] = (byte)geinikan1;
            shujuwei[1] = (byte)geinikan2;

            port1.Write(tongbuzi, 0, 3);
            port1.Write(caozuozi, 0, 1);
            port1.Write(shujuwei,0,2);
            

            Image img1 = Image.FromFile(@"C:\数码管图片\" + fen1 + ".jpg");
            pictureBox1.Image = img1;

            Image img2 = Image.FromFile(@"C:\数码管图片\" + fen2 + ".jpg");
            pictureBox2.Image = img2;

            Image img3 = Image.FromFile(@"C:\数码管图片\" + miao1 + ".jpg");
            pictureBox3.Image = img3;

            Image img4 = Image.FromFile(@"C:\数码管图片\" + miao2 + ".jpg");
            pictureBox4.Image = img4;

            
            pictureBox5.Image = null;

            label7.Text = "分";
            label8.Text = "秒";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string fen = this.textBox3.Text;
            string miao = this.textBox4.Text;
            int geinikan1, geinikan2;
            //string jiugeinikan1, jiugeinikan2;
            //int geili1, geili2;

            geinikan1 = int.Parse(fen);
            geinikan2 = int.Parse(miao);

            string fen1, fen2, miao1, miao2;
            fen1 = fen.Substring(0, 1);
            fen2 = fen.Substring(1, 1);
            miao1 = miao.Substring(0, 1);
            miao2 = miao.Substring(1, 1);

            char[] tongbuzi = new char[3];
            tongbuzi[0] = 'C';
            tongbuzi[1] = 'O';
            tongbuzi[2] = 'M';

            byte[] caozuozi = new byte[1];
            caozuozi[0] = 0xA4;

            byte[] fenchuan = new byte[1];
            //fenchuan = strToToHexByte(fen);
            byte[] shujuwei = new byte[2];
            //miaochuan = strToToHexByte(miao);
            shujuwei[0] = (byte)geinikan1;
            shujuwei[1] = (byte)geinikan2;

            port1.Write(tongbuzi, 0, 3);
            port1.Write(caozuozi, 0, 1);
            port1.Write(shujuwei, 0, 2);

            Image img1 = Image.FromFile(@"C:\数码管图片\" + fen1 + ".jpg");
            pictureBox1.Image = img1;

            Image img2 = Image.FromFile(@"C:\数码管图片\" + fen2 + ".jpg");
            pictureBox2.Image = img2;

            Image img3 = Image.FromFile(@"C:\数码管图片\" + miao1 + ".jpg");
            pictureBox3.Image = img3;

            Image img4 = Image.FromFile(@"C:\数码管图片\" + miao2 + ".jpg");
            pictureBox4.Image = img4;


            pictureBox5.Image = null;

            label7.Text = "分";
            label8.Text = "秒";
        }

      

        private void button13_Click(object sender, EventArgs e)
        {
            byte[] hui = new byte[3];
            byte[] zheng = new byte[1];
            byte[] xiao = new byte[1];
            char[] tongbuzi = new char[3];
            string[] huizhuan = new string[2];
            string zheng1, zheng2, xiao1, xiao2;

            //string xianshi;
            int geinikan;
            string geinikan2;
            int jiugeinikan;
            string jiugeinikan2;

            tongbuzi[0] = 'C';
            tongbuzi[1] = 'O';
            tongbuzi[2] = 'M';

            byte[] shuju = new byte[3];
            shuju[0] = 0xAA;
            shuju[1] = 0xff;
            shuju[2] = 0xff;

            port1.Write(tongbuzi, 0, 3);
            port1.Write(shuju, 0, 3);

            hui = ReceiveCommand();

            if (hui[0] == 0xAA)
            {
                zheng[0] = hui[1];
                xiao[0] = hui[2];

            }
            huizhuan[0] = byteToHexStr(zheng);
            huizhuan[1] = byteToHexStr(xiao);

            geinikan = Convert.ToInt16(huizhuan[0], 16);
            geinikan2 = geinikan.ToString();

            jiugeinikan = Convert.ToInt16(huizhuan[1], 16);
            jiugeinikan2 = jiugeinikan.ToString();

            zheng1 = geinikan2.Substring(0, 1);
            //MessageBox.Show(zheng1);

            zheng2 = geinikan2.Substring(1, 1);
            //MessageBox.Show(zheng2);

            xiao1 = jiugeinikan2.Substring(0, 1);
            //MessageBox.Show(xiao1);

            xiao2 = jiugeinikan2.Substring(1, 1);
            //MessageBox.Show(xiao2);

            Image img1 = Image.FromFile(@"C:\数码管图片\" + zheng1 + ".jpg");
            pictureBox1.Image = img1;

            Image img2 = Image.FromFile(@"C:\数码管图片\" + zheng2 + ".jpg");
            pictureBox2.Image = img2;

            Image img3 = Image.FromFile(@"C:\数码管图片\" + xiao1 + ".jpg");
            pictureBox3.Image = img3;

            Image img4 = Image.FromFile(@"C:\数码管图片\" + xiao2 + ".jpg");
            pictureBox4.Image = img4;

            pictureBox5.Image = null;

            label7.Text = "分";
            label8.Text = "秒";
            
        }

        private void button14_Click(object sender, EventArgs e)
        {
            byte[] hui = new byte[3];
            byte[] zheng = new byte[1];
            byte[] xiao = new byte[1];
            string[] huizhuan = new string[2];
            string zheng1, zheng2, xiao1, xiao2;
            char[] tongbuzi = new char[3];

           // string xianshi;
            int geinikan;
            string geinikan2;
            int jiugeinikan;
            string jiugeinikan2;

            tongbuzi[0] = 'C';
            tongbuzi[1] = 'O';
            tongbuzi[2] = 'M';

            byte[] shuju = new byte[3];
            shuju[0] = 0xAB;
            shuju[1] = 0xff;
            shuju[2] = 0xff;

            port1.Write(tongbuzi, 0, 3);
            port1.Write(shuju, 0, 3);

            hui = ReceiveCommand();

            if (hui[0] == 0xAB)
            {
                zheng[0] = hui[1];
                xiao[0] = hui[2];

            }
            huizhuan[0] = byteToHexStr(zheng);
            huizhuan[1] = byteToHexStr(xiao);

            geinikan = Convert.ToInt16(huizhuan[0], 16);
            geinikan2 = geinikan.ToString();
            //MessageBox.Show(geinikan2);

            jiugeinikan = Convert.ToInt16(huizhuan[1], 16);
            jiugeinikan2 = jiugeinikan.ToString();
            // MessageBox.Show(jiugeinikan2);


            zheng1 = geinikan2.Substring(0, 1);
            //MessageBox.Show(zheng1);

            zheng2 = geinikan2.Substring(1, 1);
            //MessageBox.Show(zheng2);

            xiao1 = jiugeinikan2.Substring(0, 1);
            //MessageBox.Show(xiao1);

            xiao2 = jiugeinikan2.Substring(1, 1);
            //MessageBox.Show(xiao2);


            Image img1 = Image.FromFile(@"C:\数码管图片\" + zheng1 + ".jpg");
            pictureBox1.Image = img1;

            Image img2 = Image.FromFile(@"C:\数码管图片\" + zheng2 + ".jpg");
            pictureBox2.Image = img2;

            Image img3 = Image.FromFile(@"C:\数码管图片\" + xiao1 + ".jpg");
            pictureBox3.Image = img3;

            Image img4 = Image.FromFile(@"C:\数码管图片\" + xiao2 + ".jpg");
            pictureBox4.Image = img4;

            pictureBox5.Image = null;

            label7.Text = "分";
            label8.Text = "秒";
        
        }

        private void button15_Click(object sender, EventArgs e)
        {
            byte[] hui = new byte[3];
            byte[] zheng = new byte[1];
            byte[] xiao = new byte[1];
            string[] huizhuan = new string[2];
            string zheng1, zheng2, xiao1, xiao2;
            char[] tongbuzi = new char[3];

           // string xianshi;
            int geinikan;
            string geinikan2;
            int jiugeinikan;
            string jiugeinikan2;

            tongbuzi[0] = 'C';
            tongbuzi[1] = 'O';
            tongbuzi[2] = 'M';

            byte[] shuju = new byte[3];
            shuju[0] = 0xAC;
            shuju[1] = 0xff;
            shuju[2] = 0xff;

            port1.Write(tongbuzi, 0, 3);
            port1.Write(shuju, 0, 3);

            hui = ReceiveCommand();

            if (hui[0] == 0xAC)
            {
                zheng[0] = hui[1];
                xiao[0] = hui[2];

            }
            huizhuan[0] = byteToHexStr(zheng);
            huizhuan[1] = byteToHexStr(xiao);

            geinikan = Convert.ToInt16(huizhuan[0], 16);
            geinikan2 = geinikan.ToString();
            //MessageBox.Show(geinikan2);

            jiugeinikan = Convert.ToInt16(huizhuan[1], 16);
            jiugeinikan2 = jiugeinikan.ToString();
            // MessageBox.Show(jiugeinikan2);

            zheng1 = geinikan2.Substring(0, 1);
            //MessageBox.Show(zheng1);

            zheng2 = geinikan2.Substring(1, 1);
            //MessageBox.Show(zheng2);

            xiao1 = jiugeinikan2.Substring(0, 1);
            //MessageBox.Show(xiao1);

            xiao2 = jiugeinikan2.Substring(1, 1);
            //MessageBox.Show(xiao2);

            Image img1 = Image.FromFile(@"C:\数码管图片\" + zheng1 + ".jpg");
            pictureBox1.Image = img1;

            Image img2 = Image.FromFile(@"C:\数码管图片\" + zheng2 + ".jpg");
            pictureBox2.Image = img2;

            Image img3 = Image.FromFile(@"C:\数码管图片\" + xiao1 + ".jpg");
            pictureBox3.Image = img3;

            Image img4 = Image.FromFile(@"C:\数码管图片\" + xiao2 + ".jpg");
            pictureBox4.Image = img4;

            pictureBox5.Image = null;

            label7.Text = "时";
            label8.Text = "分";
        
        
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //byte[] hui = new byte[3];
           // byte[] zheng = new byte[1];
           // byte[] xiao = new byte[1];
            char[] tongbuzi = new char[3];
            int chu = 0;
            char[] shujuhui = new char[3];
            //  string[] huizhuan = new string[2];
          //  string zheng1, zheng2, xiao1, xiao2;

          //  string xianshi;
          //  int geinikan;
         //   string geinikan2;
        //    int jiugeinikan;
         //   string jiugeinikan2;

            tongbuzi[0] = 'C';
            tongbuzi[1] = 'O';
            tongbuzi[2] = 'M';

            byte[] shuju = new byte[3];
            shuju[0] = 0xAE;
            shuju[1] = 0xff;
            shuju[2] = 0xff;

            port1.Write(tongbuzi, 0, 3);
            port1.Write(shuju, 0, 3);

           // hui = ReceiveCommand();

          
            //string bijiao;
            int state = 0;
            while (chu == 0)
            {

                switch (state)
                {
                    case 0:
                        //caozuozi[0] = (char)port1.ReadChar();
                        if (MySS.data[0] == 'C')
                            state = 1;
                        else state = 0;
                        break;
                    case 1:
                        //caozuozi[1] = (char)port1.ReadChar();
                        if (MySS.data[1] == 'O')
                            state = 2;
                        else state = 0;
                        break;
                    case 2:
                        //caozuozi[2] = (char)port1.ReadChar();
                        if (MySS.data[2] == 'M')
                            state = 3;
                        //else state = 0;
                        break;
                    case 3:
                        shujuhui[0] = (Char)MySS.data[3];
                        shujuhui[1] = (Char)MySS.data[4];
                        shujuhui[2] = (Char)MySS.data[5];
                        state = 0;
                        chu = 1;
                        break;
                }

            }
           
            if (shujuhui[0] == 0xAE)
            {
                if ((shujuhui[1] & 0x80) != 0)
                {
                    Image img6 = Image.FromFile(@"C:\数码管图片\dot.jpg");
                    pictureBox6.Image = img6;
                }
                else
                {
                    pictureBox6.Image = null;
                   
                }

                if ((shujuhui[1] & 0x40) != 0)
                {
                    Image img7 = Image.FromFile(@"C:\数码管图片\dot.jpg");
                    pictureBox7.Image = img7;
                }
                else
                {
                    pictureBox7.Image = null;

                }

                if ((shujuhui[1] & 0x20) != 0)
                {
                    Image img8 = Image.FromFile(@"C:\数码管图片\dot.jpg");
                    pictureBox8.Image = img8;
                }
                else
                {
                    pictureBox7.Image = null;

                }
            
            }

          




        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
            label7.Text = "";
            label8.Text = "";
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label12.Text = DateTime.Now.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }



    


    }
}
