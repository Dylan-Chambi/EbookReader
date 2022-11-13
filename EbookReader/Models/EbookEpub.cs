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
            this.EbookCoverImage = Image.FromStream(new MemoryStream(eBook.CoverImage));
        }
    }
}