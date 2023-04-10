/*
작성자 : 이우열
작성일 : 23.03.29
최근 수정 일자 : 23.04.05
최근 수정 사항 : 게임 씬 로드 시 수행할 기능 이 클래스로 이전
*/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Client
{
    public class GameScene : MonoBehaviour
    {
        private void Start()
        {
            GameManager.InGameData.StateChange(Define.State.Play);
            GameManager.UI.ShowSceneUI<UI_GameScene>();
            GameManager.GameStart();
            StartCoroutine(GameManager.InGameData.Cooldown.CooldownCoroutine());
        }
    }
}
