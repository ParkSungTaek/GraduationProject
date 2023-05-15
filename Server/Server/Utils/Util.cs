/******
공동 작성
작성일 : 23.03.31

최근 수정 일자 : 23.04.05
최근 수정 사항 : json parsing 함수 구현
******/

namespace Server
{
    public class Util
    {
       

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// json 파일 파싱하여 오브젝트로 반환<br/>
        /// <br/>
        /// * 필요 클래스<br/>
        /// {0} - 실제 데이터 가지고 있는 클래스<br/>
        /// {0}Handler - member로 이름이 ({0}s).ToLower() 인 List&lt;{0}&gt; 필요<br/>
        ///<br/>
        /// * json 파일<br/>
        /// Assets/Resources/Jsons/{0}s.json<br/>
        /// <br/>
        /// ex)<br/>
        /// {0} = MonsterStat인 경우,<br/>
        /// <br/>
        /// class MonsterStat - 데이터 가지고 있는 클래스<br/>
        /// class MonsterStatHandler { public List&lt;MonsterStat&gt; monsterstats; }<br/>
        /// Assets/Resources/Jsons/MonsterStats.json
        /// </summary>
        /// <typeparam name="Handler">{0}Handler</typeparam>
        public static Handler ParseJson<Handler>(string path = null, string handle = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                string name = typeof(Handler).Name;
                int idx = name.IndexOf("Handler");

                path = string.Concat(name.Substring(0, idx), 's');
                handle = path.ToLower();
            }
            else if (string.IsNullOrEmpty(handle))
            {
                string name = typeof(Handler).Name;
                int idx = name.IndexOf("Handler");
                handle = string.Concat(name.Substring(0, idx), 's').ToLower();
            }

            TextAsset jsonTxt = Resources.Load<TextAsset>($"Jsons/{path}");
            if(jsonTxt == null)
            {
                Debug.LogError($"Can't load json : {path}");
                return default(Handler);
            }
            string json = jsonTxt.text;
            return JsonUtility.FromJson<Handler>($"{{\"{handle}\" : {json} }}");
        }
    }
}