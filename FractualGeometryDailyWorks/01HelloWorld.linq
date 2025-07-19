<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <NuGetReference>SkiaSharp</NuGetReference>
  <NuGetReference>System.Linq.Async</NuGetReference>
  <NuGetReference>System.Runtime.Numerics</NuGetReference>
  <Namespace>SkiaSharp</Namespace>
  <Namespace>SkiaSharp.Internals</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Windows.Controls</Namespace>
</Query>

// 按 F4 添加 NuGet 包：
//   - SkiaSharp (用于绘图)
//   - System.Numerics (复数计算)

void Main()
{
	// 1. 生成分形图像（以Koch雪花为例）
	SKBitmap mandelbrot = GenerateMandelbrot();

	// 2. 在LINQPad中预览
	//mandelbrot.Dump("Koch雪花预览");

	// 3. 保存到当前脚本所在目录
	SaveToCurrentDirectory(mandelbrot, "Mandelbrot.png");
}

void SaveToCurrentDirectory(SKBitmap bitmap, string fileName, SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
{
	// 获取当前LINQ脚本的目录路径

	string scriptPath = Util.CurrentQueryPath;
	string filePath = null;
	if (string.IsNullOrEmpty(scriptPath))
	{
		// 如果脚本未保存，使用文档目录
		string docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		filePath = Path.Combine(docsPath, fileName);
		SaveBitmap(bitmap, filePath, format, quality);
		$"脚本未保存，图片已保存到文档目录: {filePath}".Dump();
		return;
	}

	// 获取脚本所在目录
	string scriptDir = Path.GetDirectoryName(scriptPath);
	filePath = Path.Combine(scriptDir, fileName);

	// 保存图片
	SaveBitmap(bitmap, filePath, format, quality);

	// 显示成功消息和文件路径
	$"图片已保存到当前脚本目录:".Dump();
	filePath.Dump();
	// 在 SaveToCurrentDirectory 函数末尾添加：
	try
	{
		Process.Start(new ProcessStartInfo
		{
			FileName = filePath,
			UseShellExecute = true
		});
	}
	catch (Exception ex)
	{
		$"无法打开图片: {ex.Message}".Dump();
	}
}

// 实际的位图保存函数
void SaveBitmap(SKBitmap bitmap, string filePath,
			   SKEncodedImageFormat format, int quality)
{
	// 确保目录存在
	string dir = Path.GetDirectoryName(filePath);
	if (!Directory.Exists(dir))
	{
		Directory.CreateDirectory(dir);
	}

	// 保存图像
	using (var image = SKImage.FromBitmap(bitmap))
	using (var data = image.Encode(format, quality))
	using (var stream = File.Create(filePath))
	{
		data.SaveTo(stream);
	}
}

// 创建分形图像
SKBitmap GenerateMandelbrot(int width = 800, int height = 600, int maxIter = 100)
{
	var bitmap = new SKBitmap(width, height);
	var pixels = bitmap.PeekPixels();

	Parallel.For(0, height, y =>
	{
		for (int x = 0; x < width; x++)
		{
			double re = (x - width / 2.0) * 4.0 / width;
			double im = (y - height / 2.0) * 4.0 / height;

			Complex c = new Complex(re, im);
			Complex z = Complex.Zero;
			int iter = 0;

			while (iter < maxIter && z.Magnitude < 2.0)
			{
				z = z * z + c;
				iter++;
			}

			// 根据迭代次数着色
			SKColor color = (iter == maxIter)
				? SKColors.Black
				: SKColor.FromHsv((byte)(iter % 255), 255, 255);

			bitmap.SetPixel(x, y, color);
		}
	});

	return bitmap;
}