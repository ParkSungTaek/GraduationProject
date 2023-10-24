/******
작성자 : 이우열
작성일 : 23.03.29

최근 수정 일자 : 23.05.09
최근 수정 사항 : 게임 시작 트리거 제거, GameManager로 이관
******/

using UnityEngine;

namespace Client
{
    public class GameScene : MonoBehaviour
    {
        private void Start()
        {

            Application.targetFrameRate = 60;
            GameManager.UI.ShowSceneUI<UI_GameScene>();
            GameManager.UI.ShowPopUpUI<UI_ClassSelect>();

            GameManager.InGameData.GenerateMonsterSpawnPoint();

            StartCoroutine(GameManager.InGameData.Cooldown.CooldownCoroutine());
        }
    }
}
