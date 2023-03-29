using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class Priest : PlayerController
    {
        public override void IsAttack()
        {

        }
        public override void IsSkill()
        {

        }

        protected override void init()
        {

            _myClass = Define.Charcter.Priest;
            MaxHP = 98754321;
            MoveSpeed = 10;
            AttackDMG = 10;
            Position = Vector2.zero;// 시작위치
        }

    }
}
