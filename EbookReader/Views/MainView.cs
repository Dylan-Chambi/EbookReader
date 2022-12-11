using EbookReader.Models;
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

        public TextBox SearchTextBox { get => textBox1; }

        public Form MainViewForm { get => this; }

        public MainView()
        {
            InitializeComponent();
            this.Click += delegate
            {
                Debug.WriteLine("MainView clicked");
            };
            foreach (UserControl uc in tableLayoutPanel1.Controls)
            {
                uc.Click += delegate
                {
                    Debug.WriteLine("UserControl clicked");
                };
            }
        }

        public void AsoociateAndRaiseEvents()
        {
            foreach (Form form in tableLayoutPanel1.Controls)
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
