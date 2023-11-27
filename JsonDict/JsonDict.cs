using System;
using System.Collections.Generic;
namespace JsonDict
{


    public class JsonDict
    {
        public static JsonObject Parse(string json) {   
            var parser = new JsonParser(json);
            var obj =  parser.Parse();
            return new JsonObject(obj);
        }

    }

}