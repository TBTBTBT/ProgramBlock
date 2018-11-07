using System;
//-----------------------------------------------------------------------------

//マスターデータの定義ファイル
//定義方法 
// IMasterRecordを実装したクラスを作る
// MasterPath属性でjsonのパスを指定(Asset/Resource以下)
//ひな形
//[MasterPath("")]
//public class MstNameRecord : IMasterRecord{public int id { get; set; }}

//-----------------------------------------------------------------------------

[Serializable]
[MasterPath("/Master/mst_unit.json")]
public class MstUnitRecord : IMasterRecord
{
    public int id { get; set; }
    public int atk { get; set; }
    public int def { get; set; }
    public int spd { get; set; }

    public int allowequipid { get; set; }
    public string jobname { get; set; }
}
[Serializable]
[MasterPath("/Master/mst_func.json")]
public class MstFunctionRecord : IMasterRecord
{
    public int id { get; set; }
    public string key { get; set; }
    public string name { get; set; }
    public int waitframe { get; set; }

}