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
    public class Rifleman : PlayerController
    {
        /// <summary> 라이플맨 기본 공격 : 원거리, 단일 </summary>
        public override void IsAttack()
        {
            //쿨타임 중이 아닐 때
            if (GameManager.InGameData.Cooldown.CanAttack())
            {
                MonsterController mon = NearMoster();

                //사거리 내에 몬스터가 존재할 때
                if (mon != null && Vector2.Distance(_currPosition, mon.transform.position) <= _itemStat.AttackRange)
                {
                    SeeTarget(mon.transform.position);
                    _char4D.AnimationManager.ShotBow();

                    mon.BeAttacked(Mathf.RoundToInt(_itemStat.AttackRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetAttackCool(_itemStat.AttackCool);
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

                //사거리 내에 몬스터가 존재할 때
                if (mon != null && Vector2.Distance(_currPosition, mon.transform.position) <= _itemStat.SkillRange)
                {
                    SeeTarget(mon.transform.position);
                    _char4D.AnimationManager.ShotBow();

                    mon.BeAttacked(Mathf.RoundToInt(_itemStat.SkillRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetSkillCool(_itemStat.SkillCool);
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
            AttackDMG = 20;
            Position = Vector2.zero;// 시작위치

            StatUpdate();
        }
    }
}
