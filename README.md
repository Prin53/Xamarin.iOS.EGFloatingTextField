[![NuGet version](https://badge.fury.io/nu/Prin53.Xamarin.iOS.EGFloatingTextField.svg)](https://badge.fury.io/nu/Prin53.Xamarin.iOS.EGFloatingTextField)
## Xamarin.iOS port for [EGFloatingTextField](https://github.com/enisgayretli/EGFloatingTextField) Swift library
## Usage Example
```cs
var textField = new FloatingTextField
{
	TranslatesAutoresizingMaskIntoConstraints = false,
	Placeholder = "Placeholder"
};

View.AddSubview(textField);

View.AddConstraints(new NSLayoutConstraint[]
{
	textField.TopAnchor.ConstraintEqualTo(View.TopAnchor, 128),
    textField.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 16),
	textField.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -16),
	textField.HeightAnchor.ConstraintEqualTo(48)
});
```
## Custom Usage Example
```cs
var textField = new FloatingTextField
{
	Placeholder = "Placeholder",
	Font = Fonts.AppFont,
    TextColor = Palette.AppColor,
    LineColor = Palette.LineColor,
    LineHeight = 1,
    LineShouldHide = false,
    LabelActiveTextColor = Palette.ActiveColor,
    LabelInactiveTextColor = Palette.InactiveColor
};
```