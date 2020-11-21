using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Puzzle01
{

	public partial class MainWindow 
    {
        private void Create_ImageArray(){

            BitmapImage bitmapimageOriginal = new BitmapImage(new Uri(filename, UriKind.RelativeOrAbsolute));
//            bitmapimageOriginal.CacheOption = BitmapCacheOption.OnLoad; 

            // BitmapImageのPixelFormatをPbgra32に変換する
            bitmap = new FormatConvertedBitmap(bitmapimageOriginal, PixelFormats.Pbgra32, null, 0);

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var tileWidth = width / 4;
            var tileHeight = height / 4;
            byte[] originalPixels = new byte[width * height * 4];
            byte[] tile00 = new byte[tileWidth * tileHeight * 4];
            byte[] tile01 = new byte[tileWidth * tileHeight * 4];
            byte[] tile02 = new byte[tileWidth * tileHeight * 4];
            byte[] tile03 = new byte[tileWidth * tileHeight * 4];
            byte[] tile10 = new byte[tileWidth * tileHeight * 4];
            byte[] tile11 = new byte[tileWidth * tileHeight * 4];
            byte[] tile12 = new byte[tileWidth * tileHeight * 4];
            byte[] tile13 = new byte[tileWidth * tileHeight * 4];
            byte[] tile20 = new byte[tileWidth * tileHeight * 4];
            byte[] tile21 = new byte[tileWidth * tileHeight * 4];
            byte[] tile22 = new byte[tileWidth * tileHeight * 4];
            byte[] tile23 = new byte[tileWidth * tileHeight * 4];
            byte[] tile30 = new byte[tileWidth * tileHeight * 4];
            byte[] tile31 = new byte[tileWidth * tileHeight * 4];
            byte[] tile32 = new byte[tileWidth * tileHeight * 4];
            byte[] tile33 = new byte[tileWidth * tileHeight * 4];
            // BitmapSourceから配列にコピー
            int stride = (width * bitmap.Format.BitsPerPixel + 7) / 8;
            bitmap.CopyPixels(originalPixels, stride, 0);
//            filename1.Text = tileHeight.ToString();
            // tile00 ～ tile03 作成
            for(int y = 0; y < tileHeight; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    int n = x + y * width ;
                    int o = n + y * width * 3 ;
                    int p = width * height ;
                    tile00[n] = originalPixels[o];
                    tile01[n] = originalPixels[o + width];
                    tile02[n] = originalPixels[o + width * 2];
                    tile03[n] = originalPixels[o + width * 3];
                    tile10[n] = originalPixels[o + p ];
                    tile11[n] = originalPixels[o + p + width ];
                    tile12[n] = originalPixels[o + p + width * 2];
                    tile13[n] = originalPixels[o + p + width * 3];
                    tile20[n] = originalPixels[o + p * 2];
                    tile21[n] = originalPixels[o + p * 2 + width ];
                    tile22[n] = originalPixels[o + p * 2 + width * 2];
                    tile23[n] = originalPixels[o + p * 2 + width * 3];
                    tile30[n] = originalPixels[o + p * 3];
                    tile31[n] = originalPixels[o + p * 3 + width ];
                    tile32[n] = originalPixels[o + p * 3 + width * 2];
                    tile33[n] = originalPixels[o + p * 3 + width * 3];
                }
            }

            // ウィンドウに表示する
            BitmapSource originalBitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, originalPixels, stride);
            image.Source = originalBitmap;
            int tileStride = (tileWidth * bitmap.Format.BitsPerPixel + 7) / 8;
            BitmapSource tile00Bitmap = BitmapSource.Create(tileWidth, tileHeight, 96, 96, PixelFormats.Pbgra32, null, tile00, tileStride);
            BitmapSource tile01Bitmap = BitmapSource.Create(tileWidth, tileHeight, 96, 96, PixelFormats.Pbgra32, null, tile01, tileStride);
            BitmapSource tile02Bitmap = BitmapSource.Create(tileWidth, tileHeight, 96, 96, PixelFormats.Pbgra32, null, tile02, tileStride);
            BitmapSource tile03Bitmap = BitmapSource.Create(tileWidth, tileHeight, 96, 96, PixelFormats.Pbgra32, null, tile03, tileStride);
            BitmapSource tile10Bitmap = BitmapSource.Create(tileWidth, tileHeight, 96, 96, PixelFormats.Pbgra32, null, tile10, tileStride);
            BitmapSource tile11Bitmap = BitmapSource.Create(tileWidth, tileHeight, 96, 96, PixelFormats.Pbgra32, null, tile11, tileStride);
            BitmapSource tile12Bitmap = BitmapSource.Create(tileWidth, tileHeight, 96, 96, PixelFormats.Pbgra32, null, tile12, tileStride);
            BitmapSource tile13Bitmap = BitmapSource.Create(tileWidth, tileHeight, 96, 96, PixelFormats.Pbgra32, null, tile13, tileStride);
            BitmapSource tile20Bitmap = BitmapSource.Create(tileWidth, tileHeight, 96, 96, PixelFormats.Pbgra32, null, tile20, tileStride);
            BitmapSource tile21Bitmap = BitmapSource.Create(tileWidth, tileHeight, 96, 96, PixelFormats.Pbgra32, null, tile21, tileStride);
            BitmapSource tile22Bitmap = BitmapSource.Create(tileWidth, tileHeight, 96, 96, PixelFormats.Pbgra32, null, tile22, tileStride);
            BitmapSource tile23Bitmap = BitmapSource.Create(tileWidth, tileHeight, 96, 96, PixelFormats.Pbgra32, null, tile23, tileStride);
            BitmapSource tile30Bitmap = BitmapSource.Create(tileWidth, tileHeight, 96, 96, PixelFormats.Pbgra32, null, tile30, tileStride);
            BitmapSource tile31Bitmap = BitmapSource.Create(tileWidth, tileHeight, 96, 96, PixelFormats.Pbgra32, null, tile31, tileStride);
            BitmapSource tile32Bitmap = BitmapSource.Create(tileWidth, tileHeight, 96, 96, PixelFormats.Pbgra32, null, tile32, tileStride);
            BitmapSource tile33Bitmap = BitmapSource.Create(tileWidth, tileHeight, 96, 96, PixelFormats.Pbgra32, null, tile33, tileStride);
//            image00.Source = tile00Bitmap;
            var imageName = "tile00";  // OK
//            var sourceName = imageName+"Bitmap"; //NG
            tile_display(imageName, tile00Bitmap, 0, 0);
            tile_display("tile01", tile01Bitmap, 0, 1);
            tile_display("tile02", tile02Bitmap, 0, 2);
            tile_display("tile03", tile03Bitmap, 0, 3);
            tile_display("tile10", tile10Bitmap, 1, 0);
            tile_display("tile11", tile11Bitmap, 1, 1);
            tile_display("tile12", tile12Bitmap, 1, 2);
            tile_display("tile13", tile13Bitmap, 1, 3);
            tile_display("tile20", tile20Bitmap, 2, 0);
            tile_display("tile21", tile21Bitmap, 2, 1);
            tile_display("tile22", tile22Bitmap, 2, 2);
            tile_display("tile23", tile23Bitmap, 2, 3);
            tile_display("tile30", tile30Bitmap, 3, 0);
            tile_display("tile31", tile31Bitmap, 3, 1);
            tile_display("tile32", tile32Bitmap, 3, 2);
            tile_display("tile33", tile33Bitmap, 3, 3);
        }
        private void tile_display(string imageName, BitmapSource sourceName, int y, int x)
        {
            var Target = (Image)LogicalTreeHelper.FindLogicalNode(canvas, imageName);
            var elementNew = new Image();
            elementNew.Name = imageName;
            elementNew.Source = sourceName;
            Canvas.SetLeft(elementNew, OFFSET + x * (WIDTH+1));
            Canvas.SetTop(elementNew, OFFSET + y * (HEIGHT+1));
            canvas.Children.Add(elementNew);
        }
	}
}