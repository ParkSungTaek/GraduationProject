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
            //Move(GameManager.InGameData.MonsterSpawn.transform.position);
        }

        protected override void Dead()
        {

        }
        // Update is called once per frame
        void FixedUpdate()
        {
            Move(GameManager.InGameData.MonsterSpawn.transform.position);
        }

    }
}
