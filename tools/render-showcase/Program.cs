using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

const int VideoWidth = 1920;
const int VideoHeight = 1080;
const int FramesPerSecond = 30;
const double TransitionDuration = 0.4;

var root = FindRepositoryRoot(AppContext.BaseDirectory);
var imageDir = Path.Combine(root, "Images");
var videoDir = Path.Combine(root, "Video");
var stillDir = Path.Combine(videoDir, "stills");
var ffmpegPath = Path.Combine(root, ".local-tools", "ffmpeg", "ffmpeg.exe");
var outputPath = Path.Combine(videoDir, "image-detail-denoise-showcase.mp4");

Directory.CreateDirectory(videoDir);
Directory.CreateDirectory(stillDir);

if (!File.Exists(ffmpegPath))
{
    throw new FileNotFoundException("ffmpeg.exe was not found. Download it into .local-tools/ffmpeg/ffmpeg.exe first.", ffmpegPath);
}

var source = new SourceImages(
    Original: Path.Combine(imageDir, "EdSplash.jpg"),
    Rich: Path.Combine(imageDir, "ed_splash_denoise_detail_3_rich.jpg"),
    Balanced: Path.Combine(imageDir, "ed_splash_denoise_detail_2_balanced.jpg"),
    Clean: Path.Combine(imageDir, "ed_splash_denoise_detail_1_clean_simplified.jpg"));

source.Validate();

using var original = Image.FromFile(source.Original);
using var rich = Image.FromFile(source.Rich);
using var balanced = Image.FromFile(source.Balanced);
using var clean = Image.FromFile(source.Clean);

var stills = new List<StillSpec>
{
    SaveStill(Path.Combine(stillDir, "slide_00_thumbnail.jpg"), RenderThumbnail(original, rich, balanced, clean), 2.8),
    SaveStill(Path.Combine(stillDir, "slide_01_intro.jpg"), RenderIntro(original, balanced), 3.6),
    SaveStill(Path.Combine(stillDir, "slide_02_original.jpg"), RenderStageSlide(original, "SOURCE BASELINE", "Original Image", "Before the Codex skill is applied", "The original source image is used as the comparison baseline for every denoise detail level.", "01 / 04", Color.FromArgb(247, 194, 84), 0), 1.4),
    SaveStill(Path.Combine(stillDir, "slide_03_detail_level_3.jpg"), RenderStageSlide(rich, "DETAIL LEVEL 3", "Rich Detail", "Maximum texture retention", "Preserves the rich rendering and material detail while selectively removing distracting dot-like noise.", "02 / 04", Color.FromArgb(97, 206, 255), 1), 1.4),
    SaveStill(Path.Combine(stillDir, "slide_04_detail_level_2.jpg"), RenderStageSlide(balanced, "DETAIL LEVEL 2", "Balanced Detail", "The recommended default", "Keeps a detailed illustrated look while smoothing noisy micro-contrast, grain, and scattered highlights.", "03 / 04", Color.FromArgb(126, 225, 158), 2), 1.4),
    SaveStill(Path.Combine(stillDir, "slide_05_detail_level_1.jpg"), RenderStageSlide(clean, "DETAIL LEVEL 1", "Clean Simplified", "Clearer forms with less visual noise", "Strongly reduces speckled noise and busy micro-texture while keeping the original composition readable.", "04 / 04", Color.FromArgb(255, 132, 118), 3), 5.4),
};

RenderVideoWithFfmpeg(ffmpegPath, stills, outputPath);

Console.WriteLine($"Wrote video: {outputPath}");
Console.WriteLine($"Wrote preserved stills: {stillDir}");

