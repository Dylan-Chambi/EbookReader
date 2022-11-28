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

        public override List<string> getChapterContentSplitBySize(string chapter, int width, int height)
        {
            List<string> chapterContentSplitBySize = new List<string>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(chapter);
            // set the width and height metadata
            HtmlNode head = htmlDocument.DocumentNode.SelectSingleNode("//head");
            HtmlNode meta = HtmlNode.CreateNode("<meta name=\"viewport\" content=\"width=" + width + ", height=" + height + "\" />");
            HtmlNode body = htmlDocument.DocumentNode.SelectSingleNode("//body");
            head.AppendChild(meta);
            // set the font size
            body.Attributes.Add("style", "font-size: 20px;");
            // set the font family
            body.Attributes.Add("style", "font-family: Arial;");
            // set the text align
            body.Attributes.Add("style", "text-align: justify;");
            // set the line height
            body.Attributes.Add("style", "line-height: 1.5;");
            // set the margin
            body.Attributes.Add("style", "margin: 0;");
            // set the padding
            body.Attributes.Add("style", "padding: 0;");
            // set the background color
            body.Attributes.Add("style", "background-color: #ffffff;");
            // set the color
            body.Attributes.Add("style", "color: #000000;");
            // set the text indent
            body.Attributes.Add("style", "text-indent: 0;");
            // set the text decoration
            body.Attributes.Add("style", "text-decoration: none;");
            // set the text transform
            body.Attributes.Add("style", "text-transform: none;");
            // set the text shadow
            body.Attributes.Add("style", "text-shadow: none;");
            // set the text overflow
            body.Attributes.Add("style", "text-overflow: clip;");
            // set the text wrap
            body.Attributes.Add("style", "word-wrap: break-word;");

            // split the content by the height
            int contentHeight = 0;
            int contentWidth = 0;
            int contentHeightLimit = height;
            int contentWidthLimit = width;
            int contentHeightLimitMargin = 0;
            int contentWidthLimitMargin = 0;
            int contentHeightLimitMarginMax = 0;
            int contentWidthLimitMarginMax = 0;

            // get the content height and width
            HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//body//p");
            foreach (HtmlNode htmlNode in htmlNodeCollection)
            {
                contentHeight += htmlNode.OuterHtml.Length;
                contentWidth += htmlNode.OuterHtml.Length;
            }

            // get the content height and width limit
            contentHeightLimit = height;
            contentWidthLimit = width;

            // get the content height and width limit margin
            contentHeightLimitMargin = contentHeightLimit / 10;
            contentWidthLimitMargin = contentWidthLimit / 10;

            // get the content height and width limit margin max
            contentHeightLimitMarginMax = contentHeightLimit - contentHeightLimitMargin;
            contentWidthLimitMarginMax = contentWidthLimit - contentWidthLimitMargin;

            
            // split the content by the height

            
            chapterContentSplitBySize.Add(htmlDocument.DocumentNode.OuterHtml);
            return chapterContentSplitBySize;
        }
    }
}