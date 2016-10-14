using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace MaxSu.Framework.Common.IO
{
    static public class CommonZipFunctions
    {
        /// <summary>
        /// UnZip ZipFiles in output directory
        /// </summary>
        /// <param name="fileFullPathName"></param>
        /// <param name="outputDirectory"></param>
        static public void UnZip(string fileFullPathName, string outputDirectory)
        {
            ZipInputStream myZipInputStream = new ZipInputStream(File.OpenRead(fileFullPathName));
            ZipEntry myZipEntry;

            while ((myZipEntry = myZipInputStream.GetNextEntry()) != null)
            {
                if (myZipEntry.IsDirectory)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(outputDirectory, myZipEntry.Name)));
                }
                else
                {
                    // Ensure non-empty file name
                    if (myZipEntry.Name.Length > 0)
                    {
                        // Create a new file, get the file stream
                        FileStream myFileStream = File.Create(Path.Combine(outputDirectory, myZipEntry.Name));
                        int size = 2048;
                        byte[] data = new byte[2048];

                        // Write out the file
                        while (true)
                        {
                            size = myZipInputStream.Read(data, 0, data.Length);

                            if (size > 0)
                            {
                                myFileStream.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }

                        myFileStream.Close();
                    }
                }
            }

            myZipInputStream.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filesToZip"></param>
        /// <param name="path"></param>
        /// <param name="compression"></param>
        /// <param name="IgnoreFileNames">KeyValuePair name and mimeType</param>
        /// <param name="mimeType"></param>
        public static void WriteZipFileIgnoreCase(List<string> filesToZip, string path, int compression, List<KeyValuePair<string, string>> IgnoreFileNames)
        {
            ValidateCompressionRate(compression);

            ValidateDirectoryExist(path);

            ValidateFilesExiste(filesToZip);

            Crc32 crc32 = new Crc32();
            ZipOutputStream stream = new ZipOutputStream(File.Create(path));
            stream.SetLevel(compression);

            for (int i = 0; i < filesToZip.Count; i++)
            {
                string zipEntryName = Path.GetFileName(filesToZip[i]);
                string zipEntryNameNoMimeType = zipEntryName.Split('.')[0];

                //if (zipEntryNameNoMimeType.Equals(IgnoreFileName) == false)
                if (IgnoreFileNames.Exists(c => c.Key == zipEntryNameNoMimeType) == false)
                {
                    string fullPathNameFile = filesToZip[i];

                    AddFileToZipoutputStrem(crc32, stream, zipEntryName, fullPathNameFile);
                }
            }

            stream.Finish();
            stream.Close();
        }

        private static void AddFileToZipoutputStrem(Crc32 crc32, ZipOutputStream stream, string zipEntryName, string fullPathNameFile)
        {
            ZipEntry entry = new ZipEntry(zipEntryName);
            entry.DateTime = DateTime.Now;

            using (FileStream fs = File.OpenRead(fullPathNameFile))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                //entry.Size = fs.Length;
                fs.Close();
                crc32.Reset();
                crc32.Update(buffer);
                entry.Crc = crc32.Value;
                stream.PutNextEntry(entry);
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filesToZip"></param>
        /// <param name="path"></param>
        /// <param name="compression"></param>
        public static void WriteZipFile(List<string> filesToZip, string path, int compression, string AddFileName, string mimeType, byte[] bytecontent)
        {
            ValidateCompressionRate(compression);

            ValidateDirectoryExist(path);

            ValidateFilesExiste(filesToZip);

            Crc32 crc32 = new Crc32();
            ZipOutputStream stream = new ZipOutputStream(File.Create(path));
            stream.SetLevel(compression);

            for (int i = 0; i < filesToZip.Count; i++)
            {
                string zipEntryName = Path.GetFileName(filesToZip[i]);
                string zipEntryNameNoMimeType = zipEntryName.Split('.')[0];

                if (zipEntryNameNoMimeType.Equals(AddFileName) == false)
                {
                    string fullPathNameFile = filesToZip[i];

                    AddFileToZipoutputStrem(crc32, stream, zipEntryName, fullPathNameFile);
                }
            }

            if (bytecontent != null)
                AddStreamFileToZipOutputStream(stream, new byte[bytecontent.Length], AddFileName + mimeType, bytecontent);

            stream.Finish();
            stream.Close();
        }

        private static void ValidateFilesExiste(List<string> filesToZip)
        {
            foreach (string c in filesToZip)
                if (!File.Exists(c))
                    throw new ArgumentException(string.Format("The File {0} does not exist!", c));
        }

        private static void ValidateDirectoryExist(string path)
        {
            if (!Directory.Exists(new FileInfo(path).Directory.ToString()))
                throw new ArgumentException("The Path does not exist.");
        }

        private static void ValidateCompressionRate(int compression)
        {
            if (compression < 0 || compression > 9)
                throw new ArgumentException("Invalid compression rate.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipStream"></param>
        /// <param name="buffer"></param>
        /// <param name="zipEntryName"></param>
        /// <param name="byteContent"></param>
        public static void AddStreamFileToZipOutputStream(ZipOutputStream zipStream, byte[] buffer, string zipEntryName, byte[] byteContent)
        {
            var fileName = ZipEntry.CleanName(zipEntryName);

            ZipEntry entry = new ZipEntry(fileName);
            zipStream.PutNextEntry(entry);
            var inStream = new MemoryStream(byteContent);
            StreamUtils.Copy(inStream, zipStream, buffer);
            inStream.Close();
        }
    }
}
