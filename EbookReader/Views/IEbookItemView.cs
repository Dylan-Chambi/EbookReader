using EbookReader.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EbookReader.Views
{
    public interface IEbookItemView
    {
        // Properties
        string EbookPath { get; set; }
        string EbookTitle { get; set; }
        string EbookAuthor { get; set; }
        Image EbookCoverImage { get; set; }
        EbookType EbookType { get; set; }

        // Events
        event EventHandler EbookItemClicked;
         
        // Methods
        void Show();
        
    }
}
