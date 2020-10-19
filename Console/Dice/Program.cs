using System;
/*
 * サイの出目を分析する
 */
class Dice
{
    const int SEED_ONE = 215; /*   サイ１の乱数シード   */
    const int SEED_TWO = 216; /*   サイ２の乱数シード   */
    const int TOTAL_NUMBER = 216 * 216; /*   試技数　　　　　　   */
    const int COLUMN_NUMBER = 24; /*   １列あたりの表示数   */
    const int ROW_NUMBER = 9; /*   表示行数　　　　　   */
    const int MAX_NUMBER = 6; /*   出目の最高値　　　   */

    static void Main ()
    {
        int i, j, sum, sum1, sum2;
        int[] die_one = new int[TOTAL_NUMBER]; /*   サイ１の出目　　　   */
        int[] die_two = new int[TOTAL_NUMBER]; /*   サイ２の出目　　　   */
        int[] even_odd = new int[TOTAL_NUMBER]; /*   丁半（偶数奇数）　   */
        int[, ] cross_sum = new int[MAX_NUMBER, MAX_NUMBER]; /*   出目の集計表　　　   */
        int[, ] sequence_one = new int[MAX_NUMBER, MAX_NUMBER]; /*   サイ１の推移表　　   */
        int[, ] sequence_two = new int[MAX_NUMBER, MAX_NUMBER]; /*   サイ２の推移表　　   */

        /*   サイ1の出目を配列に格納する   */

        Random r1 = new System.Random (SEED_ONE);

        for (i = 0; i < TOTAL_NUMBER; i++)
        {
            die_one[i] = r1.Next (1, 7);
        }
        /*   サイ2の出目を配列に格納する   */

        Random r2 = new System.Random (SEED_TWO);

        for (i = 0; i < TOTAL_NUMBER; i++)
        {
            die_two[i] = r2.Next (1, 7);
        }
        /*   丁半の結果を０１で配列に格納する   */

        for (i = 0; i < TOTAL_NUMBER; i++)
        {
            even_odd[i] = (die_one[i] + die_two[i]) % 2;
        }

        /*   指定した行数列数までの出目を表示する   */

        Console.WriteLine ();
        Console.WriteLine (string.Format ("The result of 1=>{0,3}", ROW_NUMBER * COLUMN_NUMBER));
        Console.WriteLine (string.Format ("die1 seed={0} : die2 seed={1}", SEED_ONE, SEED_TWO));
        Console.WriteLine ();
        for (j = 0; j < ROW_NUMBER; j++)
        {
            Console.WriteLine (string.Format ("{0}=>{1}", j * COLUMN_NUMBER + 1 , (j + 1) * COLUMN_NUMBER ));
            Console.Write ("die_one :");
            for (i = 0; i < COLUMN_NUMBER; i++)
            {
                Console.Write (string.Format ("{0,2}", die_one[j * ROW_NUMBER + i]));
            }
            Console.WriteLine ();

            Console.Write ("die_two :");
            for (i = 0; i < COLUMN_NUMBER; i++)
            {
                Console.Write (string.Format ("{0,2}", die_two[j * ROW_NUMBER + i]));
            }
            Console.WriteLine ();

            Console.Write ("even/odd:");
            sum = 0;
            for (i = 0; i < COLUMN_NUMBER; i++)
            {
                Console.Write (string.Format ("{0,2}", even_odd[j * ROW_NUMBER + i]));
                sum += even_odd[j * ROW_NUMBER + i];
            }
            Console.WriteLine (string.Format (" : {0,2}/{1,2}", COLUMN_NUMBER - sum, sum));
            Console.WriteLine ();

        }
/*   出目の集計表を表示する   */

    Console.WriteLine();
    Console.WriteLine(string.Format("The trial of {0}", TOTAL_NUMBER));
    Console.WriteLine(string.Format("die1 seed={0} : die2 seed={1}", SEED_ONE, SEED_TWO ));
    Console.WriteLine();

    Console.WriteLine("The cross sum of dice");
    for( i=0 ; i < TOTAL_NUMBER ; i++ ){
        cross_sum[(die_two[i]-1),(die_one[i]-1)]++ ;
    }

    Console.WriteLine("   die1 |   (1)   (2)   (3)   (4)   (5)   (6)" ) ;
    Console.WriteLine("--------+--------------------------------------" ) ;
    for ( j = 0 ; j < 6 ; j++ ){
        sum = 0 ;
        Console.Write(string.Format("die2({0}) |", j + 1 )) ;
        for ( i = 0 ; i < 6 ; i++ ){
            Console.Write(string.Format("{0,6}", cross_sum[j,i] )) ;
            sum+=cross_sum[j,i] ;
        }
        Console.WriteLine(string.Format("  :{0,6}", sum )) ;
    }
    Console.WriteLine("--------+--------------------------------------" ) ;
    Console.Write("         ") ;
    for ( i = 0 ; i < 6 ; i++ ){
        sum = 0 ;
        for ( j = 0 ; j < 6 ; j++ ){
            sum+=cross_sum[j,i] ;
        }
        Console.Write(string.Format("{0,6}", sum )) ;
    }
    Console.WriteLine();

/*   サイ１の推移表を表示する   */

    Console.WriteLine();
    Console.WriteLine("The sequence of die1");
    for( i=0 ; i < TOTAL_NUMBER-1 ; i++ ){
        sequence_one[(die_one[i+1]-1),(die_one[i]-1)]++ ;
    }

    Console.WriteLine("   die1 |   (1)   (2)   (3)   (4)   (5)   (6)" ) ;
    Console.WriteLine("--------+--------------------------------------" ) ;
    for ( j = 0 ; j < 6 ; j++ ){
        sum = 0 ;
        Console.Write(string.Format("next({0}) |", j + 1 )) ;
        for ( i = 0 ; i < 6 ; i++ ){
            Console.Write(string.Format("{0,6}", sequence_one[j,i] )) ;
            sum+=sequence_one[j,i] ;
        }
        Console.WriteLine(string.Format("  :{0,6}", sum )) ;
    }
    Console.WriteLine("--------+--------------------------------------" ) ;
    Console.Write("         ") ;
    for ( i = 0 ; i < 6 ; i++ ){
        sum = 0 ;
        for ( j = 0 ; j < 6 ; j++ ){
            sum+=sequence_one[j,i] ;
        }
        Console.Write(string.Format("{0,6}", sum )) ;
    }
    Console.WriteLine();

/*   サイ2の推移表を表示する   */

    Console.WriteLine();
    Console.WriteLine("The sequence of die2");
    for( i=0 ; i < TOTAL_NUMBER-1 ; i++ ){
        sequence_two[(die_two[i+1]-1),(die_two[i]-1)]++ ;
    }

    Console.WriteLine("   die2 |   (1)   (2)   (3)   (4)   (5)   (6)" ) ;
    Console.WriteLine("--------+--------------------------------------" ) ;
    for ( j = 0 ; j < 6 ; j++ ){
        sum = 0 ;
        Console.Write(string.Format("next({0}) |", j + 1 )) ;
        for ( i = 0 ; i < 6 ; i++ ){
            Console.Write(string.Format("{0,6}", sequence_one[j,i] )) ;
            sum+=sequence_two[j,i] ;
        }
        Console.WriteLine(string.Format("  :{0,6}", sum )) ;
    }
    Console.WriteLine("--------+--------------------------------------" ) ;
    Console.Write("         ") ;
    for ( i = 0 ; i < 6 ; i++ ){
        sum = 0 ;
        for ( j = 0 ; j < 6 ; j++ ){
            sum+=sequence_two[j,i] ;
        }
        Console.Write(string.Format("{0,6}", sum )) ;
    }
    Console.WriteLine();

/*   ３連続の出現表を表示する   */

    Console.WriteLine();
    Console.WriteLine("3 sequence of die1  die2");
    for ( j = 0 ; j < 6 ; j++){
        sum1 = 0 ;
        sum2 = 0 ;
        for( i=0 ; i < TOTAL_NUMBER-2 ; i++ ){
            if ( die_one[i] == j + 1 && die_one[i+1] == j+1 && die_one[i+2] == j+1 ){
                sum1++ ;
            }
            if ( die_two[i] == j + 1 && die_two[i+1] == j+1 && die_two[i+2] == j+1 ){
                sum2++ ;
            }
        }
        Console.WriteLine(string.Format("       {0}{1}{2} {3,6}{4,6}", j+1,j+1,j+1,sum1, sum2 ));
    }

/*   ４連続の出現表を表示する   */

    Console.WriteLine();
    Console.WriteLine("4 sequence of die1  die2");
    for ( j = 0 ; j < 6 ; j++){
        sum1 = 0 ;
        sum2 = 0 ;
        for( i=0 ; i < TOTAL_NUMBER-3 ; i++ ){
            if ( die_one[i] == j + 1 && die_one[i+1] == j+1 && die_one[i+2] == j+1 && die_one[i+3] == j+1 ){
                sum1++ ;
            }
            if ( die_two[i] == j + 1 && die_two[i+1] == j+1 && die_two[i+2] == j+1 && die_two[i+3] == j+1 ){
                sum2++ ;
            }
        }
        Console.WriteLine(string.Format("      {0}{1}{2}{3} {4,6}{5,6}", j+1,j+1,j+1,j+1,sum1, sum2 ));
    }

/*   ５連続の出現表を表示する   */

    Console.WriteLine();
    Console.WriteLine("5 sequence of die1  die2");
    for ( j = 0 ; j < 6 ; j++){
        sum1 = 0 ;
        sum2 = 0 ;
        for( i=0 ; i < TOTAL_NUMBER-4 ; i++ ){
            if ( die_one[i] == j + 1 && die_one[i+1] == j+1 && die_one[i+2] == j+1 && die_one[i+3] == j+1 && die_one[i+4] == j+1 ){
                sum1++ ;
            }
            if ( die_two[i] == j + 1 && die_two[i+1] == j+1 && die_two[i+2] == j+1 && die_two[i+3] == j+1 && die_two[i+4] == j+1 ){
                sum2++ ;
            }
        }
        Console.WriteLine(string.Format("     {0}{1}{2}{3}{4} {5,6}{6,6}", j+1,j+1,j+1,j+1,j+1,sum1, sum2 ));
    }

/*   ６連続の出現表を表示する   */

    Console.WriteLine();
    Console.WriteLine("6 sequence of die1  die2");
    for ( j = 0 ; j < 6 ; j++){
        sum1 = 0 ;
        sum2 = 0 ;
        for( i=0 ; i < TOTAL_NUMBER-5 ; i++ ){
            if ( die_one[i] == j + 1 && die_one[i+1] == j+1 && die_one[i+2] == j+1 && die_one[i+3] == j+1 && die_one[i+4] == j+1 && die_one[i+5] == j+1 ){
                sum1++ ;
            }
            if ( die_two[i] == j + 1 && die_two[i+1] == j+1 && die_two[i+2] == j+1 && die_two[i+3] == j+1 && die_two[i+4] == j+1 && die_two[i+5] == j+1 ){
                sum2++ ;
            }
        }
        Console.WriteLine(string.Format("    {0}{1}{2}{3}{4}{5} {6,6}{7,6}", j+1,j+1,j+1,j+1,j+1,j+1,sum1, sum2 ));
    }


    }
}