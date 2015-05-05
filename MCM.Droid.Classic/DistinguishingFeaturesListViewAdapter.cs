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
    public class DistinguishingFeaturesListViewAdapter : BaseAdapter<DataObjects.DistinguishingFeature>
    {
        List<DataObjects.DistinguishingFeature> items;
        Activity context;
        public DistinguishingFeaturesListViewAdapter(Activity context, List<DataObjects.DistinguishingFeature> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override DataObjects.DistinguishingFeature this[int position]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.DistinguishingFeatureListItem, null);
            view.FindViewById<TextView>(Resource.Id.BodyChartNbrText).Text = item.BodyChartNbr <= 0 ? string.Empty : item.BodyChartNbr.ToString();
            view.FindViewById<TextView>(Resource.Id.DistinguishingFeatureText).Text = item.Feature;

            return view;
        }
    }
}