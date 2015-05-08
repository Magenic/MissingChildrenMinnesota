using System;
using System.Threading.Tasks;

using Android.App;

namespace MCM.Droid.Classic.DataObjects
{
    public class DistinguishingFeature
    {
        private GlobalVars _globalVars;
        //private System.Threading.Timer _timer;
        //private int _timerCount = 0;

        public string Id { get; set; }
        public string ChildId { get; set; }
        public string Feature { get; set; }
        public int BodyChartNbr { get; set; }

        public DistinguishingFeature Save(Activity activityContext)
        {
            _globalVars = ((GlobalVars)activityContext.Application);
            if (string.IsNullOrWhiteSpace(Id))
            {
                InsertToTable(activityContext);
            }
            else
            {
                UpdateTable(activityContext);
            }

            return this;
        }

        public void Delete(Activity activityContext)
        {
            DeleteFromTable(activityContext);
        }

        private void InsertToTable(Activity activityContext)
        {
            var featureTable = _globalVars.MobileServiceClient.GetTable<DataObjects.DistinguishingFeature>();

            Task t = Task.Run(() => featureTable.InsertAsync(this));

            t.Wait();
        }

        private void UpdateTable(Activity activityContext)
        {
            var featureTable = _globalVars.MobileServiceClient.GetTable<DataObjects.DistinguishingFeature>();
            Task t = Task.Run(() => featureTable.UpdateAsync(this));

            t.Wait();
        }

        private void DeleteFromTable(Activity activityContext)
        {
            Task t = null;

            var featureTable = _globalVars.MobileServiceClient.GetTable<DataObjects.DistinguishingFeature>();
            t = Task.Run(() => featureTable.DeleteAsync(this));

            t.Wait();
        }
    }
}
