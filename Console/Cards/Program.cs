using System;

namespace Cards
{
    class Program
    {
        const int SEED = 52; /*   シャッフルの乱数シード   */
        const int OFFSET = 1; /*   先頭の番号(0または1)　   */
        const int TOTAL = 52; /*   カードの総数(花札:48)    */
        const int CUT = 6; /*   カードをカットする枚数   */
        const int ROW = 4; /*   表示する行数　　　　　   */
        const int COL = 13; /*   表示する列数(花札:12)    */
        public static Random r1 = new Random (SEED);

        /*   配列をコピーするメソッド   */

        static void copy (int[] deck1, int[] deck2, int total)
        {
            int i;
            for (i = 0; i < total; i++)
            {
                deck2[i] = deck1[i]; /*   deck1からdeck2へコピー   */

            }
        }

        /*   配列の要素を表示するメソッド   */

        static void display (int[] deck, int row, int col)
        {
            int i, j;
            for (i = 0; i < row; i++)
            {
                for (j = 0; j < col; j++)
                {
                    Console.Write (string.Format ("{0,4}", deck[col * i + j]));
                }
                Console.WriteLine ();

            }
        }

        /*   配列に番号順に格納するメソッド   */

        static void set (int[] deck, int total, int offset)
        {
            int i;
            for (i = 0; i < total; i++)
            {
                deck[i] = i + offset;
            }
        }

        /*   配列にランダムに格納するメソッド   */

        static void set_random (int[] deck, int total, int offset)
        {
            int i, pick;
            int[] card_in_use = new int[100]; /*   未使用 0 で初期化   */

            for (i = 0; i < total; i++)
            {
                do
                {
                    pick = r1.Next (0, total);
                } while (card_in_use[pick] == 1); /*   未使用を探す        */

                deck[i] = pick + offset; /*   offsetからの数      */
                card_in_use[pick] = 1; /*   使用済みは 1        */
            }
        }

        /*   配列を半分に分け交互に格納するメソッド（シャッフル）   */

        static void shuffle (int[] deck1, int total)
        {

            int i;
            int[] deck2 = new int[100];

            copy (deck1, deck2, total); /*   コピーを作る   */
            for (i = 0; i < total / 2; i++)
            {
                deck1[i * 2] = deck2[i]; /*   偶数番目       */
                deck1[i * 2 + 1] = deck2[i + total / 2]; /*   奇数番目       */
            }
        }

        /*   配列をCUTずらして格納するメソッド（カット）   */
        static void cut (int[] deck1,
            int total,
            int cut)
        {

            int i;
            int[] deck2 = new int[100];

            copy (deck1, deck2, total); /*   コピーを作る   */
            for (i = 0; i < total; i++)
            {
                deck1[i % total] = deck2[(i + cut) % total]; /*   剰余を利用     */
            }
        }

        /*   メインクラス   */

        static void Main ()
        {
            int i;
            int[] deck = new int[TOTAL]; /*   カード格納配列　　　   */

            /*   deckに番号順に格納する   */

            set (deck, TOTAL, OFFSET);

            Console.WriteLine ();
            Console.WriteLine ("配列（整列）");
            display (deck, ROW, COL);

            /*   deckを半分に分け交互に格納する（シャッフル）   */

            shuffle (deck, TOTAL);
            Console.WriteLine ();
            Console.WriteLine ("配列（シャッフル）");
            display (deck, ROW, COL);

            /*   deckをCUTだけずらして格納する（カット）   */

            cut (deck, TOTAL, CUT);
            Console.WriteLine ();
            Console.WriteLine (string.Format ("配列（カット　{0}）", CUT));
            display (deck, ROW, COL);

            /*   deckにランダムに格納する   */

            Random r2 = new Random (SEED);

            set_random (deck, TOTAL, OFFSET);
            Console.WriteLine ();
            Console.WriteLine (string.Format ("配列（ランダム　SEED={0}）", SEED));
            display (deck, ROW, COL);

            /*   deckに番号順に格納する   */

            set (deck, TOTAL, OFFSET);

            Console.WriteLine ();
            Console.WriteLine ("シャッフル");
            Console.WriteLine ();
            Console.WriteLine (string.Format ("{0}回目", 0));
            display (deck, ROW, COL);

            /*   シャッフルを繰り返す   */
            for (i = 0; i < 8; i++)
            {
                shuffle (deck, TOTAL);
                Console.WriteLine ();
                Console.WriteLine (string.Format ("{0}回目", i + 1));
                display (deck, ROW, COL);
            }
        }
    }
}