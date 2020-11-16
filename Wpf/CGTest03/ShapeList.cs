using System;
using System.Collections.ObjectModel;
using CGTest03;

public class ShapeList {
		const double WIDTH = 100;
		const double HEIGHT = 100;
		const byte FF = 0xFF;
        // バインディングの指定先プロパティ
        public ObservableCollection<ShapeElement> DataS { get; set; }
 
        // コンストラクタ(データ入力)
        public ShapeList() {
            DataS = new ObservableCollection<ShapeElement>();
			DataS.Add(new ShapeElement{
				name = "rect00",
				x = 0, y = 0, z = 0,
				width = WIDTH, height = HEIGHT,
				color = "Red", r = FF, g = 0, b = 0, a = FF
			});
			
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

