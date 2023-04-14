/******
공동 작성
작성일 : 23.03.31

최근 수정 일자 : 23.04.13
최근 수정 내용 : 아이템 종류 enum 추가
 ******/

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
            Idle,
            Play,
            Defeat,
            Win,
            MaxCount
        }

        /// <summary> 아이템 종류 </summary>
        public enum ItemKind
        {
            Damage,
            Range,
            Cooldown,
            Weight,
            Speed,
            Slow,
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