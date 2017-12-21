using System;
using System.Collections;
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
using System.IO;
using System.IO.Compression;
using MySql.Data.MySqlClient;
using System.Configuration;
namespace Management_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ArrayList uploadList = new ArrayList();
        private MySqlConnection myConnection;
        private MySqlCommand myCommand;

        public MainWindow()
        {
            InitializeComponent();
            listBoxFiles.AllowDrop = true;
            listBoxFiles.Drop += listBoxFiles_DragDrop; 
            listBoxFiles.DragEnter += listBoxFiles_DragEnter;
            

        }


        private void listBoxFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
                listBoxFiles.Items.Add(file);
        }

        private void listBoxFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effects = DragDropEffects.Copy;
        }

     
        private void UploadBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = ConfigurationManager.AppSettings["connectionString"];
            MessageBox.Show(name);
            uploadList.Clear();
            foreach (string file in listBoxFiles.Items) {
                uploadList.Add(file);

                MessageBox.Show(file);
            }
            MySqlDataReader rdr;
            myConnection = new MySqlConnection(name);
            myConnection.Open();

 //           myCommand = new MySqlCommand("insert into CustomerDetails values ('', 'starx','999','3');", myConnection);
 //          myCommand.ExecuteNonQuery();

            /*
             * //壓縮  
            //己經有確定要壓縮的檔案
            FileStream sourceFile = File.OpenRead(@"C:\sample.xml");
            //壓縮後的檔案名稱
            FileStream destFile = File.Create(@"C:\sample.gz");
            //開始
            GZipStream compStream = new GZipStream(destFile, CompressionMode.Compress, true);
            try
            {
                int theByte = sourceFile.ReadByte();
                while (theByte != -1)
                {
                    compStream.WriteByte((byte)theByte);
                    theByte = sourceFile.ReadByte();
                }
            }
            finally
            {
                compStream.Flush();
                compStream.Dispose();
                sourceFile.Flush();
                sourceFile.Dispose();
                destFile.Flush();
                destFile.Dispose();
            }
             */
            myCommand = new MySqlCommand ("Select id, Customer from CustomerDetails",myConnection);
            rdr = myCommand.ExecuteReader();
          
                while (rdr.Read())
                {
                    MessageBox.Show(rdr["id"].ToString(), rdr["Customer"].ToString());
                }
            
            rdr.Close();
            myConnection.Close();
            uploadList.Clear();
            listBoxFiles.Items.Clear();

            
            

        }

        //解壓縮
        protected void Decompress()
        {
            //被壓縮後的檔案
            FileStream sourceFile = File.OpenRead(@"C:\sample.gz");
            //解壓縮後的檔案
            FileStream destFile = File.Create(@"C:\Unsample.xml");

            //開始
            GZipStream compStream = new GZipStream(sourceFile, CompressionMode.Decompress, true);
            try
            {
                int theByte = compStream.ReadByte();
                while (theByte != -1)
                {
                    destFile.WriteByte((byte)theByte);
                    theByte = compStream.ReadByte();
                }
            }
            finally
            {
                compStream.Flush();
                compStream.Dispose();
                sourceFile.Flush();
                sourceFile.Dispose();
                destFile.Flush();
                destFile.Dispose();
            }
        }

    
    }
    
}
