/******
작성자 : 이우열
작성 일자 : 23.10.22

최근 수정 일자 : 23.10.22
최근 수정 내용 : 방 리스트 토큰 클래스 생성
 ******/

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client
{
    public class RoomListToken : MonoBehaviour
    {
        public int RoomNameIndex { get; set; }
        public TMP_Text RoomNameText;

        public void Init(int idx)
        {
            RoomNameIndex = idx;
            RoomNameText.text = GameManager.ExistRoom.RoomList[idx];
        }

        public void OnClick(PointerEventData evt)
        {
            if (null == GameManager.ExistRoom.RoomList)
            {
                return;
            }

            if (RoomNameIndex < 0 || RoomNameIndex >= GameManager.ExistRoom.RoomList.Count)
            {
                return;
            }

            CTS_EnterRoom enterPacket = new CTS_EnterRoom();
            enterPacket.roomName = GameManager.ExistRoom.RoomList[RoomNameIndex];

            GameManager.UI.ShowPopUpUI<UI_Log>().SetLog("방 입장 중");
            GameManager.Network.Send(enterPacket.Write());
        }
    }
}