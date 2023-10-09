/******
작성자 : 이우열, 박성택(빠른 입장)
작성 일자 : 23.04.19

최근 수정 일자 : 23.09.26
최근 수정 내용 : <박성택> 빠른입장을 위한 함수 생성(RandomQuickEnterRoomName , AllowQuickEnter)
 ******/

using ServerCore;
using System;
using System.Collections.Generic;
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

        LinkedList<Room> _quickEnterRooms = new LinkedList<Room>();

        #region Singleton
        /// <summary> 싱글톤 </summary>
        public static RoomManager Instance { get; } = new RoomManager();
        RoomManager() { }
        #endregion Singleton
        
        public List<string> RoomNames() 
        {
            return new List<string>(_rooms.Keys);
        }

        /// <summary>
        /// 랜덤한 빠른입장 가능하면 true & 방 이름을 out 해주고 빠른 입장이 불가능하다면 false RoomName = null
        /// </summary>
        /// <param name="roomName">빠른 입장 가능한 방 이름 out </param>
        /// <returns>현재 빠른 입장이 가능한가?</returns>
        public string RandomQuickEnterRoomName()
        {
            Random rand = new Random();
            int randnum = rand.Next(0, _quickEnterRooms.Count);
            if(_quickEnterRooms.Count == 0)
            {
                return null;
            }

            LinkedListNode<Room> currentNode = _quickEnterRooms.First;

            for (int i = 1; i < randnum; i++)
            {
                currentNode = currentNode.Next;
            }

            Room item;
            int find = 0;

            ///아직 방이 남아있어
            if (_rooms.TryGetValue(currentNode.Value.RoomName, out item))
            {
                //빈 자리도 있고 & 빠른입장도 허용했어
                if (item.CanEnter() && item.AllowQuickEntry)
                {
                    return item.RoomName;
                }

            }
            else
            {
                _quickEnterRooms.Remove(currentNode.Value);                
            }

            // 렌덤으로 빠른방 찾기는 실패 맨 앞부터 사용 가능한 빠른방이 있나 확인
            
            for (LinkedListNode<Room> node = _quickEnterRooms.First; node != null; node = node.Next)
            {
                if (_rooms.TryGetValue(currentNode.Value.RoomName, out item))
                {
                    //빈 자리도 있고 & 빠른입장도 허용했어
                    if (item.CanEnter() && item.AllowQuickEntry)
                    {
                        return item.RoomName;
                    }
                }
                else
                {
                    _quickEnterRooms.Remove(currentNode.Value);
                }
            }

            return null;

        }

        /// <summary>
        /// 빠른 입장 허용 || 비허용
        /// </summary>
        /// <param name="RoomName"></param>
        public void AllowQuickEnter(Room room, bool allowQuickEnter)
        {
            if (allowQuickEnter)
            {
                room.AllowQuickEntry = true;
                if (!_quickEnterRooms.Contains(room))
                {
                    Console.WriteLine($"ADD {room.RoomName}");
                    _quickEnterRooms.AddLast(room);
                }

            }
            else
            {
                room.AllowQuickEntry = false;
                if (_quickEnterRooms.Contains(room))
                {
                    Console.WriteLine($"Delete {room.RoomName}");
                    _quickEnterRooms.Remove(room);
                }
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

            _quickEnterRooms.Remove(room);
        }
        #endregion jobs
    }
}