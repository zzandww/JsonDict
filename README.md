# JsonDict
A very simple. net JSON parser that parses JSON content into a dictionary  Support. net native AOT  Support using Path to access JSON content

一个非常简单的.net JSON解析器, 把JSON内容解析到字典.

支持.net8 native AOT

支持使用Path来访问JSON内容

```C#
string jsonString = "{\"name\":\"John\",\"age\":30,\"city\":\"New York\",\"grades\":[90, 85, 92]}";

var jo = JsonDict.Parse(jsonString);

// 使用路径访问
Console.WriteLine("使用路径访问:");
Console.WriteLine($"Name: {jo.GetValue("name")}");
Console.WriteLine($"City: {jo.GetValue("city")}");
Console.WriteLine($"First Grade: {jo.GetValue("grades.0")}");

```

## 复杂JSON访问
```C#
string jsonString3 = @"{
    ""glossary"": {
        ""title"": ""example glossary"",
		""GlossDiv"": {
            ""title"": ""S"",
			""GlossList"": {
                ""GlossEntry"": {
                    ""ID"": ""SGML"",
					""SortAs"": ""SGML"",
					""GlossTerm"": ""Standard Generalized Markup Language"",
					""Acronym"": ""SGML"",
					""Abbrev"": ""ISO 8879:1986"",
					""GlossDef"": {
                        ""para"": ""A meta-markup language, used to create markup languages such as DocBook."",
						""GlossSeeAlso"": [""GML"", ""XML""]
                    },
					""GlossSee"": ""markup""
                }
            }
        }
    }
}";
var jo3 = JsonDict.Parse(jsonString3);
Console.WriteLine(jo3.GetValue("glossary.title"));
Console.WriteLine(jo3.GetValue("glossary.GlossDiv.GlossList.GlossEntry.GlossDef.para"));
Console.WriteLine(jo3.GetValue("glossary.GlossDiv.GlossList.GlossEntry.GlossDef.GlossSeeAlso.0"));
```

## 字典和数组的访问
```C#
var arr = jo3.GetValue("glossary.GlossDiv.GlossList.GlossEntry.GlossDef.GlossSeeAlso").Array;
foreach (var item in arr)
{
    Console.WriteLine(item);
}
foreach (var item in jo3.Keys)
{
    Console.WriteLine($"{item}\t\t{jo3.GetValue(item)}");
}
```