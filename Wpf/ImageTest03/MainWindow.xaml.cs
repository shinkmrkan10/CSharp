using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;

namespace ImageTest03
{
    public partial class MainWindow : Window
    {
        public string filename = "C:/Image/horse2Q.jpg";
        public int effect = 0;
        public MainWindow()
        {
        string[] files = System.IO.Directory.GetFiles(@"C:\Image\", "*.*");
        foreach (string s in files)
        {
            System.IO.FileInfo fi = null;
            try
            {
                fi = new System.IO.FileInfo(s);
            }
            catch (System.IO.FileNotFoundException)
            {
                continue;
            }
        }

            InitializeComponent();
            comboBox.DataContext = files;
            Threshold_Check();
        }

        private void comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string selectedItem = comboBox.SelectedItem.ToString();
            filename = selectedItem;
            textBlock2.Text = selectedItem;
            textBlock2.Text = "test";
            Threshold_Check();
        }

        private void Threshold_Check(){
            byte threshold;
            bool repeat = true;
            while (repeat)
            {
                try{
                    threshold = (byte)Int32.Parse(textBox.Text);
                    Create_ImageArray(filename, effect, threshold);
                    textBox.DataContext = threshold;
                    textBlock.DataContext = filename;
                    repeat = false;
                }
                catch (FormatException)
                {
                    textBox.Text = "100";
                }
            }
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            Threshold_Check();
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
                    filename = comboBox.SelectedItem.ToString();
                    Threshold_Check();
        }

        private void button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_Click_bin(object sender, RoutedEventArgs e)
        {
                    effect = 1;
                    Threshold_Check();
        }
        private void button_Click_rev(object sender, RoutedEventArgs e)
        {
                    effect = 2;
                    Threshold_Check();
        }
        private void button_Click_lowS(object sender, RoutedEventArgs e)
        {
                    effect = 3;
                    Threshold_Check();
        }
        private void button_Click_hiS(object sender, RoutedEventArgs e)
        {
                    effect = 4;
                    Threshold_Check();
        }
        private void button_Click_midS(object sender, RoutedEventArgs e)
        {
                    effect = 5;
                    Threshold_Check();
        }
        private void Create_ImageArray(string filename, int effect, byte threshold){

            BitmapImage bitmapimageOriginal = new BitmapImage(new Uri(filename, UriKind.RelativeOrAbsolute));

            // BitmapImageのPixelFormatをPbgra32に変換する
            FormatConvertedBitmap bitmap = new FormatConvertedBitmap(bitmapimageOriginal, PixelFormats.Pbgra32, null, 0);

            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            byte[] originalPixels = new byte[width * height * 4];
            byte[] gray2 = new byte[width * height * 4];
            byte[] bi = new byte[width * height * 4];
            byte[] lookUp = new byte[width * height * 4];

            // 処理選択
            switch(effect){
                case 1:
            // LookUp Table作成:2値化
                    for(int x = threshold; x < 255; x++){
                        lookUp[x] = 255;
                    }
                    break;
                case 2:
            // LookUp Table作成:ネガポジ反転
                    for(int x = 0; x < 255; x++){
                        lookUp[x] = (byte)(255 - x);
                    }
                    break;
                case 3:
            // LookUp Table作成:暗部強調
                    for(int x = 0; x < 127; x++){
                        lookUp[x] = (byte)(x * 2);
                    }
                    for(int x = 128; x < 255; x++){
                        lookUp[x] = 255;
                    }
                    break;
                case 4:
            // LookUp Table作成:明部強調
                    for(int x = 128; x < 255; x++){
                        lookUp[x] = (byte)((x - 128) * 2);
                    }
                    break;
                case 5:
            // LookUp Table作成:中間部強調
                    for(int x = 64; x < 191; x++){
                        lookUp[x] = (byte)((x - 64) * 2);
                    }
                    for(int x = 192; x < 255; x++){
                        lookUp[x] = 255;
                    }
                    break;
                default:
            // LookUp Table作成:グレースケール
                for(int x = 0; x < 255; x++){
                    lookUp[x] = (byte)x;
                }
                break;
            }
            // BitmapSourceから配列にコピー
            int stride = (width * bitmap.Format.BitsPerPixel + 7) / 8;
            bitmap.CopyPixels(originalPixels, stride, 0);
            // RGB要素を抜き出す
            for (int x = 0; x < originalPixels.Length; x = x + 4)
            {
            // red画像作成
//                red[x] = 0;
//                red[x + 1] = 0;
//                red[x + 2] = originalPixels[x + 2];
//                red[x + 3] = 255;
            // green画像作成
//                green[x] = 0;
//                green[x + 1] = originalPixels[x + 1];
//                green[x + 2] = 0;
//                green[x + 3] = 255;
            // blue画像作成
//                blue[x] = originalPixels[x];
//                blue[x + 1] = 0;
//                blue[x + 2] = 0;
//                blue[x + 3] = 255;
            // gray画像作成
            // γ=2.2 で最適のYIQ変換
//                byte y = (byte)(originalPixels[x + 2] * 0.299 
//                      + originalPixels[x + 1] * 0.587 
//                      + originalPixels[x] * 0.114);
//                gray[x] = y;
//                gray[x + 1] = y;
//                gray[x + 2] = y;
//                gray[x + 3] = 255;
            // γ=2.2 以外で推奨のYIQ変換
                byte y2 = (byte)(originalPixels[x + 2] * 0.3086 
                      + originalPixels[x + 1] * 0.6094 
                      + originalPixels[x] * 0.0820);
                gray2[x] = y2;
                gray2[x + 1] = y2;
                gray2[x + 2] = y2;
                gray2[x + 3] = 255;

            // 2値画像作成
            // 2値化変換:if文
            //    if(y2 > threshold){
            //        y2 = 255;
            //    }
            //    else{
            //        y2 = 0;
            //    }
            //    bi[x] = y2;
            //    bi[x + 1] = y2;
            //    bi[x + 2] = y2;
            //    bi[x + 3] = 255;
            // 画素変換:LookUpTable
                byte y3 = (byte)lookUp[y2];
                bi[x] = y3;
                bi[x + 1] = y3;
                bi[x + 2] = y3;
                bi[x + 3] = 255;

            }

            // ウィンドウに表示する
            BitmapSource originalBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, originalPixels, stride);
            image.Source = originalBitmap;
//            BitmapSource grayBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, gray, stride);
//            imageGray.Source = grayBitmap;
            BitmapSource gray2Bitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, gray2, stride);
            imageGray2.Source = gray2Bitmap;
            BitmapSource biBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, bi, stride);
            imageBinary.Source = biBitmap;
        }
    }
}
