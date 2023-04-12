/******
작성자 : 이우열
작성 일자 : 23.04.12

최근 수정 일자 : 23.04.12
최근 수정 내용 : 버프 컨트롤러 구현
******/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class BuffController
    {
        /// <summary> 클라이언트 캐릭터가 받은 버프 목록 </summary>
        List<Buff> _buffs = new List<Buff>();

        /// <summary> 버프 남은 시간 계산 중 여부 </summary>
        bool _isCounting = false;

        /// <summary> 버프 효과 값 반환 </summary>
        public float GetBuffRate()
        {
            float rate = 1f;
            foreach (Buff buff in _buffs)
                rate += buff.BuffRatio;

            return rate;
        }

        /// <summary>
        /// 새로운 버프 추가
        /// </summary>
        /// <returns>버프 남은 시간 계산 코루틴</returns>
        public Func<IEnumerator> AddBuff(Buff buff)
        {
            _buffs.Add(buff);
            GameManager.InGameData.MyPlayer.StatUpdate();

            return BuffCount;
        }

        /// <summary> 버프 남은 시간 계산 </summary>
        IEnumerator BuffCount()
        {
            if (_isCounting) yield break;
            _isCounting = true;

            while (GameManager.InGameData.CurrState == Define.State.Play && _buffs.Count > 0)
            {
                for(int i = 0;i < _buffs.Count;i++)
                {
                    _buffs[i].RemainTime -= 0.25f;
                    if (_buffs[i].RemainTime <= 0)
                    {
                        _buffs.RemoveAt(i);
                        GameManager.InGameData.MyPlayer.StatUpdate();
                        i--;
                    }
                }

                yield return new WaitForSeconds(0.25f);
            }

            _isCounting = false;
        }
    }
}
