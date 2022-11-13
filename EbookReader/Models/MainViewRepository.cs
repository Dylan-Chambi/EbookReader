using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbookReader.Models
{
    public class MainViewRepository : IMainViewRepository
    {
        public void AddEbookItem(Ebook ebookItem)
        {
            throw new NotImplementedException();
        }

        public List<Ebook> GetEbookItems()
        {
            EbookEpub epub1 = new EbookEpub(@"C:\\Users\\DylanPC\\source\\repos\\EbookReader\\EbookReader\\Resources\\Batman_ Nightwalker - Marie Lu.epub");
            EbookEpub epub2 = new EbookEpub(@"C:\\Users\\DylanPC\\source\\repos\\EbookReader\\EbookReader\\Resources\\Batman_ Nightwalker - Marie Lu.epub");
            EbookEpub epub3 = new EbookEpub(@"C:\\Users\\DylanPC\\source\\repos\\EbookReader\\EbookReader\\Resources\\Batman_ Nightwalker - Marie Lu.epub");
            EbookEpub epub4 = new EbookEpub(@"C:\\Users\\DylanPC\\source\\repos\\EbookReader\\EbookReader\\Resources\\Batman_ Nightwalker - Marie Lu.epub");
            
            List<Ebook> ebookItems = new List<Ebook>();

            ebookItems.Add(epub1);
            ebookItems.Add(epub2);
            ebookItems.Add(epub3);
            ebookItems.Add(epub4);

            return ebookItems;
        }

        public List<Ebook> GetEbookItemsByTitle(string title)
        {
            throw new NotImplementedException();
        }

        public void RemoveEbookItem(Ebook ebookItem)
        {
            throw new NotImplementedException();
        }
    }
}
