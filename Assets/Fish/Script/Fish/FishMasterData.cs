using System.Collections;
using System.Collections.Generic;
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
            new BodyData(){Weight = 3, Height = 3, Attack = 3}
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
            new FinData(){Weight = 3, Height = 3, Attack = 3 ,Sight = 3},
            new FinData(){Weight = 3, Height = 3, Attack = 3 ,Sight = 3}
        };
    }
}
public class FishData
{
    public int Id { get; set; }
    public PartsData Body { get; set; }// 0:head 1:eye
    public PartsData Eye { get; set; }
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
                Fin.Add(new PartsData(){_id = id,_pos = pos});
                break;
        }

    }
    public void ChangeParts(){
        
    }


}
public class LocalDataManager
{
    public FishData LoadFishData(int id)
    {
        string dataPath = "Fish:" + id;
//        string idPath = "/Parts:";  
        FishData fish = new FishData();
        //List<PartsData> parts = PlayerPrefsUtility.LoadList<PartsData>(dataPath);
        //fish.Body = parts[0];
        //fish.Eye = parts[1];

//        FishData ret = PlayerPrefs.GetString();
        return fish;
    }
    public void SaveFishData(int id)
    {
        string dataPath = "Fish:" + id;

    }

    public void LoadDeckData(int id)
    {
        string dataPath = "Deck:" + id;
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