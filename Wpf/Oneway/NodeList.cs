using System.Collections.ObjectModel;
using System.Windows.Data;
 
public class NodeList {
  // スレッド間の排他ロックに利用するオブジェクト
  private object _lockObject = new object();
		const int INF = 1000000;
		const int WIDTH = 9;
		const int HEIGHT =9;
        // バインディングの指定先プロパティ
        public ObservableCollection<Node> DataN { get; set; }
 
        // コンストラクタ(データ入力)
        public NodeList() {
            DataN = new ObservableCollection<Node>();
			for(int j=0;j<HEIGHT;j++){
				for(int i=0;i<WIDTH;i++){
					int n = j * WIDTH + i;
					DataN.Add(new Node{num=n,x=i,y=j,cost=INF,used=false,from=n});
				}
			}
		// スタートノード(num==0)設定
		    foreach (Node d in DataN)
		    {
		      	if (d.num == 0){
		        d.cost = 0;
//				d.used = true;
				}
		    }		
            // 複数スレッドからコレクション操作できるようにする
            BindingOperations.EnableCollectionSynchronization(this.DataN, _lockObject);
 		}
		 public void set_true(int num){
			 foreach(Node d in DataN){
				 if(d.num == num){
					 d.used = true;
				 }
			 }
		 }
    }   

