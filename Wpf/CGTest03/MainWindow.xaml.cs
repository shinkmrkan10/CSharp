using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CGTest03
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
    {
        public ObservableCollection<ShapeElement> DataS { get; set; }
        public ObservableCollection<ShapeElement> tempDataS { get; set; }
        public Dictionary<string, string> ColorDic { get; set; }
        public MainWindow()
        {
            ColorDic = new Dictionary<string, string>()
            {
                { "Red", "赤(Red)" },
                { "Green", "緑(Green)" },
                { "Blue", "青(Blue)" },
                { "Yellow", "黄(Yellow)" },
                { "Pink", "ピンク(Pink)" },
                { "Purple", "紫(Purple)" },
                { "Black", "黒(Black)" },
                { "Gray", "グレイ(Gray)" },
                { "White", "白(White)" },
            };
            InitializeComponent();
            DataS = new ObservableCollection<ShapeElement>();
            tempDataS = new ObservableCollection<ShapeElement>();
            dataGrid1.DataContext = DataS;
            comboBoxColor.DataContext = ColorDic;
            rectRadioButton.IsChecked = true;
            button_reset();
        }

        // 操作対象のエレメント名
        private string elementName;
        // 操作対象のエレメントの色
        private string elementColor;
        private byte r = 255;
        private byte g = 255;
        private byte b = 255;
        private byte a = 255;
        // 操作対象のエレメントのZIndex
        private int z = 0;
        // 操作対象のエレメントのTop
        private double elementTop = 0;
        // 操作対象のエレメントのLeft
        private double elementLeft = 0;
        // 操作対象のエレメントの高さ
        private string elementHeight = "100";
        // 操作対象のエレメントの幅
        private string elementWidth = "100";
        // 操作対象のエレメントの扁平率
        private string elementFlattening = "1.0";
        // 操作対象のエレメントのid
        private int count = 0;
        // エレメント修正中フラグ
        private bool _isReshape = false;
        // マウス押下中フラグ
        private bool _isMouseDown;
        // マウスの移動が開始されたときの座標
        private Point _startPoint;
        // マウスの現在位置座標
        private Point _currentPoint;
        // マウスが離れたときのイベントハンドラ
        private void OperationArea_MouseLeave(object sender, MouseEventArgs e)
        {
            _isMouseDown = false;

            e.Handled = true;
        }
        
        // マウス左ボタン押下イベントのイベントハンドラ
        private void OperationArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // フラグを"マウス押下中"にする
            _isMouseDown = true;

            // GetPositionメソッドで現在のマウス座標を取得し、マウス移動開始点を更新
            // （マウス座標は、OperationAreaからの相対的な位置とする）
            _startPoint = e.GetPosition(OperationArea);

            // イベントを処理済みとする（当イベントがこの先伝搬されるのを止めるため）
            e.Handled = true;
        }

        // マウス左ボタン解放イベントのハンドラ
        private void OperationArea_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // マウス押下中フラグを落とす
            _isMouseDown = false;

            e.Handled = true;
        }

        // マウス移動イベントのイベントハンドラ
        private void OperationArea_MouseMove(object sender, MouseEventArgs e)
		{
			// マウス押下中でなければドラッグ操作ではないのでメソッドを抜ける
			if (!_isMouseDown)
			{
				return;
			}

			// マウスの現在位置座標を取得（OperationAreaからの相対位置）
			_currentPoint = e.GetPosition(OperationArea);

			//移動開始点と現在位置の差から、MouseMoveイベント1回分の移動量を算出
			double offsetX = _currentPoint.X - _startPoint.X;
			double offsetY = _currentPoint.Y - _startPoint.Y;

            elementLeft += offsetX;
            elementTop += offsetY;
            textForm();
            button_moving();
			// 操作対象の図形をTargetに取得
			//            moveElement.Text = elementName;
			elementName = moveElement.Text;
            if(elementName.Substring(0,4) == "rect")
            {
    			var Target = (Rectangle)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
    			if (Target != null)
	    		{
		    		// 操作対象の図形からMatrixオブジェクトを取得
			    	// このMatrixオブジェクトを用いて図形を描画上移動させる
				    var matrix = ((MatrixTransform)Target.RenderTransform).Matrix;

    				// TranslateメソッドにX方向とY方向の移動量を渡し、移動後の状態を計算
	    			matrix.Translate(offsetX, offsetY);

		    		// 移動後の状態を計算したMatrixオブジェクトを描画に反映する
			    	Target.RenderTransform = new MatrixTransform(matrix);

				    // 移動開始点を現在位置で更新する
    				// （今回の現在位置が次回のMouseMoveイベントハンドラで使われる移動開始点となる）
	    			_startPoint = _currentPoint;

		    		e.Handled = true;
			    }
            }
            else
            {
			    var Target = (Ellipse)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
    			if (Target != null)
	    		{
		    		// 操作対象の図形からMatrixオブジェクトを取得
			    	// このMatrixオブジェクトを用いて図形を描画上移動させる
				    var matrix = ((MatrixTransform)Target.RenderTransform).Matrix;

    				// TranslateメソッドにX方向とY方向の移動量を渡し、移動後の状態を計算
	    			matrix.Translate(offsetX, offsetY);

		    		// 移動後の状態を計算したMatrixオブジェクトを描画に反映する
			    	Target.RenderTransform = new MatrixTransform(matrix);

				    // 移動開始点を現在位置で更新する
    				// （今回の現在位置が次回のMouseMoveイベントハンドラで使われる移動開始点となる）
	    			_startPoint = _currentPoint;

		    		e.Handled = true;
			    }
            }
		}
        // dataGrid1のnameでのマウスダブルクリックイベント処理
        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var elem = e.MouseDevice.DirectlyOver as FrameworkElement;
            if (elem != null)
            {
                DataGridCell cell = elem.Parent as DataGridCell;
              　if (cell == null)
                {
                     // ParentでDataGridCellが拾えなかった時はTemplatedParentを参照
                     // （Borderをダブルクリックした時）
                     cell = elem.TemplatedParent as DataGridCell;
                }
                if (cell != null)
                {
                    // ここでcellの内容を処理
                    // （cell.DataContextにバインドされたものが入っている）
                    var preText = "System.Windows.Controls.TextBox: ";
                    var cellInfo = dataGrid1.SelectedCells[0];
                    var cellContent = cellInfo.Column.GetCellContent(cellInfo.Item);
                    var cellText = cellContent.ToString();
                    moveElement.Text = cellText.Substring(preText.Length);
                    elementName = moveElement.Text;
                    var item1 = DataS.LastOrDefault(x => x.name == elementName);
		        	if (item1 != null)
        			{
                        moveWidth.Text = item1.width.ToString();
                        elementWidth = moveWidth.Text;
                        moveHeight.Text = item1.height.ToString();
                        elementHeight = moveHeight.Text;
                        textBoxLeft.Text = item1.x.ToString();
                        elementLeft = double.Parse(textBoxLeft.Text);
                        textBoxTop.Text = item1.y.ToString();
                        elementTop = double.Parse(textBoxTop.Text);
                        zIndex.Text = item1.z.ToString();
                        comboBoxColor.SelectedValue = item1.color;
                    }
                    //イベントをキャンセルします
                    e.Handled = true;
                }
            }
        }
        // dataGrid1をリフレッシュ
        private void dataRefresh(object sender, RoutedEventArgs e)
        {
            _dataRefresh();
        }
        private void _dataRefresh()
        {
            tempDataS.Clear();
            var rCount = DataS.Count;
            for (int row = 0; row < rCount; row++ )
            {
                var tempName = DataS[row].name;
                var same = true;
                for (int after = row + 1 ; after < rCount; after++)
                {
                    if(DataS[after].name == tempName || tempName == moveElement.Text)
                    {
                        same = false;
                        break;
                    }
                }
                if (same)
                {
                    tempDataS.Add(DataS[row]);
                }
            }
            DataS.Clear();
            foreach (ShapeElement dS in tempDataS){
                DataS.Add(dS);
            }
        }
        // dataGrid更新時のイベント（未利用）
        private void dataGrid1_SourceUpdated(object sender, EventArgs e)
        {
            var preText = "System.Windows.Controls.TextBox: ";
            var cellInfo = dataGrid1.SelectedCells[0];
            var cellContent = cellInfo.Column.GetCellContent(cellInfo.Item);
            var cellText = cellContent.ToString();
            moveElement.Text = cellText.Substring(preText.Length);
            elementName = moveElement.Text;

//                        foreach (ShapeElement dS in DataS){
//                            if (dS.name == elementName){
//                                DataS.Remove(dS); //NG
//                                moveElement.Text = ; // ok
//                                 moveElement.Text = dS2.color;  // ok
//                                dS2.color = "Black"; // NOP
//                                dS2.name = "rect9"; // NOP
//                            }
            var item1 = DataS.LastOrDefault(x => x.name == elementName);
			if (item1 != null)
			{
//                        moveElement.Text = "Changed"; // ok
//                        moveElement.Text = item1.color;  // ok
                zIndex.Text = item1.z.ToString();
						}
//                        }

        }
        // エレメントの削除
        private void deleteShape(object sender, RoutedEventArgs e)
        {
            elementName = moveElement.Text;
            foreach (ShapeElement dS in DataS){
        		if(elementName == dS.name){
                    if(dS.name.Substring(0,4) == "rect")
                    {
                        var Target = (Rectangle)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
                        canvas.Children.Remove(Target);
                        foreach (ShapeElement dS2 in DataS){
                            if (dS2.name == elementName){
                                var item = DataS.LastOrDefault(i => i.name == moveElement.Text);
                                dataGrid1.BeginEdit();
                                dataGrid1.CommitEdit();
                            }
                        }
                    }
                    else
                    {
                        var Target = (Ellipse)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
                        Target.Stroke=Brushes.Blue;
                        canvas.Children.Remove(Target);
                        foreach (ShapeElement dS2 in DataS){
                            if (dS2.name == elementName){
                                var item = DataS.LastOrDefault(i => i.name == moveElement.Text);
                                dataGrid1.BeginEdit();
                                dataGrid1.CommitEdit();
                            }
                        }
                    }
                }
            }
            tempDataS.Clear();
            var rCount = DataS.Count;
            for (int row = 0; row < rCount; row++ )
            {
                var tempName = DataS[row].name;
                    if(elementName == tempName)
                    {
                        continue;
                    }
                
                    tempDataS.Add(DataS[row]);
            }
            DataS.Clear();
            foreach (ShapeElement dS in tempDataS){
                DataS.Add(dS);
            }

//            count--;
        }
        // エレメントの更新
        private void updateShape(object sender, EventArgs e)
        {
            elementName = moveElement.Text;
            _isReshape = true;
//            shapeRadioButton();
/*            elementTop = double.Parse(textBoxTop.Text);
            elementLeft = double.Parse(textBoxLeft.Text);
            elementWidth = moveWidth.Text;
            elementHeight = moveHeight.Text;*/
            foreach (ShapeElement dS in DataS){
        		if(elementName == dS.name){
                    if(dS.name.Substring(0,4) == "rect")
                    {
                        var Target = (Rectangle)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
                        Target.Stroke=Brushes.Blue;
                        Target.StrokeThickness = 2;
                        zIndex.Text = dS.z.ToString();
                    }
                    else
                    {
                        var Target = (Ellipse)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
                        Target.Stroke=Brushes.Blue;
                        Target.StrokeThickness = 2;
                        zIndex.Text = dS.z.ToString();
                    }
                }
            }
//            dataRefresh();
            button_reshape();
            count--;
        }
        // 新規エレメントを登録し次のエレメント入力へ
        public void nextShape(object sender, RoutedEventArgs e)
        {
            textForm();
			// 操作対象の図形をTargetに取得
			elementName = moveElement.Text;
            if(elementName.Substring(0,4) == "rect")
            {
			    var Target = (Rectangle)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
    			if (Target != null)
	    		{
                    DataS.Add(new ShapeElement{
				        name = moveElement.Text,
    	    			x = double.Parse(textBoxLeft.Text), 
                        y = double.Parse(textBoxTop.Text), 
                        z = int.Parse(zIndex.Text), 
	    	    		width = double.Parse(moveWidth.Text), 
                        height = double.Parse(moveHeight.Text), 
		    	    	color = comboBoxColor.SelectedValue.ToString(), 
                        r = r, g = g, b = b, a = 0xFF
			        });
                    Canvas.SetZIndex(Target, int.Parse(zIndex.Text));
                    Target.Stroke = null;
                    addInit("rect");
                }
            }
            else
            {
			    var Target = (Ellipse)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
    			if (Target != null)
	    		{
                    DataS.Add(new ShapeElement{
				        name = moveElement.Text,
    	    			x = double.Parse(textBoxLeft.Text), 
                        y = double.Parse(textBoxTop.Text), 
                        z = int.Parse(zIndex.Text), 
	    	    		width = double.Parse(moveWidth.Text), 
                        height = double.Parse(moveHeight.Text), 
		    	    	color = comboBoxColor.SelectedValue.ToString(), 
                        r = r, g = g, b = b, a = 0xFF
			        });

                    Canvas.SetZIndex(Target, int.Parse(zIndex.Text));
                    Target.Stroke = null;
                    addInit("ellipse");
                }
            }
        }
        // 長方形と楕円の選択
        private void shapeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if(rectRadioButton.IsChecked == true)
            {
                moveElement.Text = "rect" + count;
            }
            else
            {
                moveElement.Text = "ellipse" + count;
            }
   			elementName = moveElement.Text;
        }
		// エレメントを追加
        private void addclick(object sender, RoutedEventArgs e)
        {
            elementTop = 0;
            elementLeft = 0;
            textForm();
            if(elementName.Substring(0,4) == "rect")
            {
    			var Target = (Rectangle)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
                var elementNew = new Rectangle();
                elementNew.Name = moveElement.Text;
                Canvas.SetTop(elementNew,elementTop);
                Canvas.SetLeft(elementNew,elementLeft);
                elementNew.Height = double.Parse(moveHeight.Text);
                elementNew.Width = double.Parse(moveHeight.Text) * double.Parse(moveFlattening.Text);
                elementNew.Stroke=Brushes.Blue;
                elementNew.StrokeThickness = 2;
                var mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = Color.FromArgb(255, 255, 0, 0);
                elementNew.Fill=mySolidColorBrush;
                z = count;
                zIndex.Text = z.ToString();
                canvas.Children.Add(elementNew);
                Canvas.SetZIndex(elementNew, z);
            }
            else
            {
    			var Target = (Ellipse)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
                var elementNew = new Ellipse();
                elementNew.Name = moveElement.Text;
                Canvas.SetTop(elementNew,elementTop);
                Canvas.SetLeft(elementNew,elementLeft);
                elementNew.Height = double.Parse(moveHeight.Text);
                elementNew.Width = double.Parse(moveHeight.Text) * double.Parse(moveFlattening.Text);
                elementNew.Stroke=Brushes.Blue;
                elementNew.StrokeThickness = 2;
                var mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = Color.FromArgb(255, 255, 0, 0);
                elementNew.Fill=mySolidColorBrush;
                z = count;
                zIndex.Text = z.ToString();
                canvas.Children.Add(elementNew);
                Canvas.SetZIndex(elementNew, z);
            }
        }
        // 新規エレメントを破棄
        private void delclick(object sender, RoutedEventArgs e)
        {
			elementName = moveElement.Text;
            if(elementName.Substring(0,4) == "rect")
            {
                var rec = (Rectangle)LogicalTreeHelper.FindLogicalNode(canvas, moveElement.Text);
                canvas.Children.Remove(element: rec);
            }
            else
            {
                var rec = (Ellipse)LogicalTreeHelper.FindLogicalNode(canvas, moveElement.Text);
                canvas.Children.Remove(element: rec);
            }
            if(_isReshape)
            {
                _dataRefresh();
            }
            button_reset();
        }

        // 左へ+1移動
        private void moveLeft(object sender, RoutedEventArgs e)
        {
			// 移動対象の図形をTargetに取得
			elementName = moveElement.Text;
            button_moving();
            if(elementName.Substring(0,4) == "rect")
            {
		     	var Target = (Rectangle)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
			    if (Target != null)
    			{
                    var left = Canvas.GetLeft(Target);
                    Canvas.SetLeft(Target,left - 1);
                    elementLeft -= 1;
                    textForm();
                }
            }
            else
            {
    			var Target = (Ellipse)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
	    		if (Target != null)
		    	{
                    var left = Canvas.GetLeft(Target);
                    Canvas.SetLeft(Target,left - 1);
                    elementLeft -= 1;
                    textForm();
                }
            }
        }
        // 左へ+10移動
        private void moveLeftLeft(object sender, RoutedEventArgs e)
        {
			// 移動対象の図形をTargetに取得
			elementName = moveElement.Text;
            button_moving();
            if(elementName.Substring(0,4) == "rect")
            {
		     	var Target = (Rectangle)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
			    if (Target != null)
    			{
                    var left = Canvas.GetLeft(Target);
                    Canvas.SetLeft(Target,left - 10);
                    elementLeft -= 10;
                    textForm();
                }
            }
            else
            {
    			var Target = (Ellipse)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
	    		if (Target != null)
		    	{
                    var left = Canvas.GetLeft(Target);
                    Canvas.SetLeft(Target,left - 10);
                    elementLeft -= 10;
                    textForm();
                }
            }
        }
        // 右へ+1移動
        private void moveRight(object sender, RoutedEventArgs e)
        {
			// 移動対象の図形をTargetに取得
			elementName = moveElement.Text;
            button_moving();
			if (elementName.Substring(0, 4) != "rect")
			{
				var Target = (Ellipse)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
				if (Target != null)
				{
					var left = Canvas.GetLeft(Target);
					Canvas.SetLeft(Target, left + 1);
                    elementLeft += 1;
                    textForm();
				}
			}
			else
			{
				var Target = (Rectangle)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
				if (Target != null)
				{
					var left = Canvas.GetLeft(Target);
					Canvas.SetLeft(Target, left + 1);
                    elementLeft += 1;
                    textForm();
				}
			}
		}
        // 右へ+10移動
        private void moveRightRight(object sender, RoutedEventArgs e)
        {
			// 移動対象の図形をTargetに取得
			elementName = moveElement.Text;
            button_moving();
			if (elementName.Substring(0, 4) != "rect")
			{
				var Target = (Ellipse)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
				if (Target != null)
				{
					var left = Canvas.GetLeft(Target);
					Canvas.SetLeft(Target, left + 10);
                    elementLeft += 10;
                    textForm();
				}
			}
			else
			{
				var Target = (Rectangle)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
				if (Target != null)
				{
					var left = Canvas.GetLeft(Target);
					Canvas.SetLeft(Target, left + 10);
                    elementLeft += 10;
                    textForm();
				}
			}
		}
        // 上へ+1移動
        private void moveUp(object sender, RoutedEventArgs e)//上へ+1移動
        {
			// 移動対象の図形をTargetに取得
			elementName = moveElement.Text;
            button_moving();
            if(elementName.Substring(0,4) == "rect")
            {
		     	var Target = (Rectangle)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
			    if (Target != null)
    			{
                    var top = Canvas.GetTop(Target);
                    Canvas.SetTop(Target,top - 1);
                    elementTop -= 1;
                    textForm();
                }
            }
            else
            {
    			var Target = (Ellipse)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
	    		if (Target != null)
		    	{
                    var top = Canvas.GetTop(Target);
                    Canvas.SetTop(Target,top - 1);
                    elementTop -= 1;
                    textForm();
                }
            }
        }
        // 上へ+10移動
        private void moveUpUp(object sender, RoutedEventArgs e)
        {
			// 移動対象の図形をTargetに取得
			elementName = moveElement.Text;
            button_moving();
            if(elementName.Substring(0,4) == "rect")
            {
		     	var Target = (Rectangle)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
			    if (Target != null)
    			{
                    var top = Canvas.GetTop(Target);
                    Canvas.SetTop(Target,top - 10);
                    elementTop -= 10;
                    textForm();
                }
            }
            else
            {
    			var Target = (Ellipse)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
	    		if (Target != null)
		    	{
                    var top = Canvas.GetTop(Target);
                    Canvas.SetTop(Target,top - 10);
                    elementTop -= 10;
                    textForm();
                }
            }
        }
        // 下へ+1移動
        private void moveDown(object sender, RoutedEventArgs e)
        {
			// 移動対象の図形をTargetに取得
			elementName = moveElement.Text;
            button_moving();
            if(elementName.Substring(0,4) == "rect")
            {
		     	var Target = (Rectangle)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
			    if (Target != null)
    			{
                    var top = Canvas.GetTop(Target);
                    Canvas.SetTop(Target,top + 1);
                    elementTop += 1;
                    textForm();
                }
            }
            else
            {
    			var Target = (Ellipse)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
	    		if (Target != null)
		    	{
                    var top = Canvas.GetTop(Target);
                    Canvas.SetTop(Target,top + 1);
                    elementTop += 1;
                    textForm();
                }
            }
        }
        // 下へ+10移動
        private void moveDownDown(object sender, RoutedEventArgs e)
        {
			// 移動対象の図形をTargetに取得
			elementName = moveElement.Text;
            button_moving();
            if(elementName.Substring(0,4) == "rect")
            {
		     	var Target = (Rectangle)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
			    if (Target != null)
    			{
                    var top = Canvas.GetTop(Target);
                    Canvas.SetTop(Target,top + 10);
                    elementTop += 10;
                    textForm();
                }
            }
            else
            {
    			var Target = (Ellipse)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
	    		if (Target != null)
		    	{
                    var top = Canvas.GetTop(Target);
                    Canvas.SetTop(Target,top + 10);
                    elementTop += 10;
                    textForm();
                }
            }
        }
        // 大きさ変更
        private void sizeChange(object sender, RoutedEventArgs e)
        {
			// 操作対象の図形をTargetに取得
			elementName = moveElement.Text;
            button_moving();
            if(elementName.Substring(0,4) == "rect")
            {
    			var Target = (Rectangle)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
	    		if (Target != null)
		    	{
                    elementHeight = moveHeight.Text;
                    elementWidth = moveWidth.Text;
                    Target.Height = double.Parse(elementHeight);
                    Target.Width = double.Parse(elementWidth);
                    var x =(double.Parse(elementWidth) / double.Parse(elementHeight));
                    moveFlattening.Text = x.ToString();
                }
            }
            else
            {
    			var Target = (Ellipse)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
	    		if (Target != null)
		    	{
                    elementHeight = moveHeight.Text;
                    elementWidth = moveWidth.Text;
                    Target.Height = double.Parse(elementHeight);
                    Target.Width = double.Parse(elementWidth);
                    var x =(double.Parse(elementWidth) / double.Parse(elementHeight));
                    moveFlattening.Text = x.ToString();
                }
            }
        }
        // 扁平率変更
        private void flutteningChange(object sender, RoutedEventArgs e)
        {
			// 操作対象の図形をTargetに取得
			elementName = moveElement.Text;
            button_moving();
            if(elementName.Substring(0,4) == "rect")
            {
    			var Target = (Rectangle)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
	    		if (Target != null)
		    	{
                    elementWidth = moveWidth.Text;
                    elementFlattening = moveFlattening.Text;
                    var x =(double.Parse(elementWidth) / double.Parse(elementFlattening));
                    elementHeight = x.ToString();
                    moveHeight.Text = elementHeight;
                    Target.Height = double.Parse(elementHeight);
                }
            }
            else
            {
    			var Target = (Ellipse)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
	    		if (Target != null)
		    	{
                    elementWidth = moveWidth.Text;
                    elementFlattening = moveFlattening.Text;
                    var x =(double.Parse(elementWidth) / double.Parse(elementFlattening));
                    elementHeight = x.ToString();
                    moveHeight.Text = elementHeight;
                    Target.Height = double.Parse(elementHeight);
                }
            }
        }
        // textBox表示桁数調整
        private void textForm()
        {
            textBoxTop.Text = elementTop.ToString();
            textBoxLeft.Text = elementLeft.ToString();
            try
            {
                string textL = textBoxLeft.Text.Substring(0, 5);
                textBoxLeft.Text = textL;
            }
            catch (ArgumentOutOfRangeException e)
            {
                var eM = e.Message; // dummy
            }
            try
            {
                string textT = textBoxTop.Text.Substring(0, 5); 
                textBoxTop.Text = textT;
            }
            catch (ArgumentOutOfRangeException e)
            {
                var eM = e.Message; // dummy
            }
        }
        // エレメント入力初期化
        private void addInit(string elementName)
        {
                    count++;
//                    rectRadioButton.IsChecked = true;
                    moveElement.Text = elementName+count;
                    button_reset();
                    comboBoxColor.SelectedIndex = 0;
                    moveWidth.Text = "100";
                    moveHeight.Text = "100";
                    moveFlattening.Text = "1.0";
                    elementTop = 0;
                    elementLeft = 0;
                    textBoxTop.Text = elementTop.ToString();
                    textBoxLeft.Text = elementLeft.ToString();
        }
        // ボタンカラー初期値
        private void button_reset()
        {
                    button1.Background = Brushes.Orange;
                    buttonNext.Background = Brushes.LightGray;
                    button2.Background = Brushes.LightGray;
        }
        // ボタンカラー形状変更時
        private void button_reshape()
        {
                    button1.Background = Brushes.LightGray;
                    buttonNext.Background = Brushes.Orange;
                    button2.Background = Brushes.LightGray;
        }
        // ボタンカラー移動中
        private void button_moving()
        {
                    button1.Background = Brushes.LightGray;
                    buttonNext.Background = Brushes.Orange;
                    button2.Background = Brushes.Orange;
        }
        // カラー選択comboBox設定
        private void comboBoxColor_SelectedValueChanged(object sender, EventArgs e) 
        {
            elementColor = comboBoxColor.SelectedValue.ToString();
            switch(elementColor){
                case "Red":
                    r = 255;
                    g = 0;
                    b = 0;
                    break;
                case "Green":
                    r = 0;
                    g = 255;
                    b = 0;
                    break;
                case "Blue":
                    r = 0;
                    g = 0;
                    b = 255;
                    break;
                case "Yellow":
                    r = 255;
                    g = 255;
                    b = 0;
                    break;
                case "Pink":
                    r = 0xff;
                    g = 0xc0;
                    b = 0xcb;
                    break;
                case "Purple":
                    r = 0x80;
                    g = 0x00;
                    b = 0x80;
                    break;
                case "Black":
                    r = 0;
                    g = 0;
                    b = 0;
                    break;
                case "Gray":
                    r = 0x80;
                    g = 0x80;
                    b = 0x80;
                    break;
                default: // White
                    r = 255;
                    g = 255;
                    b = 255;
                    break;
            }
            // 操作対象の図形をTargetに取得
			elementName = moveElement.Text;
            if(elementName.Substring(0,4) == "rect")
            {
    			var Target = (Rectangle)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
	    		if (Target != null)
		    	{
                    var mySolidColorBrush = new SolidColorBrush();
                    mySolidColorBrush.Color = Color.FromArgb(a, r, g, b);
                    Target.Fill=mySolidColorBrush;
                }
            }
            else
            {
	    		var Target = (Ellipse)LogicalTreeHelper.FindLogicalNode(canvas, elementName);
		    	if (Target != null)
			    {
                    var mySolidColorBrush = new SolidColorBrush();
                    mySolidColorBrush.Color = Color.FromArgb(a, r, g, b);
                    Target.Fill=mySolidColorBrush;
                }
            }
        }
    }
}
