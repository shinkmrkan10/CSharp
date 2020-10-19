using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Test01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // DataGrid描画完了後にスタート
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("loaded");
            CreateDataGrid();
        }

        // メイン
        private void CreateDataGrid()
        {
            int rows = 10;
            int columns = 10;

            // ヘッダーカラム作成 列数
            for (int i = 0; i < columns; ++i)
            {
                var column = new DataGridTextColumn();
                column.Header = $"列{i}";
                column.Binding = new Binding($"[{i}]");
                dataGrid.Columns.Add(column);
            }

            // 列数に応じた行作成
            var dataList = new ObservableCollection<List<string>>();
            for (int i = 0; i < columns; ++i)
            {
                // 適当なデータ作成
                var data = CreateData(rows, columns, i);
                dataList.Add(data);
            }
            dataGrid.ItemsSource = dataList;

            // ItemsSource追加後にスクロールイベントを設定
            ScrollEvent();
        }

        // 今回はList<string>型で作成
        private List<string> CreateData(int rows, int columns, int plus)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < rows; ++i)
            {
                list.Add((columns * plus + i).ToString());
            }
            return list;
        }

        // スクロール時に発生するイベントを設定
        private void ScrollEvent()
        {
            Decorator child = VisualTreeHelper.GetChild(this.dataGrid, 0) as Decorator;
            ScrollViewer sc = child.Child as ScrollViewer;
            sc.ScrollChanged += new ScrollChangedEventHandler(EditDataGrid);
        }

        // 各セルごとに対する処理 内容に応じた装飾など
        private void EditDataGrid(object sender, RoutedEventArgs e)
        {
            // 行列数
            int rowCount = dataGrid.Items.Count;
            int columnCount = dataGrid.Columns.Count;

            for (int i = 0; i < rowCount; ++i)
            {
                // データグリッドの行オブジェクトを取得します。
                var row = dataGrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                // 行オブジェクトが取得できない場合
                if (row == null)
                {
                    // 画面に表示されていないセルはnullとなる
                    continue;
                }

                for (int j = 0; j < columnCount; ++j)
                {
                    // データグリッドのセルオブジェクトを取得します。
                    DataGridCell cell = (DataGridCell)dataGrid.Columns[j].GetCellContent(row).Parent;
                    // データグリッドのセルオブジェクトが取得できない場合
                    if (cell != null)
                    {
                        // 画面に表示されていないセルはnullとなる ここから各セルに対する処理

                        // セルの子オブジェクトを取得                        
                        var cellObject = dataGrid.Columns[j].GetCellContent(row);
                        TextBlock tb = cellObject as TextBlock;

                        try
                        {
                            // 数値変換でエラーが出るケース有り
                            if ((Int32.Parse(tb.Text) % 3) == 0)
                            {
                                cell.Background = Brushes.Cyan;
                            }
                            else
                            {
                                cell.Background = Brushes.Yellow;
                            }
                            if ((Int32.Parse(tb.Text) % 5) == 0)
                            {
                                cell.Foreground = Brushes.Black;
                            }
                            else
                            {
                                cell.Foreground = Brushes.Red;
                            }
                        }
                        catch
                        {
                        }

                        // Console.WriteLine(tb.Text);
                    }
                }
            }
        }
    }
}
