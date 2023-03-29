using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public abstract class Entity : MonoBehaviour
    {
        int _nowhp;
        int _maxhp;
        int _attackDMG;
        float _moveSpeed;
        Vector2 _position;

        protected int MaxHP { get { return _maxhp; } set { _maxhp = value; } }

        protected float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
        public int AttackDMG { get { return _attackDMG; } set { _attackDMG = value; } }
        public Vector2 Position { get { return _position; } set { _position = value; } }
        protected abstract void init();

        protected abstract void Dead();
        private void Start()
        {
            init();
        }

        public void BeAttacked(int DMG)
        {
            _nowhp -= DMG;
            if (_nowhp < 0)
            {
                Dead();
            }
        }

        public void Move(Vector3 _destPos)
        {
            Vector3 dir = _destPos - transform.position;
            //dir.y = 0;

            float moveDist = Mathf.Clamp(_moveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);

        }
    }
}

