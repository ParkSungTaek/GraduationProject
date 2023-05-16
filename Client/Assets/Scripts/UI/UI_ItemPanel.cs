/******
작성자 : 이우열
작성 일자 : 23.05.16

최근 수정 일자 : 23.05.16
최근 수정 내용 : 아이템 정보 표기를 위한 UI 클래스 생성
 ******/

using Assets.HeroEditor4D.InventorySystem.Scripts.Elements;
using System.Collections.Generic;
using TMPro;

namespace Client
{
    public class UI_ItemPanel : UI_Base
    {
        enum Texts
        {
            Item1,
            Item2,
            Item3,
            Item4,
            Item5,
            Item6,
            Item7,
            Item8
        }

        public override void Init()
        {
            Bind<TMP_Text>(typeof(Texts));

            for (int i = 0; i < 8; i++)
                GetText(i).text = string.Empty;
        }

        /// <summary> 아이템 정보 텍스트 업데이트 </summary>
        public void TextUpdate(int idx, int itemIdx)
        {
            GetText(idx).text = itemIdx.ToString();
        }
    }
}
