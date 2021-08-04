using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class UIButton : MonoBehaviour
{

    public Toggle DLX, DLXM, DFS, GBFS;

    void Start()//初始化求解算法按钮的监听器
    {
        DLX.onValueChanged.AddListener(DLXEvent);
        DLXM.onValueChanged.AddListener(DLXMEvent);
        DFS.onValueChanged.AddListener(DFSEvent);
        GBFS.onValueChanged.AddListener(GBFSEvent);
    }

    private void DLXEvent(bool arg0)//DLX算法
    {
        if (arg0 == true) DLXM.GetComponent<Toggle>().isOn = false;
        if (arg0 == true) DFS.GetComponent<Toggle>().isOn = false;
        if (arg0 == true) GBFS.GetComponent<Toggle>().isOn = false;
    }

    private void DLXMEvent(bool arg0)//DLXM
    {
        if (arg0 == true) DLX.GetComponent<Toggle>().isOn = false;
        if (arg0 == true) DFS.GetComponent<Toggle>().isOn = false;
        if (arg0 == true) GBFS.GetComponent<Toggle>().isOn = false;
    }

    private void DFSEvent(bool arg0)//DFX算法
    {
        if (arg0 == true) DLX.GetComponent<Toggle>().isOn = false;
        if (arg0 == true) DLXM.GetComponent<Toggle>().isOn = false;
        if (arg0 == true) GBFS.GetComponent<Toggle>().isOn = false;
    }

    private void GBFSEvent(bool arg0)//IDDFX算法
    {
        if (arg0 == true) DLX.GetComponent<Toggle>().isOn = false;
        if (arg0 == true) DLXM.GetComponent<Toggle>().isOn = false;
        if (arg0 == true) DFS.GetComponent<Toggle>().isOn = false;
    }

    public static void generate()//生成新游戏
    {
        Data.initboard(ref Data.question);
        Sudoku.generate(ref Data.question);
        Sudoku.printSudoku(ref Data.question);
        Data.copyboard(ref Data.condition, ref Data.question);
    }

    public static void hint()//提示
    {
        Data.copyboard(ref Data.solution, ref Data.condition);
        KeyValuePair<int, int> pos = Sudoku.hint(ref Data.solution);
        if (pos.Key != -1)
        {
            Data.condition[pos.Key][pos.Value] = Data.solution[pos.Key][pos.Value];
        }
        Sudoku.printSudoku(ref Data.condition);
    }

    public void solve()//解答
    {
        Data.copyboard(ref Data.solution, ref Data.condition);
        if (DLXM.GetComponent<Toggle>().isOn)
        {
            Data.now = 0;
            Data.sudokunum = 0;
            Data.sudoku.Clear();
            Data.matrix.Clear();
            Multithreading.solve();
            transform.Find("Solve/Next").gameObject.SetActive(true);
            transform.Find("Solve/Last").gameObject.SetActive(true);
        }
        if (DLX.GetComponent<Toggle>().isOn)
        {
            Sudoku.solve_Sudoku(ref Data.solution);
        }
        if (DFS.GetComponent<Toggle>().isOn) Sudoku.solveSudoku(ref Data.solution);
        if (GBFS.GetComponent<Toggle>().isOn) Sudoku.solveSudokuid(ref Data.solution);
        if (DLX.GetComponent<Toggle>().isOn || DFS.GetComponent<Toggle>().isOn || GBFS.GetComponent<Toggle>().isOn)
            Sudoku.printSudoku(ref Data.solution);
    }

    public static void nextandlast(ref List<List<char>> board, int number)
    {
        if (number > 0)
        {
            if (Data.now == Data.sudoku[Data.sudokunum].solve.Count && Data.sudokunum < Data.sudoku.Count - 1)
            {
                Data.now = 0; Data.sudokunum++;
            }
            if (Data.now < Data.sudoku[Data.sudokunum].solve.Count)
            {
                Data.now += number;
                List<List<int>> current = Data.matrix[Data.sudokunum];
                List<int> solution = Data.sudoku[Data.sudokunum].matrix2sudoku(ref current, Data.sudoku[Data.sudokunum].solve[Data.now - 1]);
                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                        board[i][j] = (char)solution[i * 9 + j];
                Sudoku.printSudoku(ref board);
            }
        }

        if (number < 0)
        {
            if (Data.now == 1 && Data.sudokunum > 0)
            {
                Data.sudokunum--; Data.now = Data.sudoku[Data.sudokunum].solve.Count + 1;
            }
            if (Data.now > 1)
            {
                Data.now += number;
                List<List<int>> current = Data.matrix[Data.sudokunum];
                List<int> solution = Data.sudoku[Data.sudokunum].matrix2sudoku(ref current, Data.sudoku[Data.sudokunum].solve[Data.now - 1]);
                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                        board[i][j] = (char)solution[i * 9 + j];
                Sudoku.printSudoku(ref board);
            }
        }

    }

    public void next()
    {
        if (DFS.GetComponent<Toggle>().isOn) return;
        nextandlast(ref Data.solution, 1);
        Sudoku.printSudoku(ref Data.solution);
    }

    public void last()
    {
        if (DFS.GetComponent<Toggle>().isOn) return;
        nextandlast(ref Data.solution, -1);
        Sudoku.printSudoku(ref Data.solution);
    }

    public static void input()//自定义游戏
    {
        Data.initboard(ref Data.condition);
        Data.initboard(ref Data.question);
        Sudoku.printSudoku(ref Data.condition);
    }

    public static void inputsave()//自定义游戏
    {
        int number = 0;
        for (int i = 0; i < 9; ++i)
            for (int j = 0; j < 9; ++j)
                if (Data.condition[i][j] != 0)
                    number++;
        if (number >= 17)
        {
            Data.copyboard(ref Data.question, ref Data.condition);
            Sudoku.printSudoku(ref Data.condition);
        }
        else generate();
    }

    public void exit()//退出游戏
    {
        StartCoroutine("exitFunc");
    }

    IEnumerator exitFunc()//退出游戏
    {
        PlayerPrefs.Save();
        yield return new WaitForSeconds(5);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void start()//设置进入游戏状态
    {
        transform.GetComponent<Animator>().SetBool("GameEntry", true);
    }

    public void end()//设置游戏结束状态
    {
        transform.GetComponent<Animator>().SetBool("End", false);
    }

    public void save(bool condition)//设置进入读取存档名称状态
    {
        transform.GetComponent<Animator>().SetBool("Save", condition);
        if (condition == false) SaveBoard.inputnumber = 0;
    }

    public void saveboard(bool condition)//设置进入存档面板状态
    {
        transform.GetComponent<Animator>().SetBool("SaveBoard", condition);
    }

}
