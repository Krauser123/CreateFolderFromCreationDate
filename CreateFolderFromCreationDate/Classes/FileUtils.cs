using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace CreateFolderFromCreationDate
{
    class FileUtils
    {
        /// <summary>
        /// Search files in folderPath
        /// </summary>
        /// <param name="folderPath"></param>
        public Dictionary<long, string> SearchFiles(string folderPath, ref ToolStripLabel lblInfoImport, ref ToolStripProgressBar progressBar)
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
                    if (!file.IsDirectory)
                    {
                        // Read an epub file                        
                        pathList.Add(count, file.FullPath);

                        // Update the percentage complete...
                        long progress = ++count * 100 / Interlocked.Read(ref total);
                        lblInfoImportRef.Text = string.Format("{0} Items found", count);
                        if (progress > percentage && progress <= 100)
                        {
                            percentage = progress;
                            progressBarRef.Value = Convert.ToInt32(percentage);
                        }
                    }
                };

            filesToProcess.Find();
            lblInfoImportRef.Text = string.Format("{0} Items found", count);

            return pathList;
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

        public bool IsExifFormat(string dateFormat)
        {
            bool isExifFormat = false;
            int count = dateFormat.Count(f => f == ':');

            if (count > 5)
            {
                isExifFormat = true;
            }

            return isExifFormat;
        }

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
                    if (IsExifFormat(metadata.Description))
                    {
                        validDates.Add(DateTaken(metadata.Description));
                    }
                    else
                    {
                        validDates.Add(DateTime.Parse(metadata.Description));
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            }

            //Get little value in date as valid year
            int year = validDates.Min(a => a).Year;

            return year;
        }
    }
}
