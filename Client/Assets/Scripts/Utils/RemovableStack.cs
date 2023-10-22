/******
작성자 : 이우열
작성 일자 : 23.10.22

최근 수정 일자 : 23.10.22
최근 수정 내용 : 중간 원소 삭제 가능한 Stack 추가
 ******/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Client
{
    public class RemovableStack<T>
    {
        private LinkedList<T> _stack = new LinkedList<T>();

        public int Count => _stack.Count;

        public void Push(T item) => _stack.AddFirst(item);
        public bool TryPop(out T item)
        {
            if(_stack.Count <= 0)
            {
                item = default(T);
                return false;
            }

            item = _stack.First.Value;
            _stack.RemoveFirst();
            return true;
        }
        public bool TryPeek(out T item)
        {
            if (_stack.Count <= 0)
            {
                item = default(T);
                return false;
            }

            item = _stack.First.Value;
            return true;
        }

        public T Pop()
        {
            if( _stack.Count <= 0)
                return default(T);

            var item = _stack.First.Value;
            _stack.RemoveFirst();
            return item;
        }
        public T Peek()
        {
            if (_stack.Count <= 0)
                return default(T);
            var item = _stack.First.Value;
            return item;
        }

        /// <summary> 특정 원소 제거 </summary>
        /// <param name="action"> 제거 후 다른 원소들에 수행할 작업 </param>
        /// <param name="doHigher"> 더 위에 있는 원소들에 작업할 지, 더 아래에 있는 원소들에 작업할 지 </param>
        public bool Remove(T item, Action<T> action = null, bool doHigher = true)
        {
            if(_stack.Count <= 0)
            {
                return false;
            }

            var iter = _stack.First;

            while (iter != null)
            {
                var current = iter.Value;
                if(current.Equals(item))
                {
                    if (null != action)
                    {
                        var actionIter = doHigher ? iter.Previous : iter.Next;

                        while(actionIter != null)
                        {
                            action.Invoke(actionIter.Value);
                            actionIter = doHigher ? actionIter.Previous : actionIter.Next;
                        }
                    }
                    break;
                }
            }

            if (iter != null)
            {
                _stack.Remove(iter);
                return true;
            }

            return false;
        }


        public bool Contains(T item) => _stack.Contains(item);
        public void Clear() => _stack.Clear();
    }
}