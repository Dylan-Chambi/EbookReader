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
            LoadChaptersList();
            LoadChapterPages(8);
        }


        public void LoadChaptersList()
        {
            foreach (Chapter chapter in ebook.getChapters())
            {
                IIndexItemView indexItemView = new IndexItemView();
                new IndexItemPresenter(indexItemView, chapter.ChapterTitle, chapter.ChapterIndexPage);
                bookReadView.FlowLayoutPanel.Controls.Add((Control)indexItemView);
            }
        }

        public void LoadChapterPages(int chapter)
        {
            IPageItemView pageItemView = new PageItemView();
            new PageItemPresenter(pageItemView, "");
            Control control = (UserControl)pageItemView;
            bookReadView.TableLayoutPanel.Controls.Add(control);
            control.Dock = DockStyle.Fill;
            pageItemView.WebBrowser.ScrollBarsEnabled = false;

            IPageItemView pageItemView2 = new PageItemView();
            new PageItemPresenter(pageItemView2, "");
            Control control2 = (UserControl)pageItemView2;
            bookReadView.TableLayoutPanel.Controls.Add(control2);
            control2.Dock = DockStyle.Fill;
            pageItemView2.WebBrowser.ScrollBarsEnabled = false;

            IPageItemView pageItemViewForLoad = new PageItemView();
            new PageItemPresenter(pageItemViewForLoad, "");
            Control controlForLoad = (UserControl)pageItemViewForLoad;
            bookReadView.TableLayoutPanel.Controls.Add(controlForLoad);
            controlForLoad.Dock = DockStyle.Fill;
            pageItemViewForLoad.WebBrowser.ScrollBarsEnabled = false;
            controlForLoad.Visible = false;


            control.BeginInvoke ((MethodInvoker)delegate {
                control.Width = bookReadView.TableLayoutPanel.Width / 2;
                control.Height = bookReadView.TableLayoutPanel.Height;
                control2.Width = bookReadView.TableLayoutPanel.Width / 2;
                control2.Height = bookReadView.TableLayoutPanel.Height;
                controlForLoad.Width = bookReadView.TableLayoutPanel.Width / 2;
                controlForLoad.Height = bookReadView.TableLayoutPanel.Height;
                SplitDocument(ebook.getChaptersContent()[chapter], control.Height, pageItemViewForLoad.WebBrowser.Document.Body.ScrollRectangle.Height);
            });

            bookReadView.TableLayoutPanel.SizeChanged += (sender, e) =>
            {
                control.Width = bookReadView.TableLayoutPanel.Width / 2;
                control.Height = bookReadView.TableLayoutPanel.Height;
                control2.Width = bookReadView.TableLayoutPanel.Width / 2;
                control2.Height = bookReadView.TableLayoutPanel.Height;
                controlForLoad.Width = bookReadView.TableLayoutPanel.Width / 2;
                controlForLoad.Height = bookReadView.TableLayoutPanel.Height;

                SplitDocument(ebook.getChaptersContent()[chapter], control.Height, pageItemViewForLoad.WebBrowser.Document.Body.ScrollRectangle.Height);

            };

            void SplitDocument(string HtmlContent, int browserHeight, int documentHeight)
            {
                // remvoe all content in html element
                pageItemViewForLoad.WebBrowser.Document.Body.InnerHtml = "";
                HtmlElement body = pageItemViewForLoad.WebBrowser.Document.Body;
                
                List<string> pages = new List<string>();
                int numPages = (int)Math.Ceiling((double)((double)documentHeight) / ((double)browserHeight));

                HtmlDocument content = new HtmlDocument();
                content.LoadHtml(HtmlContent);

                HtmlDocument newDocument = new HtmlDocument();
                HtmlNode head = HtmlNode.CreateNode(content.DocumentNode.SelectSingleNode("//head").OuterHtml);
                newDocument.DocumentNode.AppendChild(head);

                HtmlNode bodyNode = HtmlNode.CreateNode("<body></body>");
                newDocument.DocumentNode.AppendChild(bodyNode);



                void appendElementUntilDocumentHeigth(HtmlNode htmlNode)
                {
                    if ((htmlNode.ChildNodes.Count == 1 && htmlNode.ChildNodes[0].NodeType == HtmlNodeType.Text) || (htmlNode.ChildNodes.Count == 0 && htmlNode.NodeType != HtmlNodeType.Text))
                    {
                        HtmlElement element = pageItemViewForLoad.WebBrowser.Document.CreateElement(htmlNode.Name);
                        if (htmlNode.HasChildNodes)
                        {
                            element.InnerHtml = htmlNode.InnerHtml;
                        }
                        body.AppendChild(element);
                        bodyNode.ChildNodes.Add(htmlNode);
                        if (body.ScrollRectangle.Height + 20 > browserHeight)
                        {
                            bodyNode.RemoveChild(htmlNode);
                            pages.Add(newDocument.DocumentNode.OuterHtml);
                            newDocument = new HtmlDocument();
                            head = HtmlNode.CreateNode(pageItemViewForLoad.WebBrowser.Document.GetElementsByTagName("head")[0].OuterHtml);
                            newDocument.DocumentNode.AppendChild(head);

                            element = pageItemViewForLoad.WebBrowser.Document.CreateElement(htmlNode.Name);
                            if (htmlNode.HasChildNodes)
                            {
                                element.InnerHtml = htmlNode.InnerHtml;
                            }

                            body.InnerHtml = "";
                            body.AppendChild(element);

                            bodyNode = HtmlNode.CreateNode("<body></body>");
                            newDocument.DocumentNode.AppendChild(bodyNode);

                            bodyNode.ChildNodes.Add(htmlNode);
                        }
                        return;
                    }
                    else
                    {
                        foreach (HtmlNode childNode in htmlNode.ChildNodes)
                        {
                            appendElementUntilDocumentHeigth(childNode);
                        }
                    }
                }

                appendElementUntilDocumentHeigth(content.DocumentNode.SelectSingleNode("//body"));

                pageItemView.WebBrowser.DocumentText = pages[0];
                pageItemView2.WebBrowser.DocumentText = pages[1];
            }
        }

    }
}