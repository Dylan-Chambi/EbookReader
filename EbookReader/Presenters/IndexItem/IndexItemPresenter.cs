using EbookReader.Views.IndexItem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbookReader.Presenters.IndexItem
{
    public class IndexItemPresenter
    {
        private IIndexItemView indexItemView;

        public IndexItemPresenter(IIndexItemView indexItemView, string indexTitle, int indexPageNumber)
        {
            this.indexItemView = indexItemView;

            indexItemView.IndexItemTitle = indexTitle;
            indexItemView.IndexItemClicked += IndexItemView_IndexItemClicked;
        }

        private void IndexItemView_IndexItemClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Index item clicked " + indexItemView.IndexItemPageNumber);
        }
    }
}
