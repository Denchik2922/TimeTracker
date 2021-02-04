﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeTracker.Droid;
using TimeTracker.ExtraClass;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CardView), typeof(MyFrameRender))]
namespace TimeTracker.Droid
{
	class MyFrameRender : FrameRenderer
	{
		public MyFrameRender(Context context) : base(context)
		{

		}

		protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
		{
			base.OnElementChanged(e);
			var element = e.NewElement as CardView;
			if (element == null) return;
			if (element.HasShadow)
			{
				Elevation = 30.0f;
				TranslationZ = 0.0f;
				SetZ(30f);
			}
		}
	}
}