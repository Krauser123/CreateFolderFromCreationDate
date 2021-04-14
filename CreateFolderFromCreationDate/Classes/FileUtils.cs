using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Directory = System.IO.Directory;

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

        private List<string> GetFiles(string initialDir, bool recursiveSearch, string[] extensionFilters)
        {
            //Get files in root directory
            List<string> files = null;

            if (extensionFilters != null)
            {
                files = extensionFilters.SelectMany(filter => Directory.GetFiles(initialDir, filter)).ToList();
            }
            else
            {
                files = Directory.GetFiles(initialDir).ToList();
            }

            if (recursiveSearch)
            {
                var dir = Directory.GetDirectories(initialDir);

                foreach (var subDir in dir)
                {
                    var subItems = extensionFilters.SelectMany(filter => Directory.GetFiles(subDir, filter)).ToList();
                    files.AddRange(subItems);
                }
            }

            return files;
        }
    }
}