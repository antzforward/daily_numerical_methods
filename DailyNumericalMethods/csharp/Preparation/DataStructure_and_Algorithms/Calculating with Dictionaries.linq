<Query Kind="Statements" />

var dict = new Dictionary<int, string>()
{
	[3] = "three",
	[10] = "ten"
};

var dict2 = new Dictionary<int, string>()
{
 { 5, "five" },
 { 10, "ten" }
};

var prices = new Dictionary<string, float>()
{
	{"ACME", 45.23f},
 	{"AAPL", 612.78f},
 	{"IBM", 205.55f},
 	{"HPQ", 37.20f},
 	{"FB", 10.75f}
};
//运行时，只是显示为<string,string>但实际结构还是对的。
Console.WriteLine( prices );
//min_price = min(zip(prices.values(), prices.keys())) 对应的是
Console.WriteLine( prices.ToDictionary(pair=>pair.Value,pair=>pair.Key).MinBy(pair=>pair.Value) );
//prices_sorted = sorted(prices.items(),key=lambda item: item[1])
var prices_sorted = prices.OrderBy(pair=>pair.Value).ToDictionary(pair=>pair.Value,pair=>pair.Key).ToArray();
Console.WriteLine( prices_sorted );
//prices_sorted = sorted(prices.items(),key=lambda item: item[1],reverse=True)
var prices_sorted_r = prices.OrderByDescending(pair => pair.Value).ToDictionary(pair=>pair.Value,pair=>pair.Key).ToArray();//默认升序，降序换个API
Console.WriteLine(prices_sorted_r);

//prices_sorted = sorted( prices.items() )
var prices_sorted2 = prices.OrderBy(pair=>pair.Key).ToArray();
Console.WriteLine( prices_sorted2 );
