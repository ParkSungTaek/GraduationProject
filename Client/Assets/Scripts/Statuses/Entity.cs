using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		if(_nowhp < 0)
        {
			Dead();
        }
	}

	public void Move(Vector2 _destPos)
	{
		/*
		Vector3 dir = _destPos - transform.position;
		dir.y = 0;

		if (dir.magnitude < 0.1f)
		{
			State = Define.State.Idle;
		}
		else
		{
			Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
			if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
			{
				if (Input.GetMouseButton(0) == false)
					State = Define.State.Idle;
				return;
			}

			float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
			transform.position += dir.normalized * moveDist;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
		}
		*/
	}

}
