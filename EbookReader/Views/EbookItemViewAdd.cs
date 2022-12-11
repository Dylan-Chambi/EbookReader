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
    public partial class EbookItemViewAdd : UserControl, IEbookItemViewAdd
    {
        public EbookItemViewAdd()
        {
            InitializeComponent();
        }

        public Button ButtonAdd { get => this.button1;}
        public UserControl EbookItemAddUserControl { get => this; }
    }
}
