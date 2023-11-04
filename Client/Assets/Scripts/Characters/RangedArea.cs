/******
작성자 : 이우열
작성일 : 23.04.07

최근 수정 일자 : 23.04.07
최근 수정 사항 : 범위 공격 구현
******/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    /// <summary> 범위 공격을 위한 충돌체 클래스 </summary>
    public class RangedArea : MonoBehaviour
    {
        int _dmg;
        List<Collider2D> _damaged = new List<Collider2D>();

        public void SetDamage(int dmg,float damageTime = 0.1f)
        {
            _damaged.Clear();
            _dmg = dmg;
            Destroy(gameObject, damageTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Monster" && !_damaged.Contains(collision))
            {
                _damaged.Add(collision);
                collision.GetComponent<MonsterController>()?.BeAttacked(_dmg);
            }
        }
    }
}
