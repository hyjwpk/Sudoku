using System.Collections.Generic;
using UnityEngine;

public class Data
{
    static public GameObject[,] unitarray = new GameObject[10, 10];
    static public List<List<char>> question = new List<List<char>>(9);
    static public List<List<char>> condition = new List<List<char>>(9);
    static public List<List<char>> solution = new List<List<char>>(9);
    static public int now;//当前sudoku的解的序数,从1开始
    static public int sudokunum;//当前的sudoku的数组下标,从0开始
    static public List<DLX> sudoku = new List<DLX>();
    static public List<List<List<int>>> matrix = new List<List<List<int>>>();

    static public void initboard(ref List<List<char>> board)//初始化
    {
        board.Clear();
        for (int i = 0; i < 9; i++)
        {
            List<char> row = new List<char> { (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0 };
            board.Add(row);
        }
    }

    static public void copyboard(ref List<List<char>> des, ref List<List<char>> sorce)//复制
    {
        des.Clear();
        for (int i = 0; i < 9; i++)
        {
            List<char> row = new List<char>();
            for (int j = 0; j < 9; j++)
                row.Add(sorce[i][j]);
            des.Add(row);
        }
    }

}
