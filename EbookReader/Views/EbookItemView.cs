using EbookReader.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace EbookReader.Views
{
    public partial class EbookItemView : UserControl, IEbookItemView
    {
        public EbookItemView()
        {
            InitializeComponent();
            AssociateAndRaiseViewEvents();
        }

        private void AssociateAndRaiseViewEvents()
        {
            SetControlsRecursively(this.Controls);
        }

        public void SetControlsRecursively(ControlCollection controlCollection)
        {
            foreach(Control control in controlCollection)
            {
                control.Click += delegate { EbookItemClicked?.Invoke(control, EventArgs.Empty); };
                SetControlsRecursively(control.Controls);
            }
        }
        
        public string ebookPath;
        public string ebookTitle;
        public string ebookAuthor;
        public Image ebookCoverImage;
        public EbookType ebookType;

        public event EventHandler EbookItemClicked;

        public string EbookPath { get => ebookPath; set => ebookPath = value; }
        public string EbookTitle 
        { 
            get => ebookTitle; 
            set
            {
                ebookTitle = value;
                labelTitle.Text = value;
            }
        }
        public string EbookAuthor 
        { 
            get => ebookAuthor; 
            set
            {
                ebookAuthor = value;
                labelAuthor.Text = value;
            }
        }
        public Image EbookCoverImage 
        {
            get => ebookCoverImage;
            set
            {
                ebookCoverImage = value;
                pictureBoxCoverImage.BackgroundImage = value;
            }
        }
        public EbookType EbookType { get => ebookType; set => ebookType = value; }

    }
}
