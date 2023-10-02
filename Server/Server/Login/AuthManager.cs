/******
작성자 : 이우열
작성 일자 : 23.10.01

최근 수정 일자 : 23.10.01
최근 수정 내용 : 인증 관리자 클래스 생성
 ******/

using System.Collections.Generic;

namespace Server
{
    class AuthManager
    {
        public static AuthManager Instance { get; } = new AuthManager();
        object _lock = new object();

        List<string> _authClients = new List<string>();

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 로그인 서버 -> 게임 서버 인증 전송 시 저장
        /// </summary>
        public void OnAuth(string email)
        {
            lock(_lock)
            {
                _authClients.Add(email);
            }
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 클라 -> 게임 서버 인증
        /// </summary>
        public bool TryAuth(string email)
        {
            int idx;
            lock(_lock)
            {
                idx = _authClients.IndexOf(email);
                
                if(idx >= 0)
                {
                    _authClients.RemoveAt(idx);
                }
            }

            return idx >= 0;
        }
    }
}