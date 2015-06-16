using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Graphics;
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

        public static void SaveFileToInternalStorage(ContextWrapper context, Java.IO.File srcFile, string dstFileName)
        {
            ////var fileName = GetFileName(directory);
            //using (var os = new FileStream(fileName, FileMode.CreateNew))
            //{
            //    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 95, os);
            //}

            //Bitmap bm = BitmapFactory.DecodeByteArray(e.Result, 0, e.Result.Length);

            //ContextWrapper cw = new ContextWrapper(this.ApplicationContext);
            //File directory = cw.GetDir("imgDir", FileCreationMode.Private);
            //File myPath = new File(directory, "test.png");

            //FileOutputStream fos = null;
            //try
            //{
            //    fos = new FileOutputStream(myPath);
            //    bm.Compress(Bitmap.CompressFormat.Png, 100, fos);
            //    fos.Close();
            //}
            //catch (Exception ex)
            //{
            //    System.Console.Write(ex.Message);
            //}

            Java.IO.File dstFile = new Java.IO.File(context.ApplicationInfo.DataDir, dstFileName);
            if (!srcFile.CanonicalPath.Equals(dstFile.CanonicalPath))
            {
                if (srcFile.Exists())
                {
                    Java.IO.InputStream fis = new Java.IO.FileInputStream(srcFile);
                    Java.IO.OutputStream fos = new Java.IO.FileOutputStream(dstFile);

                    byte[] buff = new byte[1024];
                    int len;
                    while ((len = fis.Read(buff)) > 0)
                    {
                        fos.Write(buff, 0, len);
                    }
                    fis.Close();
                    fos.Close();
                }
            }


            //Java.IO.File file1 = context.GetFileStreamPath(dstFileName);
            //if (file1.Exists())
            //{
            //    Java.Text.SimpleDateFormat sdf = new Java.Text.SimpleDateFormat("MM/dd/yyyy HH:mm:ss");
            //    DateTime fileDt = DateTime.Parse(sdf.Format(file1.LastModified()));
            //}

            //Bitmap bm = BitmapFactory.DecodeByteArray(e.Result, 0, e.Result.Length);

            ////ContextWrapper cw = new ContextWrapper(this.ApplicationContext);
            //Java.IO.File directory = context.GetDir("imgDir", FileCreationMode.Private);
            //string myPath = System.IO.Path.Combine(directory.AbsolutePath, "test.png");

            //try
            //{
            //    using (var os = new System.IO.FileStream(Application.Context.Resources.Assets.Handle, FileAccess.Read))
            //    {
            //        os.Read()
            //        bm.Compress(Bitmap.CompressFormat.Png, 100, os);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    System.Console.Write(ex.Message);
            //}
        }
    }
}