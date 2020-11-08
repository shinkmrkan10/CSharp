namespace ImageTest05
{
	public partial class MainWindow
    {

        /// <summary>
        /// 周波数フィルタリング
        /// </summary>
        public  byte[,] FrequencyFiltering(ref byte[,] image, int filter)
        {
            try
            {
                // サイズ取得
                int xSize = image.GetLength(0);
                int ySize = image.GetLength(1);

				// x, y が 2^n 以外はそのまま返却
 				if((xSize & (xSize -1)) != 0){
					return image;
				}
				if((ySize & (ySize -1)) != 0){
					return image;
				}
  
                // double型配列にする
                double[,] data = ByteToDouble2D(image);
  
                double[,] re ;
                double[,] im = new double[xSize, ySize];
  
                // 2次元フーリエ変換
                FFT2D(data, im, out re, out im, xSize, ySize);
                /*
				// power spectram を求める
                for (int i = 0; i < ySize; i++)
                {
                    for (int j = 0; j < xSize; j++)
                    {
						pow[j, i] = Math.Sqrt(
										Math.Pow(re[j, i], 2) + 
										Math.Pow(im[j, i], 2));
					}
                }
                */
				// 象限交換
                for (int i = 0; i < ySize/2; i++)
                {
                    for (int j = 0; j < xSize/2; j++)
                    {
						double tempre = re[j, i];
						double tempim = im[j, i];
						re[j, i] = re[j + xSize/2, i + ySize/2];
						re[j + xSize/2, i + ySize/2] = tempre;
						im[j, i] = im[j + xSize/2, i + ySize/2];
						im[j + xSize/2, i + ySize/2] = tempim;
                    }
                }
                for (int i = 0; i < ySize/2; i++)
                {
                    for (int j = xSize/2; j < xSize; j++)
                    {
						double tempre = re[j, i];
						double tempim = im[j, i];
						re[j, i] = re[j - xSize/2, i + ySize/2];
						re[j - xSize/2, i + ySize/2] = tempre;
						im[j, i] = im[j - xSize/2, i + ySize/2];
						im[j - xSize/2, i + ySize/2] = tempim;
                    }
                }
                // Filtering
                double low = lowpass;
                double hei = heighpass;
                switch(filter){
                case 0:
                    low = lowpass;
                    hei = heighpass;
                    break;
                case 1:
                    low = lowpassR;
                    hei = heighpassR;
                    break;
                case 2:
                    low = lowpassG;
                    hei = heighpassG;
                    break;
                case 3:
                    low = lowpassB;
                    hei = heighpassB;
                    break;
                default:
                    break;
                }
                if(low > hei){
				// Filtering(low pass > heighpass)
                    for (int i = 0; i < ySize; i++)
                    {
                        for (int j = 0; j < xSize; j++)
                        {
				    		if(((i-ySize/2) * (i-ySize/2) + (j-xSize/2) * (j-xSize/2)) > low * low){
					    		re[j, i] = 0;
						    	im[j, i] = 0;
    						}
	    					else if(((i-ySize/2) * (i-ySize/2) + (j-xSize/2) * (j-xSize/2)) < hei * hei){
		    					re[j, i] = 0;
			    				im[j, i] = 0;
				    		}
                        }
                    }
                }
                else{
				// Filtering(heigh pass > low pass)
                    for (int i = 0; i < ySize; i++)
                    {
                        for (int j = 0; j < xSize; j++)
                        {
				    		if(((i-ySize/2) * (i-ySize/2) + (j-xSize/2) * (j-xSize/2)) < hei * hei){
    	    					if(((i-ySize/2) * (i-ySize/2) + (j-xSize/2) * (j-xSize/2)) > low * low){
	    	    					re[j, i] = 0;
		    	    				im[j, i] = 0;
			    	    		}
    						}
                        }
                    }
                }
                /*
				// power spectram を求める
                for (int i = 0; i < ySize; i++)
                {
                    for (int j = 0; j < xSize; j++)
                    {
						powFil[j, i] = Math.Sqrt(
											Math.Pow(re[j, i], 2) + 
											Math.Pow(im[j, i], 2));
                    }
                }
                */
				// 象限交換
                for (int i = 0; i < ySize/2; i++)
                {
                    for (int j = 0; j < xSize/2; j++)
                    {
						double tempre = re[j, i];
						double tempim = im[j, i];
						re[j, i] = re[j + xSize/2, i + ySize/2];
						re[j + xSize/2, i + ySize/2] = tempre;
						im[j, i] = im[j + xSize/2, i + ySize/2];
						im[j + xSize/2, i + ySize/2] = tempim;
                    }
                }
                for (int i = 0; i < ySize/2; i++)
                {
                    for (int j = xSize/2; j < xSize; j++)
                    {
						double tempre = re[j, i];
						double tempim = im[j, i];
						re[j, i] = re[j - xSize/2, i + ySize/2];
						re[j - xSize/2, i + ySize/2] = tempre;
						im[j, i] = im[j - xSize/2, i + ySize/2];
						im[j - xSize/2, i + ySize/2] = tempim;
                    }
                }

  
                // 2次元逆フーリエ変換　周波数画像からもとの空間画像に戻す
                IFFT2D(re, im, out data, out im, xSize, ySize);

  
                // byte型配列に戻す
//                image = DoubleToByte2D(re);  // FFT.real
//                image = DoubleToByte2D(pow);  // FFT.powerSpectram
//                image = DoubleToByte2D(powFil);  // Filtered.powerSpectram
                image = DoubleToByte2D(data);  // IFFT.real
//                image = DoubleToByte2D(im);  // IFFT.im = 0
    
                return image;
            }
            catch
            {
                return null;
            }
        }
  
  
        /// <summary>
        /// byte型2次元配列からdouble型2次元配列に変換
        /// </summary>
        public  double[,] ByteToDouble2D(byte[,] data)
        {
            try
            {
                // サイズ取得
                int xSize = data.GetLength(0);
                int ySize = data.GetLength(1);
    
                double[,] convdata = new double[xSize, ySize];
                for (int i = 0; i < ySize; i++)
                {
                    for (int j = 0; j < xSize; j++)
                    {
                        convdata[j, i] = (double)data[j, i];
                    }
                }
                return convdata;
            }
            catch
            {
                return null;
            }
        }
  
        /// <summary>
        /// double型2次元配列からbyte型2次元配列に変換
        /// </summary>
        public  byte[,] DoubleToByte2D(double[,] data)
        {
            try
            {
                // サイズ取得
                int xSize = data.GetLength(0);
                int ySize = data.GetLength(1);
  
  
                byte[,] convdata = new byte[xSize, ySize];
                for (int i = 0; i < ySize; i++)
                {
                    for (int j = 0; j < xSize; j++)
                    {
                        if (data[j, i] >= 255)
                        {
                            convdata[j, i] = 255;
                        }
                        else if (data[j, i] < 0)
                        {
                            convdata[j, i] = 0;
                        }
                        else
                        {
                            convdata[j, i] = (byte)data[j, i];
                        }
                    }
                }
                return convdata;
            }
            catch
            {
                return null;
            }
        }
  
  
  
        /// <summary>
        /// 1次元FFT
        /// </summary>
        public  void FFT(
            double[] inputRe,
            double[] inputIm,
            out double[] outputRe,
            out double[] outputIm,
            int bitSize)
        {
            int dataSize = 1 << bitSize;
            int[] reverseBitArray = BitScrollArray(dataSize);
  
  
  
            outputRe = new double[dataSize];
            outputIm = new double[dataSize];
  
  
  
            // バタフライ演算のための置き換え
            for (int i = 0; i < dataSize; i++)
            {
                outputRe[i] = inputRe[reverseBitArray[i]];
                outputIm[i] = inputIm[reverseBitArray[i]];
            }
  
  
  
            // バタフライ演算
            for (int stage = 1; stage <= bitSize; stage++)
            {
                int butterflyDistance = 1 << stage;
                int numType = butterflyDistance >> 1;
                int butterflySize = butterflyDistance >> 1;
  
  
  
                double wRe = 1.0;
                double wIm = 0.0;
                double uRe =
                    System.Math.Cos(System.Math.PI / butterflySize);
                double uIm =
                    -System.Math.Sin(System.Math.PI / butterflySize);
  
  
  
                for (int type = 0; type < numType; type++)
                {
                    for (int j = type; j < dataSize; j += butterflyDistance)
                    {
                        int jp = j + butterflySize;
                        double tempRe =
                            outputRe[jp] * wRe - outputIm[jp] * wIm;
                        double tempIm =
                            outputRe[jp] * wIm + outputIm[jp] * wRe;
                        outputRe[jp] = outputRe[j] - tempRe;
                        outputIm[jp] = outputIm[j] - tempIm;
                        outputRe[j] += tempRe;
                        outputIm[j] += tempIm;
                    }
                    double tempWRe = wRe * uRe - wIm * uIm;
                    double tempWIm = wRe * uIm + wIm * uRe;
                    wRe = tempWRe;
                    wIm = tempWIm;
                }
            }
        }
  
  
  
        /// <summary>
        /// 1次元IFFT
        /// </summary>
        public  void IFFT(
            double[] inputRe,
            double[] inputIm,
            out double[] outputRe,
            out double[] outputIm,
            int bitSize)
        {
            int dataSize = 1 << bitSize;
            outputRe = new double[dataSize];
            outputIm = new double[dataSize];
  
  
  
            for (int i = 0; i < dataSize; i++)
            {
                inputIm[i] = -inputIm[i];
            }
            FFT(inputRe, inputIm, out outputRe, out outputIm, bitSize);
            for (int i = 0; i < dataSize; i++)
            {
                outputRe[i] /= (double)dataSize;
                outputIm[i] /= (double)(-dataSize);
            }
        }
  
  
  
        // ビットを左右反転した配列を返す
        private  int[] BitScrollArray(int arraySize)
        {
            int[] reBitArray = new int[arraySize];
            int arraySizeHarf = arraySize >> 1;
  
  
  
            reBitArray[0] = 0;
            for (int i = 1; i < arraySize; i <<= 1)
            {
                for (int j = 0; j < i; j++)
                    reBitArray[j + i] = reBitArray[j] + arraySizeHarf;
                arraySizeHarf >>= 1;
            }
            return reBitArray;
        }
  
  
  
        /// <summary>
        /// 2次元FFT
        /// </summary>
        /// <param name="inDataR">実数入力部
        /// <param name="inDataI">虚数入力部
        /// <param name="outDataR">実数出力部
        /// <param name="outDataI">虚数出力部
        /// <param name="xSize">x方向サイズ
        /// <param name="ySize">y方向サイズ
        public  void FFT2D(double[,] inDataRe, double[,] inDataIm,
        out double[,] outDataRe, out double[,] outDataIm, int xSize, int ySize)
        {
            double[,] tempRe = new double[ySize, xSize];
            double[,] tempIm = new double[ySize, xSize];
  
  
  
            int xbit = GetBitNum(xSize);
            int ybit = GetBitNum(ySize);
  
  
  
            outDataRe = new double[xSize, ySize];
            outDataIm = new double[xSize, ySize];
  
  
  
            for (int j = 0; j < ySize; j++)
            {
                double[] re = new double[xSize];
                double[] im = new double[xSize];
                FFT(
                    GetArray(inDataRe, j),
                    GetArray(inDataIm, j),
                    out re, out im, xbit);
  
  
  
                for (int i = 0; i < xSize; i++)
                {
                    tempRe[j, i] = re[i];
                    tempIm[j, i] = im[i];
                }
            }
  
  
  
            for (int i = 0; i < xSize; i++)
            {
                double[] re = new double[ySize];
                double[] im = new double[ySize];
                FFT(
                    GetArray(tempRe, i),
                    GetArray(tempIm, i),
                    out re, out im, ybit);
  
  
  
                for (int j = 0; j < ySize; j++)
                {
                    outDataRe[i, j] = re[j];
                    outDataIm[i, j] = im[j];
                }
            }
        }
  
  
  
        // ビット数取得
        private  int GetBitNum(int num)
        {
            int bit = -1;
            while (num > 0)
            {
                num >>= 1;
                bit++;
            }
            return bit;
        }
  
  
  
        // 1次元配列取り出し
        private  double[] GetArray(double[,] data2D, int seg)
        {
            double[] reData = new double[data2D.GetLength(0)];
            for (int i = 0; i < data2D.GetLength(0); i++)
            {
                reData[i] = data2D[i, seg];
            }
            return reData;
        }
  
  
  
        /// <summary>
        /// 2次元IFFT
        /// </summary>
        /// <param name="inDataR">実数入力部
        /// <param name="inDataI">虚数入力部
        /// <param name="outDataR">実数出力部
        /// <param name="outDataI">虚数出力部
        /// <param name="xSize">x方向サイズ
        /// <param name="ySize">y方向サイズ
        public  void IFFT2D(double[,] inDataRe, 
        double[,] inDataIm, out double[,] outDataRe,
            out double[,] outDataIm,
            int xSize,
  
            int ySize)
        {
  
            double[,] tempRe = new double[ySize, xSize];
            double[,] tempIm = new double[ySize, xSize];
  
  
  
            int xbit = GetBitNum(xSize);
            int ybit = GetBitNum(ySize);
  
  
            outDataRe = new double[xSize, ySize];
            outDataIm = new double[xSize, ySize];
  
  
  
            for (int j = 0; j < ySize; j++)
            {
                double[] re = new double[xSize];
                double[] im = new double[xSize];
                IFFT(
                    GetArray(inDataRe, j),
                    GetArray(inDataIm, j),
                    out re, out im, xbit);
  
                for (int i = 0; i < xSize; i++)
                {
                    tempRe[j, i] = re[i];
                    tempIm[j, i] = im[i];
                }
            }
  
  
  
            for (int i = 0; i < xSize; i++)
            {
                double[] re = new double[ySize];
                double[] im = new double[ySize];
                IFFT(
                    GetArray(tempRe, i),
                    GetArray(tempIm, i),
                    out re, out im, ybit);
  
  
                for (int j = 0; j < ySize; j++)
                {
                    outDataRe[i, j] = re[j];
                    outDataIm[i, j] = im[j];
                }
            }
        }
    }  
}

