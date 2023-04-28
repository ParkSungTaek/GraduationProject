/******
작성자 : 이우열
작성일 : 23.03.29

최근 수정 일자 : 23.04.14
최근 수정 사항 : 아이템 스텟 확장
******/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    /// <summary> 사제 기본 공격 : 근접, 단일 </summary>
    public class Priest : PlayerController
    {
        /// <summary>
        /// 사제 스킬 : 자신, 가장 가까운 아군 강화(최종 데미지 50% 상승)
        /// </summary>
        public override void IsSkill()
        {
            //쿨타임 중이 아닐 때
            if (GameManager.InGameData.Cooldown.CanSkill())
            {
                //애니메이션 재생
                SeeDirection(Vector2.down);
                _char4D.AnimationManager.Jab();

                //나에게 버프 시전
                Func<IEnumerator> buffCounting = GameManager.InGameData.Buff.AddBuff(new Buff(_itemStat.SkillRatio));
                StartCoroutine(buffCounting.Invoke());

                //TODO : 가장 가까운 아군에게 버프 시전


                GameManager.InGameData.Cooldown.SetSkillCool(_itemStat.SkillCool);
            }
            else
                Debug.Log("skill cool");
        }

        protected override void Init()
        {
            base.Init();
            MyClass = Define.Charcter.Priest;
            MoveSpeed = 5.0f;
            AttackDMG = 20;

            StatUpdate();
        }

    }
}
