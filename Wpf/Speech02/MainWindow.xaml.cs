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
                if(value == "終わり" || value == "終り" || value == "終了" || value == "おわり"){
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
                if(value == "色は赤" || value == "いろは赤")
                {
                    mainWindow.bColor.Background = Brushes.Red;
                    r = 255;
                    g = 0;
                    b = 0;
                }
                else if(value == "色は青" || value == "いろは青")
                {
                    mainWindow.bColor.Background = Brushes.Blue;
                    mainWindow.bColor.Foreground = Brushes.White;
                    r = 0;
                    g = 0;
                    b = 255;
                }
                else if(value == "色は緑" || value == "いろは緑")
                {
                    mainWindow.bColor.Background = Brushes.Green;
                    mainWindow.bColor.Foreground = Brushes.White;
                    r = 0;
                    g = 0x80;
                    b = 0;
                }
                else if(value == "色は黄色" || value == "いろは黄色" || value == "いろはきいろ")
                {
                    mainWindow.bColor.Background = Brushes.Yellow;
                    r = 255;
                    g = 255;
                    b = 0;
                }
                else if(value == "色はピンク"  || value == "いろはぴんく")
                {
                    mainWindow.bColor.Background = Brushes.Pink;
                    r = 0xff;
                    g = 0xc0;
                    b = 0xcb;
                }
                else if(value == "色はオレンジ"  || value == "いろはおれんじ")
                {
                    mainWindow.bColor.Background = Brushes.Orange;
                    r = 0xff;
                    g = 0xa5;
                    b = 0x00;
                }
                else if(value == "色は紫" || value == "いろはむらさき")
                {
                    mainWindow.bColor.Background = Brushes.Purple;
                    mainWindow.bColor.Foreground = Brushes.White;
                    r = 0x80;
                    g = 0x00;
                    b = 0x80;}
                else if(value == "色は灰色" || value == "いろははいいろ")
                {
                    mainWindow.bColor.Background = Brushes.Gray;
                    mainWindow.bColor.Foreground = Brushes.White;
                    r = 0x80;
                    g = 0x80;
                    b = 0x80;                }
                else if(value == "色は黒" || value == "いろはくろ")
                {
                    mainWindow.bColor.Background = Brushes.Black;
                    mainWindow.bColor.Foreground = Brushes.White;
                    r = 0;
                    g = 0;
                    b = 0;}
                else if(value == "色は白" || value == "いろはしろ")
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
                if(value == "終わり" || value == "終り" || value == "終了" || value == "おわり"){
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
                if(value == "もっと大きく" || value == "もっと大き")
                {
                    mainWindow.rect.Width += 5;
                    mainWindow.rect.Height += 5;
                }
                else if(value == "もっと小さく" || value == "もっと小さ")
                {
                    mainWindow.rect.Width -= 5;
                    mainWindow.rect.Height -= 5;
                }
                else if(value == "もっと広く" || value == "もっと広")
                {
                    mainWindow.rect.Width += 5;
                }
                else if(value == "もっと狭く" || value == "もっと狭")
                {
                    mainWindow.rect.Width -= 5;
                }
                else if(value == "もっと高く" || value == "もっと高")
                {
                    mainWindow.rect.Height += 5;
                }
                else if(value == "もっと低く" || value == "もっと低")
                {
                    mainWindow.rect.Height -= 5;
                }
                else if(value == "もっと右" || value == "もっとみぎ")
                {
                    var left = Canvas.GetLeft(Target);
                    Canvas.SetLeft(Target,left + 5);            
                }
                else if(value == "もっと左" || value == "もっとひだり")
                {
                    var left = Canvas.GetLeft(Target);
                    Canvas.SetLeft(Target,left - 5);            
                }
                else if(value == "もっと上" || value == "もっとうえ")
                {
                    var top = Canvas.GetTop(Target);
                    Canvas.SetTop(Target,top - 5);            
                }
                else if(value == "もっと下" || value == "もっとした")
                {
                    var top = Canvas.GetTop(Target);
                    Canvas.SetTop(Target,top + 5);            
                }
                else{
                    mainWindow.bColor.Text = "もっと～";
                }

                
            }
        }
    }
}
