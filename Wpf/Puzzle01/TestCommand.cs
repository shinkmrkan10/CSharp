using System;
using System.Windows;
using System.Windows.Input;

namespace Puzzle01
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
        	MessageBox.Show("パズルにする画像は\n256画素×256画素を選んでください",
			 "画像の15パズル",MessageBoxButton.OK, MessageBoxImage.Information);
    	}
	}
}