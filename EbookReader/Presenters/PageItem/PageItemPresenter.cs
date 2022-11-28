using EbookReader.Views.PageItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersOne.Epub;

namespace EbookReader.Presenters.PageItem
{
    public class PageItemPresenter
    {
        private IPageItemView pageItemView;

        public PageItemPresenter(IPageItemView pageItemView, string content)
        {
            this.pageItemView = pageItemView;

            pageItemView.Content = content;
        }
    }
}
