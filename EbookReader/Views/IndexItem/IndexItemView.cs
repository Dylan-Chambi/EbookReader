using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EbookReader.Views.IndexItem
{
    public partial class IndexItemView : UserControl, IIndexItemView
    {
        private int indexItemPageNumber;

        public IndexItemView()
        {
            InitializeComponent();
        }

        public int IndexItemPageNumber { get => indexItemPageNumber; set => indexItemPageNumber = value; }
        public string IndexItemTitle { get => this.button1.Text; set => this.button1.Text = value; }
        public Button IndexItemButton { get => this.button1; }
        
        public event EventHandler IndexItemClicked;

    }
}
