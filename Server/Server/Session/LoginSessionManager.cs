/******
작성자 : 이우열
작성 일자 : 23.10.01

최근 수정 일자 : 23.10.01
최근 수정 내용 : 로그인 서버와의 연결 제어 클래스 생성
 ******/

using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;

namespace Server
{
    class LoginSessionManager
    {
        /// <summary> 싱글톤 </summary>
        public static LoginSessionManager Instance { get; } = new LoginSessionManager();
        LoginSessionManager() 
        {
            //연결 실패 시 재연결 함수 연결
            _connector = new Connector(RetryConnect);
        }

        private object _lock = new object();

        /// <summary> 로그인 서버로의 연결자 </summary>
        private Connector _connector = null;
        /// <summary> 현재 연결되어 있는 로그인 서버 세션 </summary>
        private LoginServerSession _aliveSession = null;

        /// <summary> 로그인 서버로 연결 </summary>
        public void Connect()
        {
            ServerCore.Logger.Log("Login Server Connecting..");

            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 9999);

            _connector.Connect(endPoint, () => 
            {
                lock (_lock)
                {
                    return _aliveSession = new LoginServerSession();
                }
            });
        }

        /// <summary> 연결 실패 시 5초 후 재시도 </summary>
        public void RetryConnect()
        {
            lock(_lock )
            {
                _aliveSession = null;
            }

            JobTimer.Instance.Push(Connect, 5000);
        }

        public void Disconnected()
        {
            lock(_lock )
            {
                _aliveSession = null;
            }

            Connect();
        }

        /// <summary> 연결 검사 패킷 송신 </summary>
        public void CheckAlive()
        {
            lock (_lock)
            {
                if (_aliveSession != null)
                {
                    STC_CheckAlive alivePacket = new STC_CheckAlive();
                    _aliveSession.Send(alivePacket.Write());
                }
            }
        }
    }
}