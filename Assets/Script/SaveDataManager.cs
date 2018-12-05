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

    private readonly Dictionary<DataType, string> _firstData = new Dictionary<DataType, string>()
    {
        {DataType.Program,""},
        {DataType.Command,"1,2,3,4" }
    };
    public void SaveAnyway()
    {

    }
    //pageは0から
    public IEnumerator Save(DataType type,int page,string data,bool checkOverride) 
    {
        if (PlayerPrefs.HasKey(type.ToString()))
        {
            //Todo:上書きしますか？ダイアログ
        }
        PlayerPrefs.SetString($"{type.ToString()}_{page}",data);
        PlayerPrefs.Save();
        yield return null;
    }

    string FirstData(DataType type, int page)
    {
        if (_firstData.ContainsKey(type))
        {
            return _firstData[type];
        }

        return "";
    }
    public string Load(DataType type,int page)
    {
        if (PlayerPrefs.HasKey($"{type.ToString()}_{page}"))
        {
            return PlayerPrefs.GetString($"{type.ToString()}_{page}");
        }
        else
        {
            return FirstData(type, page);
        }
        return "";
    }
}
