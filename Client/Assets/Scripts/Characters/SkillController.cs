using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class SkillController
    {
        /// <summary>
        /// 플레이어의 쿨타임 정보
        /// </summary>
        float[] _cooldowns = new float[2];
        /// <summary>
        /// 스킬 사거리
        /// </summary>
        int[] _ranges = new int[2];
        
        /// <summary>
        /// 플레이어 직업에 따라 사거리 불러오기
        /// </summary>
        /// <param name="character"></param>
        public void LoadData(Define.Charcter character)
        {

        }

        /// <summary>
        /// 공격 쿨다운 여부 반환
        /// </summary>
        public bool CanAttack() => _cooldowns[0] <= 0;
        /// <summary>
        /// 스킬 쿨다운 여부 반환
        /// </summary>
        public bool CanSkill() => _cooldowns[1] <= 0;

        /// <summary>
        /// 기본 공격 사거리 반환
        /// </summary>
        public int GetAttackRange() => _ranges[0];
        /// <summary>
        /// 스킬 사거리 반환
        /// </summary>
        public int GetSkillRange() => _ranges[1];


        IEnumerator CooldownCoroutine()
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
