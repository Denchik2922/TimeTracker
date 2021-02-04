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
	public partial class PoligonView : ContentView
	{
        public Color Color { get; set; } = Color.Transparent;

        public PoligonView()
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


            SKPaint strokePaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.ToSKColor(),
                StrokeWidth = 10,
                
            };

            // Create the path
            SKPath path = new SKPath();

            float per = 70;

            // Define the drawing path points
            path.MoveTo(0, info.Height); // start point
            path.LineTo(info.Width, info.Height);
            path.LineTo(info.Width, (info.Height / 100) * 70); // move to this point

            for (int i = 1; i < 100; i++)
			{
                per = per - i / 95 ;
                path.LineTo(info.Width, (info.Height / 100) * per);
            }


            path.LineTo(0, (info.Height / 100) * 70); // end point

            path.Close(); // make sure path is closed
                          // draw the path with paint object
            canvas.DrawPath(path, strokePaint);
        }
    }
}