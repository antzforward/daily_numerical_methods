<Query Kind="Statements" />

// 这里主要涉及到计算机误差中的舍入误差 Round-off Error，由于有限精度浮点数存储时，无法精确存储所有数值导致的误差
//float,23bit 精度
{
	float a = 0.1f;
	float b = 0.2f;
	Console.WriteLine(a + b); //0.3
	Console.WriteLine(a + b == 0.3f); //True
	Console.WriteLine( (a - MathF.Floor(a))/a );
}
//double，48bit 精度
{
	double a = 0.1;
	double b = 0.2;
	Console.WriteLine(a + b); //0.30000000000000004
	Console.WriteLine(a + b == 0.3); //False
	Console.WriteLine( (a - Math.Floor(a))/a );
}

{
	decimal a = 0.1m;
	decimal b = 0.2m;
	Console.WriteLine(a + b); //0.3
	Console.WriteLine(a + b == 0.3m);//True
}

{
	var a = 1.0;
	var b = 0.9;
	var c = 0.1;
	Console.WriteLine(a - b - c );//-2.7755575615628914E-17
	Console.WriteLine(a - b - c == 0 );//False
	Console.WriteLine(Math.Abs(a - b - c )< Double.Epsilon);//False
	Console.WriteLine( Math.Abs(a - (b + c) ) < Double.Epsilon );//True
	
}
{
	//以下以常用的float来表示吧
	float Methods(float x)
	{
		return MathF.Sqrt(x + 1f) - 1f;
	}

	float Methods2(float x)
	{
		return x / (MathF.Sqrt(x + 1f) + 1f);
	}
	Console.WriteLine("origin :{0}", Methods(0.01f)); //origin :0.0049875975
	Console.WriteLine("new    :{0}", Methods2(0.01f));//new    :0.0049875616
	
	Console.WriteLine("origin :{0}", Methods(0.2f)); //origin :0.095445156
	Console.WriteLine("new    :{0}", Methods2(0.2f));//new    :0.09544511
	
	Console.WriteLine("origin :{0}",Methods(2f));
	Console.WriteLine("new    :{0}",Methods2(2f));

	Console.WriteLine("origin :{0}", Methods(100_000f));
	Console.WriteLine("new    :{0}", Methods2(100_000f));
	
	Console.WriteLine("origin :{0}", Methods(100_000_000f));
	Console.WriteLine("new    :{0}", Methods2(100_000_000f));
	
	Console.WriteLine("origin :{0}", Methods(100_000_000_000f));//316226.75
	Console.WriteLine("new    :{0}", Methods2(100_000_000_000f));//316226.78

	float x = 1e-8f; // 极小的 x 值
	float resultOriginal = Methods(x);
	float resultOptimized = Methods2(x);
	float reference = (float)(Math.Sqrt((double)x + 1) - 1); // 双精度结果为参考值

	Console.WriteLine($"原始公式: {resultOriginal} (误差: {resultOriginal - reference})");//原始公式: 0 (误差: -5E-09)
	Console.WriteLine($"优化公式: {resultOptimized} (误差: {resultOptimized - reference})");//优化公式: 5E-09 (误差: 0)
}

{
	float kahan_sum( IEnumerable<float> nums)
	{
		float total = 0f;
		float error = 0f;
		foreach( var item in nums)
		{
			float y = item - error;
			float temp = total + y;
			error = (temp - total) - y;//计算丢失的低位误差
			total = temp;
		}
		return total;
	}

	float simple_sum(IEnumerable<float> nums)
	{
		float total = 0f;
		foreach (var item in nums)
		{	
			total += item;
		}
		return total;
	}
	
	float reference_sum(IEnumerable<float> nums)
	{
		decimal total = 0;
		foreach (var item in nums)
		{
			total += new decimal(item);
		}
		return (float)total;
	}
	
	float reference = reference_sum(Enumerable.Range(1,10_000).Select(i=>1f/i));
	float kahan_value = kahan_sum(Enumerable.Range(1,10_000).Select(i=>1f/i));
	float simple_value = simple_sum(Enumerable.Range(1,10_000).Select(i=>1f/i));
	float simple_reverse_value = simple_sum(Enumerable.Range(1,10_000).Reverse().Select(i=>1f/i));
	Console.WriteLine("kahan_sum:1/n:{0:F20},误差:{1:F20}",kahan_value,reference - kahan_value);//kahan_sum:1/n:9.78760623931884765625,误差:0.00000000000000000000
	Console.WriteLine("simple_sum:1/n:{0:F20},误差:{1:F20}",simple_value,reference - simple_value);//simple_sum:1/n:9.78761291503906250000,误差:-0.00000667572021484375
	Console.WriteLine("simple_sum——reverse:1/n:{0:F20},误差:{1:F20}",simple_reverse_value,reference - simple_reverse_value);//simple_sum——reverse:1/n:9.78760433197021484375,误差:0.00000190734863281250
}


