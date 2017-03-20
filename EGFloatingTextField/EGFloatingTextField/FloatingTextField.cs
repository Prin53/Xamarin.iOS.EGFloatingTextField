//
// FloatingTextField.cs
//
// Author:
//       Denys Fiediaiev <prineduard@gmail.com>
//
// Copyright (c) 2017 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace EGFloatingTextField
{
	[Register("FloatingTextField")]
	public class FloatingTextField : UITextField
	{
		protected bool Active
		{
			get;
			set;
		}

		protected bool Floating
		{
			get;
			set;
		}

		public bool FloatingLabelEnabled
		{
			get;
			set;
		}

		#region Label

		private UIColor _labelInactiveTextColor = UIColor.FromWhiteAlpha(0, .54f);
		private UIColor _labelActiveColor = UIColor.Blue;

		public UILabel Label
		{
			get;
			private set;
		}

		public UIColor LabelActiveTextColor
		{
			get
			{
				return _labelActiveColor;
			}
			set
			{
				_labelActiveColor = value;

				if (Floating)
				{
					Label.TextColor = _labelActiveColor;
				}
			}
		}

		public UIColor LabelInactiveTextColor
		{
			get
			{
				return _labelInactiveTextColor;
			}
			set
			{
				_labelInactiveTextColor = value;

				if (!Floating)
				{
					Label.TextColor = _labelInactiveTextColor;
				}
			}
		}

		public override string Placeholder
		{
			get
			{
				return Label.Text;
			}
			set
			{
				Label.Text = value;
			}
		}

		#endregion

		#region Line

		private UIColor _lineColor = UIColor.Blue;
		private NSLayoutConstraint _lineHeightConstraint;
		private nfloat _lineHeight = 2;
		private bool _lineShouldHide = true;

		protected UIView Line
		{
			get;
			set;
		}

		public bool LineShouldHide
		{
			get
			{
				return _lineShouldHide;
			}
			set
			{
				_lineShouldHide = value;

				if (!_lineShouldHide)
				{
					Line.Layer.Opacity = 1;
				}
			}
		}

		public nfloat LineHeight
		{
			get
			{
				return _lineHeight;
			}
			set
			{
				_lineHeight = value;

				_lineHeightConstraint.Constant = _lineHeight;
			}
		}

		public UIColor LineColor
		{
			get
			{
				return _lineColor;
			}
			set
			{
				_lineColor = value;

				Line.BackgroundColor = _lineColor;
			}
		}

		#endregion

		public FloatingTextField()
		{
			Initialize();
		}

		public FloatingTextField(CGRect frame) : base(frame)
		{
			Initialize();
		}

		public FloatingTextField(IntPtr handle) : base(handle)
		{
			Initialize();
		}

		protected void Initialize()
		{
			FloatingLabelEnabled = true;

			Label = new UILabel
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				TextColor = LabelInactiveTextColor,
				TextAlignment = UITextAlignment.Left,
				Lines = 1,
				ClipsToBounds = false
			};

			AddSubview(Label);

			Line = new UIView
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				BackgroundColor = LineColor
			};

			Line.Layer.Opacity = 0;

			AddSubview(Line);

			AddConstraints(new NSLayoutConstraint[]
			{
				Label.CenterYAnchor.ConstraintEqualTo(CenterYAnchor),
				Label.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
				Label.WidthAnchor.ConstraintEqualTo(WidthAnchor),
				Label.HeightAnchor.ConstraintEqualTo(HeightAnchor),
				Line.BottomAnchor.ConstraintEqualTo(BottomAnchor),
				Line.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
				Line.TrailingAnchor.ConstraintEqualTo(TrailingAnchor),
				_lineHeightConstraint = Line.HeightAnchor.ConstraintEqualTo(_lineHeight)
			});
		}

		public override bool BecomeFirstResponder()
		{
			var result = base.BecomeFirstResponder();

			if (result && !Active)
			{
				if (FloatingLabelEnabled)
				{
					if (!Floating || string.IsNullOrEmpty(Text))
					{
						FloatLabelTop();
						Floating = true;
					}
				}
				else
				{
					Label.TextColor = LabelActiveTextColor;
					Label.Layer.Opacity = 0;
				}

				Label.TextColor = LabelActiveTextColor;
				if (LineShouldHide)
				{
					ShowActiveBorder();
				}
				Active = true;
			}

			return result;
		}

		public override bool ResignFirstResponder()
		{
			var result = base.ResignFirstResponder();

			if (result && Active)
			{
				if (FloatingLabelEnabled)
				{
					if (Floating && string.IsNullOrEmpty(Text))
					{
						FloatLabelBack();
						Floating = false;
					}
				}
				else
				{
					if (string.IsNullOrEmpty(Text))
					{
						Label.Layer.Opacity = 1;
					}
				}

				Label.TextColor = LabelInactiveTextColor;
				if (LineShouldHide)
				{
					ShowInactiveBorder();
				}
				Active = false;
			}

			return result;
		}

		public override void DrawRect(CGRect area, UIViewPrintFormatter formatter)
		{
			base.DrawRect(area, formatter);

			var textRect = TextRect(area);
			var context = UIGraphics.GetCurrentContext();
			var borderlines = new CGPoint[]
			{
				new CGPoint(0, textRect.Height - 1),
				new CGPoint(textRect.Width, textRect.Height - 1)
			};

			context.BeginPath();
			context.AddLines(borderlines);
			context.SetLineWidth(1);
			if (Enabled)
			{
				context.SetLineDash(0, new nfloat[] { 2, 4 });
			}
			context.SetStrokeColor(LineColor.CGColor);
			context.StrokePath();
		}

		protected void FloatLabelTop()
		{
			CATransaction.Begin();
			CATransaction.CompletionBlock = () =>
			{
				Label.TextColor = LabelActiveTextColor;
			};

			var animation = CABasicAnimation.FromKeyPath("transform");
			var fromTransform = CATransform3D.MakeScale(1, 1, 1);
			var toTransform = CATransform3D.MakeScale(.5f, .5f, 1);

			toTransform = toTransform.Translate(-Label.Frame.Width * 0.5f, -Label.Frame.Height, 0);

			animation.From = NSValue.FromCATransform3D(fromTransform);
			animation.To = NSValue.FromCATransform3D(toTransform);
			animation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseOut);

			var animationGroup = new CAAnimationGroup
			{
				Animations = new CAAnimation[]
				{
					animation
				},
				Duration = .3f,
				FillMode = CAFillMode.Forwards,
				RemovedOnCompletion = false
			};

			Label.Layer.AddAnimation(animationGroup, "_floatLabelTop");

			ClipsToBounds = false;

			CATransaction.Commit();
		}

		protected void FloatLabelBack()
		{
			CATransaction.Begin();
			CATransaction.CompletionBlock = () =>
			{
				Label.TextColor = LabelInactiveTextColor;
			};

			var animation = CABasicAnimation.FromKeyPath("transform");
			var fromTransform = CATransform3D.MakeScale(.5f, .5f, 1);
			var toTransform = CATransform3D.MakeScale(1, 1, 1);

			fromTransform = fromTransform.Translate(-Label.Frame.Width * 0.5f, -Label.Frame.Height, 0);

			animation.From = NSValue.FromCATransform3D(fromTransform);
			animation.To = NSValue.FromCATransform3D(toTransform);
			animation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseOut);

			var animationGroup = new CAAnimationGroup
			{
				Animations = new CAAnimation[]
				{
					animation
				},
				Duration = .3f,
				FillMode = CAFillMode.Forwards,
				RemovedOnCompletion = false
			};

			Label.Layer.AddAnimation(animationGroup, "_floatLabelBack");

			ClipsToBounds = false;

			CATransaction.Commit();
		}

		protected void ShowActiveBorder()
		{
			Line.Layer.Transform = CATransform3D.MakeScale(.01f, 1, 1);
			Line.Layer.Opacity = 1;

			CATransaction.Begin();

			Line.Layer.Transform = CATransform3D.MakeScale(.01f, 1, 1);

			var animation = CABasicAnimation.FromKeyPath("transform");
			var fromTransform = CATransform3D.MakeScale(.01f, 1, 1);
			var toTransform = CATransform3D.MakeScale(1, 1, 1);

			animation.From = NSValue.FromCATransform3D(fromTransform);
			animation.To = NSValue.FromCATransform3D(toTransform);
			animation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseOut);
			animation.FillMode = CAFillMode.Forwards;
			animation.RemovedOnCompletion = false;

			Line.Layer.AddAnimation(animation, "_activeBorder");

			CATransaction.Commit();
		}

		protected void ShowInactiveBorder()
		{
			CATransaction.Begin();
			CATransaction.CompletionBlock = () =>
			{
				Line.Layer.Opacity = 0;
			};

			var animation = CABasicAnimation.FromKeyPath("transform");
			var fromTransform = CATransform3D.MakeScale(1, 1, 1);
			var toTransform = CATransform3D.MakeScale(0.01f, 1, 1);

			animation.From = NSValue.FromCATransform3D(fromTransform);
			animation.To = NSValue.FromCATransform3D(toTransform);
			animation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseOut);
			animation.FillMode = CAFillMode.Forwards;
			animation.RemovedOnCompletion = false;

			Line.Layer.AddAnimation(animation, "_activeBorder");

			CATransaction.Commit();
		}
	}
}

