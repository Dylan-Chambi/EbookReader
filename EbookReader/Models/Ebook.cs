using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbookReader.Models
{
    public abstract class Ebook
    {
        private string ebookPath;
        private string ebookTitle;
        private string ebookAuthor;
        private Image ebookCoverImage;
        private EbookType ebookType;

        public Ebook(string ebookPath, string ebookTitle, string ebookAuthor, Image ebookCoverImage, EbookType ebookType)
        {
            this.ebookPath = ebookPath;
            this.ebookTitle = ebookTitle;
            this.ebookAuthor = ebookAuthor;
            this.ebookCoverImage = ebookCoverImage;
            this.ebookType = ebookType;
        }

        public string EbookPath { get => ebookPath; set => ebookPath = value; }
        public string EbookTitle { get => ebookTitle; set => ebookTitle = value; }
        public string EbookAuthor { get => ebookAuthor; set => ebookAuthor = value; }
        public Image EbookCoverImage { get => ebookCoverImage; set => ebookCoverImage = value; }
        public EbookType EbookType { get => ebookType; set => ebookType = value; }

        public abstract List<Chapter> getChapters();
    }

    public class Chapter
    {
        private string chapterTitle;
        private int chapterIndexPage;

        public Chapter(string chapterTitle, int chapterIndexPage)
        {
            this.chapterTitle = chapterTitle;
            this.chapterIndexPage = chapterIndexPage;
        }

        public string ChapterTitle { get => chapterTitle; set => chapterTitle = value; }
        public int ChapterIndexPage { get => chapterIndexPage; set => chapterIndexPage = value; }
    }

    public enum EbookType {
        EPUB,
        PDF
    }
}
