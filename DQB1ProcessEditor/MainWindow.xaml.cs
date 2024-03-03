using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DQB1ProcessEditor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private KeyboardHook mHook = new KeyboardHook();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			mHook.KeyDownEvent += MHook_KeyDownEvent;
			mHook.Hook();
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			mHook.UnHook();
		}

		private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var info = (sender as ListBox)?.SelectedItem as ItemInfo;
			if (info == null) return;

			var vm = DataContext as ViewModel;
			if (vm == null) return;

			vm.AppendItem(info);
		}

		private void MHook_KeyDownEvent(int keyCode)
		{
			var vm = DataContext as ViewModel;
			if (vm == null) return;

			vm.KeyboardAction(keyCode);
		}
	}
}