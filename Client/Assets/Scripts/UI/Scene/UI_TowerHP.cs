/******
작성자 : 박성택
작성 일자 : 23.04.05

최근 수정 일자 : 23.04.05
최근 수정 내용 : sorting order 수정
 ******/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class UI_TowerHP : UI_Scene
    {
        public override void Init()
        {
            GameManager.UI.SetCanvas(gameObject, false, -1);
        }
    }
}
