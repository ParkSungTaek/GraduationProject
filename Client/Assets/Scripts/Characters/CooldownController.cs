/******
작성자 : 이우열
작성일 : 23.04.05

최근 수정 일자 : 23.04.10
최근 수정 사항 : 주석 및 region 추가
******/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    /// <summary>
    /// 플레이어 쿨타임 관리 클래스, InGameDataManager에서 인스턴스 생성
    /// </summary>
    public class CooldownController
    {
        /// <summary> 플레이어의 현재 남은 쿨타임 정보 </summary>
        float[] _cooldowns = new float[2];
        /// <summary> 플레이어 기술의 쿨타임, 버튼 비율 계산용 </summary>
        float[] _maxCooldowns = new float[2];

        /// <summary> 쿨타임 정보 초기화 </summary>
        public void Clear()
        {
            for(int i = 0; i< _cooldowns.Length; i++) _cooldowns[i] = 0;
        }

        #region BasicAttack
        /// <summary> 공격 가능 여부(쿨타임 아님) 반환 </summary>
        public bool CanAttack() => _cooldowns[0] <= 0;
        /// <summary> 공격 후 쿨타임 설정 </summary>
        public void SetAttackCool(float cool) => _cooldowns[0] = _maxCooldowns[0] = cool;
        /// <summary> 남은 공격 쿨타임 비율 반환 </summary>
        public float GetAttackCoolRate()
        {
            if (_maxCooldowns[0] == 0) return 1;
            return (_maxCooldowns[0] - _cooldowns[0]) / _maxCooldowns[0];
        }
        #endregion BasicAttack

        #region Skill
        /// <summary> 스킬 가능 여부(쿨타임 아님) 반환 </summary>
        public bool CanSkill() => _cooldowns[1] <= 0;
        /// <summary> 스킬 후 쿨타임 설정 </summary>
        public void SetSkillCool(float cool) => _cooldowns[1] = _maxCooldowns[1] = cool;
        /// <summary> 남은 스킬 쿨타임 비율 반환 </summary>
        public float GetSkillCoolRate()
        {
            if (_maxCooldowns[1] == 0) return 1;
            return (_maxCooldowns[1] - _cooldowns[1]) / _maxCooldowns[1];
        }
        #endregion Skill

        public IEnumerator CooldownCoroutine()
        {
            while(GameManager.InGameData.CurrState == Define.State.Play)
            {
                for(int i = 0;i < 2;i++)
                    if (_cooldowns[i] > 0)
                    {
                        _cooldowns[i] -= 0.1f;
                        if (_cooldowns[i] <= 0)
                            _cooldowns[i] = 0;
                    }
                
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
