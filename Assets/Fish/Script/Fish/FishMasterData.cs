using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FishMasterData {
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
    public static List<EyeData> EyeDatas;
    public static List<FinData> FinDatas;
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
    public List<PartsData> _parts;// 0:head 1:eye 2~:fin 
    public void AddParts(int id,Vector3 pos){
        _parts.Add(new PartsData() { _id = id, _pos = pos });
    }
    public void Save(){
        string dataPath = "Fish:" + Id;

    }
    public void Load(){
        string dataPath = "Fish:" + Id;
    }
}
[System.Serializable]
public class PartsData{
    public int _id = 0;
    public Vector3 _pos = new Vector3(0,0,0);


}
