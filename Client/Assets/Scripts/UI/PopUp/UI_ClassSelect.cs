/******
작성자 : 이우열
작성 일자 : 23.05.09

최근 수정 일자 : 23.05.09
최근 수정 내용 : 클래스 선택을 위한 UI 생성
 ******/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class UI_ClassSelect : UI_PopUp
    {
        enum Buttons
        {
            WarriorBtn,
            RiflemanBtn,
            WizardBtn,
            PriestBtn,
        }

        public override void Init()
        {
            base.Init();
            Bind<Button>(typeof(Buttons));

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
            for (int i = 0; i < 4; i++)
                GetButton(i).interactable = false;

            CTS_SelectClass classPacket = new CTS_SelectClass();
            classPacket.PlayerClass = (ushort)character;

            GameManager.Network.Send(classPacket.Write());

            GameManager.UI.ShowPopUpUI<UI_Log>().SetLog("다른 플레이어의 선택 기다리는 중");
        }
        #endregion Button

        public override void ReOpen() { }
    }
}
