using EbookReader.Models;
using EbookReader.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbookReader.Presenters
{
    public class EbookItemPresenter
    {
        private IEbookItemView ebookItemView;
        private Ebook ebook;

        public EbookItemPresenter(IEbookItemView ebookItemView, Ebook ebook)
        {
            Debug.WriteLine("EbookItemPresenter constructor");
            this.ebookItemView = ebookItemView;
            this.ebook = ebook;

            this.ebookItemView.EbookPath = ebook.EbookPath;
            this.ebookItemView.EbookTitle = ebook.EbookTitle;
            this.ebookItemView.EbookAuthor = ebook.EbookAuthor;
            this.ebookItemView.EbookCoverImage = ebook.EbookCoverImage;
            this.ebookItemView.EbookType = ebook.EbookType;

            this.ebookItemView.EbookItemClicked += EbookItemView_EbookItemClicked;

            this.ebookItemView.Show();
        }

        private void EbookItemView_EbookItemClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Ebook item clicked");
        }
    }
}
