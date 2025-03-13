<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Text.Json</Namespace>
</Query>

// 定义测试数据结构
public class PlayerData
{
	public int Id { get; set; }
	public string Name { get; set; }
	public List<Vector3> Positions { get; set; }
	public Dictionary<string, int> Inventory { get; set; }
}

public class Vector3
{
	public float X { get; set; }
	public float Y { get; set; }
	public float Z { get; set; }
}

[MemoryDiagnoser] // 分析内存分配
public class JsonBenchmarks
{
	private PlayerData _testData;
	private string _serializedNewtonsoft;
	private string _serializedSystemText;

	[GlobalSetup]
	public void Setup()
	{
		// 生成测试数据
		_testData = new PlayerData
		{
			Id = 1,
			Name = "Player1",
			Positions = new List<Vector3>
			{
				new Vector3 { X = 1.0f, Y = 2.0f, Z = 3.0f },
				new Vector3 { X = 4.0f, Y = 5.0f, Z = 6.0f }
			},
			Inventory = new Dictionary<string, int>
			{
				{ "Sword", 1 },
				{ "Shield", 2 }
			}
		};

		// 预序列化数据
		_serializedNewtonsoft = JsonConvert.SerializeObject(_testData);
		_serializedSystemText = System.Text.Json.JsonSerializer.Serialize(_testData);
	}

	// 测试 Newtonsoft.Json 序列化性能
	[Benchmark]
	public string Serialize_Newtonsoft() => JsonConvert.SerializeObject(_testData);

	// 测试 System.Text.Json 序列化性能
	[Benchmark]
	public string Serialize_SystemText() => System.Text.Json.JsonSerializer.Serialize(_testData);

	// 测试 Newtonsoft.Json 反序列化性能
	[Benchmark]
	public PlayerData Deserialize_Newtonsoft() => JsonConvert.DeserializeObject<PlayerData>(_serializedNewtonsoft);

	// 测试 System.Text.Json 反序列化性能
	[Benchmark]
	public PlayerData Deserialize_SystemText() => System.Text.Json.JsonSerializer.Deserialize<PlayerData>(_serializedSystemText);
}

public class Program
{
	public static void Main(string[] args)
	{
		var summary = BenchmarkRunner.Run<JsonBenchmarks>();
	}
}