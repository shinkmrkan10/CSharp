using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;

namespace ImageTest05
{
	public partial class MainWindow : Window
    {
        public ObservableCollection<FileName> DataFileName { get; set; }
        public Dictionary<string, string> ContDic { get; set; }
        public Dictionary<string, string> ContImageDic { get; set; }
        public string filename = "C:/Image/horse2Q.jpg";
        public string outputPath = @"C:\Image\NewImage\test.png";
        public int effect  = 0;
        public int effectR = 0;
        public int effectG = 0;
        public int effectB = 0;
        public byte threshold  = 100;
        public byte thresholdR = 100;
        public byte thresholdG = 100;
        public byte thresholdB = 100;
        public string component = "gray2";
        public double gamma  = 1.0;
        public double gammaR = 1.0;
        public double gammaG = 1.0;
        public double gammaB = 1.0;
        public MainWindow()
        {
            ContDic = new Dictionary<string, string>()
            {
                { "0", "グレースケール" },
                { "1", "2値化" },
                { "2", "ネガポジ反転" },
                { "3", "暗部強調" },
                { "4", "明部強調" },
                { "5", "中間部強調" },
                { "6", "γ変換" },
            };
            ContImageDic = new Dictionary<string, string>()
            {
                { "gray2", "グレースケール画像" },
                { "red", "R成分画像" },
                { "green", "G成分画像" },
                { "blue", "B成分画像" },
            };
            string[] files = System.IO.Directory.GetFiles(@"C:\Image\", "*.*");
            foreach (string s in files)
            {
                System.IO.FileInfo fi = null;
                try
                {
                fi = new System.IO.FileInfo(s);
//				DataFileName.Add(new FileName{fname = s});
                }
                catch (System.IO.FileNotFoundException)
                {
                    continue;
                }
            }

            InitializeComponent();
            FileNameList fileNameList = new FileNameList();
            comboBox.DataContext = fileNameList.DataFileName;
            comboBoxCont.DataContext  = ContDic;
            comboBoxContR.DataContext = ContDic;
            comboBoxContG.DataContext = ContDic;
            comboBoxContB.DataContext = ContDic;
            slider.Value  = 100;
            sliderCon.Value  = 1.0;
            sliderConR.Value = 1.0;
            sliderConG.Value = 1.0;
            sliderConB.Value = 1.0;
            Threshold_Check();
        }

        private void textBoxBin_TextChanged(object sender, EventArgs e)
        {
            threshold  = (byte)Int32.Parse(textBoxBin.Text);
            thresholdR = (byte)Int32.Parse(textBoxBin.Text);
            thresholdG = (byte)Int32.Parse(textBoxBin.Text);
            thresholdB = (byte)Int32.Parse(textBoxBin.Text);
            gamma  = Double.Parse(textBoxCon.Text);
            gammaR = Double.Parse(textBoxCon.Text);
            gammaG = Double.Parse(textBoxCon.Text);
            gammaB = Double.Parse(textBoxCon.Text);
            sliderR.Value = thresholdR;
            sliderG.Value = thresholdG;
            sliderB.Value = thresholdB;
            sliderCon.Value  = gamma;
            sliderConR.Value = gammaR;
            sliderConG.Value = gammaG;
            sliderConB.Value = gammaB;
            Threshold_Check();
        }

        private void textBoxBinR_TextChanged(object sender, EventArgs e)
        {
            thresholdR = (byte)Int32.Parse(textBoxBinR.Text);
            gammaR = Double.Parse(textBoxConR.Text);
            Threshold_Check();
        }

        private void textBoxBinG_TextChanged(object sender, EventArgs e)
        {
            thresholdG = (byte)Int32.Parse(textBoxBinG.Text);
            gammaG = sliderConG.Value;
            Threshold_Check();
        }
        private void textBoxBinB_TextChanged(object sender, EventArgs e)
        {
            thresholdB = (byte)Int32.Parse(textBoxBinB.Text);
            gammaB = sliderConB.Value;
            Threshold_Check();
        }


        private void comboBox_SelectedValueChanged(object sender, EventArgs e) 
        {
            filename = comboBox.SelectedValue.ToString();
            Threshold_Check();
        }

        private void comboBoxCont_SelectedValueChanged(object sender, EventArgs e) 
        {
            effect  = Int32.Parse(comboBoxCont.SelectedValue.ToString());
            effectR = Int32.Parse(comboBoxCont.SelectedValue.ToString());
            effectG = Int32.Parse(comboBoxCont.SelectedValue.ToString());
            effectB = Int32.Parse(comboBoxCont.SelectedValue.ToString());
            comboBoxContR.SelectedValue = effect;
            comboBoxContG.SelectedValue = effect;
            comboBoxContB.SelectedValue = effect;
            Threshold_Check();
        }
        private void comboBoxContR_SelectedValueChanged(object sender, EventArgs e) 
        {
            effectR = Int32.Parse(comboBoxContR.SelectedValue.ToString());
            Threshold_Check();
        }
        private void comboBoxContG_SelectedValueChanged(object sender, EventArgs e) 
        {
            effectG = Int32.Parse(comboBoxContG.SelectedValue.ToString());
            Threshold_Check();
        }

        private void comboBoxContB_SelectedValueChanged(object sender, EventArgs e) 
        {
            effectB = Int32.Parse(comboBoxContB.SelectedValue.ToString());
            Threshold_Check();
        }

        private void button_Click_Save(object sender, RoutedEventArgs e)
        {
            string outputName = textBoxSave.Text;
            outputPath = @"C:\Image\NewImage\" + outputName + ".png";
            Create_ImageArray(filename, true);
        }

        private void button_Click_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Threshold_Check(){
            bool repeat = true;
            while (repeat)
            {
                try{
                    Create_ImageArray(filename, false);
                    textBoxBin.DataContext  = threshold;
                    textBoxBinR.DataContext = thresholdR;
                    textBoxBinG.DataContext = thresholdG;
                    textBoxBinB.DataContext = thresholdB;
                    textBoxCon.DataContext  = gamma;
                    textBoxConR.DataContext = gammaR;
                    textBoxConG.DataContext = gammaG;
                    textBoxConB.DataContext = gammaB;
                    repeat = false;
                }
                catch (FormatException)
                {
                    slider.Value   = 100;
                    sliderR.Value  = 100;
                    sliderG.Value  = 100;
                    sliderB.Value  = 100;
                    sliderCon.Value   = 1.0;
                    sliderConR.Value  = 1.0;
                    sliderConG.Value  = 1.0;
                    sliderConB.Value  = 1.0;
                }
            }
        }

        private void Create_ImageArray(string filename, bool output=false){

            BitmapImage bitmapimageOriginal = new BitmapImage(new Uri(filename, UriKind.RelativeOrAbsolute));

            // BitmapImageのPixelFormatをPbgra32に変換する
            FormatConvertedBitmap bitmap = new FormatConvertedBitmap(bitmapimageOriginal, PixelFormats.Pbgra32, null, 0);

            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            byte[] originalPixels = new byte[width * height * 4];
            byte[] red = new byte[width * height * 4];
            byte[] green = new byte[width * height * 4];
            byte[] blue = new byte[width * height * 4];
            byte[] redNew = new byte[width * height * 4];
            byte[] greenNew = new byte[width * height * 4];
            byte[] blueNew = new byte[width * height * 4];
            byte[] colorNew = new byte[width * height * 4];
            byte[] gray2 = new byte[width * height * 4];
            byte[] bi = new byte[width * height * 4];
            byte[] lookUp  = new byte[256];
            byte[] lookUpR = new byte[256];
            byte[] lookUpG = new byte[256];
            byte[] lookUpB = new byte[256];

            // LookUpTable作成
            Create_LookupTable(lookUp, effect, threshold, gamma);
            Create_LookupTable(lookUpR, effectR, thresholdR, gammaR);
            Create_LookupTable(lookUpG, effectG, thresholdG, gammaG);
            Create_LookupTable(lookUpB, effectB, thresholdB, gammaB);
            // BitmapSourceから配列にコピー
            int stride = (width * bitmap.Format.BitsPerPixel + 7) / 8;
            bitmap.CopyPixels(originalPixels, stride, 0);
            // RGB要素を抜き出す
            for (int x = 0; x < originalPixels.Length; x = x + 4)
            {
            // red画像作成
                red[x] = 0;
                red[x + 1] = 0;
                red[x + 2] = originalPixels[x + 2];
                red[x + 3] = 255;
            // green画像作成
                green[x] = 0;
                green[x + 1] = originalPixels[x + 1];
                green[x + 2] = 0;
                green[x + 3] = 255;
            // blue画像作成
                blue[x] = originalPixels[x];
                blue[x + 1] = 0;
                blue[x + 2] = 0;
                blue[x + 3] = 255;
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

            // 画素変換:LookUpTable利用
                byte y3 = (byte)lookUp[y2];
                bi[x] = y3;
                bi[x + 1] = y3;
                bi[x + 2] = y3;
                bi[x + 3] = 255;
            // redNew画像作成
                y3 = (byte)lookUpR[red[x + 2]];
                redNew[x] = 0;
                redNew[x + 1] = 0;
                redNew[x + 2] = y3;
                redNew[x + 3] = 255;
            // greenNew画像作成
                y3 = (byte)lookUpG[green[x + 1]];
                greenNew[x] = 0;
                greenNew[x + 1] = y3;
                greenNew[x + 2] = 0;
                greenNew[x + 3] = 255;
            // blueNew画像作成
                y3 = (byte)lookUpB[blue[x]];
                blueNew[x] = y3;
                blueNew[x + 1] = 0;
                blueNew[x + 2] = 0;
                blueNew[x + 3] = 255;
            // colorNew画像作成
                colorNew[x] = blueNew[x];
                colorNew[x + 1] = greenNew[x + 1];
                colorNew[x + 2] = redNew[x + 2];
                colorNew[x + 3] = 255;
            }

            // ウィンドウに表示する
            BitmapSource originalBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, originalPixels, stride);
            image.Source = originalBitmap;
            BitmapSource redBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, red, stride);
            imageRed.Source = redBitmap;
            BitmapSource greenBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, green, stride);
            imageGreen.Source = greenBitmap;
            BitmapSource blueBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, blue, stride);
            imageBlue.Source = blueBitmap;
            BitmapSource redNewBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, redNew, stride);
            imageRedNew.Source = redNewBitmap;
            BitmapSource greenNewBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, greenNew, stride);
            imageGreenNew.Source = greenNewBitmap;
            BitmapSource blueNewBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, blueNew, stride);
            imageBlueNew.Source = blueNewBitmap;
            BitmapSource colorNewBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, colorNew, stride);
            imageColorNew.Source = colorNewBitmap;
            BitmapSource gray2Bitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, gray2, stride);
            imageGray2.Source = gray2Bitmap;
            BitmapSource biBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, bi, stride);
            imageBinary.Source = biBitmap;

            // 出力BitmapSource(outputBitmap)を作る
            BitmapSource outputBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, colorNew, stride);

            // 出力path
