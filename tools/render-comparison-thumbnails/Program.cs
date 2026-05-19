using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

const int Width = 1280;
const int Height = 720;

var root = FindRepositoryRoot(AppContext.BaseDirectory);
var imageDir = Path.Combine(root, "Images");
var templateDir = Path.Combine(root, "Video", "templates");
var outputDir = Path.Combine(root, "Video");

var originalPath = Path.Combine(imageDir, "EdSplash.jpg");
var level3Path = Path.Combine(imageDir, "ed_splash_denoise_detail_3_rich.jpg");
var level1Path = Path.Combine(imageDir, "ed_splash_denoise_detail_1_clean_simplified.jpg");
var twoStageTemplatePath = Path.Combine(templateDir, "youtube-thumbnail-template-2stage-original-denoised.png");
var threeStageTemplatePath = Path.Combine(templateDir, "youtube-thumbnail-template-3stage-original-level3-level1.png");

foreach (var path in new[] { originalPath, level3Path, level1Path, twoStageTemplatePath, threeStageTemplatePath })
{
    if (!File.Exists(path))
    {
        throw new FileNotFoundException("Missing thumbnail source file.", path);
    }
}

using var original = Image.FromFile(originalPath);
using var level3 = Image.FromFile(level3Path);
using var level1 = Image.FromFile(level1Path);
using var twoStageTemplate = Image.FromFile(twoStageTemplatePath);
using var threeStageTemplate = Image.FromFile(threeStageTemplatePath);

var twoStageOutput = Path.Combine(outputDir, "youtube-thumbnail-2stage-original-denoised.jpg");
var threeStageOutput = Path.Combine(outputDir, "youtube-thumbnail-3stage-original-level3-level1.jpg");

using (var twoStage = RenderTwoStage(twoStageTemplate, original, level3))
{
    SaveJpeg(twoStage, twoStageOutput, 95L);
}

using (var threeStage = RenderThreeStage(threeStageTemplate, original, level3, level1))
{
    SaveJpeg(threeStage, threeStageOutput, 95L);
}

Console.WriteLine(twoStageOutput);
Console.WriteLine(threeStageOutput);

static Bitmap RenderTwoStage(Image template, Image original, Image level3)
{
    var bitmap = CreateCanvas();
    using var graphics = CreateGraphics(bitmap);
    DrawTemplate(graphics, template);

    DrawImageSlot(graphics, original, SlotFromTemplate(template, 70, 365, 720, 490), 0.23f);
    DrawImageSlot(graphics, level3, SlotFromTemplate(template, 890, 365, 710, 490), 0.23f);

    return bitmap;
}

static Bitmap RenderThreeStage(Image template, Image original, Image level3, Image level1)
{
    var bitmap = CreateCanvas();
    using var graphics = CreateGraphics(bitmap);
    DrawTemplate(graphics, template);

    DrawImageSlot(graphics, original, SlotFromTemplate(template, 67, 350, 470, 510), 0.24f);
    DrawImageSlot(graphics, level3, SlotFromTemplate(template, 612, 350, 450, 510), 0.24f);
    DrawImageSlot(graphics, level1, SlotFromTemplate(template, 1138, 350, 470, 510), 0.24f);

    return bitmap;
}

static Bitmap CreateCanvas()
{
    var bitmap = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
    bitmap.SetResolution(96, 96);
    return bitmap;
}

static Graphics CreateGraphics(Image image)
{
    var graphics = Graphics.FromImage(image);
    graphics.CompositingQuality = CompositingQuality.HighQuality;
    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
    graphics.SmoothingMode = SmoothingMode.AntiAlias;
    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
    return graphics;
}

static void DrawTemplate(Graphics graphics, Image template)
{
    graphics.DrawImage(template, new Rectangle(0, 0, Width, Height));
}

static RectangleF SlotFromTemplate(Image template, float x, float y, float width, float height)
{
    var scaleX = Width / (float)template.Width;
    var scaleY = Height / (float)template.Height;
    return new RectangleF(x * scaleX, y * scaleY, width * scaleX, height * scaleY);
}

static void DrawImageSlot(Graphics graphics, Image source, RectangleF bounds, float focusY)
{
    using var clip = RoundedRectangle(bounds, 8f);
    graphics.SetClip(clip);

    var crop = CropToAspect(source.Size, bounds.Width / bounds.Height, focusY);
    graphics.DrawImage(source, bounds, crop, GraphicsUnit.Pixel);
    graphics.ResetClip();

    using var innerStroke = new Pen(Color.FromArgb(105, 255, 255, 255), 1.3f);
    graphics.DrawPath(innerStroke, clip);
}

static RectangleF CropToAspect(Size source, float targetAspect, float focusY)
{
    var sourceAspect = source.Width / (float)source.Height;
    if (sourceAspect > targetAspect)
    {
        var width = source.Height * targetAspect;
        return new RectangleF((source.Width - width) / 2f, 0, width, source.Height);
    }

    var height = source.Width / targetAspect;
    var y = (source.Height - height) * Math.Clamp(focusY, 0f, 1f);
    return new RectangleF(0, y, source.Width, height);
}

static GraphicsPath RoundedRectangle(RectangleF bounds, float radius)
{
    var path = new GraphicsPath();
    var diameter = radius * 2f;
    path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
    path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
    path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
    path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
    path.CloseFigure();
    return path;
}

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
