/******
공동 작성
작성 일자 : 23.04.05

최근 수정 일자 : 23.05.27
최근 수정 내용 : 아이템 등급 정보 추가
 ******/

using System;
using System.Collections.Generic;

namespace Client
{
    [Serializable]
    public class ItemDataHandler
    {
        public List<ItemData> itemdatas = new List<ItemData>();

        /// <summary> 확률에 따른 무작위 장비 반환 </summary>
        public ItemData GetRandomItem()
        {
            //확률 계산
            float randValue = UnityEngine.Random.Range(0, 1f);

            //확률 누적합
            float accumulateProb = 0;

            int idx;
            for (idx = 0; idx < itemdatas.Count - 1;idx++)
            {
                accumulateProb += itemdatas[idx].Prob;
                if (randValue < accumulateProb)
                    break;
            }

            return itemdatas[idx];
        }

        public ItemData this[int idx]
        {
            get => itemdatas[idx];
        }
    }

    [Serializable]
    public class ItemData
    {

        public ItemData()
        {
            Idx = 0;
            Name = string.Empty;
            Kind = Define.ItemKind.MaxCount;
            Rank = Define.ItemRank.Normal;
            Stat = 0;
            Prob = 0;
        }

        /// <summary> 아이템 인덱스 </summary>
        public int Idx;
        /// <summary> 아이템 이름 </summary>
        public string Name;
        /// <summary> 아이템 종류 </summary>
        public Define.ItemKind Kind;
        /// <summary> 아이템 등급 </summary>
        public Define.ItemRank Rank;
        /// <summary> 아이템으로 상승하는 수치 </summary>
        public float Stat;
        /// <summary> 아이템 등장 확률 </summary>
        public float Prob;
    }
}
