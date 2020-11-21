using System.Collections.ObjectModel;


namespace Puzzle01
{
	public class Tile 
	{
		public string name { get; set; }
		public int originalX { get; set; }
		public int originalY { get; set; }
		public int x { get; set; }
		public int y { get; set; }
	}
}

/*

        // バインディングの指定先プロパティ
        public ObservableCollection<Tile> DataPE { get; set; }
 
        // コンストラクタ(データ入力)
		public Tile(int width, int height)
		{
            DataPE = new ObservableCollection<Tile>();
			for(int y = 0; y < height; y++)
			{
				for(int x = 0; x < width; x++)
				{
					DataPE.Add(new Tile{
						name = "{00}{01}", y, x,
						originalX = x;
						originalY = y;
						x = x;
						y = y;
					});
				}
			}
		}



*/