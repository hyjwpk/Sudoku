using UnityEngine;
using UnityEditor;

public class JarodDeletePlayerPrefs
{
    [MenuItem("Assets/PlayerPrefs_DeleteAll")]
    static void PlayerPrefsDeleteAll()//添加删除存档功能
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("DeleteAll finish!");
    }
}