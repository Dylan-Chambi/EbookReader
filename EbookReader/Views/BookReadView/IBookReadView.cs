using EbookReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EbookReader.Views
{
    public interface IBookReadView
    {
        Ebook CurrentEbook { get; set; }
        TableLayoutPanel ListIndexTable { get; }
        TableLayoutPanel TableLayoutPanel { get; }

        TableLayoutPanel SideTableLayoutPanel { get; }
        Form EbookReadForm { get; }



        string CurrentTheme { get; set; }


        event EventHandler ChangeTheme;



        //event EventHandler BookReadViewLoad;

        void Show();
    }
}
