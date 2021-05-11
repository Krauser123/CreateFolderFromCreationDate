using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace CreateFolderFromCreationDate
{
    internal class FileUtils
    {
        private static readonly ILog _logger = Logger.Create();

        /// <summary>
        /// Most used extensions for image and video files
        /// </summary>
        private static readonly string[] availableExtensions = new string[] { ".dib", ".arw", ".nrw", ".k25", ".webp", ".jif", ".jfif", ".jfi", ".jp2", "webp", ".indd", ".AI", ".eps", ".pdf", ".heif", ".jpg", ".png", "jpeg", ".eps", ".cr2", ".xcf", ".orf", ".psd", ".sr2", ".raw", ".exif", ".jpe", ".3gp", ".bmp", ".tiff", ".tif", ".gif", ".svg", ".jp2", ".j2k", ".jpf", ".jpx", ".jpm", ".mj2", "mp4", ".mov", ".wmv", ".avi", ".avchd", ".flv", ".f4v", "swf", ".mkv", ".ogg" };

        /// <summary>
        /// Search files in folderPath
        /// </summary>
        /// <param name="folderPath"></param>
        public Dictionary<long, string> SearchFiles(string folderPath, ref ToolStripLabel lblInfoImport, bool checkExtensions, ref ToolStripProgressBar progressBar)
        {
            Dictionary<long, string> pathList = new Dictionary<long, string>();
            ToolStripLabel lblInfoImportRef = lblInfoImport;
            ToolStripProgressBar progressBarRef = progressBar;

            // Create an enumeration of the files we will want to process that simply accumulates these values...
            long total = 0;
            var fileCounter = new CSharpTest.Net.IO.FindFile(folderPath, "*.*", true, true, true)
            {
                RaiseOnAccessDenied = false
            };
            fileCounter.FileFound +=
                (o, e) =>
                {
                    if (!e.IsDirectory)
                    {
                        Interlocked.Increment(ref total);
                    }
                };

            // Start a high-priority thread to perform the accumulation
            Thread t = new Thread(fileCounter.Find)
            {
                IsBackground = true,
                Priority = ThreadPriority.AboveNormal,
                Name = "file enum"
            };
            t.Start();

            // Allow the accumulator thread to get a head-start on us
            do
            {
                Thread.Sleep(100);
            }
            while (total < 100 && t.IsAlive);

            // Now we can process the files normally and update a percentage
            long count = 0;
            long percentage = 0;
            var filesToProcess = new CSharpTest.Net.IO.FindFile(folderPath, "*.*", true, true, true)
            {
                RaiseOnAccessDenied = false
            };
            filesToProcess.FileFound +=
                (o, file) =>
                {
                    if ((!file.IsDirectory && !checkExtensions)
                    || (checkExtensions && availableExtensions.Contains(file.Extension.ToLower())))
                    {
                        // Add to
                        pathList.Add(count, file.FullPath);
                    }

                    // Update the percentage complete
                    long progress = ++count * 100 / Interlocked.Read(ref total);
                    lblInfoImportRef.Text = string.Format("{0} Items found", count);
                    if (progress > percentage && progress <= 100)
                    {
                        percentage = progress;
                        progressBarRef.Value = Convert.ToInt32(percentage);
                    }
                };

            filesToProcess.Find();
            lblInfoImportRef.Text = string.Format("{0} Items found", count);

            return pathList;
        }

        /// <summary>
        /// Returns the EXIF Image Data of the Date Taken GPS.
        /// </summary>
        /// <param name="getImage">Image (If based on a file use Image.FromFile(f);)</param>
        /// <returns>Date Taken or Null if Unavailable</returns>
        public static DateTime DateGPSTaken(string dateTakenTag)
        {
            string[] parts = dateTakenTag.Split(':', ' ');
            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);

            return new DateTime(year, month, day);
        }

        /// <summary>
        /// Returns the EXIF Image Data of the Date Taken.
        /// </summary>
        /// <param name="getImage">Image (If based on a file use Image.FromFile(f);)</param>
        /// <returns>Date Taken or Null if Unavailable</returns>
        public static DateTime DateTaken(string dateTakenTag)
        {
            string[] parts = dateTakenTag.Split(':', ' ');
            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);
            int hour = int.Parse(parts[3]);
            int minute = int.Parse(parts[4]);
            int second = int.Parse(parts[5]);

            return new DateTime(year, month, day, hour, minute, second);
        }

        /// <summary>
        /// Check if a string have Exif date format
        /// </summary>
        /// <param name="dateFormat"></param>
        /// <returns></returns>
        public bool IsExifFormat(string dateFormat)
        {
            bool isExifFormat = false;
            int count = dateFormat.Count(f => f == ':');

            if (count >= 4)
            {
                isExifFormat = true;
            }

            return isExifFormat;
        }

        /// <summary>
        /// Check if a string have Exif GPS date format
        /// </summary>
        /// <param name="dateFormat"></param>
        /// <returns></returns>
        public bool IsExifGPSFormat(string dateFormat)
        {
            bool isExifFormat = false;
            int count = dateFormat.Count(f => f == ':');

            if (count == 2 && dateFormat.Length == 10)
            {
                isExifFormat = true;
            }

            return isExifFormat;
        }

        /// <summary>
        /// Get the lowest year between file properties
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public int GetMinimunYear(FileInfoExtended file)
        {
            List<DateTime> validDates = new List<DateTime>();
            validDates.Add(file.ExtendedInfo.LastWriteTime);
            validDates.Add(file.ExtendedInfo.CreationTime);

            var dateRelatedMetadatas = file.Metadatas.Where(o => o.Metadata != null && o.Metadata.ToLower().Contains("date"));
            foreach (var metadata in dateRelatedMetadatas)
            {
                try
                {
                    validDates.Add(GetDateTimeFromMetadata(metadata));
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message + " - Metadata: " + metadata.Metadata + " - Value " + metadata.Description);
                }
            }

            //Get little value in date as valid year
            int year = validDates.Min(a => a).Year;

            return year;
        }

        /// <summary>
        /// Return the datetime inside of Metadata
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        private DateTime GetDateTimeFromMetadata(MetaData metadata)
        {
            DateTime dateToReturn;

            if (IsExifFormat(metadata.Description))
            {
                dateToReturn = DateTaken(metadata.Description);
            }
            else if (IsExifGPSFormat(metadata.Description))
            {
                dateToReturn = DateGPSTaken(metadata.Description);
            }
            else
            {
                bool isValidDateTime = DateTime.TryParse(metadata.Description, out DateTime dateConverted);

                if (!isValidDateTime)
                {
                    dateConverted = DateTime.ParseExact(metadata.Description, "ddd MMM dd HH:mm:ss K yyyy", CultureInfo.InvariantCulture);
                }

                dateToReturn = dateConverted;
            }

            return dateToReturn;
        }

    }
}
