using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ImageEvolution
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bitmap firstImage, secondImage;
        ImageComparator imageComparator;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            secondImage = new Bitmap(ImageOpener.ReadImageFromFile());
            this.geneticImage.Source = Bitmap2BitmapImage(secondImage);
            /*
            b = new Bitmap(1, 1);
            b.SetPixel(0, 0, System.Drawing.Color.Red);
            var result = new Bitmap(b, 1024, 1024);

            b = result;

            this.wynik.Source = Bitmap2BitmapImage(result);
            */
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            firstImage = new Bitmap(ImageOpener.ReadImageFromFile());
            this.originalImage.Source = Bitmap2BitmapImage(firstImage);
            /*
            Graphics g = Graphics.FromImage(b);

            PointF po1 = new PointF(270f, 130f);
            PointF po2 = new PointF(690f, 640f);
            PointF po3 = new PointF(870f, 150f);

            PointF[] test = new PointF[] { po1, po2, po3 };

            System.Drawing.Brush brush = new SolidBrush(System.Drawing.Color.FromArgb(100, 0, 255, 0));
            g.FillPolygon(brush, test);
            g.Dispose();
            this.wynik.Source = Bitmap2BitmapImage(b);
            */
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            imageComparator = new ImageComparator(firstImage.Height, firstImage.Height);

          porownanie.Content = imageComparator.CompareImages(firstImage, secondImage);



        }

        private BitmapSource Bitmap2BitmapImage(Bitmap bitmap)
        {

            BitmapSource i = Imaging.CreateBitmapSourceFromHBitmap(
                           bitmap.GetHbitmap(),
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());
            return i;
        }

        
    }
}
