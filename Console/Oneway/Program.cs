using System;

namespace Oneway
{
  class Program
  {
    const int INF = 1000000000;                     /*   無限大の代用数値   */
    const int PATH = 1;                             /*   距離=1  通過可能   */
    const int WIDTH = 15;                           /*   迷路の幅　         */
    const int HEIGHT = 15;                          /*   迷路の奥行         */
    const int NUMBER = WIDTH * HEIGHT;                          /*   ノード数　         */
    const int DIRECTION = 4;                        /*   壁の方向          */
//  0:i+  1:j+  2:i-  3:j- の方向にある壁の扉の状態
    public static Random r1 = new System.Random ();    /*  乱数        */
    static int[ ,,, ] dist = new int[WIDTH,HEIGHT,WIDTH,HEIGHT];  /*  ノード間の距離      */
    static int[] cost = new int[NUMBER];            /*  ノード0からの距離   */
    static bool[] used = new bool[NUMBER];          /*  距離計算済みフラグ  */
    static int[,,] maze = new int[WIDTH,HEIGHT,DIRECTION];  /*  扉の状態   */
//  -1以下:手前に開く  0:開かない  +1以上:向こうへ開く
//  全ての壁が開かない  All 0

/*   ダイクストラアルゴリズムのメソッド   */

    static void dijkstra (int[] cost, bool[] used, int[,,,] dist)
    {
        int num, num1, x, y, i, j, min;
        while(true){
            min = INF;
            for(x = 0; x < WIDTH; x++){
                for(y = 0; y < HEIGHT; y++){
                    num = x * HEIGHT + y;
                    if(!used[num] && (min > cost[num])){
                       min = cost[num];
                       used[num] = true;
                    }
                }
              }
            if(min == INF){
              break;
            }
            for(x = 0; x < WIDTH; x++){
                for(y = 0; y < HEIGHT; y++){
                    num = x * HEIGHT + y;
                    if(cost[num] == min){
                       for(i = 0; i < WIDTH; i++){
                           for(j = 0; j < HEIGHT; j++){
                               num1 = i * HEIGHT + j;
                               if(cost[num1] > dist[x,y,i,j] + cost[num]){
                                  cost[num1] = dist[x,y,i,j] + cost[num];
                                }
                            }
                        }
                    }
                }
            }
        }
    }

/*   メインクラス   */

    static void Main ()
    {
      int i, j, x, y, num, r2;

// 距離の初期化
      for(i = 0; i < WIDTH; i++){
        for(j = 0; j < HEIGHT; j++){
            for(x = 0; x < WIDTH; x++){
                for(y = 0; y < HEIGHT; y++){
                    dist[i,j,x,y] = INF;
                }
            }
        }
        dist[i,i,i,i] = 0;
      }

/*
// test距離代入
      maze[0,0,0] = 1;       // [0,0]のi+が1
      maze[1,0,2] = -1;      // [1,0]のi-が-1
      dist[0,0,1,0] = PATH;     // [0,0][1,0]の距離が1

      maze[0,0,1] = 1;       // [0,0]のj+が1
      maze[0,1,3] = -1;      // [0,1]のj-が-1
      dist[0,0,0,1] = PATH;     // [0,0][1,0]の距離が1

      maze[1,0,1] = 1;       // [1,0]のj+が1
      maze[1,1,2] = -1;      // [1,1]のj-が-1
      dist[1,0,1,1] = PATH;     // [1,0][1,1]の距離が1

      maze[1,1,1] = 1;       // [1,1]のj+が1
      maze[1,2,2] = -1;      // [1,2]のj-が-1
      dist[1,1,1,2] = PATH;     // [1,0][1,1]の距離が1
*/

// 内部にランダムに扉を配置する

    for(i = 0; i < WIDTH-1; i++){
        for(j = 0; j < HEIGHT-1; j++){
            r2 = r1.Next(-5, 20);  // -5から19
            maze[i,j,0] = r2;       // [i,j]のi+がr2
            maze[i+1,j,2] = -r2;      // [i+1,j]のi-が-r2
            if(r2 > 0){
                dist[i,j,i+1,j] = PATH;     // [i,j][i+1,j]の距離が1
            }
            else if(r2 < 0){
                dist[i+1,j,i,j] = PATH;     // [i+1,j][i,j]の距離が1
            }
            r2 = r1.Next(-5, 20);  // -5から19
            maze[i,j,1] = r2;       // [i,j]のj+がr2
            maze[i+1,j,3] = -r2;      // [i+1,j]のj-が-r2
            if(r2 > 0){
                dist[i,j,i,j+1] = PATH;     // [i,j][i,j+1]の距離が1
            }
            else if(r2 < 0){
                dist[i,j+1,i,j] = PATH;     // [i+1,j][i,j]の距離が1
            }
            
            
        }
    }

// [0,0]からはどちらへも行ける
      maze[0,0,0] = 1;       // [0,0]のi+が1
      maze[1,0,2] = -1;      // [1,0]のi-が-1
      dist[0,0,1,0] = PATH;     // [0,0][1,0]の距離が1

      maze[0,0,1] = 1;       // [0,0]のj+が1
      maze[0,1,3] = -1;      // [0,1]のj-が-1
      dist[0,0,0,1] = PATH;     // [0,0][1,0]の距離が1

// [WIDTH,HEIGHT]へははどちらからも行ける
      maze[WIDTH-2,HEIGHT-1,0] = 1;       // [WIDTH-2,HEIGHT-1]のi+が1
      maze[WIDTH-1,HEIGHT-1,2] = -1;      // [WIDTH-1,HEIGHT-1]のi-が-1
      dist[WIDTH-2,HEIGHT-1,WIDTH-1,HEIGHT-1] = PATH;     // [WIDTH-2,HEIGHT-1][WIDTH-1,HEIGHT-1]の距離が1

      maze[WIDTH-1,HEIGHT-2,1] = 1;       // [WIDTH-1,HEIGHT-2]のj+が1
      maze[WIDTH-1,HEIGHT-1,3] = -1;      // [WIDTH-1,HEIGHT-1]のj-が-1
      dist[WIDTH-1,HEIGHT-2,WIDTH-1,HEIGHT-1] = PATH;     // [WIDTH-1,HEIGHT-2][WIDTH-1,HEIGHT-1]の距離が1


/*
// 距離の表示
      for(i = 0; i < WIDTH; i++){
        for(j = 0; j < HEIGHT; j++){
          if(dist[0,0,i,j] == INF){
            Console.Write("X ");
          }
          else{
            Console.Write (string.Format ("{0} ",dist[0,0,i,j]));
          }
        }
        Console.WriteLine ();
      }
      Console.WriteLine ();
*/
// コストの初期化 
      for(i = 0; i < NUMBER; i++){
        cost[i] = INF;
        used[i] = false;
      }
      cost[0] = 0;

// ダイクストラ法
      dijkstra(cost, used, dist);

// 最短距離計算結果の表示
      for(i = 0; i < WIDTH; i++){
          for(j = 0; j < HEIGHT; j++){
              num = i * HEIGHT + j;
                if(cost[num] == INF){
                 Console.Write("X ");
                }
                else{
                 Console.Write (string.Format ("{0} ",cost[num]));
                }
            }
           Console.WriteLine ();
        }

    }
  }
}
