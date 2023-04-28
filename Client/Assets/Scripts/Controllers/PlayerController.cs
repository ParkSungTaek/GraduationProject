/******
작성자 : 이우열
작성일 : 23.03.29

최근 수정 일자 : 23.04.14
최근 수정 사항 : 아이템 스텟 확장
******/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using Unity.VisualScripting;

namespace Client
{
    public abstract class PlayerController : Entity
    {
        /// <summary> 내 직업 </summary>
        public Define.Charcter MyClass { get; protected set; }

        /// <summary> 아이템 고려 스텟 </summary>
        [Header("Stat")]
        protected PlayerStat _itemStat = new PlayerStat();

        [Header("Animation")]
        protected Character4D _char4D;
        /// <summary> 모델 위치를 감안한 보정 위치 </summary>
        protected Vector3 _currPosition { get => transform.position + Vector3.up; }

        /// <summary> animation 연결 및 초기화 </summary>
        protected override void Init()
        {
            _char4D = GetComponent<Character4D>();
            _char4D.AnimationManager.SetState(CharacterState.Idle);

            GameObject camera = GameObject.Find("Main Camera").gameObject;
            camera.transform.parent = transform;
            camera.transform.localPosition = new Vector3(0, 1, -10);
        }
        
        /// <summary> 공격 시전, 단일 공격 기준 </summary>
        public virtual void IsAttack()
        {
            //쿨타임 중이 아닐 때
            if (GameManager.InGameData.Cooldown.CanAttack())
            {
                MonsterController mon = NearMoster();

                //사거리 내에 몬스터가 존재할 때
                if (mon != null && Vector2.Distance(_currPosition, mon.CorrectPosition) <= _itemStat.AttackRange)
                {
                    SeeTarget(mon.CorrectPosition);
                    _char4D.AnimationManager.Attack();

                    mon.BeAttacked(Mathf.RoundToInt(_itemStat.AttackRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetAttackCool(_itemStat.AttackCool);
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

                //사거리 내에 몬스터가 존재할 때
                if (mon != null && Vector2.Distance(_currPosition, mon.CorrectPosition) <= _itemStat.SkillRange)
                {
                    SeeTarget(mon.CorrectPosition);
                    _char4D.AnimationManager.Attack();

                    mon.BeAttacked(Mathf.RoundToInt(_itemStat.SkillRatio * AttackDMG));
                    GameManager.InGameData.Cooldown.SetSkillCool(_itemStat.SkillCool);
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
        enum PlayerState
        {
            Idle,
            Move,
            Attack,
            Skill,
        }

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

        public void IsMove() => transform.Translate(_moveDirection * Time.deltaTime * _itemStat.Speed);
        
        /// <summary>
        /// 조이스틱에 따라 방향 결정
        /// </summary>
        /// <param name="dir"></param>
        public void SetDirection(Vector2 dir)
        {
            _state = PlayerState.Move;
            _moveDirection = dir;
            SeeDirection(dir);
            _char4D.AnimationManager.SetState(CharacterState.Run);
        }
        /// <summary>
        /// 조이스틱 조작 종료
        /// </summary>
        public void StopMove()
        {
            _state = PlayerState.Idle;
            _char4D.AnimationManager.SetState(CharacterState.Idle);
        }
        #endregion Move

        #region TargetSelect
        /// <summary> 가장 가까운 몬스터 반환 </summary>
        protected MonsterController NearMoster()
        {
            //현재 맵에 존재하는 맵들 받아옴
            List<MonsterController> monsters = GameManager.InGameData.MonsterSpawn.Monsters;

            if (monsters.Count <= 0)
                return null;

            MonsterController nearMon = monsters[0];
            float pivotDis = Vector3.Distance(_currPosition, nearMon.CorrectPosition);
            for(int i = 1; i < monsters.Count;i++)
            {
                float currDis = Vector3.Distance(_currPosition, monsters[i].CorrectPosition);
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
        protected RangedArea GenerateRangedArea(float range, Vector3 enemyPos)
        {
            GameObject rangedArea = GameManager.Resource.Instantiate("Player/RangedArea");
            rangedArea.transform.localScale = new Vector3(1, range, 1);

            float angle = Mathf.Atan2(enemyPos.y - _currPosition.y, enemyPos.x - _currPosition.x) * Mathf.Rad2Deg;
            rangedArea.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);


            Vector3 dir = (enemyPos - _currPosition).normalized;
            rangedArea.transform.position =  _currPosition + dir * range / 2f;

            return Util.GetOrAddComponent<RangedArea>(rangedArea);
        }

        /// <summary>
        /// 타겟 지점에 범위 공격 오브젝트 생성
        /// </summary>
        /// <param name="range"> 범위 오브젝트의 반경 </param>
        /// <param name="enemyPos"> 타겟 위치 </param>
        protected RangedArea GenerateTargetArea(float range, Vector3 enemyPos)
        {
            GameObject targetArea = GameManager.Resource.Instantiate("Player/TargetArea");
            targetArea.transform.localScale = new Vector3(range, range, 1);

            targetArea.transform.position = enemyPos;
            return Util.GetOrAddComponent<RangedArea>(targetArea);
        }
        #endregion TargetSelect

        #region AnimationDirection
        /// <summary>
        /// 대상 위치에 따라 상하좌우 중 가장 근접한 방향으로 애니메이션 돌리기
        /// </summary>
        /// <param name="targetPos">대상 위치</param>
        protected void SeeTarget(Vector3 targetPos) => SeeDirection((targetPos - _currPosition).normalized);

        /// <summary>
        /// 조이스틱 방향에 따라 상하좌우 중 가장 근접한 방향으로 애니메이션 돌리기
        /// </summary>
        /// <param name="dir">조이스틱 방향(normalized)</param>
        protected void SeeDirection(Vector2 dir)
        {
            Vector2 resultDir;
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                resultDir = (dir.x < 0 ? Vector2.left : Vector2.right);
            else
                resultDir = (dir.y < 0 ? Vector2.down : Vector2.up);

            _char4D.SetDirection(resultDir);
        }
        #endregion AnimationDirection

        #region StatUpdate
        /// <summary> 아이템 및 버프 상태에 따른 스텟 계산 </summary>
        public void StatUpdate()
        {
            float[] pivots = new float[(int)Define.ItemKind.MaxCount] { 1, 1, 1, 0, 0, 0 };

            //아이템 효과 추가
            List<ItemData> items = GameManager.InGameData.MyInventory;
            foreach(ItemData item in items)
            {
                switch(item.Kind)
                {
                    case Define.ItemKind.Cooldown:
                        pivots[(int)item.Kind] = Mathf.Max(0, pivots[(int)item.Kind] - item.Stat);
                        break;
                    case Define.ItemKind.Slow:
                        pivots[(int)item.Kind] = Mathf.Max(pivots[(int)item.Kind], item.Stat);
                        break;
                    default:
                        pivots[(int)item.Kind] += item.Stat;
                        break;
                }
            }

            //버프 효과 추가
            pivots[(int)Define.ItemKind.Damage] *= GameManager.InGameData.Buff.GetBuffRate();

            PlayerStat basicStat = GameManager.InGameData.PlayerStats[MyClass];

            _itemStat.AttackRatio = basicStat.AttackRatio * pivots[(int)Define.ItemKind.Damage];
            _itemStat.SkillRatio = basicStat.SkillRatio * pivots[(int)Define.ItemKind.Damage];

            _itemStat.AttackRange = basicStat.AttackRange * pivots[(int)Define.ItemKind.Range];
            _itemStat.SkillRange = basicStat.SkillRange * pivots[(int)Define.ItemKind.Range];

            _itemStat.AttackCool = basicStat.AttackCool * pivots[(int)Define.ItemKind.Cooldown];
            _itemStat.SkillCool = basicStat.SkillCool * pivots[(int)Define.ItemKind.Cooldown];

            gameObject.GetComponent<Rigidbody2D>().mass = _itemStat.Weight = basicStat.Weight + pivots[(int)Define.ItemKind.Weight];
            _itemStat.Speed = basicStat.Speed + pivots[(int)Define.ItemKind.Speed];
            _itemStat.Slow = basicStat.Slow + pivots[(int)Define.ItemKind.Slow];

            Debug.Log($"stat update : {_itemStat}");
        }
        #endregion StatUpdate
    }
}
