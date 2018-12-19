using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Imaging.Filters;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using System.IO; //Seri port için
using System.IO.Ports; // port aktif etme


namespace WindowsFormsApplication41
{
    public partial class Form1 : Form

    {
        private FilterInfoCollection VideoCapTureDevices;
        private VideoCaptureDevice CurrentDevices;
        private string CurrentCaptureDevice { get; set; }

        public Form1()
        {
            InitializeComponent();
            LoadKamera(); 
        }

           private void LoadKamera()
        {
               // Kamerayı aktif etmek için
               VideoCapTureDevices = new AForge.Video.DirectShow.FilterInfoCollection(AForge.Video.DirectShow.FilterCategory.VideoInputDevice);          
            foreach (AForge.Video.DirectShow.FilterInfo VideoCaptureDevice in VideoCapTureDevices)
            {
                Kamera.Items.Add(VideoCaptureDevice.Name);
            }
            Kamera.SelectedIndex = 0;
        }

           private void Sec_Click(object sender, AForge.Video.NewFrameEventArgs eventArgs)
           {
               Bitmap image = (Bitmap)eventArgs.Frame.Clone();
               Bitmap image1 = (Bitmap)eventArgs.Frame.Clone();
               
               pictureBox1.Image = image;
               Sec.Enabled = true;
               if (true)
               {
                   AForge.Imaging.Filters.EuclideanColorFiltering filter = new AForge.Imaging.Filters.EuclideanColorFiltering();
                   filter.CenterColor = new AForge.Imaging.RGB(Color.FromArgb(215, 0, 0));
                   filter.Radius = 100;
                   filter.ApplyInPlace(image1);
                   nesnebul(image1);
               }
           }
           

          
           void Finalvideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
           {

               Bitmap image = (Bitmap)eventArgs.Frame.Clone();
               Bitmap image1 = (Bitmap)eventArgs.Frame.Clone();
               Mirror filter1 = new Mirror(false, true); //aynalama
               filter1.ApplyInPlace(image);
               filter1.ApplyInPlace(image1);
               pictureBox1.Image = image;
               if (radioButton1.Checked)
               {

                   // create filter
                   EuclideanColorFiltering filter = new EuclideanColorFiltering();
                   // set center colol and radius
                   filter.CenterColor = new RGB(Color.FromArgb(215, 0, 0));
                   filter.Radius = 100;
                   // apply the filter
                   filter.ApplyInPlace(image1);


                    nesnebul(image1);

               }

               if (radioButton2.Checked)
               {

                   // create filter
                   EuclideanColorFiltering filter = new EuclideanColorFiltering();
                   // set center color and radius
                   filter.CenterColor = new RGB(Color.FromArgb(30, 144, 255));
                   filter.Radius = 100;
                   // apply the filter
                   filter.ApplyInPlace(image1);

                   nesnebul(image1);

               }
               if (radioButton3.Checked)
               {

                   // create filter
                   EuclideanColorFiltering filter = new EuclideanColorFiltering();
                   // set center color and radius
                   filter.CenterColor = new RGB(Color.FromArgb(0, 215, 0));
                   filter.Radius = 100;
                   // apply the filter
                   filter.ApplyInPlace(image1);

                   nesnebul(image1);
               }
              
           }
                 public void nesnebul(Bitmap image)
        {
            BlobCounter blobCounter = new BlobCounter();
            blobCounter.MinWidth = 5;
            blobCounter.MinHeight = 5;
            blobCounter.FilterBlobs = true;
            blobCounter.ObjectsOrder = ObjectsOrder.Size;
           

            System.Drawing.Imaging.BitmapData objectsData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, image.PixelFormat);
            // grayscaling
            Grayscale grayscaleFilter = new Grayscale(0.2125, 0.7154, 0.0721);
            UnmanagedImage grayImage = grayscaleFilter.Apply(new UnmanagedImage(objectsData));
            // unlock image
            image.UnlockBits(objectsData);


