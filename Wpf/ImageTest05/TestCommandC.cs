using System;
using System.Windows;
using System.Windows.Input;

namespace ImageTest05
{
	public class TestCommandC : ICommand
	{
    	public event EventHandler CanExecuteChanged;
 
	    public bool CanExecute(object parameter)
    	{
        	return true;
    	}
 
    	public void Execute(object parameter)
    	{
        	MessageBox.Show("フォルダを変更して\n処理する画像を変更可能です",
			 "画像の変更",MessageBoxButton.OK, MessageBoxImage.Information);
    	}
	}
}