
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Newtonsoft.Json;

namespace MCM.Droid.Classic
{
    [Activity(Label = "@string/childbasics_layout_label")]			
	public class ChildBasicsActivity : Activity
	{
        const int DATE_DIALOG_ID = 0;

        private GlobalVars _globalVars;

        private EditText _firstNameText;
        private EditText _middleNameText;
        private EditText _lastNameText;
        private TextView _dateDisplayTextView;
        private TextView _pageTitleTextView;
        private Button _pickDate;
        private DateTime _date;

        private ProgressDialog _progressDialog;
        private System.Threading.Timer _timer;
        private int _timerCount = 0;
        private DataObjects.Child _child;

        private string _orgFirstName;
        private string _orgMiddleName;
        private string _orgLastName;
        private DateTime _orgBirthDate;

        private bool _childAddedOrUpdated = false;

        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);

            _globalVars = ((GlobalVars)this.Application);
            _child = JsonConvert.DeserializeObject<DataObjects.Child>(Intent.GetStringExtra("Child"));

            RequestWindowFeature(WindowFeatures.ActionBar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            
            SetContentView(Resource.Layout.ChildBasics);

            _firstNameText = FindViewById<EditText>(Resource.Id.FirstNameText);
            _middleNameText = FindViewById<EditText>(Resource.Id.MiddleNameText);
            _lastNameText = FindViewById<EditText>(Resource.Id.LastNameText);
            _dateDisplayTextView = FindViewById<TextView>(Resource.Id.DateDisplayTextView);
            _pageTitleTextView = FindViewById<TextView>(Resource.Id.textView1);

            // capture our View elements
            _pickDate = FindViewById<Button>(Resource.Id.PickDateButton);

            // add a click event handler to the button
            _pickDate.Click += delegate { ShowDialog(DATE_DIALOG_ID); };

            // display the current date (this method is below)
            InitializeDisplay();
            UpdateBirthDateDisplay();
        }

       
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_childbasics, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_save_info:
                    AddUpdateChild();
                    return true;

                case Resource.Id.menu_cancel_info:
                    if (!_orgBirthDate.Equals(_date) ||
                        _orgFirstName.Equals(_firstNameText.Text.Trim(), StringComparison.OrdinalIgnoreCase) ||
                        _orgMiddleName.Equals(_middleNameText.Text.Trim(), StringComparison.OrdinalIgnoreCase) ||
                        _orgLastName.Equals(_lastNameText.Text.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        ///
                        ///TODO: dialog to ask to discard changes
                        ///
                        
                        //replace fields with original values
                        InitializeDisplay();
                        UpdateBirthDateDisplay();
                    }
                    return true;

                case Resource.Id.menu_delete_info:
                    DeleteChild();
                    return true;

                default:
                    Finish();
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void Finish()
        {
            if (_childAddedOrUpdated)
            {
                Intent returnIntent = new Intent();
                returnIntent.PutExtra("Child", JsonConvert.SerializeObject(_child));
                this.SetResult(Result.Ok, returnIntent);
            }
            base.Finish();
        }

        protected override Dialog OnCreateDialog(int id)
        {
            switch (id)
            {
                case DATE_DIALOG_ID:
                    return new DatePickerDialog(this, OnDateSet, _date.Year, _date.Month - 1, _date.Day);
            }
            return null;
        }
        
        // the event received when the user "sets" the date in the dialog
        private void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            this._date = e.Date;
            UpdateBirthDateDisplay();
        }        

