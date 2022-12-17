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

            mainView.SearchTextBox.IconLeftCursor = Cursors.Hand;
            
            mainView.SearchTextBox.IconLeftClick += (sender, e) => {
                loadAllEbooksList(nameFilter);
            };
            
        }


        public void loadAllEbooksList(string nameFilter)
        {
            int maxHeight = mainView.TableLayoutPanel.Height;
            int maxWidth = mainView.TableLayoutPanel.Width;

            // reset table layout
            mainView.TableLayoutPanel.Controls.Clear();
            mainView.TableLayoutPanel.RowCount = 0;
            mainView.TableLayoutPanel.ColumnCount = 0;


            EbookItemView ebookItemViewTemp = new EbookItemView();
            UserControl itemUserControlTemp = ebookItemViewTemp.EbookItemUserControl;

            mainView.TableLayoutPanel.ColumnCount = maxWidth / (itemUserControlTemp.Width + mainView.TableLayoutPanel.Margin.Horizontal/2 + mainView.TableLayoutPanel.Padding.Horizontal/2);

            EbookItemViewAdd ebookItemViewAdd = new EbookItemViewAdd();
            new EbookItemViewAddPresenter(ebookItemViewAdd);

            mainView.TableLayoutPanel.Controls.Add(ebookItemViewAdd.EbookItemAddUserControl);

        

            ebookItemViewAdd.ButtonAdd.Click += (sender, e) => {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                // only allow to select one file and .epub
                openFileDialog.Multiselect = false;
                openFileDialog.Filter = "Epub files (*.epub)|*.epub";
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string fileName in openFileDialog.FileNames)
                    {
                        // check if file is already in database
                        if (mainViewRepository.GetEbookItems().Where(x => x.EbookPath == fileName).Count() == 0)
                        {
                            try {
                                // add to database
                            mainViewRepository.AddEbookItem(new EbookEpub(fileName));
                            // add to table
                            IEbookItemView ebookItemView = new EbookItemView();
                            new EbookItemPresenter(ebookItemView, mainViewRepository.GetEbookItems().Where(x => x.EbookPath == fileName).First());
                            UserControl itemUserControl = ebookItemView.EbookItemUserControl;
                            mainView.TableLayoutPanel.Controls.Add(itemUserControl);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("This file is not a valid ebook file or has an unsupported format",
                                "Invalid Ebook file",
                                 MessageBoxButtons.OK, 
                                 MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("The file that you are trying to add is already in the database",
                                "File already in database",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                        }
                    }
                }
            };
            int totalWidth = 0;

            foreach (Ebook ebook in mainViewRepository.GetEbookItems())
            {
                totalWidth += itemUserControlTemp.Width + mainView.TableLayoutPanel.Margin.Horizontal / 2 + mainView.TableLayoutPanel.Padding.Horizontal / 2;
                IEbookItemView ebookItemView = new EbookItemView();
                new EbookItemPresenter(ebookItemView, ebook);
                UserControl itemUserControl = ebookItemView.EbookItemUserControl;
                if (nameFilter != "")
                {
                    if (ebook.EbookTitle.ToLower().Contains(nameFilter.ToLower()))
                    {
                        mainView.TableLayoutPanel.Controls.Add(itemUserControl);
                        if (totalWidth > maxWidth)
                        {
                            mainView.TableLayoutPanel.RowCount++;
                        }
                    }

                }
                else
                {
                    mainView.TableLayoutPanel.Controls.Add(itemUserControl);
                    if (totalWidth > maxWidth)
                    {
                        mainView.TableLayoutPanel.RowCount++;
                    }
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
