using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgramEditManager : MonoBehaviourWithStatemachine<ProgramEditManager.State>
{
    public enum State
    {
        Init,
        Wait,
        SelectCommandList,
        DragCommandList,
        ReleaseCommandList,
        SelectProgramView,
        DragProgramView,
        ReleaseProgramView,
        Preview,
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
    //UnitProfileViewer

    private int _viewX = 0;//なくしたい
    private int _viewY = 0;
    private int _commandIndex = 0;
    private Vector2 _dragPos;
    //---------------------------------------------------------
    //requests
    //---------------------------------------------------------
    public void StartPreview()
    {
        if (Current == State.Wait)
        {
            Next(State.Preview);
        }
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
    void OnDragCommandList(int pos, Vector2 touchPos)
    {
        _dragPos = touchPos;
        if (Current == State.SelectCommandList)
        {
            Next(State.DragCommandList);
        }
    }
    void OnReleaseCommandList(int pos)
    {
        if (Current == State.SelectCommandList ||
            Current == State.DragCommandList)
        {
            Next(State.ReleaseCommandList);
        }
    }
    void OnTapProgramView(int x, int y)
    {
        if (Current == State.Wait)
        {
            Debug.Log(x + "," + y);
            _viewX = x;
            _viewY = y;
            Next(State.SelectProgramView);
        }
    }
    void OnDragProgramView(int x, int y,Vector2 touchPos)
    {
        _dragPos = touchPos;
        if (Current == State.SelectProgramView)
        {
            Next(State.DragProgramView);
        }
    }
    void OnReleaseProgramView(int x, int y)
    {
        if (Current == State.SelectProgramView ||
            Current == State.DragProgramView)
        {
            Next(State.ReleaseProgramView);
        }
    }
    //---------------------------------------------------------
    //states
    //---------------------------------------------------------
    IEnumerator Init()
    {
        _program = new ProgramFormat();
        _commandList = new List<ProgramFormat.OrderFormat>();
        InitCommandListData();
        InitCommandListView();
        InitProgramData();
        InitProgramView();

        Next(State.Wait);
        yield return null;
    }

    IEnumerator SelectCommandList()
    {
        switch (_touchState)
        {
            case TouchState.None:
            case TouchState.SelectViewCommand:
                _touchState = TouchState.SelectListCommand;
                break;
            case TouchState.SelectListCommand:

                break;

        }
        // Next(State.Wait);
        yield return null;
    }
    IEnumerator DragCommandList()
    {
        yield return null;
    }
    IEnumerator ReleaseCommandList()
    {

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
    
        yield return null;
    }
    IEnumerator DragProgramView()
    {
        Vector2 dragStart = _dragPos;
        while(true){
            if((_dragPos - dragStart).magnitude > 50){
                Vector2 dist = _dragPos - dragStart;
                int nextX = 0;
                int nextY = 0;
                if (Mathf.Abs(dist.x) > Mathf.Abs(dist.y)){
                    nextX = Mathf.CeilToInt(Mathf.Sign(dist.x));
                }else{
                    nextY = Mathf.CeilToInt(Mathf.Sign(dist.y));
                }
                Debug.Log($"{_viewX + nextX} , {_viewY+ nextY}");
                yield return SetArrow(new Vector2Int(_viewX,_viewY),new Vector2Int(nextX, nextY));
            }

            yield return null;
        }

    }

    IEnumerator ReleaseProgramView()
    {

        Next(State.Wait);
        yield return null;
    }
    IEnumerator Preview()
    {
        GameManager.UnitData data = new GameManager.UnitData()
        {
            id = 0,
            program = Interpreter.Stringify(_program)
        };
        //Debug.Log(Interpreter.Stringify(_program));
        _preview.Setup(new List<GameManager.UnitData> { data });
        _preview.Run();
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
        string[] commands = SaveDataManager.Instance.Load(SaveDataManager.DataType.Command, 0).Split(',');
        //Todo:master対応
        foreach (var command in commands)
        {
            Debug.Log("command");
            MstFunctionRecord func = MasterdataManager.Get<MstFunctionRecord>(int.Parse(command));
            _commandList.Add(new ProgramFormat.OrderFormat() { key = func.functionkey });
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
        //表の画像とコールバック設定
        for (int i = 0; i < _program.OrderList.GetLength(0); i++)
        {
            for (int j = 0; j < _program.OrderList.GetLength(1); j++)
            {

                _programView.SetCommandImage(i, j, _program.OrderList[i, j]);
                int x = i;
                int y = j;
                _programView.SetCommandCallback(i, j, Command.CallbackType.PointerDown, pos =>
                {
                    OnTapProgramView(x, y);
                });

                _programView.SetCommandCallback(i, j, Command.CallbackType.Drag, pos =>
                {
                    OnDragProgramView(x, y,pos);
                });
                _programView.SetCommandCallback(i, j, Command.CallbackType.PointerUp, pos =>
                {
                    OnReleaseProgramView(x, y);
                });
            }
        }
        //_programView.SetCommandCallbackAll(OnTapProgramView);
    }
    private void InitCommandListView()
    {
        if (_commandListViewer == null)
        {
            return;
        }
        //表の画像とコールバック設定
        _commandListViewer.SetupList(_commandList.Count, (num, go) =>
        {
            Debug.Log(num);
            _commandListViewer.SetupCommandImage(num, go.GetComponent<Command>(), _commandList[num]);
            _commandListViewer.SetupCommandCallback(num, go.GetComponent<Command>(), Command.CallbackType.PointerDown, pos =>
             {
                 OnTapCommandList(num);
             });
            _commandListViewer.SetupCommandCallback(num, go.GetComponent<Command>(), Command.CallbackType.Drag, pos =>
            {
                OnDragCommandList(num,pos);
            });
            _commandListViewer.SetupCommandCallback(num, go.GetComponent<Command>(), Command.CallbackType.PointerUp, pos =>
            {
                OnReleaseCommandList(num);
            });
        });
    }

    IEnumerator Switch(int x, int y, ProgramFormat.OrderFormat format)
    {
        //データ
        if (format == null)
        {
            Debug.Log("null");
            yield break;
        }
        SwitchData(x, y, format);
        //見た目
        SwitchView(x, y, format);
       
       
       
        yield return null;
    }
    void SwitchData(int x, int y, ProgramFormat.OrderFormat format){
        //Todo:OrderList[]indexCheck
        if (_program.OrderList[x, y] == null)
        {
            _program.OrderList[x, y] = new ProgramFormat.OrderFormat();
        }
        Debug.Log(format.key);
        _program.OrderList[x, y].key = format.key;
        _program.OrderList[x, y].param = format.param;
        //矢印はそのまま
    }
    void SwitchView(int x, int y, ProgramFormat.OrderFormat format)
    {
        _programView.SetCommandImage(x, y, _program.OrderList[x, y]);
    }
    IEnumerator SetArrow(Vector2Int from, Vector2Int yes)
    {
        yield return SetArrow(from,yes, new Vector2Int(0,0));
    }
    IEnumerator SetArrow(Vector2Int from,Vector2Int yes, Vector2Int no)
    {
        SetArrowData(from, yes, no);
        SetArrowView(from, yes, no);
        yield return null;
    }
    void SetArrowData(Vector2Int from, Vector2Int yes, Vector2Int no)
    {
        if(_program.OrderList[from.x, from.y] == null){
            return;
        }
        _program.OrderList[from.x, from.y].yes = yes;
        _program.OrderList[from.x, from.y].no = no;
    }
    void SetArrowView(Vector2Int from,Vector2Int yes, Vector2Int no){
        _programView.SetArrow(from.x, from.y,true,yes - from);
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
