using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramEditManager : MonoBehaviourWithStatemachine<ProgramEditManager.State> {
    public enum State
    {
        Init,
        Wait,
        End
    }

    private ProgramFormat _program;

    [SerializeField] private ProgramViewer _programView;
    //CommandListViewer
    //UnitProfileViewer
    IEnumerator Init()
    {
        _program = new ProgramFormat();
        InitView();
        yield return null;
    }

    ProgramFormat LoadProgram(string path)
    {
        ProgramFormat ret = null;
        return null;
    }
    void InitView()
    {
        if (_program == null)
        {
            return;
        }

        for (int i = 0; i < _program.OrderList.GetLength(0); i++)
        {
            for (int j = 0; j < _program.OrderList.GetLength(1); j++)
            {
                _programView.SetBlock(i,j,_program.OrderList[i,j]);
            }
        }
        
    }
}
