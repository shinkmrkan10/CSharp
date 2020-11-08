using System;

namespace ImageTest05
{

	public partial class MainWindow 
    {
        private void Create_LookupTable(ref byte[] lookUp, int effect, byte threshold, double gamma){
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
            // LookUp Table作成:4値化
                    for(int x = 0; x < 65; x++){
                        lookUp[x] = 0;
                    }
                    for(int x = 65; x < 128; x++){
                        lookUp[x] = 85;
                    }
                    for(int x = 128; x < 192; x++){
                        lookUp[x] = 170;
                    }
                    for(int x = 192; x < 256; x++){
                        lookUp[x] = 255;
                    }
                    break;
            // LookUp Table作成:8値化
                case 3:
                    for(int x = 0; x < 32; x++){
                        lookUp[x] = 0;
                    }
                    for(int x = 32; x < 64; x++){
                        lookUp[x] = 36;
                    }
                    for(int x = 64; x < 96; x++){
                        lookUp[x] = 73;
                    }
                    for(int x = 96; x < 128; x++){
                        lookUp[x] = 108;
                    }
                    for(int x = 128; x < 160; x++){
                        lookUp[x] = 145;
                    }
                    for(int x = 160; x < 192; x++){
                        lookUp[x] = 192;
                    }
                    for(int x = 192; x < 235; x++){
                        lookUp[x] = 235;
                    }
                    for(int x = 235; x < 256; x++){
                        lookUp[x] = 255;
                    }
                    break;
                case 4:
            // LookUp Table作成:ネガポジ反転
                    for(int x = 0; x < 256; x++){
                        lookUp[x] = (byte)(255 - x);
                    }
                    break;
                case 5:
            // LookUp Table作成:暗部強調
                    for(int x = 0; x < 128; x++){
                        lookUp[x] = (byte)(x * 2);
                    }
                    for(int x = 128; x < 256; x++){
                        lookUp[x] = 255;
                    }
                    break;
                case 6:
            // LookUp Table作成:明部強調
                    for(int x = 0; x < 128; x++){
                        lookUp[x] = 0;
                    }
                    for(int x = 128; x < 256; x++){
                        lookUp[x] = (byte)((x - 128) * 2);
                    }
                    break;
                case 7:
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
                case 8:
            // LookUp Table作成:γ変換
                    for(int x = 0; x < 256; x++){
                        lookUp[x] = (byte)(255 * Math.Pow(((double)x / 255), (1 / gamma)));
                    }
                    break;
                default:
            // LookUp Table作成:無変換
                for(int x = 0; x < 256; x++){
                    lookUp[x] = (byte)x;
                }
                break;
            }
        }
	}
}