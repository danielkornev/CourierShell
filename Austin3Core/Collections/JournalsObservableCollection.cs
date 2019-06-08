using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZU.Apps.Austin3.Storage;
using ZU.Collections.ObjectModel;

namespace ZU.Apps.Austin3.Collections
{
    public partial class JournalsObservableCollection<T> : RangeObservableCollection<T> where T : JournalEntity
    {
        public JournalsObservableCollection(StorageContext storageContext)
        {
            this.StorageContext = storageContext;
        }

        #region Properties
        public StorageContext StorageContext { get; }
        #endregion

        #region Methods
        public void Load()
        {
            var allJournals = this.StorageContext.LoadJournals() as List<T>;

            foreach (var journal in allJournals)
            {
                // saving reference to the Storage Context
                journal.StorageContext = this.StorageContext;
            }

            // loading
            this.AddRange(allJournals);
        }

        public void AddJournal(T journal)
        {
            Exception exception;

            // two steps process
            if (this.StorageContext.TryAddJournal(journal as T, out exception))
            {
                // it is loaded cause we've just created it
                journal.IsLoaded = true;

                // adding to here, to provoke INotifyPropertyChanged event for WPF
                base.Add(journal);
            }
            else
            {
                throw new Exception("Failed to add a Journal: " + journal.DisplayName, exception);
            }
        }

        public void RemoveJournal(T journal)
        {
            Exception exception;

            // two steps process
            if (this.StorageContext.TryMarkJournalAsDeleted(journal as JournalEntity, out exception))
            {
                // removing from here, to provoke INotifyPropertyChanged event for WPF
                base.Remove(journal);
            }
            else
            {
                throw new Exception("Failed to remove a given Journal: " + journal.DisplayName, exception);
            }
        }

        public bool Exists(T journal)
        {
            // ok, this is a dirty hack for now
            if (this.Count(j => j.Id == journal.Id) == 1)
                return true;
            return false;
        }
        #endregion
    } // class
} // namespace