static Bitmap RenderThumbnail(Image original, Image rich, Image balanced, Image clean)
{
    var bitmap = CreateCanvas();
    using var graphics = CreateGraphics(bitmap);
    DrawBackground(graphics, Color.FromArgb(97, 206, 255));

    using var eyebrowFont = new Font("Segoe UI Semibold", 30, FontStyle.Regular, GraphicsUnit.Pixel);
    using var titleFont = new Font("Segoe UI Semibold", 96, FontStyle.Regular, GraphicsUnit.Pixel);
    using var bodyFont = new Font("Segoe UI", 36, FontStyle.Regular, GraphicsUnit.Pixel);
    using var labelFont = new Font("Segoe UI Semibold", 24, FontStyle.Regular, GraphicsUnit.Pixel);
    using var accentBrush = new SolidBrush(Color.FromArgb(97, 206, 255));
    using var whiteBrush = new SolidBrush(Color.FromArgb(248, 250, 252));
    using var mutedBrush = new SolidBrush(Color.FromArgb(188, 194, 204));

    graphics.DrawString("CODEX-ONLY IMAGE SKILL", eyebrowFont, accentBrush, new RectangleF(140, 112, 760, 48), NearFormat());
    graphics.DrawString("Image Detail Denoise", titleFont, whiteBrush, new RectangleF(140, 182, 950, 118), NearFormat());
    graphics.DrawString(
        "Reduce speckled noise while preserving useful detail, texture, color, and composition.",
        bodyFont,
        mutedBrush,
        new RectangleF(146, 328, 830, 120),
        NearFormat());

    var cards = new[]
    {
        (Image: original, Label: "Original", Rect: new RectangleF(1120, 130, 265, 354)),
        (Image: rich, Label: "Level 3", Rect: new RectangleF(1430, 130, 265, 354)),
        (Image: balanced, Label: "Level 2", Rect: new RectangleF(1120, 560, 265, 354)),
        (Image: clean, Label: "Level 1", Rect: new RectangleF(1430, 560, 265, 354)),
    };

    foreach (var card in cards)
    {
        DrawMiniImageCard(graphics, card.Image, card.Rect, card.Label, labelFont, Color.FromArgb(97, 206, 255));
    }

    using var smallFont = new Font("Segoe UI", 26, FontStyle.Regular, GraphicsUnit.Pixel);
    graphics.DrawString("Original > Rich Detail > Balanced Detail > Clean Simplified", smallFont, mutedBrush, new RectangleF(142, 928, 920, 48), NearFormat());

    return bitmap;
}

static Bitmap RenderIntro(Image original, Image balanced)
{
    var bitmap = CreateCanvas();
    using var graphics = CreateGraphics(bitmap);
    DrawBackground(graphics, Color.FromArgb(126, 225, 158));

    using var eyebrowFont = new Font("Segoe UI Semibold", 28, FontStyle.Regular, GraphicsUnit.Pixel);
    using var titleFont = new Font("Segoe UI Semibold", 82, FontStyle.Regular, GraphicsUnit.Pixel);
    using var bodyFont = new Font("Segoe UI", 34, FontStyle.Regular, GraphicsUnit.Pixel);
    using var labelFont = new Font("Segoe UI Semibold", 26, FontStyle.Regular, GraphicsUnit.Pixel);
    using var accentBrush = new SolidBrush(Color.FromArgb(126, 225, 158));
    using var whiteBrush = new SolidBrush(Color.FromArgb(248, 250, 252));
    using var mutedBrush = new SolidBrush(Color.FromArgb(190, 197, 206));

    graphics.DrawString("FEATURE INTRO", eyebrowFont, accentBrush, new RectangleF(140, 116, 640, 44), NearFormat());
    graphics.DrawString("What It Improves", titleFont, whiteBrush, new RectangleF(140, 184, 820, 100), NearFormat());
    graphics.DrawString(
        "The skill cleans tiny speckles, glitter-like dots, grainy micro-shading, and scattered highlights while keeping the image identity intact.",
        bodyFont,
        mutedBrush,
        new RectangleF(144, 316, 810, 180),
        NearFormat());

    DrawComparisonPanel(graphics, original, balanced, new RectangleF(1030, 120, 720, 860), labelFont);

    using var footFont = new Font("Segoe UI", 27, FontStyle.Regular, GraphicsUnit.Pixel);
    graphics.DrawString("Built for Codex workflows only.", footFont, whiteBrush, new RectangleF(144, 905, 780, 48), NearFormat());

    return bitmap;
}

