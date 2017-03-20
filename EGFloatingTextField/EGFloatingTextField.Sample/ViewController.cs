//
// ViewController.cs
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
using UIKit;

namespace EGFloatingTextField.Sample
{
	public class ViewController : UIViewController
	{
		protected FloatingTextField UsernameTextField { get; private set; }

		protected FloatingTextField PasswordTextField { get; private set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			View.BackgroundColor = UIColor.White;

			UsernameTextField = new FloatingTextField
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Placeholder = "Username",
				ReturnKeyType = UIReturnKeyType.Next
			};

			UsernameTextField.ShouldReturn = _ => PasswordTextField.BecomeFirstResponder();

			PasswordTextField = new FloatingTextField
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Placeholder = "Password",
				SecureTextEntry = true,
				ReturnKeyType = UIReturnKeyType.Done
			};

			PasswordTextField.ShouldReturn = _ => PasswordTextField.ResignFirstResponder();

			View.AddSubviews(UsernameTextField, PasswordTextField);

			View.AddConstraints(new NSLayoutConstraint[]
			{
				UsernameTextField.TopAnchor.ConstraintEqualTo(View.TopAnchor, 128),
				UsernameTextField.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 16),
				UsernameTextField.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -16),
				UsernameTextField.HeightAnchor.ConstraintEqualTo(48),
				PasswordTextField.TopAnchor.ConstraintEqualTo(UsernameTextField.BottomAnchor, 16),
				PasswordTextField.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 16),
				PasswordTextField.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -16),
				PasswordTextField.HeightAnchor.ConstraintEqualTo(48)
			});
		}
	}
}

