using EbookReader.Models;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EbookReader.Views
{
    public partial class MainView : Form, IMainView
    {
        private List<Ebook> ebookItems = new List<Ebook>();


        public List<Ebook> EbookItems { get => ebookItems; set => ebookItems = value; }
        public TableLayoutPanel TableLayoutPanel { get => tableLayoutPanel1; }

        public Guna2TextBox SearchTextBox { get => this.guna2TextBox1; }

        public Form MainViewForm { get => this; }

        public MainView()
        {
            InitializeComponent();
        }

        public void AsoociateAndRaiseEvents()
        {
            foreach (Form form in TableLayoutPanel.Controls)
            {
                if (form is IEbookItemView)
                {
                    IEbookItemView ebookItemView = (IEbookItemView)form;
                    //form.Click += delegate { EbookItemClicked?.Invoke(form, EventArgs.Empty); };
                }
            }
        }

        //public event EventHandler EbookItemClicked;

        //public void SetEbookItemListBindingSource(BindingSource ebookItemListBindingSource)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
