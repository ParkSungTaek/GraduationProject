/******
작성자 : 이우열
작성 일자 : 23.10.01

최근 수정 일자 : 23.10.01
최근 수정 내용 : 로그인 씬 클래스 추가
 ******/

using UnityEngine;

namespace Client
{
    public class LoginScene : MonoBehaviour
    {
        void Start()
        {
            GameManager.UI.ShowSceneUI<UI_LoginScene>();
        }
    }
}
