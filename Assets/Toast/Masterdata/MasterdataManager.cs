﻿using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

//stateless
//Exception : 初期化してください =>InitMasterdataAsync
public class MasterdataManager : SingletonMonoBehaviour<MasterdataManager>
{
 //   Dictionary<Type,MasterTable<>>
    public static T Get<T>(int id) where T : class,IMasterRecord
    {
        return MasterTable<T>.Instance.Get(id);
        /*
        if (typeof(T) == typeof(MstBulletRecord))
        {
            return (T)(object)MstBulletRecordTable.First(_ => _.id == id);
        }
        return null;*/
    }

    public static T[] Records<T>() where T : class, IMasterRecord
    {
        return MasterTable<T>.Instance.Records.Records;
    }
    /*
    public static MstBulletRecord[] MstBulletRecordTable = {
        new MstBulletRecord(){id = 10,gravy = -20f,gravx = 0.95f,rad = 10,weight = 1f}
    };*/
    /*
    protected override void Awake()
    {

        base.Awake();
        InitMasterdata(FindMasterdata());
    }
    */

    //initialize
    protected override void Awake()
    {

        base.Awake();
        InitMasterdata();//Todo:データがでかくなってきたら非同期で別スクリプトから読むようにする。
    }
    void InitRecord<T>() where T:class,IMasterRecord
    {
        Attribute[] attributes = Attribute.GetCustomAttributes(typeof(T), typeof(MasterPath));
        //object[] attributes = typeof(T).GetCustomAttributes(typeof(MasterPath),false);
        if (attributes == null || attributes.Length == 0)
        {
            throw new InvalidOperationException("The provided object is not serializable");
           
        }
        MasterPath path = attributes[0] as MasterPath;
        if (path == null)
        {
            return;
        }
        Debug.Log("Load Master : "+typeof(T));
        MasterTable<T>.Instance.Init(path.Path);
        Debug.Log(MasterTable<T>.Instance.Records.Records.Length);
    }
    static Type[] GetInterfaces<T>()
    {
        return Assembly.GetExecutingAssembly().GetTypes().Where(c => c.GetInterfaces().Any(t => t == typeof(T))).ToArray();
    }
    Type[] FindMasterdata()
    {
        var masterDataList = GetInterfaces<IMasterRecord>();
        Debug.Log(masterDataList.Length);
        return masterDataList;
    }
    public IEnumerator InitMasterdataAsync()
    {
        var masterDataList = FindMasterdata();
        Debug.Log("Listup Masterdata Class.");
        yield return null;
        var method = typeof(MasterdataManager).GetMethod("InitRecord", BindingFlags.Public |
                                                          BindingFlags.NonPublic |
                                                          BindingFlags.Instance |
                                                          BindingFlags.Static |
                                                          BindingFlags.DeclaredOnly);
        if (method == null)
        {
            Debug.Log("null");
            yield break;

        }
        yield return null;
        foreach (var type in masterDataList)
        {
            var generic = method.MakeGenericMethod(type);
            generic.Invoke(this, null);
            yield return null;
            // InitRecord<>();
        }
        Debug.Log("Initialized.");
    }

    public void InitMasterdata()
    {
        var masterDataList = FindMasterdata();
        Debug.Log("Listup Masterdata Class.");
        var method = typeof(MasterdataManager).GetMethod("InitRecord", BindingFlags.Public |
                                                                       BindingFlags.NonPublic |
                                                                       BindingFlags.Instance |
                                                                       BindingFlags.Static |
                                                                       BindingFlags.DeclaredOnly);
        if (method == null)
        {
            Debug.Log("null");

        }
        foreach (var type in masterDataList)
        {
            var generic = method.MakeGenericMethod(type);
            generic.Invoke(this, null);
        }
        Debug.Log("Initialized.");

    }
}
