using System.Collections.ObjectModel;

public class FileNameList {
    // バインディングの指定先プロパティ
    public ObservableCollection<FileName> DataFileName { get; set; }
 
    // コンストラクタ(データ入力)
    public FileNameList() {
        DataFileName = new ObservableCollection<FileName>();

        string[] files = System.IO.Directory.GetFiles(@"C:\Image\", "*.*");
        foreach (string s in files)
        {
            System.IO.FileInfo fi = null;
            try
            {
            fi = new System.IO.FileInfo(s);
			DataFileName.Add(new FileName{fname = s});
            }
            catch (System.IO.FileNotFoundException)
            {
                continue;
            }
        }
    }
}   

