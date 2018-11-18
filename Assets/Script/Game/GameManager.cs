using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourWithStatemachine<GameManager.State>
{
   public class UnitData{
        public int id;
        public int teamId;
        public string program;
    }
    private List<UnitCore> _units = new List<UnitCore>();

    public List<UnitCore> Units => _units;
    public enum State
    {
        Init,
        Wait,
        Running,
        Stop,
        Step
    }

    public void Setup(List<UnitData> _program){
        Debug.Log("[Game]Setup");
        for (int i = 0; i < _program.Count && i < _units.Count; i ++){
            _units[i].Setup(_program[i].id,this, _program[i].teamId,_program[i].program);
        }
    }
    public void ChangeProgram(){

    }
    public void Run()
    {
        Debug.Log("[Game]Run");
        Next(State.Running);
    }
    IEnumerator Init()
    {
        _units.ForEach(_ => Destroy(_));
        _units.Clear();
        _units.Add(Instantiate(Resources.Load<GameObject>("Unit/Unit_00")).GetComponent<UnitCore>());

        yield return null;
    }

    IEnumerator Running()
    {
        foreach (var unit in _units)
        {
            //unit.Setup(0,this,1,"0,0:v1:0:1,0;1,0:v10:0:0,0");
            unit.StartProcess();
        }

        yield return null;
    }
    IEnumerator Stop(){
        yield return null;
    }
}
