/******
작성자 : 이우열
작성 일자 : 23.10.01

최근 수정 일자 : 23.10.01
최근 수정 내용 : db query 수행을 위한 Queue 클래스 생성
 ******/

using Npgsql;
using System;
using System.Collections.Generic;

namespace LoginServer.DB
{
    public class Query
    {
        public bool IsInsertOrDelete;
        public string QueryStr;
        public Action<bool> ResultCallback;
    }

    public class QueryQueue
    {
        const string connectionStr = "host=localhost;username=photon;password=photoncannon;database=photoncannon";

        Queue<Query> _queryQueue = new Queue<Query>();
        object _lock = new object();

        /// <summary> Flush 실행 중 여부 </summary>
        bool _flush = false;

        /// <summary> 새로운 작업 예약 </summary>
        public void Push(Query query)
        {
            bool flush = false;

            lock (_lock)
            {
                _queryQueue.Enqueue(query);

                //현재 아무도 작업을 비우고 있지 않음 -> 내가 비우러 출발
                if (_flush == false)
                    flush = _flush = true;
            }

            //비우러 출발
            if (flush)
                Flush();
        }

        /// <summary> 예약된 작업들 수행 </summary>
        void Flush()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionStr))
            {
                try
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    while (true)
                    {
                        Query query = Pop();

                        if (query == null)
                        {
                            _flush = false;
                            return;
                        }

                        using (NpgsqlCommand command = new NpgsqlCommand(query.QueryStr, connection))
                        {
                            try
                            {
                                if (query.IsInsertOrDelete)
                                {
                                    var result = command.ExecuteNonQuery();

                                    query.ResultCallback?.Invoke(result == 1);
                                }
                                else
                                {
                                    using (var reader = command.ExecuteReader())
                                    {
                                        query.ResultCallback?.Invoke(reader != null && reader.Read());
                                    }
                                }
                            }
                            catch (NpgsqlException ex)
                            {
                                if (query.IsInsertOrDelete)
                                {
                                    query.ResultCallback?.Invoke(false);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ServerCore.Logger.Log($"sql error : {e}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary> 가장 예약한지 오래된 작업 반환 </summary>
        Query Pop()
        {
            lock (_lock)
            {
                if (_queryQueue.Count == 0)
                    return null;
                return _queryQueue.Dequeue();
            }
        }
    }
}
