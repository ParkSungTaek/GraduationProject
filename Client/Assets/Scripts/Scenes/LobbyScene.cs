/******
작성자 : 이우열
작성 일자 : 23.05.08

최근 수정 일자 : 23.10.02
최근 수정 내용 : 오타 수정
 ******/

using UnityEngine;

namespace Client
{
    public class LobbyScene : MonoBehaviour
    {
        void Start()
        {
            GameManager.UI.ShowSceneUI<UI_LobbyScene>();
        }
    }
}
