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

namespace maze
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();

            MazeList mazeList = new MazeList();
        
            NodeList nodeList = new NodeList();

            EdgeList edgeList = new EdgeList();

            DijkstraC dijkstra = new DijkstraC();


            dataGrid1.DataContext = dijkstra.DataM;
            dataGrid2.DataContext = mazeList.DataM;
            listViewNode.DataContext = dijkstra.DataNew;
            listViewEdge.DataContext = dijkstra.DataE;
            listViewDijk.DataContext = dijkstra.DataN;

        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MazeList mazeList = new MazeList();

            NodeList nodeList = new NodeList();

            EdgeList edgeList = new EdgeList();

            DijkstraC dijkstra = new DijkstraC();

            dataGrid1.DataContext = dijkstra.DataM;
            dataGrid2.DataContext = mazeList.DataM;
            listViewNode.DataContext = dijkstra.DataNew;
            listViewEdge.DataContext = dijkstra.DataE;
            listViewDijk.DataContext = dijkstra.DataN;
        }
    }
}