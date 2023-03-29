using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public abstract class PlayerController : Entity
    {
        protected Define.Charcter _myClass;
        List<Item> _items = new List<Item>(); //아이템을 +-하며 전체를 계속 참조할거라 List
        public List<Item> MyItems { get { return _items; } }
        public Define.Charcter MyClass { get { return _myClass; } }

        // 데미지 넣 는 수식은 Status.BeAttacked(float DMG)에서 통제
        public abstract void IsAttack();
        public abstract void IsSkill();
        protected override void Dead() { }
        public void IsMove()
        {
            Vector2 vector2;
            // 

        }

        public MonsterController NearMoster()
        {
            return new MonsterController();
        }
    }
}
