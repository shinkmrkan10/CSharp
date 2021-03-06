using System;
using System.Windows;
using System.Windows.Input;

namespace ImageTest05
{
	public class TestCommand : ICommand
	{
    	public event EventHandler CanExecuteChanged;
 
	    public bool CanExecute(object parameter)
    	{
        	return true;
    	}
 
    	public void Execute(object parameter)
    	{
        	MessageBox.Show("処理する画像を変更すると\nそれまでのパラメータが引き継がれます",
			 "画像の階調処理",MessageBoxButton.OK, MessageBoxImage.Information);
    	}
	}
}