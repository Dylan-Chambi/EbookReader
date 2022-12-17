using EbookReader.Models;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EbookReader.Views
{
    public interface IMainView
    {
        List<Ebook> EbookItems { get; set; }
        TableLayoutPanel TableLayoutPanel { get; }

        Guna2TextBox SearchTextBox { get; }

        Form MainViewForm { get; }

        //event EventHandler EbookItemClicked;

        //void SetEbookItemListBindingSource(BindingSource ebookItemListBindingSource);
    }
}
