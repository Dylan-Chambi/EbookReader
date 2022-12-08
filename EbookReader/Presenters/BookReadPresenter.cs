using EbookReader.Models;
using EbookReader.Presenters.IndexItem;
using EbookReader.Presenters.PageItem;
using EbookReader.Views;
using EbookReader.Views.IndexItem;
using EbookReader.Views.PageItem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using System.Text.RegularExpressions;
using System.Xml;

namespace EbookReader.Presenters
{
    public class BookReadPresenter
    {
        private IBookReadView bookReadView;
        private Ebook ebook;
        private Dictionary<string, byte[]> images;
        private Dictionary<string, byte[]> fonts;

        private Dictionary<string, string> styles;

        private int currentChapter = 1;

        private IPageItemView pageItemView;
        private IPageItemView pageItemView2;
        private IPageItemView pageItemViewForLoad;

        private Control control;
        private Control control2;
        private Control controlForLoad;

        private string backgroundColor = "#ffffff";
        private string textColor = "#000000";

        public BookReadPresenter(IBookReadView bookReadView, Ebook ebook)
        {
            this.bookReadView = bookReadView;
            this.ebook = ebook;

            bookReadView.CurrentEbook = ebook;
            InitPageItems();
            LoadChaptersList();
            images = ebook.getImages();
            fonts = ebook.getFonts();
            styles = ebook.getStylesheets();
        }

        public void InitPageItems()
        {
            pageItemView = new PageItemView();
            new PageItemPresenter(pageItemView, "");
            control = (UserControl)pageItemView;
            bookReadView.TableLayoutPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
            pageItemView.WebBrowser.ScrollBarsEnabled = false;

            pageItemView2 = new PageItemView();
            new PageItemPresenter(pageItemView2, "");
            control2 = (UserControl)pageItemView2;
            bookReadView.TableLayoutPanel.Controls.Add(control2);
            control2.Dock = DockStyle.Fill;
            pageItemView2.WebBrowser.ScrollBarsEnabled = false;

            pageItemViewForLoad = new PageItemView();
            new PageItemPresenter(pageItemViewForLoad, "");
            controlForLoad = (UserControl)pageItemViewForLoad;
            bookReadView.TableLayoutPanel.Controls.Add(controlForLoad);
            controlForLoad.Dock = DockStyle.Fill;
            pageItemViewForLoad.WebBrowser.ScrollBarsEnabled = false;
            controlForLoad.Visible = false;
            
            
            control.BeginInvoke ((MethodInvoker)delegate {
                // suspend layout
                control.SuspendLayout();
                control2.SuspendLayout();
                controlForLoad.SuspendLayout();
                controlForLoad.Width = control.Width;
                controlForLoad.Height = control.Height;
                LoadChapterPages(currentChapter);
                // resume layout
                control.ResumeLayout();
                control2.ResumeLayout();
                controlForLoad.ResumeLayout();

            });

            bookReadView.TableLayoutPanel.SizeChanged += (sender, e) =>
            {
                controlForLoad.Width = control.Width;
                controlForLoad.Height = control.Height;
                LoadChapterPages(currentChapter);

            };
        }


        public void LoadChaptersList()
        {
            foreach (Chapter chapter in ebook.getChapters())
            {
                IIndexItemView indexItemView = new IndexItemView();
                new IndexItemPresenter(indexItemView, chapter.ChapterTitle, chapter.ChapterIndexPage);
                Control control = (UserControl)indexItemView;
                bookReadView.ListIndexTable.Controls.Add(control);
                control.Dock = DockStyle.Top;
                indexItemView.IndexItemButton.Click += (sender, e) =>
                {
                    currentChapter = indexItemView.IndexItemPageNumber;
                    LoadChapterPages(currentChapter);
                    Debug.WriteLine("Chapter: " + currentChapter);
                };
            }
        }