            blobCounter.ProcessImage(image);
            Rectangle[] rect = blobCounter.GetObjectsRectangles();
            Blob[] blobs = blobCounter.GetObjectsInformation();
            pictureBox2.Image = image;

            if (radioButton4.Checked)
            {
                //Nesne Takibi

                foreach (Rectangle recs in rect)
                {
                    if (rect.Length > 0)
                    {
                        Rectangle objectRect = rect[0];
                        Graphics g = Graphics.FromImage(image);
                       
                        using (Pen pen = new Pen(Color.FromArgb(252, 3, 26), 2))
                        {
                            g.DrawRectangle(pen, objectRect);
                        }
                        //Cizdirilen Dikdörtgenin Koordinatlari aliniyor.
                        int objectX = objectRect.X + (objectRect.Width / 2);
                        int objectY = objectRect.Y + (objectRect.Height / 2);
                        
                        int uzunluk = image.Width;
                        int genislik = image.Height;
                        if (objectX < 213 && objectY < 160)
                        {
                            Console.WriteLine("1.Ledi Yak");
                            serialPort1.Write("1");
                        }
                        if (objectX > 213 && objectX < 416 && objectY < 160)
                        {
                            Console.WriteLine("2.Ledi Yak");
                            serialPort1.Write("2");
                        }
                        if (objectX > 416 && objectX < 640 && objectY < 160)
                        {
                            Console.WriteLine("3.Ledi Yak");
                            serialPort1.Write("3");
                        }
                        if (objectX < 213 && objectY > 160 && objectY < 320)
                        {
                            Console.WriteLine("4.Ledi Yak");
                            serialPort1.Write("4");
                        }
                        if (objectX > 213 && objectX < 416 && objectY > 160 && objectY < 320)
                        {
                            Console.WriteLine("5.Ledi Yak");
                            serialPort1.Write("5");
                        }
                        if (objectX > 416 && objectX < 640 && objectY > 160 && objectY < 320)
                        {
                            Console.WriteLine("6.Ledi Yak");
                            serialPort1.Write("6");
                        }
                        if (objectX < 213 && objectY > 320 && objectY < 480)
                        {
                            Console.WriteLine("7.Ledi Yak");
                            serialPort1.Write("7");
                        }
                        if (objectX > 213 && objectX < 416 && objectY > 320 && objectY < 480)
                        {
                            Console.WriteLine("8.Ledi Yak");
                            serialPort1.Write("8");
                        }
                        if (objectX > 416 && objectX < 640 && objectY > 320 && objectY < 480)
                        {
                            Console.WriteLine("9.Ledi Yak");
                            serialPort1.Write("9");
                        }
                       
                        g.Dispose();







                        if (checkBox1.Checked)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                richTextBox1.Text = objectRect.Location.ToString() + "\n" + richTextBox1.Text + "\n"; ;
                            });
                        }
                    }

                }
            }
            
           }

           public VideoCaptureDevice Finalvideo { get; set; }

        Bitmap bmp;
        private void Sec_Click(object sender, EventArgs e)
        {
            Finalvideo = new VideoCaptureDevice(VideoCapTureDevices[Kamera.SelectedIndex].MonikerString);
            Finalvideo.NewFrame += new NewFrameEventHandler(Finalvideo_NewFrame);
            Finalvideo.DesiredFrameRate = 30;//saniyede kaç görüntü alsın istiyorsanız. FPS
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
          Finalvideo.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = SerialPort.GetPortNames();
        }

      
        private void button3_Click(object sender, EventArgs e)
        {
            serialPort1.BaudRate = 9600;
            serialPort1.PortName = comboBox1.SelectedItem.ToString();
            serialPort1.Open();
            if (serialPort1.IsOpen)
            {
                label1.Text = "Port Açık";    
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            serialPort1.Close();
            label1.Text = "Port Kapalı";
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
    
            Finalvideo.Stop();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Finalvideo.IsRunning)
            {
                Finalvideo.Stop();

            }

            Application.Exit();
        }
    }
    }

