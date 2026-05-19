using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

const int Width = 1280;
const int Height = 720;

var root = FindRepositoryRoot(AppContext.BaseDirectory);
var imageDir = Path.Combine(root, "Images");
var outputDir = Path.Combine(root, "Video");
Directory.CreateDirectory(outputDir);

var outputPath = Path.Combine(outputDir, "youtube-thumbnail-chatgpt-images-2-sparkle-noise-reduction.jpg");
var sources = new[]
{
    Path.Combine(imageDir, "EdSplash.jpg"),
    Path.Combine(imageDir, "ed_splash_denoise_detail_3_rich.jpg"),
    Path.Combine(imageDir, "ed_splash_denoise_detail_2_balanced.jpg"),
    Path.Combine(imageDir, "ed_splash_denoise_detail_1_clean_simplified.jpg"),
};

foreach (var source in sources)
{
    if (!File.Exists(source))
    {
        throw new FileNotFoundException("Missing source image.", source);
    }
}

using var original = Image.FromFile(sources[0]);
using var rich = Image.FromFile(sources[1]);
using var balanced = Image.FromFile(sources[2]);
using var clean = Image.FromFile(sources[3]);

using var canvas = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
canvas.SetResolution(96, 96);

using (var graphics = Graphics.FromImage(canvas))
{
    graphics.CompositingQuality = CompositingQuality.HighQuality;
    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
    graphics.SmoothingMode = SmoothingMode.AntiAlias;
    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

    DrawBackground(graphics);

    DrawRotatedImage(graphics, original, new PointF(470, 428), new SizeF(450, 600), -18f, 0.92f);
    DrawRotatedImage(graphics, rich, new PointF(630, 405), new SizeF(470, 627), -6f, 0.96f);
    DrawRotatedImage(graphics, balanced, new PointF(795, 420), new SizeF(490, 653), 7f, 0.98f);
    DrawRotatedImage(graphics, clean, new PointF(990, 455), new SizeF(520, 693), 18f, 1f);

    DrawTextLayer(graphics);
}

SaveJpeg(canvas, outputPath, 94L);
Console.WriteLine(outputPath);

static void DrawBackground(Graphics graphics)
{
    var bounds = new Rectangle(0, 0, Width, Height);
    using var baseBrush = new LinearGradientBrush(bounds, Color.FromArgb(12, 13, 16), Color.FromArgb(34, 30, 27), 22f);
    graphics.FillRectangle(baseBrush, bounds);

    using var warmWash = new LinearGradientBrush(
        new Rectangle(0, 0, Width, Height),
        Color.FromArgb(80, 248, 181, 70),
        Color.FromArgb(0, 248, 181, 70),
        0f);
    graphics.FillRectangle(warmWash, 0, 0, 560, Height);

    using var darkVeil = new LinearGradientBrush(
        new Rectangle(0, 0, 760, Height),
        Color.FromArgb(235, 8, 9, 12),
        Color.FromArgb(30, 8, 9, 12),
        0f);
    graphics.FillRectangle(darkVeil, 0, 0, 760, Height);

    using var linePen = new Pen(Color.FromArgb(24, 255, 255, 255), 1.2f);
    for (var x = -220; x < Width + 260; x += 64)
    {
        graphics.DrawLine(linePen, x, 0, x + 320, Height);
    }
}

