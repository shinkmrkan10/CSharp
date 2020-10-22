using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
 
public class MazeList {
  // スレッド間の排他ロックに利用するオブジェクト
  private object _lockObject = new object();
		const int INF = 1000000;
		const int WIDTH = 9;
		const int HEIGHT =9;
        // バインディングの指定先プロパティ
        public ObservableCollection<MazeN> DataM { get; set; }
 
        // コンストラクタ(データ入力)
        public MazeList() {
            DataM = new ObservableCollection<MazeN>();
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
		// スタートノード(num==0)設定
/*		    foreach (Node d in DataM)
		    {
		      	if (d.num == 0){
		        d.cost = 0;
//				d.used = true;
				}
		    }		
*/
            // 複数スレッドからコレクション操作できるようにする
            BindingOperations.EnableCollectionSynchronization(this.DataM, _lockObject);
 		}
/*
		 public void set_true(int num){
			 foreach(Node d in DataM){
				 if(d.num == num){
					 d.used = true;
				 }
			 }
		 }
*/
    }   

