using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
            PlayerPrefs.SetInt("Class", (int)character);
            SceneManager.LoadScene(Define.Scenes.Game);
        }
        #endregion Button

    }
}
