using CommandLine;
using KikadSvgConverter;

static string ReadPath(string comment)
{
    Console.Write(comment);
    return (Console.ReadLine() ?? string.Empty).Replace("\"", string.Empty).Trim();
}

if (args.Length == 0)
{
    while (true)
    {

        if (ReadPath("Enter path to SVG file: ") is { } svgFile && ReadPath("Enter path to PNG file (Enter = auto-generate): ") is { } pngFile)
        {
            if (string.IsNullOrEmpty(pngFile))
                pngFile = Path.ChangeExtension(svgFile, ".png");

            Console.Write("Enter scale factor (Enter = 4): ");
            if (!int.TryParse(Console.ReadLine(), out int scale) || scale <= 0)
                scale = 4;

            using KiCadConverter image = new();
            try
            {
                image.SvgToPng(svgFile, pngFile, scale);

                Console.WriteLine($"Conversion completed successfully!");
                Console.WriteLine($"Output file: {pngFile}");
                Console.WriteLine($"Scale factor: {scale}x");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please try again.");
        }

        Console.WriteLine("==========");
    }
}
else
{
    Parser.Default.ParseArguments<Options>(args)
    .WithParsed<Options>(opts =>
    {
        if (string.IsNullOrEmpty(opts.PngFile))
            opts.PngFile = Path.ChangeExtension(opts.SvgFile, ".png");

        using KiCadConverter image = new();
        try
        {
            image.SvgToPng(opts.SvgFile, opts.PngFile, opts.Scale);
            Console.WriteLine("Conversion completed successfully!");
            Console.WriteLine($"Output file: {opts.PngFile}");
            Console.WriteLine($"Scale factor: {opts.Scale}x");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    })
    .WithNotParsed<Options>(errs =>
    {
        Console.WriteLine("Error parsing arguments. Please check your input and try again.");
        foreach (var error in errs)
        {
            Console.WriteLine($"  - {error}");
        }
    });
}
