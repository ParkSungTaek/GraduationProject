/******
작성자 : 이우열, 박성택(빠른 입장)
작성 일자 : 23.04.19

최근 수정 일자 : 23.09.26
최근 수정 내용 : <박성택> 빠른입장을 위한 함수 생성(RandomQuickEnterRoomName , AllowQuickEnter)
 ******/

using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class RoomManager
    {
        public const int MAX_PLAYER_COUNT = 4;
        /// <summary> 현재 존재하는 방들 </summary>
        Dictionary<string, Room> _rooms = new Dictionary<string, Room>();
        /// <summary> 작업 관리 큐 </summary>
        JobQueue _jobQueue = new JobQueue();

        LinkedList<Room> _publicRooms = new LinkedList<Room>();

        #region Singleton
        /// <summary> 싱글톤 </summary>
        public static RoomManager Instance { get; } = new RoomManager();
        RoomManager() { }
        #endregion Singleton
        
        /// <summary> 공개 방 목록 반환 </summary>
        public List<string> GetPublicRooms() 
        {
            var list = new List<string>(_publicRooms.Count);
            foreach (var room in _publicRooms)
            {
                if (room.Count < 4 && room.IsLobby())
                {
                    list.Add(room.RoomName);
                }
            }

            return list;
        }

        /// <summary> 빠른 입장
        public void TryQuickEnter(ClientSession session)
        {
            if (_publicRooms.Count <= 0)
            {
                STC_QuickEnterFail failPacket = new STC_QuickEnterFail();
                session.Send(failPacket.Write());
                return;
            }

            var canEnterRooms = from room in _publicRooms
                                where room.CanEnter() && room.IsPublicRoom
                                select room;

            if (canEnterRooms.Count() <= 0)
            {
                STC_QuickEnterFail failPacket = new STC_QuickEnterFail();
                session.Send(failPacket.Write());
                return;
            }

            int rand = new Random().Next(0, canEnterRooms.Count() - 1);
            var selectedRoom = canEnterRooms.Skip(rand).First();

            selectedRoom.Push(() => selectedRoom.Enter(session));
        }

        /// <summary> 공개방 설정 </summary>
        public void SetPublicRoom(Room room, bool isPublic)
        {
            room.Push(() => room.SetPublicRoom(isPublic));

            if (isPublic)
            {
                if (!_publicRooms.Contains(room))
                {
                    ServerCore.Logger.Log($"public room ADD : {room.RoomName}");
                    _publicRooms.AddLast(room);
                }
            }
            else
            {
                ServerCore.Logger.Log($"public room Delete : {room.RoomName}");
                _publicRooms.Remove(room);
            }
        }

        public void Push(Action job) => _jobQueue.Push(job);

        /// <summary> 모든 방들 예약된 브로드캐스트 수행 </summary>
        public void Flush()
        {
            foreach (Room room in _rooms.Values)
                Task.Factory.StartNew(() => room.Push(room.Flush));
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
        }

        /// <summary> 방 입장 </summary>
        /// <param name="session"> 입장 시도 클라이언트 </param>
        /// <param name="roomName"> 방 이름 </param>
        public void EnterRoom(ClientSession session, string roomName)
        {
            Room room;

            //방 존재하지 않음
            if(_rooms.TryGetValue(roomName, out room) == false || room.RoomName == string.Empty)
            {
                STC_RejectEnter existPacket = new STC_RejectEnter();
                existPacket.errorCode = STC_RejectEnter.ErrorCode.NotExist;
                session.Send(existPacket.Write());
                return;
            }

            room.Push(() => room.Enter(session));
        }

        /// <summary> 모든 인원이 나갔을 때, 방 제거 </summary>
        public void Remove(Room room)
        {
            _rooms.Remove(room.RoomName);
            _publicRooms.Remove(room);
        }
        #endregion jobs
    }
}