/******
작성자 : 이우열
작성일 : 23.03.29

최근 수정 일자 : 23.04.05
최근 수정 사항 : 타이틀 씬 제작
******/

using UnityEngine;

namespace Client
{
    public class TitleScene : MonoBehaviour
    {
        void Start()
        {

            GameManager.UI.ShowSceneUI<UI_TitleScene>();


        }
    }
}
