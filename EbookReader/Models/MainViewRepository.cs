using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EbookReader.Models
{
    public class MainViewRepository : IMainViewRepository
    {
        private List<Ebook> ebookItems;
        private SQLiteConnection sqliteConnection;

        private string databasePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EbookReader\\ebookDB.db"; // path: C:\Users\DylanPC\AppData\Roaming\EbookReader\ebookDB.db
        // this is the path where the database will be stored

        public MainViewRepository()
        {
            ebookItems = new List<Ebook>();

            // create the database if it doesn't exist
            if (!System.IO.File.Exists(databasePath))
            {
                // create the database directory
                System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EbookReader");
                // create the database
                SQLiteConnection.CreateFile(databasePath);
            }

            //the command to add a nuget package as a dependency is: Install-Package System.Data.SQLite -Version

            sqliteConnection = new SQLiteConnection("Data Source=" + databasePath);

            sqliteConnection.Open();

            Debug.WriteLine(sqliteConnection.State);

            // load all the ebooks from the database
            SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM Ebooks", sqliteConnection);
            // it throws an exception if the table doesn't exist, so we need to create the table

            SQLiteDataReader sqliteDataReader = null;
            try
            {
                sqliteDataReader = sqliteCommand.ExecuteReader();
            }
            catch (Exception)
            {
                // create the table
                SQLiteCommand sqliteCommandCreateTable = new SQLiteCommand("CREATE TABLE Ebooks (EbookPath TEXT)", sqliteConnection);
                sqliteCommandCreateTable.ExecuteNonQuery();

                // load all the ebooks from the database
                sqliteDataReader = sqliteCommand.ExecuteReader();
            }

            while (sqliteDataReader.Read())
            {
                Debug.WriteLine(sqliteDataReader["EbookPath"].ToString());
                try
                {
                    ebookItems.Add(new EbookEpub(sqliteDataReader["EbookPath"].ToString()));
                }
                catch (Exception e)
                {
                    // show a message box with the error while reading the ebooks, deleting conflicting ebooks from the database
                    MessageBox.Show(e.Message, "Error while reading the ebook, deleting conflicting ebooks from the database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // delete the ebook from the database
                    SQLiteCommand sqliteCommandDelete = new SQLiteCommand("DELETE FROM Ebooks WHERE EbookPath = @EbookPath", sqliteConnection);
                    sqliteCommandDelete.Parameters.AddWithValue("@EbookPath", sqliteDataReader["EbookPath"].ToString());
                    sqliteCommandDelete.ExecuteNonQuery();
                }
            }
        }

        public void AddEbookItem(Ebook ebookItem)
        {
            ebookItems.Add(ebookItem);
            SQLiteCommandBuilder sqliteCommandBuilder = new SQLiteCommandBuilder();
            SQLiteCommand sqliteCommand = new SQLiteCommand("INSERT INTO Ebooks (EbookPath) VALUES (@EbookPath)", sqliteConnection);
            sqliteCommand.Parameters.AddWithValue("@EbookPath", ebookItem.EbookPath);
            sqliteCommand.ExecuteNonQuery();
        }

        public List<Ebook> GetEbookItems()
        {
            return ebookItems;
        }

        public List<Ebook> GetEbookItemsByTitle(string title)
        {
            throw new NotImplementedException();
        }

        public void RemoveEbookItem(Ebook ebookItem)
        {
            throw new NotImplementedException();
        }
    }
}
