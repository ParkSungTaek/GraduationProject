/******
작성자 : 공동 작성
작성 일자 : 23.04.05

최근 수정 일자 : 23.09.28
최근 수정 내용 : 주기적으로 연결 검사
 ******/

using ServerCore;
using System;
using System.Net;

namespace Server
{

    internal class ServerMain
    {
        static Listener _listener = new Listener();

        static void FlushRoom()
        {
            RoomManager.Instance.Flush();
            JobTimer.Instance.Push(FlushRoom, 250);
        }
        static void CheckAlive()
        {
            ClientSessionManager.Instance.CheckAlive();
            LoginSessionManager.Instance.CheckAlive();

            JobTimer.Instance.Push(CheckAlive, 10000);
        }

        /// <summary>
        /// 박성택 : Server Listener 을 추가하고 Client 대기
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            //TODO : Listener listen
            _listener.Init(endPoint, () => { return ClientSessionManager.Instance.Generate(); });
            Console.WriteLine("I'm Game Server");

            LoginSessionManager.Instance.Connect();

            JobTimer.Instance.Push(FlushRoom, 0);
            JobTimer.Instance.Push(CheckAlive, 10000);

            //계속 예약된 작업 수행 시도
            while(true)
            {
                JobTimer.Instance.Flush();
            }
        }
    }
}