/******
작성자 : 이우열
작성일 : 23.04.01

최근 수정 일자 : 23.04.07
최근 수정 사항 : 플레이어 생성 기능 InGameManager로 이전
******/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

namespace Client
{
    public class UI_GameScene : UI_Scene
    {
        PlayerController _player { get => GameManager.InGameData.MyPlayer; }
        public static Action TextChangeAction { get; private set; }
        enum GameObjects
        {
            joystickBG,
            joystickHandle,
        }
        enum Buttons
        {
            AttackBtn,
            SkillBtn,
            ItemBtn,
        }
        enum Texts 
        { 
            MoneyTxt,
            ScoreTxt
        }

        public override void Init()
        {
            base.Init();
            Bind<GameObject>(typeof(GameObjects));
            Bind<Button>(typeof(Buttons));
            Bind<TMP_Text>(typeof(Texts));

            attackBtnImage = GetButton((int)Buttons.AttackBtn).GetComponent<Image>();
            skillBtnImage = GetButton((int)Buttons.SkillBtn).GetComponent<Image>();

            JoystickBind();
            ButtonBind();
            TextChangeAction += TextUpdate;
        }

        #region BtnImageUpdate
        Image attackBtnImage;
        Image skillBtnImage;
        private void Update()
        {
            attackBtnImage.fillAmount = GameManager.InGameData.Cooldown.GetAttackCoolRate();
            skillBtnImage.fillAmount = GameManager.InGameData.Cooldown.GetSkillCoolRate();
        }
        #endregion BtnImageUpdate

        #region Joystick
        /// <summary>
        /// joystick handle 기본 위치
        /// </summary>
        Vector2 _joystickPivotPos;

        /// <summary>
        /// joystick 최대 이동 거리
        /// </summary>
        float _joystickLimit;
        /// <summary>
        /// joystick handle
        /// </summary>
        GameObject _joystickHandle;

        /// <summary>
        /// joystick 방향 벡터
        /// </summary>
        Vector2 _directionVector = Vector2.zero;

        void JoystickBind()
        {
            GameObject joystickBG = Get<GameObject>((int)GameObjects.joystickBG);
            _joystickHandle = Get<GameObject>((int)GameObjects.joystickHandle);

            //기본 위치와 최대 이동 거리 계산
            _joystickPivotPos = _joystickHandle.transform.position;
            _joystickLimit = ((joystickBG.transform as RectTransform).rect.width - (_joystickHandle.transform as RectTransform).rect.width) / 2f;

            //이벤트 bind
            BindEvent(_joystickHandle, JoystickDrag, Define.UIEvent.Drag);
            BindEvent(_joystickHandle, JoystickDragEnd, Define.UIEvent.DragEnd);
        }

        /// <summary>
        /// 조이스틱 드래그
        /// </summary>
        /// <param name="evt"></param>
        void JoystickDrag(PointerEventData evt)
        {
            _directionVector = (evt.position - _joystickPivotPos).normalized;

            _joystickHandle.transform.position = _joystickPivotPos + _directionVector * Mathf.Min((evt.position - _joystickPivotPos).magnitude, 50);

            _player?.SetDirection(_directionVector);
        }

        /// <summary>
        /// 조이스틱 드래그 종료
        /// </summary>
        /// <param name="evt"></param>
        void JoystickDragEnd(PointerEventData evt)
        {
            _directionVector = Vector2.zero;
            _joystickHandle.transform.position = _joystickPivotPos;

            _player?.StopMove();
        }
        #endregion Joystick

        #region Buttons
        void ButtonBind()
        {
            BindEvent(GetButton((int)Buttons.AttackBtn).gameObject, Btn_Attack);
            BindEvent(GetButton((int)Buttons.SkillBtn).gameObject, Btn_Skill);
            BindEvent(GetButton((int)Buttons.ItemBtn).gameObject, Btn_GetItem);
        }

        /// <summary>
        /// 기본 공격 버튼
        /// </summary>
        void Btn_Attack(PointerEventData evt)
        {
            _player?.IsAttack();
        }

        /// <summary>
        /// 스킬 시전 버튼
        /// </summary>
        void Btn_Skill(PointerEventData evt)
        {
            _player?.IsSkill();
        }

        /// <summary>
        /// 아이템 뽑기 버튼
        /// </summary>
        void Btn_GetItem(PointerEventData evt)
        {
            GameManager.UI.ShowPopUpUI<UI_GetItem>();
        }
        #endregion Buttons

        #region Texts
        void TextUpdate()
        {
            GetText((int)Texts.MoneyTxt).text = $"Money: {GameManager.InGameData.Money}";
            GetText((int)Texts.ScoreTxt).text = $"Score: {GameManager.InGameData.Score}";
        }
        #endregion
    }
}
