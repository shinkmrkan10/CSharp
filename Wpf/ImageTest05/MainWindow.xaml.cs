using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageTest05
{
    public partial class MainWindow : Window
    {
        public TestCommand TestCmd { get; set; }
        public TestCommandC TestCmdC { get; set; }
        public TestCommandF TestCmdF { get; set; }
        public FolderChange folderChange { get; set; }
        public CloseWindow closeWindow { get; set; }
        public ImageSave imageSave { get; set; }
        public ImageSaveF imageSaveF { get; set; }
        public ObservableCollection<FileName> DataFileName { get; set; }
        public Dictionary<string, string> ContDic { get; set; }
        public Dictionary<string, string> ContFilterDic { get; set; }
   		public FormatConvertedBitmap bitmap = new FormatConvertedBitmap();
		public BitmapSource outputBitmap;
		public BitmapSource outputFBitmap;
		private const string CaptionSave = "Save File";
		private const string CaptionSaveError = "Save error";
		private const string OpenFileType = "Image Files(*.bmp;*.jpg;*.gif;*.png)|*.bmp;*.jpg;*.gif;*.png|All files (*.*)|*.*";
		private const string SaveFileType = "(*.bmp)|*.bmp|(*.jpg)|*.jpg|(*.gif)|*.gif|(*.png)|*.png";
        public static string foldername = @"C:\Image\"; 
        private string filename = "C:/Image/AAcolorBar.png";
        private string outputPath = @"C:\Image\NewImage\test.png";
        private string outputFPath = @"C:\Image\NewImage\FFTtest.png";
        private int effect  = 0;
        private int effectR = 0;
        private int effectG = 0;
        private int effectB = 0;
        private byte threshold  = 0;
        private byte thresholdR = 0;
        private byte thresholdG = 0;
        private byte thresholdB = 0;
        private string component = "gray2";
        private double gamma  = 1.0;
        private double gammaR = 1.0;
        private double gammaG = 1.0;
        private double gammaB = 1.0;
		private double lowpass = 128;
		private double lowpassR = 128;
		private double lowpassG = 128;
		private double lowpassB = 128;
		private double heighpass = 0;
		private double heighpassR = 0;
		private double heighpassG = 0;
		private double heighpassB = 0;
        private int width;
        private int height;
        public MainWindow()
        {
            ContDic = new Dictionary<string, string>()
            {
                { "0", "無変換" },
                { "1", "2値化" },
                { "2", "4値化" },
                { "3", "8値化" },
                { "4", "ネガポジ反転" },
                { "5", "暗部強調" },
                { "6", "明部強調" },
                { "7", "中間部強調" },
                { "8", "γ変換" },
            };
            ContFilterDic = new Dictionary<string, string>()
            {
                { "0", "矩形以外未実装" },
                { "1", "矩形フィルタ" },
                { "2", "ガウシャンフィルタ" },
                { "3", "LoGフィルタ" },
            };

            DataFileName = new ObservableCollection<FileName>();

            string[] files = System.IO.Directory.GetFiles(foldername, "*.*");
            foreach (string s in files)
            {
                System.IO.FileInfo fi = null;
                try
                {
                fi = new System.IO.FileInfo(s);
				DataFileName.Add(new FileName{fname = s});
                }
                catch (System.IO.FileNotFoundException)
                {
                    continue;
                }
            }
            InitializeComponent();
            this.SizeToContent = SizeToContent.Width;
            TestCmd = new TestCommand();
            TestCmdC = new TestCommandC();
            TestCmdF = new TestCommandF();
            folderChange = new FolderChange();
            closeWindow = new CloseWindow();
            imageSave = new ImageSave();
            imageSaveF = new ImageSaveF();
            DataContext = this;
			comboBox.DataContext = DataFileName;
			comboBoxCont.DataContext  = ContDic;
            comboBoxContR.DataContext = ContDic;
            comboBoxContG.DataContext = ContDic;
            comboBoxContB.DataContext = ContDic;
            comboBoxFilter.DataContext  = ContFilterDic;
            comboBoxFilterR.DataContext  = ContFilterDic;
            comboBoxFilterG.DataContext  = ContFilterDic;
            comboBoxFilterB.DataContext  = ContFilterDic;
            slider.Value  = 127;
            sliderCon.Value  = 1.0;
            sliderConR.Value = 1.0;
            sliderConG.Value = 1.0;
            sliderConB.Value = 1.0;
            sliderFilter.Value  = 0;
            sliderFilterH.Value  = 0;
            Threshold_Check();
            ThresholdFFT_Check();
        }
        public void folder_Change(object sender, RoutedEventArgs e){
            var filePath = string.Empty;
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "ファイルを開く";
            openFileDialog.InitialDirectory = foldername;
            openFileDialog.FilterIndex = 1;
            openFileDialog.Filter = OpenFileType;

            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
				DataFileName.Insert(0, new FileName{fname = filePath});
                comboBox.DataContext = DataFileName; // 追加される
                comboBox.SelectedIndex = 0;
            }
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            lowpass  = Double.Parse(textBoxFilter.Text);
            lowpassR  = Double.Parse(textBoxFilter.Text);
            lowpassG  = Double.Parse(textBoxFilter.Text);
            lowpassB  = Double.Parse(textBoxFilter.Text);
            heighpass  = Double.Parse(textBoxFilterH.Text);
            heighpassR  = Double.Parse(textBoxFilterH.Text);
            heighpassG  = Double.Parse(textBoxFilterH.Text);
            heighpassB  = Double.Parse(textBoxFilterH.Text);
            sliderFilter.Value  = lowpass;
            sliderFilterR.Value  = lowpassR;
            sliderFilterG.Value  = lowpassG;
            sliderFilterB.Value  = lowpassB;
            sliderFilterH.Value  = heighpass;
            sliderFilterRH.Value  = heighpassR;
            sliderFilterGH.Value  = heighpassG;
            sliderFilterBH.Value  = heighpassB;
            ThresholdFFT_Check();
        }
        private void textBoxFilterR_TextChanged(object sender, EventArgs e)
        {
            lowpassR  = Double.Parse(textBoxFilterR.Text);
            heighpassR  = Double.Parse(textBoxFilterRH.Text);
            sliderFilterR.Value  = lowpassR;
            sliderFilterRH.Value  = heighpassR;
            ThresholdFFT_Check();
        }
        private void textBoxFilterG_TextChanged(object sender, EventArgs e)
        {
            lowpassG  = Double.Parse(textBoxFilterG.Text);
            heighpassG  = Double.Parse(textBoxFilterGH.Text);
            sliderFilterG.Value  = lowpassG;
            sliderFilterGH.Value  = heighpassG;
            ThresholdFFT_Check();
        }
        private void textBoxFilterB_TextChanged(object sender, EventArgs e)
        {
            lowpassB  = Double.Parse(textBoxFilterB.Text);
            heighpassB  = Double.Parse(textBoxFilterBH.Text);
            sliderFilterB.Value  = lowpassB;
            sliderFilterBH.Value  = heighpassB;
            ThresholdFFT_Check();
        }
        private void comboBoxFilter_SelectedValueChanged(object sender, EventArgs e) 
        {
            ThresholdFFT_Check();
        }
        private void comboBoxFilterR_SelectedValueChanged(object sender, EventArgs e) 
        {
            ThresholdFFT_Check();
        }
        private void comboBoxFilterG_SelectedValueChanged(object sender, EventArgs e) 
        {
            ThresholdFFT_Check();
        }
        private void comboBoxFilterB_SelectedValueChanged(object sender, EventArgs e) 
        {
            ThresholdFFT_Check();
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
            ThresholdFFT_Check();
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
        public void button_Click_Save(object sender, RoutedEventArgs e)
        {
                var saveFileDialog = new SaveFileDialog();
				saveFileDialog.Title = "ファイルを保存";
				saveFileDialog.Filter = SaveFileType;
				if (saveFileDialog.ShowDialog() == true)
				{
					// 出力path
					outputPath = saveFileDialog.FileName;
					if (outputPath == filename)
					{
						MessageBoxResult messageBoxRewrite = MessageBox.Show(
							"元の画像は書き換えられません",
							caption: CaptionSaveError);
						return;
					}
					textBlockSaveFile.Text = outputPath;
					// BitmapSourceを保存する
					using (Stream stream = new FileStream(outputPath, FileMode.Create))
					{
                        if (outputPath.EndsWith(".bmp") || outputPath.EndsWith(".BMP")){
						BmpBitmapEncoder encoder = new BmpBitmapEncoder();
						encoder.Frames.Add(BitmapFrame.Create(outputBitmap));
						encoder.Save(stream);
                        }
                        if (outputPath.EndsWith(".jpg") || outputPath.EndsWith(".JPG")){
						JpegBitmapEncoder encoder = new JpegBitmapEncoder();
						encoder.Frames.Add(BitmapFrame.Create(outputBitmap));
						encoder.Save(stream);
                        }
                        if (outputPath.EndsWith(".gif") || outputPath.EndsWith(".GIF")){
						GifBitmapEncoder encoder = new GifBitmapEncoder();
						encoder.Frames.Add(BitmapFrame.Create(outputBitmap));
						encoder.Save(stream);
                        }
                        if (outputPath.EndsWith(".png") || outputPath.EndsWith(".PNG"))
					{
						PngBitmapEncoder encoder = new PngBitmapEncoder();
						encoder.Frames.Add(BitmapFrame.Create(outputBitmap));
						encoder.Save(stream);
                        }
					}

				}
				else
				{
					textBlockSaveFile.Text = "キャンセルされました";
				}
		}
        public void button_Click_SaveF(object sender, RoutedEventArgs e)
        {
				var saveFileDialogF = new SaveFileDialog();
				saveFileDialogF.Title = "ファイルを保存";
				saveFileDialogF.Filter = SaveFileType;
				if (saveFileDialogF.ShowDialog() == true)
				{
					// 出力path
					outputPath = saveFileDialogF.FileName;
					if (outputPath == filename)
					{
						MessageBoxResult messageBoxRewriteF = MessageBox.Show(
							"元の画像は書き換えられません",
							caption: CaptionSaveError);
						return;
					}
					textBlockSaveFileF.Text = outputPath;
					// BitmapSourceを保存する
					using (Stream stream = new FileStream(outputPath, FileMode.Create))
					{
                        if (outputFPath.EndsWith(".bmp") || outputFPath.EndsWith(".BMP")){
						BmpBitmapEncoder encoder = new BmpBitmapEncoder();
						encoder.Frames.Add(BitmapFrame.Create(outputFBitmap));
						encoder.Save(stream);
                        }
                        if (outputFPath.EndsWith(".jpg") || outputFPath.EndsWith(".JPG")){
						JpegBitmapEncoder encoder = new JpegBitmapEncoder();
						encoder.Frames.Add(BitmapFrame.Create(outputFBitmap));
						encoder.Save(stream);
                        }
                        if (outputFPath.EndsWith(".gif") || outputFPath.EndsWith(".GIF")){
						GifBitmapEncoder encoder = new GifBitmapEncoder();
						encoder.Frames.Add(BitmapFrame.Create(outputFBitmap));
						encoder.Save(stream);
                        }
                        if (outputFPath.EndsWith(".png") || outputFPath.EndsWith(".PNG")){
						PngBitmapEncoder encoder = new PngBitmapEncoder();
						encoder.Frames.Add(BitmapFrame.Create(outputFBitmap));
						encoder.Save(stream);
                        }
            		}

				}
				else
				{
					textBlockSaveFileF.Text = "キャンセルされました";
				}
        }
        // ThreasholdCheck.cs : 下記記述は不要
        // partial void Threshold_Check();
        // ImageArray.cs
//        partial void Create_ImageArray();
        // LookupTable.cs
        partial void Create_LookupTable();
    }
}

