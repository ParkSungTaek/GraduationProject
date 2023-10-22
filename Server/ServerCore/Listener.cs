/******
작성자 : 박성택
작성 일자 : 23.04.26

최근 수정 일자 : 23.10.02
최근 수정 내용 : 에러 처리 추가
 ******/


using System;
using System.Net;
using System.Net.Sockets;

namespace ServerCore
{
    public class Listener
    {
        Socket _listenSocket;
        Func<Session> _sessionFactory;

        /// <summary>
        /// Listening Socket 생성 & register 개의 delegate를 이용하여 Client의 연결을 대기 
        /// Async하게 구현
        /// </summary>
        /// <param name="endPoint"> Listening Socket이 연결할 서버의 EndPoint</param>
        /// <param name="sessionFactory"> 서버에서 이용 할 사용자 정의 Session Generatege 받기</param>
        /// <param name="register"> 몇 개의 Delegate가 대기 할 것인가? Default는 10개 </param>
        /// <param name="backlog"> 대기 가능 인원 Default는 100명 </param>
        public void Init(IPEndPoint endPoint, Func<Session> sessionFactory, int register = 10, int backlog = 100)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _sessionFactory += sessionFactory;
            _listenSocket.Bind(endPoint);

            _listenSocket.Listen(backlog);

            for (int i = 0; i < register; i++)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
                RegisterAccept(args);
            }
        }

        /// <summary>
        /// 연결 대기 중 
        /// </summary>
        /// <param name="args"> Data를 받고 전달할 SocketAsyncEventArgs </param>
        void RegisterAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;

            bool pending = _listenSocket.AcceptAsync(args);
            if (pending == false)
                OnAcceptCompleted(null, args);
        }
        /// <summary>
        /// Client의 접속 확인 후 연결작업 
        /// 연결 종료 후 다시 Delegate는 Client를 대기
        /// </summary>
        /// <param name="args"> Data를 받고 전달할 SocketAsyncEventArgs </param>
        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            try
            {
                if (args.SocketError == SocketError.Success && args.AcceptSocket.Connected)
                {
                    Session session = _sessionFactory.Invoke();
                    session.Start(args.AcceptSocket);
                    session.OnConnected(args.AcceptSocket.RemoteEndPoint);
                }
                else
                    ServerCore.Logger.Log(args.SocketError.ToString());
            }
            catch(Exception e)
            {
                ServerCore.Logger.Log(e.ToString());
            }

            RegisterAccept(args);
        }
    }
}
