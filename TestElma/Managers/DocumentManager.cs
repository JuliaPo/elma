using System;
using System.Collections.Generic;
using System.Linq;
using TestElma.Models;
using NHibernate;
using TestElma.Helpers;
using NHibernate.Criterion;

namespace TestElma.Models
{
    public class DocumentManager : IDocumentManager
    {
        public void Add(Document item)
        {

            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Save(item);
                        session.Flush();
                        transaction.Commit();
                        session.Close();
                    }

                    catch (HibernateException)
                    {
                        if (transaction.IsActive)
                            transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public IEnumerable<Document> List()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var criteria = session.CreateCriteria(typeof(Document));

                var docs = criteria.List<Document>();

                return docs;
            }
        }

        public IEnumerable<Document> List(string search, string field)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var criteria = session.CreateCriteria(typeof(Document));

                if (!string.IsNullOrWhiteSpace(search) && !string.IsNullOrWhiteSpace(field))
                {
                    criteria.Add(Restrictions.Like(field, search, MatchMode.Anywhere));
                }

                var docs = criteria.List<Document>();

                return docs;
            }
        }
        //public IEnumerable<Document> List(string search, string field, string sortOrder, bool isAsc)
        //{
        //    using (ISession session = NHibernateHelper.OpenSession())
        //    {
        //        var criteria = session.CreateCriteria(typeof(Document));

        //        if (!string.IsNullOrWhiteSpace(search) && !string.IsNullOrWhiteSpace(field))
        //        {
        //            criteria.Add(Restrictions.Like(field, search, MatchMode.Anywhere));
        //        }

        //        if (string.IsNullOrWhiteSpace(sortOrder))
        //            sortOrder = "Name";

        //        var docs = criteria.List<Document>().OrderBy(d => d.GetType()
        //                                                           .GetProperty(sortOrder)
        //                                                           .GetValue(d));
        //        if (!isAsc)
        //            docs.Reverse();
        //        return docs;
        //    }
        //}
    }
}
