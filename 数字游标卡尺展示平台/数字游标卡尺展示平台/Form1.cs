using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace 数字游标卡尺展示平台
{
    public partial class 数字游标卡尺展示平台 : Form
    {
        double[] shu1 = new double[1000];
        double[] shu2 = new double[1000];
        //用于标识数据序号
        int i = 0;
        //串口
        int j = 0;
        int k = 0;
        //声明一个com对象
        public SerialPort COM;
        public 数字游标卡尺展示平台()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            string[] coms = SerialPort.GetPortNames();
            foreach (string com in coms)
            {
                comboBox1.Items.Add(com);
            }
            //定义控件表头
            listView1.View = View.Details;
            ColumnHeader header1 = new ColumnHeader();
            header1.Text = "数据";
            header1.TextAlign = HorizontalAlignment.Center;
            header1.Width = 40;
            listView1.Columns.Add(header1);

            ColumnHeader header2 = new ColumnHeader();
            header2.Text = "mm/毫米";
            header2.TextAlign = HorizontalAlignment.Center;
            header2.Width = 80;
            listView1.Columns.Add(header2);

            ColumnHeader header3 = new ColumnHeader();
            header3.Text = "in/英寸";
            header3.TextAlign = HorizontalAlignment.Center;
            header3.Width = 80;
            listView1.Columns.Add(header3);

            ColumnHeader header4 = new ColumnHeader();
            header4.Text = "记录时间";
            header4.TextAlign = HorizontalAlignment.Center;
            header4.Width = 170;
            listView1.Columns.Add(header4);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int g = 0;
            if (COM == null & g == 0) 
            {
                //创建一个串口
                COM = new SerialPort(comboBox1.Text, 115200);

                COM.DataReceived += COM_DataReceived;

                COM.Open();

                button1.Text = "关闭串口";

                g = 1;
            }
            else if(g == 1)
            {
                button1.Text = "打开串口";
                g = 0;
            }
        }

        private void COM_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(10);

            int len = COM.BytesToRead;
            if (len>1)
            {
                byte[] buff = new byte[len];

                COM.Read(buff,0,len);


                //把收到的内容转换成字符串
                string content = ASCIIEncoding.ASCII.GetString(buff);

                Console.WriteLine(content);
                


                this.Invoke(new Action(
                    () => {
                        double f1 = Convert.ToSingle(content);

                        if (f1!=500.00)
                        {
                            //数据写入label控件
                            label3.Text = content;

                            double f = 2.54;

                            double f2 = f1 / f;

                            label4.Text = f2.ToString("0.00");

                            //数据写入图像控件
                            hslGauge1.Value = Convert.ToSingle(content);
                        }
                        else
                        {
                            i++;

                            double shu1 = Convert.ToSingle(content);
                            //double shu2 = f2;

                            //将数据写入控件
                            listView1.BeginUpdate();
                            ListViewItem lvi = new ListViewItem();
                            lvi.Text = "" + i;
                            lvi.SubItems.Add(label3.Text);
                            lvi.SubItems.Add(label4.Text);
                            lvi.SubItems.Add(DateTime.Now.ToLocalTime().ToString());
                            listView1.Items.Add(lvi);
                            listView1.EndUpdate();

                            label5.Visible = true;
                            // MessageBox.Show("数据保存成功");
                            timer1.Start();
                        }
                        //下面代码为备份
                        /*
                        else
                        {

                            label5.Visible = true;
                            // MessageBox.Show("数据保存成功");
                            timer1.Start();

                            i++;

                            //将数据写入控件
                            listView1.BeginUpdate();
                            ListViewItem lvi = new ListViewItem();
                            lvi.Text = "" + i;
                            lvi.SubItems.Add(shu1.ToString("0.00"));
                            lvi.SubItems.Add(shu2.ToString("0.00"));
                            lvi.SubItems.Add(DateTime.Now.ToLocalTime().ToString());
                            listView1.Items.Add(lvi);
                            listView1.EndUpdate();
                            //数据写入图像控件
                            hslGauge1.Value = (int)shu1;


                        ===================================================
                            label3.Text = content;

                            //double f1 = Convert.ToSingle(content);

                            double f = 2.54;

                            double f2 = f1 / f;

                            label4.Text = f2.ToString("0.00");

                            double shu1 = Convert.ToSingle(content);
                            double shu2 = f2;
                        }
                        */
                    }
                    ));

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label5.Visible = false;
            timer1.Stop();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {


        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            DialogResult dh = MessageBox.Show("你确定要清除数据吗","清除数据成功！",MessageBoxButtons.OKCancel,MessageBoxIcon.Exclamation);
            if(dh == DialogResult.OK)
            {
                listView1.Items.Clear();
                Array.Clear(shu1, 0, shu1.Length);
                Array.Clear(shu2, 0, shu2.Length);
                label3.Text = "";
                label4.Text = "";
            }
        }

        private void hslCurveHistory1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int j = Convert.ToInt32(textBox1.Text);
            int k = Convert.ToInt32(textBox2.Text);
            COM.WriteLine(""+j+k);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            COM.WriteLine("0090");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
