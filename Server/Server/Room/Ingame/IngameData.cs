/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.04.19
최근 수정 내용 : 클래스 생성
 ******/

using System.Collections.Generic;

namespace Server
{
    public class IngameData
    {
        /// <summary> 중앙 타워 체력 정보 </summary>
        TowerInfo _towerInfo;
        /// <summary> 몬스터 정보 </summary>
        List<MonsterInfo> _mosters = new List<MonsterInfo>();

        /// <summary> 플레이어 정보 </summary>
        Dictionary<int, PlayerInfo> _players = new Dictionary<int, PlayerInfo>();

        public void Clear()
        {
            _players.Clear();
            _mosters.Clear();
        }
    }
}