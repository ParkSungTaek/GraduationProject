/******
작성자 : 이우열
작성 일자 : 23.05.09

최근 수정 일자 : 23.05.09
최근 수정 내용 : 현재 참여 중인 방 관리를 위한 Room Manager 생성
 ******/

using System.Collections.Generic;
using System;

namespace Client
{
    public class RoomManager
    {
        /// <summary> 
        /// 작성자 : 이우열 <br/>
        /// 현재 참여 중인 방 정보 
        /// </summary>
        RoomInfo _roomInfo = new RoomInfo();

        public Action<RoomInfo> LobyUpdate { get; set; }

        /// <summary> 
        /// 작성자 : 이우열 <br/>
        /// 나를 호스트로 설정 
        /// </summary>
        public void SetHost()
        {
            _roomInfo.IsHost = true;
            LobyUpdate?.Invoke(_roomInfo);
        }

        /// <summary> 
        /// 작성자 : 이우열 <br/>
        /// 새로운 플레이어 입장 
        /// </summary>
        public void EnterPlayer(int playerId)
        {
            _roomInfo.AddPlayer(playerId);
            LobyUpdate?.Invoke(_roomInfo);
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
            LobyUpdate?.Invoke(_roomInfo);
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 방 입장 시 존재하는 플레이어 목록 받음
        /// </summary>
        public void SetExistPlayers(List<int> existPlayers)
        {
            _roomInfo.AddPlayers(existPlayers);
            _roomInfo.IsHost = existPlayers.Count <= 1;

            LobyUpdate?.Invoke(_roomInfo);
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

        public void Clear()
        {
            _roomInfo.Clear();
        }
    }
}