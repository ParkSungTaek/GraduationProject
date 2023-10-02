/******
작성자 : 
작성 일자 : 23.05.03

최근 수정 일자 : 23.05.03
최근 수정 사항 : 서버에 연결을 위한 클래스 생성
 ******/

using System;
using System.Net;
using System.Net.Sockets;

namespace ServerCore
{
    /// <summary> 서버에 최초 연결을 설정하는 클래스 </summary>
    public class Connector
    {
        /// <summary> 연결 성공 시 세션 생성 함수 </summary>
        Func<Session> _sessionFactory;
        Action _retryConnect;

        public Connector(Action retry = null)
        {
            _retryConnect = retry;
        }

        /// <summary> 서버에 연결 시도 </summary>
        public void Connect(IPEndPoint endPoint, Func<Session> sessionFactory, int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                // 휴대폰 설정
                Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _sessionFactory = sessionFactory;

                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += OnConnectCompleted;
                args.RemoteEndPoint = endPoint;
                args.UserToken = socket;

                RegisterConnect(args);
            }
        }

        void RegisterConnect(SocketAsyncEventArgs args)
        {
            Socket socket = args.UserToken as Socket;
            if (socket == null)
                return;

            bool pending = socket.ConnectAsync(args);
            if (pending == false)
                OnConnectCompleted(null, args);
        }

        void OnConnectCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                Session session = _sessionFactory.Invoke();
                session.Start(args.ConnectSocket);
                session.OnConnected(args.RemoteEndPoint);
            }
            else
            {
                Console.WriteLine($"Fail To Connect To Login Server : {args.SocketError}");
                _retryConnect?.Invoke();
            }
        }
    }
}
