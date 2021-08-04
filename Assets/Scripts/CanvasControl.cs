using UnityEngine;

public class CanvasControl : MonoBehaviour
{
    public GameObject prefab; public Transform father;

    void Start()
    {
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
            {
                Data.unitarray[i, j] = Instantiate(prefab);
                Data.unitarray[i, j].transform.GetComponent<Unit>().i = i; Data.unitarray[i, j].transform.GetComponent<Unit>().j = j;
                Data.unitarray[i, j].GetComponent<RectTransform>().localPosition = new Vector3((j / 3) * 20 + 95 * j + 1920 / 2 - 700, -(i / 3) * 20 - 95 * i + 1080 / 2 + 420);
                Data.unitarray[i, j].transform.SetParent(father);
            }
        //PlayerPrefs.DeleteAll();
        UIButton.generate();//游戏开始时生成新游戏
    }

    void Update()//检测游戏是否完成
    {
        if (Sudoku.isfinish(ref Data.condition))
            transform.GetComponent<Animator>().SetBool("End", true);
    }
}
