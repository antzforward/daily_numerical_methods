<Query Kind="Statements" />

var colors = new List<string> { "black", "Blue" };
var sizes = new List<string> {"S","M"};

var result1 = 
	from color in colors
	from size  in sizes
	select (color, size);
	
var result2 = colors.SelectMany(
	_ => sizes,
	(color, size) => (color, size)
);

// 输出结果
Console.WriteLine("Method 1:");
foreach (var item in result1)
	Console.WriteLine($"{item.color}-{item.size}");

Console.WriteLine("\nMethod 2:");
foreach (var item in result2)
	Console.WriteLine($"{item.color}-{item.size}");

int a1 = 2, a2 = 1,a3 = 3;
int y1 = 2, y2 = 1,y3 = 1;
int M1 = 5 * 7,M2 = 3 * 7,M3 = 3 * 5,M = 3 * 5 * 7;
Console.WriteLine((a1*y1*M1+a2*y2*M2+a3*y3*M3)%M);