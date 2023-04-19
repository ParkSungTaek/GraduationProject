/******
작성자 : 공동 작성
작성 일자 : 23.04.05

최근 수정 일자 : 23.04.19
최근 수정 내용 : JobTimer 추가
 ******/

using System;

namespace Server
{
    internal class MainProgram
    {
        static void FlushRoom()
        {
            RoomManager.Instance.Flush();
            JobTimer.Instance.Push(FlushRoom, 250);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            //계속 예약된 작업 수행 시도
            while(true)
            {
                JobTimer.Instance.Flush();
            }
        }
    }
}