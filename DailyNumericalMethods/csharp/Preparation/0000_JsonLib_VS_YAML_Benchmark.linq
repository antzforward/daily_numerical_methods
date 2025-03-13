<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <NuGetReference>YamlDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>YamlDotNet.Serialization</Namespace>
  <Namespace>YamlDotNet.Serialization.NamingConventions</Namespace>
</Query>

// 定义测试数据结构
//因为system json 比newton json速度快很多，这里就用system json 来跟yaml dotnet 比较一下性能
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
public class JsonVsYamlBenchmarks
{
	private PlayerData _testData;
	private string _serializedYaml;
	private string _serializedSystemText;
	private ISerializer _serialzier;
	private IDeserializer _deserializer;
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
		_serialzier = new SerializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.Build();
		_deserializer = new DeserializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.Build();
				
		_serializedYaml = _serialzier.Serialize(_testData);
		_serializedSystemText = System.Text.Json.JsonSerializer.Serialize(_testData);
	}

	// 测试 Newtonsoft.Json 序列化性能
	[Benchmark]
	public string Serialize_Yaml() => _serialzier.Serialize(_testData);

	// 测试 System.Text.Json 序列化性能
	[Benchmark]
	public string Serialize_SystemText() => System.Text.Json.JsonSerializer.Serialize(_testData);

	// 测试 Newtonsoft.Json 反序列化性能
	[Benchmark]
	public PlayerData Deserialize_Yaml() => _deserializer.Deserialize<PlayerData>(_serializedYaml);

	// 测试 System.Text.Json 反序列化性能
	[Benchmark]
	public PlayerData Deserialize_SystemText() => System.Text.Json.JsonSerializer.Deserialize<PlayerData>(_serializedSystemText);
}

public class Program
{
	public static void Main(string[] args)
	{
		var summary = BenchmarkRunner.Run<JsonVsYamlBenchmarks>();
	}
}