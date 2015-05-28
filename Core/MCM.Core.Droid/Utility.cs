using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MCM.Core
{
    public static class Utility
    {
        public static void SaveContentToInternalStorage(ContextWrapper context, string fileName, string content, FileCreationMode mode)
        {
            using (var output = new StreamWriter(context.OpenFileOutput(fileName, mode)))
            {
                output.Write(content);
            }
        }

        public static async void SaveContentToInternalStorageAsync(ContextWrapper context, string fileName, string content, FileCreationMode mode)
        {
            using (var output = new StreamWriter(context.OpenFileOutput(fileName, mode)))
            {
                await output.WriteAsync(content).ConfigureAwait(false);
            }
        }
    }
}