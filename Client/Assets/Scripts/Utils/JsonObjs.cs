using System;
using System.Collections.Generic;

namespace Client
{
    [Serializable]
    public class TestjsonHandler
    {
        public List<Testjson> testjsons = new List<Testjson>();
    }

    [Serializable]
    public class Testjson
    {
        public int idx;
        public string name;
    }
}