static void DrawTextLayer(Graphics graphics)
{
    using var titleFont = new Font("Segoe UI Semibold", 76, FontStyle.Regular, GraphicsUnit.Pixel);
    using var titleFontSmall = new Font("Segoe UI Semibold", 70, FontStyle.Regular, GraphicsUnit.Pixel);
    using var subtitleFont = new Font("Segoe UI Black", 64, FontStyle.Regular, GraphicsUnit.Pixel);
    using var eyebrowFont = new Font("Segoe UI Semibold", 24, FontStyle.Regular, GraphicsUnit.Pixel);
    using var whiteBrush = new SolidBrush(Color.FromArgb(252, 252, 250));
    using var goldBrush = new SolidBrush(Color.FromArgb(255, 207, 84));
    using var mutedBrush = new SolidBrush(Color.FromArgb(210, 217, 227));
    using var shadowBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0));

    graphics.DrawString("CODEX SKILL SHOWCASE", eyebrowFont, mutedBrush, new RectangleF(70, 76, 520, 36), NearFormat());

    DrawShadowedText(graphics, "ChatGPT", titleFont, whiteBrush, shadowBrush, new PointF(66, 138));
    DrawShadowedText(graphics, "Images 2.0", titleFontSmall, whiteBrush, shadowBrush, new PointF(66, 218));

    using var barBrush = new SolidBrush(Color.FromArgb(255, 207, 84));
    graphics.FillRectangle(barBrush, 72, 326, 96, 8);

    DrawShadowedText(graphics, "Sparkle", subtitleFont, goldBrush, shadowBrush, new PointF(66, 360));
    DrawShadowedText(graphics, "Noise Reduction", subtitleFont, whiteBrush, shadowBrush, new PointF(66, 432));
}

static void DrawShadowedText(Graphics graphics, string text, Font font, Brush brush, Brush shadowBrush, PointF point)
{
    graphics.DrawString(text, font, shadowBrush, new PointF(point.X + 5, point.Y + 7), NearFormat());
    graphics.DrawString(text, font, brush, point, NearFormat());
}

static void DrawRotatedImage(Graphics graphics, Image source, PointF center, SizeF size, float angle, float opacity)
{
    using var image = new Bitmap((int)Math.Round(size.Width), (int)Math.Round(size.Height), PixelFormat.Format32bppArgb);
    image.SetResolution(96, 96);

    using (var imageGraphics = Graphics.FromImage(image))
    {
        imageGraphics.CompositingQuality = CompositingQuality.HighQuality;
        imageGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        imageGraphics.SmoothingMode = SmoothingMode.AntiAlias;
        imageGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        var crop = CropToAspect(source.Size, size.Width / size.Height);
        imageGraphics.DrawImage(source, new RectangleF(0, 0, size.Width, size.Height), crop, GraphicsUnit.Pixel);
    }

    var state = graphics.Save();
    graphics.TranslateTransform(center.X, center.Y);
    graphics.RotateTransform(angle);

    using var shadow = new SolidBrush(Color.FromArgb(150, 0, 0, 0));
    graphics.FillRectangle(shadow, -size.Width / 2 + 16, -size.Height / 2 + 22, size.Width, size.Height);

    var colorMatrix = new ColorMatrix
    {
        Matrix33 = Math.Clamp(opacity, 0f, 1f),
    };
    using var attributes = new ImageAttributes();
    attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

    graphics.DrawImage(
        image,
        new Rectangle((int)Math.Round(-size.Width / 2), (int)Math.Round(-size.Height / 2), image.Width, image.Height),
        0,
        0,
        image.Width,
        image.Height,
        GraphicsUnit.Pixel,
        attributes);

    graphics.Restore(state);
}

static RectangleF CropToAspect(Size source, float targetAspect)
{
    var sourceAspect = source.Width / (float)source.Height;
    if (sourceAspect > targetAspect)
    {
        var width = source.Height * targetAspect;
        return new RectangleF((source.Width - width) / 2f, 0, width, source.Height);
    }

    var height = source.Width / targetAspect;
    return new RectangleF(0, (source.Height - height) / 2f, source.Width, height);
}

static StringFormat NearFormat() => new()
{
    Alignment = StringAlignment.Near,
    LineAlignment = StringAlignment.Near,
    Trimming = StringTrimming.EllipsisWord,
    FormatFlags = StringFormatFlags.LineLimit,
};

static void SaveJpeg(Image image, string path, long quality)
{
    var codec = ImageCodecInfo.GetImageEncoders().First(c => c.MimeType == "image/jpeg");
    using var parameters = new EncoderParameters(1);
    parameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
    image.Save(path, codec, parameters);
}

static string FindRepositoryRoot(string start)
{
    var directory = new DirectoryInfo(start);
    while (directory != null)
    {
        if (Directory.Exists(Path.Combine(directory.FullName, ".git")))
        {
            return directory.FullName;
        }

        directory = directory.Parent;
    }

    return Directory.GetCurrentDirectory();
}
