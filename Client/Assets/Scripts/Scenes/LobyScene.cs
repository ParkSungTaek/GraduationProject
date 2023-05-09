/******
작성자 : 이우열
작성 일자 : 23.05.08

최근 수정 일자 : 23.05.08
최근 수정 내용 : RobyScene 생성
 ******/

using UnityEngine;

namespace Client
{
    public class LobyScene : MonoBehaviour
    {
        void Start()
        {
            GameManager.UI.ShowSceneUI<UI_LobyScene>();
            //ui update 용
            GameManager.Room.EnterPlayer(GameManager.Network.PlayerId);
        }
    }
}
