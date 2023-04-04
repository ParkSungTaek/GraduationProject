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
        public static Action ShopAction { get; private set; }
        
        enum Buttons
        { 
            CloseBtn,
            BuyBtn,
        }
        enum Items
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


        public override void Init()
        {
            base.Init();
            Bind<Button>(typeof(Buttons));
            Bind<GameObject>(typeof(Items));


            BindEvent(GetButton((int)Buttons.CloseBtn).gameObject, Btn_Close);
            BindEvent(GetButton((int)Buttons.BuyBtn).gameObject, Btn_Buy);
            Buy_ActiveControl();
            for (int i = 0 ; i < GameManager.InGameData.MyInventory.Count; i++)
            {
                GetGameObject(i).GetComponent<TextMeshProUGUI>().text = GameManager.InGameData.MyInventory[i].Name;
            }
            
        }
        private void OnEnable()
        {
            ShopAction += Buy_ActiveControl;
        }

        #region Buttons
        void Btn_Close(PointerEventData evt)
        {
            ShopAction -= Buy_ActiveControl;
            ShopAction = null;
            GameManager.UI.ClosePopUpUI(this);
        }

        void Buy_ActiveControl()
        {

            if (GameManager.InGameData.Money >= GameManager.InGameData.ItemCost)
            {
                GetButton((int)Buttons.BuyBtn).gameObject.SetActive(true);
            }
            else
            {
                GetButton((int)Buttons.BuyBtn).gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// 당연히 이런식으로 글로 쓸 게 아니기 때문에 안에 바뚸야해
        /// </summary>
        /// <param name="evt"></param>
        void Btn_Buy(PointerEventData evt)
        {
            if(GameManager.InGameData.Money - GameManager.InGameData.ItemCost >= 0)
            {
                GameManager.InGameData.Money -= GameManager.InGameData.ItemCost;

                ///여기부터 아이템 어찌할지 확정하고 바꿔야해

                GameManager.InGameData.MyInventoryRandomADD();
                GetGameObject(GameManager.InGameData.MyInventory.Count-1).GetComponent<TextMeshProUGUI>().text = GameManager.InGameData.MyInventory[GameManager.InGameData.MyInventory.Count - 1].Name;
                ///

            }
        }
        #endregion Buttons
    }
}
