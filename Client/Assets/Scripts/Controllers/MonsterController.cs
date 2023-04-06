using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Client
{
    public class MonsterController : Entity
    {
        [SerializeField]
        Define.MonsterName _monsterName;

        GameObject _monsterHpBar;
        Coroutine _attack;
        
        Define.MonsterState _monsterState;


        Animator _animator;
        Vector3 _offsetCorrection;
        Vector3 _monsterHpBarOffset;


        // Start is called before the first frame update
        void Start()
        {
            init();

        }

        protected override void init()
        {
            Monsterstat mystat = GameManager.InGameData.MonsterStates[_monsterName];
            _animator = GetComponent<Animator>();
            _animator.SetInteger("State", 3);

            //Debug.Log(mystat.Name);
            MaxHP = mystat.MaxHP;
            AttackDMG = mystat.AttackDMG;
            MoveSpeed = mystat.MoveSpeed;
            AttackSpeed = mystat.AttackSpeed;
            _offsetCorrection = new Vector3 (0, mystat._offsetCorrection , 0);
            _monsterHpBarOffset = new Vector3 (0, mystat._monsterHpBarOffset, 0);



            Nowhp = MaxHP;
            _monsterHpBar = Instantiate(GameManager.InGameData.MonsterHpBar);
            _monsterHpBar.transform.SetParent(GameManager.InGameData.MonsterSpawn.MonsterHPCanvas);
            HpBarSlider = _monsterHpBar.GetComponent<Slider>();
            _monsterState = Define.MonsterState.Idle;
        }

        protected override void Dead()
        {
            GameManager.InGameData.Money += GameManager.InGameData.MoneyRewards;
            GameManager.InGameData.Score += GameManager.InGameData.ScoreRewards;


            GameManager.InGameData.MonsterSpawn.Monsters.Remove(this);
            Destroy(_monsterHpBar);
            Destroy(gameObject);


        }
        // Update is called once per frame
        void FixedUpdate()
        {
            Move(GameManager.InGameData.Tower.transform.position + _offsetCorrection);
            _monsterHpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + _monsterHpBarOffset);

        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag == "Tower")
            {
                _attack = StartCoroutine(Attack());

                _animator.SetInteger("State", 1);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.transform.tag == "Tower")
            {
                _animator.SetInteger("State", 3);

                StopCoroutine(_attack);
            }
        }




        IEnumerator Attack()
        {
            while (true)
            {
                GameManager.InGameData.Tower.BeAttacked(AttackDMG);
                _animator.SetBool("Attack", true);
                yield return new WaitForSeconds(AttackSpeed);
            }
        }


    }

}
