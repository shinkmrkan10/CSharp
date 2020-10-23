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
        public ObservableCollection<MazeN> DataM { get; set; }

 
        // コンストラクタ
        public DijkstraC() {
            DataE = new ObservableCollection<Edge>();
            DataN = new ObservableCollection<Node>();
            DataNew = new ObservableCollection<Node>();
            DataM = new ObservableCollection<MazeN>();
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

		// Maze cost data(DataM) 作成
			for(int j=0;j<HEIGHT;j++){
				int n = j * WIDTH;
				DataM.Add(new MazeN{ c0=n,
									c1=n+1,
									c2=n+2,
									c3=n+3,
									c4=n+4,
									c5=n+5,
									c6=n+6,
									c7=n+7,
									c8=n+8
									}
				);
			}

		// Maze cost data(DataM) 更新
			var item1 = DataN.FirstOrDefault(x => x.num == 0);
			var item2 = DataM.FirstOrDefault(y => y.c0 == 0);
			for(int j = 0; j < HEIGHT; j++){
				int n = j * WIDTH;
				item2 = DataM.FirstOrDefault(y => y.c0 == n);
				if (item2 != null)
				{
				//	for(int i=0;i<WIDTH;i++){
					//	var cnum = $"c{i}";
						item1 = DataN.FirstOrDefault(x => x.num==n);
						if (item1 != null)
						{
						    item2.c0 = item1.cost;
						}
						n++;
						item1 = DataN.FirstOrDefault(x => x.num==n);
						if (item1 != null)
						{
						    item2.c1 = item1.cost;
						}
						n++;
						item1 = DataN.FirstOrDefault(x => x.num==n);
						if (item1 != null)
						{
						    item2.c2 = item1.cost;
						}
						n++;
						item1 = DataN.FirstOrDefault(x => x.num==n);
						if (item1 != null)
						{
						    item2.c3 = item1.cost;
						}
						n++;
						item1 = DataN.FirstOrDefault(x => x.num==n);
						if (item1 != null)
						{
						    item2.c4 = item1.cost;
						}
						n++;
						item1 = DataN.FirstOrDefault(x => x.num==n);
						if (item1 != null)
						{
						    item2.c5 = item1.cost;
						}
						n++;
						item1 = DataN.FirstOrDefault(x => x.num==n);
						if (item1 != null)
						{
						    item2.c6 = item1.cost;
						}
						n++;
						item1 = DataN.FirstOrDefault(x => x.num==n);
						if (item1 != null)
						{
						    item2.c7 = item1.cost;
						}
						n++;
						item1 = DataN.FirstOrDefault(x => x.num==n);
						if (item1 != null)
						{
						    item2.c8 = item1.cost;
						}
			
				//	}
				}
			}
		// INF -> -1 変換
		// スタートノード(cost=-1 for cost==INF)設定:foreach
		    foreach (MazeN d in DataM)
		    {
		      	if (d.c0 == INF){
		        d.c0 = -1;
				}
		      	if (d.c1 == INF){
		        d.c1 = -1;
				}
		      	if (d.c2 == INF){
		        d.c2 = -1;
				}
		      	if (d.c3 == INF){
		        d.c3 = -1;
				}
		      	if (d.c4 == INF){
		        d.c4 = -1;
				}
		      	if (d.c5 == INF){
		        d.c5 = -1;
				}
		      	if (d.c6 == INF){
		        d.c6 = -1;
				}
		      	if (d.c7 == INF){
		        d.c7 = -1;
				}
		      	if (d.c8 == INF){
		        d.c8 = -1;
				}
		    }
		}
	}		

