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

namespace dijkstra
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
        
            NodeList nodeList = new NodeList();

            EdgeList edgeList = new EdgeList();

            DijkstraC dijkstra = new DijkstraC();

            listViewNode.DataContext = dijkstra.DataNew;
            listViewEdge.DataContext = dijkstra.DataE;
            listViewDijk.DataContext = dijkstra.DataN;

        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NodeList nodeList = new NodeList();

            EdgeList edgeList = new EdgeList();

            DijkstraC dijkstra = new DijkstraC();

            listViewNode.DataContext = dijkstra.DataNew;
            listViewEdge.DataContext = dijkstra.DataE;
            listViewDijk.DataContext = dijkstra.DataN;
        }
        
    }
}
