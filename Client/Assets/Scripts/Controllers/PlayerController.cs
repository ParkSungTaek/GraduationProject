using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public abstract class PlayerController : Entity
    {
        /// <summary> 기본 공격 데미지 계수 </summary>
        protected float _attackDMGRatio;
        /// <summary> 스킬 데미지 계수 </summary>
        protected float _skillDMGRatio;

        enum PlayerState
        { 
            Idle,
            Move,
            Attack,
            Skill,
        }

        /// <summary>
        /// 내 직업
        /// </summary>
        public Define.Charcter MyClass { get; protected set; }

        /// <summary> 공격 시전, 단일 공격 기준 </summary>
        public virtual void IsAttack()
        {
            //쿨타임 중이 아닐 때
            if (GameManager.InGameData.Cooldown.CanAttack())
            {
                MonsterController mon = NearMoster();
                Characterstat stat = GameManager.InGameData.CharacterStat[MyClass];

                //사거리 내에 몬스터가 존재할 때
                if (mon != null && Vector2.Distance(transform.position, mon.transform.position) <= stat.AttackRange)
                {
                    mon.BeAttacked(Mathf.RoundToInt(_attackDMGRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetAttackCool(stat.AttackCool);
                }
                else
                    Debug.Log("no near mon");
            }
            else
                Debug.Log("attack cool");

        }
        /// <summary> 스킬 시전, 단일 공격 기준 </summary>
        public virtual void IsSkill()
        {
            //쿨타임 중이 아닐 때
            if (GameManager.InGameData.Cooldown.CanSkill())
            {
                MonsterController mon = NearMoster();
                Characterstat stat = GameManager.InGameData.CharacterStat[MyClass];

                //사거리 내에 몬스터가 존재할 때
                if (mon != null && Vector2.Distance(transform.position, mon.transform.position) <= stat.SkillRange)
                {
                    mon.BeAttacked(Mathf.RoundToInt(_skillDMGRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetSkillCool(stat.SkillCool);
                }
                else
                    Debug.Log("no near mon");
            }
            else
                Debug.Log("skill cool");
        }
        /// <summary> 플레이어는 죽을 일 없음, 빈 함수 </summary>
        protected sealed override void Dead() { }

        #region Move
        /// <summary>
        /// 현재 플레이어 상태
        /// </summary>
        PlayerState _state = PlayerState.Idle;
        /// <summary>
        /// 이동 방향 벡터
        /// </summary>
        Vector2 _moveDirection = Vector2.zero;
        private void FixedUpdate()
        {
            if (_state == PlayerState.Move)
                IsMove();
        }

        public void IsMove() => transform.Translate(_moveDirection * Time.deltaTime * MoveSpeed);
        
        /// <summary>
        /// 조이스틱에 따라 방향 결정
        /// </summary>
        /// <param name="dir"></param>
        public void SetDirection(Vector2 dir)
        {
            _state = PlayerState.Move;
            _moveDirection = dir;
        }
        /// <summary>
        /// 조이스틱 조작 종료
        /// </summary>
        public void StopMove() => _state = PlayerState.Idle;
        #endregion Move

        #region TargetSelect
        /// <summary>
        /// 가장 가까운 몬스터 반환
        /// </summary>
        protected MonsterController NearMoster()
        {
            List<MonsterController> monsters = GameManager.InGameData.MonsterSpawn.Monsters;

            if (monsters.Count <= 0)
                return null;

            MonsterController nearMon = monsters[0];
            float pivotDis = Vector3.Distance(transform.position, nearMon.transform.position);
            for(int i = 1; i < monsters.Count;i++)
            {
                float currDis = Vector3.Distance(transform.position, monsters[i].transform.position);
                if (currDis < pivotDis)
                {
                    pivotDis = currDis;
                    nearMon = monsters[i];
                }
            }

            return nearMon;
        }
        /// <summary>
        /// 시전자에서 최대 사거리까지 이어지는 범위 공격 오브젝트 생성
        /// </summary>
        /// <param name="range">공격의 사거리</param>
        /// <param name="enemyPos">타겟 위치</param>
        protected RangedArea GenerateRangedArea(int range, Vector3 enemyPos)
        {
            GameObject rangedArea = GameManager.Resource.Instantiate("Player/RangedArea");
            rangedArea.transform.localScale = new Vector3(1, range, 1);

            float angle = Mathf.Atan2(enemyPos.y - transform.position.y, enemyPos.x - transform.position.x) * Mathf.Rad2Deg;
            rangedArea.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);


            Vector3 dir = (enemyPos - transform.position).normalized;
            rangedArea.transform.position = gameObject.transform.position + dir * range / 2f;

            return Util.GetOrAddComponent<RangedArea>(rangedArea);
        }

        /// <summary>
        /// 타겟 지점에 범위 공격 오브젝트 생성
        /// </summary>
        /// <param name="range"> 범위 오브젝트의 반경 </param>
        /// <param name="enemyPos"> 타겟 위치 </param>
        protected RangedArea GenerateTargetArea(int range, Vector3 enemyPos)
        {
            GameObject targetArea = GameManager.Resource.Instantiate("Player/TargetArea");
            targetArea.transform.localScale = new Vector3(range, range, 1);

            targetArea.transform.position = enemyPos;
            return Util.GetOrAddComponent<RangedArea>(targetArea);
        }
        #endregion TargetSelect
    }
}
