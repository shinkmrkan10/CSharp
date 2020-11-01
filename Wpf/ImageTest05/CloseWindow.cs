using System;
using System.Windows;
using System.Windows.Input;

namespace ImageTest05
{
	public class CloseWindow : ICommand
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
				mainWindow.Close();
			}
    	}
	}
}