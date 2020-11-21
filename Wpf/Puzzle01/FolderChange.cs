using System;
using System.Windows;
using System.Windows.Input;

namespace Puzzle01
{
	public class FolderChange : ICommand
	{
    	public event EventHandler CanExecuteChanged;
 
	    public bool CanExecute(object parameter)
    	{
        	return true;
    	}
 
    	public void Execute(object parameter)
    	{
			var mainWindow = (Application.Current.MainWindow as MainWindow);
			if (mainWindow != null)
			{
				var sender = new Object();
				var e = new RoutedEventArgs();
				mainWindow.folder_Change(sender, e);
			}
    	}
	}
}