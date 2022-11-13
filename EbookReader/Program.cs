using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using VersOne.Epub;

namespace EbookReader
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //EpubBook book = EpubReader.ReadBook(@"C:\Users\DylanPC\source\repos\EbookReader\EbookReader\Resources\Batman_ Nightwalker - Marie Lu.epub");
            //foreach (EpubTextContentFile contentFile in book.ReadingOrder)
            //{
            //    Debug.WriteLine(contentFile.Content);
            //}
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}