/******
작성자 : 이우열
작성 일자 : 23.10.01

최근 수정 일자 : 23.10.01
최근 수정 내용 : 로그인 서버 클래스 생성
 ******/

using ServerCore;
using System.Net;
using System;

namespace LoginServer
{
    public class LoginMain
    {
        static Listener _serverListener = new Listener();
        static Listener _loginListener = new Listener();

        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 9999);

            //From Game Server
            _serverListener.Init(endPoint, () => { return Session.SessionManager.Instance.GenerateServerSession(); });

            //From Client
            endPoint.Port = 8888;
            _loginListener.Init(endPoint, () => { return Session.SessionManager.Instance.GenerateLoginSession(); });

            Console.WriteLine("I'm Login Server");

            while (true) 
            {
                JobTimer.Instance.Flush();
            }
        }
    }
}