/******
작성자 : 이우열
작성 일자 : 23.10.01

최근 수정 일자 : 23.10.01
최근 수정 내용 : db 관리자 클래스 생성
 ******/
using System;

namespace LoginServer.DB
{
    public class DBManager
    {
        public static DBManager Instance { get; } = new DBManager();

        private QueryQueue _queryQueue = new QueryQueue();

        public void CreateUser(string email, string password, Action<bool> resultCallback)
        {
            if (email.Length > 60)
                email = email.Substring(0, 60);
            if (password.Length > 32)
                password = password.Substring(0, 32);

            Query query = new Query { IsInsert = true, QueryStr = $"INSERT INTO users VALUES('{email}', '{password}')", ResultCallback = resultCallback };
            _queryQueue.Push(query);
        }

        public void LoginUser(string email, string password, Action<bool> resultCallback)
        {
            if (email.Length > 60)
                email = email.Substring(0, 60);
            if (password.Length > 32)
                password = password.Substring(0, 32);

            Query query = new Query { IsInsert = false, QueryStr = $"SELECT email from users WHERE email = '{email}' AND passwd = '{password}'", ResultCallback = resultCallback };
            _queryQueue.Push(query);
        }
    }
}
