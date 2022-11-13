using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using VersOne.Epub;
using EbookReader.Views;
using EbookReader.Models;
using EbookReader.Presenters;

namespace EbookReader
{
    internal static class Program
    {
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
            IMainView mainView = new MainView();
            new MainMenuPresenter(mainView, new MainViewRepository());
            Application.Run((Form) mainView);
        }
    }
}