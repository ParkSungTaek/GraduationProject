/******
공동 작성
작성 일자 : 23.04.05

최근 수정 일자 : 23.04.12
최근 수정 내용 : json 파싱 클래스로 변경
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
            float accumulateProb = itemdatas[0].Prob;

            int idx;
            for (idx = 1; accumulateProb < randValue && idx < itemdatas.Count - 1; idx++)
            {
                accumulateProb += itemdatas[idx].Prob;
            }

            return itemdatas[idx];
        }
    }

    [Serializable]
    public class ItemData
    {
        /// <summary> 아이템 이름 </summary>
        public string Name;
        /// <summary> 아이템 종류 </summary>
        public Define.ItemKind Kind;
        /// <summary> 아이템으로 상승하는 수치 </summary>
        public float Stat;
        /// <summary> 아이템 등장 확률 </summary>
        public float Prob;
    }
}
