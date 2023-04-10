using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public abstract class UI_PopUp : UI_Base
    {
        /// <summary> 정렬 설정 </summary>
        public override void Init()
        {
            GameManager.UI.SetCanvas(gameObject, true);
        }

        /// <summary> pop up 닫기 </summary>
        public virtual void ClosePopUpUI()
        {
            GameManager.UI.ClosePopUpUI(this);
        }

        /// <summary>
        /// 비활성화된 UI 다시 활성화 시 호출
        /// </summary>
        public abstract void ReOpen();
    }
}