        public void LoadChapterPages(int chapter)
        {
                pageItemViewForLoad.WebBrowser.Document.Body.InnerHtml = "";
                HtmlElement body = pageItemViewForLoad.WebBrowser.Document.Body;
                HtmlElement head = pageItemViewForLoad.WebBrowser.Document.GetElementsByTagName("head")[0];
                
                List<string> pages = new List<string>();

                HtmlDocument content = new HtmlDocument();
                content.LoadHtml(ebook.getChaptersContent()[chapter]);

                HtmlDocument newDocument = new HtmlDocument();
                HtmlNode html = newDocument.CreateElement("html");
                newDocument.DocumentNode.AppendChild(html);

                HtmlNode headNode = HtmlNode.CreateNode("<head></head>");
                html.AppendChild(headNode);

                HtmlNode metaEdge = HtmlNode.CreateNode("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">");
                headNode.AppendChild(metaEdge);

                HtmlNode metaCharset = HtmlNode.CreateNode("<meta charset=\"utf-8\">");
                headNode.AppendChild(metaCharset);

                //set viewport width and height
                HtmlNode metaViewport = HtmlNode.CreateNode("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no\">");
                headNode.AppendChild(metaViewport);

                // align content to center
                // margin positions: top, right, bottom, left
                HtmlNode definitiveStyle = HtmlNode.CreateNode("<style> body { margin-top: 50px; margin-bottom: 0; margin-left: 50px; margin-right: 50px; display: flex-box; } </style>");
                headNode.AppendChild(definitiveStyle);

                //set theme
                HtmlNode backgroundColorStyle = HtmlNode.CreateNode("<style> body { background-color: " + backgroundColor + "!important; color: " + textColor + "!important; } </style>");
                headNode.AppendChild(backgroundColorStyle);
                
                HtmlNode bodyNode = HtmlNode.CreateNode("<body></body>");
                html.AppendChild(bodyNode);

                

                int recursiveCount = 0;

                List<HtmlNode> treeNodes = new List<HtmlNode>();

                void appendElementUntilDocumentHeigth(HtmlNode currentNode)
                {
                    recursiveCount++;
                    HtmlNode parentNode = null;

                    if (treeNodes.Count > 0){
                        parentNode = treeNodes.Last();
                    }
                    HtmlNode copyCurrentNode = HtmlNode.CreateNode(currentNode.OuterHtml);
                    copyCurrentNode.RemoveAllChildren();

                    if ( !((currentNode.ChildNodes.Count == 1 && currentNode.ChildNodes[0].NodeType == HtmlNodeType.Text) || (currentNode.ChildNodes.Count == 0 && currentNode.NodeType != HtmlNodeType.Text)) )
                    {
                        if (treeNodes.Count > 0)
                        {
                            parentNode.AppendChild(copyCurrentNode);
                        }
                        treeNodes.Add(copyCurrentNode);
                        foreach (HtmlNode childNode in currentNode.ChildNodes)
                        {
                            appendElementUntilDocumentHeigth(childNode);
                        }
                    }
                    else
                    {
                        HtmlNode tempCurrentNode = HtmlNode.CreateNode(currentNode.OuterHtml);
                        parentNode.AppendChild(tempCurrentNode);
                        if(tempCurrentNode.Name == "img")
                        {
                            string srcPath = tempCurrentNode.Attributes["src"].Value;
                            srcPath = srcPath.Replace("../", "");

                            if (tempCurrentNode.Attributes["style"] == null) tempCurrentNode.Attributes.Add("style", "max-width:80vw;max-height:80vh;");
                            else tempCurrentNode.Attributes["style"].Value = "max-width:80vw;max-height:80vh;";

                            if (tempCurrentNode.Attributes["src"] == null) tempCurrentNode.Attributes.Add("src", "data:image/png;base64," + Convert.ToBase64String(images[srcPath]));
                            else tempCurrentNode.Attributes["src"].Value = "data:image/png;base64," + Convert.ToBase64String(images[srcPath]);
                        }
                        body.InnerHtml = treeNodes.First().InnerHtml;
                        bodyNode.InnerHtml = treeNodes.First().InnerHtml;
                        if (pageItemViewForLoad.WebBrowser.Document.Body.ScrollRectangle.Height + 150 > controlForLoad.Height)
                        {
                            if (treeNodes.Count > 0){
                                parentNode.RemoveChild(tempCurrentNode);
                            }
                            bodyNode.InnerHtml = treeNodes.First().InnerHtml;
                            pages.Add(newDocument.DocumentNode.OuterHtml);
                            bodyNode.InnerHtml = "";

                            for (int i = 0; i < treeNodes.Count; i++)
                            {
                                treeNodes[i].RemoveAllChildren();
                            }

                            for (int i = treeNodes.Count - 1; i > 0; i--)
                            {
                                treeNodes[i - 1].AppendChild(treeNodes[i]);
                            }
                            parentNode.AppendChild(tempCurrentNode);
                        }

                    }
                    treeNodes.Remove(copyCurrentNode);
                }

                void appendStylesAndFonts(HtmlNode htmlNode)
                {
                    if (htmlNode.Name == "style")
                    {
                        headNode.ChildNodes.Add(htmlNode);
                    }
                    else if (htmlNode.Name == "link")
                    {
                        // create style element
                        string stylePath = htmlNode.Attributes["href"].Value;
                        string linkType = htmlNode.Attributes["type"].Value;

                        if (linkType == "text/css")
                        {
                            stylePath = stylePath.Replace("../", "");
                            string stylesheet = styles[stylePath];
                            

                            // get all font path
                            string[] fontPaths = Regex.Matches(stylesheet, @"url\((.*?)\)").Cast<Match>().Select(m => m.Groups[1].Value).ToArray();
                            // string[] fontPaths = new string[0];
                            foreach (string fontPath in fontPaths)
                            {
                                string fontBase64 = Convert.ToBase64String(fonts[fontPath]);
                                fontBase64 = "data:font/opentype;base64," + fontBase64;
                                stylesheet = stylesheet.Replace(fontPath, fontBase64);
                            }
                            HtmlNode styleNode = HtmlNode.CreateNode("<style></style>");
                            styleNode.InnerHtml = stylesheet;
                            headNode.ChildNodes.Add(styleNode);
                        }
                    }
                }

                foreach (HtmlNode childNode in content.DocumentNode.SelectNodes("//head").Nodes())
                {
                    appendStylesAndFonts(childNode);
                }

                pageItemViewForLoad.WebBrowser.DocumentText = newDocument.DocumentNode.OuterHtml.Clone().ToString();

                appendElementUntilDocumentHeigth(content.DocumentNode.SelectSingleNode("//body"));

                



                Debug.WriteLine("recursiveCount: " + recursiveCount);

                if (pages.Count == 0)
                {
                    pages.Add(newDocument.DocumentNode.OuterHtml);
                }

                pageItemView.WebBrowser.DocumentText = pages[0];
                if (pages.Count > 1)
                {
                    pageItemView2.WebBrowser.DocumentText = pages[1];
                }else
                {
                    pageItemView2.WebBrowser.DocumentText = "";
                }
        }

    }
}