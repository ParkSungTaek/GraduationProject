/******
작성자 : 이우열
작성 일자 : 23.05.08

최근 수정 일자 : 23.10.22
최근 수정 내용 : 타이틀 화면 정리
 ******/

using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Client
{
    public class UI_TitleScene : UI_Scene
    {
        enum Buttons
        {
            CreateOrEnterBtn,
            RoomListBtn,
            QuickEnterBtn,
            LogoutBtn,
            QuitBtn,
        }

        public override void Init()
        {
            base.Init();
            Bind<Button>(typeof(Buttons));
            BindButton();
        }

        #region Btn
        void BindButton()
        {
            BindEvent(GetButton((int)Buttons.CreateOrEnterBtn).gameObject, CreateOrEnterBtn);
            BindEvent(GetButton((int)Buttons.RoomListBtn).gameObject, RoomListBtn);
            BindEvent(GetButton((int)Buttons.QuickEnterBtn).gameObject, QuickEnterBtn);
            BindEvent(GetButton((int)Buttons.LogoutBtn).gameObject, LogoutBtn);
            BindEvent(GetButton((int)Buttons.QuitBtn).gameObject, QuitBtn);
        }

        /// <summary> 방 생성 또는 입장 버튼 </summary>
        private void CreateOrEnterBtn(PointerEventData evt) => GameManager.UI.ShowPopUpUI<UI_CreateOrEnterRoom>();
        /// <summary> 방 목록 보기 버튼 </summary>
        private void RoomListBtn(PointerEventData evt) => GameManager.UI.ShowPopUpUI<UI_PublicRoomList>();
        /// <summary> 빠른 입장 버튼 </summary>
        private void QuickEnterBtn(PointerEventData evt)
        {
            CTS_QuickEnter pkt = new CTS_QuickEnter();
            GameManager.UI.ShowPopUpUI<UI_Log>().SetLog("방 입장 중");
            GameManager.Network.Send(pkt.Write());
        }
        /// <summary> 로그아웃 버튼 </summary>
        private void LogoutBtn(PointerEventData evt)
        {
            GameManager.UI.ShowPopUpUI<UI_SimpleSelect>().SetData("로그아웃 하시겠습니까?", AcceptLogoutBtn);

            void AcceptLogoutBtn()
            {
                GameManager.Network.Disconnect();
                SceneManager.LoadScene(Define.Scenes.Login);
            }
        }
        /// <summary> 게임 종료 버튼 </summary>
        private void QuitBtn(PointerEventData evt)
        {
            GameManager.UI.ShowPopUpUI<UI_SimpleSelect>().SetData("정말로 종료하시겠습니까?", AcceptQuitBtn);

            void AcceptQuitBtn()
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
#else
                UnityEngine.Application.Quit();
#endif
            }
        }

#endregion Btn
    }
}