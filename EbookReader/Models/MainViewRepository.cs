using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbookReader.Models
{
    public class MainViewRepository : IMainViewRepository
    {
        private List<Ebook> ebookItems;
        private SQLiteConnection sqliteConnection;

        public MainViewRepository()
        {
            ebookItems = new List<Ebook>();
            string projectPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string databasePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(projectPath)), "Resources", "ebooksDB.db");

            sqliteConnection = new SQLiteConnection("Data Source=" + databasePath);

            sqliteConnection.Open();

            Debug.WriteLine(sqliteConnection.State);

            // load all the ebooks from the database
            SQLiteCommand sqliteCommand = new SQLiteCommand("SELECT * FROM Ebooks", sqliteConnection);
            SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();

            while (sqliteDataReader.Read())
            {
                Debug.WriteLine(sqliteDataReader["EbookPath"].ToString());
                ebookItems.Add(new EbookEpub(sqliteDataReader["EbookPath"].ToString()));
            }
        }

        public void AddEbookItem(Ebook ebookItem)
        {
            ebookItems.Add(ebookItem);
            // add the ebook to the database
            
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
