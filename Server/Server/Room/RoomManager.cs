/******
작성자 : 이우열
작성 일자 : 23.04.19

최근 수정 일자 : 23.04.19
최근 수정 내용 : 클래스 인터페이스 설계
 ******/

using ServerCore;
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

        /// <summary> 모든 방들 예약된 브로드캐스트 수행 </summary>
        public void Flush()
        {
            foreach(Room room in _rooms.Values)
                room.Flush();
        }
    }
}