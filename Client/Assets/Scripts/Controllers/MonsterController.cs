/******
작성자 : 박성택
작성 일자 : 23.03.31

최근 수정 일자 : 23.04.28
최근 수정 내용 : 좌표 보정식 수정
 ******/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Client
{
    public class MonsterController : Entity
    {
        #region 몬스터 특성 정보
        [SerializeField]
        Define.MonsterName _monsterName;

        GameObject _monsterHpBar;
        Coroutine _attack;
        Define.MonsterState _monsterState;
        Animator _animator;
        Vector3 _offsetCorrection;
        Vector3 _monsterHpBarOffset;
        #endregion 몬스터 특성 정보

        #region 몬스터 서버 동기화용 정보
        public ushort MonsterID { get; set; }
        public ushort AttackCnt { get; set; }
        #endregion 몬스터 서버 동기화용 정보

        public Vector3 CorrectPosition { get => transform.position + _offsetCorrection; }


        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        protected override void Init()
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
            _offsetCorrection = new Vector3 (0, GetComponent<BoxCollider2D>().offset.y * transform.localScale.y, 0);
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
            AttackCnt = 1;
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
            GameManager.InGameData.MonsterSpawn.Monsters.Remove(this.MonsterID);
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
                Move(GameManager.InGameData.Tower.transform.position - _offsetCorrection);
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

        public override void BeAttacked(int DMG)
        {
            CTS_MonsterHPUpdate hpUpdatePacket = new CTS_MonsterHPUpdate();
            hpUpdatePacket.ID = MonsterID;
            hpUpdatePacket.updateHP = (short)DMG;

            GameManager.Network.Send(hpUpdatePacket.Write());
        }

        public override void HPUpdate(int DMG)
        {
            Nowhp -= DMG;
            Debug.Log($"ID: {MonsterID} _ {Nowhp}");
            if (Nowhp <= 0)
            {
                Dead();
            }
            if (HpBarSlider != null)
            {
                HpBarSlider.value = (float)Nowhp / (float)MaxHP;
            }
            else
            {
                Debug.Log($"{name} Don't have HpBar!");
            }
        }

        IEnumerator Attack()
        {
            while (true)
            {
                GameManager.InGameData.Tower.BeAttacked(AttackDMG);

                CTS_TowerDamage towerDamage = new CTS_TowerDamage();
                towerDamage.DMG = (ushort)AttackDMG;
                towerDamage.MonsterID = MonsterID;
                towerDamage.AttackCnt = AttackCnt;
                GameManager.Network.Send(towerDamage.Write());
                AttackCnt++;


                _monsterState = Define.MonsterState.Attack;
                _animator.SetBool("Attack", true);
                _monsterState = Define.MonsterState.Idle;

                yield return new WaitForSeconds(AttackSpeed);
            }
        }


    }

}
