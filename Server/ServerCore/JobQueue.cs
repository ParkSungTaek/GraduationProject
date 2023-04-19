/******
작성자 : 공동 작성
작성일 : 23.04.19

최근 수정 일자 : 23.04.19
최근 수정 내용 : 클래스 추가
 ******/

using System;
using System.Collections.Generic;

namespace ServerCore
{
    /// <summary> 비동기 환경에서 작업 관리를 위한 lock queue </summary>
    public class JobQueue
    {
        Queue<Action> _jobQueue = new Queue<Action>();
        object _lock = new object();
        
        /// <summary> Flush 실행 중 여부 </summary>
        bool _flush = false;

        /// <summary> 새로운 작업 예약 </summary>
        public void Push(Action job)
        {
            bool flush = false;

            lock (_lock)
            {
                _jobQueue.Enqueue(job);
                
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
            while(true)
            {
                Action action = Pop();

                if (action == null)
                    return;

                action.Invoke();
            }
        }

        /// <summary> 가장 예약한지 오래된 작업 반환 </summary>
        Action Pop()
        {
            lock(_lock)
            {
                if (_jobQueue.Count == 0)
                    return null;
                return _jobQueue.Dequeue();
            }
        }
    }
}