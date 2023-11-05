/******
작성자 : 공동 작성
작성 일자 : 23.05.03

최근 수정 일자 : 23.10.02
최근 수정 내용 : 연결 끊김 시 로그인 화면으로 전환 추가
 ******/

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

        /// <summary> 현재 접속 중인 계정 이메일 </summary>
        public string Email { get; set; } = string.Empty;

        object _lock = new object();
        Queue<Action> _jobQueue = new Queue<Action>();
        IPEndPoint _endPoint { get; set; }

        /// <summary> endPoint 설정 </summary>
        public void Init(ConnectionInfo con)
        {
            IPHostEntry host = Dns.GetHostEntry(con.Host);
            IPAddress addr = host.AddressList[0];

            _endPoint = new IPEndPoint(addr, con.GamePort);
        }

        /// <summary> connector를 사용하여 서버에 연결 </summary>
        public void Connect(CTS_Auth authPacket)
        {
            GameManager.UI.CloseAllPopUpUI();
            GameManager.UI.ShowPopUpUI<UI_Log>().SetLog("게임 서버와 연결 중");

            Connector connector = new Connector(authPacket);

            connector.Connect(_endPoint,
                () => { return Generate(); });
        }

        /// <summary> 서버와 연결 끊기 </summary>
        public void Disconnect()
        {
            Email = string.Empty;
            Debug.Log("Disconnect");
            _session?.Disconnect();
            _session = null;
        }

        /// <summary> 서버 연결 끊김 시 로그인 화면으로 전환 </summary>
        public void GoBackToLogin()
        {
            GameManager.Network.Push(() =>
            {
                UnityEngine.SceneManagement.SceneManager.sceneLoaded -= ShowDisconnectAlert;
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += ShowDisconnectAlert;
                SceneManager.LoadScene(Define.Scenes.Login);
            });

            void ShowDisconnectAlert(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
            {
                GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("서버와의 연결이 끊어졌습니다.");
                UnityEngine.SceneManagement.SceneManager.sceneLoaded -= ShowDisconnectAlert;
            }
        }

        /// <summary> 연결 성공 시 세션 생성 함수 </summary>
        ServerSession Generate()
        {
            Application.quitting -= Disconnect;
            Application.quitting += Disconnect;

            return _session = new ServerSession();
        }

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
