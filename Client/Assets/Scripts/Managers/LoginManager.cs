/******
작성자 : 이우열
작성 일자 : 23.10.01

최근 수정 일자 : 23.10.01
최근 수정 내용 : 로그인 매니저 클래스 추가
 ******/

//#define AWS_Login

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

        /// <summary>
        /// 서버 endPoint 반환 <br/>
        /// 현재는 로컬 서버 가정, 추후 AWS 사용 시 변경 필요
        /// </summary>
        IPEndPoint GetServerEndPoint()
        {
            IPHostEntry ipHost;

            if (Application.platform == RuntimePlatform.Android)
            {
                ipHost = Dns.GetHostEntry("3.35.30.92");
            }
            else
            {
#if AWS_Login
                ipHost = Dns.GetHostEntry("3.35.30.92");
#else
                string host = Dns.GetHostName();
                ipHost = Dns.GetHostEntry(host);
#endif
            }

            IPAddress ipAddr = ipHost.AddressList[0];

            return new IPEndPoint(ipAddr, PORT);
        }

        /// <summary> connector를 사용하여 서버에 연결 </summary>
        public void Post(IPacket packet)
        {
            State = Define.LoginState.SendLogin;
            GameManager.UI.CloseAllPopUpUI();
            GameManager.UI.ShowPopUpUI<UI_Log>().SetLog("로그인 서버와 연결 중");

            IPEndPoint endPoint = GetServerEndPoint();

            Connector connector = new Connector(packet);

            connector.Connect(endPoint, () => _session = new LoginSession());
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
