using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Client
{
    public class Warrior : PlayerController
    {
        public override void IsAttack()
        {
            NearMoster().BeAttacked(AttackDMG);
        }
        public override void IsSkill()
        {
            NearMoster().BeAttacked(Mathf.RoundToInt(AttackDMG * 1.5f));
        }

        protected override void init()
        {
            _myClass = Define.Charcter.Warrior;
            MaxHP = 98754321;
            MoveSpeed = 10.0f;
            AttackDMG = 10;
            Position = Vector2.zero;// 시작위치

        }

    }
}