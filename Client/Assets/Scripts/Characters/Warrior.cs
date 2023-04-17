/*
작성자 : 이우열
작성일 : 23.03.29
최근 수정 일자 : 23.04.14
최근 수정 사항 : 아이템 스텟 확장
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

                //사거리 내에 몬스터가 존재할 때
                if (mon != null && Vector2.Distance(_currPosition, mon.CorrectPosition) <= _itemStat.AttackRange)
                {
                    SeeTarget(mon.CorrectPosition);
                    _char4D.AnimationManager.Attack();
                    GenerateRangedArea(_itemStat.AttackRange, mon.transform.position).SetDamage(Mathf.RoundToInt(_itemStat.AttackRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetAttackCool(_itemStat.AttackCool);
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
                PlayerStat stat = GameManager.InGameData.PlayerStats[MyClass];

                //사거리 내에 몬스터가 존재할 때
                if (mon != null && Vector2.Distance(_currPosition, mon.CorrectPosition) <= stat.SkillRange)
                {
                    SeeTarget(mon.CorrectPosition);
                    _char4D.AnimationManager.Attack();
                    GenerateRangedArea(_itemStat.SkillRange, mon.transform.position).SetDamage(Mathf.RoundToInt(_itemStat.SkillRatio * AttackDMG));
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
            AttackDMG = 20;
            Position = Vector2.zero;// 시작위치

            StatUpdate();
        }

    }
}