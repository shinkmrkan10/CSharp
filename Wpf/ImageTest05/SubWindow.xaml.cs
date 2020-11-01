using System;
using System.Windows;

namespace ImageTest05
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SubWindow : Window
    {
//        public ImageSave imageSave { get; set; }
        public SubWindow()
        {
            InitializeComponent();
//            imageSave = new ImageSave();

            im_save.Text = "この画像を保存しますか？";  
			var mainWindow = (Application.Current.MainWindow as MainWindow);
			if (mainWindow != null)
			{
				image.Source = mainWindow.imageColorNew.Source;
			}
        }
        private void imageSave(object sender, RoutedEventArgs e)
        {
			var mainWindow = (Application.Current.MainWindow as MainWindow);
			if (mainWindow != null)
			{
				var sender2 = new Object();
				var e2 = new RoutedEventArgs();
				mainWindow.button_Click_Save(sender2, e2);
			}
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
