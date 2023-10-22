/******
작성자 : 이우열
작성 일자 : 23.10.22

최근 수정 일자 : 23.10.22
최근 수정 내용 : 로그 출력 클래스 생성
 ******/

using System;

namespace ServerCore
{
    public class Logger
    {
        public static string HeadString { get; set; }

        public static void Log(string message)
        {
            Console.WriteLine($"{HeadString} {message}");
        }
    }
}
