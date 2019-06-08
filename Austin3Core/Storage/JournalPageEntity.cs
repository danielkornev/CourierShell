using ZU.Shared.Wpf.Ink;
using LiteDB;
using System;
using System.Windows.Ink;
using System.Windows;
using System.ComponentModel;

namespace ZU.Apps.Austin3.Storage
{
    public class JournalPageEntity : INotifyPropertyChanged
    {
        #region Fields
        InkStrokeCollectionConverter _inkStrokeConverter = new InkStrokeCollectionConverter();
        JournalEntity _journal;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateDeleted { get; set; }
        public bool IsSoftDeleted { get; set; }
        public string InkStrokes { get; set; }
        public bool IsCoverPage { get; set; }


        [BsonIgnore]
        public JournalEntity Journal
        {
            get; internal set;
        }



        /// <summary>
        /// Thumbnail is important; later we'll use it to minimize loading time
        /// </summary>
        public string PageThumbnailUri { get; set; }

        // public PagePapers PagePaper        
        #endregion

        #region Computer Properties & Collections
        [BsonIgnore]
        public bool IsOdd
        {
            get
            {
                if (Id % 2 == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        [BsonIgnore]
        public StorageContext StorageContext { get; internal set; }

        [BsonIgnore]
        public StrokeCollection InkLayer
        {
            get
            {
                return _inkStrokeConverter.Convert(InkStrokes, typeof(StrokeCollection), null, null) as StrokeCollection;
            }
            set
            {
                
            }
        }
        // public ObservableCollection<VisualObject> ObjectLayer { get; set; }
        // public ObservableCollection<string> SearchableTextLayer {get; set; }
        #endregion

        public JournalPageEntity()
        {
            // loading ink 

            // loading objects
            // TODO
        }

        #region INotifyPropertyChanged Implementation
        public virtual void Changed(string propertyName)
        {
            if (PropertyChanged != null)
            {
                var action =
                    new Action
                    (
                        delegate
                        {
                            Platform.InputInvalidate(propertyName);
                            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                        }
                    );
                Platform.InvokeInUI(action);
            }
        }
        #endregion
    } // class
} // namespace
