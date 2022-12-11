using EbookReader.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EbookReader.Views
{
    public partial class BookReadView : Form, IBookReadView
    {
        public BookReadView()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private Ebook currentEbook;

        public Ebook CurrentEbook { get => currentEbook; set => currentEbook = value; }
        public Form EbookReadForm { get => this; }
        public TableLayoutPanel ListIndexTable { get => this.tableLayoutPanel2; }
        public TableLayoutPanel TableLayoutPanel { get => this.tableLayoutPanel1; }

    }
}
