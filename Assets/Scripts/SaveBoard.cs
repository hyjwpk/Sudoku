using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SaveBoard : MonoBehaviour
{

    public static int inputnumber;
    void Start()//存档界面初始化
    {
        inputnumber = 0;
        transform.Find("Save1/Text").GetComponent<Text>().text = "存档1 " +
        PlayerPrefs.GetString("save1-string") + PlayerPrefs.GetString("name1");
        transform.Find("Save2/Text").GetComponent<Text>().text = "存档2 " +
        PlayerPrefs.GetString("save2-string") + PlayerPrefs.GetString("name2");
        transform.Find("Save3/Text").GetComponent<Text>().text = "存档3 " +
        PlayerPrefs.GetString("save3-string") + PlayerPrefs.GetString("name3");
        transform.Find("Save4/Text").GetComponent<Text>().text = "存档4 " +
        PlayerPrefs.GetString("save4-string") + PlayerPrefs.GetString("name4");
        transform.Find("Save5/Text").GetComponent<Text>().text = "存档5 " +
        PlayerPrefs.GetString("save5-string") + PlayerPrefs.GetString("name5");

    }

    public void save(int number)//存档
    {
        inputnumber = number;
        Sudoku.save(number);
        PlayerPrefs.SetString("save" + number.ToString() + "-string", time());
        PlayerPrefs.SetString("name" + inputnumber.ToString(), "");
        transform.Find("Name/Text2").GetComponent<Text>().text = "请使用键盘输入字母……";
        transform.Find("Save" + number.ToString() + "/Text").GetComponent<Text>().text = "存档" + number.ToString() + " " +
        PlayerPrefs.GetString("save" + number.ToString() + "-string") + PlayerPrefs.GetString("name" + number.ToString());
    }

    public void load(int number)//读档
    {
        Sudoku.show(number);
        Sudoku.printSudoku(ref Data.condition);
    }

    public static String time() //获取当前时间
    {
        int hour = DateTime.Now.Hour, minute = DateTime.Now.Minute, second = DateTime.Now.Second,
        year = DateTime.Now.Year, month = DateTime.Now.Month, day = DateTime.Now.Day;
        return string.Format("{0:D2}:{1:D2}:{2:D2} " + "{3:D4}/{4:D2}/{5:D2}", hour, minute, second, year, month, day);
    }

    void Update()//读取键盘输入(仅能读取字母)
    {
        if (inputnumber != 0)
            if (Input.anyKeyDown)
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                    if (Input.GetKeyDown(keyCode) && ((int)keyCode) >= 97 && ((int)keyCode) <= 122)
                    {
                        char[] name = PlayerPrefs.GetString("name" + inputnumber.ToString(), "").ToCharArray();
                        PlayerPrefs.SetString("name" + inputnumber.ToString(), new String(name) + keyCode.ToString());
                        transform.Find("Save" + inputnumber.ToString() + "/Text").GetComponent<Text>().text = "存档" + inputnumber.ToString() + " " +
                        PlayerPrefs.GetString("save" + inputnumber.ToString() + "-string") + PlayerPrefs.GetString("name" + inputnumber.ToString());
                        transform.Find("Name/Text2").GetComponent<Text>().text = PlayerPrefs.GetString("name" + inputnumber.ToString());
                    }
    }
}
