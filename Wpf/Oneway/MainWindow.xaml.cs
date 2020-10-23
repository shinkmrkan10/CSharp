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

namespace Oneway
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
            dataGrid2.DataContext = mazeList.DataM;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MazeList mazeList = new MazeList();

            NodeList nodeList = new NodeList();

            EdgeList edgeList = new EdgeList();

            DijkstraC dijkstra = new DijkstraC();

//            dataGrid1.DataContext = listView.View;
            dataGrid1.DataContext = dijkstra.DataM;
            dataGrid2.DataContext = mazeList.DataM;
            listViewNode.DataContext = dijkstra.DataNew;
            listViewEdge.DataContext = dijkstra.DataE;
            listViewDijk.DataContext = dijkstra.DataN;
//            listView.DataContext = dijkstra.DataM;

        // cost行列(data[9][9])の作成
            string[] c0 = new string[9];
            string[][] data = new string[9][];

            for(int x=0; x<9; x++){
                data[x] = new string[9];
            }
        // cost行列へのcost代入
            for(int y=0; y<9; y++){
                for(int x=0; x<9; x++){
                    int num = y * 9 + x;
			        var item = dijkstra.DataN.FirstOrDefault(i => i.num == num);
        			if (item != null && item.cost < 10000)
		        	{
			            c0[x] = item.cost.ToString();
                    }
                    else{
                        c0[x] = "-1";
                    }
                    data[y][x] = c0[x];
                }
            }
            SetUpDataGrid(dataGrid, data);
        }

        // ListViewのセットアップ
		private static void SetUpDataGrid(DataGrid dataGrid, string[][] data)
        {
            GridView view = new GridView();
            for (int i = 0; i < 9; i++)
            {
                view.Columns.Add(new GridViewColumn
                {
//                    Header = i,
                    DisplayMemberBinding = new Binding(string.Format("[{0}]", i))
                });
            }
            dataGrid.ItemsSource = data;
        }
    }
}