        private async void AddUpdateChild()
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_firstNameText.Text))
            {
                sb.Append("First Name is required.");
            }
            if (string.IsNullOrWhiteSpace(_lastNameText.Text))
            {
                if (!string.IsNullOrWhiteSpace(sb.ToString()))
                {
                    sb.Append("\n");
                }
                sb.Append("Last Name is required.");
            }
            if (!string.IsNullOrWhiteSpace(sb.ToString()))
            {
                CreateAndShowDialog(sb.ToString(), "Please correct the following: \n");
            }
            else
            {
                try
                {
                    _child.FirstName = _firstNameText.Text.Trim();
                    _child.MiddleName = _middleNameText.Text.Trim();
                    _child.LastName = _lastNameText.Text.Trim();
                    _child.BirthDate = _date;

                    if (string.IsNullOrWhiteSpace(_child.Id))
                    {
                        await InsertToTable();
                    }
                    else
                    {
                        await UpdateTable();
                    }
                }
                catch
                {
                    CreateAndShowDialog("Unable to add Child.", "Add Child");
                }

                //CreateAndShowDialog(_children.Count.ToString(), " Children Found");
            }
        }

        private Task InsertToTable()
        {
            Task task = null;
            _progressDialog = new ProgressDialog(this);
            _progressDialog.SetTitle("Adding Child Information");
            _progressDialog.SetMessage("Please Wait...");
            _progressDialog.Show();            
            try
            {
                var childTable = _globalVars.MobileServiceClient.GetTable<DataObjects.Child>();
                task = Task.Factory.StartNew(() => childTable.InsertAsync(_child));

                //timer is used to assure that the Id assigned is retrieved. saw that it may take longer than expected
                //to retrieve the returned Id from the mobile service.
                _timerCount = 0;
                _timer = new System.Threading.Timer(TimerDelegate, null, 250, 250);
            }
            catch
            {
                _progressDialog.Dismiss();
                CreateAndShowDialog("Unable to add Child.", "Add Child");
            }

            return task;
        }

        private Task UpdateTable()
        {
            Task task = null;
            _progressDialog = new ProgressDialog(this);
            _progressDialog.SetTitle("Updating Child Information");
            _progressDialog.SetMessage("Please Wait...");
            _progressDialog.Show();
            try
            {
                var childTable = _globalVars.MobileServiceClient.GetTable<DataObjects.Child>();
                task = Task.Factory.StartNew(() => childTable.UpdateAsync(_child));

                //timer is used to assure that the Id assigned is retrieved. saw that it may take longer than expected
                //to retrieve the returned Id from the mobile service.
                _timerCount = 0;
                _timer = new System.Threading.Timer(TimerDelegate, null, 250, 250);
            }
            catch
            {
                _progressDialog.Dismiss();
                CreateAndShowDialog("Unable to update Child.", "Add Child");
            }

            return task;
        }

        private async void DeleteChild()
        {
            _progressDialog = new ProgressDialog(this);
            _progressDialog.SetTitle("Removing Child Information");
            _progressDialog.SetMessage("Please Wait...");
            _progressDialog.Show();
            try
            {

                await DeleteFromTable();

                _progressDialog.Dismiss();
                CreateAndShowDialog(string.Format("Child '{0} {1}' removed.", _child.FirstName, _child.LastName), "Remove Child");
                Finish();
            }
            catch
            {
                _progressDialog.Dismiss();
                CreateAndShowDialog("Unable to remove Child.", "Remove Child");
            }
        }

        private Task DeleteFromTable()
        {
            Task task = null;

            try
            {
                var childTable = _globalVars.MobileServiceClient.GetTable<DataObjects.Child>();
                task = Task.Factory.StartNew(() => childTable.DeleteAsync(_child));

                ////timer is used to assure that the Id assigned is retrieved. saw that it may take longer than expected
                ////to retrieve the returned Id from the mobile service.
                //_timerCount = 0;
                //_timer = new System.Threading.Timer(TimerDelegate, null, 250, 250);
            }
            catch
            {
                CreateAndShowDialog("Unable to remove Child.", "Remove Child");
            }

            return task;
        }

        private void TimerDelegate(object state)
        {
            if (!string.IsNullOrWhiteSpace(_child.Id))
            {
                _childAddedOrUpdated = true;
                SaveOriginalValues();
                _timer.Dispose();
                _progressDialog.Dismiss();
            }
            else
            {
                //if more than 3 seconds has elapsed, consider error
                if (_timerCount > 12)
                {
                    //final check to see if there is an Id
                    if (string.IsNullOrWhiteSpace(_child.Id))
                    {
                        _childAddedOrUpdated = true;
                        SaveOriginalValues();
                        _timer.Dispose();
                        _progressDialog.Dismiss();
                    }
                }
            }

            _timerCount++;
        }

        private void InitializeDisplay()
        {
            if (string.IsNullOrWhiteSpace(_child.Id))
            {
                // get the current date
                _date = DateTime.Today;
            }
            else
            {
                _pageTitleTextView.Text = _pageTitleTextView.Text.Replace("Child ", _child.FirstName + "'s ");
                _firstNameText.Text = _child.FirstName;
                _middleNameText.Text = _child.MiddleName;
                _lastNameText.Text = _child.LastName;
                _date = _child.BirthDate;
            }

            SaveOriginalValues();
        }

        private void SaveOriginalValues()
        {
            _orgBirthDate = _date;
            _orgFirstName = _child.FirstName;
            _orgMiddleName = _child.MiddleName;
            _orgLastName = _child.LastName;
        }

        // updates the date in the TextView
        private void UpdateBirthDateDisplay()
        {
            _dateDisplayTextView.Text = _date.ToString("d");
        }

        private void CreateAndShowDialog(string message, string title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }
    }
}

