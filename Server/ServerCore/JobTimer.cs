/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.04.19
최근 수정 내용 : 클래스 인터페이스 설계
 ******/

using System;

namespace ServerCore
{
    /// <summary> 일정 시간 후 수행할 작업 </summary>
    struct JobTimerElem : IComparable<JobTimerElem>
    {
        /// <summary> 실행할 시간 </summary>
        public int execTick;
        /// <summary> 실행할 작업 </summary>
        public Action action;

        /// <summary> execTick 클 수록 우선순위 낮음(우선순위 낮으면 음수) </summary>
        public int CompareTo(JobTimerElem other) => other.execTick - execTick;
    }


    /// <summary> 일정 시간 후 수행 해야하는 작업 관리 </summary>
    public class JobTimer
    {
        /// <summary> 우선순위 큐 </summary>
        PriorityQueue<JobTimerElem> _pq = new PriorityQueue<JobTimerElem>();
        object _lock = new object();

        /// <summary> 싱글톤 </summary>
        public static JobTimer Instance { get; } = new JobTimer();
    
        /// <summary> 새로운 작업 예약 추가 </summary>
        /// <param name="action"> 예약할 작업 </param>
        /// <param name="tickAfter"> 실행까지 남은 시간(ms 단위) </param>
        public void Push(Action action, int tickAfter)
        {
            JobTimerElem job;
            job.execTick = System.Environment.TickCount + tickAfter;
            job.action = action;

            lock(_lock)
            {
                _pq.Push(job);
            }
        }

        /// <summary> 시간 지난 작업 수행 및 큐에서 제거 </summary>
        public void Flush()
        {
            while(true)
            {
                //현재 시간
                int now = System.Environment.TickCount;

                JobTimerElem job;

                lock(_lock)
                {
                    //예약된 작업 없음 -> 종료
                    if (_pq.Count == 0)
                        break;

                    job = _pq.Peek();
                    //가장 짧게 남은게 아직 안지남 -> 종료
                    if (job.execTick > now)
                        break;

                    _pq.Pop();
                }

                //실행
                job.action.Invoke();
            }
        }
    }

    struct RemovableJob : IComparable<RemovableJob>
    {
        public int jobId;
        public int execTick;
        public Action action;

        public int CompareTo(RemovableJob other) => other.execTick - other.execTick;
    }

    public class RemovableJobTimer
    {
        PriorityQueue<RemovableJob> _pq = new PriorityQueue<RemovableJob>();
        object _lock = new object();

        int _id = 0;
        public static RemovableJobTimer Instance { get; } = new RemovableJobTimer();

        /// <summary> 시간 지난 작업 수행 및 큐에서 제거 </summary>
        public int Push(Action action, int tickAfter)
        {
            RemovableJob job;
            job.execTick = System.Environment.TickCount + tickAfter;
            job.action = action;
            int ret;

            lock (_lock)
            {
                ret = job.jobId = _id++;
                _pq.Push(job);
            }

            return ret;
        }

        /// <summary> 시간 지난 작업 수행 및 큐에서 제거 </summary>
        public void Flush()
        {
            while (true)
            {
                //현재 시간
                int now = System.Environment.TickCount;

                RemovableJob job;

                lock (_lock)
                {
                    //예약된 작업 없음 -> 종료
                    if (_pq.Count == 0)
                        break;

                    job = _pq.Peek();
                    //가장 짧게 남은게 아직 안지남 -> 종료
                    if (job.execTick > now)
                        break;

                    _pq.Pop();
                }

                //실행
                job.action.Invoke();
            }
        }

        /// <summary> 특정 작업 제거 </summary>
        public void Remove(int jobId)
        {
            lock(_lock)
            {
                if (_pq.Count <= 0)
                {
                    return;
                }

                _pq.Remove((job) => job.jobId == jobId);
            }
        }
    }
}