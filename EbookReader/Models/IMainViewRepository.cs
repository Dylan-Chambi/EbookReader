using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbookReader.Models
{
    public interface IMainViewRepository
    {
        void AddEbookItem(Ebook ebookItem);
        void RemoveEbookItem(Ebook ebookItem);
        List<Ebook> GetEbookItems();
        List<Ebook> GetEbookItemsByTitle(string title);
    }
}
