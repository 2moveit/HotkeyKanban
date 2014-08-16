using System;
using System.Windows;
using System.Windows.Input;
using KCT.HotkeyAPI;

namespace KCT
{
	/// <summary>
	/// 	Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
	    public MainWindow()
		{
		    InitializeComponent();
			Loaded += (s, e) =>
			{
			    var hotkey = new Hotkey(ModifierKeys.Alt, Keys.Space, this);
			    hotkey.HotkeyPressed += (k) =>
			          		    {
                                    WindowState = WindowState.Normal;
                                    Activate();
			          		        Console.Beep();
			          		    };
			};
		}
	}
}