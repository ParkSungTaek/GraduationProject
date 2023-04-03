using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public abstract class PlayerController : Entity
    {
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
        protected Define.Charcter _myClass;
        /// <summary>
        /// 현재 가지고 있는 아이템 리스트
        /// </summary>
        List<Item> _items = new List<Item>();

        public List<Item> MyItems { get { return _items; } }
        public Define.Charcter MyClass { get { return _myClass; } }

        /// <summary>
        /// 현재 플레이어 상태
        /// </summary>
        PlayerState _state = PlayerState.Idle;
        /// <summary>
        /// 이동 방향 벡터
        /// </summary>
        Vector2 _moveDirection = Vector2.zero;

        // 데미지 넣 는 수식은 Status.BeAttacked(float DMG)에서 통제
        public abstract void IsAttack();
        public abstract void IsSkill();
        protected override void Dead() { }
        public void IsMove()
        {
            transform.Translate(_moveDirection * Time.deltaTime * MoveSpeed);
        }

        #region Joystick
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
        #endregion Joystick

        private void FixedUpdate()
        {
            if (_state == PlayerState.Move)
                IsMove();
        }

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
    }
}
