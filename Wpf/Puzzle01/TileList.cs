using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
 
namespace Puzzle01
{
	public class TileList {
  		// スレッド間の排他ロックに利用するオブジェクト
  		private object _lockObject = new object();
		const int INF = 1000000;
		const int WIDTH = 4;
		const int HEIGHT =4;
        // バインディングの指定先プロパティ
        public ObservableCollection<Tile> DataT { get; set; }
 
        // コンストラクタ(データ入力)
        public TileList() {
            DataT = new ObservableCollection<Tile>();
            int number = 1;
			for(int y = 0; y < HEIGHT; y++)
			{
				for(int x = 0; x < WIDTH; x++)
				{
					DataT.Add(new Tile{
						name = String.Format("tile{0}", number++ ),
						originalX = x,
						originalY = y,
						x = x,
						y = y
					});
				}
			}
            // 複数スレッドからコレクション操作できるようにする
            BindingOperations.EnableCollectionSynchronization(this.DataT, _lockObject);
 		}
/*		 public void set_true(int num){
			 foreach(Tile d in DataT){
				 if(d.num == num){
					 d.used = true;
				 }
			 }
		 }*/
    } 
}  

