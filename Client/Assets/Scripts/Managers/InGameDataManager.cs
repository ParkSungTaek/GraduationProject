using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class InGameDataManager
{
    #region state machine

    bool[] _state;
    MonsterSpawn _monsterSpawn;
    public MonsterSpawn MonsterSpawn { get { return _monsterSpawn.GetComponent<MonsterSpawn>(); } }
    public void init()
    {
        _state = new bool[(int)State.MaxCount];
        if (_monsterSpawn == null)
        {
            GameObject monsterSpawn = GameObject.Find("MonsterSpawn");
            if (monsterSpawn == null)
            {
                monsterSpawn = new GameObject { name = "MonsterSpawn" };
                monsterSpawn.AddComponent<MonsterSpawn>();
            }
            _monsterSpawn = monsterSpawn.GetComponent<MonsterSpawn>();

        }
    }
    
    public void StateChange(State nowState)
    {
        for (State stat = 0; stat < State.MaxCount; stat++)
        {
            if (nowState == stat)
            {
                _state[(int)stat] = true;
            }
            else
            {
                _state[(int)stat] = false;
            }
        }
        
    }
    public State Stat()
    {
        for (State stat = 0; stat < State.MaxCount; stat++)
        {
            if (_state[(int)stat])
            {
                return stat;
            }
        }
        return State.End;
    }
    #endregion

    
}
