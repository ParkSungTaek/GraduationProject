/*
작성자 : 이우열
작성일 : 23.03.29
최근 수정 일자 : 23.04.10
최근 수정 사항 : 아이템 스텟과 기본 스텟 분리
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    /// <summary> 위저드 기본 공격 : 원거리 단일 </summary>
    public class Wizard : PlayerController
    {
        /// <summary> 위저드 스킬 : 원거리 범위(타겟 중심) </summary>
        public override void IsSkill()
        {
            //쿨타임 중이 아닐 떄
            if (GameManager.InGameData.Cooldown.CanSkill())
            {
                MonsterController mon = NearMoster();
                CharacterStat stat = GameManager.InGameData.CharacterStats[MyClass];

                //사거리 내에 몬스터가 존재할 때
                if (mon != null && Vector2.Distance(_currPosition, mon.transform.position) <= stat.SkillRange)
                {
                    SeeTarget(mon.transform.position);
                    _char4D.AnimationManager.Jab();
                    GenerateTargetArea(1, mon.transform.position).SetDamage(Mathf.RoundToInt(_skillDMGRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetSkillCool(stat.SkillCool);
                }
            }
            else
                Debug.Log("skill cool");
        }

        protected override void init()
        {
            base.init();
            MyClass = Define.Charcter.Wizard;
            MoveSpeed = 5.0f;
            AttackDMG = 20;
            Position = Vector2.zero;// 시작위치

            _basicAttackRatio = 1;
            _basicSkillRatio = 2;
            StatUpdate();
        }
    }
}