/******
작성자 : 박성택
작성 일자 : 23.10.22

최근 수정 일자 : 23.10.22
최근 수정 내용 : 현재 존재하는 방 목록 관리자 클래스 생성
 ******/

using System;
using System.Collections.Generic;

namespace Client
{
    public class ExistRoomManager
    {
        public Action OnPublicRoomListUpdate { get; set; }
        private List<string> _publicRoomList;
        public List<string> RoomList => _publicRoomList;

        public void PublicRoomsUpdate(List<string> existRooms)
        {
            _publicRoomList?.Clear();
            _publicRoomList = existRooms;
            OnPublicRoomListUpdate?.Invoke();
        }

        public void Clear()
        {
            _publicRoomList?.Clear();
            _publicRoomList = null;
            OnPublicRoomListUpdate = null;
        }
    }
}