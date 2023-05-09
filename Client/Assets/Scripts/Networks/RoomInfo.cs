/******
작성자 : 이우열
작성 일자 : 23.05.09

최근 수정 일자 : 23.05.09
최근 수정 내용 : 방에 존재하는 인원 정보 저장 클래스 생성
 ******/

using System.Collections.Generic;

namespace Client
{
    /// <summary> 방에 존재하는 인원 정보 저장 클래스 </summary>
    public class RoomInfo
    {
        /// <summary> 이 방의 호스트 여부 </summary>
        public bool IsHost = false;
        /// <summary> 이 방에 존재하는 클라이언트 정보 </summary>
        public Dictionary<int, Define.Charcter> PlayerClasses = new Dictionary<int, Define.Charcter>();

        /// <summary> 플레이어 정보 추가 </summary>
        public void AddPlayers(List<int> players)
        {
            foreach (int i in players)
                AddPlayer(i);
        }

        /// <summary> 플레이어 정보 추가 </summary>
        public void AddPlayer(int playerId)
        {
            if (!PlayerClasses.ContainsKey(playerId))
                PlayerClasses.Add(playerId, Define.Charcter.MaxCount);
        }

        /// <summary> 플레이어 정보 제거 </summary>
        public void RemovePlayer(int playerId)
        {
            if(PlayerClasses.ContainsKey(playerId))
                PlayerClasses.Remove(playerId);
        }

        /// <summary> 플레이어 클래스 선택 정보 업데이트 </summary>
        public void SelectClass(int playerId, Define.Charcter playerClass)
        {
            if (PlayerClasses.ContainsKey(playerId))
                PlayerClasses[playerId] = playerClass;
        }

        /// <summary> 정보 초기화 </summary>
        public void Clear()
        {
            IsHost = false;
            PlayerClasses.Clear();
        }
    }
}
