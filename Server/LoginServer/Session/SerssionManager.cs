using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Session
{
    class SessionManager
    {
        /// <summary> 싱글톤 </summary>
        public static SessionManager Instance { get; } = new SessionManager();
        SessionManager() { }

        private object _lock = new object();

        /// <summary> 게임 서버와의 연결 세션 </summary>
        public GameServerSession AliveServer = null;

        /// <summary> 게임 서버의 인증을 대기하는 로그인 세션들 </summary>
        Dictionary<string, LoginSession> _waitingSessions = new Dictionary<string, LoginSession>();

        public GameServerSession GenerateServerSession()
        {
            if (AliveServer != null)
            {
                AliveServer.Disconnect();
                AliveServer = null;
            }

            return AliveServer = new GameServerSession();
        }

        public LoginSession GenerateLoginSession()
        {
            return new LoginSession();
        }

        /// <summary> 게임 서버의 인증 대기 등록 </summary>
        public void PushWaitingSession(string email, LoginSession session)
        {
            lock (_lock)
            {
                _waitingSessions.Remove(email);
                _waitingSessions.Add(email, session);
            }
        }

        /// <summary> 게임 서버의 인증 완료 처리 </summary>
        public LoginSession PopWaitingSession(string email)
        {
            LoginSession session = null;

            lock (_lock)
            { 
                _waitingSessions.TryGetValue(email, out session);
                
                if (session != null)
                {
                    _waitingSessions.Remove(email);
                }
            }

            return session;
        }
    }
}

