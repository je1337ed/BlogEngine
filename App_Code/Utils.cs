using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Script.Serialization;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Text;
using System.Security.Cryptography;
using System.Drawing;
using System.IO;

/// <summary>
/// Summary description for Utils
/// </summary>
public static class Utils
{
    public static class AESEncryption
    {
        private static string _defaultPassword = "p@$$w0rd";
        private static string _defaultSalt = "157m&5445@";
        private static string _defaultHashAlgorithm = "SHA1";
        private static int _defaultPasswordIterations = 3;
        private static string _defaultInitialVector = "asdfqwerzxcvpoiu";
        private static int _defaultKeySize = 128;

        public static string Encrypt(string PlainText)
        {
            //string _defaultPassword = "p@$$w0rd";
            //string _defaultSalt = "";
            //string _defaultHashAlgorithm = "SHA1";
            //int _defaultPasswordIterations = 3;
            //string _defaultInitialVector = "asdfqwerzxcvpoiu";
            //int _defaultKeySize = 128;
            return Encrypt(PlainText, _defaultPassword, _defaultSalt, _defaultHashAlgorithm, _defaultPasswordIterations, _defaultInitialVector, _defaultKeySize);
        }

        public static string Encrypt(string PlainText, string Password, string Salt, string HashAlgorithm, int PasswordIterations, string InitialVector, int KeySize)
        {
            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] PlainTextBytes = Encoding.UTF8.GetBytes(PlainText);
            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
            RijndaelManaged SymmetricKey = new RijndaelManaged();
            SymmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(KeyBytes, InitialVectorBytes);
            MemoryStream MemStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] CipherTextBytes = MemStream.ToArray();
            MemStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(CipherTextBytes);
        }

        public static string Decrypt(string PlainText)
        {
            //string _defaultPassword = "p@$$w0rd";
            //string _defaultSalt = "";
            //string _defaultHashAlgorithm = "SHA1";
            //int _defaultPasswordIterations = 3;
            //string _defaultInitialVector = "asdfqwerzxcvpoiu";
            //int _defaultKeySize = 128;
            return Decrypt(PlainText, _defaultPassword, _defaultSalt, _defaultHashAlgorithm, _defaultPasswordIterations, _defaultInitialVector, _defaultKeySize);
        }

        public static string Decrypt(string CipherText, string Password, string Salt, string HashAlgorithm, int PasswordIterations, string InitialVector, int KeySize)
        {
            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] CipherTextBytes = Convert.FromBase64String(CipherText);
            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
            RijndaelManaged SymmetricKey = new RijndaelManaged();
            SymmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor(KeyBytes, InitialVectorBytes);
            MemoryStream MemStream = new MemoryStream(CipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read);
            byte[] PlainTextBytes = new byte[CipherTextBytes.Length];
            int ByteCount = cryptoStream.Read(PlainTextBytes, 0, PlainTextBytes.Length);
            MemStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(PlainTextBytes, 0, ByteCount);
        }
    }

    public static void GZResponse(HttpContext context)
    {
        if (-1 < Convert.ToString(context.Request.ServerVariables["HTTP_ACCEPT_ENCODING"]).IndexOf("gzip"))
        {
            context.Response.AddHeader("Content-Encoding", "gzip");
            context.Response.Filter = new System.IO.Compression.GZipStream(
                context.Response.Filter,
                System.IO.Compression.CompressionMode.Compress
            );
        }
    }

    public class JavaScriptDataTableConverter : JavaScriptConverter
    {

        public override IEnumerable<Type> SupportedTypes 
        {
            //Define the datatable as a supported type.
            get { return new ReadOnlyCollection<Type>(new List<Type>(new Type[] { typeof(DataTable) })); }
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            //notes on 02/04/09
            //example of old format just for reference:
            //       "rows" : 
            //[{"TABLE_CATALOG":"AtlasDev01"}]
            //}
            //example of new json format (this will reduce the footprint of table listings significantly):
            //      {
            //        "cols": ["first", "last"],
            //        "rows": [["Homer", "Simpson"],
            //                 ["Hank", "Hill"],
            //                 ["Peter", "Griffin"]]
            //      }

            DataTable dt = obj as DataTable;
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (dt != null & dt.Rows.Count > 0)
            {
                // List for row values
                List<object> rowValues = new List<object>();
                List<object> colNameValues = new List<object>();
                List<string> columnNames = new List<string>();
                for (int colCount = dt.Columns.Count - 1; colCount >= 0; colCount--)
                {
                    columnNames.Add(dt.Columns[colCount].ColumnName);
                }
                colNameValues.Add(columnNames);
                result["columns"] = colNameValues;

                for (int drCount = dt.Rows.Count - 1; drCount >= 0; drCount--)
                {
                    DataRow dr = dt.Rows[drCount];
                    List<string> colValues = new List<string>();
                    for (int colCount = dt.Columns.Count - 1; colCount >= 0; colCount--)
                    {
                        colValues.Add(dr[colCount].ToString().Trim());
                    }

                    //Add values to row
                    rowValues.Add(colValues);
                    //Add rows to serialized object
                    result["rows"] = rowValues;
                }
                return result;
            }
            return new Dictionary<string, object>();
        }


        //example I went off of for serializing an object in JSON
        //this example serializes the name-value data from a listbox

        //Dim listType As ListItemCollection = TryCast(obj, ListItemCollection)

        //If listType IsNot Nothing Then
        //    ' Create the representation.
        //    Dim result As New Dictionary(Of String, Object)()
        //    Dim itemsList As New ArrayList()
        //    For Each item As ListItem In listType
        //        'Add each entry to the dictionary.
        //        Dim listDict As New Dictionary(Of String, Object)()
        //        listDict.Add("Value", item.Value)
        //        listDict.Add("Text", item.Text)
        //        itemsList.Add(listDict)
        //    Next
        //    result("List") = itemsList

        //    Return result
        //End If

        //deserializing the datatable is verboten right now
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {

            //If dictionary Is Nothing Then
            //    Throw New ArgumentNullException("dictionary")
            //End If

            //If type.ToString = GetType(ListItemCollection).ToString Then
            //    ' Create the instance to deserialize into.
            //    Dim list As New ListItemCollection()

            //    ' Deserialize the ListItemCollection's items.
            //    Dim itemsList As ArrayList = DirectCast(dictionary("List"), ArrayList)
            //    For i As Integer = 0 To itemsList.Count - 1
            //        list.Add(serializer.ConvertToType(Of ListItem)(itemsList(i)))
            //    Next

            //    Return list
            //End If
            return null;
        }
    }

    public static string GetSafeTitleString(string Title)
    {
        Title = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(RemoveNonAlphaChars(Title)).Replace(" ", "");
        return Title;
    }

    public static string RemoveNonAlphaChars(string rawstring)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in rawstring.ToCharArray())
        {
            if (char.IsLetter(c) || char.IsSeparator(c) || char.IsNumber(c))
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    public static string SerializeDt(DataTable dt)
    {
        //old format
        //{       "rows" : 
        //[{"TABLE_CATALOG":"AtlasDev01"}]
        //}

        //new format to be parsed by dataview
        // var images2 = [
        //{ Name: "Oryx", Description: "Along the safari ride at Wild Animal Park." },
        //{ Name: "Spanish Architecture", Description: "In Balboa Park, in downtown San Diego." }
        //];
        StringBuilder sb = new StringBuilder();
        if (dt != null & dt.Rows.Count > 0)
        {
            // List for row values
            sb.Append("[");
            foreach (DataRow dr in dt.Rows)
            {
                //Dictionary for col name / col value
                sb.Append("{");
                foreach (DataColumn dc in dt.Columns)
                {
                    if ((dt.Columns.IndexOf(dc) == dt.Columns.Count - 1))
                    {
                        sb.Append(dc.ColumnName + ":\"" + dr[dc].ToString().Trim() + "\"");
                    }
                    else
                    {
                        sb.Append(dc.ColumnName + ":\"" + dr[dc].ToString().Trim() + "\",");
                    }
                }
                if ((dt.Rows.IndexOf(dr) != dt.Rows.Count - 1))
                {
                    sb.Append("},");
                    sb.Append(System.Environment.NewLine);
                }
                else
                {
                    sb.Append("}");
                }
            }
            sb.Append("]");
            return sb.ToString();
        }
        return "[{error: \"error retrieving records\"}]";
    }

    public static string getSupportedMimeType(string FileExtension)
    {
        string result = string.Empty;
        try
        {
            Dictionary<string, string> SupportedMimeTypes = new Dictionary<string, string>();
            SupportedMimeTypes.Add("bmp", "image/bmp");
            SupportedMimeTypes.Add("dib", "image/dib");
            SupportedMimeTypes.Add("jpg", "image/jpeg");
            SupportedMimeTypes.Add("jpeg", "image/jpeg");
            SupportedMimeTypes.Add("gif", "image/gif");
            SupportedMimeTypes.Add("ani", "image/ani");
            SupportedMimeTypes.Add("cur", "image/x-win-bitmap");
            SupportedMimeTypes.Add("ico", "image/x-icon");
            SupportedMimeTypes.Add("doc", "application/msword");
            SupportedMimeTypes.Add("pdf", "application/pdf");
            SupportedMimeTypes.Add("txt", "text/plain");
            SupportedMimeTypes.Add("xls", "application/vnd.ms-excel");
            SupportedMimeTypes.Add("csv", "text/csv");
            SupportedMimeTypes.TryGetValue(FileExtension, out result);
            return result;
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return string.Empty;
        }
    }

    public static void SendEmail(string From, string Recipients, string Subject, string Body)
    {
        System.Net.Mail.SmtpClient objMail = new System.Net.Mail.SmtpClient("");
        objMail.Send(From, Recipients, Subject, Body);
    }

    public static string getFileExtension(string fileName)
    {
        string extension = "";
        char[] arr = fileName.ToCharArray();
        int index = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == '.')
            {
                index = i; //get the last dot in the string
            }
        }
        for (int x = index + 1; x < arr.Length; x++)//build the new string
        {
            extension = extension + arr[x];
        }
        return extension.Trim();
    }

    public static string GetRandomString(int maxSize)
    {
        char[] chars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        byte[] data = new byte[1];
        RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
        crypto.GetNonZeroBytes(data);
        data = new byte[maxSize];
        crypto.GetNonZeroBytes(data);
        StringBuilder result = new StringBuilder(maxSize);
        foreach (byte b in data)
        {
            result.Append(chars[b % (chars.Length - 1)]);
        }
        return result.ToString();
    }

    public static XmlDocument GetValidXmlFromString(string xml)
    {
        XmlDocument XmlDoc = new XmlDocument();
        try
        {
            XmlDoc.LoadXml(xml);
            return XmlDoc;
        }
        catch (Exception ex)
        {
            //System.Diagnostics.Debug.WriteLine(ex.ToString());
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            XmlDoc.LoadXml("<?xml version = \"1.0\"><errors><error message=\"unable to convert string to xml\"/></errors>");
            return XmlDoc;
        }
    }

    public static DataTable getDataTableFromList(List<string> strColumns)
    {
        try
        {
            DataTable dt1 = new DataTable("Test");

            foreach (string s in strColumns)
            {
                dt1.Columns.Add(s, System.Type.GetType("system.string", false, true));
            }

            int rowcount = 0;
            while ((rowcount < 200))
            {
                List<Object> columnArray = new List<object>();

                foreach (string s in strColumns)
                {
                    columnArray.Add(s);
                }

                dt1.Rows.Add(columnArray.ToArray());
                rowcount += 1;
            }
            return dt1;
        }
        catch (Exception ex)
        {
            //System.Diagnostics.Debug.WriteLine(ex.ToString());
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return new DataTable("Test");
        }
    }

    public static DataTable getDataTableFromXml(string xml)
    {
        try
        {
            XmlDocument XmlDoc = GetValidXmlFromString(xml);
            DataTable dt = new DataTable(XmlDoc.DocumentElement.Name);
            bool FirstPass = true;
            XmlNodeList nodes = XmlDoc.DocumentElement.ChildNodes;
            foreach (XmlNode node in nodes)
            {
                if (FirstPass)
                {
                    FirstPass = false;
                    foreach (XmlAttribute attr in node.Attributes)
                    {
                        dt.Columns.Add(attr.Name);
                    }
                }
                DataRow dr = dt.NewRow();
                foreach (XmlAttribute attr in node.Attributes)
                {
                    dr[attr.Name] = attr.Value;
                }
                dt.Rows.Add(dr);

            }
            return dt;
        }
        catch (Exception ex)
        {
            //System.Diagnostics.Debug.WriteLine(ex.ToString());
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return new DataTable("error!");
        }

    }

    public static DataTable getTestDataTable(string FilePath)
    {
        try
        {
            System.IO.FileStream wordFile = new System.IO.FileStream(FilePath,
                System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.StreamReader baseReader = new System.IO.StreamReader(wordFile);
            String tempstring = baseReader.ReadToEnd().ToString();
            String[] arrWords = tempstring.Split('\n');

            DataTable dt = new DataTable("testTable");
            dt.Columns.Add("rw1");
            dt.Columns.Add("rw2");
            dt.Columns.Add("rw3");
            dt.Columns.Add("rw4");
            dt.Columns.Add("rw5");
            dt.Columns.Add("rw6");
            dt.Columns.Add("rw7");
            dt.Columns.Add("rw8");
            int rowcount = 0;
            while (rowcount < 1000)
            {
                String word1 = getRandomWord(arrWords);
                String word2 = getRandomWord(arrWords);
                String word3 = getRandomWord(arrWords);
                String word4 = getRandomWord(arrWords);
                String word5 = getRandomWord(arrWords);
                String word6 = getRandomWord(arrWords);
                String word7 = getRandomWord(arrWords);
                String word8 = getRandomWord(arrWords);

                dt.Rows.Add(word1, word2, word3, word4, word5, word6, word7, word8);
                rowcount += 1;
            }

            return dt;
        }
        catch (Exception ex)
        {
            //string unUsedException = ex.ToString();
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return null;
        }

    }

    private static string getRandomWord(string[] arrWords)
    {
        try
        {
            int randomint = getRandomNumNoBiggerThanCeiling(arrWords.Length - 1);
            return arrWords[randomint].ToString();
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return ex.ToString();
        }
    }

    private static int getRandomNumNoBiggerThanCeiling(int ceiling)
    {
        try
        {
            int timeSeed = DateTime.Now.Millisecond;
            System.Random random = new System.Random(timeSeed);
            int randomInt = random.Next();
            while (randomInt > ceiling)
            {
                randomInt -= ceiling;
            }
            while (randomInt < 0)
            {
                randomInt += (ceiling - 1);
            }
            return randomInt;
        }
        catch (Exception ex)
        {
            //string unusedException = ex.ToString();
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body);
            return 1;
        }
    }

    public static void MoveAndResizeImage(string CurrentImagePath, string NewImagePath)
    {
        try
        {
            int Width = 640;
            int Height = 0;
            Bitmap imgIn = new Bitmap(CurrentImagePath);
            double y = imgIn.Height;
            double x = imgIn.Width;
            double factor = 1;
            if ((Width > 0))
            {
                factor = Width / x;
            }
            else if ((Height > 0))
            {
                factor = Height / y;
            }

            if (imgIn.Width > 640)
            {
                Bitmap imgOut = new Bitmap((int)(x * factor), (int)(y * factor));
                Graphics g = Graphics.FromImage(imgOut);
                g.Clear(Color.White);
                g.DrawImage(imgIn, new Rectangle(0, 0, (int)(factor * x), (int)(factor * y)), new Rectangle(0, 0, (int)x, (int)y), GraphicsUnit.Pixel);
                imgOut.Save(NewImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                imgIn.Dispose();
                imgIn = null;
                imgOut.Dispose();
                imgOut = null;
                g.Dispose();
                g = null;
                GC.Collect();
                try
                {
                    System.IO.File.Delete(CurrentImagePath);
                }
                catch
                {
                }
            }
            else
            {
                imgIn.Dispose();
                imgIn = null;
                GC.Collect();
                //Have to release the hold on the file
                System.IO.File.Move(CurrentImagePath, NewImagePath);
                //Just move it since doesn't have to be resized
            }
        }
        catch (Exception ex)
        {
            string Body = "An error was encountered: " + Environment.NewLine + ex.ToString();
            Utils.SendEmail("Metatron@skynetsoftware.net", "jeff@skynetsoftware.net", "error encountered", Body); 
        }
    }

    //public static void ConvertPosts(HttpContext context)
    //{
    //    try
    //    {
    //        DataTable dt = XmlManager.GetPosts(context.Server.MapPath("~/App_Data/data.xml"));

    //        for (int RowCount = dt.Rows.Count - 1; RowCount >= 0; RowCount--)
    //        {
    //            DataRow dr = dt.Rows[RowCount];
    //            XmlManager.InsertBlogPost(context.Server.MapPath("~/App_Data/BlogPosts.xml"), dr["Title"].ToString(),
    //            dr["Description"].ToString(), dr["Body"].ToString(), dr["Link"].ToString(), 
    //            String.Format("{0:dddd, MMMM d, yyyy}", DateTime.Parse(dr["PubDate"].ToString())));
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string exString = ex.ToString();
    //        //return null;
    //    }
    //}

    //public static void ConvertComments(HttpContext context)
    //{
    //    DataTable dtComments = XmlManager.GetComments(context.Server.MapPath("~/App_Data/comment.xml"));
    //    List<BlogPost> Posts = XmlManager.GetBlogPosts(context.Server.MapPath("~/App_Data/BlogPosts.xml"));
    //    if (dtComments != null)
    //    {
    //        for (int CommentsRowCount = dtComments.Rows.Count - 1; CommentsRowCount >= 0; CommentsRowCount--)
    //        {
    //            DataRow drComment = dtComments.Rows[CommentsRowCount];
    //            long BlogPostId = 0;
    //            foreach(BlogPost post in Posts)
    //            {
    //                if (post.Link == drComment["Link"].ToString())
    //                {
    //                    BlogPostId = post.Id;
    //                }
    //            }
    //            if (BlogPostId != 0)
    //            {
    //                XmlManager.InsertBlogPostComment(context.Server.MapPath("~/App_Data/BlogComments.xml"), BlogPostId,
    //                drComment["User"].ToString(), drComment["Email"].ToString(), drComment["HomePage"].ToString(),
    //                drComment["Comment"].ToString());
    //            }
    //        }
    //    }
    //}
}
