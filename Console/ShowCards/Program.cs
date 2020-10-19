using System;

namespace ShowCards
{
    class Program
    {
        const int SEED = 52; /*   シャッフルの乱数シード   */
        const int OFFSET = 0; /*   先頭の番号(0または1)　   */
        const int TOTAL = 52; /*   カードの総数(花札:48)    */
        const int CUT = 6; /*   カードをカットする枚数   */
        const int ROW = 4; /*   表示する行数　　　　　   */
        const int COL = 13; /*   表示する列数(花札:12)    */
           public static Random r1 = new System.Random (SEED);

        /*   配列をコピーするメソッド   */

        static void copy (int[] deck1, int[] deck2, int total)
        {
            int i;
            for (i = 0; i < total; i++)
            {
                deck2[i] = deck1[i]; /*   deck1からdeck2へコピー   */

            }
        }

        /* カードのマークを返すメソッド */

        static char suit (int index)
        {

            char[] mark = { 'S', 'H', 'D', 'C' };

            return mark[index / 13];
        }

        /* カードの数字を返すメソッド */

        static char number (int index)
        {

            char[] numbers = { 'A', '2', '3', '4', '5', '6', '7', '8', '9', '+', 'J', 'Q', 'K' };

            return numbers[index % 13];
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

        /* 配列のカードを表示する関数 */

        static void display_card (int[] deck, int row, int col)
        {
            int i, j;
            for (i = 0; i < row; i++)
            {
                for (j = 0; j < col; j++)
                {
                    Console.Write (string.Format ("  {0}{1}", suit (deck[col * i + j]), number (deck[col * i + j])));
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

        /* 手札の役を判定するメソッド */

        static void judge (int[] hand)
        {
            int i, j, tmp;
            int sum, multi;

            int fl_straight = 0;
            int fl_flush = 0;

            /* sum, multiを計算 */

            sum = 0;
            multi = 1;
            for (i = 0; i < 5; i++)
            {
                sum += hand[i] / 13;
                multi *= hand[i] / 13;
            }

            /* 手札がフラッシュであるか？ */

            if (sum == 0 || sum == 15)
                fl_flush = 1;
            else if (sum == 5 && multi == 1)
                fl_flush = 1;
            else if (sum == 10 && multi == 32)
                fl_flush = 1;
            else
                fl_flush = 0;

            /* 手札を数でソート */

            for (i = 0; i < 4; i++)
            {
                for (j = 4; j > i; j--)
                {
                    if (hand[j - 1] % 13 > hand[j] % 13)
                    {
                        tmp = hand[j];
                        hand[j] = hand[j - 1];
                        hand[j - 1] = tmp;
                    }
                }
            }

            /* 手札を数の順で表示 */

            Console.WriteLine ();
            Console.WriteLine ("手札整列");
            for (i = 0; i < 5; i++)
            {
                Console.Write (string.Format ("  {0}{1}", suit (hand[i]), number (hand[i])));
            }

            /* 手札がストレートであるか？ */

            if (hand[4] % 13 - hand[0] % 13 == 4)
            {
                if (hand[1] % 13 == hand[0] % 13 + 1 && hand[2] % 13 == hand[1] % 13 + 1 && hand[3] % 13 == hand[2] % 13 + 1)
                    fl_straight = 1;
            }
            else if (hand[0] % 13 == 0 && hand[1] % 13 == 9 && hand[2] % 13 == 10 && hand[3] % 13 == 11 &&
                hand[4] % 13 == 12)
                fl_straight = 1;
            else
                fl_straight = 0;

            /* ストレートフラッシュの判定 */

            if (fl_flush == 1 && fl_straight == 1)
                Console.WriteLine ("　ストレートフラッシュです。");
            else if (fl_flush == 1)
                Console.WriteLine ("　フラッシュです。");
            else if (fl_straight == 1)
                Console.WriteLine ("　ストレートです。");

            /* フォーカードの判定 */

            else if (hand[0] % 13 == hand[3] % 13 || hand[1] % 13 == hand[4] % 13)
                Console.WriteLine ("　フォーカードです。");

            /* フルハウスの判定 */

            else if ((hand[0] % 13 == hand[2] % 13 && hand[3] % 13 == hand[4] % 13) ||
                (hand[0] % 13 == hand[1] % 13 && hand[2] % 13 == hand[4] % 13))
                Console.WriteLine ("　フルハウスです。");

            /* スリーカードの判定 */

            else if (hand[0] % 13 == hand[2] % 13 || hand[1] % 13 == hand[3] % 13 || hand[2] % 13 == hand[4] % 13)
                Console.WriteLine ("　スリーカードです。");

            /* ツーペアの判定 */

            else if ((hand[0] % 13 == hand[1] % 13 && hand[2] % 13 == hand[3] % 13) ||
                (hand[0] % 13 == hand[1] % 13 && hand[3] % 13 == hand[4] % 13) ||
                (hand[1] % 13 == hand[2] % 13 && hand[3] % 13 == hand[4] % 13))
                Console.WriteLine ("　ツーペアです。");

            /* ワンペアの判定 */

            else if (hand[0] % 13 == hand[1] % 13 || hand[1] % 13 == hand[2] % 13 || hand[2] % 13 == hand[3] % 13 ||
                hand[3] % 13 == hand[4] % 13)
                Console.WriteLine ("　ワンペアです。");

            else
                Console.WriteLine ("　ノーペアです。");

        }

        /*   メインクラス   */

        static void Main ()
        {
            int i, j, k;
            int[] deck = new int[TOTAL]; /*   カード格納配列　　　   */
            int[] hand = new int[5]; /* 手札格納配列 */

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

            Random r1 = new Random (SEED);

            /*   5回繰り返す   */

            for (k = 0; k < 5; k++)
            {
                Console.WriteLine ();
                Console.WriteLine (string.Format ("new deck {0}", k + 1));

                set_random (deck, TOTAL, OFFSET);

                Console.WriteLine ();
                Console.WriteLine (string.Format ("配列（ランダム　SEED={0}）", SEED));
                display (deck, ROW, COL);

                /*   5枚ずつ10回配る   */

                for (j = 0; j < 10; j++)
                {

                    /*   手札を配る   */

                    for (i = 0; i < 5; i++)
                    {
                        hand[i] = deck[i + j * 5];
                    }

                    /*   手札を表示   */

                    Console.WriteLine ();
                    Console.WriteLine ("手札");
                    for (i = 0; i < 5; i++)
                    {
                        Console.Write (string.Format ("  {0}{1}", suit (hand[i]), number (hand[i])));
                    }

                    /*   手札の役を表示   */

                    judge (hand);

                }

            }
        }
    }
}