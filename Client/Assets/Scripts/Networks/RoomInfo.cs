/******
작성자 : 이우열
작성 일자 : 23.05.09

최근 수정 일자 : 23.10.02
최근 수정 내용 : id 정보 이동, 플레이어 이메일 정보 추가
 ******/

using System.Collections.Generic;

namespace Client
{
    public class PlayerClassInfo
    {
        public string email;
        public Define.Charcter playerClass;
    }

    /// <summary> 방에 존재하는 인원 정보 저장 클래스 </summary>
    public class RoomInfo
    {
        /// <summary> 이 방의 호스트 여부 </summary>
        public bool IsHost = false;
        public bool IsPublic { get; set; } = false;
        public int MyId { get; set; } = -1;
        /// <summary> 이 방에 존재하는 클라이언트 정보 </summary>
        public Dictionary<int, PlayerClassInfo> PlayerClasses = new Dictionary<int, PlayerClassInfo>();

        /// <summary> 플레이어 정보 추가 </summary>
        public void AddPlayers(List<STC_ExistPlayers.PlayerInfo> players)
        {
            foreach (STC_ExistPlayers.PlayerInfo i in players)
                AddPlayer(i);
        }

        /// <summary> 플레이어 정보 추가 </summary>
        public void AddPlayer(STC_ExistPlayers.PlayerInfo playerInfo)
        {
            if (GameManager.Network.Email == playerInfo.email)
                GameManager.Room.MyId = playerInfo.playerId;

            if (!PlayerClasses.ContainsKey(playerInfo.playerId))
                PlayerClasses.Add(playerInfo.playerId, new PlayerClassInfo { email = playerInfo.email, playerClass = Define.Charcter.MaxCount });
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
                PlayerClasses[playerId].playerClass = playerClass;
        }

        /// <summary> 정보 초기화 </summary>
        public void Clear()
        {
            IsHost = false;
            MyId = -1;
            PlayerClasses.Clear();
        }
    }
}
