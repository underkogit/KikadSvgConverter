# KiCad SVG to PNG Converter

A command-line utility for converting SVG files to PNG format with scaling capabilities, specifically designed for KiCad compatibility.

## Features

- Convert SVG files to PNG format
- Configurable scale factor for resolution control
- Interactive mode for manual operation
- Command-line mode for automation and scripting
- Support for various SVG dimension units (mm, cm, in, px, pt)
- High-quality output using ImageMagick engine

## Installation

### Prerequisites

- [.NET Runtime](https://dotnet.microsoft.com/download) (version 6.0 or higher)
- [ImageMagick](https://imagemagick.org/script/download.php) installed on your system

### Build from Source

```bash
git clone <repository-url>
cd KiCadSvgConverter
dotnet build -c Release
```

### Install as Global Tool

```bash
dotnet tool install --global KiCadSvgConverter
```

## Usage

The utility supports two operating modes:

### 1. Interactive Mode

Run the utility without any arguments to enter interactive mode:

```bash
KiCadSvgConverter
```

You will be prompted to enter:
- **SVG file path**: Path to the input SVG file
- **PNG file path**: Output PNG file path (press Enter to auto-generate from SVG filename)
- **Scale factor**: Scaling multiplier (default: 4, press Enter to accept default)

Example interaction:
```
Enter path to SVG file: C:\Projects\schematic.svg
Enter path to PNG file (Enter = auto-generate): 
Enter scale factor (Enter = 4): 6
Conversion completed successfully!
Output file: C:\Projects\schematic.png
Scale factor: 6x
==========
```

### 2. Command-Line Mode

Use command-line arguments for automation:

```bash
KiCadSvgConverter -i <input.svg> -o <output.png> [-s <scale>]
```

#### Parameters

| Parameter | Short | Long | Required | Description |
|-----------|-------|------|----------|-------------|
| `-i` | `--input` | `-i` | Yes | Path to input SVG file |
| `-o` | `--output` | `-o` | Yes | Path to output PNG file |
| `-s` | `--scale` | `-s` | No | Scale factor (default: 4) |

#### Examples

Basic conversion:
```bash
KiCadSvgConverter -i schematic.svg -o schematic.png
```

Conversion with custom scale:
```bash
KiCadSvgConverter -i schematic.svg -o schematic.png -s 8
```

## Error Handling

The utility provides descriptive error messages for common issues:

- **File not found**: Input SVG file doesn't exist
- **Invalid format**: SVG file contains malformed dimensions
- **Unsupported unit**: Dimension unit not recognized
- **Conversion error**: ImageMagick processing failure

## Technical Details

### Architecture

- **SvgParser**: Extracts and validates SVG dimensions
- **KiCadConverter**: Handles the conversion using ImageMagick
- **Options**: Command-line argument parsing using CommandLineParser

### Dependencies

- **CommandLineParser**: For argument parsing
- **ImageMagick.NET**: For image conversion
- **System.Xml.Linq**: For SVG parsing

## Performance Notes

- The utility uses a default density of 300 DPI for high-quality output
- Scale factor multiplies the output dimensions (e.g., 4× = 4 times larger)
- Large SVG files may require significant memory and processing time

## Troubleshooting

### Issue: "ImageMagick not found"
**Solution**: Ensure ImageMagick is installed and in your system PATH

### Issue: "Invalid dimension format"
**Solution**: Verify SVG dimensions use supported units (mm, cm, in, px, pt)

### Issue: "Access denied" on output file
**Solution**: Ensure write permissions in the output directory
 