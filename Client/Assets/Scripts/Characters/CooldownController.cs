using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class CooldownController
    {
        /// <summary>
        /// 플레이어의 쿨타임 정보
        /// </summary>
        float[] _cooldowns = new float[2];

        public void Clear()
        {
            for(int i = 0; i< _cooldowns.Length; i++) _cooldowns[i] = 0;
        }

        /// <summary>
        /// 공격 쿨다운 여부 반환
        /// </summary>
        public bool CanAttack() => _cooldowns[0] <= 0;
        /// <summary>
        /// 스킬 쿨다운 여부 반환
        /// </summary>
        public bool CanSkill() => _cooldowns[1] <= 0;


        public IEnumerator CooldownCoroutine()
        {
            while(GameManager.InGameData.Stat() == Define.State.Play)
            {
                for(int i = 0;i < 2;i++)
                    if (_cooldowns[i] > 0)
                    {
                        _cooldowns[i] -= 0.25f;
                        if (_cooldowns[i] <= 0)
                            _cooldowns[i] = 0;
                    }
                
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
