using TestElma.Models;
using System.Collections.Generic;

namespace TestElma.Models
{
    public interface IDocumentManager
    {
        void Add(Document item);
        IEnumerable<Document> List();
        //IEnumerable<Document> List(string search, string field, string sortOrder, bool isAsc);
        IEnumerable<Document> List(string search, string field);
    }
}
