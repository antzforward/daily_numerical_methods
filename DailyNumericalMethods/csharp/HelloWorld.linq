<Query Kind="Statements">
  <NuGetReference>SkiaSharp</NuGetReference>
  <NuGetReference>System.Runtime.Numerics</NuGetReference>
</Query>

// Program.cs
using System;

Console.WriteLine("Hello, C#!");

// 定义一个简单的函数
void PrintMessage(string message)
{
	Console.WriteLine(message);
}

// 调用函数
PrintMessage("This is a single-file C# program.");

int a = 10000;
int b = (a >> 3) + (a < 9 ? 3 : 6);
Console.WriteLine($"{a} :{b}");