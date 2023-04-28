/******
작성자 : 이우열
작성일 : 23.03.31

최근 수정 일자 : 23.03.31
최근 수정 사항 : 기본 UI 시스템 구현
******/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public abstract class UI_Scene : UI_Base
    {
        public override void Init()
        {
            GameManager.UI.SetCanvas(gameObject, false);
        }
    }
}
