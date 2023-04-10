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
    public class Rifleman : PlayerController
    {
        /// <summary> 라이플맨 기본 공격 : 원거리, 단일 </summary>
        public override void IsAttack()
        {
            //쿨타임 중이 아닐 때
            if (GameManager.InGameData.Cooldown.CanAttack())
            {
                MonsterController mon = NearMoster();
                Characterstat stat = GameManager.InGameData.CharacterStats[MyClass];

                //사거리 내에 몬스터가 존재할 때
                if (mon != null && Vector2.Distance(transform.position, mon.transform.position) <= stat.AttackRange)
                {
                    SeeTarget(mon.transform.position);
                    _char4D.AnimationManager.ShotBow();
                    mon.BeAttacked(Mathf.RoundToInt(_attackDMGRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetAttackCool(stat.AttackCool);
                }
                else
                    Debug.Log("no near mon");
            }
            else
                Debug.Log("attack cool");
        }
        /// <summary> 라이플맨 스킬 : 원거리, 단일 </summary>
        public override void IsSkill()
        {
            //쿨타임 중이 아닐 때
            if (GameManager.InGameData.Cooldown.CanSkill())
            {
                MonsterController mon = NearMoster();
                Characterstat stat = GameManager.InGameData.CharacterStats[MyClass];

                //사거리 내에 몬스터가 존재할 때
                if (mon != null && Vector2.Distance(transform.position, mon.transform.position) <= stat.SkillRange)
                {
                    SeeTarget(mon.transform.position);
                    _char4D.AnimationManager.ShotBow();
                    mon.BeAttacked(Mathf.RoundToInt(_skillDMGRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetSkillCool(stat.SkillCool);
                }
                else
                    Debug.Log("no near mon");
            }
            else
                Debug.Log("skill cool");
        }
        protected override void init()
        {
            base.init();
            MyClass = Define.Charcter.Rifleman;
            MoveSpeed = 5.0f;
            AttackDMG = 20;
            Position = Vector2.zero;// 시작위치

            _basicAttackRatio = 1.5f;
            _basicSkillRatio = 5;
            StatUpdate();
        }
    }
}
