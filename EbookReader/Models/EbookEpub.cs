using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VersOne.Epub;
using HtmlAgilityPack;
using System.Diagnostics;
using System.Windows.Forms;

namespace EbookReader.Models
{
    public class EbookEpub : Ebook
    {
        private EpubBook eBook;

        public EbookEpub(String ebookpath) : base(ebookpath, "", "", null, EbookType.EPUB)
        {
            try
            {
                this.eBook = EpubReader.ReadBook(ebookpath);
                this.EbookTitle = eBook.Title;
                this.EbookAuthor = eBook.Author;
                if (eBook.CoverImage != null)
                {
                    this.EbookCoverImage = Image.FromStream(new MemoryStream(eBook.CoverImage));
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error while reading the ebook: " + e.Message);
            }
        }

        public override List<Chapter> getChapters()
        {
            List<Chapter> chapters = new List<Chapter>();

            List<string> chaptersContent = new List<string>();

            foreach (EpubTextContentFile textContentFile in eBook.ReadingOrder)
            {
                chaptersContent.Add(textContentFile.FilePathInEpubArchive);
            }

            foreach (EpubNavigationItem navigationItem in eBook.Navigation)
            {
                EpubNavigationItemLink navigationItemLink = navigationItem.Link;
                int index = chaptersContent.IndexOf(navigationItemLink.ContentFilePathInEpubArchive);
                chapters.Add(new Chapter(navigationItem.Title, index));
            }
            return chapters;
        }

        public override string getContent()
        {
            string content = "";
            foreach (EpubTextContentFile textContentFile in eBook.ReadingOrder)
            {
                // return string with all html code
                content += textContentFile.Content;

                // return string with all text
                //content += textContentFile.ContentAsPlainText;

            }
            return content;
        }

        public override List<string> getChaptersContent()
        {
            List<string> chaptersContent = new List<string>();
            foreach (EpubTextContentFile textContentFile in eBook.ReadingOrder)
            {
                chaptersContent.Add(textContentFile.Content);
            }
            return chaptersContent;
        }

        public override Dictionary<string, byte[]> getImages()
        {
            return eBook.Content.Images.ToDictionary(image => image.Key, image => image.Value.Content);
        }

        public override Dictionary<string, byte[]> getFonts()
        {
            return eBook.Content.Fonts.ToDictionary(font => font.Key, font => font.Value.Content);
        }

        public override Dictionary<string, string> getStylesheets()
        {
            return eBook.Content.Css.ToDictionary(stylesheet => stylesheet.Key, stylesheet => stylesheet.Value.Content);
        }

        
    }
}