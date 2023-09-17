/******
작성자 : 박성택
작성 일자 : 23.09.17

최근 수정 일자 : 23.09.17
최근 수정 내용 : UI_LoginScene 클래스 생성
 ******/


using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class UI_LoginScene : UI_Scene
    {
        enum Buttons
        {
            EnterBtn,
        }

        enum InputFields
        {
            ID_Input
        }
        public override void Init()
        {
            base.Init();
            Bind<Button>(typeof(Buttons));
            Bind<TMP_InputField>(typeof(InputFields));
            ButtonBind();

        }

        #region Btn
        /// <summary>
        /// BindEvent 함수 정리
        /// </summary>
        void ButtonBind()
        {
            BindEvent(GetButton((int)Buttons.EnterBtn).gameObject, EnterTitleBtn);
        }
        /// <summary>
        /// Title 입장 버튼 ID 적용 
        /// </summary>
        void EnterTitleBtn(PointerEventData evt)
        {

        }


        #endregion

    }
}
