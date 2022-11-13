using EbookReader.Models;
using EbookReader.Presenters.IndexItem;
using EbookReader.Views;
using EbookReader.Views.IndexItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EbookReader.Presenters
{
    public class BookReadPresenter
    {
        private IBookReadView bookReadView;
        private Ebook ebook;

        public BookReadPresenter(IBookReadView bookReadView, Ebook ebook)
        {
            this.bookReadView = bookReadView;
            this.ebook = ebook;

            bookReadView.CurrentEbook = ebook;
            loadChaptersList();
        }


        public void loadChaptersList()
        {
            foreach (Chapter chapter in ebook.getChapters())
            {
                IIndexItemView indexItemView = new IndexItemView();
                new IndexItemPresenter(indexItemView, chapter.ChapterTitle, chapter.ChapterIndexPage);
                bookReadView.FlowLayoutPanel.Controls.Add((Control) indexItemView);
            }
        }
    }
}