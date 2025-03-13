<Query Kind="Statements" />



var words = new List<string>{
	 "look", "into", "my", "eyes", "look", "into", "my", "eyes",
 "the", "eyes", "the", "eyes", "the", "eyes", "not", "around", "the",
 "eyes", "don't", "look", "around", "the", "eyes", "look", "into",
 "my", "eyes", "you're", "under"
};

/*** python 代码中的Counter
from collections import Counter
word_counts = Counter(words)
top_three = word_counts.most_common(3) #[('eyes', 8), ('the', 5), ('look', 4)]
print(top_three)
*/

var counter = words.GroupBy(w => w)
.Select(group => new {Key=group.Key,Count=group.Count()})
.ToDictionary(pair=>pair.Key,pair=>pair.Count);
Console.WriteLine(counter.OrderByDescending(pair=>pair.Value).Take(3).ToArray());

//print( word_counts['not'])#1
//# 所有的总数
//print(word_counts.total())#29
Console.WriteLine(counter["not"]);//1
Console.WriteLine(counter.Values.Sum());//29

//morewords = ['why','are','you','not','looking','in','my','eyes']
//for word in morewords:#手动更新，非常不好的方法
//    word_counts[word] += 1

//这是个简单的用法
var morewords = new List<string>() { "why", "are", "you", "not", "looking", "in", "my", "eyes"};
foreach( var word in morewords)
{
	int n=0;
	counter.TryGetValue(word, out n);
	counter[word] = ++n;
}

//# 某个key的数量
//print(word_counts['why']) #1
//# 所有的总数
//print(word_counts.total())#37
Console.WriteLine(counter["why"]);//1
Console.WriteLine(counter.Values.Sum());//37

//但是没有Update的方法
//#下面的方法有个update， 这个好太多了
//word_counts.update(morewords)
//最短的代码，但是效率较低的方案,1.产生新的dict，然后Concat 两个dict
var newCount = morewords.GroupBy(w => w)
.Select(group => new { Key = group.Key, Count = group.Count() })
.ToDictionary(pair => pair.Key, pair => pair.Count);

counter = counter.Concat( newCount )
.GroupBy(kv=>kv.Key)
.ToDictionary(
g=>g.Key,
g=>g.Sum(kv=>kv.Value)
);

//# 某个key的数量
//print(word_counts['why']) #2
//# 所有的总数
//print(word_counts.total())#45
Console.WriteLine(counter["why"]);//2
Console.WriteLine(counter.Values.Sum());//45

