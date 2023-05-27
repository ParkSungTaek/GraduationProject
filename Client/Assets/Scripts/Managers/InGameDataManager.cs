/******
공동 작성
작성일 : 23.03.29

최근 수정 일자 : 23.05.27
최근 수정 사항 : 아이템 UI update 시 ItemData 자체를 전달하도록 변경
******/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class InGameDataManager
    {
        #region Money   
        //Money 관련 작성자 : 박성택

        /// <summary> 보유 중인 돈 </summary>
        int _money = 0;
        /// <summary> set 시, 아이템 구매 및 메인 UI 업데이트 </summary>
        public int Money
        {
            get { return _money; }
            set
            {
                _money = value;
                OnMoneyChanged?.Invoke();
            }
        }

        /// <summary>
        /// Wave 마다 올려줘야하나?
        /// </summary>
        int _moneyRewards = 5;
        public int MoneyRewards { get { return _moneyRewards; } }

        public Action OnMoneyChanged { get; set; } = null;
        #endregion Money

        #region Score
        //Score 관련 작성자 : 박성택

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
                OnScoreChanged?.Invoke();
            }
        }

        /// <summary> score 변경 ui에 적용 </summary>
        public Action OnScoreChanged { get; set; } = null;
        #endregion Score

        #region Item
        //Item 관련 작성자 : 이우열

        /// <summary> 모든 아이템 정보 </summary>
        ItemDataHandler _itemData;

        /// <summary> 아이템 최대 보유 가능 수 </summary>
        public readonly int MAXITEMCOUNT = 8;
        /// <summary> 아이템 구매에 필요한 비용 </summary>
        public int ItemCost { get; set; } = 10;
        /// <summary> 아이템 구매 가능 여부 반환 </summary>
        public bool CanBuyItem { get => _money >= ItemCost; }


        /// <summary> 플레이어들의 아이템 정보 </summary>
        Dictionary<int, List<ItemData>> _inventorys = new Dictionary<int, List<ItemData>>();
        /// <summary> 내 플레이어 아이템 정보 </summary>
        public List<ItemData> MyInventory { get => _inventorys[GameManager.Network.PlayerId]; }

        public Action<int, int, ItemData> ItemInfoUpdate { get; set; } = null;

        /// <summary> 작성자 : 이우열 <br/>
        /// 새로운 아이템 구매
        /// </summary>
        public void AddRandomItem(Action<int> uiImageUpdate) 
        {
            Money -= ItemCost;

            //빈 자리 있음 -> 새로운 아이템 만들어 채우기
            if (MyInventory.Count < MAXITEMCOUNT)
            {
                MyInventory.Add(_itemData.GetRandomItem());
                GameManager.InGameData.MyPlayer.StatUpdate();
                uiImageUpdate.Invoke(MyInventory.Count - 1);

                SendItemInfo(MyInventory.Count - 1, MyInventory[MyInventory.Count - 1].Idx);
                ItemInfoUpdate?.Invoke(GameManager.Network.PlayerId, MyInventory.Count - 1, MyInventory[MyInventory.Count - 1]);
            }
            //빈 자리 없음 -> 버리기 UI 띄우기
            else
            {
                UI_DropItem ui_DropItem = GameManager.UI.ShowPopUpUI<UI_DropItem>();
                ui_DropItem.ItemUpdate(_itemData.GetRandomItem(), uiImageUpdate);
            }
        }
        /// <summary> 작성자 : 이우열 <br/>
        /// 보유 중인 아이템 버리기 
        /// </summary>
        public void ReplaceItem(int position, ItemData newItem)
        {
            MyInventory[position] = newItem;
            MyPlayer.StatUpdate();

            SendItemInfo(position, newItem.Idx);
            ItemInfoUpdate?.Invoke(GameManager.Network.PlayerId, position, newItem);
        }

        /// <summary> 아이템 정보 동기화 전송 </summary>
        void SendItemInfo(int position, int itemIdx)
        {
            CTS_ItemUpdate itemPacket = new CTS_ItemUpdate();
            itemPacket.position = (ushort)position;
            itemPacket.itemIdx = (ushort)itemIdx;

            GameManager.Network.Send(itemPacket.Write());
        }

        /// <summary> 패킷으로 받은 아이템 정보 동기화 </summary>
        public void SyncItemInfo(int playerId, int position, int itemIdx)
        {
            ItemInfoUpdate?.Invoke(playerId, position, _itemData[itemIdx]);

            List<ItemData> inventory;

            if(_inventorys.TryGetValue(playerId, out inventory))
            {
                if(inventory.Count <= position)
                    inventory.Add(_itemData[itemIdx]);
                else
                    inventory[position] = _itemData[itemIdx];

                PlayerController player;

                if (_playerControllers.TryGetValue(playerId, out player))
                    player.StatUpdate(inventory);
            }
        }
        #endregion

        #region Player
        //플레이어 관련 작성자 : 이우열

        /// <summary> 플레이어 쿨타임 컨트롤러 </summary>
        public CooldownController Cooldown { get; } = new CooldownController();
        /// <summary> 모든 직업에 대한 스텟 정보 </summary>
        public PlayerStatHandler PlayerStats { get; private set; }
        /// <summary> 현재 게임에 참여한 플레이어 캐릭터 오브젝트들 (SessionId가 key)</summary>
        Dictionary<int, PlayerController> _playerControllers = new Dictionary<int, PlayerController>();
        /// <summary> 클라이언트의 캐릭터 </summary>
        public PlayerController MyPlayer
        {
            get
            {
                PlayerController myPlayer;
                if(_playerControllers.TryGetValue(GameManager.Network.PlayerId, out myPlayer))
                    return myPlayer;

                return null;
            }
        }
        /// <summary> 사제 버프 받을 가장 가까운 플레이어 </summary>
        int NearPlayerId
        {
            get
            {
                PlayerController near = null;
                int nearIdx = -1;

                foreach(var player in _playerControllers)
                {
                    if(player.Value.MyPlayer == false)
                    {
                        if (near == null)
                        {
                            nearIdx = player.Key;
                            near = player.Value;
                        }
                        else if(Vector3.Distance(MyPlayer.transform.position, near.transform.position) > Vector3.Distance(MyPlayer.transform.position, player.Value.transform.position))
                        {
                            nearIdx = player.Key;
                            near = player.Value;
                        }
                    }
                }

                return nearIdx;
            }
        }

        /// <summary> 클라이언트 캐릭터가 받은 버프 관리자 </summary>
        public BuffController Buff { get; } = new BuffController();
        #endregion Player

        #region Monster
        //작성자 : 박성택
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

        #region Tower
        //작성자 : 박성택
        /// <summary> 중앙 타워 </summary>
        TowerController _tower;
        /// <summary> 중앙 타워 </summary>
        public TowerController Tower { get { return _tower; } }
        #endregion

        #region State
        //State 관련 작성자 : 박성택

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
        public void GameStart(Dictionary<int, Define.Charcter> players)
        {
            OnMoneyChanged?.Invoke();
            OnScoreChanged?.Invoke();

            GenerateTower();
            GeneratePlayer(players);
        }
        #region GameStart_Generate
        /// <summary> 작성자 : 박성택 <br/>
        /// 몬스터 스폰 포인트 생성 
        /// </summary>
        public void GenerateMonsterSpawnPoint()
        {
            GameObject monsterSpawn = GameObject.Find("MonsterSpawn");
            if (monsterSpawn == null)
                monsterSpawn = GameManager.Resource.Instantiate("Monster/MonsterSpawn/MonsterSpawn");
            _monsterSpawn = monsterSpawn.GetComponent<MonsterSpawn>();
        }
        /// <summary> 작성자 : 박성택 <br/>
        /// 중앙 타워 생성 
        /// </summary>
        void GenerateTower()
        {
            GameObject tower = GameObject.Find("Tower");
            if (tower == null)
                tower = GameManager.Resource.Instantiate("Tower/Tower");
            _tower = tower.GetComponent<TowerController>();
        }
        /// <summary> 작성자 : 이우열 <br/>
        /// 플레이어 생성
        /// </summary>
        void GeneratePlayer(Dictionary<int, Define.Charcter> players)
        {
            _playerControllers.Clear();
            _inventorys.Clear();

            int xPos = -3;

            foreach (var player in players)
            {
                GameObject playerGO;
                PlayerController playerController = null;

                switch (player.Value)
                {
                    case Define.Charcter.Warrior:
                        playerGO = GameManager.Resource.Instantiate("Player/Warrior");
                        playerController = Util.GetOrAddComponent<Warrior>(playerGO);
                        break;
                    case Define.Charcter.Rifleman:
                        playerGO = GameManager.Resource.Instantiate("Player/Rifleman");
                        playerController = Util.GetOrAddComponent<Rifleman>(playerGO);
                        break;
                    case Define.Charcter.Wizard:
                        playerGO = GameManager.Resource.Instantiate("Player/Wizard");
                        playerController = Util.GetOrAddComponent<Wizard>(playerGO);
                        break;
                    default:
                        playerGO = GameManager.Resource.Instantiate("Player/Priest");
                        playerController = Util.GetOrAddComponent<Priest>(playerGO);
                        break;
                }

                playerGO.transform.position = 2 * Vector3.up + Vector3.right * xPos;
                xPos += 2;

                _playerControllers.Add(player.Key, playerController);
                _inventorys.Add(player.Key, new List<ItemData>());

                playerController.MyPlayer = player.Key == GameManager.Network.PlayerId;

                if(playerController.MyPlayer)
                {
                    GameObject camera = Camera.main.gameObject;
                    camera.transform.parent = playerController.transform;
                    camera.transform.localPosition = new Vector3(0, 1, -10);
                }
            }
        }
        #endregion GameStart_Generate

        /// <summary> 
        /// 작성자 : 이우열 <br/>
        /// 내가 아닌 플레이어의 이동 동기화 
        /// </summary>
        public void Move(int playerId, Vector2 targetPos)
        {
            PlayerController player;
            if(_playerControllers.TryGetValue(playerId, out player))
                player.SetTargetPos(targetPos);
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 내가 아닌 플레이어의 공격 애니메이션 동기화
        /// </summary>
        public void Attack(int playerId, int direction, bool isSkill)
        {
            PlayerController player;
            if (_playerControllers.TryGetValue(playerId, out player))
                player.SyncAnimationInfo(direction, isSkill);
        }

        /// <summary> 내 플레이어에게 버프 </summary>
        public void AddBuff(float buffRate)
        {
            Func<IEnumerator> buffCoroutine = Buff.AddBuff(new Buff(buffRate));

            MyPlayer.StatUpdate();
            MyPlayer.StartCoroutine(buffCoroutine.Invoke());
        }

        /// <summary> 가까운 플레이어에게 버프 전달 </summary>
        public void SendBuff(float buffRate)
        {
            CTS_PriestBuff buffPacket = new CTS_PriestBuff();

            int playerId = NearPlayerId;
            if (playerId < 0)
                return;

            buffPacket.buffRate = buffRate;
            buffPacket.playerId = playerId;

            GameManager.Network.Send(buffPacket.Write());
        }


        /// <summary> 게임 플레이 정보 초기화 </summary>
        public void Clear()
        {
            _money = _score = _wave = 0;
            _money = 1000;
            _score = 0;

            OnMoneyChanged = null;
            OnScoreChanged = null;
            ItemInfoUpdate = null;

            _playerControllers.Clear();
            Cooldown.Clear();
            _inventorys.Clear();
        }
    }

}
