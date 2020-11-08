using System;
using System.Windows;

namespace ImageTest05
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SubWindowF : Window
    {
//        public ImageSave imageSave { get; set; }
        public SubWindowF()
        {
            InitializeComponent();
//            imageSave = new ImageSave();

            im_saveF.Text = "この画像を保存しますか？";  
			var mainWindow = (Application.Current.MainWindow as MainWindow);
			if (mainWindow != null)
			{
				imageF.Source = mainWindow.imageColorF.Source;
			}
        }
        private void imageSaveF(object sender, RoutedEventArgs e)
        {
			var mainWindow = (Application.Current.MainWindow as MainWindow);
			if (mainWindow != null)
			{
				var sender2 = new Object();
				var e2 = new RoutedEventArgs();
				mainWindow.button_Click_SaveF(sender2, e2);
			}
        }
        private void button_ClickF(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
