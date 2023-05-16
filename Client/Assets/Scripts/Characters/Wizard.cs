/******
작성자 : 이우열
작성일 : 23.03.29

최근 수정 일자 : 23.05.16
최근 수정 사항 : 애니메이션 동기화
 ******/

using UnityEngine;

namespace Client
{
    public class Wizard : PlayerController
    {
        /// <summary> 위저드 기본 공격 : 원거리 단일 </summary>
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
                    GameManager.Sound.Play(Define.SFX.WizzardAttack);

                    mon.BeAttacked(Mathf.RoundToInt(_itemStat.AttackRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetAttackCool(_itemStat.AttackCool);
                }
                else
                    Debug.Log("no near mon");
            }
            else
                Debug.Log("attack cool");

        }

        /// <summary> 위저드 스킬 : 원거리 범위(타겟 중심) </summary>
        public override void IsSkill()
        {
            //쿨타임 중이 아닐 떄
            if (GameManager.InGameData.Cooldown.CanSkill())
            {
                MonsterController mon = NearMoster();

                //사거리 내에 몬스터가 존재할 때
                if (mon != null && Vector2.Distance(_currPosition, mon.CorrectPosition) <= _itemStat.SkillRange)
                {
                    int direction = SeeTarget(mon.CorrectPosition);
                    _char4D?.AnimationManager?.Jab();

                    SendAnimationInfo(direction, true);
                    GameManager.Sound.Play(Define.SFX.WizzardAttack);

                    GenerateTargetArea(1, mon.transform.position).SetDamage(Mathf.RoundToInt(_itemStat.SkillRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetSkillCool(_itemStat.SkillCool);
                }
            }
            else
                Debug.Log("skill cool");
        }

        /// <summary> 패킷으로 받은 애니메이션 동기화 </summary>
        public override void SyncAnimationInfo(int direction, bool isSkill)
        {
            _char4D.SetDirection(GetDirection(direction));

            if (isSkill)
                _char4D?.AnimationManager?.Jab();
            else
                _char4D?.AnimationManager?.Attack();

            GameManager.Sound.Play(Define.SFX.WizzardAttack);
        }

        protected override void Init()
        {
            base.Init();
            MyClass = Define.Charcter.Wizard;
            AttackDMG = 20;

            StatUpdate();
        }
    }
}