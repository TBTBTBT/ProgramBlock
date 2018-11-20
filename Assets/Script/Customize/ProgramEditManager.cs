using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private GameManager _preview;
    //CommandListViewer
    //UnitProfileViewer

    private int _viewX = 0;//なくしたい
    private int _viewY = 0;
    private int _commandIndex = 0;
    //---------------------------------------------------------
    //requests
    //---------------------------------------------------------
    public void StartPreview(){
        GameManager.UnitData data = new GameManager.UnitData()
        {
            id = 0,
            program = "0,0:v0:0:1,0;1,0:v10:0:0,0"
        };
        _preview.Setup(new List<GameManager.UnitData>{data});
        _preview.Run();
    }
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
                _touchState = TouchState.SelectListCommand;
                break;
            case TouchState.SelectListCommand:

                break;
            case TouchState.SelectViewCommand:
                _touchState = TouchState.None;
                break;
        }
        Next(State.Wait);
        yield return null;
    }
    IEnumerator SelectProgramView()
    {
        Debug.Log(_touchState);
        switch (_touchState)
        {
            case TouchState.None:
                _touchState = TouchState.SelectViewCommand;
                break;
            case TouchState.SelectListCommand:
                _touchState = TouchState.None;
                yield return Switch(_viewX, _viewY, _commandList[_commandIndex]);

                break;
            case TouchState.SelectViewCommand:

                _touchState = TouchState.None;
                break;
        }

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
            _commandList.Add(new ProgramFormat.OrderFormat() { key = func.functionkey});
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
            }
        }
        _programView.SetupButton(OnTapViewCommand);
    }
    private void InitCommandListView()
    {
        if (_commandListViewer == null)
        {
            return;
        }
        _commandListViewer.SetupList(_commandList.Count, (num, go) =>
        {
           // Debug.Log(_commandList[num]);
            _commandListViewer.SetImage(go.GetComponent<Image>(), _commandList[num]);
            _commandListViewer.SetupButton(num,go.GetComponent<Button>(), OnTapCommandList);
        });
    }

    IEnumerator Switch(int x,int y,ProgramFormat.OrderFormat format)
    {
        if (format == null)
        {
            Debug.Log("null");
            yield break;
        }
        //Todo:OrderList[]indexCheck
        if (_program.OrderList[x, y] == null)
        {
            _program.OrderList[x, y] = new ProgramFormat.OrderFormat();
        }
        _program.OrderList[x, y].key = format.key;
        _program.OrderList[x, y].param = format.param;
        //矢印はそのまま
        _programView.SetBlock(x,y, _program.OrderList[x, y]);
        yield return null;
    }
    IEnumerator SetArrow(Vector2Int from, Vector2Int yes)
    {
        yield return SetArrow(from,yes, new Vector2Int(0,0));
    }
    IEnumerator SetArrow(Vector2Int from,Vector2Int yes, Vector2Int no)
    {
        _program.OrderList[from.x, from.y].yes = yes;
        _program.OrderList[from.x, from.y].no = no;
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
            _commandIndex = pos;
            Debug.Log(pos);
            Next(State.SelectCommandList);
        }
    }

    void OnTapViewCommand(int x,int y)
    {
        if (Current == State.Wait)
        {
            Debug.Log(x+","+y);
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
