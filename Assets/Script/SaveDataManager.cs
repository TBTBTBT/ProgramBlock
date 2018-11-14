using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//stateless
public class SaveDataManager : SingletonMonoBehaviour<SaveDataManager>
{
    public enum DataType
    {
        Program,//ページ数設定したい
        Command,//所有コマンド
        Gold,
        Name,
        Mission,

    }

    public void SaveAnyway()
    {

    }

    public IEnumerator Save(DataType type,string data,bool checkOverride) 
    {
        if (PlayerPrefs.HasKey(type.ToString()))
        {
            //Todo:上書きしますか？ダイアログ
        }
        PlayerPrefs.SetString(type.ToString(),data);
        PlayerPrefs.Save();
        yield return null;
    }

    public string Load(DataType type)
    {
        if (PlayerPrefs.HasKey(type.ToString()))
        {
            return PlayerPrefs.GetString(type.ToString());
        }

        return "";
    }
}
