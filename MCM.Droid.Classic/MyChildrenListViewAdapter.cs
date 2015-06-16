using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace MCM.Droid.Classic
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
            view.FindViewById<TextView>(Resource.Id.Age).Text = item.DisplayAge; 
            view.FindViewById<TextView>(Resource.Id.FirstName).Text = item.FirstName;
            view.FindViewById<TextView>(Resource.Id.PercentComplete).Text = item.DisplayCompletion;
            var imageView = view.FindViewById<ImageView>(Resource.Id.Image);
            imageView.SetImageBitmap(null);

            if (!string.IsNullOrEmpty(item.PictureUri))
            {

                Java.IO.File file = new Java.IO.File(this.context.ApplicationInfo.DataDir, item.PictureUri);
                if (file.Exists())
                {
                    //the size of the image is 100, 100 as code in the axml.
                    Bitmap bm = file.Path.LoadAndResizeBitmap(100, 100);
                    imageView.SetImageBitmap(bm);
                }
            }
            return view;
        }
    }
}