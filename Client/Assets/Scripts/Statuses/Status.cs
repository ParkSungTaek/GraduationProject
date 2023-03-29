using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Æó±â ¿¹Á¤
public class Status : MonoBehaviour
{
    float _hp;
    Vector2 _position;

    public void BeAttacked(float DMG)
    {
        _hp -= DMG;
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
