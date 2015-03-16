using System;
using System.Threading.Tasks;
using Android.App;

namespace MCM.Droid.Classic.DataObjects
{
    public class Child
    {
        private GlobalVars _globalVars;
        private System.Threading.Timer _timer;
        private int _timerCount = 0;

        public string Id { get; set; }
        public string UserAccount { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string HairColor { get; set; }
        public string EyeColor { get; set; }
        public bool Glasses { get; set; }
        public bool Contacts { get; set; }
        public string SkinTone { get; set; }
        public string RaceEthnicity { get; set; }
        public string DoctorName { get; set; }
        public string DoctorClinicName { get; set; }
        public string DoctorAddress1 { get; set; }
        public string DoctorAddress2 { get; set; }
        public string DoctorCity { get; set; }
        public string DoctorState { get; set; }
        public string DoctorPostalCode { get; set; }
        public string DoctorPhoneNumber { get; set; }
        public string DentistName { get; set; }
        public string DentistClinicName { get; set; }
        public string DentistAddress1 { get; set; }
        public string DentistAddress2 { get; set; }
        public string DentistCity { get; set; }
        public string DentistState { get; set; }
        public string DentistPostalCode { get; set; }
        public string DentistPhoneNumber { get; set; }
        public string MedicalAlertInfo { get; set; }

        public string Picture { get; set; }
        public string PictureUri { get; set; }

        public string DisplayAge
        {
            get
            {
                if (AgeInYears < 2)
                {
                    return string.Format("{0} Months old.", AgeInMonths);
                }
                else
                {
                    return string.Format("{0} Years old.", AgeInYears);
                }
            }
        }

        public int AgeInMonths
        {
            get
            {
                TimeSpan tsAge = DateTime.Now.Subtract(BirthDate);
                var d = new DateTime(tsAge.Ticks);
                return ((d.Year - 1) * 12) + d.Month;
            }
        }

        public int AgeInYears
        {
            get
            {
                TimeSpan tsAge = DateTime.Now.Subtract(BirthDate);

                return new DateTime(tsAge.Ticks).Year - 1;
            }
        }

        public string DisplayCompletion
        {
            get
            {
                return string.Format("{0}%", CompletionValue);
            }
        }
        public int CompletionValue
        {
            get
            {
                decimal numberOfFields = 0M;
                decimal empty = 0M;
                numberOfFields++; if (string.IsNullOrEmpty(UserAccount)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(FirstName)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(MiddleName)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(LastName)) { empty++; }
                //numberOfFields++; //public DateTime BirthDate { get; set; }
                numberOfFields++; if (string.IsNullOrEmpty(HairColor)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(EyeColor)) { empty++; }
                //numberOfFields++; //Glasses { get; set; }
                //numberOfFields++; //Contacts { get; set; }
                numberOfFields++; if (string.IsNullOrEmpty(SkinTone)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(RaceEthnicity)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(DoctorName)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(DoctorClinicName)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(DoctorAddress1)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(DoctorAddress2)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(DoctorCity)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(DoctorState)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(DoctorPostalCode)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(DoctorPhoneNumber)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(DentistName)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(DentistClinicName)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(DentistAddress1)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(DentistAddress2)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(DentistCity)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(DentistState)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(DentistPostalCode)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(DentistPhoneNumber)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(MedicalAlertInfo)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(Picture)) { empty++; }
                numberOfFields++; if (string.IsNullOrEmpty(PictureUri)) { empty++; }

                return Convert.ToInt32(Math.Floor(((decimal)((numberOfFields - empty) / numberOfFields))*100M));
            }
        }

        public async Task<Child> Save( Activity activityContext)
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

        public async Task Delete(Activity activityContext)
        {
            _globalVars = ((GlobalVars)activityContext.Application);
            await DeleteFromTable(activityContext);
        }

        private Task InsertToTable(Activity activityContext)
        {
            Task task = null;
            var childTable = _globalVars.MobileServiceClient.GetTable<DataObjects.Child>();
            task = Task.Factory.StartNew(() => childTable.InsertAsync(this));

            //timer is used to assure that the Id assigned is retrieved. saw that it may take longer than expected
            //to retrieve the returned Id from the mobile service.
            _timerCount = 0;
            _timer = new System.Threading.Timer(TimerDelegate, null, 250, 250);

            return task;
        }

        private Task UpdateTable(Activity activityContext)
        {
            Task task = null;

            var childTable = _globalVars.MobileServiceClient.GetTable<DataObjects.Child>();
            task = Task.Factory.StartNew(() => childTable.UpdateAsync(this));

            //timer is used to assure that the Id assigned is retrieved. saw that it may take longer than expected
            //to retrieve the returned Id from the mobile service.
            _timerCount = 0;
            _timer = new System.Threading.Timer(TimerDelegate, null, 250, 250);

            return task;
        }

        private Task DeleteFromTable(Activity activityContext)
        {
            Task task = null;

            var childTable = _globalVars.MobileServiceClient.GetTable<DataObjects.Child>();
            task = Task.Factory.StartNew(() => childTable.DeleteAsync(this));

            return task;
        }

        private void TimerDelegate(object state)
        {
            System.Diagnostics.Debug.WriteLine("Timer Child update/add fired");
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
                        System.Diagnostics.Debug.WriteLine("Update/Save of Child timed out");
                        _timer.Dispose();
                        throw new ApplicationException("Update/Save of Child timed out.");
                    }
                }
            }

            _timerCount++;
        }
    }
}
