/******
작성자 : 박성택
작성 일자 : 23.05.06

최근 수정 일자 : 23.05.06
최근 수정 내용 : 클래스 생성
 ******/

namespace Server
{
    public struct MonsterControlInfo
    {
        /// <summary> 다음번 몬스터 ID</summary>
        ushort _nextMosterID;
        /// <summary> 이번 라운드 몬스터 나온 숫자 </summary>
        ushort _thistypeCount;

        /// <summary> 몬스터 Type </summary>
        ushort _monsterTypeNum;

        public ushort NextMosterID { get { return _nextMosterID; }  set { _nextMosterID = value; } }
        public ushort MonsterTypeNum { get { return _monsterTypeNum; } set { _monsterTypeNum = value; } }
        public float MonsterToMonster { get { return (26.0f / (16.0f + _monsterTypeNum)); } }
        public float WaveToWave { get { return 8f; } }


        public bool NextWave { get; set; }
        /// <summary> 이번 라운드 몬스터 나와야 할 수량 </summary>
        int _wavenum { get { return _monsterTypeNum % 10 != 9 ? (4 * (_monsterTypeNum / 10) + 2 * (_monsterTypeNum % 10) + 8) : 1; } }

        public void MonsterTypePlus()
        {
            if(_thistypeCount > _wavenum)
            {
                _monsterTypeNum++;
                NextWave = true;
                _thistypeCount = 0;
            }
           
        }

        /// <summary> 이번 라운드 몬스터 나온 숫자 </summary>
        public ushort ThistypeCount { 
            get { return _thistypeCount; }
            set { 
                _thistypeCount = value;
            } 
        }
        public MonsterControlInfo() 
        {
            _nextMosterID = 0;
            _thistypeCount = 0;
            _monsterTypeNum = 0;
            NextWave = false;  
        }

    }
}
