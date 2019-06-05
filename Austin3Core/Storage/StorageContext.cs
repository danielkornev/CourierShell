
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ZU.Apps.Austin3.Storage
{
    public partial class StorageContext
    {
        public static StorageContext Instance
        {
            get; private set;
        }

        public StorageContext()
        {
            var appData = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var courierShell = System.IO.Path.Combine(appData, "CourierShell");
            System.IO.Directory.CreateDirectory(courierShell);
            var courierShellData = System.IO.Path.Combine(courierShell, "Data");
            System.IO.Directory.CreateDirectory(courierShellData);

            // temp hack
            this.Journals = new ObservableCollection<JournalEntity>();

            // second temp hack
            FillJournalsWithDemoData();

            // setting our poor singleton
            Instance = this;
        }

        private void FillJournalsWithDemoData()
        {
            var n1 = new JournalEntity
            {
                FrontCoverBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(Constants.Colors.Violet)),
                FrontCoverColor = (Color)ColorConverter.ConvertFromString(Constants.Colors.Violet),
                DisplayName = "Space Travel",
                Width = 205,
                X = 350,
                Y = 490
            };

            this.Journals.Add(n1);

            var n2 = new JournalEntity
            {
                FrontCoverBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(Constants.Colors.LightBlue)),
                FrontCoverColor = (Color)ColorConverter.ConvertFromString(Constants.Colors.LightBlue),
                DisplayName = "Short Stories",
                Width = 185,
                X = 80,
                Y = 129
            };

            this.Journals.Add(n2);

            var n3 = new JournalEntity
            {
                FrontCoverBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(Constants.Colors.BeautifulGreen)),
                FrontCoverColor = (Color)ColorConverter.ConvertFromString(Constants.Colors.BeautifulGreen),
                DisplayName = "Meeting Notes",
                Width = 300,
                X = 300,
                Y = 100
            };

            this.Journals.Add(n3);

            var n4 = new JournalEntity
            {
                FrontCoverBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(Constants.Colors.KingBrown)),
                FrontCoverColor = (Color)ColorConverter.ConvertFromString(Constants.Colors.KingBrown),
                DisplayName = "Notes",
                Width = 150,
                X = 650,
                Y = 150
            };

            this.Journals.Add(n4);
        }

        public ObservableCollection<JournalEntity> Journals
        {
            get; internal set;
        }

    } // class
} // namespace
