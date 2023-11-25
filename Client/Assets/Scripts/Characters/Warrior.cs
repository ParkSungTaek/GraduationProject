/******
작성자 : 이우열
작성일 : 23.03.29

최근 수정 일자 : 23.05.16
최근 수정 사항 : 애니메이션 동기화
******/
using UnityEngine;


namespace Client
{
    public class Warrior : PlayerController
    {
        /// <summary> 인식 사거리 비율 </summary>
        const float RANGE_MULTI = 2.5f;

        /// <summary> 워리어 기본 공격 : 근접, 범위 </summary>
        public override void IsAttack()
        {
            //공격이 쿨타임 중이 아닐 때
            if (GameManager.InGameData.Cooldown.CanAttack())
            {
                MonsterController mon = NearMoster();

                //사거리 내에 몬스터가 존재할 때
                if (mon != null && Vector2.Distance(_currPosition, mon.CorrectPosition) <= RANGE_MULTI * _itemStat.AttackRange)
                {
                    int direction = SeeTarget(mon.CorrectPosition);
                    _char4D?.AnimationManager?.Attack();

                    SendAnimationInfo(direction, false);
                    GameManager.Sound.Play(Define.SFX.WarriorAttack);

                    GenerateRangedArea(_itemStat.AttackRange, mon.transform.position).SetDamage(Mathf.RoundToInt(_itemStat.AttackRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetAttackCool(_itemStat.AttackCool);
                }
            }
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
                if (mon != null && Vector2.Distance(_currPosition, mon.CorrectPosition) <= RANGE_MULTI * stat.SkillRange)
                {
                    int direction = SeeTarget(mon.CorrectPosition);
                    _char4D?.AnimationManager?.Attack();

                    SendAnimationInfo(direction, true);
                    GameManager.Sound.Play(Define.SFX.WarriorAttack);

                    GenerateRangedArea(_itemStat.SkillRange, mon.transform.position).SetDamage(Mathf.RoundToInt(_itemStat.SkillRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetSkillCool(stat.SkillCool);
                }
            }
        }

        /// <summary> 패킷으로 받은 애니메이션 동기화 </summary>
        public override void SyncAnimationInfo(int direction, bool isSkill)
        {
            _char4D.SetDirection(GetDirection(direction));
            _char4D?.AnimationManager?.Attack();
            GameManager.Sound.Play(Define.SFX.WarriorAttack);
        }

        protected override void Init()
        {
            base.Init();
            MyClass = Define.Charcter.Warrior;
            AttackDMG = 20;

            StatUpdate();
        }

    }
}