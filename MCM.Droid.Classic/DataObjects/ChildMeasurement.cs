using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Microsoft.WindowsAzure.MobileServices;

namespace MCM.Droid.Classic.DataObjects
{
    public class ChildMeasurement
    {
        private GlobalVars _globalVars;
        private System.Threading.Timer _timer;
        private int _timerCount = 0;

        public string Id { get; set; }
        public string ChildId { get; set; }
        public int Feet { get; set; }
        public int Inches { get; set; }
        public int Pounds { get; set; }
        public int Ounces { get; set; }
        public DateTime MeasurementDate { get; set; }

        public static  ChildMeasurement GetChildMeasurement(MobileServiceClient mobileServiceClient, string childId)
        {
            System.Diagnostics.Debug.WriteLine("GetChildMeasurement.");

            var cm = GetMeasurement(mobileServiceClient, childId);
            if (cm.Result != null && cm.Result.Count() > 0) return cm.Result[0];
            return new ChildMeasurement() { ChildId = childId };
        }

        private static Task<List<ChildMeasurement>> GetMeasurement(MobileServiceClient mobileServiceClient, string childId)
        {
            Task<List<ChildMeasurement>> measurements = Task.Factory.StartNew(() => mobileServiceClient.GetTable<ChildMeasurement>()
                                                                                            .Where(x => x.ChildId == childId)
                                                                                            .ToListAsync()
                                                                                            .Result);
            return measurements;
        }

        public async Task<ChildMeasurement> Save(Activity activityContext)
        {

            _globalVars = ((GlobalVars)activityContext.Application);
            if (string.IsNullOrWhiteSpace(Id))
            {
                await InsertToTable(activityContext);
            }
            else
            {
                await UpdateTable(activityContext);
            }

            return this;
        }

        private Task InsertToTable(Activity activityContext)
        {
            Task task = null;
            var childMeasurementTable = _globalVars.MobileServiceClient.GetTable<DataObjects.ChildMeasurement>();
            task = Task.Factory.StartNew(() => childMeasurementTable.InsertAsync(this));

            //timer is used to assure that the Id assigned is retrieved. saw that it may take longer than expected
            //to retrieve the returned Id from the mobile service.
            _timerCount = 0;
            _timer = new System.Threading.Timer(TimerDelegate, null, 250, 250);

            return task;
        }

        private Task UpdateTable(Activity activityContext)
        {
            Task task = null;

            var childMeasurementTable = _globalVars.MobileServiceClient.GetTable<DataObjects.ChildMeasurement>();
            task = Task.Factory.StartNew(() => childMeasurementTable.UpdateAsync(this));

            //timer is used to assure that the Id assigned is retrieved. saw that it may take longer than expected
            //to retrieve the returned Id from the mobile service.
            _timerCount = 0;
            _timer = new System.Threading.Timer(TimerDelegate, null, 250, 250);

            return task;
        }

        private void TimerDelegate(object state)
        {
            System.Diagnostics.Debug.WriteLine("Timer Child Measurement update/add fired");
            if (!string.IsNullOrWhiteSpace(Id))
            {
                _timer.Dispose();
            }
            else
            {
                //if more than 3 seconds has elapsed, consider error
                if (_timerCount > 12)
                {
                    //final check to see if there is an Id
                    if (string.IsNullOrWhiteSpace(Id))
                    {
                        System.Diagnostics.Debug.WriteLine("Update/Save of Child Measurement timed out");
                        _timer.Dispose();
                        throw new ApplicationException("Update/Save of Child Measurement timed out.");
                    }
                }
            }

            _timerCount++;
        }
    }
}