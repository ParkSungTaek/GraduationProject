using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Client
{
    /// <summary>
    /// 라이플맨 기본 공격 : 원거리, 단일 <br/>
    /// 라이플맨 스킬 : 원거리, 단일
    /// </summary>
    public class Rifleman : PlayerController
    {
        protected override void init()
        {
            MyClass = Define.Charcter.Rifleman;
            MoveSpeed = 5.0f;
            AttackDMG = 20;
            Position = Vector2.zero;// 시작위치

            _attackDMGRatio = 1.5f;
            _skillDMGRatio = 5;
        }
    }
}
