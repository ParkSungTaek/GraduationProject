/******
작성자 : 이우열
작성일 : 23.04.13

최근 수정 일자 : 23.10.02
최근 수정 내용 : 에러 캐치 추가
 ******/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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

        /// <summary> 수신 버퍼 </summary>
        RecvBuffer _recvBuffer = new RecvBuffer(65535);
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

        /// <summary> 연결 끊기 </summary>
        public void Disconnect()
        {
            //이미 다른 쓰레드에서 연결 끊음
            if (Interlocked.Exchange(ref _disconnected, 1) == 1)
                return;

            OnDisconnected(_socket.RemoteEndPoint);
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            Clear();
        }

        #region Callbacks
        /// <summary> 연결 성공 콜백 </summary>
        public abstract void OnConnected(EndPoint endPoint);
        /// <summary> 수신 콜백 </summary>
        public abstract int OnRecv(ArraySegment<byte> buffer);
        /// <summary> 송신 콜백 </summary>
        public abstract void OnSend(int byteTransfered);
        /// <summary> 연결 종료 콜백 </summary>
        public abstract void OnDisconnected(EndPoint endPoint);
        #endregion Callbacks

        #region Send
        /// <summary> 데이터 전송 큐에 넣기(list type) </summary>
        /// <param name="sendBuffList"> 보낼 데이터의 리스트 </param>
        public void Send(List<ArraySegment<byte>> sendBuffList)
        {
            if (sendBuffList.Count <= 0)
                return;

            try
            {
                lock (_lock)
                {
                    foreach (var sendBuff in sendBuffList)
                        _sendQueue.Enqueue(sendBuff);

                    if (_pendingList.Count == 0)
                        RegisterSend();
                }
            }
            catch
            {
                Disconnect();
            }
        }
        /// <summary> 데이터 전송 큐에 넣기 </summary>
        /// <param name="data"> 보낼 데이터 </param>
        public void Send(ArraySegment<byte> data)
        {
            try
            {
                lock (_lock)
                {
                    _sendQueue.Enqueue(data);
                    if (_pendingList.Count == 0)
                        RegisterSend();
                }
            }
            catch
            {
                Disconnect();
            }
        }

        /// <summary> 데이터 전송(큐->리스트), 비동기 송신 </summary>
        void RegisterSend()
        {
            if (_disconnected == 1)
                return;

            while (_sendQueue.Count > 0)
            {
                ArraySegment<byte> buff = _sendQueue.Dequeue();
                _pendingList.Add(buff);
            }
            _sendArgs.BufferList = _pendingList;

            try
            {
                bool pending = _socket.SendAsync(_sendArgs);
                if (pending == false)
                    OnSendCompleted(null, _sendArgs);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to RegisterSend : {e}");
            }
        }

        /// <summary> 비동기 송신 완료 </summary>
        void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            lock (_lock)
            {
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    try
                    {
                        _sendArgs.BufferList = null;
                        _pendingList.Clear();

                        OnSend(_sendArgs.BytesTransferred);

                        //추가로 보낼 데이터 존재
                        if (_sendQueue.Count > 0)
                            RegisterSend();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Failed to OnSendCompleted : {e}");
                    }
                }
                else
                {
                    Disconnect();
                }
            }
        }

        /// <summary> 보낼 데이터 초기화(queue, list) </summary>
        void Clear()
        {
            lock (_lock)
            {
                _pendingList.Clear();
                _sendQueue.Clear();
            }
        }
        #endregion Send

        #region Receive
        /// <summary> 다음 비동기 수신 시작 </summary>
        void RegisterRecv()
        {
            if (_disconnected == 1)
                return;

            _recvBuffer.Clean();
            ArraySegment<byte> segment = _recvBuffer.WriteSegment;
            _recvArgs.SetBuffer(segment.Array, segment.Offset, segment.Count);

            try
            {
                bool pending = _socket.ReceiveAsync(_recvArgs);
                if (pending == false)
                    OnRecvCompleted(null, _recvArgs);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed To RegisterRecv : {e}");
            }

        }

        /// <summary> 비동기 수신 완료 </summary>
        void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                try
                {
                    if (_recvBuffer.OnWrite(args.BytesTransferred) == false)
                    {
                        Disconnect();
                        return;
                    }

                    int processLen = OnRecv(_recvBuffer.ReadSegment);
                    if (processLen < 0 || _recvBuffer.DataSize < processLen)
                    {
                        Disconnect();
                        return;
                    }

                    if (_recvBuffer.OnRead(processLen) == false)
                    {
                        Disconnect();
                        return;
                    }

                    RegisterRecv();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed To OnRecvCompleted : {e}");
                }
            }
        }
        #endregion Receive
    }
}