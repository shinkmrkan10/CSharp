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
/*
        private void dataGrid1_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            //ここでは、クリックしたセルのデータと同じデータのセルの背景色を赤にする処理をしています。
            //どうやってセルにアクセスすればよいのかは感じてもらえればと思います。
            //DataGridのSelectionUnitがCellであることを想定しています。
            DataGrid dg = sender as DataGrid;
             
            //クリックしたセルの列のインデックスを取得
            int columnIndex = dg.Columns.IndexOf(e.AddedCells[0].Column);
            //DataGrid上での行番号を表す変数
            int index = -1;
            //選択されたセルのある行に入っているオブジェクトを取得。
            //データベースからLINQで取得した時に無名クラスで一行を表していたので、
            //そのクラスがここに現れる。
            object o = e.AddedCells[0].Item;
            //選択された行のクラスと同じものであればそこが行番号になる。
            for (int i = 0; i < dg.Items.Count; i++)
            {
                if (dg.Items[i] == o)
                {
                    index = i;
                    break;
                }
            }
            //DataGridの行オブジェクトを取得
            DataGridRow dgr = dg.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;
            //選択されているセルの内容を取得
            string data = ((TextBlock)e.AddedCells[0].Column.GetCellContent(dgr)).Text;
             
            DataGridCell dgc = null;
            DataGridRow temp;
            string strTemp;
            for (int i = 0; i < dg.Items.Count; i++)
            {
                //行のインデックスから行を取得する
                temp = dg.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                //今の行のデータを取得
                strTemp = ((TextBlock)dg.Columns[columnIndex].GetCellContent(temp)).Text;
                //コンテナがDataGridCellなのでこのようにして取得
                dgc = dg.Columns[columnIndex].GetCellContent(temp).Parent as DataGridCell;
                //同じデータなら全て色を変えるなら直下のコメントようにする。
                //クリックされたものだけならコメントになっていないようにする。
                //if (strTemp == data)
                if (strTemp == data && temp == dgr)
                {
                    //赤い色に設定
                    dgc.Background = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    //選択されていないところは白に設定
                    dgc.Background = new SolidColorBrush(Colors.White);
                }
            }
             
        }
*/    




        // 各セルごとに対する処理 内容に応じた装飾など
        
/*
        private void EditDataGrid(object sender, RoutedEventArgs e)
        {
            // 行列数
            int rowCount = dataGrid1.Items.Count;
            int columnCount = dataGrid1.Columns.Count;

            for (int i = 0; i < rowCount; ++i)
            {
/*                // データグリッドの行オブジェクトを取得します。
                var row = dataGrid1.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                // 行オブジェクトが取得できない場合
                if (row == null)
                {
                    // 画面に表示されていないセルはnullとなる
                    continue;
                }

                for (int j = 0; j < columnCount; ++j)
                {
/*                    // データグリッドのセルオブジェクトを取得します。
                    DataGridCell cell = (DataGridCell)dataGrid1.Columns[j].GetCellContent(row).Parent;
                    // データグリッドのセルオブジェクトが取得できない場合
                    if (cell != null)
                    {
                        // 画面に表示されていないセルはnullとなる ここから各セルに対する処理

                        // セルの子オブジェクトを取得                        
                        var cellObject = dataGrid1.Columns[j].GetCellContent(row);
                        var tb = cellObject;

                        try
                        {
                          // 数値変換でエラーが出るケース有り
                            int cell = dataGrid1.View[2,3].Value;
                            if (((cell % 3) == 0)
                            {
                                cell.Background = Brushes.Cyan;
                            }
                            else
                            {
                                cell.Background = Brushes.Yellow;
                            }
                            if ((Int32(tb) % 5) == 0)
                            {
                                cell.Foreground = Brushes.Black;
                            }
                            else
                            {
                                cell.Foreground = Brushes.Red;
                            }
 /*                       }
                        catch
                        {
                        }

                        // Console.WriteLine(tb.Text);
                    }
                }
            }
        }*/
    }
}
/*
dataGrid1.Rows[2].Cell[3].Value

private void GetCellValue(DataGrid myGrid){
   DataGridCell myCell = new DataGridCell();
   // Use and arbitrary cell.
   myCell.RowNumber = 1;
   myCell.ColumnNumber = 1;
   Console.WriteLine(myGrid[myCell]);
}
*/

/*
//CellFormattingイベントハンドラ
private void DataGridView1_CellFormatting(object sender,
    DataGridViewCellFormattingEventArgs e)
{
    DataGridView dgv = (DataGridView)sender;

    //セルの列を確認
    if (dgv.Columns[e.ColumnIndex].Name == "Column1" && e.Value is int)
    {
        int val = (int)e.Value;
        //セルの値により、背景色を変更する
        if (val < 0)
        {
            e.CellStyle.BackColor = Color.Yellow;
        }
        else if (val == 0)
        {
            e.CellStyle.BackColor = Color.Red;
        }
    }
}
*/

// DataGridView1[0, 0].Style.BackColor = Color.Pink;