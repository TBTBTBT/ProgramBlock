using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FishMasterData :SingletonMonoBehaviour<FishMasterData>{
    public class BodyData{
        public int Weight { get; set; }
        public int Height { get; set; }
        public int Attack { get; set; }
    }
    public class EyeData{
        public int Aggressive { get; set; }
        public int Attack { get; set; }
        public int Sight { get; set; }
    }
    public class FinData
    {
        public int Weight { get; set; }
        public int Height { get; set; }
        public int Attack { get; set; }
        public int Sight { get; set; }
    }

    public static List<BodyData> BodyDatas;
    public static List<EyeData>  EyeDatas;
    public static List<FinData>  FinDatas;
    public static readonly Dictionary<PartsType, string> MaterialPath = new Dictionary<PartsType, string>()
    {
        {PartsType.Body ,"Material/Body/Body_"},
        {PartsType.Eye  ,"Material/Eye/Eye_"},
        {PartsType.Fin  ,"Material/Fin/Fin_"}

    };
  	protected override void Awake()
	{
        base.Awake();
        LoadMaster();
        DontDestroyOnLoad(this.gameObject);
	}

	public static BodyData GetBody(int id) {
        if(BodyDatas.Count > id){
            return BodyDatas[id];
        }
        return null;
    }
    public static EyeData GetEye(int id)
    {
        if (EyeDatas.Count > id)
        {
            return EyeDatas[id];
        }
        return null;
    }
    public static FinData GetFin(int id)
    {
        if (FinDatas.Count > id)
        {
            return FinDatas[id];
        }
        return null;
    }

    public static void LoadMaster(){
        LoadBody();
        LoadEye();
        LoadFin();
        Debug.Log("MasterData Loaded.");

    }
    static void LoadBody(){
        BodyDatas = new List<BodyData>(){
            new BodyData(){Weight = 3, Height = 3, Attack = 3},
            new BodyData(){Weight = 1, Height = 3, Attack = 3}
        };
    }
    static void LoadEye(){
        EyeDatas = new List<EyeData>(){
            new EyeData(){ Attack = 3, Aggressive = 3 ,Sight = 3},
            new EyeData(){ Attack = 3, Aggressive = 3 ,Sight = 3}
        };
        
    }
    static void LoadFin()
    {
        FinDatas = new List<FinData>(){
            new FinData(){Weight = 3, Attack = 3 ,Sight = 3},
            new FinData(){Weight = 3, Attack = 3 ,Sight = 3}
        };
    }
}
[System.Serializable]
public class FishData
{
    public int Id = -1;
    public PartsData Body = new PartsData();// 0:head 1:eye
    public PartsData Eye = new PartsData();
    public List<PartsData> Fin = new List<PartsData>();
    public FishData(){
        Body = new PartsData() { _id = -1, _pos = new Vector2(0,0) };
        Eye = new PartsData() { _id = -1, _pos = new Vector2(0, 0)  };
    }
    public void AddParts(PartsType type, int id, Vector2 pos)
    {
        switch (type)
        {
            case PartsType.Body:
                Body._id = id;
                Body._pos = pos;
                break;
            case PartsType.Eye:
                Eye._id = id;
                Eye._pos = pos;
                break;
            case PartsType.Fin:
                if(Fin.Count < 4)
                Fin.Add(new PartsData(){_id = id,_pos = pos});
                break;
        }

    }
    public void ChangePartsPos(int index,Vector2 pos){
        if (index == 0)
        {
            //Body._id = id;
            Body._pos = pos;
        }

        if (index == 1)
        {
           // Eye._id = id;
            Eye._pos = pos;
        }

        if (index >= 2)
        {
            int i = index - 2;
            if (i < Fin.Count)
            {
             //   Fin[i]._id = id;
                Fin[i]._pos = pos;
            }
        }
    }
    public void RemoveParts(int index){
        //フィンの場合つめる
        int finIndex = index - 2;
        if (finIndex  >= 0 && finIndex < Fin.Count)
        {

            Fin.RemoveAt(finIndex);
            Debug.Log("RemoveFinAt" + finIndex);
            Debug.Log("Fincount" + Fin.Count);

        }
        else
        {
            Eye._id = -1;
        }
    }

}
public class LocalDataManager
{

    public static void SaveFishData(FishData data)
    {
        string directoryPath = Application.dataPath + "/Savedata/";
        string dataPath = "fish.txt";
        var json = JsonUtility.ToJson(data);
        var path = directoryPath + dataPath;
        var writer = new StreamWriter(path, false); // 上書き
        writer.WriteLine(json);
        writer.Flush();
        writer.Close();
        Debug.Log("Save");
    }

    public void LoadDeckData(int i)
    {

    }
    public static FishData LoadFishData(int id)
    {
        string directoryPath = Application.dataPath + "/Savedata/"; 
        string dataPath = "fish.txt";
        var info = new FileInfo(directoryPath + dataPath);
        var reader = new StreamReader(info.OpenRead());
        var json = reader.ReadToEnd();
        var data = JsonUtility.FromJson<FishData>(json);
        Debug.Log("Load");
        return data;
    }
}

public class DeckData
{
    public class IdPos
    {
        public int Id { get; set; }
        public Vector2 Pos { get; set; }
    } 
    public int Id { get; set; }
    public List<IdPos> _fish = new List<IdPos>();
}
[System.Serializable]
public class PartsData{
    public int _id = 0;
    public Vector2 _pos = new Vector2(0,0);


}
public enum PartsType
{
    Body,
    Eye,
    Fin
}