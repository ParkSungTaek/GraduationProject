/******
작성자 : 공동 작성
작성 일자 : 23.03.29

최근 수정 일자 : 23.03.29
최근 수정 내용 : 클래스 생성
 ******/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public abstract class Entity : MonoBehaviour
    {
        int _nowhp;
        int _maxhp;
        int _attackDMG;
        float _moveSpeed;
        float _attackSpeed;
        Vector2 _position;
        Slider _hpBarSlider;

        protected int MaxHP { get { return _maxhp; } set { _maxhp = value; } }
        protected int Nowhp { get { return _nowhp; } set { _nowhp = value; if (_nowhp < 0) { _nowhp = 0; } } }
        public float AttackSpeed { get { return _attackSpeed; } set { _attackSpeed = value;} }

        public Slider HpBarSlider { get { return _hpBarSlider; } set { _hpBarSlider = value; } }
        
        protected float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
        public int AttackDMG { get { return _attackDMG; } set { _attackDMG = value; } }
        protected abstract void Init();

        protected abstract void Dead();
        private void Start()
        {
            Init();
        }

        public virtual void BeAttacked(int DMG)
        {
            _nowhp -= DMG;
            if (_nowhp <= 0)
            {
                Dead();
            }
            if (_hpBarSlider != null)
            {
                _hpBarSlider.value = (float)Nowhp / (float)MaxHP;
            }
            else
            {
                Debug.Log($"{name} Don't have HpBar!");
            }
        }


        public virtual void HPUpdate(int DMG)
        {
            _nowhp -= DMG;
            if (_nowhp <= 0)
            {
                Dead();
            }
            if (_hpBarSlider != null)
            {
                _hpBarSlider.value = (float)Nowhp / (float)MaxHP;
            }
            else
            {
                Debug.Log($"{name} Don't have HpBar!");
            }
        }

        public virtual void Move(Vector3 _destPos)
        {            
            transform.position = Vector3.MoveTowards(transform.position, _destPos, _moveSpeed * Time.deltaTime);
        }
    }
}

