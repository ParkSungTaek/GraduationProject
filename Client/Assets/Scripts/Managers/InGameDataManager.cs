using System;
using System.Collections.Generic;
using UnityEngine;
using static Client.Define;

namespace Client
{
    public class InGameDataManager
    {
        GameObject _gameOver;
        public GameObject GameOver { get { return _gameOver; } }


        #region Score & Money       
        int _money = 0;
        public int Money { get { return _money; } set { 
                _money = value; 
                UI_GameScene.TextsAction.Invoke();
                if (UI_GetItem.ShopAction != null)
                {
                    UI_GetItem.ShopAction.Invoke();
                }
            } 
        }

        /* public readonly*/
        int _itemCost = 10;
        /// <summary>
        /// 이게 가격 변동이 있을 일이 있나?
        /// </summary>
        public int ItemCost { get { return _itemCost; } set { _itemCost = value; } }

        int _moneyRewards = 5;
        int _scoreRewards = 1;
        /// <summary>
        /// Wave 마다 올려줘야하나?
        /// </summary>
        public int MoneyRewards { get { return _moneyRewards; } }
        public int ScoreRewards { get { return _scoreRewards; } }

        int _score = 0;
        public int Score { get { return _score; } 
            set { _score = value; 
                UI_GameScene.TextsAction.Invoke();
            } 
        }

        #endregion


        #region Item

        public readonly int MAXITEMNUM = 8;
        Item[] _itemDB;
        List<Item> _myInventory;
        public List<Item> MyInventory { get { return _myInventory; } }
        public Item[] ItemDB { get { return _itemDB;} }



        public void MyInventoryRandomADD() {
            if (_myInventory.Count < MAXITEMNUM)
            {
                _myInventory.Add(_itemDB[UnityEngine.Random.Range(0, _itemDB.Length)]);
            }
            else
            {

            }
        }

        public void MyInventoryDelete(Item item)
        {

        }
        #endregion

        MonsterSpawn _monsterSpawn;
        TowerController _tower;
        GameObject _monsterHpBar;



        public MonsterSpawn MonsterSpawn { get { return _monsterSpawn; } }
        public TowerController Tower { get { return _tower; } }
        public GameObject MonsterHpBar { get { return _monsterHpBar; } }


        #region state machine

        bool[] _state;



        /// <summary>
        /// 굳이 이런식으로 적어두는 이유는 Play누르기 전에 배치 해보고 Play해보고도 싶기 때문 
        /// 배치 하든 말든 하나만 잘 나오도록
        /// </summary>


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

        public void init()
        {
            if (_state == null)
            {
                _state = new bool[(int)State.MaxCount];
            }
            if (_monsterSpawn == null)
            {
                GameObject monsterSpawn = GameObject.Find("MonsterSpawn");
                if (monsterSpawn == null)
                {
                    monsterSpawn = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Monster/MonsterSpawn"));
                    monsterSpawn.name = "MonsterSpawn";
                }
                _monsterSpawn = monsterSpawn.GetComponent<MonsterSpawn>();
            }

            if (_tower == null)
            {
                GameObject tower = GameObject.Find("Tower");
                if (tower == null)
                {
                    tower = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/Tower/Tower"));
                    tower.name = "Tower";
                }
                _tower = tower.GetComponent<TowerController>();

            }

            _monsterHpBar = Resources.Load<GameObject>("Prefabs/UI/MonsterHP");
            _gameOver = Resources.Load<GameObject>("Prefabs/UI/GameOver");
            
            _itemDB = new Item[(int)Define.Item.MaxCount];

            
            string[] _itemDBstr = Enum.GetNames(typeof(Define.Item));
            
            for (int i = 0; i < (int)Define.Item.MaxCount; i++)
            {
                _itemDB[i] = new Item();
                _itemDB[i].Name = _itemDBstr[i];
                //Debug.Log(_itemDB[i].Name);
            }
            _myInventory = new List<Item>();
            

        }

    }

}
