using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Multithreading : MonoBehaviour
{

    static public void solve()//测试函数
    {
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                if (Data.solution[i][j] == 0)
                {
                    for (int k = 1; k <= 9; k++)
                    {
                        Data.solution[i][j] = (char)k;
                        if (Sudoku.judge(ref Data.solution, false))
                        {
                            List<List<char>> branch = new List<List<char>>();//复制solution中的内容到branch,避免不同线程读取时solution改变
                            Data.initboard(ref branch);
                            for (int m = 0; m < 9; m++)
                                for (int n = 0; n < 9; n++)
                                    branch[m][n] = Data.solution[m][n];
                            Thread thread = new Thread(new ParameterizedThreadStart(solve));
                            thread.Start(branch);
                        }
                    }
                    return;//仅通过第一个空格来分支多线程
                }

    }
    static void solve(object obj)//求解线程函数
    {
        List<List<char>> solution = (List<List<char>>)obj;
        List<List<int>> current = null;
        DLX sudoku = Sudoku.solve_SudokuMuliti(ref solution, ref current);
        if (sudoku.solve.Count != 0)
        {
            Data.sudoku.Add(sudoku);
            Data.matrix.Add(current);
        }
    }
}
