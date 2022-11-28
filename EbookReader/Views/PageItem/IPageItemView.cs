using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EbookReader.Views.PageItem
{
    public interface IPageItemView
    {
        HtmlDocument HtmlDocument { get; set; }
        WebBrowser WebBrowser { get; }
        string Content { get; set; }
    }
}
