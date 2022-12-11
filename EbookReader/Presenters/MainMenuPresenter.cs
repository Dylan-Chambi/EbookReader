using EbookReader.Models;
using EbookReader.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EbookReader.Presenters
{
    public class MainMenuPresenter
    {
        private IMainView mainView;
        private IMainViewRepository mainViewRepository;

        private Size oldSize;

        private FormWindowState oldWindowState;

        private string nameFilter = "";

        public MainMenuPresenter(IMainView mainView, IMainViewRepository mainViewRepository)
        {
            this.mainView = mainView;
            this.mainViewRepository = mainViewRepository;

            loadAllEbooksList(nameFilter);

            oldSize = mainView.MainViewForm.Size;
            
            mainView.MainViewForm.ResizeEnd += (sender, e) => {
                // check if form is dragging
                if (oldSize != mainView.MainViewForm.Size)
                {
                    loadAllEbooksList(nameFilter);
                    oldSize = mainView.MainViewForm.Size;
                }
            };

            // event when pressed on maximize button
            mainView.MainViewForm.Resize += (sender, e) => {
                if (oldWindowState != mainView.MainViewForm.WindowState)
                {
                    loadAllEbooksList(nameFilter);
                    oldWindowState = mainView.MainViewForm.WindowState;
                    // center to screen
                    mainView.MainViewForm.StartPosition = FormStartPosition.CenterScreen;
                }
            };

            mainView.SearchTextBox.TextChanged += (sender, e) => {
                nameFilter = mainView.SearchTextBox.Text;
                if (nameFilter == "")
                {
                    loadAllEbooksList(nameFilter);
                }
            };

            mainView.SearchTextBox.KeyDown += (sender, e) => {
                if (e.KeyCode == Keys.Enter)
                {
                    loadAllEbooksList(nameFilter);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            };

            mainView.SearchTextBox.GotFocus += (sender, e) => {
                mainView.SearchTextBox.Text = "";
                mainView.SearchTextBox.ForeColor = Color.Black;
            };

            mainView.SearchTextBox.LostFocus += (sender, e) => {
                if (mainView.SearchTextBox.Text == "")
                {
                    mainView.SearchTextBox.Text = "Search a book here...";
                    mainView.SearchTextBox.ForeColor = Color.Gray;
                }
            };
            
        }


        public void loadAllEbooksList(string nameFilter)
        {
            int maxHeight = mainView.MainViewForm.Height;
            int maxWidth = mainView.MainViewForm.Width;

            // reset table layout
            mainView.TableLayoutPanel.Controls.Clear();
            mainView.TableLayoutPanel.RowCount = 0;
            mainView.TableLayoutPanel.ColumnCount = 0;


            EbookItemView ebookItemViewTemp = new EbookItemView();
            UserControl itemUserControlTemp = ebookItemViewTemp.EbookItemUserControl;

            mainView.TableLayoutPanel.ColumnCount = maxWidth / (itemUserControlTemp.Width + mainView.TableLayoutPanel.Margin.Horizontal/2 + mainView.TableLayoutPanel.Padding.Horizontal/2);

            int widthTable = mainView.TableLayoutPanel.ColumnCount * (itemUserControlTemp.Width + mainView.TableLayoutPanel.Margin.Horizontal/2 + mainView.TableLayoutPanel.Padding.Horizontal/2);
            foreach (Ebook ebook in mainViewRepository.GetEbookItems())
            {
                IEbookItemView ebookItemView = new EbookItemView();
                new EbookItemPresenter(ebookItemView, ebook);
                UserControl itemUserControl = ebookItemView.EbookItemUserControl;
                if (nameFilter != "")
                {
                    if (ebook.EbookTitle.ToLower().Contains(nameFilter.ToLower()))
                    {
                        mainView.TableLayoutPanel.Controls.Add(itemUserControl);
                    }
                }
                else
                {
                    mainView.TableLayoutPanel.Controls.Add(itemUserControl);
                }
            }

            //reset table scrollbars
            mainView.TableLayoutPanel.HorizontalScroll.Maximum = 0;
            mainView.TableLayoutPanel.AutoScroll = false;
            mainView.TableLayoutPanel.VerticalScroll.Visible = false;
            mainView.TableLayoutPanel.AutoScroll = true;
            
        }
    }
}
