using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WpfShowWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            openText.Text = "Push button to open sub window.";  
            closeText.Text = "Push button to close this window.";  

        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            SubWindow sw = new SubWindow();
            sw.Owner = this;
            sw.Show();
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