//            outputPath = @"C:\Image\NewImage\test.png";
            if(output == true){
            // BitmapSourceを保存する
                using (Stream stream = new FileStream(outputPath, FileMode.Create))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(outputBitmap));
                    encoder.Save(stream);
                }
            }    



        }

        private void Create_LookupTable(byte[] lookUp, int effect, byte threshold, double gamma){
            // 処理選択
            switch(effect){
                case 1:
            // LookUp Table作成:2値化
                    for(int x = 0; x < threshold; x++){
                        lookUp[x] = 0;
                    }
                    for(int x = threshold; x < 256; x++){
                        lookUp[x] = 255;
                    }
                    break;
                case 2:
            // LookUp Table作成:ネガポジ反転
                    for(int x = 0; x < 256; x++){
                        lookUp[x] = (byte)(255 - x);
                    }
                    break;
                case 3:
            // LookUp Table作成:暗部強調
                    for(int x = 0; x < 128; x++){
                        lookUp[x] = (byte)(x * 2);
                    }
                    for(int x = 128; x < 256; x++){
                        lookUp[x] = 255;
                    }
                    break;
                case 4:
            // LookUp Table作成:明部強調
                    for(int x = 0; x < 128; x++){
                        lookUp[x] = 0;
                    }
                    for(int x = 128; x < 256; x++){
                        lookUp[x] = (byte)((x - 128) * 2);
                    }
                    break;
                case 5:
            // LookUp Table作成:中間部強調
                    for(int x = 0; x < 64; x++){
                        lookUp[x] = 0;
                    }
                    for(int x = 64; x < 192; x++){
                        lookUp[x] = (byte)((x - 64) * 2);
                    }
                    
                    for(int x = 192; x < 256; x++){
                        lookUp[x] = 255;
                    }
                    break;
                case 6:
            // LookUp Table作成:γ変換
                    for(int x = 0; x < 256; x++){
                        lookUp[x] = (byte)(255 * Math.Pow(((double)x / 255), (1 / gamma)));
                    }
                    break;
                default:
            // LookUp Table作成:グレースケール
                for(int x = 0; x < 256; x++){
                    lookUp[x] = (byte)x;
                }
                break;
            }
        }
    }
}
