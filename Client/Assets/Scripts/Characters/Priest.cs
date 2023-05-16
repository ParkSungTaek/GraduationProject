/******
작성자 : 이우열
작성일 : 23.03.29

최근 수정 일자 : 23.05.16
최근 수정 사항 : 애니메이션 동기화
******/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Client
{
    public class Priest : PlayerController
    {
        /// <summary> 사제 기본 공격 : 근접, 단일 </summary>
        public override void IsAttack()
        {
            //쿨타임 중이 아닐 때
            if (GameManager.InGameData.Cooldown.CanAttack())
            {
                MonsterController mon = NearMoster();

                //사거리 내에 몬스터가 존재할 때
                if (mon != null && Vector2.Distance(_currPosition, mon.CorrectPosition) <= _itemStat.AttackRange)
                {
                    int direction = SeeTarget(mon.CorrectPosition);
                    _char4D?.AnimationManager.Attack();

                    SendAnimationInfo(direction, false);
                    GameManager.Sound.Play(Define.SFX.PriestAttack);

                    mon.BeAttacked(Mathf.RoundToInt(_itemStat.AttackRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetAttackCool(_itemStat.AttackCool);
                }
                else
                    Debug.Log("no near mon");
            }
            else
                Debug.Log("attack cool");

        }

        /// <summary>
        /// 사제 스킬 : 자신, 가장 가까운 아군 강화(최종 데미지 50% 상승)
        /// </summary>
        public override void IsSkill()
        {
            //쿨타임 중이 아닐 때
            if (GameManager.InGameData.Cooldown.CanSkill())
            {
                //애니메이션 재생
                int direction = SeeDirection(Vector2.down);
                _char4D?.AnimationManager?.Jab();

                SendAnimationInfo(direction, true);
                GameManager.Sound.Play(Define.SFX.PriestSkill);

                //나에게 버프 시전
                GameManager.InGameData.AddBuff(_itemStat.SkillRatio);
                //가까운 아군에게 버프 시전
                GameManager.InGameData.SendBuff(_itemStat.SkillRatio);


                GameManager.InGameData.Cooldown.SetSkillCool(_itemStat.SkillCool);
            }
            else
                Debug.Log("skill cool");
        }

        /// <summary> 패킷으로 받은 애니메이션 동기화 </summary>
        public override void SyncAnimationInfo(int direction, bool isSkill)
        {
            _char4D.SetDirection(GetDirection(direction));

            if (isSkill)
            {
                _char4D?.AnimationManager?.Jab();
                GameManager.Sound.Play(Define.SFX.PriestSkill);
            }
            else
            {
                _char4D?.AnimationManager?.Attack();
                GameManager.Sound.Play(Define.SFX.PriestAttack);
            }
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
