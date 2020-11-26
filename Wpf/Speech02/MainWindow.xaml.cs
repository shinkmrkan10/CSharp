using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Speech02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel vm;
        SpeechRecognitionEngine engine;
        public MainWindow() {
            InitializeComponent();
            vm = new ViewModel();
            this.DataContext = vm;
            engine = new SpeechRecognitionEngine();
            engine.SpeechRecognized += Engine_SpeechRecognized;//認識処理
            engine.SpeechHypothesized += Engine_SpeechHypothesized;//推定処理
            engine.LoadGrammar(new DictationGrammar());//ディクテーション用の辞書
            engine.SetInputToDefaultAudioDevice();//エンジンの入力
            engine.RecognizeAsync(RecognizeMode.Multiple);//開始

        }

        private void Engine_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e) {
            vm.Voice = e.Result.Text;
        }

        private void Engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e) {
            vm.Voice = e.Result.Text;
            vm.Text = e.Result.Text;
        }
    }
    public class ViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        string _Text = "";
        public string Text {
            get {
                return _Text;
            }
            set {
                _Text = value + "\r\n" + Text;
                OnPropertyChanged("Text");
            }
        }
        string _Voice = "";
        public string Voice {
            get {
                return _Voice;
            }
            set {
                _Voice = value;
                OnPropertyChanged("Voice");
                color_check(value);
                if(_isColorSet)
                {
                    size_check(value);
                }
            }
        }
        bool _isColorSet = false;
        byte r = 255;
        byte g = 255;
        byte b = 255;
        public void color_check(string value){
            var mainWindow = (Application.Current.MainWindow as MainWindow);
    		if (mainWindow != null )
	    	{
                // "終了"　でアプリを閉じる
                if(value.Contains("終") || value.Contains("おわり")){
	    		    mainWindow.Close();
                }
            }
            // "色は～"　で色を変える
    		if (mainWindow != null && !_isColorSet )
	    	{
                // "決定"　で色を確定する
                if(value == "色は決定" || value == "いろはけってい" || value == "色は決まり")
                {
                    _isColorSet = true;
    //                mainWindow.bColor.Text = "色：決定。大きさ：もっと（大きく | 小さく）";
                    return;
                }
                mainWindow.bColor.Text = value;
                if(value.Contains("赤"))
                {
                    mainWindow.bColor.Background = Brushes.Red;
                    r = 255;
                    g = 0;
                    b = 0;
                }
                else if(value.Contains("青"))
                {
                    mainWindow.bColor.Background = Brushes.Blue;
                    mainWindow.bColor.Foreground = Brushes.White;
                    r = 0;
                    g = 0;
                    b = 255;
                }
                else if(value.Contains("緑"))
                {
                    mainWindow.bColor.Background = Brushes.Green;
                    mainWindow.bColor.Foreground = Brushes.White;
                    r = 0;
                    g = 0x80;
                    b = 0;
                }
                else if(value.Contains("黄"))
                {
                    mainWindow.bColor.Background = Brushes.Yellow;
                    r = 255;
                    g = 255;
                    b = 0;
                }
                else if(value.Contains("ピンク"))
                {
                    mainWindow.bColor.Background = Brushes.Pink;
                    r = 0xff;
                    g = 0xc0;
                    b = 0xcb;
                }
                else if(value.Contains("オレンジ"))
                {
                    mainWindow.bColor.Background = Brushes.Orange;
                    r = 0xff;
                    g = 0xa5;
                    b = 0x00;
                }
                else if(value.Contains("紫"))
                {
                    mainWindow.bColor.Background = Brushes.Purple;
                    mainWindow.bColor.Foreground = Brushes.White;
                    r = 0x80;
                    g = 0x00;
                    b = 0x80;}
                else if(value.Contains("灰"))
                {
                    mainWindow.bColor.Background = Brushes.Gray;
                    mainWindow.bColor.Foreground = Brushes.White;
                    r = 0x80;
                    g = 0x80;
                    b = 0x80;                }
                else if(value.Contains("黒"))
                {
                    mainWindow.bColor.Background = Brushes.Black;
                    mainWindow.bColor.Foreground = Brushes.White;
                    r = 0;
                    g = 0;
                    b = 0;}
                else if(value.Contains("白"))
                {
                    mainWindow.bColor.Background = Brushes.White;
                    mainWindow.bColor.Foreground = Brushes.Black;
                    r = 255;
                    g = 255;
                    b = 255;}
                else{
                    mainWindow.bColor.Background = Brushes.White;
                    mainWindow.bColor.Foreground = Brushes.Black;
//                    r = 255;
//                    g = 255;
//                    b = 255;
                    mainWindow.bColor.Text = "色は～";
                }
                    var mySolidColorBrush = new SolidColorBrush();
                    mySolidColorBrush.Color = Color.FromArgb(255, r, g, b);
                    mainWindow.rect.Fill=mySolidColorBrush;
                    mainWindow.rect.Stroke=Brushes.Black;
            }
        }
        bool _isSizeSet = false;
        public void size_check(string value){
            var mainWindow = (Application.Current.MainWindow as MainWindow);
    		if (mainWindow != null )
	    	{
                // "終了"　でアプリを閉じる
                if(value.Contains("終") || value.Contains("おわり")){
	    		    mainWindow.Close();
                }
            }
            // "もっと～"　でサイズを変える
        	if (mainWindow != null && !_isSizeSet )
	    	{
                var Target = (Rectangle)LogicalTreeHelper.FindLogicalNode(mainWindow.canvas, "rect");
                Target.Stroke=Brushes.SkyBlue;
                Target.StrokeThickness = 2;
                // "決定"　で大きさを確定する
                if(value == "大きさは決定" || value == "おおきさはけってい" || value == "大きさは決まり")
                {
                    _isSizeSet = true;
                    mainWindow.bColor.Text = "色：決定。大きさ：決定。";
                    Target.StrokeThickness = 0;
                    return;
                }
                mainWindow.bColor.Text = value;
                if(value.Contains("大"))
                {
                    mainWindow.rect.Width += 1;
                    mainWindow.rect.Height += 1;
                }
                else if(value.Contains("小"))
                {
                    mainWindow.rect.Width -= 1;
                    mainWindow.rect.Height -= 1;
                }
                else if(value.Contains("広") || value.Contains("長"))
                {
                    mainWindow.rect.Width += 1;
                }
                else if(Voice.Contains("狭") || Voice.Contains("短"))
                {
                    mainWindow.rect.Width -= 1;
                }
                else if(value.Contains("高"))
                {
                    mainWindow.rect.Height += 1;
                }
                else if(value.Contains("低"))
                {
                    mainWindow.rect.Height -= 1;
                }
                else if(value.Contains("右"))
                {
                    var left = Canvas.GetLeft(Target);
                    Canvas.SetLeft(Target,left + 1);            
                }
                else if(value.Contains("左"))
                {
                    var left = Canvas.GetLeft(Target);
                    Canvas.SetLeft(Target,left - 1);            
                }
                else if(value.Contains("上") || value.Contains("うえ"))
                {
                    var top = Canvas.GetTop(Target);
                    Canvas.SetTop(Target,top - 1);            
                }
                else if( value.Contains("下") || value.Contains("した")) 
                {
                    var top = Canvas.GetTop(Target);
                    Canvas.SetTop(Target,top + 1);            
                }
                else{
                    mainWindow.bColor.Text = "もっと～";
                }

                
            }
        }
    }
}
