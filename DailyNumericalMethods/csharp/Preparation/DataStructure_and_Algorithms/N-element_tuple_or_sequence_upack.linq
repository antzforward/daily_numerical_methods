<Query Kind="Statements" />

//在c#中还是不要追求List的Deconstruct了，没必要，效率低
var p=(4,5);
var (x, y) = p;
Console.WriteLine($"{x}, {y}");

{
	var data = ("ACME", 50, 91.1, (2012, 12, 21));//c# 里面不好这么解析List 还只能是tuple
	var (name, shares, price, date) = data;
	Console.WriteLine($"{name}, {shares},{price},{date}");
	var (year, mon, day) = date;
	Console.WriteLine($"{year}, {mon},{day}");
}


{
	var data = ("ACME", 50, 91.1, (2012, 12, 21));//c# 里面不好这么解析List 还只能是tuple
	var (name, shares, price, (year, mon, day)) = data;
	Console.WriteLine($"{name}, {shares},{price},({year}, {mon},{day})");
}

{
	var data = ("ACME", 50, 91.1, (2012, 12, 21));//c# 里面不好这么解析List 还只能是tuple
	var (_, shares, price, _) = data;
	Console.WriteLine($"{shares},{price}");
	var (name,_,_,(year, _,day)) = data;
	Console.WriteLine($"{name}, {shares},{price},({year},{day})");
}

