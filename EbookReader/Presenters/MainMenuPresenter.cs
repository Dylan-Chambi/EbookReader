using EbookReader.Models;
using EbookReader.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public MainMenuPresenter(IMainView mainView, IMainViewRepository mainViewRepository)
        {
            this.mainView = mainView;
            this.mainViewRepository = mainViewRepository;

            loadAllEbooksList();
            
            
        }

        public void loadAllEbooksList()
        {
            foreach (Ebook ebook in mainViewRepository.GetEbookItems())
            {
                IEbookItemView ebookItemView = new EbookItemView();
                new EbookItemPresenter(ebookItemView, ebook);
                mainView.FlowLayoutPanel.Controls.Add((Control)ebookItemView);
            }
        }
    }
}
