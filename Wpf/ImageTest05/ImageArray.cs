using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;

namespace ImageTest05
{

	public partial class MainWindow 
    {
        private void Create_ImageArray(){

            BitmapImage bitmapimageOriginal = new BitmapImage(new Uri(filename, UriKind.RelativeOrAbsolute));
//            bitmapimageOriginal.CacheOption = BitmapCacheOption.OnLoad; 

            // BitmapImageのPixelFormatをPbgra32に変換する
            bitmap = new FormatConvertedBitmap(bitmapimageOriginal, PixelFormats.Pbgra32, null, 0);

            width = bitmap.PixelWidth;
            height = bitmap.PixelHeight;
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
            /*
            byte[,] data = new byte[width, height];
            byte[,] dataR = new byte[width, height];
            byte[,] dataG = new byte[width, height];
            byte[,] dataB = new byte[width, height];
            byte[] grayF = new byte[width * height * 4];
            byte[] redF = new byte[width * height * 4];
            byte[] greenF = new byte[width * height * 4];
            byte[] blueF = new byte[width * height * 4];
            byte[] colorF = new byte[width * height * 4];
            */

            // LookUpTable作成
            Create_LookupTable(ref lookUp, effect, threshold, gamma);
            Create_LookupTable(ref lookUpR, effectR, thresholdR, gammaR);
            Create_LookupTable(ref lookUpG, effectG, thresholdG, gammaG);
            Create_LookupTable(ref lookUpB, effectB, thresholdB, gammaB);
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
            /*
            // data画像作成
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    // grey2から格納
                    data[j, i] = (byte)gray2[(i * width + j) * 4];
                    // redから格納
                    dataR[j, i] = (byte)red[(i * width + j) * 4 + 2];
                    // redから格納
                    dataG[j, i] = (byte)green[(i * width + j) * 4 + 1];
                    // redから格納
                    dataB[j, i] = (byte)blue[(i * width + j) * 4];
                }
            }
            // FFT -> Filtering -> IFFT
            byte[,] filterdata_2D = FrequencyFiltering(ref data, 0);
            byte[,] filterdata_2DR = FrequencyFiltering(ref dataR, 1);
            byte[,] filterdata_2DG = FrequencyFiltering(ref dataG, 2);
            byte[,] filterdata_2DB = FrequencyFiltering(ref dataB, 3);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int n = (i * width + j) * 4;
            // red画像作成
                    redF[n] = 0;
                    redF[n + 1] = 0;
                    redF[n + 2] = filterdata_2DR[j, i];
                    redF[n + 3] = 255;
            // green画像作成
                    greenF[n] = 0;
                    greenF[n + 1] = filterdata_2DG[j, i];
                    greenF[n + 2] = 0;
                    greenF[n + 3] = 255;
            // blue画像作成
                    blueF[n] = filterdata_2DB[j, i];
                    blueF[n + 1] = 0;
                    blueF[n + 2] = 0;
                    blueF[n + 3] = 255;
            // gray画像作成
                    grayF[n] = filterdata_2D[j, i];
                    grayF[n + 1] = filterdata_2D[j, i];
                    grayF[n + 2] = filterdata_2D[j, i];
                    grayF[n + 3] = 255;
            // color画像作成
                    colorF[n] = filterdata_2DB[j, i];
                    colorF[n + 1] = filterdata_2DG[j, i];
                    colorF[n + 2] = filterdata_2DR[j, i];
                    colorF[n + 3] = 255;
                }
            }
            */
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
/*
            // FFT出力BitmapSource(outputBitmap)を作る
            BitmapSource redFBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, redF, stride);
            imageRedF.Source = redFBitmap;
            BitmapSource greenFBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, greenF, stride);
            imageGreenF.Source = greenFBitmap;
            BitmapSource blueFBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, blueF, stride);
            imageBlueF.Source = blueFBitmap;
            BitmapSource grayFBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, grayF, stride);
            imageGrayF.Source = grayFBitmap;
            BitmapSource colorFBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, colorF, stride);
            imageColorF.Source = colorFBitmap;
*/
            // 出力path
//            outputPath = @"C:\Image\NewImage\test.png";
//            if(output == true){
            // 出力BitmapSource(outputBitmap)を作る
                outputBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, colorNew, stride);
/*
            // BitmapSourceを保存する
                using (Stream stream = new FileStream(outputPath, FileMode.Create))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(outputBitmap));
                    encoder.Save(stream);
                }
*/
//            }    


        }
	}
}