using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace KikadSvgConverter
{

    public class SvgParser : IDisposable
    {
        private XDocument _document;
        private bool _disposed = false;

        public SvgDimensions Dimensions { get; private set; }

        public SvgParser()
        {
            Dimensions = new SvgDimensions();
        }

        public void Parse(string svgFilePath)
        {
            if (_disposed)
                throw new InvalidOperationException("SvgParser object has been disposed");

            if (string.IsNullOrWhiteSpace(svgFilePath))
                throw new ArgumentNullException(nameof(svgFilePath), "SVG file path cannot be null or empty");

            if (!File.Exists(svgFilePath))
                throw new FileNotFoundException($"SVG file not found: {svgFilePath}");

            try
            {
                _document = XDocument.Load(svgFilePath);
                ExtractDimensions();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error parsing SVG file: {ex.Message}", ex);
            }
        }

        public async Task ParseAsync(string svgFilePath)
        {
            await Task.Run(() => Parse(svgFilePath));
        }

        private void ExtractDimensions()
        {
            if (_document?.Root == null)
                throw new InvalidOperationException("SVG document root not found");

            XElement root = _document.Root;
            string? widthAttr = root.Attribute("width")?.Value;
            string? heightAttr = root.Attribute("height")?.Value;

            if (string.IsNullOrWhiteSpace(widthAttr))
                throw new InvalidOperationException("Width attribute not found in SVG");

            if (string.IsNullOrWhiteSpace(heightAttr))
                throw new InvalidOperationException("Height attribute not found in SVG");

            Dimensions.Width = ParseDimension(widthAttr);
            Dimensions.Height = ParseDimension(heightAttr);
            Dimensions.WidthRaw = widthAttr;
            Dimensions.HeightRaw = heightAttr;
        }

        private double ParseDimension(string dimensionString)
        {
            Match match = Regex.Match(dimensionString, @"^([\d.]+)\s*([a-zA-Z%]*)$");

            if (!match.Success)
                throw new InvalidOperationException($"Invalid dimension format: {dimensionString}");

            if (!double.TryParse(match.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture, out double value))
                throw new InvalidOperationException($"Could not parse numeric value: {match.Groups[1].Value}");

            string unit = match.Groups[2].Value.ToLower();

            return unit switch
            {
                "mm" => value,
                "cm" => value * 10,
                "in" => value * 25.4,
                "px" => value / 96 * 25.4,
                "pt" => value * 0.3528,
                "" => value,
                _ => throw new InvalidOperationException($"Unsupported unit: {unit}")
            };
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _document = null;
                }
                _disposed = true;
            }
        }

        ~SvgParser()
        {
            Dispose(false);
        }
    }

    public class SvgDimensions
    {
        public double Width { get; set; }

        public double Height { get; set; }

        public string WidthRaw { get; set; }

        public string HeightRaw { get; set; }

        public override string ToString()
        {
            return $"Width: {Width}mm ({WidthRaw}), Height: {Height}mm ({HeightRaw})";
        }
    }
}

