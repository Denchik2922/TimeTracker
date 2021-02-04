using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker.ExtraClass
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GradientView : ContentView
	{

		public Color StartColor { get; set; } = Color.Transparent;
		public Color EndColor { get; set; } = Color.Transparent;
		public bool Horizontal { get; set; } = false;

		public GradientView()
		{
			InitializeComponent();

			SKCanvasView canvasView = new SKCanvasView();
			canvasView.PaintSurface += OnCanvasViewPaintSurface;
			Content = canvasView;
		}

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            var colors = new SKColor[] { StartColor.ToSKColor(), EndColor.ToSKColor() };

            using (SKPaint paint = new SKPaint())
			{
                // Createrectangle
                SKRect rect = new SKRect(0, 0, info.Width, info.Height);


                paint.Shader = SKShader.CreateLinearGradient(
                                new SKPoint(rect.Left, rect.Top),
                                new SKPoint(rect.Right, rect.Bottom),
                                colors,
                                null,
                                SKShaderTileMode.Clamp);

                // Draw the gradient on the rectangle
                canvas.DrawRect(rect, paint);
            }
                

            // Create linear gradient from upper-left to lower-right
            

            /*SKPoint startPoint = new SKPoint(0, 0);
            SKPoint endPoint = Horizontal ? new SKPoint(info.Width, 0) : new SKPoint(0, info.Height);

            var shader = SKShader.CreateLinearGradient(startPoint, endPoint, colors, null, SKShaderTileMode.Clamp);

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Shader = shader
            };

            canvas.DrawRect(new SKRect(0, 0, info.Width, info.Height), paint);*/
        }
    }
}