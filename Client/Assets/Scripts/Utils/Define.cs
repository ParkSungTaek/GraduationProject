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
            Rifleman,
            Wizard,
            Priest,
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
        public enum State
        {
            Play,
            End,
            Pause,
            MaxCount
        }


        public enum Item
        {
            제출을_깜빡한_과제,
            칠십칠중_순환참조_Class,
            마감_하루전날_날아가버린_DB,
            울리지_않은_자명종,
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
            DragEnd,
        }

        public enum Scenes
        { 
            Title,
            Game,
        }

        public enum MonsterName
        {
            Bat,
            BlackBoar,
            BlackBear,
            BlackWolf,
            CaveRat,
            Cerberus,
            Crawler,
            CrystalLizard,
            DreadEye,
            MinerBoar,
            Nightmare,
            Porcupine,
            PurpleScarab,
            Scarab,
            ShardLizard,
            SlugQueen,
            Warg,
            MaxCount
        }

        public enum MonsterState
        {
            Idle,
            Attack,
            Run,
            Walk,
            Death,
            MaxCount
        }

        
    }
}