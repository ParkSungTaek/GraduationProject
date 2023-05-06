/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.05.06
최근 수정 내용 : MonsterControlInfo추가
 ******/

using System.Collections.Generic;

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

        public void Clear()
        {
            _players.Clear();
            _mosters.Clear();
        }
    }
}