using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace EbookReader.Views
{
    public partial class EbookItemViewAdd : UserControl, IEbookItemViewAdd
    {
        public EbookItemViewAdd()
        {
            InitializeComponent();
            // set pointer hand for button
            this.guna2Button1.Cursor = Cursors.Hand;
        }

        public Guna2Button ButtonAdd { get => this.guna2Button1;}
        public UserControl EbookItemAddUserControl { get => this; }

    }
}
