using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            //p.fibonacci();
            //p.CreateShortcut("MyLink dropbox", "https://www.dropbox.com/help/desktop-web/save-website-url");
            p.UploadLinkAsset("/vctqa/value creation toolbox/vct/organization & human resources/general/hello links1/", "https://www.needledust.com/jutti/Events.jsp");
        }

        public void palidrome()
        {
            Console.WriteLine("Input a string");
            string str = Console.ReadLine();
            var arr = str.ToArray();
            int lastIndex;
            if ((arr.Length - 1) % 2 == 0)
            {
                lastIndex = (arr.Length - 1) / 2;
            }
            else
            {
                lastIndex = (arr.Length) / 2;
            }

            bool flag = false;
            int i = 0, j = 0;
            for (i = 0, j = arr.Length - 1; i < lastIndex; i++, j--)
            {
                //for(int j=arr.Length-1;j>lastIndex;j--)
                //{
                if (arr[i] == arr[j])
                    flag = true;
                else
                {
                    flag = false;
                    break;
                }
                //}
            }

            if (flag)
            {
                Console.WriteLine("palindrome");
            }
            else
            {
                Console.WriteLine("not palindrome");
            }

        }
        public void fibonacci()
        {
            string[] seperators = { ",", " ", ";", "." };
            int count;
            
            Console.WriteLine("Input a number limit to generate fibonacci series");
            if(Int32.TryParse(Console.ReadLine(), out count))
            {
                int i = 0; int j = 1; int k = 0;
                Console.WriteLine(i);
                Console.WriteLine(j);
                for(int a=0;a<=count;a++)
                {
                    k = i + j;
                    Console.WriteLine(k);
                    i = j;
                    j = k;
                }
                Console.WriteLine();
            }
            //string[] splitArray = Console.ReadLine().Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            //foreach (string num in splitArray)
            //{

            //    if (Int32.TryParse(num, out num2) == true)
            //    {
            //        if (num2 < 10)
            //        {
            //            Console.WriteLine(num + " is smaller than 10");
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine(num + " is not a f***ing number...");
            //    }
            //}
        }

        public void CreateShortcut(string name, string url)

        {

            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            using (StreamWriter writer = new StreamWriter(deskDir + "\\" + name + ".url"))

            {

                writer.WriteLine("[InternetShortcut]");

                writer.WriteLine("URL=" + url);

                writer.Flush();

            }

        }

        /// <summary>
        /// upload link internet shortcut file (.url extension file)
        /// </summary>
        /// <param name="path">Path is the joined path in dropbox along with the filename with extension to be created, like : 
        /// /VCT/Cost Improvement/Fixed Production Costs/New Asset Group name/filename.url</param>
        /// <param name="URL">external web url whose shortcut needs to be created and uploaded to dropbox</param>
        /// <returns></returns>
        public bool UploadLinkAsset(string path, string URL)
        {
            try
            {
                //Logger.Info("DropBoxUtil | UploadLinkAsset | function starts");
                string[] lines = { "[InternetShortcut]", "URL=" + URL,
            "IconFile=", "IconIndex=0" };
                StringBuilder file = new StringBuilder("[InternetShortcut]");
                file.Append("URL=" + URL);
                //file.Append("IconFile=");

                var uri = new Uri("https://content.dropboxapi.com/2/files/upload");
                //Logger.Debug("DropBoxUtil | UploadLinkAsset | uri: " + uri);
                StringBuilder requestUri = new StringBuilder(uri.ToString());
                var request = (HttpWebRequest)WebRequest.Create(new Uri(requestUri.ToString()));
                request.Method = WebRequestMethods.Http.Post;
                request.Headers.Add("Authorization: Bearer FSGTD3cTE2AAAAAAAAAEU2uh2rQh8nh7ATJZ86n-6U7O3GI3QFCaaDybHMGtkT7D");
                request.UserAgent = "api-explorer-client";
                request.ContentType = "application/octet-stream";
                string postString = "{\"path\": \"" + path + "\"}";
                request.Headers.Add("Dropbox-API-Arg: " + postString);

                // path example: /VCT/Cost Improvement/Fixed Production Costs/New Asset Group name/filename.ext

                //Logger.Info("DropBoxUtil | UploadLinkAsset | path: " + path);

                //using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(file.ToString())))
                //{
                //    StreamWriter requestWriter = new StreamWriter(request.GetRequestStream());
                //    requestWriter.Write(mem);
                //    requestWriter.Close();

                //    var response = request.GetResponse();
                //    var reader = new StreamReader(response.GetResponseStream());
                 
                //   /// Logger.Info("DropBoxUtil | UploadLinkAsset | response: " + reader.ReadToEnd());
                //}

                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.WriteLine("[InternetShortcut]");

                    writer.WriteLine("URL=" + URL);
                    //writer.Close();
                    writer.Flush();
                }

                var response = request.GetResponse();
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {
                    response.Close();
                    return true;
                }
                else
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    string responseFromServer = reader.ReadToEnd();
                    // Display the content.
                    Console.WriteLine(responseFromServer);
                    reader.Close();
                    response.Close();
                    return false;
                }
            }
            catch (WebException webExcp)
            {
                // If you reach this point, an exception has been caught.  
                Console.WriteLine("A WebException has been caught.");
                // Write out the WebException message.  
                Console.WriteLine(webExcp.ToString());
                // Get the WebException status code.  
                WebExceptionStatus status = webExcp.Status;
                // If status is WebExceptionStatus.ProtocolError,   
                //   there has been a protocol error and a WebResponse   
                //   should exist. Display the protocol error.  
                if (status == WebExceptionStatus.ProtocolError)
                {
                    Console.Write("The server returned protocol error ");
                    // Get HttpWebResponse so that you can check the HTTP status code.  
                    HttpWebResponse httpResponse = (HttpWebResponse)webExcp.Response;
                    Console.WriteLine((int)httpResponse.StatusCode + " - "
                       + httpResponse.StatusCode);
                }
                return false;
            }
            catch (Exception e)
            {
                // Code to catch other exceptions goes here.  
                return false;
            }
        }

    }
}
