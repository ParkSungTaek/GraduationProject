/******
작성자 : 이우열
작성 일자 : 23.03.29

최근 수정 일자 : 23.09.18
최근 수정 사항 : <박성택> ShowSceneUI 함수가 단순히 UI를 생성하는 것이 아니라 만약 UI가 이미 Scene에 존재한다면 그 UI를 찾아서 반환하는것으로 바꿈 
******/

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
        [Header("Pop Up")]
        Stack<UI_PopUp> _popupStack = new Stack<UI_PopUp>();
        /// <summary>
        /// popup ui 정렬 순서를 위한 변수
        /// </summary>
        int _order = 1;

        /// <summary>
        /// popup 재사용을 위한 캐싱
        /// </summary>
        Dictionary<System.Type, GameObject> _popupInstances = new Dictionary<System.Type, GameObject>();

        /// <summary>
        /// UI의 부모 
        /// </summary>
        [Header("Root")]
        GameObject _root = null;
        public GameObject Root
        {
            get
            {
                if (_root == null)
                {
                    if ((_root = GameObject.Find("UIRoot")) == null)
                        _root = new GameObject { name = "UIRoot" };
                }
                return _root;
            }
        }

        /// <summary>
        /// game object에 canvas 속성 부여, 정렬 설정
        /// </summary>
        /// <param name="go">canvas 속성이 있는 게임 오브젝트</param>
        /// <param name="sort">canvas 정렬 여부(popup->true, scene->false)</param>
        public void SetCanvas(GameObject go, bool sort = true, int order = 0)
        {
            Canvas canvas = Util.GetOrAddComponent<Canvas>(go);

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;

            if (sort)
                canvas.sortingOrder = _order++;
            else
                canvas.sortingOrder = order;
        }

        /// <summary>
        /// Scene 기본 UI 띄우기
        /// </summary>
        /// <typeparam name="T">UI_Scene을 상속받은 각 Scene의 UI</typeparam>
        /// <param name="name">Scene UI 이름, null이면 T 이름</param>
        public T ShowSceneUI<T>(string name = null) where T : UI_Scene
        {
            if(string.IsNullOrEmpty(name))
                name = typeof(T).Name;
            T sceneUI = Root.GetComponentInChildren<T>();
            if (sceneUI)
            {
                return sceneUI;
            }
            else
            {
                GameObject go = GameManager.Resource.Instantiate($"UI/Scene/{name}");
                sceneUI = Util.GetOrAddComponent<T>(go);

                go.transform.SetParent(Root.transform);
                sceneUI.Init();

                return sceneUI;
            }
            
        }

        /// <summary>
        /// Pop Up UI 띄우기
        /// </summary>
        /// <typeparam name="T">UI_PopUp을 상속받은 Pop up UI</typeparam>
        /// <param name="name">Pop Up UI 이름, null이면 T 이름</param>
        public T ShowPopUpUI<T>(string name = null) where T : UI_PopUp
        {
            if(string.IsNullOrEmpty(name))
                name = typeof(T).Name;

            GameObject popup;
            T popupUI;

            //이전에 띄운 기록 없음 -> 생성
            if (_popupInstances.TryGetValue(typeof(T), out popup) == false)
            {
                popup = GameManager.Resource.Instantiate($"UI/PopUp/{name}");
                _popupInstances.Add(typeof(T), popup);

                popupUI = Util.GetOrAddComponent<T>(popup);
                popupUI.Init();
            }
            //이전에 띄운 기록 있음 -> 재활성화
            else
            {
                popupUI = Util.GetOrAddComponent<T>(popup);
                popupUI.GetComponent<Canvas>().sortingOrder = _order++;
                popupUI.ReOpen();
            }

            _popupStack.Push(popupUI);

            popup.transform.SetParent(Root.transform);
            popup.SetActive(true);

            return popupUI;
        }

        /// <summary>
        /// 특정 pop up UI 닫기, stack의 제일 위가 아니면 수행 X
        /// </summary>
        /// <param name="popup">닫고자 하는 popup</param>
        public void ClosePopUpUI(UI_PopUp popup)
        {
            if(_popupStack.Count <= 0) return;

            if (_popupStack.Peek() != popup)
            {
                Debug.LogError("Pop Up doesn't match. Can't close pop up.");
                return;
            }

            ClosePopUpUI();
        }
        /// <summary>
        /// 가장 위의 pop up UI 닫기
        /// </summary>
        public void ClosePopUpUI()
        {
            if (_popupStack.Count <= 0) return;

            UI_PopUp popup = _popupStack.Pop();
            popup.gameObject.SetActive(false);

            _order--;
        }

        /// <summary>
        /// 모든 pop up UI 닫기
        /// </summary>
        public void CloseAllPopUpUI()
        {
            while (_popupStack.Count > 0)
                ClosePopUpUI();
        }

        /// <summary>
        /// UI 초기화
        /// </summary>
        public void Clear()
        {
            CloseAllPopUpUI();
            _popupInstances.Clear();
        }
    }
}