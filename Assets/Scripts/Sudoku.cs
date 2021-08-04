using System.Collections.Generic;
using System;

public class Sudoku
{

    static private int[] line = new int[9];
    static private int[] column = new int[9];
    static private int[,] block = new int[3, 3];
    static private int[] diagonal = new int[2];//x数独
    static private bool valid;
    static private Random ra = new Random(0);
    static private List<KeyValuePair<int, int>> spaces = new List<KeyValuePair<int, int>>();

    static private int __builtin_ctz(int x)//获取二进制数末尾的0的个数
    {
        int digit = 0;
        while ((x & (1 << digit)) == 0) digit++;
        return digit;
    }

    static private KeyValuePair<int, int> find(ref List<List<char>> board)
    {
        int max = int.MaxValue; KeyValuePair<int, int> pos = new KeyValuePair<int, int>(-1, -1);
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
            {
                if (board[i][j] == 0)
                {
                    int mask = line[i] | column[j] | block[i / 3, j / 3];
                    if (i == j) mask |= diagonal[0]; if (i + j == 8) mask |= diagonal[1];//x数独
                    mask = ~(mask) & 0x1ff;
                    if (mask == 0) return new KeyValuePair<int, int>(-1, -1);
                    int num = 1;
                    while ((mask &= (mask - 1)) != 0) num++;
                    if (num == 1) return new KeyValuePair<int, int>(i, j);
                    if (num < max) pos = new KeyValuePair<int, int>(i, j);
                }
            }
        return pos;
    }

    static private void flip(int i, int j, int digit)//改变某一位状态
    {
        line[i] ^= (1 << digit);
        column[j] ^= (1 << digit);
        block[i / 3, j / 3] ^= (1 << digit);
        if (i == j) diagonal[0] ^= 1 << digit; if (i + j == 8) diagonal[1] ^= 1 << digit;//x数独
    }

    static private void dfs(ref List<List<char>> board, int pos)//深度优先搜索
    {
        if (pos == spaces.Count)
        {
            valid = true;
            return;
        }
        int i = spaces[pos].Key, j = spaces[pos].Value;
        int mask = line[i] | column[j] | block[i / 3, j / 3];
        if (i == j) mask |= diagonal[0]; if (i + j == 8) mask |= diagonal[1];//x数独
        mask = ~(mask) & 0x1ff;
        for (; mask != 0 && !valid; mask &= (mask - 1))
        {
            int digit = __builtin_ctz(mask);
            flip(i, j, digit);
            board[i][j] = (char)(digit + 1);
            dfs(ref board, pos + 1);
            flip(i, j, digit);
        }
    }

    static private void gbfs(ref List<List<char>> board, int pos)//贪婪最佳优先搜索
    {
        if (pos == spaces.Count)
        {
            valid = true;
            return;
        }
        KeyValuePair<int, int> location = find(ref board);
        int i = location.Key, j = location.Value;
        if (i == -1) return;
        int mask = line[i] | column[j] | block[i / 3, j / 3];
        if (i == j) mask |= diagonal[0]; if (i + j == 8) mask |= diagonal[1];//x数独
        mask = ~(mask) & 0x1ff;
        for (; mask != 0 && !valid; mask &= (mask - 1))
        {
            int digit = __builtin_ctz(mask);
            flip(i, j, digit);
            board[i][j] = (char)(digit + 1);
            gbfs(ref board, pos + 1);
            if (valid) return;
            flip(i, j, digit);
            board[i][j] = (char)(0);
        }
    }

    static public bool judge(ref List<List<char>> board, bool output)//整个数独判断
    {
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                if (board[i][j] < 0 || board[i][j] > 9)
                    return false;

        Array.Clear(line, 0, line.Length);
        Array.Clear(column, 0, column.Length);
        Array.Clear(block, 0, block.Length);
        Array.Clear(diagonal, 0, diagonal.Length);//x数独
        valid = false;
        for (int i = 0; i < 9; ++i)
            for (int j = 0; j < 9; ++j)
                if (board[i][j] != 0)
                {
                    int digit = board[i][j] - 1;
                    int mask = line[i] | column[j] | block[i / 3, j / 3];
                    if (i == j) mask |= diagonal[0]; if (i + j == 8) mask |= diagonal[1];//x数独
                    if (((1 << digit) & mask) != 0)
                        return false;
                    flip(i, j, digit);
                }
        return true;
    }

    static public bool solveSudoku(ref List<List<char>> board)//dfs求解
    {
        Array.Clear(line, 0, line.Length);
        Array.Clear(column, 0, column.Length);
        Array.Clear(block, 0, block.Length);
        Array.Clear(diagonal, 0, diagonal.Length);//x数独
        valid = false;
        spaces.Clear();
        for (int i = 0; i < 9; ++i)
            for (int j = 0; j < 9; ++j)
                if (board[i][j] != 0)
                {
                    int digit = board[i][j] - 1;
                    flip(i, j, digit);
                }
        for (int i = 0; i < 9; ++i)
            for (int j = 0; j < 9; ++j)
                if (board[i][j] == 0)
                    spaces.Add(new KeyValuePair<int, int>(i, j));
        dfs(ref board, 0);
        return valid;
    }

