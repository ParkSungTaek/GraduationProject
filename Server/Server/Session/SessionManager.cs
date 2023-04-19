/******
작성자 : 이우열
작성 일자 : 23.04.19

최근 수정 일자 : 23.04.19
최근 수정 내용 : 클래스 생성
 ******/

namespace Server
{
    class SessionManager
    { 
        /// <summary> 싱글톤 </summary>
        public static SessionManager Instance { get; } = new SessionManager();

        int _sessionId = 0;
        object _lock = new object();

        public ClientSession Generate()
        {
            lock (_lock)
            {
                int sessionId = ++_sessionId;

                ClientSession session = new ClientSession();
                session.SessionId = sessionId;

                return session;
            }
        }
    }
}