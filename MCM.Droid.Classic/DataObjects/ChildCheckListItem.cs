using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Android.App;

using Microsoft.WindowsAzure.MobileServices;

namespace MCM.Droid.Classic.DataObjects
{
    public class ChildCheckListItem
    {
        private GlobalVars _globalVars;

        public string Id { get; set; }
        public string ChildId { get; set; }
        public int IDCheckListItemId { get; set; }

        public ChildCheckListItem Save(Activity activityContext)
        {
            _globalVars = ((GlobalVars)activityContext.Application);
            if (string.IsNullOrWhiteSpace(Id))
            {
                InsertToTable(activityContext);
            }

            return this;
        }

        public void Delete(Activity activityContext)
        {
            _globalVars = ((GlobalVars)activityContext.Application);
            DeleteFromTable(activityContext);
        }

        private void InsertToTable(Activity activityContext)
        {
            var childIDCheckItemTable = _globalVars.MobileServiceClient.GetTable<DataObjects.ChildCheckListItem>();

            Task t = Task.Run(() => childIDCheckItemTable.InsertAsync(this));

            t.Wait();
        }

        private void UpdateTable(Activity activityContext)
        {
            var childIDCheckItemTable = _globalVars.MobileServiceClient.GetTable<DataObjects.ChildCheckListItem>();
            Task t = Task.Run(() => childIDCheckItemTable.UpdateAsync(this));

            t.Wait();
        }

        private void DeleteFromTable(Activity activityContext)
        {
            Task t = null;

            var childIDCheckItemTable = _globalVars.MobileServiceClient.GetTable<DataObjects.ChildCheckListItem>();
            t = Task.Run(() => childIDCheckItemTable.DeleteAsync(this));

            t.Wait();
        }
    }
}