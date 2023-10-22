/******
작성자 : 이우열
작성 일자 : 23.10.22

최근 수정 일자 : 23.10.22
최근 수정 내용 : 방 목록 팝업 클래스 생성
 ******/

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class UI_PublicRoomList : UI_PopUp
    {
        private enum Texts
        {
            EmptyText,
            PageText,
        }

        private enum Buttons
        {
            RefreshBtn,
            LeftBtn,
            RightBtn,
            BackBtn,
        }

        private enum RoomListTokens
        {
            RoomToken0,
            RoomToken1,
            RoomToken2,
            RoomToken3,
            RoomToken4,
            RoomToken5,
        }

        private int _currentPage = 0;

        public override void Init()
        {
            base.Init();
            Bind<TMP_Text>(typeof(Texts));
            Bind<Button>(typeof(Buttons));
            Bind<RoomListToken>(typeof(RoomListTokens));

            BindButton();
            BindToken();

            GameManager.ExistRoom.OnPublicRoomListUpdate -= OnRoomListUpdate;
            GameManager.ExistRoom.OnPublicRoomListUpdate += OnRoomListUpdate;
            GameManager.Network.Push(() => RefreshBtn(null));
        }

        private void BindButton()
        {
            BindEvent(GetButton((int)Buttons.RefreshBtn).gameObject, RefreshBtn);
            BindEvent(GetButton((int)Buttons.LeftBtn).gameObject, LeftBtn);
            BindEvent(GetButton((int)Buttons.RightBtn).gameObject, RightBtn);
            BindEvent(GetButton((int)Buttons.BackBtn).gameObject, (evt) => ClosePopUpUI());
        }

        private void BindToken()
        {
            int count = System.Enum.GetNames(typeof(RoomListTokens)).Length;
            for (int i = 0; i < count; i++)
            {
                var token = Get<RoomListToken>(i);
                BindEvent(token.gameObject, token.OnClick);
            }
        }

        /// <summary> 서버에 방 목록 요청 버튼 </summary>
        private void RefreshBtn(PointerEventData evt)
        {
            Get<TMP_Text>((int)Texts.EmptyText).gameObject.SetActive(false);
            for (int idx = (int)RoomListTokens.RoomToken0; idx <= (int)RoomListTokens.RoomToken5; idx++)
            {
                Get<RoomListToken>(idx).gameObject.SetActive(false);
            }
            _currentPage = 0;

            CTS_GetPublicRoomList getExistRoomPacket = new CTS_GetPublicRoomList();

            GameManager.UI.ShowPopUpUI<UI_Log>().SetLog("방 목록 불러오는 중");
            GameManager.Network.Send(getExistRoomPacket.Write());
        }

        /// <summary> 서버에서 방 목록 받았을 때 새로고침 </summary>
        private void OnRoomListUpdate()
        {
            GameManager.UI.ClosePopUpUI(typeof(UI_Log));
            UpdateRoomNameToken();
        }
        private void UpdateRoomNameToken()
        {
            if (GameManager.ExistRoom.RoomList == null)
            {
                return;
            }

            int startIdx = _currentPage * 6;
            int maxIdx = Mathf.Min(startIdx + 6, GameManager.ExistRoom.RoomList.Count);

            int iter = 0;
            for (iter = startIdx; iter < maxIdx; iter++)
            {
                var token = Get<RoomListToken>(iter - startIdx);
                token.Init(iter);
                token.gameObject.SetActive(true);
            }

            for (; iter < startIdx + 6; iter++)
            {
                Get<RoomListToken>(iter - startIdx).gameObject.SetActive(false);
            }

            int maxPage = (GameManager.ExistRoom.RoomList.Count - 1) / 6;
            GetText((int)Texts.EmptyText).gameObject.SetActive(GameManager.ExistRoom.RoomList.Count <= 0);
            GetButton((int)Buttons.LeftBtn).gameObject.SetActive(_currentPage > 0);
            GetButton((int)Buttons.RightBtn).gameObject.SetActive(_currentPage < maxPage);
            GetText((int)Texts.PageText).SetText($"{_currentPage + 1} / {maxPage + 1}");
        }

        /// <summary> 왼쪽 페이지로 전환 버튼 </summary>
        private void LeftBtn(PointerEventData evt)
        {
            if (_currentPage <= 0)
            {
                return;
            }

            _currentPage--;
            UpdateRoomNameToken();
        }

        /// <summary> 오른쪽 페이지로 전환 버튼 </summary>
        private void RightBtn(PointerEventData evt)
        {
            if (GameManager.ExistRoom.RoomList == null)
            {
                return;
            }

            int maxPage = (GameManager.ExistRoom.RoomList.Count - 1) / 6;
            if (_currentPage >= maxPage)
            {
                return;
            }

            _currentPage++;
            UpdateRoomNameToken();
        }

        public override void ReOpen() => GameManager.Network.Push(() => RefreshBtn(null));
    }
}