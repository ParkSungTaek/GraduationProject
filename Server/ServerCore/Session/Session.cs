/******
작성자 : 이우열
작성일 : 23.04.13

최근 수정 일자 : 23.04.13
최근 수정 내용 : Session 클래스 설계
 ******/

using System.Net;
using System.Net.Sockets;

namespace ServerCore
{
    public abstract class Session
    {
        /// <summary> 통신을 위한 소켓 </summary>
        Socket _socket;
        /// <summary> 연결 종료 상태 </summary>
        int _disconnected = 0;

        /// <summary> lock variable </summary>
        object _lock = new object();

        /// <summary> 송신 절차 대기 중인 패킷 저장 queue </summary>
        Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>();
        /// <summary> 송신 절차에 들어간 데이터들 </summary>
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();
        /// <summary> 비동기 송신을 위한 매개변수 </summary>
        SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
        /// <summary> 비동기 수신을 위한 매개변수 </summary>
        SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();

        /// <summary> 클라이언트의 연결 요청 수락, 새로운 세션 생성 </summary>
        /// <param name="socket"> 연결된 클라이언트와의 소켓 </param>
        public void Start(Socket socket)
        {
            _socket = socket;

            //비동기 수신 완료 시 함수 연결
            _recvArgs.Completed += OnRecvCompleted;
            //비동기 송신 완료 시 함수 연결
            _sendArgs.Completed += OnSendCompleted;

            //비동기 수신 시작
            RegisterRecv();
        }

        #region Callbacks
        /// <summary> 연결 성공 콜백 </summary>
        public abstract void OnConnected(EndPoint endPoint);
        /// <summary> 수신 콜백 </summary>
        public abstract void OnRecv(ArraySegment<byte> buffer);
        /// <summary> 송신 콜백 </summary>
        public abstract void OnSend(int byteTransfered);
        /// <summary> 연결 종료 콜백 </summary>
        public abstract void OnDisconnected(EndPoint endPoint);
        #endregion Callbacks

        #region Send
        /// <summary> 데이터 전송 </summary>
        /// <param name="data"> 보낼 데이터 </param>
        public void Send(ArraySegment<byte> data)
        {

        }

        /// <summary> 비동기 송신 완료 </summary>
        void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {

        }

        /// <summary> 보낼 데이터 초기화(queue, list) </summary>
        void Clear()
        {
            
        }
        #endregion Send

        #region Receive
        /// <summary> 다음 비동기 수신 시작 </summary>
        void RegisterRecv()
        {

        }

        /// <summary> 비동기 수신 완료 </summary>
        void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
        {

        }
        #endregion Receive
    }
}