using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersOne.Epub;

namespace EbookReader.Models
{
    public class EbookEpub : Ebook
    {
        private EpubBook eBook;

        public EbookEpub(String ebookpath) : base(ebookpath, "", "", null, EbookType.EPUB)
        {
            this.eBook = EpubReader.ReadBook(ebookpath);
            this.EbookTitle = eBook.Title;
            this.EbookAuthor = eBook.Author;
            if (eBook.CoverImage != null)
            {
                this.EbookCoverImage = Image.FromStream(new MemoryStream(eBook.CoverImage));
            }
        }

        public override List<Chapter> getChapters()
        {
            List<Chapter> chapters = new List<Chapter>();
            foreach (EpubNavigationItem navigationItem in eBook.Navigation)
            {
                chapters.Add(new Chapter(navigationItem.Title, 0));
            }
            return chapters;
        }

    }
}