
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZU.Apps.Austin3.Storage
{
    public partial class StorageContext
    {
        public StorageContext()
        {
            var appData = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var courierShell = System.IO.Path.Combine(appData, "CourierShell");
            System.IO.Directory.CreateDirectory(courierShell);
            var courierShellData = System.IO.Path.Combine(courierShell, "Data");
            System.IO.Directory.CreateDirectory(courierShellData);

            

        }

        public ObservableCollection<JournalEntity> Journals
        {
            get; internal set;
        }

    } // class
} // namespace
