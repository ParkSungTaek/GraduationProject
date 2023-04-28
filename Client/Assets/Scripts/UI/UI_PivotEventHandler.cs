/******
작성자 : 이우열
작성 일자 : 23.04.27

최근 수정 일자 : 23.04.28
최근 수정 내용 : pivot object 타입으로 변경
 ******/

using System;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Client
{
    /// <summary> 매개변수를 전달하는 이벤트 핸들러 </summary>
    public class UI_PivotEventHandler : MonoBehaviour, IPointerClickHandler
    {
        public Action<PointerEventData, object> OnClickHandler = null;
        public object Pivot { get; set; } = 0;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickHandler?.Invoke(eventData, Pivot);
        }
    }
}