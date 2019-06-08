using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZU.Apps.Austin3.Storage;
using ZU.Collections.ObjectModel;

namespace ZU.Apps.Austin3.Collections
{
    public partial class JournalPagesObservableCollection<T> : RangeObservableCollection<T> where T : JournalPageEntity
    {
        public JournalPagesObservableCollection(JournalEntity journal) 
        {
            this.Journal = journal;
            this.StorageContext = journal.StorageContext;
        }

        #region Properties
        public JournalEntity Journal { get; }
        public StorageContext StorageContext { get; }
        #endregion

        #region Methods
        public void Load()
        {
            // we don't run this method if the Journal has been just created and/or loaded
            if (this.Journal.IsLoaded) return;

            // yeah, dirty hack, too, but good enough for now (obviously we shall find a more lazy approach to load pages)
            var allPages = this.StorageContext.LoadPages(this.Journal) as List<T>;

            foreach (var page in allPages)
            {
                // saving reference to the Storage Context
                page.StorageContext = this.StorageContext;

                // saving reference to parent Journal
                page.Journal = this.Journal;
            }



            // loading
            this.AddRange(allPages);
        }

        public void AddPage(T page)
        {
            Exception exception;

            // two steps process
            if (this.StorageContext.TryAddPage(this.Journal, page as JournalPageEntity, out exception))
            {
                // adding to here, to provoke INotifyPropertyChanged event for WPF
                base.Add(page);
            }
            else
            {
                throw new Exception("Failed to add a given page to Journal: " + this.Journal.DisplayName, exception);
            }
        }

        public void RemovePage(T page)
        {
            Exception exception;

            // two steps process
            if (this.StorageContext.TryRemovePage(this.Journal, page as JournalPageEntity, out exception))
            {
                // removing from here, to provoke INotifyPropertyChanged event for WPF
                base.Remove(page);
            }
            else
            {
                throw new Exception("Failed to remove a given page from Journal: " + this.Journal.DisplayName, exception);
            }
        }

        public bool Exists(T page)
        {
            // ok, this is a dirty hack for now
            if (this.Count(p => p.Id == page.Id) == 1)
                return true;
            return false;
        }
        #endregion
    } // class
} // namespace
