using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramEditManager : MonoBehaviourWithStatemachine<ProgramEditManager.State> {
    public enum State
    {
        Init,
        Wait,
        SelectCommandList,
        SelectProgramView,
        Save,
        End
    }

    public enum TouchState
    {
        None,
        SelectListCommand,
        SelectViewCommand
    }
    private ProgramFormat _program;
    private List<ProgramFormat.OrderFormat> _commandList;
    private TouchState _touchState = TouchState.None;
    [SerializeField] private ProgramViewer _programView;
    [SerializeField] private CommandListViewer _commandListViewer;
    //CommandListViewer
    //UnitProfileViewer

    private int _viewX = 0;//なくしたい
    private int _viewY = 0;

    private int _listPos = 0;
    //---------------------------------------------------------
    //requests
    //---------------------------------------------------------

    //---------------------------------------------------------
    //states
    //---------------------------------------------------------
    IEnumerator Init()
    {
        _program = new ProgramFormat();
        _commandList = new List<ProgramFormat.OrderFormat>();
        InitCommandListData();
        InitProgramData();
        InitProgramView();
        InitCommandListView();
        Next(State.Wait);
        yield return null;
    }

    IEnumerator SelectCommandList()
    {
        switch (_touchState)
        {
            case TouchState.None:
                _touchState = TouchState.SelectViewCommand;
                break;
            case TouchState.SelectListCommand:
                break;
            case TouchState.SelectViewCommand:
                break;
        }
        _touchState = TouchState.None;
        Next(State.Wait);
        yield return null;
    }
    IEnumerator SelectProgramView()
    {
        switch (_touchState)
        {
            case TouchState.None:
                _touchState = TouchState.SelectViewCommand;
                break;
            case TouchState.SelectListCommand:
                yield return Switch(_viewX, _viewY, null);
                break;
            case TouchState.SelectViewCommand:
                
                break;
        }

        _touchState = TouchState.None;
        Next(State.Wait);
        yield return null;
    }

    IEnumerator Save()
    {
        yield return SaveProgram(Interpreter.Stringify(_program));
        Next(State.Wait);
        yield return null;
    }

    IEnumerator Exit()
    {
        yield return SaveProgram(Interpreter.Stringify(_program));
    }
    //---------------------------------------------------------
    //methods
    //---------------------------------------------------------
    void InitCommandListData()
    {
        string[] commands = SaveDataManager.Instance.Load(SaveDataManager.DataType.Command,0).Split(',');
        //Todo:master対応
        foreach (var command in commands)
        {
            MstFunctionRecord func = MasterdataManager.Get<MstFunctionRecord>(int.Parse(command));
            _commandList.Add(new ProgramFormat.OrderFormat() { key = ""});
        }
        
    }

    void InitProgramData()
    {
        string program = SaveDataManager.Instance.Load(SaveDataManager.DataType.Program, 0);
        _program = Interpreter.Parse(program);
    }
    void InitProgramView()
    {
        if (_program == null)
        {
            return;
        }

        if (_programView == null)
        {
            return;
        }
        for (int i = 0; i < _program.OrderList.GetLength(0); i++)
        {
            for (int j = 0; j < _program.OrderList.GetLength(1); j++)
            {
                if (_program.OrderList[i, j] == null)
                {
                    continue;
                }

                _programView.SetBlock(i,j,_program.OrderList[i,j]);
                _programView.SetupButton(OnTapViewCommand);
            }
        }
        
    }
    private void InitCommandListView()
    {
        if (_commandListViewer == null)
        {
            return;
        }
        for (int i = 0; i < _commandList.Count; i++)
        {
            _commandListViewer.SetBlock(i,_commandList[i]); 
        }
    }

    IEnumerator Switch(int x,int y,ProgramFormat.OrderFormat format)
    {
        if (format == null)
        {
            Debug.Log("null");
            yield break;
        }
        //Todo:OrderList[]indexCheck
        _program.OrderList[x, y].key = format.key;
        _program.OrderList[x, y].param = format.param;
        //矢印はそのまま
        _programView.SetBlock(x,y, _program.OrderList[x, y]);
        yield return null;
    }

    IEnumerator SaveProgram(string program)
    {
        yield return SaveDataManager.Instance.Save(SaveDataManager.DataType.Program, 0, program, true);
    }
    //---------------------------------------------------------
    //EDITING
    //---------------------------------------------------------
    #region unfinished

  
    private void InitProfile()
    {

    }
    void OnTapCommandList(int pos)
    {
        if (Current == State.Wait)
        {
            Next(State.SelectCommandList);
        }
    }

    void OnTapViewCommand(int x,int y)
    {
        if (Current == State.Wait)
        {
            _viewX = x;
            _viewY = y;
            Next(State.SelectProgramView);
        }
    }


    ProgramFormat LoadProgram(string path)
    {
        ProgramFormat ret = null;
        return null;
    }

    string[] LoadCommandList(string path)
    {
        if (PlayerPrefs.HasKey(path))
        {
            
        }

        return new []{""};
    }
    #endregion

}
