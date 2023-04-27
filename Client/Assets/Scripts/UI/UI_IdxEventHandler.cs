/******
작성자 : 이우열
작성 일자 : 23.04.27

최근 수정 일자 : 23.04.27
최근 수정 내용 : 클래스 생성
 ******/

using System;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Client
{
    public class UI_IdxEventHandler : MonoBehaviour, IPointerClickHandler
    {
        public Action<PointerEventData, int> OnClickHandler = null;
        public int Idx { get; set; } = 0;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickHandler?.Invoke(eventData, Idx);
        }
    }
}