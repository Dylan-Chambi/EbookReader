using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EbookReader.Views.PageItem
{
    public partial class PageItemView : UserControl, IPageItemView
    {
        public PageItemView()
        {
            InitializeComponent();
            webBrowser = this.webBrowser1;
            //this.Dock = DockStyle.Fill;
        }

        private HtmlDocument htmlDocument;
        private WebBrowser webBrowser;

        public HtmlDocument HtmlDocument { get => htmlDocument; set => htmlDocument = value; }
        public WebBrowser WebBrowser { get => webBrowser; }
        public string Content { get => this.webBrowser1.DocumentText; set => this.webBrowser1.DocumentText = value; }
    }
}
