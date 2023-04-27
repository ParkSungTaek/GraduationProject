/******
작성자 : 이우열
작성 일자 : 23.04.27

최근 수정 일자 : 23.04.27
최근 수정 내용 : 클래스 생성
 ******/

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class UI_DropItem : UI_PopUp
    {
        enum Buttons
        {
            Drop0, Drop1, Drop2, Drop3, Drop4, Drop5, Drop6, Drop7, DropNew,
        }

        enum Texts
        {
            ItemTxt0, ItemTxt1, ItemTxt2, ItemTxt3, ItemTxt4, ItemTxt5, ItemTxt6, ItemTxt7, ItemTxtNew,
        }

        /// <summary> 버리기 전까지 새로 뽑은 아이템 정보 저장 </summary>
        ItemData _newItem = null;
        /// <summary> 아이템 변경 시 아이템 텍스트 정보 변경 </summary>
        Action<int> _textUpdate = null;

        public override void Init()
        {
            base.Init();
            Bind<Button>(typeof(Buttons));
            Bind<TMP_Text>(typeof(Texts));

            BindEvent();
        }

        /// <summary> 버튼에 event 할당 </summary>
        void BindEvent()
        {
            for (Buttons b = Buttons.Drop0; b <= Buttons.DropNew; b++)
                BindEvent(GetButton((int)b).gameObject, Btn_DropItem, (int)b);
        }

        /// <summary> 아이템 버리기 </summary>
        void Btn_DropItem(PointerEventData evt, int idx)
        {
            Buttons kind = (Buttons)idx;

            if (kind != Buttons.DropNew)
            {
                GameManager.InGameData.ReplaceItem(idx, _newItem);
                _textUpdate(idx);
            }         

            _newItem = null;
            ClosePopUpUI();
        }

        /// <summary> 보여줄 아이템 정보 업데이트 </summary>
        public void ItemUpdate(ItemData newItem, Action<int> textUpdate)
        {
            List<ItemData> items = GameManager.InGameData.MyInventory;
            TMP_Text text;
            for (int i = 0; i < items.Count;i++)
            {
                text = GetText(i);
                text.text = items[i].Name;
            }

            text = GetText((int)Texts.ItemTxtNew);
            text.text = newItem.Name;
            
            _newItem = newItem;
            _textUpdate = textUpdate;
        }

        public override void ReOpen()
        {

        }
    }
}
