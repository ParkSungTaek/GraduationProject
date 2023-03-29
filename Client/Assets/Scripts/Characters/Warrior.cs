using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : PlayerController
{
    public override void IsAttack()
    {

    }
    public override void IsSkill()
    {

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