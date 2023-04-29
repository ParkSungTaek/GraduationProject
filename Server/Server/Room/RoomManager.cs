/******
작성자 : 이우열
작성 일자 : 23.04.19

최근 수정 일자 : 23.04.19
최근 수정 내용 : 클래스 인터페이스 설계
 ******/

using ServerCore;
using System;
using System.Collections.Generic;

namespace Server
{
    public class RoomManager
    {
        /// <summary> 현재 존재하는 방들 </summary>
        Dictionary<string, Room> _rooms = new Dictionary<string, Room>();
        /// <summary> 작업 관리 큐 </summary>
        JobQueue _jobQueue = new JobQueue();

        /// <summary> 싱글톤 </summary>
        public static RoomManager Instance { get; } = new RoomManager();

        public void Push(Action job) => _jobQueue.Push(job);

        /// <summary> 모든 방들 예약된 브로드캐스트 수행 </summary>
        public void Flush()
        {
            foreach(Room room in _rooms.Values)
                room.Flush();
        }

        #region jobs
        /// <summary> 새로운 방 생성 </summary>
        /// <param name="session"> 방 생성 시도 클라이언트 </param>
        /// <param name="roomName"> 방 이름 </param>
        public void Create(ClientSession session, string roomName)
        {
            //중복되는 방 이름
            if (_rooms.ContainsKey(roomName))
            {
                STC_RejectRoom rejectPacket = new STC_RejectRoom();
                session.Send(rejectPacket.Write());
                return;
            }

            Room room = new Room();
            room.RoomName = roomName;
            room.Enter(session);
            session.Room = room;

            _rooms.Add(roomName, room);

            //방 생성 성공 알림
            STC_PlayerEnter enterPacket = new STC_PlayerEnter();
            enterPacket.playerId = session.SessionId;
            session.Send(enterPacket.Write());
        }

        /// <summary> 방 입장 </summary>
        /// <param name="session"> 입장 시도 클라이언트 </param>
        /// <param name="roomName"> 방 이름 </param>
        public void EnterRoom(ClientSession session, string roomName)
        {
            Room room;

            //방 존재하지 않음
            if(_rooms.TryGetValue(roomName, out room) == false)
            {
                STC_RejectEnter_Exist existPacket = new STC_RejectEnter_Exist();
                session.Send(existPacket.Write());
                return;
            }

            room.Push(() => room.Enter(session));
        }

        /// <summary> 모든 인원이 나갔을 때, 방 제거 </summary>
        public void Remove(Room room)
        {
            _rooms.Remove(room.RoomName);
        }
        #endregion jobs
    }
}