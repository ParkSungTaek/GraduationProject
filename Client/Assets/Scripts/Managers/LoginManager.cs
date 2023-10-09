/******
작성자 : 이우열
작성 일자 : 23.10.01

최근 수정 일자 : 23.10.01
최근 수정 내용 : 로그인 매니저 클래스 추가
 ******/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using ServerCore;
using UnityEngine;

namespace Client
{

    public class LoginManager
    {
        object _lock = new object();
        Queue<Action> _jobQueue = new Queue<Action>();

        const int PORT = 8888;

        public Define.LoginState State = Define.LoginState.CleanLogin;
        public Coroutine PacketTimer = null;
        private LoginSession _session;

        public string resentEmail = string.Empty;

        IPEndPoint _endPoint { get; set; }

        public void Init(ConnectionInfo con)
        {
            IPHostEntry host = Dns.GetHostEntry(con.Host);
            IPAddress addr = host.AddressList[0];

            _endPoint = new IPEndPoint(addr, con.LoginPort);
        }

        /// <summary> connector를 사용하여 서버에 연결 </summary>
        public void Post(IPacket packet)
        {
            State = Define.LoginState.SendLogin;
            GameManager.UI.ShowPopUpUI<UI_Log>().SetLog("로그인 서버와 연결 중");

            Connector connector = new Connector(packet);

            connector.Connect(_endPoint, () => _session = new LoginSession());
            GameManager.Network.Push(() => { PacketTimer = GameManager.SetCoroutine(PacketTimerCoroutine()); });
        }

        private IEnumerator PacketTimerCoroutine()
        {
            yield return new WaitForSeconds(10);

            if (State == Define.LoginState.SendLogin)
            {
                State = Define.LoginState.CleanLogin;
                _session?.Disconnect();
                _session = null;
                PacketTimer = null;

                GameManager.UI.CloseAllPopUpUI();
                GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("Connection Time Expired");
            }
        }

        public void RemoveTimer()
        {
            if (PacketTimer != null)
            {
                GameManager.Network.Push(() => 
                {
                    GameManager.RemoveCoroutine(PacketTimer);
                    PacketTimer = null;
                });
            }

            State = Define.LoginState.CleanLogin;
        }
    }
}
