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
    public class Warrior : PlayerController
    {
        /// <summary> 워리어 기본 공격 : 근접, 범위 </summary>
        public override void IsAttack()
        {
            //공격이 쿨타임 중이 아닐 때
            if (GameManager.InGameData.Cooldown.CanAttack())
            {
                MonsterController mon = NearMoster();
                CharacterStat stat = GameManager.InGameData.CharacterStats[MyClass];

                //사거리 내에 몬스터가 존재할 때
                if (mon != null && Vector2.Distance(transform.position, mon.transform.position) <= stat.AttackRange)
                {
                    SeeTarget(mon.transform.position);
                    _char4D.AnimationManager.Attack();
                    GenerateRangedArea(stat.AttackRange, mon.transform.position).SetDamage(Mathf.RoundToInt(_attackDMGRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetAttackCool(stat.AttackCool);
                }
            }
            else
                Debug.Log("attack cool");
        }
        /// <summary> 워리어 스킬 : 근접, 범위 </summary>
        public override void IsSkill()
        {
            //공격이 쿨타임 중이 아닐 때
            if (GameManager.InGameData.Cooldown.CanSkill())
            {
                MonsterController mon = NearMoster();
                CharacterStat stat = GameManager.InGameData.CharacterStats[MyClass];

                //사거리 내에 몬스터가 존재할 때
                if (mon != null && Vector2.Distance(transform.position, mon.transform.position) <= stat.SkillRange)
                {
                    SeeTarget(mon.transform.position);
                    _char4D.AnimationManager.Attack();
                    GenerateRangedArea(stat.SkillRange, mon.transform.position).SetDamage(Mathf.RoundToInt(_skillDMGRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetSkillCool(stat.SkillCool);
                }
            }
            else
                Debug.Log("skill cool");
        }

        protected override void init()
        {
            base.init();
            MyClass = Define.Charcter.Warrior;
            MoveSpeed = 5.0f;
            AttackDMG = 20;
            Position = Vector2.zero;// 시작위치

            _basicAttackRatio = 1;
            _basicSkillRatio = 3;
            StatUpdate();
        }

    }
}