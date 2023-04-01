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


        protected Define.Charcter _myClass;
        List<Item> _items = new List<Item>(); //아이템을 +-하며 전체를 계속 참조할거라 List
        public List<Item> MyItems { get { return _items; } }
        public Define.Charcter MyClass { get { return _myClass; } }
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

        public void SetDirection(Vector2 dir)
        {
            _state = PlayerState.Move;
            _moveDirection = dir;
        }
        public void StopMove() => _state = PlayerState.Idle;

        private void FixedUpdate()
        {
            if (_state == PlayerState.Move)
                IsMove();
        }

        public MonsterController NearMoster()
        {
            return new MonsterController();
        }
    }
}
