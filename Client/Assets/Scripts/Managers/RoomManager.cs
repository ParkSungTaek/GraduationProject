/******
작성자 : 이우열
작성 일자 : 23.05.09

최근 수정 일자 : 23.10.02
최근 수정 내용 : email 정보 추가
 ******/

using System.Collections.Generic;
using System;
using System.Diagnostics;

#if UNITY_EDITOR
using UnityEngine;
#endif
namespace Client
{
    public class RoomManager
    {
        /// <summary> 
        /// 작성자 : 이우열 <br/>
        /// 현재 참여 중인 방 정보 
        /// </summary>
        RoomInfo _roomInfo = new RoomInfo();
        public Action OnSetPublic { get; set; } = null;

        public int MyId
        {
            get => _roomInfo.MyId;
            set { _roomInfo.MyId = value; }
        }
        private Action<RoomInfo> _lobbyUpdate = null;
        public Action<RoomInfo> LobbyUpdate
        {
            get => _lobbyUpdate;
            set
            {
                _lobbyUpdate = value;
                _lobbyUpdate?.Invoke(_roomInfo);
            }
        }

        /// <summary> 
        /// 작성자 : 이우열 <br/>
        /// 나를 호스트로 설정 
        /// </summary>
        public void SetHost()
        {
            _roomInfo.IsHost = true;
            LobbyUpdate?.Invoke(_roomInfo);
        }

        /// <summary> 공개방으로 설정 </summary>
        public void SetPublic(bool isPublic)
        {
            _roomInfo.IsPublic = isPublic;

            if (isPublic)
            {
                OnSetPublic?.Invoke();
            }
        }

        /// <summary> 공개방 인지 반환 </summary>
        public bool IsPublic() => _roomInfo.IsPublic;

        /// <summary> 
        /// 작성자 : 이우열 <br/>
        /// 새로운 플레이어 입장 
        /// </summary>
        public void EnterPlayer(STC_ExistPlayers.PlayerInfo playerInfo)
        {
            _roomInfo.AddPlayer(playerInfo);
            LobbyUpdate?.Invoke(_roomInfo);
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 내가 퇴장
        /// </summary>
        public void Leave()
        {
            _roomInfo.Clear();
            CTS_LeaveRoom leavePacket = new CTS_LeaveRoom();
            GameManager.Network.Send(leavePacket.Write());
        }
        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 다른 플레이어 퇴장
        /// </summary>
        public void LeavePlayer(int playerId)
        {
            _roomInfo.RemovePlayer(playerId);
            LobbyUpdate?.Invoke(_roomInfo);
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 방 입장 시 존재하는 플레이어 목록 받음
        /// </summary>
        public void SetExistPlayers(List<STC_ExistPlayers.PlayerInfo> existPlayers)
        {
            _roomInfo.AddPlayers(existPlayers);
            _roomInfo.IsHost = existPlayers.Count <= 1;

            LobbyUpdate?.Invoke(_roomInfo);
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 클래스 선택
        /// </summary>
        public void SelectClass(int playerId, Define.Charcter playerClass)
        {
            _roomInfo.SelectClass(playerId, playerClass);
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 게임 시작
        /// </summary>
        public void GameStart()
        {
            GameManager.GameStart(_roomInfo.PlayerClasses);
        }

        public Dictionary<int, PlayerClassInfo> GetPlayerInfo() => _roomInfo.PlayerClasses;

        public void Clear()
        {
            _roomInfo.Clear();
        }
    }
}