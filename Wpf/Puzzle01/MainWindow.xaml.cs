using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Puzzle01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TestCommand TestCmd { get; set; }
        public FolderChange folderChange { get; set; }
        public CloseWindow closeWindow { get; set; }
        public FormatConvertedBitmap bitmap = new FormatConvertedBitmap();
        private const string OpenFileType = "Image Files(*.bmp;*.jpg;*.gif;*.png)|*.bmp;*.jpg;*.gif;*.png|All files (*.*)|*.*";
		private string filename = "C:/Image/AAcolorBar.png";
        public static string foldername = @"C:\Image\"; 
        // canvas表示のoffset値
        const int OFFSET = 18;
        // 縦、横のタイル数
        const int SIZE =4;
        // 横のタイル数
        const int lengthOfX = SIZE;
        // 縦のタイル数
        const int lengthOfY = SIZE;
        // タイルの横pixel数
        const int WIDTH = 64;
        // タイルの縦pixel数
		const int HEIGHT = 64;
        ObservableCollection<Tile> DataT { get; set; }
        ObservableCollection<Tile> tempDataT { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            DataT = new ObservableCollection<Tile>();
            tempDataT = new ObservableCollection<Tile>();
            TestCmd = new TestCommand();
            folderChange = new FolderChange();
            closeWindow = new CloseWindow();
            DataContext = this;
//            PuzzleElement DataPE = new PuzzleElement();
            listViewDataT.DataContext = DataT;
            textBlock2.Text = "　　座標";
            init_tileList(lengthOfY, lengthOfX);
//            dataGrid1.DataContext = DataT;
            name_display();
            add_rectangle();
        }
        Random r1 = new System.Random();
        // 空白のタイル位置
        private int blankX = 4;
        private int blankY = 3;
        // 開始フラグ
        private bool _isNotStarted = false;
        // 完了フラグ
        private bool _isFinished = false;
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

            //  OFFSET-1 + y * (HEIGHT+1)
            int mouseX = -1;
            int mouseY = -1;
            if( OFFSET -1 < _startPoint.X && _startPoint.X < OFFSET + WIDTH )
            {
                mouseX = 0;
            }
            else if( OFFSET + WIDTH + 1 < _startPoint.X && _startPoint.X < OFFSET + 2 * (WIDTH +1) )
            {
                mouseX = 1;
            }
            else if( OFFSET + (WIDTH + 1) * 2 < _startPoint.X && _startPoint.X < OFFSET + 3 * (WIDTH +1) )
            {
                mouseX = 2;
            }
            else if( OFFSET + (WIDTH + 1) * 3 < _startPoint.X && _startPoint.X < OFFSET + 4 * (WIDTH +1) )
            {
                mouseX = 3;
            }
            if( OFFSET -1 < _startPoint.Y && _startPoint.Y < OFFSET + HEIGHT )
            {
                mouseY = 0;
            } 
            else if( OFFSET + HEIGHT + 1 < _startPoint.Y && _startPoint.Y < OFFSET + 2 * (HEIGHT +1) )
            {
                mouseY = 1;
            }
            else if( OFFSET + (HEIGHT + 1) * 2 < _startPoint.Y && _startPoint.Y < OFFSET + 3 * (HEIGHT +1) )
            {
                mouseY = 2;
            }
            else if( OFFSET + (HEIGHT + 1) * 3 < _startPoint.Y && _startPoint.Y < OFFSET + 4 * (HEIGHT +1) )
            {
                mouseY = 3;
            }
            string positionName = "　　座標" + mouseY + mouseX;
            textBlock2.Text = positionName;

            if(blankX == mouseX && blankY > mouseY)
            {
                tile_moveDown();
            }
            else if(blankX == mouseX && blankY < mouseY)
            {
                tile_moveUp();
            }
            else if(blankY == mouseY && blankX < mouseX)
            {
                tile_moveLeft();
            }
            else if(blankY == mouseY && blankX > mouseX)
            {
                tile_moveRight();
            }
            isFinished();
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
		}

        // Tile List (DataT) の初期化
        private void init_tileList(int lengthOfY, int lengthOfX)
        {
//            int number = 1;
			for(int y = 0; y < lengthOfY; y++)
			{
				for(int x = 0; x < lengthOfX; x++)
				{
					DataT.Add(new Tile{
						name = String.Format("tile{0}{1}", y, x ),
						originalX = x,
						originalY = y,
						x = x,
						y = y
					});
				}
			}
        }
        // tile name の表示
        private void name_display()
        {
            for(int y =0; y < 1; y++)
            {
                for(int x = 0; x < 1; x++)
                {
                    int n = WIDTH * y + x +1;
                    string t_name = "tile"+n;
                    string y_x ="textBlock"+y+x;
                    var tBy_x = new TextBlock();
                    tBy_x.Name = y_x;
                    tBy_x.Text = "test";
                    Tile tElement = DataT.FirstOrDefault(t => t.name == t_name); 
                }
            }
        }
        // canvasへのTileの表示
        private void set_tile(string rectName, int y, int x)
        {
            // Add a Rectangle Element
            var Target = (Rectangle)LogicalTreeHelper.FindLogicalNode(canvas, rectName);
            var elementNew = new Rectangle();
            elementNew.Name = rectName;
            Canvas.SetTop(elementNew, OFFSET-1 + y * (HEIGHT+1));
            Canvas.SetLeft(elementNew, OFFSET-1 + x * (WIDTH+1));
            elementNew.Stroke = System.Windows.Media.Brushes.Yellow;
            elementNew.Fill = System.Windows.Media.Brushes.SkyBlue;
            elementNew.HorizontalAlignment = HorizontalAlignment.Left;
            elementNew.VerticalAlignment = VerticalAlignment.Center;
            elementNew.Height = HEIGHT+1;
            elementNew.Width = WIDTH+1;
            canvas.Children.Add(elementNew);
        }
        public void folder_Change(object sender, RoutedEventArgs e){
            //            var fileContent = string.Empty;
            textBlock2.Text = "座標";
            var filePath = string.Empty;
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "ファイルを開く";
            openFileDialog.InitialDirectory = foldername;
            openFileDialog.FilterIndex = 1;
            openFileDialog.Filter = OpenFileType;

            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
                filename = openFileDialog.FileName;
                filename1.Text = filename;
                tile_deleteAll();
                init_tileList(lengthOfY, lengthOfX);
                blankX = 4;
                blankY = 3;
                _isNotStarted = true;
                _isFinished = false;
                _dataRefresh();
                Create_ImageArray();
            }
        }
        private void game_start(object sender, RoutedEventArgs e)
        {
            if(_isNotStarted)
            {
                _isNotStarted = false;
                tile_hide("tile33");
                blankX = 3;
                blankY = 3;
                // タイル移動
                for(int n = 0; n < 1000; n++)
                {
                    int r = r1.Next(0, 4);
                    switch(r){
                        case 0:
                            tile_moveDown();
                            break;
                        case 1:
                            tile_moveUp();
                            break;
                        case 2:
                            tile_moveRight();
                            break;
                        case 3:
                            tile_moveLeft();
                            break;
                    }
                }
            }
        }
        private void tile_moveUp()
        {
            if(blankY != 3)
            {
            foreach (Tile t in DataT){
    			if(t.x == blankX && t.y == blankY + 1){
                    string imageName = t.name;
                    var Target = (Image)LogicalTreeHelper.FindLogicalNode(canvas, imageName);
                    var left = Canvas.GetLeft(Target);
                    var top = Canvas.GetTop(Target);
                    Canvas.SetLeft(Target, left);
                    Canvas.SetTop(Target, top - 65);
                    t.y--;
                    blankY++;
                    break;
                }
             }
             _dataRefresh();
            }
        }
        private void tile_moveDown()
        {
            if(blankY != 0)
            {
            foreach (Tile t in DataT){
    			if(t.x == blankX && t.y == blankY - 1){
                    string imageName = t.name;
                    var Target = (Image)LogicalTreeHelper.FindLogicalNode(canvas, imageName);
                    var left = Canvas.GetLeft(Target);
                    var top = Canvas.GetTop(Target);
                    Canvas.SetLeft(Target, left);
                    Canvas.SetTop(Target, top + 65);
                    t.y++;
                    blankY--;
                    break;
                }
             }
             _dataRefresh();
            }
        }
        private void tile_moveRight()
        {
            if(blankX != 0)
            {
            foreach (Tile t in DataT){
    			if(t.x == blankX - 1 && t.y == blankY){
                    string imageName = t.name;
                    var Target = (Image)LogicalTreeHelper.FindLogicalNode(canvas, imageName);
                    var left = Canvas.GetLeft(Target);
                    var top = Canvas.GetTop(Target);
                    Canvas.SetLeft(Target, left + 65);
                    Canvas.SetTop(Target, top);
                    t.x++;
                    blankX--;
                    break;
                }
             }
             _dataRefresh();
            }
        }
        private void tile_moveLeft()
        {
            if(blankX != 3)
            {
            foreach (Tile t in DataT){
    			if(t.x == (blankX + 1) && t.y == blankY){
                    string imageName = t.name;
                    var Target = (Image)LogicalTreeHelper.FindLogicalNode(canvas, imageName);
                    var left = Canvas.GetLeft(Target);
                    var top = Canvas.GetTop(Target);
                    Canvas.SetLeft(Target, left - 65);
                    Canvas.SetTop(Target, top);
                    t.x--;
                    blankX++;
                    break;
                }
             }
             _dataRefresh();
            }
        }
        private void tile_deleteAll()
        {
			for(int y = 0; y < lengthOfY; y++)
			{
				for(int x = 0; x < lengthOfX; x++)
				{
					string fileName = String.Format("tile{0}{1}", y, x );
                    tile_delete(fileName);
				}
			}
        }
        private void tile_delete(string imageName)
        {
            var Target = (Image)LogicalTreeHelper.FindLogicalNode(canvas, imageName);
            canvas.Children.Remove(Target);
        }
        private void tile_hide(string imageName)
        {
            if(!_isFinished)
            {
                var Target = (Image)LogicalTreeHelper.FindLogicalNode(canvas, imageName);
                Canvas.SetLeft(Target, 278);
                Canvas.SetTop(Target, 213);
                tile_evacuation(imageName);
            }
        }
        private void tile_evacuation(string imageName)
        {
            foreach (Tile t in DataT){
    			if(t.name == imageName){
                    t.x = 4;
                    t.y = 3;
                }
             }
             _dataRefresh();
        }
        private void _dataRefresh()
        {
            tempDataT.Clear();
            var rCount = DataT.Count;
            for (int row = 0; row < rCount; row++ )
            {
                var tempName = DataT[row].name;
                var same = true;
                for (int after = row + 1 ; after < rCount; after++)
                {
                    if(DataT[after].name == tempName)
                    {
                        same = false;
                        break;
                    }
                }
                if (same)
                {
                    tempDataT.Add(DataT[row]);
                }
            }
            DataT.Clear();
            foreach (Tile dT in tempDataT){
                DataT.Add(dT);
            }
        }
        // Add a Rectangle Element
        private void add_rectangle()
        {
            for(int y = 0; y < lengthOfY; y++)
            {
                for(int x = 0; x < lengthOfX; x++)
                {
                    string tileName = "rect"+y+x;
                    set_tile(tileName, y, x);
                }
            }
        }
        private void isFinished()
        {
            _isFinished = true;
            foreach (Tile t in tempDataT){
                if(t.originalX != t.x || t.originalY != t.y)
                {
                    if(t.x != 4)
                    {
                        _isFinished = false;
                    }
                }
            }
            if(_isFinished)
            {
                var Target = (Image)LogicalTreeHelper.FindLogicalNode(canvas, "tile33");
                Canvas.SetLeft(Target, 213);
                Canvas.SetTop(Target, 213);
                textBlock2.Text = "　　完成!";
            }
        }
    }
}
