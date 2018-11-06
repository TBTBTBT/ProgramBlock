using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kernel: MonoBehaviourWithStatemachine<Kernel.State>
{
    public enum State
    {
        Boot,
        Init,


    }

    private MachineCore _machine;
    private List<ModuleBase> _modules;

    private List<ProcessorBase> _processores;

    IEnumerator Boot()
    {
        //カーネル初期化
        //ハードウエア初期化
        yield return null;
    }

    IEnumerator Init()
    {
        //プロセッサ初期化
        //モジュール初期化
        yield return null;
    }
    //ConnectModule
    //RemoveModule
    //
}
//heatX
//hp
//