using System;
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

        public Vector3 CorrectPosition { get => transform.position - _offsetCorrection / 2; }


        // Start is called before the first frame update
        void Start()
        {
            init();

        }

        protected override void init()
        {
            _monsterName = (Define.MonsterName)Enum.Parse(typeof(Define.MonsterName), name);
            MonsterStat mystat = GameManager.InGameData.MonsterStats[_monsterName];
            _animator = GetComponent<Animator>();
            _animator.SetInteger("State", 3);
            _monsterState = Define.MonsterState.Idle;

            //Debug.Log(mystat.Name);
            MaxHP = mystat.MaxHP;
            AttackDMG = mystat.AttackDMG;
            MoveSpeed = mystat.MoveSpeed;
            AttackSpeed = mystat.AttackSpeed;
            _offsetCorrection = new Vector3 (0, mystat._offsetCorrection , 0);
            _monsterHpBarOffset = new Vector3 (0, mystat._monsterHpBarOffset, 0);



            Debug.Log(
@$"name = {mystat.Name};
MaxHP = {mystat.MaxHP};
AttackDMG = {mystat.AttackDMG};
MoveSpeed = {mystat.MoveSpeed};
AttackSpeed = {mystat.AttackSpeed};
_offsetCorrection = new Vector3 (0, {mystat._offsetCorrection} , 0);
_monsterHpBarOffset = new Vector3 (0, {mystat._monsterHpBarOffset}, 0);
            ");

            Nowhp = MaxHP;
            _monsterHpBar = Instantiate(GameManager.InGameData.HPBarPrefab);
            _monsterHpBar.transform.SetParent(GameManager.InGameData.MonsterSpawn.MonsterHPCanvas);
            HpBarSlider = _monsterHpBar.GetComponent<Slider>();
            _monsterState = Define.MonsterState.Idle;
        }

        protected override void Dead()
        {
            _animator.SetInteger("State", 9);
            _monsterState = Define.MonsterState.Death;
            GameManager.InGameData.Money += GameManager.InGameData.MoneyRewards;
            GameManager.InGameData.Score += GameManager.InGameData.ScoreRewards;
            if (_attack != null)
            {
                StopCoroutine(_attack);
            }
            GameManager.InGameData.MonsterSpawn.Monsters.Remove(this);
            if(GameManager.InGameData.MonsterSpawn.Monsters.Count == 0 && GameManager.InGameData.MonsterSpawn.WaveEnd())
            {
                GameManager.GameOver(Define.State.Win);
            }
            
            Destroy(_monsterHpBar);

            gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject,1.0f);

        }
        // Update is called once per frame
        void FixedUpdate()
        {
            if (_monsterState != Define.MonsterState.Death)
            {
                Move(GameManager.InGameData.Tower.transform.position + _offsetCorrection);
                _monsterHpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + _monsterHpBarOffset);
            }
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag == "Tower" && _monsterState != Define.MonsterState.Death)
            {
                _attack = StartCoroutine(Attack());

                _animator.SetInteger("State", 1);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.transform.tag == "Tower" && _monsterState != Define.MonsterState.Death)
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

                _monsterState = Define.MonsterState.Attack;
                _animator.SetBool("Attack", true);
                _monsterState = Define.MonsterState.Idle;

                yield return new WaitForSeconds(AttackSpeed);
            }
        }


    }

}
