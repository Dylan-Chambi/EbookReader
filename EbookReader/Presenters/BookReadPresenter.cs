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

        List<string> pages = new List<string>();

        List<Chapter> chapters = new List<Chapter>();
        List<string> chaptersContent = new List<string>();

        private int currentChapter = 1;
        private int currentPage = 0;

        private IPageItemView pageItemView;
        private IPageItemView pageItemView2;
        private IPageItemView pageItemViewForLoad;

        private Control control;
        private Control control2;
        private Control controlForLoad;

        private string backgroundColor = "#ffffff";
        private string textColor = "#000000";

        private string selectedButtonColor = "#000000";
        private string selectedButtonTextColor = "#ffffff";

        private string normalButtonColor;
        private string normalButtonTextColor;
        private int currentBrowserHeight = 0;
        private int currentBrowserWidth = 0;

        private Control prevButton;
        private Dictionary<int, Control> buttons = new Dictionary<int, Control>();

        private Size oldSize;

        private FormWindowState oldWindowState;

        private Timer myTimer = new Timer();


        public BookReadPresenter(IBookReadView bookReadView, Ebook ebook)
        {
            this.bookReadView = bookReadView;
            this.ebook = ebook;

            bookReadView.CurrentEbook = ebook;
            images = ebook.getImages();
            fonts = ebook.getFonts();
            styles = ebook.getStylesheets();
            chaptersContent = ebook.getChaptersContent();
            InitPageItems();
            

            bookReadView.ListIndexTable.SizeChanged += (sender, e) =>
            {
                bookReadView.ListIndexTable.Height = bookReadView.EbookReadForm.Height - bookReadView.ListIndexTable.Location.Y - 20;
            };

            // execute after 1 second
            myTimer.Interval = 200;
            myTimer.Tick += new EventHandler(
                (sender, e) =>
                {
                    oldWindowState = bookReadView.EbookReadForm.WindowState;
                    controlForLoad.Width = control.Width;
                    controlForLoad.Height = control.Height;

                    currentPage = (currentPage * currentBrowserHeight * currentBrowserWidth) / (controlForLoad.Height * controlForLoad.Width);

                    if (currentPage % 2 == 1)
                    {
                        currentPage--;
                    }
                    LoadChapterPages(currentChapter);
                    SetBrowsersDocumentPages();
                    currentBrowserHeight = controlForLoad.Height;
                    currentBrowserWidth = controlForLoad.Width;

                    bookReadView.EbookReadForm.StartPosition = FormStartPosition.CenterScreen;
                    myTimer.Stop();
                }
            );
            
            myTimer.Start();


        }

        private void InitPageItems()
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


            controlForLoad.BeginInvoke((MethodInvoker)delegate ()
            {
                controlForLoad.Width = control.Width;
                controlForLoad.Height = control.Height;
                currentPage = 0;
                currentChapter = 1;
                LoadChapterPages(currentChapter);
                SetBrowsersDocumentPages();
                LoadChaptersList();
                HandleChaptersChange(currentChapter);
                currentBrowserHeight = controlForLoad.Height;
                currentBrowserWidth = controlForLoad.Width;
            });

            bookReadView.EbookReadForm.ResizeEnd += (sender, e) =>
            {
                if (oldSize != bookReadView.EbookReadForm.Size)
                {
                    oldSize = bookReadView.EbookReadForm.Size;
                    controlForLoad.Width = control.Width;
                    controlForLoad.Height = control.Height;

                    currentPage = (currentPage * currentBrowserHeight * currentBrowserWidth) / (controlForLoad.Height * controlForLoad.Width);

                    if (currentPage % 2 == 1)
                    {
                        currentPage--;
                    }
                    LoadChapterPages(currentChapter);
                    SetBrowsersDocumentPages();
                    currentBrowserHeight = controlForLoad.Height;
                    currentBrowserWidth = controlForLoad.Width;
                }
            };

            bookReadView.EbookReadForm.Resize += (sender, e) =>
            {
                if (oldWindowState != bookReadView.EbookReadForm.WindowState)
                {
                    oldWindowState = bookReadView.EbookReadForm.WindowState;
                    controlForLoad.Width = control.Width;
                    controlForLoad.Height = control.Height;

                    currentPage = (currentPage * currentBrowserHeight * currentBrowserWidth) / (controlForLoad.Height * controlForLoad.Width);

                    if (currentPage % 2 == 1)
                    {
                        currentPage--;
                    }
                    LoadChapterPages(currentChapter);
                    SetBrowsersDocumentPages();
                    currentBrowserHeight = controlForLoad.Height;
                    currentBrowserWidth = controlForLoad.Width;

                    bookReadView.EbookReadForm.StartPosition = FormStartPosition.CenterScreen;
                }
            };

            // avoid web browser preview key down event to be fired twice
            pageItemView.WebBrowser.PreviewKeyDown += new PreviewKeyDownEventHandler((sender, e) =>
            {
                Debug.WriteLine("PReviewKeyDown: " + e.KeyCode);
                e.IsInputKey = true;
                if (e.KeyCode == Keys.Right)
                {
                    HandleRigthKeyPressed();
                }
                else if (e.KeyCode == Keys.Left)
                {
                    HandleLeftKeyPressed();
                }
            });

            pageItemView2.WebBrowser.PreviewKeyDown += new PreviewKeyDownEventHandler((sender, e) =>
            {
                Debug.WriteLine("PReviewKeyDown: " + e.KeyCode);
                e.IsInputKey = true;
                if (e.KeyCode == Keys.Right)
                {
                    HandleRigthKeyPressed();
                }
                else if (e.KeyCode == Keys.Left)
                {
                    HandleLeftKeyPressed();
                }
            });

            bookReadView.EbookReadForm.KeyPreview = true;
            bookReadView.EbookReadForm.KeyDown += new KeyEventHandler((sender, e) =>
            {
                Control focusedControl = bookReadView.EbookReadForm.ActiveControl;
                if (focusedControl == control || focusedControl == control2)
                {
                    return;
                }
                Debug.WriteLine("KeyDown: " + e.KeyCode);
                if (e.KeyCode == Keys.Right)
                {
                    HandleRigthKeyPressed();
                }
                else if (e.KeyCode == Keys.Left)
                {
                    HandleLeftKeyPressed();
                }
            });
            
        }

        private void HandleChaptersChange(int chapter)
        {
            if (prevButton != null)
            {
                prevButton.BackColor = ColorTranslator.FromHtml(normalButtonColor);
                prevButton.ForeColor = ColorTranslator.FromHtml(normalButtonTextColor);
            }
            if (buttons.ContainsKey(currentChapter))
            {
                buttons[chapter].BackColor = ColorTranslator.FromHtml(selectedButtonColor);
                buttons[chapter].ForeColor = ColorTranslator.FromHtml(selectedButtonTextColor);
                prevButton = buttons[chapter];
            }
        }

        private void HandleRigthKeyPressed()
        {
            if (currentPage < pages.Count - 2)
            {
                currentPage += 2;
                SetBrowsersDocumentPages();
            }
            else
            {
                if (currentChapter < chaptersContent.Count - 1)
                {
                    currentChapter++;
                    currentPage = 0;
                    LoadChapterPages(currentChapter);
                    SetBrowsersDocumentPages();
                    HandleChaptersChange(currentChapter);
                }
            }
        }

        private void HandleLeftKeyPressed()
        {
            if (currentPage > 1)
            {
                currentPage -= 2;
                SetBrowsersDocumentPages();
            }
            else
            {
                if (currentChapter > 1)
                {
                    currentChapter--;
                    LoadChapterPages(currentChapter);
                    currentPage = (pages.Count - 1) - (pages.Count - 1) % 2;
                    SetBrowsersDocumentPages();
                    HandleChaptersChange(currentChapter);
                }
            }
        }

        private void SetBrowsersDocumentPages()
        {
            if (currentPage % 2 == 1) //TODO: check if this is solution for the problem
            {
                currentPage--;
            }
            pageItemView.WebBrowser.DocumentText = pages[currentPage];
            if (currentPage < pages.Count - 1)
            {
                pageItemView2.WebBrowser.DocumentText = pages[currentPage + 1];
            }
            else
            {
                pageItemView2.WebBrowser.DocumentText = "";
            }
        }


        private void LoadChaptersList()
        {
            chapters = ebook.getChapters();
            buttons = new Dictionary<int, Control>();
            prevButton = null;

            foreach (Chapter chapter in chapters)
            {
                IIndexItemView indexItemView = new IndexItemView();
                new IndexItemPresenter(indexItemView, chapter.ChapterTitle, chapter.ChapterIndexPage);
                Control control = (UserControl)indexItemView;
                bookReadView.ListIndexTable.Controls.Add(control);
                control.Dock = DockStyle.Top;
                normalButtonColor = indexItemView.IndexItemButton.BackColor.Name;

                indexItemView.IndexItemButton.Click += (sender, e) =>
                {
                    currentPage = 0;
                    currentChapter = indexItemView.IndexItemPageNumber;
                    LoadChapterPages(currentChapter);
                    SetBrowsersDocumentPages();
                    indexItemView.IndexItemButton.BackColor = ColorTranslator.FromHtml(selectedButtonColor);
                    HandleChaptersChange(indexItemView.IndexItemPageNumber);
                };
                indexItemView.IndexItemButton.PreviewKeyDown += new PreviewKeyDownEventHandler((sender, e) =>
                {
                    e.IsInputKey = true;
                });
                // check if key is not in dictionary
                if (!buttons.ContainsKey(indexItemView.IndexItemPageNumber)){
                    buttons.Add(indexItemView.IndexItemPageNumber, indexItemView.IndexItemButton);
                }
            }
        }



        private void LoadChapterPages(int chapter)
        {
            pageItemViewForLoad.WebBrowser.DocumentText = "";
            HtmlElement body = pageItemViewForLoad.WebBrowser.Document.Body;


            pages = new List<string>();

            HtmlDocument content = new HtmlDocument();
            content.LoadHtml(chaptersContent[chapter]);

            HtmlDocument newDocument = new HtmlDocument();
            newDocument.OptionOutputAsXml = true;
            HtmlNode html = newDocument.CreateElement("html");
            newDocument.DocumentNode.AppendChild(html);

            HtmlNode headNode = HtmlNode.CreateNode("<head></head>");
            html.AppendChild(headNode);

            HtmlNode metaEdge = HtmlNode.CreateNode("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">");
            headNode.AppendChild(metaEdge);

            HtmlNode metaCharset = HtmlNode.CreateNode("<meta charset=\"utf-8\">");
            headNode.AppendChild(metaCharset);

            //set viewport width and height
            HtmlNode metaViewport = HtmlNode.CreateNode("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
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

                if (treeNodes.Count > 0)
                {
                    parentNode = treeNodes.Last();
                }
                HtmlNode copyCurrentNode = HtmlNode.CreateNode(currentNode.OuterHtml);
                copyCurrentNode.RemoveAllChildren();

                if (currentNode.ChildNodes.Count > 0)
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
                    if (tempCurrentNode.Name == "img")
                    {
                        string srcPath = tempCurrentNode.Attributes["src"].Value;
                        srcPath = srcPath.Replace("../", "");

                        if (tempCurrentNode.Attributes["style"] == null) tempCurrentNode.Attributes.Add("style", "max-width:60vw;max-height:60vh;");
                        else tempCurrentNode.Attributes["style"].Value = "max-width:60vw;max-height:60vh;";

                        if (tempCurrentNode.Attributes["src"] == null) tempCurrentNode.Attributes.Add("src", "data:image/png;base64," + Convert.ToBase64String(images[srcPath]));
                        else tempCurrentNode.Attributes["src"].Value = "data:image/png;base64," + Convert.ToBase64String(images[srcPath]);
                    }
                    body.InnerHtml = treeNodes.First().InnerHtml;
                    bodyNode.InnerHtml = treeNodes.First().InnerHtml;
                    if (pageItemViewForLoad.WebBrowser.Document.Body.ScrollRectangle.Height + 100 > controlForLoad.Height)
                    {
                        if (treeNodes.Count > 0)
                        {
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

            pages.Add(newDocument.DocumentNode.OuterHtml);
        }

    }
}