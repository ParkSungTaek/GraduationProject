using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class UIManager
    {
        /// <summary>
        /// 팝업 UI 관리를 위한 stack
        /// </summary>
        Stack<UI_PopUp> _popupStack = new Stack<UI_PopUp>();
        /// <summary>
        /// 현재 Scene의 기본 UI
        /// </summary>
        UI_Scene _sceneUI = null;

        /// <summary>
        /// UI의 부모 
        /// </summary>
        public GameObject Root
        {
            get
            {
                GameObject root = GameObject.Find("UIRoot");
                if (root == null)
                    root = new GameObject { name = "UIRoot" };
                return root;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="go">canvas 속성이 있는 게임 오브젝트</param>
        /// <param name="sort">canvas 정렬 여부(popup->true, scene->false)</param>
        public void SetCanvas(GameObject go, bool sort = true)
        {

        }
    }
}