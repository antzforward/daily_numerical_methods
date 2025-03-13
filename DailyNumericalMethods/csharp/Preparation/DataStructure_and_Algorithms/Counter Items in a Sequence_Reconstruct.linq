<Query Kind="Program" />

void Main()
{
	var words = new List<string>{
		 "look", "into", "my", "eyes", "look", "into", "my", "eyes",
	 "the", "eyes", "the", "eyes", "the", "eyes", "not", "around", "the",
	 "eyes", "don't", "look", "around", "the", "eyes", "look", "into",
	 "my", "eyes", "you're", "under"
	};
	var counter = new Counter<string>(words);
	Console.WriteLine(counter.most_common(3).ToArray());
	
	Console.WriteLine(counter["not"]);//1
	Console.WriteLine(counter.total());//29
	var morewords = new List<string>() { "why", "are", "you", "not", "looking", "in", "my", "eyes"};
	counter.Update(morewords);
	Console.WriteLine(counter["why"]);//1
	Console.WriteLine(counter.total());//37

}

//由于dict本身不支持+ operator 这里干脆实现一些扩张的写法吧
public static class DictionaryExtensions
{
	public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
	this Dictionary<TKey, TValue> dict1,
	Dictionary<TKey, TValue> dict2,
	Func<TValue, TValue, TValue> mergeFunc
	)
	{
		var merged = new Dictionary<TKey, TValue>(dict1);
		foreach (var kv in dict2)
		{
			merged[kv.Key] = merged.ContainsKey(kv.Key)
			? mergeFunc(merged[kv.Key], kv.Value)
			: kv.Value;
		}
		return merged;
	}
	public static Dictionary<TKey, int> Counter<TKey>(
	IEnumerable<TKey> iterator
	)
	{
		var counter = new Dictionary<TKey, int>();
		foreach (var item in iterator)
		{
			int n = 0;
			counter.TryGetValue(item, out n);
			counter[item] = ++n;
		}
		return counter;
	}

	public static IEnumerable<(TKey, int)> most_common<TKey>(
		this Dictionary<TKey, int> dict,
		int top
	)
	{
		return dict.OrderByDescending(pair => pair.Value).Select(pair => (pair.Key, pair.Value)).Take(top);
	}
	public static int total<TKey>(this Dictionary<TKey, int> dict)
	{
		return dict.Values.Sum();
	}

	public static void Update<TKey>(
	this Dictionary<TKey, int> counter,
	IEnumerable<TKey> iterator
	)
	{
		foreach (var item in iterator)
		{
			int n = 0;
			counter.TryGetValue(item, out n);
			counter[item] = ++n;
		}
	}
}

public class Counter<TKey> : Dictionary<TKey, int>
{
	private  Counter(){}
	public Counter(IEnumerable<TKey> words)
	{
		foreach (var item in words)
		{
			int n = 0;
			TryGetValue(item, out n);
			this[item] = ++n;
		}
	}

	// 重载 + 操作符
	public static Counter<TKey> operator +(
		Counter<TKey> dict1,
		Counter<TKey> dict2)
	{
		var merged = new Counter<TKey>();
		foreach (var kv in dict1)
		{
			merged.Add(kv.Key, kv.Value);
		}
		foreach (var kv in dict2)
		{
			if (merged.ContainsKey(kv.Key))
			{
				merged[kv.Key] += (dynamic)kv.Value;
			}
			else
			{
				merged.Add(kv.Key, kv.Value);
			}
		}
		return merged;
	}
}
