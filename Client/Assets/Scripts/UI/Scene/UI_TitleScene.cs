/******
작성자 : 이우열
작성 일자 : 23.05.08

최근 수정 일자 : 23.10.02
최근 수정 내용 : 로그아웃 버튼 추가
 ******/

using Assets.HeroEditor4D.Common.Scripts.Common;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class UI_TitleScene : UI_Scene
    {
        bool BeforeInit = false;
        const int ExistRoomsButtons = 6;
        #region Entity
        /// <summary> 버튼 목록 </summary>

        enum Buttons
        {
            QuickEnter,
            CreateBtn,
            EnterBtn,
            LogoutBtn,
            QuitBtn,

            Room0Btn,
            Room1Btn,
            Room2Btn,
            Room3Btn,
            Room4Btn,
            Room5Btn,

            Right,
            Left,
            Refresh,

        }

        enum InputFields
        {
            RoomInput,
        }
        enum Texts 
        {
            Room0Name,
            Room1Name,
            Room2Name, 
            Room3Name,
            Room4Name,
            Room5Name,
        }
        #endregion Entity

        #region 방 리스트 처리
        /// <summary>
        /// 방 리스트
        /// </summary>
        List<string> _roomsNames { get; set; }

        /// <summary>
        /// 0장 = 0~5, 1장 6~11
        /// </summary>
        int _listPage { get; set; } = 0;
        public void SetExistRoomsName()
        {

            if (!BeforeInit)
            {
                Init();
            }
            if (GameManager.InGameData.RoomsNames != null )
            {
                _roomsNames = GameManager.InGameData.RoomsNames;
                ShowListIdx(_listPage);
            }

        }

        void ShowListIdx(int listPage)
        {
            for (int i = listPage * 6; i < Mathf.Min(_roomsNames.Count, (listPage + 1) * 6); i++)
            {
                Debug.Log($"roomsName {i} : {_roomsNames[i]}");
                GetText(i - (listPage * 6)).text = _roomsNames[i];
            }
            
            for (int i = _roomsNames.Count; i < (listPage + 1) * 6; i++)
            {
                GetText(i - (listPage * 6)).text = "Empty";
            }

            BlockLeftOrRight(_listPage);
        }


        #endregion 방 리스트 처리;

        /// <summary> 바인드 </summary>
        public override void Init()
        {
            if (!BeforeInit)
            {
                BeforeInit = true;
                base.Init();
                Bind<Button>(typeof(Buttons));
                Bind<TMP_InputField>(typeof(InputFields));
                Bind<TMP_Text>(typeof(Texts));
                Debug.Log("BindText");

                BindButton();

            }
        }


        #region Btn
        void BindButton()
        {
            BindEvent(GetButton((int)Buttons.QuickEnter).gameObject, QuickEnterBtn);
            BindEvent(GetButton((int)Buttons.CreateBtn).gameObject, CreateRoomBtn);
            BindEvent(GetButton((int)Buttons.EnterBtn).gameObject, EnterRoomBtn);
            BindEvent(GetButton((int)Buttons.LogoutBtn).gameObject, LogoutBtn);
            BindEvent(GetButton((int)Buttons.QuitBtn).gameObject, QuitBtn);

            BindEvent(GetButton((int)Buttons.Room0Btn).gameObject, ExistRoomEnterBtn);
            BindEvent(GetButton((int)Buttons.Room1Btn).gameObject, ExistRoomEnterBtn);
            BindEvent(GetButton((int)Buttons.Room2Btn).gameObject, ExistRoomEnterBtn);
            BindEvent(GetButton((int)Buttons.Room3Btn).gameObject, ExistRoomEnterBtn);
            BindEvent(GetButton((int)Buttons.Room4Btn).gameObject, ExistRoomEnterBtn);
            BindEvent(GetButton((int)Buttons.Room5Btn).gameObject, ExistRoomEnterBtn);

            BindEvent(GetButton((int)Buttons.Right).gameObject, RightListBtn);
            BindEvent(GetButton((int)Buttons.Left).gameObject, LeftListBtn);
            BindEvent(GetButton((int)Buttons.Refresh).gameObject, RefreshListBtn);



        }
        /// <summary> 방 생성 버튼 </summary>
        void CreateRoomBtn(PointerEventData evt)
        {
            string roomName = Get<TMP_InputField>((int)InputFields.RoomInput).text;

            if(string.IsNullOrEmpty(roomName))
            {
                GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("방 이름을 입력하세요.");
                return;
            }

            CTS_CreateRoom pkt = new CTS_CreateRoom();
            pkt.roomName = roomName;
            GameManager.Network.Send(pkt.Write());
            GameManager.UI.ShowPopUpUI<UI_Log>().SetLog("방 생성 중");
        }

        /// <summary> 방 입장 버튼 </summary>
        void EnterRoomBtn(PointerEventData evt)
        {
            string roomName = Get<TMP_InputField>((int)InputFields.RoomInput).text;

            if (string.IsNullOrEmpty(roomName))
            {
                GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("방 이름을 입력하세요.");
                return;
            }

            CTS_EnterRoom pkt = new CTS_EnterRoom();
            pkt.roomName = roomName;

            GameManager.Network.Send(pkt.Write());
            GameManager.UI.ShowPopUpUI<UI_Log>().SetLog("방 입장 중");


        }
        /// <summary>
        /// 방 빠른 입장
        /// </summary>
        void ExistRoomEnterBtn(PointerEventData evt)
        {
            string roomName = "";
            for (int i=0;i< ExistRoomsButtons; i++)
            {
                if(evt.selectedObject == GetButton((int)Buttons.Room1Btn + i).gameObject)
                {
                    roomName = GetText((int)Texts.Room1Name + i).text;
                    break;
                }
            }
            //string roomName = Get<TMP_InputField>((int)InputFields.RoomInput).text;

            if (string.IsNullOrEmpty(roomName))
            {
                GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("방 이름을 입력하세요.");
                return;
            }

            CTS_EnterRoom pkt = new CTS_EnterRoom();
            pkt.roomName = roomName;

            GameManager.Network.Send(pkt.Write());
            GameManager.UI.ShowPopUpUI<UI_Log>().SetLog("방 입장 중");
        }
        /// <summary> 빠른 입장 버튼 </summary>
        void QuickEnterBtn(PointerEventData evt)
        {

            CTS_QuickEnterRoom pkt = new CTS_QuickEnterRoom();
            Debug.Log("Enter");
            GameManager.Network.Send(pkt.Write());
        }
        /// <summary> 로그아웃 버튼 </summary>
        void LogoutBtn(PointerEventData evt)
        {
            GameManager.UI.ShowPopUpUI<UI_SimpleSelect>().SetData("로그아웃 하시겠습니까?", AcceptLogoutBtn);
        }
        void QuitBtn(PointerEventData evt)
        {
            GameManager.UI.ShowPopUpUI<UI_SimpleSelect>().SetData("정말로 종료하시겠습니까?", AcceptQuitBtn);
        }

        /// <summary> 방 리스트 오른쪽 버튼</summary>

        void RightListBtn(PointerEventData evt)
        {
            _listPage++;
            ShowListIdx(_listPage);
            

        }
        /// <summary> 방 리스트 왼쪽 버튼</summary>
        void LeftListBtn(PointerEventData evt)
        {
            if (_listPage <= 0) return;
            _listPage--;
            ShowListIdx(_listPage);
        }

        /// <summary> 왼,오른쪽 버튼 ON/OFF </summary>
        void BlockLeftOrRight(int listPage)
        {
            if(listPage <= 0)
            {
                GetButton((int)Buttons.Left).SetActive(false);
            }
            else
            {
                GetButton((int)Buttons.Left).SetActive(true);
            }
            
            if(_roomsNames.Count <= (listPage + 1) * 6)
            {
                GetButton((int)Buttons.Right).SetActive(false);
            }
            else
            {
                GetButton((int)Buttons.Right).SetActive(true);
            }
        }

        /// <summary> 방 리스트 새로고침 버튼 </summary>
        void RefreshListBtn(PointerEventData evt)
        {
            CTS_GetExistRooms pkt = new CTS_GetExistRooms();
            GameManager.Network.Send(pkt.Write());
        }

        void AcceptLogoutBtn()
        {
            GameManager.Network.Disconnect();
            SceneManager.LoadScene(Define.Scenes.Login);
        }
        void AcceptQuitBtn()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

       

#endregion Btn
    }
}