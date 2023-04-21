/*
작성자 : 이우열
작성일 : 23.03.31
최근 수정 일자 : 23.03.31
최근 수정 사항 : 기본 UI 시스템 구현
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Client
{
    public abstract class UI_Base : MonoBehaviour
    {
        /// <summary>
        /// 관리할 산하 오브젝트들
        /// </summary>
        protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

        /// <summary>
        /// UI 최초 초기화
        /// </summary>
        public abstract void Init();
        private void Start()
        {
            Init();
        }
        /// <summary>
        /// 산하의 T type object들 _objects dictionary에 저장
        /// </summary>
        /// <typeparam name="T">해당 타입</typeparam>
        /// <param name="type">해당 타입 정보 가진 enum(각 UI에서 정의)</param>
        protected void Bind<T>(Type type) where T : UnityEngine.Object
        {
            string[] names = Enum.GetNames(type);
            UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
            _objects.Add(typeof(T), objects);

            for(int i = 0; i < names.Length; i++)
            {
                if (typeof(T) == typeof(GameObject))
                    objects[i] = Util.FindChild(gameObject, names[i], true);
                else
                    objects[i] = Util.FindChild<T>(gameObject, names[i], true);

                if (objects[i] == null)
                    Debug.LogError($"Failed to bind : {names[i]} on {gameObject.name}");
            }
        }

        /// <summary>
        /// bind된 object에서 원하는 object 얻기
        /// </summary>
        protected T Get<T>(int idx) where T : UnityEngine.Object
        {
            UnityEngine.Object[] objects = null;
            if (_objects.TryGetValue(typeof(T), out objects) == false)
                return null;

            return objects[idx] as T;
        }
        #region Get_Override
        protected GameObject GetGameObject(int idx) => Get<GameObject>(idx);
        protected TMP_Text GetText(int idx) => Get<TMP_Text>(idx);

        protected Image GetImage(int idx) => Get<Image>(idx);
        protected Button GetButton(int idx) => Get<Button>(idx);
        #endregion Get_Override
    
        /// <summary>
        /// 해당 game object에 이벤트 할당
        /// </summary>
        /// <param name="action">할당할 이벤트</param>
        /// <param name="type">이벤트 발생 조건</param>
        public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
        {
            UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

            switch(type)
            {
                case Define.UIEvent.Click:
                    evt.OnClickHandler -= action;
                    evt.OnClickHandler += action;
                    break;
                case Define.UIEvent.Drag:
                    evt.OnDragHandler -= action;
                    evt.OnDragHandler += action;
                    break;
                case Define.UIEvent.DragEnd:
                    evt.OnDragEndHandler -= action;
                    evt.OnDragEndHandler += action;
                    break;
            }
        }
    }
}
