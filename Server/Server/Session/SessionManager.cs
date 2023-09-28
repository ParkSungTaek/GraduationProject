/******
작성자 : 이우열
작성 일자 : 23.04.19

최근 수정 일자 : 23.09.28
최근 수정 내용 : 주기적 연결 검사를 위해 모든 세션 정보 저장
 ******/

using System.Collections.Generic;

namespace Server
{
    class SessionManager
    { 
        /// <summary> 싱글톤 </summary>
        public static SessionManager Instance { get; } = new SessionManager();
        SessionManager() { }

        int _sessionId = 0;
        object _lock = new object();
        LinkedList<ClientSession> _sessions = new LinkedList<ClientSession>();
        List<ClientSession> _closedSessions = new List<ClientSession>();

        public ClientSession Generate()
        {
            lock (_lock)
            {
                int sessionId = ++_sessionId;

                ClientSession session = new ClientSession();
                session.SessionId = sessionId;
                _sessions.AddFirst(session);

                return session;
            }
        }

        /// <summary> 세션 연결 종료 </summary>
        public void OnDisconnect(ClientSession session)
        {
            lock (_lock)
            {
                _closedSessions.Add(session);
            }
        }

        /// <summary> 연결 검사 패킷 송신 </summary>
        public void CheckAlive()
        {
            STC_CheckAlive alivePacket = new STC_CheckAlive();
            var segment = alivePacket.Write();

            foreach(ClientSession session in _sessions)
            {
                session.Send(segment);
            }

            //send 실패 시 _closedSessions에 채워짐

            lock (_lock)
            {
                foreach (ClientSession closedSession in _closedSessions)
                {
                    _sessions.Remove(closedSession);
                }

                _closedSessions.Clear();
            }

            JobTimer.Instance.Push(CheckAlive, 10000);
        }
    }
}