static Bitmap RenderStageSlide(Image source, string eyebrow, string title, string subtitle, string body, string step, Color accent, int levelIndex)
{
    var bitmap = CreateCanvas();
    using var graphics = CreateGraphics(bitmap);
    DrawBackground(graphics, accent);

    using var topFont = new Font("Segoe UI", 24, FontStyle.Regular, GraphicsUnit.Pixel);
    using var stepFont = new Font("Segoe UI Semibold", 28, FontStyle.Regular, GraphicsUnit.Pixel);
    using var eyebrowFont = new Font("Segoe UI Semibold", 25, FontStyle.Regular, GraphicsUnit.Pixel);
    using var titleFont = new Font("Segoe UI Semibold", 82, FontStyle.Regular, GraphicsUnit.Pixel);
    using var subtitleFont = new Font("Segoe UI", 37, FontStyle.Regular, GraphicsUnit.Pixel);
    using var bodyFont = new Font("Segoe UI", 31, FontStyle.Regular, GraphicsUnit.Pixel);
    using var smallFont = new Font("Segoe UI", 22, FontStyle.Regular, GraphicsUnit.Pixel);

    DrawTopBar(graphics, topFont, stepFont, step);
    DrawImagePanel(graphics, source, new RectangleF(160, 120, 700, 890), accent);

    var textX = 950f;
    var textY = 248f;
    using var accentBrush = new SolidBrush(accent);
    using var whiteBrush = new SolidBrush(Color.FromArgb(248, 250, 252));
    using var mutedBrush = new SolidBrush(Color.FromArgb(188, 194, 204));
    using var softBrush = new SolidBrush(Color.FromArgb(148, 154, 164));

    graphics.FillRectangle(accentBrush, textX, textY - 2, 78, 5);
    graphics.DrawString(eyebrow, eyebrowFont, accentBrush, new RectangleF(textX, textY + 25, 760, 42), NearFormat());
    graphics.DrawString(title, titleFont, whiteBrush, new RectangleF(textX, textY + 88, 800, 104), NearFormat());
    graphics.DrawString(subtitle, subtitleFont, mutedBrush, new RectangleF(textX, textY + 206, 780, 52), NearFormat());
    graphics.DrawString(body, bodyFont, whiteBrush, new RectangleF(textX, textY + 300, 770, 170), NearFormat());
    DrawLevelDots(graphics, levelIndex, 4, accent);
    graphics.DrawString("Sequence: Original > Rich Detail > Balanced Detail > Clean Simplified", smallFont, softBrush, new RectangleF(textX, 930, 800, 42), NearFormat());
    graphics.DrawString("Codex-only skill showcase", smallFont, softBrush, new RectangleF(textX, 970, 800, 42), NearFormat());

    return bitmap;
}

static StillSpec SaveStill(string path, Bitmap bitmap, double duration)
{
    using (bitmap)
    {
        SaveJpeg(bitmap, path, 94L);
    }

    return new StillSpec(path, duration);
}

static void RenderVideoWithFfmpeg(string ffmpegPath, IReadOnlyList<StillSpec> stills, string outputPath)
{
    if (File.Exists(outputPath))
    {
        File.Delete(outputPath);
    }

    var frameDir = Path.Combine(Path.GetDirectoryName(outputPath)!, "tmp", "windshield-wipe-frames");
    var totalDuration = RenderTimelineFrames(stills, frameDir);

    var args = new List<string>();
    args.AddRange(new[]
    {
        "-hide_banner",
        "-loglevel", "error",
        "-nostdin",
        "-y",
        "-framerate", FramesPerSecond.ToString(System.Globalization.CultureInfo.InvariantCulture),
        "-i", Path.Combine(frameDir, "frame_%04d.jpg"),
        "-t", totalDuration.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture),
        "-r", FramesPerSecond.ToString(System.Globalization.CultureInfo.InvariantCulture),
        "-vf", "format=yuv420p",
        "-c:v", "libx264",
        "-preset", "medium",
        "-crf", "18",
        "-pix_fmt", "yuv420p",
        "-movflags", "+faststart",
        outputPath,
    });

    var startInfo = new ProcessStartInfo
    {
        FileName = ffmpegPath,
        UseShellExecute = false,
        RedirectStandardError = false,
        RedirectStandardOutput = false,
    };

    foreach (var arg in args)
    {
        startInfo.ArgumentList.Add(arg);
    }

    using var process = Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start ffmpeg.");
    process.WaitForExit();

    if (process.ExitCode != 0)
    {
        throw new InvalidOperationException($"ffmpeg failed with exit code {process.ExitCode}.");
    }
}

