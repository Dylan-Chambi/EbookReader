using EbookReader.Models;
using EbookReader.Presenters;
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
            this.currentTheme = "Light";
        }

        private Ebook currentEbook;
        private string currentTheme;

        private BookReadPresenter bookReadPresenter;

        public Ebook CurrentEbook { get => currentEbook; set => currentEbook = value; }
        public Form EbookReadForm { get => this; }
        public TableLayoutPanel ListIndexTable { get => this.tableLayoutPanel2; }
        public TableLayoutPanel TableLayoutPanel { get => this.tableLayoutPanel1; }

        public TableLayoutPanel SideTableLayoutPanel { get => this.tableLayoutPanel2; }

        public string CurrentTheme { get => currentTheme; set => currentTheme = value; }

        public event EventHandler ChangeTheme;



        private void customRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (customRadioButton1.Checked)
            {
                this.currentTheme = "Light";
                ChangeTheme?.Invoke(this, EventArgs.Empty);
            }
        }

        private void customRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (customRadioButton2.Checked)
            {
                this.currentTheme = "Dark";
                ChangeTheme?.Invoke(this, EventArgs.Empty);
            }
        }

        private void customRadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (customRadioButton3.Checked)
            {
                this.currentTheme = "DarkKhaki";
                ChangeTheme?.Invoke(this, EventArgs.Empty);
            }
        }

        private void customRadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (customRadioButton4.Checked)
            {
                this.currentTheme = "CornflowerBlue";
                ChangeTheme?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
