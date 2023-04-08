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
                Characterstat stat = GameManager.InGameData.CharacterStat[MyClass];

                //사거리 내에 몬스터가 존재할 때
                if (mon != null && Vector2.Distance(transform.position, mon.transform.position) <= stat.SkillRange)
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

            _attackDMGRatio = 1;
            _skillDMGRatio = 2;
        }
    }
}