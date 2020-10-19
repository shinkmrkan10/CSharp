using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

    public class DijkstraC
    {
		const int INF = 1000000;
		const int WIDTH = 9;
		const int HEIGHT = 9;
 		const int NUMBER = WIDTH * HEIGHT ;
		const int UNDER = -3;
		const int OVER = 10;
		int numNew, min;
		Random r1 = new System.Random();
        public ObservableCollection<Edge> DataE { get; set; }
        public ObservableCollection<Node> DataN { get; set; }
        public ObservableCollection<Node> DataNew { get; set; }
 
        // コンストラクタ
        public DijkstraC() {
            DataE = new ObservableCollection<Edge>();
            DataN = new ObservableCollection<Node>();
            DataNew = new ObservableCollection<Node>();
        // (データ入力:ノード)
			for(int j = 0; j < HEIGHT; j++){
				for(int i = 0; i < WIDTH; i++){
					int n = j * WIDTH + i;
					DataN.Add(new Node{num=n,x=i,y=j,cost=INF,used=false,from=n});
				}
			}
		// スタートノード(cost=0 for num==0)設定:foreach
/*		    foreach (Node d in DataN)
		    {
		      	if (d.num == 0){
		        d.cost = 0;
				}
		    }
*/
		// スタートノード(cost=0 for num==0)設定:Linq
			var item = DataN.FirstOrDefault(i => i.num == 0);
			if (item != null)
			{
			    item.cost = 0;
			}

        // (データ入力:エッジ i方向)
			for(int j = 0; j < HEIGHT; j++){
				for(int i = 0; i <WIDTH-1; i++){
					int n = j * HEIGHT + i;
					int r;
			//		if(j<HEIGHT/5 || j>HEIGHT*4/5){
						r = r1.Next(UNDER, OVER);
			//		}
			//		else{
			//			r = r1.Next(-OVER,-UNDER);
			//		}
					if(r>0){
						DataE.Add(new Edge{from=n,to=n + 1,cost=1});
					}
					else if(r<0){
						DataE.Add(new Edge{to=n,from=n + 1,cost=1});
					}
				}
			}
        // (データ入力:エッジ j方向)
			for(int i = 0; i < WIDTH; i++){
				for(int j = 0; j < HEIGHT - 1; j++){
					int n = j * WIDTH + i;
					int r;
			//		if(i<WIDTH/5 || i>WIDTH*4/5){
						r = r1.Next(UNDER,OVER);
			//		}
			//		else{
			//			r = r1.Next(-OVER,-UNDER);
			//		}
					if(r>0){
						DataE.Add(new Edge{from= n,to=n + WIDTH,cost=1});
					}
					else if(r<0){
						DataE.Add(new Edge{to= n,from=n + WIDTH,cost=1});
					}
				}
			}
		// dijkstra	
	     	while(true){
        		min = INF;
		    	foreach (Node dN in DataN){
          			if(!dN.used && (min > dN.cost)){
            			min = dN.cost;
            			dN.used = true;
			//			numN = dN.num;
          			}
        		}
		        if(min == INF){
        			break;
      			}
		  
			    foreach (Node dN2 in DataN){
    			    if(dN2.cost == min){
        			    foreach(Edge dE in DataE){
							if(dE.from == dN2.num){
								numNew = dE.to;
								item = DataN.FirstOrDefault(i => i.num == numNew);
								if ((item != null) && (item.cost > (dE.cost + min))){
									item.cost = dE.cost + min;
									item.from = dE.from;
								}
							/*	foreach(Node dNew in DataN)
								{
									if((dNew.num == numNew) && (dNew.cost > (dE.cost + min)))
									{
										dNew.cost = dE.cost + min;
										dNew.from = dE.from;
									}
								}*/
							}
    		    	    }
				    }
       			}
	    	}
		// Node order list(DataNew) 作成
			int from_n =NUMBER -1;
			from_n = createDataNew(from_n);
			while((from_n != 0) && (from_n != (NUMBER -1))){
				from_n = createDataNew(from_n);
			}
			from_n = 0;
			from_n = createDataNew(from_n);

			
			int createDataNew(int from_n){
				item = DataN.FirstOrDefault(i => i.num == from_n);
				if (item != null && item.num == 0)
				{
					DataNew.Insert (0, new Node{num=item.num,x=item.x,y=item.y,cost=item.cost,used=true,from=item.from});
					return item.from;
				}
				else if (item != null && item.from != item.num)
				{
					DataNew.Insert (0, new Node{num=item.num,x=item.x,y=item.y,cost=item.cost,used=true,from=item.from});
					return item.from;
				}
				else{
					return from_n;
				}
			}

/*
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
        }*/



		}		
	}