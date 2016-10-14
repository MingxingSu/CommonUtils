using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MaxSu.Framework.Common.Ftp
{
    //only applicable for Console currently
    public class UploadFileToFtp
    {
        static void Upload(string ftpServerAddress,
            string remotePath,
            String ftpUsername,
            string ftpPassword,
            string sourceFile
                )
        {
            try
            {
                //Check source file
                if (string.IsNullOrWhiteSpace(sourceFile))
                {
                    Console.WriteLine("No source file specified!");
                    return;
                }
                string sourceFilePath = sourceFile;
                if (String.IsNullOrEmpty(sourceFilePath) || File.Exists(sourceFilePath) == false)
                {
                    Console.WriteLine("Source file not found!");
                    return;
                }

                string ftpUrl = String.Format("ftp://{0}/{1}",ftpServerAddress,remotePath);
                //Show configurations to user
                string filename = Path.GetFileName(sourceFilePath);
                Console.WriteLine("FTP Path: " + ftpUrl);
                Console.WriteLine("FTP Username: " + ftpUsername);
                Console.WriteLine("File selected:" + filename);

                //Create ftp request based on remote folder and source file name
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(Path.Combine(ftpUrl, filename));
                ftp.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;

                //Save the file content to memory
                FileStream fs = File.OpenRead(sourceFilePath);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                //Write file to file ftp server
                Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(buffer, 0, buffer.Length);
                ftpstream.Close();

                Console.WriteLine("Done!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
