using SkiaSharp;

var sourceImage = SKBitmap.Decode("/Users/davidbetteridge/Personal/Lego/Lego/sample5.jpeg");
var sourceWidth = sourceImage.Width;
var sourceHeight = sourceImage.Height;

var circlesAcross = 100;

var circlesDown = sourceHeight / (sourceWidth / circlesAcross);

int circleRadius = 10;
int frameWidth = 20;


var imageInfo = new SKImageInfo(
    width: (2 * frameWidth) + (circlesAcross * (circleRadius * 2)),
    height: (2 * frameWidth) + (circlesDown * (circleRadius * 2)),
    colorType: SKColorType.Rgba8888,
    alphaType: SKAlphaType.Premul);
    
var surface = SKSurface.Create(imageInfo);

var canvas = surface.Canvas;

canvas.Clear(SKColor.Parse("#FFFFFF"));


var framePaint = new SKPaint
{
    Color = new SKColor(0,0,0),
    StrokeWidth = 5,
    IsAntialias = true,
    Style = SKPaintStyle.Fill
};

for (int column = 0; column < circlesAcross + 2; column++)
{
    canvas.DrawRect(1 + column * (circleRadius * 2), 
                    2, 
                    19, 19, 
                    framePaint);
    
    canvas.DrawRect(1 + column * (circleRadius * 2), 
        imageInfo.Height - (circleRadius * 2), 
        19, 19, 
        framePaint);
}

for (int row = 1; row < circlesDown + 2; row++)
{
    // Left hand side
    canvas.DrawRect(1, 
        2 + row * (circleRadius * 2), 
        19, 19, 
        framePaint);
    
    // Right hand side
    canvas.DrawRect(
        imageInfo.Width - (circleRadius * 2), 
        2 + row * (circleRadius * 2), 
        19, 19, 
        framePaint);
}

var rand = new Random();
int lineCount = 1000;

for (int column = 0; column < circlesAcross; column++)
{
    for (int row = 0; row < circlesDown; row++)
    {
        float lineWidth = 5;
        // var lineColor = new SKColor(
        //     red: (byte)rand.Next(255),
        //     green: (byte)rand.Next(255),
        //     blue: (byte)rand.Next(255),
        //     alpha: (byte)rand.Next(255));

        // Sample the source image
        var totalRed = 0.0;
        var totalGreen = 0.0;
        var totalBlue = 0.0;
        var sampleCount = 0;
        
        for (int x = 0; x < sourceWidth / circlesAcross; x++)
        {
            for (int y = 0; y < sourceHeight / circlesDown; y++)
            {
                var pixel = sourceImage.GetPixel(column * (sourceWidth / circlesAcross) + x, 
                                                 row * (sourceHeight / circlesDown) + y);
                totalRed += pixel.Red;
                totalGreen += pixel.Green;
                totalBlue += pixel.Blue;
                sampleCount++;
            }
        }
        
        var lineColor = new SKColor(
            red: (byte)(totalRed / sampleCount),
            green: (byte)(totalGreen / sampleCount),
            blue: (byte)(totalBlue / sampleCount),
            alpha: 255);
        
        // var pixel = sourceImage.GetPixel(0, 0);
        // var r = pixel.Red;
        // var g = pixel.Green;
        // var b = pixel.Blue;
        
        var linePaint = new SKPaint
        {
            Color = lineColor,
            StrokeWidth = lineWidth,
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };

        canvas.DrawCircle((circleRadius * 2) + circleRadius + (column * (circleRadius * 2)), 
                          20 + circleRadius + (row * (circleRadius * 2)), 
                          10, linePaint);
    }
}

using (var image = surface.Snapshot())
using (var data = image.Encode(SKEncodedImageFormat.Png, 80))
using (var stream = File.OpenWrite("lego.png"))
{
    // save the data to a stream
    data.SaveTo(stream);
}