using System;
using System.Collections.ObjectModel;
 
public class EdgeList {
		const int WIDTH = 6;
		const int HEIGHT =6;
		Random r1 = new System.Random();
        // バインディングの指定先プロパティ
        public ObservableCollection<Edge> DataE { get; set; }
 
        // コンストラクタ(データ入力)
        public EdgeList() {
            DataE = new ObservableCollection<Edge>();
			for(int j=0;j<HEIGHT;j++){
				for(int i=0;i<WIDTH-1;i++){
					int n = j * HEIGHT + i;
					int r = r1.Next(-5,6);
					if(r>0){
						DataE.Add(new Edge{from=n,to=n + 1,cost=1});
					}
					else if(r<0){
						DataE.Add(new Edge{to=n,from=n + 1,cost=1});
					}
				}
			}
			for(int i=0;i<WIDTH;i++){
				for(int j=0;j<HEIGHT-1;j++){
					int n = j * WIDTH + i;
					int r = r1.Next(-5,6);
					if(r>0){
						DataE.Add(new Edge{from= n,to=n + WIDTH,cost=1});
					}
					else if(r<0){
						DataE.Add(new Edge{to= n,from=n + WIDTH,cost=1});
					}
				}
			}
			
		// 自身へのコスト(cost=0)設定
/*		    foreach (Edge d in DataE)
		    {
		      	if (d.from == d.to){
		        d.cost = 0;
				}
		    }		
*/		
		}
    }   

