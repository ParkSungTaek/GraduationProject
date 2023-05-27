/******
작성자 : 이우열
작성 일자 : 23.05.16

최근 수정 일자 : 23.05.16
최근 수정 내용 : 아이템 정보 표기를 위한 UI 클래스 생성
 ******/

using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class UI_ItemPanel : UI_Base
    {
        enum Images
        {
            ItemGrid0,
            ItemGrid1,
            ItemGrid2,
            ItemGrid3,
            ItemGrid4,
            ItemGrid5,
            ItemGrid6,
            ItemGrid7,

            ItemIcon0,
            ItemIcon1,
            ItemIcon2,
            ItemIcon3,
            ItemIcon4,
            ItemIcon5,
            ItemIcon6,
            ItemIcon7,
        }

        public override void Init()
        {
            Bind<Image>(typeof(Images));

            for (int i = 0; i < 8; i++)
                GetImage(i).gameObject.SetActive(false);
        }

        /// <summary> 아이템 정보 텍스트 업데이트 </summary>
        public void ImageUpdate(int idx, ItemData item)
        {
            GetImage((int)Images.ItemGrid0 + idx).sprite = GameManager.Resource.Load<Sprite>($"Sprites/Items/Grid_{(int)item.Rank}");
            GetImage((int)Images.ItemIcon0 + idx).sprite = GameManager.Resource.Load<Sprite>($"Sprites/Items/{item.Kind}");
            GetImage(idx).gameObject.SetActive(true);
        }
    }
}
