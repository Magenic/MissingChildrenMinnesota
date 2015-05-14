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

        //blog on keeping track of state of the checkboxes: http://androidcocktail.blogspot.com/2012/04/adding-checkboxes-to-custom-listview-in.html
        //code in blog was used as a starting guideline
        //these are needed to keep track of the state of the checkboxes
        //android re-uses the convertView during scrolling.
        //find that you check a box and when you scroll it checks other boxes that
        //were not visible and becomes checked.
        //using the blog as a guideline to fix problem with the scrolling.

        private Dictionary<int, bool> _checkBoxStates;
        public Dictionary<int, bool> CheckBoxStates
        {
            get { return _checkBoxStates; }
        }

        public IDCheckListListViewAdapter(Activity context, List<DataObjects.IDCheckListItem> idCheckItems, List<DataObjects.ChildCheckListItem> childCheckedItems)
            : base()
        {
            _context = context;
            _items = idCheckItems;
            if (_items != null)
            {
                _checkBoxStates = new Dictionary<int, bool>();
                foreach(DataObjects.IDCheckListItem listItem in _items)
                {
                    if (childCheckedItems != null && childCheckedItems.Count > 0)
                    {
                        var childlistItem = childCheckedItems.FirstOrDefault<DataObjects.ChildCheckListItem>(_ => _.IDCheckListItemId == listItem.IDCheckListItemId);
                        if (childlistItem != null)
                        {
                            _checkBoxStates.Add(listItem.IDCheckListItemId, true);
                        }
                        else
                        {
                            _checkBoxStates.Add(listItem.IDCheckListItemId, false);
                        }
                    }
                    else 
                    {
                        _checkBoxStates.Add(listItem.IDCheckListItemId, false);
                    }
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

            if (_checkBoxStates.ContainsKey(item.IDCheckListItemId))
            {
                holder.chkBox.Checked = _checkBoxStates[item.IDCheckListItemId];
            }
            else
            {
                holder.chkBox.Checked = false;
            }
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