    static public bool solveSudokuid(ref List<List<char>> board)//gbfs求解
    {
        Array.Clear(line, 0, line.Length);
        Array.Clear(column, 0, column.Length);
        Array.Clear(block, 0, block.Length);
        Array.Clear(diagonal, 0, diagonal.Length);//x数独
        valid = false;
        spaces.Clear();
        for (int i = 0; i < 9; ++i)
            for (int j = 0; j < 9; ++j)
                if (board[i][j] != 0)
                {
                    int digit = board[i][j] - 1;
                    flip(i, j, digit);
                }
        for (int i = 0; i < 9; ++i)
            for (int j = 0; j < 9; ++j)
                if (board[i][j] == 0)
                    spaces.Add(new KeyValuePair<int, int>(i, j));
        gbfs(ref board, 0);
        return valid;
    }

    static public void printSudoku(ref List<List<char>> board)//输出数独
    {
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                Data.unitarray[i, j].GetComponent<Unit>().Changevalue((int)(board[i][j]));
    }

    static public KeyValuePair<int, int> hint(ref List<List<char>> board)//提示
    {
        KeyValuePair<int, int> pos = new KeyValuePair<int, int>(-1, -1);
        if (isfinish(ref board)) return pos;
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                if (pos.Key == -1 && board[i][j] == (char)0) pos = new KeyValuePair<int, int>(i, j);
        if (solve_Sudoku(ref board))
            return pos;
        return new KeyValuePair<int, int>(-1, -1);
    }

    static public bool put(ref List<List<char>> board, int line, int column, int number)//填入数字
    {
        board[line][column] = (char)number;
        if (!judge(ref board, true))
        {
            board[line][column] = (char)0;
            return false;
        }
        return true;
    }

    public static void save(int number)//保存数独
    {
        int[,] condition = new int[9, 9];
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                condition[i, j] = (int)Data.condition[i][j];
        PlayerPrefsX.SetInt2Array("save" + number.ToString() + "-condition", condition);

        int[,] question = new int[9, 9];
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                question[i, j] = (int)Data.question[i][j];
        PlayerPrefsX.SetInt2Array("save" + number.ToString() + "-question", question);
    }

    public static void show(int number)//读取数独
    {
        int[,] condition = PlayerPrefsX.GetInt2Array("save" + number.ToString() + "-condition");
        Data.initboard(ref Data.condition);
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                Data.condition[i][j] = (char)condition[i, j];

        int[,] question = PlayerPrefsX.GetInt2Array("save" + number.ToString() + "-question");
        Data.initboard(ref Data.question);
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                Data.question[i][j] = (char)question[i, j];
    }

    static public void generate(ref List<List<char>> board)//生成数独
    {
        int step = 0;
        while (true)
        {
            int number, line, column;
            while (step <= 6)
            {
                number = ra.Next(1, 9); line = ra.Next(0, 8); column = ra.Next(0, 8);
                board[line][column] = (char)(number);
                if (!judge(ref board, false)) board[line][column] = (char)0;
                else step++;
            }
            if (solve_Sudoku(ref board)) break;
        }

        for (step = 1; step <= 60; step++)
        {
            int line = ra.Next(0, 9), column = ra.Next(0, 9);
            if (board[line][column] == 0) step--;
            board[line][column] = (char)(0);
        }
    }

    static public bool isfinish(ref List<List<char>> board)//判断游戏是否完成
    {
        for (int i = 0; i < 9; ++i)
            for (int j = 0; j < 9; ++j)
                if (board[i][j] == 0)
                    return false;
        return true;
    }

    static public bool solve_Sudoku(ref List<List<char>> board)//DLX
    {

        char[] problem = new char[81];
        for (int i = 0; i < 81; i++) problem[i] = board[i / 9][i % 9];
        List<List<int>> matrix = DLX.sudoku2matrix(new string(problem));

        //DLX sudoku=new DLX(ref matrix, matrix.Count, 324);
        DLX sudoku = new DLX(ref matrix, matrix.Count, 342); //x数独
        if (!sudoku.Search()) return false;
        List<int> solution = sudoku.matrix2sudoku(ref matrix, sudoku.getResult());
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                board[i][j] = (char)solution[i * 9 + j];
        return true;
    }

    static public DLX solve_Sudokuall(ref List<List<char>> board, ref List<List<int>> matrix)//DLX求多解
    {

        char[] problem = new char[81];
        for (int i = 0; i < 81; i++) problem[i] = board[i / 9][i % 9];
        matrix = DLX.sudoku2matrix(new string(problem));
        //DLX sudoku=new DLX(ref matrix, matrix.Count, 324);
        DLX sudoku = new DLX(ref matrix, matrix.Count, 342); //x数独
        sudoku.Searchall();
        return sudoku;
    }

    static public DLX solve_SudokuMuliti(ref List<List<char>> board, ref List<List<int>> matrix)//DLX多线程求解
    {

        char[] problem = new char[81];
        for (int i = 0; i < 81; i++) problem[i] = board[i / 9][i % 9];
        matrix = DLX.sudoku2matrix(new string(problem));
        //DLX sudoku=new DLX(ref matrix, matrix.Count, 324);
        DLX sudoku = new DLX(ref matrix, matrix.Count, 342); //x数独
        sudoku.Searchall();
        return sudoku;
    }

}