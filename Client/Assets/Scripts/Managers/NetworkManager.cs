/******
작성자 : 공동 작성
작성 일자 : 23.05.03

최근 수정 일자 : 23.05.09
최근 수정 내용 : 방 정보 관리 기능 추가
 ******/

#define AWS

using System;
using System.Collections.Generic;
using System.Net;
using ServerCore;
using UnityEngine;

namespace Client
{

    public class NetworkManager
    {
        ServerSession _session;

        public int PlayerId { get => _session.SessionId; }

        object _lock = new object();
        Queue<Action> _jobQueue = new Queue<Action>();

        const int PORT = 7777;

        /// <summary>
        /// 서버 endPoint 반환 <br/>
        /// 현재는 로컬 서버 가정, 추후 AWS 사용 시 변경 필요
        /// </summary>
        IPEndPoint GetServerEndPoint()
        {
            IPHostEntry ipHost;

            if (Application.platform == RuntimePlatform.Android)
            {
                ipHost = Dns.GetHostEntry("www.teamphotongp.o-r.kr");
            }
            else
            {
#if AWS
                ipHost = Dns.GetHostEntry("www.teamphotongp.o-r.kr");
#else
                string host = Dns.GetHostName();
                ipHost = Dns.GetHostEntry(host);
#endif
            }

            IPAddress ipAddr = ipHost.AddressList[0];

            return new IPEndPoint(ipAddr, PORT);
        }

        /// <summary> connector를 사용하여 서버에 연결 </summary>
        public void Connect()
        {
            GameManager.UI.ShowPopUpUI<UI_Log>().SetLog("서버와 연결 중");

            IPEndPoint endPoint = GetServerEndPoint();

            Connector connector = new Connector();

            connector.Connect(endPoint,
                () => { return Generate(); });
        }

        /// <summary> 연결 성공 시 세션 생성 함수 </summary>
        ServerSession Generate() => (_session = new ServerSession());

        public void Send(ArraySegment<byte> segment) => _session?.Send(segment);
        public void Send(List<ArraySegment<byte>> segmentList) => _session?.Send(segmentList);
    
        /// <summary> 패킷을 받았을 때 처리할 작업 넣기 </summary>
        public void Push(Action job)
        {
            lock(_lock)
            {
                _jobQueue.Enqueue(job);
            }
        }
        /// <summary>
        /// 유니티는 메인이 아닌 쓰레드에서 게임 컨텐츠 접근 불가 <br/>
        /// -> queue에 넣어놓고 Update에서 호출
        /// </summary>
        public void Flush()
        {
            while(_jobQueue.Count > 0)
            {
                Action job;
                lock (_lock)
                {
                    job = _jobQueue.Dequeue();
                }
                job.Invoke();
            }
        }
    }
}
