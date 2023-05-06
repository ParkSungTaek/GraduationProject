/******
작성자 : 이우열
작성일 : 23.04.05

최근 수정 일자 : 23.04.05
최근 수정 사항 : 타이틀 씬 구현
******/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Client
{
    public class UI_TitleScene : UI_Scene
    {
        enum Buttons
        {
            WarriorBtn,
            RiflemanBtn,
            WizardBtn,
            PriestBtn,
        }
        enum InputTexts
        {
            RoomName,
        }
        public override void Init()
        {
            base.Init();
            Bind<Button>(typeof(Buttons));
            Bind<TMP_InputField>(typeof(InputTexts));
            
            ButtonBind();
        }

        #region Button
        void ButtonBind()
        {
            BindEvent(Get<Button>((int)Buttons.WarriorBtn).gameObject, SelectWarrior);
            BindEvent(Get<Button>((int)Buttons.RiflemanBtn).gameObject, SelectRifleman);
            BindEvent(Get<Button>((int)Buttons.WizardBtn).gameObject, SelectWizard);
            BindEvent(Get<Button>((int)Buttons.PriestBtn).gameObject, SelectPriest);
        }

        void SelectWarrior(PointerEventData evt) => SelectClass(Define.Charcter.Warrior);
        void SelectRifleman(PointerEventData evt) => SelectClass(Define.Charcter.Rifleman);
        void SelectWizard(PointerEventData evt) => SelectClass(Define.Charcter.Wizard);
        void SelectPriest(PointerEventData evt) => SelectClass(Define.Charcter.Priest);

        void SelectClass(Define.Charcter character)
        {
            PlayerPrefs.SetInt("Class", (int)character);
            SceneManager.LoadScene(Define.Scenes.Game);
            CTS_CreateRoom _createRoom = new CTS_CreateRoom();
            _createRoom.roomName = Get<TMP_InputField>((int)InputTexts.RoomName).text;
            GameManager.Network.Send(_createRoom.Write());
            CTS_StartGameRoom _startGameRoom = new CTS_StartGameRoom();
            GameManager.Network.Send(_startGameRoom.Write());

        }
        #endregion Button

    }
}
