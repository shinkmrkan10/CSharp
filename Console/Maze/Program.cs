using System;

namespace Maze
{
  class Program
  {
    const int INF = 1000000000;                     /*   無限大の代用数値   */
    const int NUMBER = 15;                          /*   ノード数　         */
//        public static Random r1 = new System.Random (SEED);
    static int[ , ] dist = new int[NUMBER,NUMBER];  /*  ノード間の距離      */
    static int[] cost = new int[NUMBER];            /*  ノード0からの距離   */
    static bool[] used = new bool[NUMBER];          /*  距離計算済みフラグ  */


/*   ダイクストラアルゴリズムのメソッド   */

    static void dijkstra (int[] cost, bool[] used, int[,] dist)
    {
      int x, y, min;
      while(true){
        min = INF;
        for(x = 0; x < NUMBER; x++){
          if(!used[x] && (min > cost[x])){
            min = cost[x];
            used[x] = true;
          }
        }
        if(min == INF){
          break;
        }
        for(y = 0; y < NUMBER; y++){
          if(cost[y] == min){
            for(x = 0; x < NUMBER; x++){
              if(cost[x] > dist[x,y] + cost[y]){
                cost[x] = dist[x,y] + cost[y];
              }
            }
          }
        }
      }
    }

/*   メインクラス   */

    static void Main ()
    {
      int i, j;

// 距離の初期化
      for(i = 0; i < NUMBER; i++){
        for(j = 0; j < NUMBER; j++){
          dist[i,j] = INF;
        }
        dist[i,i] = 0;
      }
      for(i = 0; i < NUMBER -1; i++){
        dist[i,(NUMBER + i -  1) % NUMBER] = i % 5 + 1;
        dist[(NUMBER + i - 1) % NUMBER,i] = i % 5 + 1;
        dist[i,(NUMBER + i + 1) % NUMBER ] = i % 6 + 1;
        dist[(NUMBER + i + 1) % NUMBER ,i] = i % 6 + 1;
        dist[i,(NUMBER + i + 7) % NUMBER ] = i % 9 + 1;
        dist[(NUMBER + i + 7) % NUMBER ,i] = i % 9 + 1;
      }

// 距離の表示
      for(i = 0; i < NUMBER; i++){
        for(j = 0; j < NUMBER; j++){
          if(dist[i,j] == INF){
            Console.Write("X ");
          }
          else{
            Console.Write (string.Format ("{0} ",dist[i,j]));
          }
        }
        Console.WriteLine ();
      }
      Console.WriteLine ();

// コストの初期化 
      for(i = 0; i < NUMBER; i++){
        cost[i] = INF;
        used[i] = false;
      }
      cost[0] = 0;

// ダイクストラ法
      dijkstra(cost, used, dist);

// 最短距離計算結果の表示
      for(i = 0; i < NUMBER; i++){
        Console.WriteLine (string.Format("{0} : {1}", i, cost[i]));
      }

    }

  }
}
