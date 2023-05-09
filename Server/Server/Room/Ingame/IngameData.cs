/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.05.06
최근 수정 내용 : MonsterControlInfo추가
 ******/

using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public class IngameData
    {
        public enum state
        {
            Pause,
            Play,
            Win,
            Lose
        }
        /// <summary> 중앙 타워 체력 정보 </summary>
        TowerInfo _towerInfo;
        /// <summary> 몬스터 정보 </summary>
        List<MonsterInfo> _mosters = new List<MonsterInfo>();

        public MonsterControlInfo MonsterControlInfo;

        public state State { get; set; }

        /// <summary> 플레이어 정보 </summary>
        Dictionary<int, PlayerInfo> _players = new Dictionary<int, PlayerInfo>();

        public IngameData()
        {
            State = state.Pause;
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 로비 -> 캐릭터 선택 전환 시 호출, 플레이어 정보 초기화
        /// </summary>
        public void Init(List<ClientSession> clients)
        {
            _players.Clear();
            _mosters.Clear();
            State = state.Pause;

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