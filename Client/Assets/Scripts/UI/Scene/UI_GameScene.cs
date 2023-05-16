/******
작성자 : 이우열
작성일 : 23.04.01

최근 수정 일자 : 23.05.16
최근 수정 사항 : 아이템 정보 표기 UI 시작
******/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using Assets.HeroEditor4D.Common.Scripts.Common;

namespace Client
{
    public class UI_GameScene : UI_Scene
    {
        PlayerController _player { get => GameManager.InGameData.MyPlayer; }
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
            GameManager.InGameData.OnMoneyChanged += TextUpdate;

            ItemPanelInit();
        }

        #region ItemInfo
        enum ItemPanels
        {
            ItemPanel1,
            ItemPanel2,
            ItemPanel3,
            ItemPanel4,
        }

        /// <summary> 아이템 정보 표기를 위한 UI </summary>
        List<UI_ItemPanel> _itemPanelList = new List<UI_ItemPanel>();
        /// <summary> playerId로 정보 표기 UI 접근 </summary>
        Dictionary<int, UI_ItemPanel> _panelMap = new Dictionary<int, UI_ItemPanel>();

        /// <summary> 아이템 정보 표기 UI 초기화 </summary>
        void ItemPanelInit()
        {
            //bind 진행
            ItemPanels panel = ItemPanels.ItemPanel1;
            for(int i = 0;i < transform.childCount && panel <= ItemPanels.ItemPanel4;i++)
            {
                Transform child = transform.GetChild(i);

                if(child.name == panel.ToString())
                {
                    _itemPanelList.Add(Util.GetOrAddComponent<UI_ItemPanel>(child.gameObject));
                    _itemPanelList[_itemPanelList.Count - 1].Init();
                    panel++;
                }
            }

            Dictionary<int, Define.Charcter> playerInfos = GameManager.Room.GetPlayerInfo();
            panel = ItemPanels.ItemPanel1;

            foreach (var info in playerInfos)
            {
                _panelMap.Add(info.Key, _itemPanelList[(int)panel]);
                panel++;
            }

            for (; panel <= ItemPanels.ItemPanel4; panel++)
                _itemPanelList[(int)panel].SetActive(false);

            GameManager.InGameData.ItemInfoUpdate += ItemInfoUpdate;
        }
        
        /// <summary> 해당 플레이어의 아이템 정보 업데이트 </summary>
        void ItemInfoUpdate(int playerId, int idx, int itemIdx)
        {
            UI_ItemPanel panel;

            if(_panelMap.TryGetValue(playerId, out panel))
            {
                panel.TextUpdate(idx, itemIdx);
            }
        }
        #endregion ItemInfo

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
