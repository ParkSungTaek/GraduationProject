using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class MonsterController : Entity
    {

        // Start is called before the first frame update
        void Start()
        {
            init();
        }
        protected override void init()
        {
            MaxHP = 100;
            AttackDMG = 5;
            MoveSpeed = 3.0f;
        }

        protected override void Dead()
        {
            Destroy(gameObject);
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            Move(GameManager.InGameData.MonsterSpawn.transform.position);
        }

    }
}
