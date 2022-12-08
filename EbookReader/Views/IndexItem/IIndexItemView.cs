using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EbookReader.Views.IndexItem
{
    public interface IIndexItemView
    {
        // Properties
        string IndexItemTitle { get; set; }
        int IndexItemPageNumber { get; set; }
        Button IndexItemButton { get; }

        // Events
        event EventHandler IndexItemClicked;

        // Methods
        void Show();
    }
}
