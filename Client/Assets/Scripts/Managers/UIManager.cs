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
        /// 
        /// GameObject root 밖으로 빼고 
        /// if(root == null)
        ///     GameObject root = GameObject.Find("UIRoot");
        /// 하면 좀 더 싸지 않을까...하는 생각을 한번 해봄. 왠지 엄청 자주 접근할것 같은데 매번 Find면 조금 마음 아프달까......
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