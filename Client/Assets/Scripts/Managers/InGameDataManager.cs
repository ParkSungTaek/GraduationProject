/******
공동 작성
작성일 : 23.03.29

최근 수정 일자 : 23.04.12
최근 수정 사항 : 버프 컨트롤러 추가
******/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class InGameDataManager
    {
        #region Money   
        /// <summary> 보유 중인 돈 </summary>
        int _money = 0;
        /// <summary> set 시, 아이템 구매 및 메인 UI 업데이트 </summary>
        public int Money
        {
            get { return _money; }
            set
            {
                _money = value;
                UI_GameScene.TextChangeAction?.Invoke();
                UI_GetItem.OnMoneyChangedAction?.Invoke();
            }
        }

        /// <summary>
        /// Wave 마다 올려줘야하나?
        /// </summary>
        int _moneyRewards = 5;
        public int MoneyRewards { get { return _moneyRewards; } }
        #endregion Money

        #region Score
        int _scoreRewards = 1;
        public int ScoreRewards { get { return _scoreRewards; } }

        int _wave = 0;
        public int Wave { get { return _wave; } set { _wave = value; } }

        int _score = 0;
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                UI_GameScene.TextChangeAction?.Invoke();
            }
        }
        #endregion Score

        #region Item
        /// <summary> 모든 아이템 정보 </summary>
        ItemDataHandler _itemData;

        /// <summary> 아이템 최대 보유 가능 수 </summary>
        public readonly int MAXITEMCOUNT = 8;
        /// <summary> 아이템 구매 가능 여부 반환 </summary>
        public bool CanBuyItem { get => _money >= _itemCost; }
        /// <summary>
        /// 이게 가격 변동이 있을 일이 있나?
        /// </summary>
        int _itemCost = 10;
        public int ItemCost { get { return _itemCost; } set { _itemCost = value; } }

        /// <summary> 현재 보유 중인 아이템 정보 </summary>
        List<ItemData> _myInventory = new List<ItemData>();
        /// <summary> 현재 보유 중인 아이템 정보 </summary>
        public List<ItemData> MyInventory { get { return _myInventory; } }

        /// <summary> 새로운 아이템 구매 </summary>
        public void AddRandomItem() 
        {
            if (_myInventory.Count < MAXITEMCOUNT)
                _myInventory.Add(_itemData.GetRandomItem());
            else
            {
                //TODO : 버릴 아이템 선택
            }

            GameManager.InGameData.MyPlayer.StatUpdate();
        }
        /// <summary> 보유 중인 아이템 버리기 </summary>
        public void MyInventoryDelete(int idx)
        {
            _myInventory.RemoveAt(idx);
            GameManager.InGameData.MyPlayer.StatUpdate();
        }
        #endregion

        #region Player
        /// <summary> 플레이어 쿨타임 컨트롤러 </summary>
        public CooldownController Cooldown { get; } = new CooldownController();
        /// <summary> 모든 직업에 대한 스텟 정보 </summary>
        public PlayerStatHandler PlayerStats { get; private set; }
        /// <summary> 현재 게임에 참여한 플레이어 캐릭터 오브젝트들 </summary>
        List<PlayerController> _playerControllers = new List<PlayerController>();
        /// <summary> 클라이언트의 캐릭터 </summary>
        public PlayerController MyPlayer
        {
            get
            {
                if (_playerControllers.Count > 0)
                    return _playerControllers[0];

                return null;
            }
        }
        /// <summary>
        /// 사제 버프 받을 가장 가까운 플레이어<br/>
        /// 서버 연동 시 변경 예정
        /// </summary>
        public PlayerController NearPlayer => null;

        /// <summary> 클라이언트 캐릭터가 받은 버프 관리자 </summary>
        public BuffController Buff { get; } = new BuffController();
        #endregion Player

        #region Monster 박성택 작업부
        /// <summary> 모든 몬스터 스텟 정보 </summary>
        public MonsterStatHandler MonsterStats { get; private set; }
        /// <summary> 몬스터 소환 관리 클래스 </summary>
        MonsterSpawn _monsterSpawn;
        /// <summary> 몬스터 소환 관리 클래스 </summary>
        public MonsterSpawn MonsterSpawn { get { return _monsterSpawn; } }
        /// <summary> 몬스터 체력바 프리팹 </summary>
        GameObject _hpBarPrefab;
        /// <summary> 몬스터 체력바 프리팹 </summary>
        public GameObject HPBarPrefab { get { return _hpBarPrefab; } }
        
        #endregion

        #region Tower 박성택 작업부
        /// <summary> 중앙 타워 </summary>
        TowerController _tower;
        /// <summary> 중앙 타워 </summary>
        public TowerController Tower { get { return _tower; } }
        #endregion

        #region State
        /// <summary> 상태 정보 저장 </summary>
        Define.State _state = Define.State.Idle;
        /// <summary> 게임 진행 상태 변경 </summary>
        public void StateChange(Define.State state) => _state = state;
        /// <summary> 현재 상태 반환 </summary>
        public Define.State CurrState { get => _state; }
        #endregion State

        /// <summary> 아이템 db 초기화, 스텟 정보, 프리팹 불러오기 </summary>
        public void Init()
        {
            _wave = 0;
            _itemData = Util.ParseJson<ItemDataHandler>();
            PlayerStats = Util.ParseJson<PlayerStatHandler>();
            MonsterStats = Util.ParseJson<MonsterStatHandler>();
            _hpBarPrefab = GameManager.Resource.Load<GameObject>("Prefabs/UI/MonsterHP");
        }

        /// <summary> 새로운 게임 시작 - 몬스터 스폰 위치와 중앙 타워 생성 </summary>
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
        /// <summary> 플레이어 생성 <br/>
        /// 현재는 하나만 생성하지만, 나중에 참여 인원수만큼 생성 </summary>
        void GeneratePlayer()
        {
            _playerControllers.Clear();

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

            playerGO.transform.position = 2 * Vector3.up;
            _playerControllers.Add(playerController);
        }
        #endregion GameStart_Generate

        /// <summary> 게임 플레이 정보 초기화 </summary>
        public void Clear()
        {
            _money = _score = _wave = 0;
            Cooldown.Clear();
            _myInventory.Clear();
        }
    }

}
