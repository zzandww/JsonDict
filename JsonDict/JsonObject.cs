namespace JsonDict
{
    public class JsonObject
    { 

        public JsonObject(object obj)
        {
            this.Value = obj;
        }
        public string Path { get; set; }

        public object Value { get; set; }
        /// <summary>
        /// 获取指定路径的值
        /// </summary>
        /// <param name="path">路径字符串，用点号分隔</param>
        /// <returns>路径对应的值</returns>
        public JsonObject GetValue(string path)
        { 
            object result = Value;
            string[] pathSegments = path.Split('.');

            foreach (var segment in pathSegments)
            {
                if (result is Dictionary<string, object> dictionary && dictionary.ContainsKey(segment))
                {
                    result = dictionary[segment];
                }
                else if (result is List<object> list && int.TryParse(segment, out int index) && index >= 0 && index < list.Count)
                {
                    result = list[index];
                }
                else
                {
                    // 路径未找到
                    return new JsonObject(null);
                }
            }
            return new JsonObject(result); ;
        }

        public IEnumerable<JsonObject> Array
        {
            get
            {
                if(Value is List<object> list)
                {
                    foreach (var item in list)
                    {
                        yield return new JsonObject(item);
                    }
                    yield break;
                }
                else
                {
                     
                }
            }
        }

        public IEnumerable<string> Keys
        {
            get
            {
                if (Value is Dictionary<string, object> dictionary)
                {
                    foreach (var item in dictionary.Keys)
                    {
                        yield return item;
                    } 
                } 
            }
        }

        public override string ToString()
        {
            return Value?.ToString();
        }


        public void TraverseObject()
        {
            TraverseObject(this.Value, "");
        }




        private void TraverseObject(object obj, string currentPath = "")
        {
            if (obj is Dictionary<string, object> dictionary)
            {
                foreach (var kvp in dictionary)
                {
                    string newPath = currentPath + (currentPath == "" ? "" : ".") + kvp.Key;
                    Console.WriteLine($"{newPath}: {kvp.Value}");
                    TraverseObject(kvp.Value, newPath);
                }
            }
            else if (obj is List<object> list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    string newPath = currentPath + (currentPath == "" ? "" : ".") + i;
                    Console.WriteLine($"{newPath}: {list[i]}");
                    TraverseObject(list[i], newPath);
                }
            }
            // Leaf nodes (scalar values) will not be traversed further
            // Console.WriteLine(obj);
        }
    }

}