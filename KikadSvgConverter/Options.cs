using CommandLine;

namespace KikadSvgConverter
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Path to SVG file")]
        public string SvgFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "Path to output PNG file")]
        public string PngFile { get; set; }

        [Option('s', "scale", Required = false, Default = 4, HelpText = "Scale factor")]
        public int Scale { get; set; }
    }
}