static double RenderTimelineFrames(IReadOnlyList<StillSpec> stills, string frameDir)
{
    if (Directory.Exists(frameDir))
    {
        Directory.Delete(frameDir, true);
    }

    Directory.CreateDirectory(frameDir);

    using var frameSet = new FrameBufferSet(stills.Select(still => still.Path));
    var totalDuration = CalculateTotalDuration(stills);
    var totalFrames = (int)Math.Round(FramesPerSecond * totalDuration);

    for (var frameIndex = 0; frameIndex < totalFrames; frameIndex++)
    {
        var timestamp = frameIndex / (double)FramesPerSecond;
        var plan = GetFramePlan(timestamp, stills);
        var outputFrame = Path.Combine(frameDir, $"frame_{frameIndex:0000}.jpg");

        if (!plan.IsTransition)
        {
            File.Copy(stills[plan.From].Path, outputFrame, true);
            continue;
        }

        using var frame = plan.Style == TransitionStyle.Fade
            ? BlendFade(frameSet[plan.From], frameSet[plan.To], plan.Progress)
            : BlendWindshieldWipe(frameSet[plan.From], frameSet[plan.To], plan.Progress);
        SaveJpeg(frame, outputFrame, 92L);
    }

    return totalDuration;
}

static double CalculateTotalDuration(IReadOnlyList<StillSpec> stills)
{
    return stills.Sum(still => still.Duration) - TransitionDuration * (stills.Count - 1);
}

static FramePlan GetFramePlan(double timestamp, IReadOnlyList<StillSpec> stills)
{
    var slideStart = 0.0;
    for (var index = 0; index < stills.Count - 1; index++)
    {
        var transitionStart = slideStart + stills[index].Duration - TransitionDuration;
        if (timestamp < transitionStart)
        {
            return new FramePlan(false, index, index, 0, TransitionStyle.None);
        }

        if (timestamp < transitionStart + TransitionDuration)
        {
            var style = index < 2 ? TransitionStyle.Fade : TransitionStyle.WindshieldWipe;
            return new FramePlan(true, index, index + 1, (timestamp - transitionStart) / TransitionDuration, style);
        }

        slideStart = transitionStart;
    }

    return new FramePlan(false, stills.Count - 1, stills.Count - 1, 0, TransitionStyle.None);
}

static Bitmap BlendFade(FrameBuffer from, FrameBuffer to, double progress)
{
    var output = new byte[from.Bytes.Length];
    var alpha = EaseInOutCubic(progress);

    for (var offset = 0; offset < output.Length; offset++)
    {
        output[offset] = BlendByte(from.Bytes[offset], to.Bytes[offset], alpha);
    }

    return FrameBuffer.CreateBitmap(output);
}

