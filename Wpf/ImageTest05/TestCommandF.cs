using System;
using System.Windows;
using System.Windows.Input;

namespace ImageTest05
{
	public class TestCommandF : ICommand
	{
    	public event EventHandler CanExecuteChanged;
 
	    public bool CanExecute(object parameter)
    	{
        	return true;
    	}
 
    	public void Execute(object parameter)
    	{
        	MessageBox.Show("スライダーの設定で\nバンドパスフィルタ、\nノッチフィルタも処理可能です。",
			"画像のフィルタ処理",MessageBoxButton.OK, MessageBoxImage.Information);
    	}
	}
}