using EbookReader.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EbookReader.Presenters
{
    public class EbookItemViewAddPresenter
    {
        private EbookItemViewAdd ebookItemViewAdd;

        private UserControl ebookItemViewAddUserControl;

        public EbookItemViewAddPresenter(EbookItemViewAdd ebookItemViewAdd)
        {
            this.ebookItemViewAdd = ebookItemViewAdd;
            this.ebookItemViewAddUserControl = ebookItemViewAdd.EbookItemAddUserControl;
        }
    }
}
