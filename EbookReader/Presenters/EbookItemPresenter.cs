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
            this.ebookItemView = ebookItemView;
            this.ebook = ebook;

            // Set the view's properties
            this.ebookItemView.EbookPath = ebook.EbookPath;
            this.ebookItemView.EbookTitle = ebook.EbookTitle;
            this.ebookItemView.EbookAuthor = ebook.EbookAuthor;
            if (ebook.EbookCoverImage != null)
            {
                this.ebookItemView.EbookCoverImage = ebook.EbookCoverImage;
            }
            this.ebookItemView.EbookType = ebook.EbookType;

            // Subscribe to events
            this.ebookItemView.EbookItemClicked += EbookItemView_EbookItemClicked;

            
            this.ebookItemView.Show();
        }

        private void EbookItemView_EbookItemClicked(object sender, EventArgs e)
        {
            IBookReadView bookReadView = new BookReadView();
            new BookReadPresenter(bookReadView, ebook);
            bookReadView.Show();
        }
    }
}
