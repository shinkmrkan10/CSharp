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
		const int INF = 1000000;
		const int WIDTH = 9;
		const int HEIGHT = 9;
        int num;
            public MainWindow()
        {
            InitializeComponent();
            MazeList mazeList = new MazeList();
            dataGrid2.DataContext = mazeList.DataM;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Node item;

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

        // cost行列(data[WIDTH][HEIGHT])の作成
            string[] c0 = new string[WIDTH];
            string[][] data = new string[HEIGHT][];

            for(int x = 0; x < WIDTH; x++){
                data[x] = new string[WIDTH];
            }
        // cost行列へのcost代入
            for(int y = 0; y < HEIGHT; y++){
                for(int x = 0; x < WIDTH; x++){
                    num = y * 9 + x;
			        item = dijkstra.DataN.FirstOrDefault(i => i.num == num);
        			if (item != null && item.cost != INF)
		        	{
			            c0[x] = item.cost.ToString();
                    }
                    else{
                        c0[x] = "-1";
                    }
                    data[y][x] = c0[x];
                }
            }
//            SetUpDataGrid(dataGrid, data);
        // cost行列へのroute代入(fromを逆順にたどる)
            num =80;
			item = dijkstra.DataN.FirstOrDefault(i => i.num == num);
        	if (item != null && item.cost != INF)
	    	{
	            num = item.from;
                data[item.y][item.x] = "R";
                while(num != 0){
	    		    item = dijkstra.DataN.FirstOrDefault(i => i.num == num);
            		if (item != null)
		        	{
		                num = item.from;
                        data[item.y][item.x] = "R";
                    }
               }
            }
            SetUpDataGrid(dataGrid, data);
        }

        // DataGridのセットアップ
		private static void SetUpDataGrid(DataGrid dataGrid, string[][] data)
        {
            GridView view = new GridView();
            for (int i = 0; i < WIDTH; i++)
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