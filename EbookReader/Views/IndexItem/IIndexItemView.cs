using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbookReader.Views.IndexItem
{
    public interface IIndexItemView
    {
        // Properties
        string IndexItemTitle { get; set; }
        int IndexItemPageNumber { get; set; }

        // Events
        event EventHandler IndexItemClicked;

        // Methods
        void Show();
    }
}
