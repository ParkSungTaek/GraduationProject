using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Client
{
    public class UI_GetItem : UI_PopUp
    {
        /// <summary> 재화 수치 변경 시 콜백 </summary>
        public static Action OnMoneyChangedAction { get; private set; }
        
        enum Buttons
        { 
            CloseBtn,
            BuyBtn,
        }
        enum ItemTexts
        {
            Item0,
            Item1,
            Item2,
            Item3,
            Item4,
            Item5,
            Item6,
            Item7,
        }

        /// <summary> UI 최초 생성 - bind, 텍스트 초기화 진행 </summary>
        public override void Init()
        {
            base.Init();

            //오브젝트 바인드
            Bind<Button>(typeof(Buttons));
            Bind<TMP_Text>(typeof(ItemTexts));

            //버튼에 이벤트 연결
            BindEvent(GetButton((int)Buttons.CloseBtn).gameObject, Btn_Close);
            BindEvent(GetButton((int)Buttons.BuyBtn).gameObject, Btn_Buy);

            OnMoneyChangedAction = Buy_ActiveControl;
            Buy_ActiveControl();

            //아이템 텍스트 초기화
            for (int i = 0 ; i < GameManager.InGameData.MyInventory.Count; i++)
                GetText(i).text = GameManager.InGameData.MyInventory[i].Name;
        }

        /// <summary> 비활성화 상태에서 다시 열 시 호출 </summary>
        public override void ReOpen()
        {
            OnMoneyChangedAction = Buy_ActiveControl;
            Buy_ActiveControl();
        }

        #region Buttons
        /// <summary> 닫기 버튼 - 텍스트 업데이트 비활성화 </summary>
        void Btn_Close(PointerEventData evt)
        {
            OnMoneyChangedAction = null;
            GameManager.UI.ClosePopUpUI(this);
        }
        
        /// <summary> 소지금에 따라 구매 버튼 활성화/비활성화 </summary>
        void Buy_ActiveControl()
        {
            GetButton((int)Buttons.BuyBtn).gameObject.SetActive(GameManager.InGameData.CanBuyItem);
            //GetButton((int)Buttons.BuyBtn).interactable = GameManager.InGameData.Money >= GameManager.InGameData.ItemCost;
        }
        /// <summary> 아이템 구매 버튼 </summary>
        void Btn_Buy(PointerEventData evt)
        {
            if(GameManager.InGameData.CanBuyItem)
            {
                GameManager.InGameData.Money -= GameManager.InGameData.ItemCost;

                ///여기부터 아이템 어찌할지 확정하고 바꿔야해

                GameManager.InGameData.AddRandomItem();
                GetText(GameManager.InGameData.MyInventory.Count - 1).text = GameManager.InGameData.MyInventory[GameManager.InGameData.MyInventory.Count - 1].Name;
                ///

            }
        }
        #endregion Buttons
    }
}
