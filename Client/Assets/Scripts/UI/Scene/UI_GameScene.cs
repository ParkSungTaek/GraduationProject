using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client
{
    public class UI_GameScene : UI_Scene
    {
        /// <summary>
        /// joystick handle �⺻ ��ġ
        /// </summary>
        Vector2 _joystickPivotPos;

        /// <summary>
        /// joystick �ִ� �̵� �Ÿ�
        /// </summary>
        float _joystickLimit;
        /// <summary>
        /// joystick handle
        /// </summary>
        GameObject _joystickHandle;

        /// <summary>
        /// joystick ���� ����
        /// </summary>
        Vector2 _directionVector = Vector2.zero;

        PlayerController _player;

        enum GameObjects
        {
            joystickBG,
            joystickHandle,
        }

        public override void Init()
        {
            base.Init();
            Bind<GameObject>(typeof(GameObjects));
            JoystickBind();

            GameObject playerGO = GameManager.Resource.Instantiate("Player/TestPlayer");
            _player = Util.GetOrAddComponent<Warrior>(playerGO);
        }


        #region Joystick
        void JoystickBind()
        {
            GameObject joystickBG = Get<GameObject>((int)GameObjects.joystickBG);
            _joystickHandle = Get<GameObject>((int)GameObjects.joystickHandle);

            //�⺻ ��ġ�� �ִ� �̵� �Ÿ� ���
            _joystickPivotPos = _joystickHandle.transform.position;
            _joystickLimit = ((joystickBG.transform as RectTransform).rect.width - (_joystickHandle.transform as RectTransform).rect.width) / 2f;

            //�̺�Ʈ bind
            BindEvent(_joystickHandle, JoystickDrag, Define.UIEvent.Drag);
            BindEvent(_joystickHandle, JoystickDragEnd, Define.UIEvent.DragEnd);
        }

        /// <summary>
        /// ���̽�ƽ �巡��
        /// </summary>
        /// <param name="evt"></param>
        void JoystickDrag(PointerEventData evt)
        {
            _directionVector = (evt.position - _joystickPivotPos).normalized;

            _joystickHandle.transform.position = _joystickPivotPos + _directionVector * Mathf.Min((evt.position - _joystickPivotPos).magnitude, 50);

            _player.SetDirection(_directionVector);
        }

        /// <summary>
        /// ���̽�ƽ �巡�� ����
        /// </summary>
        /// <param name="evt"></param>
        void JoystickDragEnd(PointerEventData evt)
        {
            _directionVector = Vector2.zero;
            _joystickHandle.transform.position = _joystickPivotPos;

            _player.StopMove();
        }
        #endregion Joystick
    }
}