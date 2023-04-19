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
    public class Room
    {
        public string RoomName;

        /// <summary> 현재 방에 존재하는 클라이언트들 </summary>
        List<ClientSession> _sessions = new List<ClientSession>();
        /// <summary> 작업 관리 queue </summary>
        JobQueue _jobQueue = new JobQueue();
        /// <summary> broadcasting 대기 중인 데이터들 </summary>
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
    
        /// <summary> 새로운 작업 수행 예약 </summary>
        public void Push(Action job) => _jobQueue.Push(job);

        /// <summary> 방 인원 수 </summary>
        public int Count => _sessions.Count;

        //job queue에서 수행하기 때문에 싱글 쓰레드 가정
        #region Jobs
        /// <summary> 대기 중인 broadcasting 모두 수행 </summary>
        public void Flush()
        {
            foreach (ClientSession session in _sessions)
                session.Send(_pendingList);

            _pendingList.Clear();
        }

        /// <summary> 브로드캐스팅 예약 </summary>
        public void Broadcast(ArraySegment<byte> segment)
        {
            _pendingList.Add(segment);
        }

        /// <summary> 새로운 클라이언트 입장 </summary>
        public void Enter(ClientSession session)
        {
            _sessions.Add(session);
            session.Room = this;

            //TODO : Enter Packet BroadCast
        }

        /// <summary> 클라이언트 퇴장 </summary>
        public void Leave(ClientSession session)
        {
            //모든 유저가 나간 방 제거
            if (_sessions.Count <= 1)
            {
                RoomManager.Instance.Push(() => RoomManager.Instance.Remove(this));
                return;
            }

            //방장이 나갈 경우, 방장 변경
            if(_sessions.IndexOf(session) <= 0)
            {
                STC_SetSuper superPacket = new STC_SetSuper();
                _sessions[1].Send(superPacket.Write());
            }

            _sessions.Remove(session);

            //TODO : Leave Packet BroadCast
        }

        /// <summary> 플레이어 이동 동기화 </summary>
        /// <param name="session"> 이동 패킷 보낸 세션 </param>
        public void Move(ClientSession session, CTS_PlayerMove movePacket)
        {
            STC_PlayerMove packet = new STC_PlayerMove();
            packet.playerId = session.SessionId;
            packet.posX = movePacket.posX;
            packet.posY = movePacket.posY;

            Broadcast(packet.Write());
        }
        #endregion Jobs
    }
}