static Bitmap BlendWindshieldWipe(FrameBuffer from, FrameBuffer to, double progress)
{
    var output = new byte[from.Bytes.Length];
    var eased = EaseInOutCubic(progress);
    var pivotX = VideoWidth * 0.18;
    var pivotY = VideoHeight + 360.0;
    var featherRadians = 0.11;
    var glowRadians = 0.034;
    var glowStrength = Math.Sin(Math.Clamp(progress, 0, 1) * Math.PI) * 0.5;
    var glowColor = (B: 255.0, G: 226.0, R: 108.0);
    var angles = new[]
    {
        Math.Atan2(0 - pivotY, 0 - pivotX),
        Math.Atan2(0 - pivotY, VideoWidth - pivotX),
        Math.Atan2(VideoHeight - pivotY, 0 - pivotX),
        Math.Atan2(VideoHeight - pivotY, VideoWidth - pivotX),
    };

    var startAngle = angles.Min() - featherRadians * 2.2;
    var endAngle = angles.Max() + featherRadians * 2.2;
    var boundary = startAngle + (endAngle - startAngle) * eased;

    for (var y = 0; y < VideoHeight; y++)
    {
        var dy = y - pivotY;
        var row = y * FrameBuffer.RowBytes;
        for (var x = 0; x < VideoWidth; x++)
        {
            var angle = Math.Atan2(dy, x - pivotX);
            var alpha = SmoothStep((boundary + featherRadians - angle) / (featherRadians * 2.0));
            var distance = Math.Abs(angle - boundary);
            var glow = Math.Exp(-(distance * distance) / (2.0 * glowRadians * glowRadians)) * glowStrength;
            var offset = row + x * 3;

            output[offset] = ApplyGlow(BlendByte(from.Bytes[offset], to.Bytes[offset], alpha), glowColor.B, glow);
            output[offset + 1] = ApplyGlow(BlendByte(from.Bytes[offset + 1], to.Bytes[offset + 1], alpha), glowColor.G, glow);
            output[offset + 2] = ApplyGlow(BlendByte(from.Bytes[offset + 2], to.Bytes[offset + 2], alpha), glowColor.R, glow);
        }
    }

    return FrameBuffer.CreateBitmap(output);
}

static byte ApplyGlow(byte value, double glowChannel, double glow)
{
    return (byte)Math.Clamp((int)Math.Round(value + (glowChannel - value) * glow), 0, 255);
}

static byte BlendByte(byte from, byte to, double alpha)
{
    return (byte)Math.Clamp((int)Math.Round(from + (to - from) * alpha), 0, 255);
}

static double SmoothStep(double value)
{
    value = Math.Clamp(value, 0, 1);
    return value * value * (3 - 2 * value);
}

static double EaseInOutCubic(double value)
{
    value = Math.Clamp(value, 0, 1);
    return value < 0.5
        ? 4 * value * value * value
        : 1 - Math.Pow(-2 * value + 2, 3) / 2;
}

