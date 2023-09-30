/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.09.29
최근 수정 내용 : 시작한 방 입장 실패
 ******/

using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public class IngameData
    {
        public enum state
        {
            Lobby,
            CharacterSelect,
            Play,
            Win,
            Lose,
            EndWave
        }
        /// <summary> 중앙 타워 체력 정보 </summary>
        TowerInfo _towerInfo;
        public TowerInfo TowerInfo { get {  return _towerInfo; } set { _towerInfo = value; } }
        /// <summary> 몬스터 정보 </summary>
        Dictionary<int, MonsterInfo> _monters = new Dictionary<int, MonsterInfo>();
        public Dictionary<int, MonsterInfo> MontersDic { get { return _monters; } }
        public MonsterControlInfo MonsterControlInfo;
        public static MonsterStatHandler monsterStatHandler { get; set; }
        
        public state State { get; set; }

        /// <summary> 플레이어 정보 </summary>
        Dictionary<int, PlayerInfo> _players = new Dictionary<int, PlayerInfo>();

        public IngameData()
        {
            State = state.Lobby;
            if (monsterStatHandler == null)
            {
                monsterStatHandler = Util.ParseJson<MonsterStatHandler>("Monsterstats");
            }

        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 로비 -> 캐릭터 선택 전환 시 호출, 플레이어 정보 초기화
        /// </summary>
        public void Init(List<ClientSession> clients)
        {
            _players.Clear();
            _monters.Clear();
            State = state.CharacterSelect;

            foreach (var client in clients)
                _players.Add(client.SessionId, new PlayerInfo());
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 플레이어의 클래스 선택, 모두 선택 여부 반환
        /// </summary>
        public bool SelectClass(int sessionId, Define.PlayerClass playerClass)
        {
            if (_players.ContainsKey(sessionId))
                _players[sessionId] = new PlayerInfo(playerClass);

            foreach (var pInfo in _players.Values)
                if (pInfo.PlayerClass == Define.PlayerClass.UnSelected)
                    return false;

            return true;
        }
    }
}