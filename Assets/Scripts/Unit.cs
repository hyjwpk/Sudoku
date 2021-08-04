using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerClickHandler
{
    public int value; public int i; public int j; public UnityEvent rightClick;

    void Start()
    {
        rightClick.AddListener(new UnityAction(ButtonRightClick));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            rightClick.Invoke();
    }

    private void ButtonRightClick()//右键删除数字
    {
        int i = transform.GetComponent<Unit>().i, j = transform.GetComponent<Unit>().j;
        if (Data.question[i][j] != (char)0) return;
        Data.condition[i][j] = (char)0;
        Sudoku.printSudoku(ref Data.condition);
    }

    public void Changevalue(int number)//修改单元的值
    {
        int i = transform.GetComponent<Unit>().i, j = transform.GetComponent<Unit>().j;
        if (number == 0)
            transform.Find("Value").GetComponent<Text>().text = "";
        else
            transform.Find("Value").GetComponent<Text>().text = number.ToString();
        if (Data.question[i][j] == 0)
            transform.Find("Value").GetComponent<Text>().color = new Color(1, 1, 1);
        else
            transform.Find("Value").GetComponent<Text>().color = new Color((71f / 255), (243f / 255), 1);
    }

    public void ChangevalueByButton(int number)//通过按钮修改单元的值
    {
        int i = transform.GetComponent<Unit>().i, j = transform.GetComponent<Unit>().j;
        if (Data.question[i][j] != 0) return;
        if (Sudoku.put(ref Data.condition, i, j, number))
            Sudoku.printSudoku(ref Data.condition);
        else
        {
            Sudoku.printSudoku(ref Data.condition);
            for (int m = 0; m < 9; m++)
                for (int n = 0; n < 9; n++)
                    if (Data.condition[m][n] == number)
                        Data.unitarray[m, n].transform.Find("Value").GetComponent<Text>().color = new Color(1, (50f / 255), 0);
        }
    }

    public void hide()
    {
        //隐藏全部数独块的按钮
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                Data.unitarray[i, j].transform.Find("Button").gameObject.SetActive(false);
    }

}