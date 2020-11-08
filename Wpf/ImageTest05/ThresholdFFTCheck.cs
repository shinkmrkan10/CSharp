using System;

namespace ImageTest05
{

	public partial class MainWindow 
    {
        private void ThresholdFFT_Check(){
            bool repeat = true;
            while (repeat)
            {
                try{
                    Create_ImageFFTArray();
                    textBoxFilter.DataContext  = lowpass;
                    textBoxFilterR.DataContext  = lowpassR;
                    textBoxFilterG.DataContext  = lowpassG;
                    textBoxFilterB.DataContext  = lowpassB;
                    textBoxFilterH.DataContext  = lowpass;
                    textBoxFilterRH.DataContext  = heighpassR;
                    textBoxFilterGH.DataContext  = heighpassG;
                    textBoxFilterBH.DataContext  = heighpassB;
                    repeat = false;
                }
                catch (FormatException)
                {
                    sliderFilter.Value   = 0;
                    sliderFilterR.Value   = 0;
                    sliderFilterG.Value   = 0;
                    sliderFilterB.Value   = 0;
                    sliderFilterH.Value   = 0;
                    sliderFilterRH.Value   = 0;
                    sliderFilterGH.Value   = 0;
                    sliderFilterBH.Value   = 0;
                }
            }
        }
	}
}