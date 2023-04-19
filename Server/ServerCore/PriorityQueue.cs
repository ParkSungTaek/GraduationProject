/******
작성자 : 공동 작성
작성일 : 23.04.19

최근 수정 일자 : 23.04.19
최근 수정 내용 : 클래스 제작
 ******/

using System;
using System.Collections.Generic;

namespace ServerCore
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"> A.CompareTo(B) -> 음수이면 B가 우선순위 높음 </typeparam>
    public class PriorityQueue<T> where T : IComparable<T>
    {
        List<T> _heap = new List<T>();
        public int Count => _heap.Count;

        /// <summary> 우선순위 큐에 새로운 원소 삽입 </summary>
        public void Push(T item)
        {
            _heap.Add(item);

            //마지막 원소의 index
            int now = _heap.Count - 1;

            while(now > 0)
            {
                //부모 원소의 index
                int next = (now - 1) / 2;

                //부모의 우선순위가 더 높으면 종료
                if (_heap[now].CompareTo(_heap[next]) < 0)
                    break;

                //위치 교환
                T tmp = _heap[now];
                _heap[now] = _heap[next];
                _heap[next] = tmp;

                //다음 위치로 이동
                now = next;
            }
        }

        /// <summary> 가장 우선순위 높은 원소 반환 </summary>
        public T Pop()
        {
            T result = _heap[0];

            //제일 뒤의 원소와 자리 교환 후 제거
            int lastIdx = _heap.Count - 1;
            _heap[0] = _heap[lastIdx];
            _heap.RemoveAt(lastIdx);
            lastIdx--;

            int now = 0;
            while(true)
            {
                int left = 2 * now + 1;
                int right = 2 * now + 2;

                int next = now;
                //왼쪽 원소가 더 우선순위 높음
                if (left <= lastIdx && _heap[next].CompareTo(_heap[left]) < 0)
                    next = left;

                //오른쪽 원소가 (왼쪽 포함) 더 우선순위 높음
                if(right <= lastIdx && _heap[next].CompareTo(_heap[right]) < 0)
                    next = right;

                //왼쪽/오른쪽 모두 현재 값보다 우선순위 낮음
                if (now == next)
                    break;

                T tmp = _heap[now];
                _heap[now] = _heap[next];
                _heap[next] = tmp;

                //다음 위치로 이동
                now = next;
            }

            return result;
        }

        /// <summary> 가장 우선순위 높은 원소 보기 </summary>
        public T Peek()
        {
            if (_heap.Count <= 0)
                return default(T);
            return _heap[0];
        }
    }
}