static Bitmap CreateCanvas()
{
    var bitmap = new Bitmap(VideoWidth, VideoHeight, PixelFormat.Format32bppRgb);
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

static void DrawBackground(Graphics graphics, Color accent)
{
    var bounds = new Rectangle(0, 0, VideoWidth, VideoHeight);
    using var baseBrush = new LinearGradientBrush(bounds, Color.FromArgb(14, 15, 18), Color.FromArgb(31, 30, 34), 24f);
    graphics.FillRectangle(baseBrush, bounds);

    using var accentBrush = new SolidBrush(Color.FromArgb(34, accent));
    graphics.FillPolygon(accentBrush, new[]
    {
        new PointF(1280, -80),
        new PointF(1960, -80),
        new PointF(1960, 1080),
        new PointF(1560, 1080),
    });

    using var linePen = new Pen(Color.FromArgb(24, 255, 255, 255), 1f);
    for (var x = -220; x < VideoWidth + 220; x += 72)
    {
        graphics.DrawLine(linePen, x, 0, x + 440, VideoHeight);
    }
}

static void DrawTopBar(Graphics graphics, Font topFont, Font stepFont, string step)
{
    using var mutedBrush = new SolidBrush(Color.FromArgb(170, 176, 188));
    using var stepBrush = new SolidBrush(Color.FromArgb(238, 241, 246));
    graphics.DrawString("IMAGE DETAIL DENOISE SKILL", topFont, mutedBrush, new RectangleF(160, 54, 720, 36), NearFormat());
    graphics.DrawString(step, stepFont, stepBrush, new RectangleF(1610, 52, 150, 42), FarFormat());
}

static void DrawImagePanel(Graphics graphics, Image image, RectangleF shell, Color accent)
{
    DrawShadow(graphics, shell, 28);
    using var shellPath = RoundedRectangle(shell, 26);
    using var shellBrush = new SolidBrush(Color.FromArgb(250, 21, 23, 28));
    using var shellPen = new Pen(Color.FromArgb(70, 255, 255, 255), 1f);
    graphics.FillPath(shellBrush, shellPath);
    graphics.DrawPath(shellPen, shellPath);

    var imageRect = new RectangleF(shell.X + 35, shell.Y + 35, shell.Width - 70, shell.Height - 70);
    var fitted = FitContain(image.Size, imageRect);
    using var clipPath = RoundedRectangle(fitted, 18);
    graphics.SetClip(clipPath);
    graphics.DrawImage(image, fitted);
    graphics.ResetClip();

    using var imagePen = new Pen(Color.FromArgb(90, accent), 3f);
    graphics.DrawPath(imagePen, clipPath);
}

static void DrawMiniImageCard(Graphics graphics, Image image, RectangleF rect, string label, Font labelFont, Color accent)
{
    DrawShadow(graphics, rect, 18);
    using var path = RoundedRectangle(rect, 18);
    using var shellBrush = new SolidBrush(Color.FromArgb(248, 23, 25, 30));
    graphics.FillPath(shellBrush, path);

    var imageRect = new RectangleF(rect.X + 14, rect.Y + 14, rect.Width - 28, rect.Height - 64);
    var fitted = FitContain(image.Size, imageRect);
    using var clip = RoundedRectangle(fitted, 12);
    graphics.SetClip(clip);
    graphics.DrawImage(image, fitted);
    graphics.ResetClip();

    using var accentBrush = new SolidBrush(accent);
    graphics.DrawString(label, labelFont, accentBrush, new RectangleF(rect.X + 18, rect.Bottom - 42, rect.Width - 36, 32), NearFormat());
}

static void DrawComparisonPanel(Graphics graphics, Image before, Image after, RectangleF bounds, Font labelFont)
{
    DrawShadow(graphics, bounds, 24);
    using var path = RoundedRectangle(bounds, 24);
    using var shellBrush = new SolidBrush(Color.FromArgb(248, 21, 23, 28));
    graphics.FillPath(shellBrush, path);

    var beforeRect = new RectangleF(bounds.X + 34, bounds.Y + 34, (bounds.Width - 86) / 2, bounds.Height - 102);
    var afterRect = new RectangleF(beforeRect.Right + 18, beforeRect.Y, beforeRect.Width, beforeRect.Height);
    DrawLabeledImage(graphics, before, beforeRect, "Before", labelFont, Color.FromArgb(247, 194, 84));
    DrawLabeledImage(graphics, after, afterRect, "After", labelFont, Color.FromArgb(126, 225, 158));
}

static void DrawLabeledImage(Graphics graphics, Image image, RectangleF rect, string label, Font labelFont, Color accent)
{
    var fitted = FitContain(image.Size, rect);
    using var clip = RoundedRectangle(fitted, 16);
    graphics.SetClip(clip);
    graphics.DrawImage(image, fitted);
    graphics.ResetClip();

    using var pen = new Pen(Color.FromArgb(120, accent), 3f);
    graphics.DrawPath(pen, clip);
    using var brush = new SolidBrush(accent);
    graphics.DrawString(label, labelFont, brush, new RectangleF(rect.X, rect.Bottom + 20, rect.Width, 36), NearFormat());
}

static void DrawShadow(Graphics graphics, RectangleF bounds, int radius)
{
    for (var i = 5; i >= 1; i--)
    {
        var grow = i * 7f;
        var alpha = 12 + i * 5;
        using var brush = new SolidBrush(Color.FromArgb(alpha, 0, 0, 0));
        using var path = RoundedRectangle(new RectangleF(bounds.X - grow / 2, bounds.Y + grow / 2, bounds.Width + grow, bounds.Height + grow), radius + grow / 3);
        graphics.FillPath(brush, path);
    }
}

static void DrawLevelDots(Graphics graphics, int index, int count, Color accent)
{
    var startX = 950f;
    var y = 780f;
    for (var i = 0; i < count; i++)
    {
        var x = startX + i * 42f;
        using var brush = new SolidBrush(i == index ? accent : Color.FromArgb(78, 255, 255, 255));
        graphics.FillEllipse(brush, x, y, i == index ? 24 : 16, i == index ? 24 : 16);
    }
}

static RectangleF FitContain(Size source, RectangleF bounds)
{
    var sourceRatio = source.Width / (float)source.Height;
    var boundsRatio = bounds.Width / bounds.Height;
    if (sourceRatio > boundsRatio)
    {
        var height = bounds.Width / sourceRatio;
        return new RectangleF(bounds.X, bounds.Y + (bounds.Height - height) / 2f, bounds.Width, height);
    }

    var width = bounds.Height * sourceRatio;
    return new RectangleF(bounds.X + (bounds.Width - width) / 2f, bounds.Y, width, bounds.Height);
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

static StringFormat NearFormat() => new()
{
    Alignment = StringAlignment.Near,
    LineAlignment = StringAlignment.Near,
    Trimming = StringTrimming.EllipsisWord,
    FormatFlags = StringFormatFlags.LineLimit,
};

static StringFormat FarFormat() => new()
{
    Alignment = StringAlignment.Far,
    LineAlignment = StringAlignment.Near,
    Trimming = StringTrimming.EllipsisCharacter,
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

record SourceImages(string Original, string Rich, string Balanced, string Clean)
{
    public void Validate()
    {
        foreach (var path in new[] { Original, Rich, Balanced, Clean })
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Missing source image.", path);
            }
        }
    }
}

record StillSpec(string Path, double Duration);

readonly record struct FramePlan(bool IsTransition, int From, int To, double Progress, TransitionStyle Style);

enum TransitionStyle
{
    None,
    Fade,
    WindshieldWipe,
}

sealed class FrameBufferSet : IDisposable
{
    private readonly List<FrameBuffer> frames;

    public FrameBufferSet(IEnumerable<string> paths)
    {
        frames = paths.Select(FrameBuffer.Load).ToList();
    }

    public FrameBuffer this[int index] => frames[index];

    public void Dispose()
    {
        foreach (var frame in frames)
        {
            frame.Dispose();
        }
    }
}

sealed class FrameBuffer : IDisposable
{
    private const int Width = 1920;
    private const int Height = 1080;
    public const int RowBytes = Width * 3;

    private FrameBuffer(Bitmap bitmap, byte[] bytes)
    {
        Bitmap = bitmap;
        Bytes = bytes;
    }

    public Bitmap Bitmap { get; }
    public byte[] Bytes { get; }

    public static FrameBuffer Load(string path)
    {
        using var source = Image.FromFile(path);
        var bitmap = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
        bitmap.SetResolution(96, 96);

        using (var graphics = Graphics.FromImage(bitmap))
        {
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.DrawImage(source, new Rectangle(0, 0, Width, Height));
        }

        return new FrameBuffer(bitmap, CopyBytes(bitmap));
    }

    public static Bitmap CreateBitmap(byte[] bytes)
    {
        var bitmap = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
        bitmap.SetResolution(96, 96);
        var data = bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
        try
        {
            if (data.Stride == RowBytes)
            {
                Marshal.Copy(bytes, 0, data.Scan0, bytes.Length);
            }
            else
            {
                for (var y = 0; y < Height; y++)
                {
                    Marshal.Copy(bytes, y * RowBytes, IntPtr.Add(data.Scan0, y * data.Stride), RowBytes);
                }
            }
        }
        finally
        {
            bitmap.UnlockBits(data);
        }

        return bitmap;
    }

    public void Dispose()
    {
        Bitmap.Dispose();
    }

    private static byte[] CopyBytes(Bitmap bitmap)
    {
        var bytes = new byte[RowBytes * Height];
        var data = bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        try
        {
            if (data.Stride == RowBytes)
            {
                Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
            }
            else
            {
                for (var y = 0; y < Height; y++)
                {
                    Marshal.Copy(IntPtr.Add(data.Scan0, y * data.Stride), bytes, y * RowBytes, RowBytes);
                }
            }
        }
        finally
        {
            bitmap.UnlockBits(data);
        }

        return bytes;
    }
}
