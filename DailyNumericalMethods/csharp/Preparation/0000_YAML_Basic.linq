<Query Kind="Program">
  <NuGetReference>YamlDotNet</NuGetReference>
  <Namespace>YamlDotNet.Serialization</Namespace>
  <Namespace>YamlDotNet.Serialization.NamingConventions</Namespace>
</Query>

public class Person
{
	public string Name { get; set; }
	public int Age { get; set; }
	public override bool Equals( object obj)
	{
		if (obj is Person p)
		{
			return Age == p.Age && Name == p.Name;
		}
		return false;
	}
	public static bool operator ==( Person p1, Person p2)
	{
		return p1.Equals( p2 );
	}
	public static bool operator !=(Person p1, Person p2)
	{
		return !(p1 == p2);
	}
	public override int GetHashCode()
	{
		return HashCode.Combine(Name, Age);
	}
}

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
	public override bool Equals(object obj)
	{
		if (obj is Vector3 p)
		{
			return X == p.X && Y == p.Y && Z == p.Z;
		}
		return false;
	}
	public static bool operator ==(Vector3 p1, Vector3 p2)
	{
		return p1.Equals(p2);
	}
	public static bool operator !=(Vector3 p1, Vector3 p2)
	{
		return !(p1 == p2);
	}
	public override int GetHashCode()
	{
		return HashCode.Combine<float,float,float>(X, Y,Z);
	}
}


public class Program
{
	public static void Main()
	{
		{
			Person person = new Person { Name = "John Doe", Age = 30 };

			var serializer = new SerializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.Build();

			string yaml = serializer.Serialize(person);
			Console.Write(yaml);
			var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
			Person other = deserializer.Deserialize<Person>(yaml);
			Console.WriteLine(person == other);
		}
		{
			var _testData = new PlayerData
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
			var serializer = new SerializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.Build();

			string yaml = serializer.Serialize(_testData);
			Console.Write(yaml);
			var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
			PlayerData other = deserializer.Deserialize<PlayerData>(yaml);
			Console.Write(serializer.Serialize(other));
		}


	}
}