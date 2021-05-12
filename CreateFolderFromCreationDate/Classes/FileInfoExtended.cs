using log4net;
using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace CreateFolderFromCreationDate
{
    internal class FileInfoExtended
    {
        private static readonly ILog mLog = Logger.Create();

        //These properties do not appears in DataGrids
        [Browsable(false)]
        public string NameWithoutExtension { get; set; }
        [Browsable(false)]
        public string Location { get; set; }
        [Browsable(false)]
        public FileInfo ExtendedInfo { get; set; }
        [Browsable(false)]
        private string LocationAfterChanges { get; set; }
        [Browsable(false)]
        private string FileExtension { get; set; }
        [Browsable(false)]
        public List<MetaData> Metadatas = new List<MetaData>();

        //Public Properties
        public string Name { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }
        public string LastAccessed { get; set; }
        public string FileSize { get; set; }

        public FileInfoExtended(string path)
        {
            try
            {
                //Set location
                this.Location = this.LocationAfterChanges = path;

                //Get name
                this.Name = Path.GetFileName(path);
                this.NameWithoutExtension = Path.GetFileNameWithoutExtension(path);

                //Get extension
                this.FileExtension = Path.GetExtension(path);

                //Load extended properties
                LoadFileInfo();
            }
            catch (Exception ex) 
            {
                mLog.Error(ex.Message + " Path->" + path);
            }
        }

        private void LoadFileInfo()
        {
            //Only load if is null
            if (ExtendedInfo == null)
            {
                ExtendedInfo = new FileInfo(this.Location);
                Created = ExtendedInfo.CreationTime.ToShortDateString();
                Modified = ExtendedInfo.LastWriteTime.ToShortDateString();
                LastAccessed = ExtendedInfo.LastAccessTime.ToShortDateString();
                FileSize = GetFormatedFileSize();
                GetMetadata();
            }
        }

        public void RenameFile()
        {
            try
            {
                File.Move(this.Location, this.LocationAfterChanges);
            }
            catch (Exception ex)
            {
                mLog.Error(ex.Message + " - Location: " + this.Location);
            }
        }

        private string GetFormatedFileSize()
        {
            long length = ExtendedInfo.Length;

            if (length < Math.Pow(1024, 1)) { return length + " B"; }
            if (length < Math.Pow(1024, 2)) { return DivideSize(length, 1) + " KB"; }
            if (length < Math.Pow(1024, 3)) { return DivideSize(length, 2) + " MB"; }
            if (length < Math.Pow(1024, 4)) { return DivideSize(length, 3) + " GB"; }

            return length + " TB";
        }

        private double DivideSize(double length, int unitsToDivide = 0)
        {
            for (int i = 0; i < unitsToDivide; i++)
            {
                length /= 1024;
            }

            return Math.Round(Convert.ToDouble(length), 2);
        }

        private void GetMetadata()
        {
            if (this.FileExtension != string.Empty)
            {
                var directories = ImageMetadataReader.ReadMetadata(this.Location);

                //Iterate over all metadata
                foreach (var directory in directories)
                {
                    foreach (var tag in directory.Tags)
                    {
                        Metadatas.Add(new MetaData(directory.Name, tag.Name, tag.Description));
                    }
                }
            }
        }
    }
}
