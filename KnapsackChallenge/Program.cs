using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnapsackChallenge
{
    /// <summary>
    /// Solution to 0-1 Knapsack Problem via Genetic Algorithm.
    /// 
    /// Author: Vuk Pejovic
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            List<int[]> TEST_WEIGHTS = new List<int[]>();
            List<int[]> TEST_VALUES = new List<int[]>();
            int[] TEST_KNAPSACK = new int[] { 165, 26, 190, 50, 104, 170, 750, 6404180 };

            TEST_WEIGHTS.Add(new int[] { 23, 31, 29, 44, 53, 38, 63, 85, 89, 82 });
            TEST_WEIGHTS.Add(new int[] { 12, 7, 11, 8, 9 });
            TEST_WEIGHTS.Add(new int[] { 56, 59, 80, 64, 75, 17 });
            TEST_WEIGHTS.Add(new int[] { 31, 10, 20, 19, 4, 3, 6 });
            TEST_WEIGHTS.Add(new int[] { 25, 35, 45, 5, 25, 3, 2, 2 });
            TEST_WEIGHTS.Add(new int[] { 41, 50, 49, 59, 55, 57, 60 });
            TEST_WEIGHTS.Add(new int[] { 70, 73, 77, 80, 82, 87, 90, 94, 98, 106, 110, 113, 115, 118, 120 });
            TEST_WEIGHTS.Add(new int[] { 382745, 799601, 909247, 729069, 467902, 44328, 34610, 698150, 823460, 903959, 853665, 551830, 610856, 670702, 488960, 951111, 323046, 446298, 931161, 31385, 496951, 264724, 224916, 169684 });

            TEST_VALUES.Add(new int[] { 92, 57, 49, 68, 60, 43, 67, 84, 87, 72 });
            TEST_VALUES.Add(new int[] { 24, 13, 23, 15, 16 });
            TEST_VALUES.Add(new int[] { 50, 50, 64, 46, 50, 5 });
            TEST_VALUES.Add(new int[] { 70, 20, 39, 37, 7, 5, 10 });
            TEST_VALUES.Add(new int[] { 350, 400, 450, 20, 70, 8, 5, 5 });
            TEST_VALUES.Add(new int[] { 442, 525, 511, 593, 546, 564, 617 });
            TEST_VALUES.Add(new int[] { 135, 139, 149, 150, 156, 163, 173, 184, 192, 201, 210, 214, 221, 229, 240 });
            TEST_VALUES.Add(new int[] { 825594, 1677009, 1676628, 1523970, 943972, 97426, 69666, 1296457, 1679693, 1902996, 1844992, 1049289, 1252836, 1319836, 953277, 2067538, 675367, 853655, 1826027, 65731, 901489, 577243, 466257, 369261 });

            // Test cases from : http://people.sc.fsu.edu/~jburkardt%20/datasets/knapsack_01/knapsack_01.html
            // RESULT 1 : 1111010000
            // RESULT 2 : 01110
            // RESULT 3 : 110010
            // RESULT 4 : 1001000
            // RESULT 5 : 10111011
            // RESULT 6 : 1001001 || The best solution is actually 0101001 which this algorithm finds every time.
            // RESULT 7 : Profit: 1458
            // RESULT 8 : Profit: 13549094

            while (true)
            {
                Console.WriteLine("Test Case [0-7]:");
                Console.SetCursorPosition(Console.CursorLeft + 17, Console.CursorTop - 1);

                String input = Console.ReadLine();

                int TEST_CASE;

                if (!int.TryParse(input, out TEST_CASE) || !Enumerable.Range(0, 8).Contains(TEST_CASE))
                {
                    break;
                }

                Challenger winner = new Challenger(TEST_KNAPSACK[TEST_CASE], TEST_WEIGHTS[TEST_CASE], TEST_VALUES[TEST_CASE]);
                winner.Go();
            }

        }
    }
}
