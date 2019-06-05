using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace ZU.Apps.Austin3.Storage
{
    public class JournalPageEntity
    {
        #region Properties
        public int PageNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        // public PagePapers PagePaper        
        #endregion

        #region Collections
        public StrokeCollection InkLayer { get; set; }
        // public ObservableCollection<VisualObject> ObjectLayer { get; set; }
        // public ObservableCollection<string> SearchableTextLayer {get; set; }
        #endregion

        public JournalPageEntity()
        {
            this.InkLayer = new StrokeCollection();
        }
    } // class
} // namespace
