/******
작성자 : 이우열
작성 일자 : 23.04.19

최근 수정 일자 : 23.09.28
최근 수정 내용 : 주기적 연결 검사를 위해 모든 세션 정보 저장
 ******/

using System.Collections.Generic;

namespace Server
{
    class ClientSessionManager
    { 
        /// <summary> 싱글톤 </summary>
        public static ClientSessionManager Instance { get; } = new ClientSessionManager();
        ClientSessionManager() { }

        object _lock = new object();
        LinkedList<ClientSession> _unloginedSessions = new LinkedList<ClientSession>();
        List<ClientSession> _closedSessions = new List<ClientSession>();

        Dictionary<string, ClientSession> _sessions = new Dictionary<string, ClientSession>();

        public ClientSession Generate()
        {
            lock (_lock)
            {
                ClientSession session = new ClientSession();
                _unloginedSessions.AddFirst(session);
                session.email = string.Empty;

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

            //send 실패 시 _closedSessions에 채워짐
            lock (_lock)
            {
                foreach (ClientSession session in _unloginedSessions)
                {
                    session.Send(segment);
                }

                foreach (ClientSession session in _sessions.Values)
                {
                    session.Send(segment);
                }

                foreach (ClientSession closedSession in _closedSessions)
                {
                    if (closedSession.email == string.Empty)
                    {
                        _unloginedSessions.Remove(closedSession);
                    }
                    else
                    {
                        _sessions.Remove(closedSession.email);
                    }
                }

                _closedSessions.Clear();
            }
        }

        /// <summary> 로그인 인증 완료 </summary>
        public void OnLogin(ClientSession session)
        {
            ClientSession duplicatedSession = null;
            lock(_lock)
            {
                if (_sessions.TryGetValue(session.email, out duplicatedSession))
                {
                    _sessions.Remove(session.email);
                }

                _unloginedSessions.Remove(session);
                _sessions.Add(session.email, session);
            }

            //중복 로그인 방지
            if(duplicatedSession != null)
            {
                STC_DuplicatedLogin duplicatedPacket = new STC_DuplicatedLogin();
                duplicatedSession.Send(duplicatedPacket.Write());
                duplicatedSession.Disconnect(false);
            }
        }
    }
}