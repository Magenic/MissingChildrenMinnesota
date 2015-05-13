using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MCM.Droid.Classic
{
    public class IDCheckListListViewAdapter : BaseAdapter<DataObjects.IDCheckListItem>
    {
        private List<DataObjects.IDCheckListItem> _items;
        private Activity _context;

        //these are needed to keep track of the state of the checkboxes
        //android re-uses the convertView during scrolling.
        //find that you check a box and when you scroll it checks other boxes that
        //were not visible and becomes checked.
        private Dictionary<int, bool> _checkBoxStates;

        public IDCheckListListViewAdapter(Activity context, List<DataObjects.IDCheckListItem> items)
            : base()
        {
            _context = context;
            _items = items;
            if (_items != null)
            {
                _checkBoxStates = new Dictionary<int, bool>();
                foreach(DataObjects.IDCheckListItem listItem in _items)
                {
                    _checkBoxStates.Add(listItem.IDCheckListItemId, false);
                }
            }
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override DataObjects.IDCheckListItem this[int position]
        {
            get { return _items[position]; }
        }
        public override int Count
        {
            get { return _items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _items[position];
            View view = convertView;

            ViewHolder holder;
            if (view == null)
            {
                //checkBoxItemId[position] = item.IDCheckListItemId;
                // no view to re-use, create new
                view = _context.LayoutInflater.Inflate(Resource.Layout.IDCheckListItem, null);
                holder = new ViewHolder();
                holder.chkBox = view.FindViewById<CheckBox>(Resource.Id.IDCheckListItemCheckBox);
                view.Tag = holder;
            }
            else
            {
                holder = (ViewHolder)view.Tag;
            }

            holder.chkBox.Checked = _checkBoxStates[item.IDCheckListItemId];
            holder.chkBox.Tag = item.IDCheckListItemId;
            holder.chkBox.Text = item.LongDescription;
            holder.chkBox.Click += (sender, e) => {
                var checkbox = (CheckBox)sender;
                //only save the state of the actual checkbox that was checked.
                //the item id is saved in the tag property of the checkbox
                _checkBoxStates[int.Parse(checkbox.Tag.ToString())] = checkbox.Checked;
            };

            return view;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public CheckBox chkBox { get; set; }
        }

        //private class CheckedChangeListener : Java.Lang.Object, CompoundButton.IOnCheckedChangeListener
        //{
        //    private Activity activity;

        //    public CheckedChangeListener(Activity activity)
        //    {
        //        this.activity = activity;
        //    }

        //    public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        //    {
        //        if (isChecked)
        //        {
        //            string name = (string)buttonView.Tag;
        //            string text = string.Format("{0} Checked.", name);
        //            Toast.MakeText(this.activity, text, ToastLength.Short).Show();
        //        }
        //    }
        //}
    }
}