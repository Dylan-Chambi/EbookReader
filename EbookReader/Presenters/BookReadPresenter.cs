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
            control.Dock = DockStyle.None;
            pageItemView.WebBrowser.ScrollBarsEnabled = false;

            pageItemView2 = new PageItemView();
            new PageItemPresenter(pageItemView2, "");
            control2 = (UserControl)pageItemView2;
            bookReadView.TableLayoutPanel.Controls.Add(control2);
            control2.Dock = DockStyle.None;
            pageItemView2.WebBrowser.ScrollBarsEnabled = false;

            pageItemViewForLoad = new PageItemView();
            new PageItemPresenter(pageItemViewForLoad, "");
            controlForLoad = (UserControl)pageItemViewForLoad;
            bookReadView.TableLayoutPanel.Controls.Add(controlForLoad);
            controlForLoad.Dock = DockStyle.None;
            pageItemViewForLoad.WebBrowser.ScrollBarsEnabled = false;
            controlForLoad.Visible = false;
            
            
            control.BeginInvoke ((MethodInvoker)delegate {
                // suspend layout
                control.SuspendLayout();
                control2.SuspendLayout();
                controlForLoad.SuspendLayout();
                control.Width = bookReadView.TableLayoutPanel.Width / 2;
                control.Height = bookReadView.TableLayoutPanel.Height;
                control2.Width = bookReadView.TableLayoutPanel.Width / 2;
                control2.Height = bookReadView.TableLayoutPanel.Height;
                controlForLoad.Width = bookReadView.TableLayoutPanel.Width / 2;
                controlForLoad.Height = bookReadView.TableLayoutPanel.Height;
                LoadChapterPages(currentChapter);
                // resume layout
                control.ResumeLayout();
                control2.ResumeLayout();
                controlForLoad.ResumeLayout();

            });

            bookReadView.TableLayoutPanel.SizeChanged += (sender, e) =>
            {
                control.Width = bookReadView.TableLayoutPanel.Width / 2;
                control.Height = bookReadView.TableLayoutPanel.Height;
                control2.Width = bookReadView.TableLayoutPanel.Width / 2;
                control2.Height = bookReadView.TableLayoutPanel.Height;
                controlForLoad.Width = bookReadView.TableLayoutPanel.Width / 2;
                controlForLoad.Height = bookReadView.TableLayoutPanel.Height;
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

                HtmlNode bodyNode = HtmlNode.CreateNode("<body></body>");
                html.AppendChild(bodyNode);


                int recursiveCount = 0;

                // void appendElementUntilDocumentHeigth(HtmlNode htmlNode)
                // {
                //     if ((htmlNode.ChildNodes.Count == 1 && htmlNode.ChildNodes[0].NodeType == HtmlNodeType.Text) || (htmlNode.ChildNodes.Count == 0 && htmlNode.NodeType != HtmlNodeType.Text))
                //     {
                //         if(htmlNode.Name == "img")
                //         {
                //             string srcPath = htmlNode.Attributes["src"].Value;
                //             srcPath = srcPath.Replace("../", "");

                //             if (htmlNode.Attributes["style"] == null)
                //             {
                //                 htmlNode.Attributes.Add("style", "max-width: 100% !important; max-height: 100% !important;");
                //             }
                //             else
                //             {
                //                 htmlNode.Attributes["style"].Value = "max-width: 100% !important; max-height: 100% !important;";
                //             }

                //             if (htmlNode.Attributes["src"] == null) htmlNode.Attributes.Add("src", "data:image/png;base64," + Convert.ToBase64String(images[srcPath]));
                //             else htmlNode.Attributes["src"].Value = "data:image/png;base64," + Convert.ToBase64String(images[srcPath]);
                //         }
                //         bodyNode.AppendChild(htmlNode);
                //         body.InnerHtml = bodyNode.InnerHtml;
                //         if (pageItemViewForLoad.WebBrowser.Document.Body.ScrollRectangle.Height > browserHeight)
                //         {
                //             bodyNode.RemoveChild(htmlNode);
                //             body.InnerHtml = bodyNode.InnerHtml;
                //             pages.Add(newDocument.DocumentNode.OuterHtml);
                //             newDocument = new HtmlDocument();
                //             headNode = HtmlNode.CreateNode(pageItemViewForLoad.WebBrowser.Document.GetElementsByTagName("head")[0].OuterHtml);
                //             newDocument.DocumentNode.AppendChild(headNode);
                //             bodyNode = HtmlNode.CreateNode("<body></body>");
                //             newDocument.DocumentNode.AppendChild(bodyNode);
                //             bodyNode.AppendChild(htmlNode);
                //             body.InnerHtml = bodyNode.InnerHtml;
                //         }
                //         return;
                //     }
                //     else
                //     {
                //         foreach (HtmlNode childNode in htmlNode.ChildNodes)
                //         {
                //             appendElementUntilDocumentHeigth(childNode);
                //         }
                //     }
                // }
                List<HtmlNode> treeNodes = new List<HtmlNode>();

                void appendElementUntilDocumentHeigth2(HtmlNode currentNode)
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
                            appendElementUntilDocumentHeigth2(childNode);
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

                            if (tempCurrentNode.Attributes["src"] == null) tempCurrentNode.Attributes.Add("src", "data:image/png;base64," + Convert.ToBase64String(images[srcPath]));
                            else tempCurrentNode.Attributes["src"].Value = "data:image/png;base64," + Convert.ToBase64String(images[srcPath]);
                        }
                        body.InnerHtml = treeNodes.First().InnerHtml;
                        bodyNode.InnerHtml = treeNodes.First().InnerHtml;
                        if (pageItemViewForLoad.WebBrowser.Document.Body.ScrollRectangle.Height > controlForLoad.Height)
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
                        HtmlElement element = pageItemViewForLoad.WebBrowser.Document.CreateElement(htmlNode.Name);
                        element.OuterHtml = htmlNode.OuterHtml;
                        pageItemViewForLoad.WebBrowser.Document.GetElementsByTagName("head")[0].AppendChild(element);
                    }
                    else if (htmlNode.Name == "link")
                    {
                        // create style element
                        string stylePath = htmlNode.Attributes["href"].Value;
                        string linkType = htmlNode.Attributes["type"].Value;

                        if (linkType == "text/css")
                        {
                            stylePath = stylePath.Replace("../", "");
                            HtmlElement element = pageItemViewForLoad.WebBrowser.Document.CreateElement("style");
                            string stylesheet = styles[stylePath];
                            

                            // get all font path
                            //string[] fontPaths = Regex.Matches(stylesheet, @"url\((.*?)\)").Cast<Match>().Select(m => m.Groups[1].Value).ToArray();
                            string[] fontPaths = new string[0];
                            foreach (string fontPath in fontPaths)
                            {
                                string fontBase64 = Convert.ToBase64String(fonts[fontPath]);
                                fontBase64 = "data:font/opentype;base64," + fontBase64;
                                stylesheet = stylesheet.Replace(fontPath, fontBase64);
                            }
                            element.OuterHtml = "<style>" + stylesheet + "</style>";
                            HtmlNode styleNode = HtmlNode.CreateNode("<style></style>");
                            styleNode.InnerHtml = stylesheet;
                            headNode.ChildNodes.Add(styleNode);
                            head.AppendChild(element);
                        }
                    }
                }

                foreach (HtmlNode childNode in content.DocumentNode.SelectNodes("//head").Nodes())
                {
                    appendStylesAndFonts(childNode);
                }

                pageItemViewForLoad.WebBrowser.DocumentText = newDocument.DocumentNode.OuterHtml;

                appendElementUntilDocumentHeigth2(content.DocumentNode.SelectSingleNode("//body"));

                



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