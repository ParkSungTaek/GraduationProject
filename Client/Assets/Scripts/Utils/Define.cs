using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class Define
    {

        public enum Charcter
        {
            Warrior,
            Wizard,
            Priest,
            Rifleman,
            MaxCount
        }

        public enum Sound
        {
            BGM,
            SFX,
            MaxCount
        }
        public enum BGM
        {



            MaxCount
        }
        public enum SFX
        {
            MaxCount
        }

        public enum Button
        {
            MaxCount
        }
        public enum System
        {

            MaxCount
        }
        public enum State
        {
            Play,
            End,
            Pause,
            MaxCount
        }


        public enum Item
        {
            MaxCount
        }
        public enum Tag
        {
            Monster,
            Tower,
            MaxCount
        }

        /// <summary>
        /// UI Event 종류 지정
        /// </summary>
        public enum UIEvent
        {
            Click,
            Drag,
        }

        /// <summary>
        /// Scene UI Prefab들 이름 지정
        /// </summary>
        public enum SceneUIs
        {
            TestScene,
            MaxCount
        }

        /// <summary>
        /// PopUp UI Prefab들 이름 지정
        /// </summary>
        public enum PopUpUIs
        {
            MaxCount
        }
    }
}