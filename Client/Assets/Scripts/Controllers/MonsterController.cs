using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Client
{
    public class MonsterController : Entity
    {

        GameObject _monsterHpBar;


        // Start is called before the first frame update
        void Start()
        {
            init();

        }

        protected override void init()
        {
            MaxHP = 100;
            Nowhp = MaxHP;
            AttackDMG = 2;
            MoveSpeed = 3.0f;
            AttackSpeed = 1.0f;
            _monsterHpBar = Instantiate(GameManager.InGameData.MonsterHpBar);
            _monsterHpBar.transform.SetParent(GameManager.InGameData.MonsterSpawn.MonsterHPCanvas);

            HpBarSlider = _monsterHpBar.GetComponent<Slider>();

        }

        protected override void Dead()
        {
            GameManager.InGameData.Money += GameManager.InGameData.MoneyRewards;
            GameManager.InGameData.Score += GameManager.InGameData.ScoreRewards;

            UI_GameScene.TextsAction.Invoke();
            GameManager.InGameData.MonsterSpawn.Monsters.Remove(this);
            Destroy(_monsterHpBar);
            Destroy(gameObject);


        }
        // Update is called once per frame
        void FixedUpdate()
        {
            Move(GameManager.InGameData.MonsterSpawn.transform.position);
            _monsterHpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.8f, 0));

        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag == "Tower")
            {
                StartCoroutine(Attack());
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.transform.tag == "Tower")
            {

                StopCoroutine(Attack());
            }
        }

        


        IEnumerator Attack()
        {
            while (true)
            {
                GameManager.InGameData.Tower.BeAttacked(AttackDMG);
                yield return new WaitForSeconds(AttackSpeed);
            }
        }


    }

}
