/******
작성자 : 이우열
작성 일자 : 23.04.12

최근 수정 일자 : 23.04.12
최근 수정 내용 : 버프 내용 구현
******/

namespace Client
{
    public class Buff
    {
        /// <summary> 버프를 통한 상승 수치 </summary>
        public float BuffRatio { get; set; }
        /// <summary> 남은 버프 지속 시간 </summary>
        public float RemainTime { get; set; }

        public Buff(float ratio = 0, float time = 5)
        {
            BuffRatio = ratio;
            RemainTime = time;
        }
    }
}