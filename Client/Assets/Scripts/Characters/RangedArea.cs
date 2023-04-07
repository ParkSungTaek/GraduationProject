using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class RangedArea : MonoBehaviour
    {
        int _dmg;
        List<Collider2D> _damaged = new List<Collider2D>();

        public void SetDamage(int dmg)
        {
            _damaged.Clear();
            _dmg = dmg;
            Destroy(gameObject, 0.1f);
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
