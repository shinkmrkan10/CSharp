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
        	MessageBox.Show("画像処理プログラムです");
    	}
	}
}