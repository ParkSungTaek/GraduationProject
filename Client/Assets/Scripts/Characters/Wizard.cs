using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class Wizard : PlayerController
    {
        public override void IsAttack()
        {

        }
        public override void IsSkill()
        {

        }

        protected override void init()
        {
            _myClass = Define.Charcter.Wizard;
            MaxHP = 98754321;
            MoveSpeed = 10.0f;
            AttackDMG = 10;
            Position = Vector2.zero;// 시작위치


        }
    }
}