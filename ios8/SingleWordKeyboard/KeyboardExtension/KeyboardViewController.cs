﻿using System;

using ObjCRuntime;
using Foundation;
using UIKit;

namespace KeyboardExtension
{
	public partial class KeyboardViewController : UIInputViewController
	{
		const string SingleWord = "SingleWord";

		UIButton nextKeyboardButton;
		UIButton mainButton;

		public KeyboardViewController (IntPtr handle)
			: base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform custom UI setup here
			SetupMainButton ();
			SetupNextKeyboardButton ();
		}

		void SetupNextKeyboardButton()
		{
			nextKeyboardButton = new UIButton (UIButtonType.System);

			nextKeyboardButton.SetTitle ("Next Keyboard", UIControlState.Normal);
			nextKeyboardButton.SizeToFit ();
			nextKeyboardButton.TranslatesAutoresizingMaskIntoConstraints = false;

			nextKeyboardButton.TouchUpInside += OnNextKeyboard;

			View.AddSubview (nextKeyboardButton);

			var nextKeyboardButtonLeftSideConstraint = NSLayoutConstraint.Create (nextKeyboardButton, NSLayoutAttribute.Left, NSLayoutRelation.Equal, View, NSLayoutAttribute.Left, 1, 10);
			var nextKeyboardButtonBottomConstraint = NSLayoutConstraint.Create (nextKeyboardButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, View, NSLayoutAttribute.Bottom, 1, 0);
			View.AddConstraints (new [] {
				nextKeyboardButtonLeftSideConstraint,
				nextKeyboardButtonBottomConstraint
			});
		}

		void OnNextKeyboard (object sender, EventArgs e)
		{
			AdvanceToNextInputMode ();
		}

		void SetupMainButton ()
		{
			mainButton = new UIButton (UIButtonType.System);
			var fontSize = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad ? 80 : 60;
			mainButton.TitleLabel.Font = UIFont.SystemFontOfSize (fontSize);

			mainButton.SetTitle (SingleWord, UIControlState.Normal);
			mainButton.SizeToFit ();
			mainButton.TranslatesAutoresizingMaskIntoConstraints = false;

			mainButton.TouchUpInside += PrintWord;

			View.AddSubview (mainButton);

			var mainButtonCenterXSideConstraint = NSLayoutConstraint.Create (mainButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, View, NSLayoutAttribute.CenterX, 1.0f, 0.0f);
			var mainButtonCenterYsConstraint = NSLayoutConstraint.Create (mainButton, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, View, NSLayoutAttribute.CenterY, 1.0f, 0.0f);
			View.AddConstraints (new [] {
				mainButtonCenterXSideConstraint,
				mainButtonCenterYsConstraint
			});
		}

		void PrintWord (object sender, EventArgs e)
		{
			TextDocumentProxy.InsertText (SingleWord);
		}

		public override void TextWillChange (NSObject textInput)
		{
			// The app is about to change the document's contents. Perform any preparation here.
		}

		public override void TextDidChange (NSObject textInput)
		{
			// The app has just changed the document's contents, the document context has been updated.
			var isDark = TextDocumentProxy.KeyboardAppearance == UIKeyboardAppearance.Dark;
			UIColor textColor = isDark ? UIColor.White : UIColor.Black;

			nextKeyboardButton.SetTitleColor (textColor, UIControlState.Normal);
		}
	}
}