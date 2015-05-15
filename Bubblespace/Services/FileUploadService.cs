using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Bubblespace.Services
{
    public class FileUploadService
    {
        /* <summary>Encapsulates fileupload</summary>
        * <param>HttpPostedFileBase</param>
        * <returns>Returns name of image/file</returns>
        * <author>Sveinbjörn / Janus</author>
        */
        public static string UploadImage(HttpPostedFileBase contentImage, string folder)
        {
            // Inside this if statement we handle the image if one is uploaded
            if(contentImage != null)
            {
                // Retreiving the file name
                string pic = System.IO.Path.GetFileName(contentImage.FileName);

                // Generate a random filename
                var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                var result = new string(
                    Enumerable.Repeat(chars, 64)
                              .Select(s => s[random.Next(s.Length)])
                              .ToArray());

                // We extract the file ending and combine it with the generated filename
                Regex regex = new Regex(@"\.\w{1,3}");
                result = result + regex.Match(pic).Value.ToLower();

                // Debug Print
                System.Diagnostics.Debug.WriteLine("The String: " + result);

                // Creating an absolute path
                string path = System.IO.Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Images/" + folder + "/" + result));

                // File is uploaded
                contentImage.SaveAs(path);

                // Return the image name
                return result;
            }
            else
            {
                return string.Empty;
            }
            
        }
    }
}