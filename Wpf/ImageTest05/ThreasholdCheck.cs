using System;

namespace ImageTest05
{

	public partial class MainWindow 
    {
        private void Threshold_Check(){
            bool repeat = true;
            while (repeat)
            {
                try{
                    Create_ImageArray();
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
	}
}