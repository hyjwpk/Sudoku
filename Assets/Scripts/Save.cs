using UnityEngine;
using System;

public static class PlayerPrefsX
{
    public static void SetInt2Array(string key, int[,] int2Array)//保存二维数组
    {
        int[] intArray = new int[81];
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                intArray[i * 9 + j] = int2Array[i, j];
        SetIntArray(key, intArray);
    }

    public static int[,] GetInt2Array(string key)//获取二维数组
    {
        int[] intArray = GetIntArray(key);
        if (intArray.Length == 0)
        {
            intArray = new int[81];
            Array.Clear(intArray, 0, intArray.Length);
        }
        int[,] int2Array = new int[10, 10];
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                int2Array[i, j] = intArray[i * 9 + j];
        return int2Array;
    }

    public static bool SetIntArray(string key, int[] intArray)//保存一维数组
    {
        if (intArray.Length == 0) return false;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < intArray.Length - 1; i++)
            sb.Append(intArray[i]).Append("|");
        sb.Append(intArray[intArray.Length - 1]);

        try { PlayerPrefs.SetString(key, sb.ToString()); }
        catch { return false; }
        return true;
    }

    public static int[] GetIntArray(string key)//获取一维数组
    {
        if (PlayerPrefs.HasKey(key))
        {
            string[] stringArray = PlayerPrefs.GetString(key).Split("|"[0]);
            int[] intArray = new int[stringArray.Length];
            for (int i = 0; i < stringArray.Length; i++)
                intArray[i] = Convert.ToInt32(stringArray[i]);
            return intArray;
        }
        return new int[0];
    }
}