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
                UI_GetItem.ShopAction?.Invoke();
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

        #region Player
        /// <summary>
        /// 플레이어 쿨타임 컨트롤러
        /// </summary>
        public CooldownController Cooldown { get; } = new CooldownController();
        /// <summary>
        /// 모든 직업에 대한 스텟 정보
        /// </summary>
        public CharacterstatHandler CharacterStat { get; private set; }
        /// <summary>
        /// 현재 게임에 참여한 플레이어 캐릭터 오브젝트들
        /// </summary>
        List<PlayerController> _playerControllers = new List<PlayerController>();
        /// <summary>
        /// 클라이언트의 캐릭터
        /// <para></para>
        /// </summary>
        public PlayerController MyPlayer { 
            get
            {
                if (_playerControllers.Count > 0)
                    return _playerControllers[0];

                return null;
            }
        }
        /// <summary>
        /// 사제 버프 받을 가장 가까운 플레이어
        /// </summary>
        public PlayerController NearPlayer => null;
        #endregion Player

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


        #region Monster
        public MonsterstatHandler MonsterStates { get; private set; }

        MonsterSpawn _monsterSpawn;
        TowerController _tower;
        GameObject _monsterHpBar;



        public MonsterSpawn MonsterSpawn { get { return _monsterSpawn; } }
        public TowerController Tower { get { return _tower; } }
        public GameObject MonsterHpBar { get { return _monsterHpBar; } }
        #endregion

        #region state machine

        bool[] _state = new bool[(int)Define.State.MaxCount];



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

        /// <summary>
        /// 
        /// </summary>
        public void init()
        {
            _itemDB = new Item[(int)Define.Item.MaxCount];
            string[] _itemDBstr = Enum.GetNames(typeof(Define.Item));
            
            for (int i = 0; i < (int)Define.Item.MaxCount; i++)
            {
                _itemDB[i] = new Item();
                _itemDB[i].Name = _itemDBstr[i];
            }

            _myInventory = new List<Item>();

            CharacterStat = Util.ParseJson<CharacterstatHandler>();
            MonsterStates = Util.ParseJson<MonsterstatHandler>();

            _monsterHpBar = Resources.Load<GameObject>("Prefabs/UI/MonsterHP");
        }

        /// <summary>
        /// 새로운 게임 시작 - 몬스터 스폰 위치와 중앙 타워 생성
        /// </summary>
        public void GameStart()
        {
            GenerateMonsterSpawnPoint();
            GenerateTower();
            GeneratePlayer();
        }

        #region GameStart_Generate
        /// <summary> 몬스터 스폰 포인트 생성 </summary>
        void GenerateMonsterSpawnPoint()
        {
            GameObject monsterSpawn = GameObject.Find("MonsterSpawn");
            if (monsterSpawn == null)
                monsterSpawn = GameManager.Resource.Instantiate("Monster/MonsterSpawn/MonsterSpawn");
            _monsterSpawn = monsterSpawn.GetComponent<MonsterSpawn>();
        }
        /// <summary> 중앙 타워 생성 </summary>
        void GenerateTower()
        {
            GameObject tower = GameObject.Find("Tower");
            if (tower == null)
                tower = GameManager.Resource.Instantiate("Tower/Tower");
            _tower = tower.GetComponent<TowerController>();
        }
        /// <summary> 플레이어 생성 
        /// <para>현재는 하나만 생성하지만, 나중에 참여 인원수만큼 생성 </para>
        /// </summary>
        void GeneratePlayer()
        {
            int classIdx = PlayerPrefs.GetInt("Class", 0);
            GameObject playerGO;
            PlayerController playerController = null;
            switch (classIdx)
            {
                case 0:
                    playerGO = GameManager.Resource.Instantiate("Player/Warrior");
                    playerController = Util.GetOrAddComponent<Warrior>(playerGO);
                    break;
                case 1:
                    playerGO = GameManager.Resource.Instantiate("Player/Rifleman");
                    playerController = Util.GetOrAddComponent<Rifleman>(playerGO);
                    break;
                case 2:
                    playerGO = GameManager.Resource.Instantiate("Player/Wizard");
                    playerController = Util.GetOrAddComponent<Wizard>(playerGO);
                    break;
                default:
                    playerGO = GameManager.Resource.Instantiate("Player/Priest");
                    playerController = Util.GetOrAddComponent<Priest>(playerGO);
                    break;
            }

            playerGO.transform.position = Vector3.down;
            _playerControllers.Add(playerController);
        }
        #endregion GameStart_Generate

        public void Clear()
        {
            _money = _score = 0;
            Cooldown.Clear();
            _myInventory.Clear();
        }
    }

}
