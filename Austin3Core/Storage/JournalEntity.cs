using LiteDB;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using ZU.Apps.Austin3.Collections;
using ZU.Collections.ObjectModel;

namespace ZU.Apps.Austin3.Storage
{
    /// <summary>
    /// Represents an entity hosted in the Zoomable Canvas
    /// </summary>
    public partial class JournalEntity
    {
        #region Fields
        SolidColorBrush _frontCoverBrush;
        Color _frontCoverColor;
        JournalPagesObservableCollection<JournalPageEntity> _pages;

        #endregion

        #region Computed Properties
        /// <summary>
        /// WPF-friendly Solid Color Brush for the Front Cover
        /// </summary>
        [BsonIgnore]
        public SolidColorBrush FrontCoverBrush
        {
            get
            {
                if (this._frontCoverBrush == null)
                {
                    this._frontCoverBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(this.FrontCoverSolidBrushHex));
                    this._frontCoverBrush.Freeze();
                }
                return this._frontCoverBrush;
            }
        }

        /// <summary>
        /// WPF-friendly Solid Color for the Front Cover
        /// </summary>
        [BsonIgnore]
        public Color FrontCoverColor
        {
            get
            {
                if (this._frontCoverColor == null)
                    this._frontCoverColor = (Color)ColorConverter.ConvertFromString(this.FrontCoverSolidColorHex);
                return this._frontCoverColor;
            }
        }

        /// <summary>
        /// Custom Image for the Journal
        /// </summary>
        [BsonIgnore]
        public ImageSource FrontCoverImageBrush { get; set; }

        [BsonIgnore]
        public StorageContext StorageContext { get; internal set; }

        [BsonIgnore]
        public bool IsLoaded { get; internal set; }
        #endregion

        #region Properties
        /// <summary>
        /// X coordinate on "All Journals" infinite canvas
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y coordinate on "All Journals" infinite canvas
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Display Name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Date Created
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Date Updated
        /// </summary>
        public DateTime DateUpdated { get; set; }

        /// <summary>
        /// Date Soft Deleted
        /// </summary>
        public DateTime? DateDeleted { get; set; }

        /// <summary>
        /// Custom Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Specifies whether this Journal was soft deleted or not.
        /// </summary>
        public bool IsSoftedDeleted { get; set; }

        /// <summary>
        /// Zoom Level. Defines the size of the journal on "All Journals" infinite canvas.
        /// </summary>
        public double ZoomLevel { get; set; }

        /// <summary>
        /// HEX representation of the Front Cover Brush
        /// </summary>
        public string FrontCoverSolidBrushHex { get; set; }

        /// <summary>
        /// HEX representation of the Front Cover Solid Color
        /// </summary>
        public string FrontCoverSolidColorHex{ get; set; }

        #endregion


        #region Temporary Properties
        /// <summary>
        /// Will be auto-calculated based on the Zoom level. Keeping it here for now.
        /// </summary>
        public double Width { get; set; }

        #endregion

        #region Constructor
        public JournalEntity()
        {
            
        }
        #endregion

        #region Collections
        /// <summary>
        /// List of all contained Journal Pages. Should be lazy loadable.
        /// </summary>
        [BsonIgnore]
        public JournalPagesObservableCollection<JournalPageEntity> Pages
        {
            get
            {
                if (this._pages==null)
                {
                    this._pages = new JournalPagesObservableCollection<JournalPageEntity>(this);
                }
                return this._pages;
            }
        }

        public int LastOpenPage { get; set; }
        #endregion

        #region Methods

        public void AddTwoMorePages()
        {
            var p1 = new JournalPageEntity
            {
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsCoverPage = false,
                IsSoftDeleted = false            };

            this.Pages.AddPage(p1);

            var p2 = new JournalPageEntity
            {
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsCoverPage = false,
                IsSoftDeleted = false
            };

            this.Pages.AddPage(p2);
        }


        #endregion
    } // class
} // namespace
