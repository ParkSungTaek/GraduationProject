/******
작성자 : 공동 작성
작성 일자 : 23.04.05

최근 수정 일자 : 23.04.26
최근 수정 내용 : Server Listener 추가
 ******/

using ServerCore;
using System;
using System.Net;

namespace Server
{

    internal class MainProgram
    {

        static Listener _listener = new Listener();

        static void FlushRoom()
        {
            RoomManager.Instance.Flush();
            JobTimer.Instance.Push(FlushRoom, 250);
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
            _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("Listening...");

            JobTimer.Instance.Push(FlushRoom, 0);

            //계속 예약된 작업 수행 시도
            while(true)
            {
                JobTimer.Instance.Flush();
            }
        }
    }
}