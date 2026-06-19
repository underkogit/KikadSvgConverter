using ImageMagick;

namespace KikadSvgConverter
{
    public class KiCadConverter : IDisposable
    {
        private MagickImage magickImage;
        private SvgParser svgParser = new();
        private int _density;
        private bool _disposed = false;

        public KiCadConverter(int density = 300)
        {
            _density = density;
            magickImage = new MagickImage();
        }

        public void SvgToPng(string svgFilePath, string outputPngPath, int scalse = 4)
        {
            if (_disposed)
                throw new InvalidOperationException("KiCadConverter object has been disposed");

            if (string.IsNullOrWhiteSpace(svgFilePath))
                throw new ArgumentNullException(nameof(svgFilePath), "SVG file path cannot be null or empty");

            if (string.IsNullOrWhiteSpace(outputPngPath))
                throw new ArgumentNullException(nameof(outputPngPath), "Output PNG path cannot be null or empty");

            if (!File.Exists(svgFilePath))
                throw new FileNotFoundException($"SVG file not found: {svgFilePath}");

            try
            {
                svgParser.Parse(svgFilePath);

                MagickReadSettings settings = new()
                {
                    Density = new Density(_density)
                };
                magickImage.Read(svgFilePath, settings);

                long width = magickImage.Width * scalse;
                long height = magickImage.Height * scalse;


                magickImage.Resize((uint)width, (uint)height);

                magickImage.Format = MagickFormat.Png;
                magickImage.Alpha(AlphaOption.On);
                magickImage.Write(outputPngPath);
            }
            catch (MagickException ex)
            {
                throw new InvalidOperationException($"Error during SVG to PNG conversion: {ex.Message}", ex);
            }
        }

        public async Task SvgToPngAsync(string svgFilePath, string outputPngPath)
        {
            await Task.Run(() => SvgToPng(svgFilePath, outputPngPath));
        }

        public void SetDensity(int density)
        {
            if (density <= 0)
                throw new ArgumentException("Density must be greater than 0", nameof(density));
            _density = density;
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
                    magickImage?.Dispose();
                    magickImage.Dispose();
                }
                _disposed = true;
            }
        }

        ~KiCadConverter()
        {
            Dispose(false);
        }
    }
}
