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

namespace MCM
{

    public class MyChildrenListViewAdapter : BaseAdapter<DataObjects.Child>
    {
        List<DataObjects.Child> items;
        Activity context;
        public MyChildrenListViewAdapter(Activity context, List<DataObjects.Child> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override DataObjects.Child this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.ChildListItem, null);
            view.FindViewById<TextView>(Resource.Id.Age).Text = item.AgeInYears.ToString(); // item.BirthDate.ToString();
            view.FindViewById<TextView>(Resource.Id.FirstName).Text = item.FirstName;
            //view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(item.ImageResourceId);
            return view;
        }
    }
}