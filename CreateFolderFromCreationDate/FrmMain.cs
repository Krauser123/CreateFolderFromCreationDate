using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateFolderFromCreationDate
{
    public partial class FrmMain : Form
    {
        const string InfoMessage = "CreateFolderFromCreationDate v1.4";

        private ILog _logger = Logger.Create();
        private FileUtils Utils = new FileUtils();
        private BindingSource BindingSource = new BindingSource();
        private List<FileInfoExtended> FilesWithInfoExtended = new List<FileInfoExtended>();

        public FrmMain()
        {
            InitializeComponent();

            //Setup dataGrids
            dgvFiles.ReadOnly = true;
            dgvFiles.AllowUserToResizeColumns = true;
            dgvFiles.AllowUserToResizeRows = true;

            //Resize columns in DataGridView
            dgvFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //Start log
            _logger.Info("Starting " + InfoMessage);
        }

        private void HelpToolStripButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(InfoMessage, "About...", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OpenToolStripButton_Click(object sender, EventArgs e)
        {
            string path = OpenFileDialog();
            if (!string.IsNullOrEmpty(path))
            {
                //Set path to textbox
                txtOriginLocation.Text = path;
                txtLocationToGenerate.Text = path;

                //Search files
                var FilesPathList = Utils.SearchFiles(path, ref toolStripLabelInfo, chkCheckExtensions.Checked, ref toolStripProgressBar).Select(o => o.Value).ToList();
                LoadFilesToDataGrid(FilesPathList);
            }
        }

        private void LoadFilesToDataGrid(List<string> filesPathList)
        {
            try
            {
                LoadGlobalList(filesPathList);

                //Refresh grids
                LoadDataBinding_Files();
            }
            catch (DirectoryNotFoundException e)
            {
                _logger.Error(e.Message);
            }
        }

        private void LoadDataBinding_Files()
        {
            BindingSource.DataSource = FilesWithInfoExtended;
            dgvFiles.DataSource = BindingSource;
        }

        private void LoadGlobalList(List<string> filesPathList)
        {
            try
            {

                //Clean global file list
                FilesWithInfoExtended.Clear();
                dgvFiles.ClearSelection();
                dgvFiles.DataSource = null;

                //Calc chunk size and split lists
                var chunkSize = Math.Round((decimal)filesPathList.Count / 4);
                var listOfPathsLists = ListExtensions.ChunkBy<string>(filesPathList, (int)chunkSize);

                List<Task<List<FileInfoExtended>>> groupOfTasks = new List<Task<List<FileInfoExtended>>>();
                foreach (var singlePathList in listOfPathsLists)
                {
                    Task<List<FileInfoExtended>> task = Task.Factory.StartNew(() => singlePathList.Select(o => new FileInfoExtended(o)).ToList());
                    groupOfTasks.Add(task);
                }

                //Wait for complete all our threads
                Task.WaitAll(groupOfTasks.ToArray());

                //Add resolved files to global list
                foreach (var completedTask in groupOfTasks)
                {
                    FilesWithInfoExtended.AddRange(completedTask.Result);
                }
            }
            catch (Exception ex)
            {
                _logger.Info("" + ex);
            }
        }

        private string OpenFileDialog()
        {
            var filePath = string.Empty;
            string folderSel = "Folder Selection";

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.ValidateNames = false;
                openFileDialog.CheckFileExists = false;
                openFileDialog.CheckPathExists = true;
                openFileDialog.FileName = folderSel;

                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog.Filter = "All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName.Replace(folderSel, "");
                }
            }

            return filePath;
        }

        private void BtnGenerateFolder_Click(object sender, EventArgs e)
        {
            //Check if we have a valid output directory
            if (!String.IsNullOrEmpty(txtLocationToGenerate.Text) && Directory.Exists(txtLocationToGenerate.Text))
            {
                StartMoveFilesProcess();
            }
            else
            {
                MessageBox.Show("Set a location for output", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            string folderSel = "Folder Selection";

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select the directory that you want to use to generate folders";
                openFileDialog.ValidateNames = false;
                openFileDialog.CheckFileExists = false;
                openFileDialog.CheckPathExists = true;
                openFileDialog.FileName = folderSel;

                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog.Filter = "All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    txtLocationToGenerate.Text = openFileDialog.FileName.Replace(folderSel, "");
                }
            }
        }

        private void StartMoveFilesProcess()
        {
            foreach (var file in FilesWithInfoExtended)
            {
                int year = Utils.GetMinimunYear(file);

                //If directory not exist we create it
                string folderPath = txtLocationToGenerate.Text + year;
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    _logger.Info("Directory " + folderPath + " created.");
                }

                try
                {
                    //Check if we can move this file
                    int counter = 0;
                    var fileToMoveNewPath = folderPath + Path.DirectorySeparatorChar + file.Name;
                    //File exists in this folder but come from another location
                    if (File.Exists(fileToMoveNewPath) && fileToMoveNewPath != file.Location)
                    {
                        while (File.Exists(fileToMoveNewPath))
                        {
                            counter++;
                            fileToMoveNewPath = folderPath + "\\" + file.NameWithoutExtension + "-" + counter + file.ExtendedInfo.Extension;
                        }
                    }

                    //Move file
                    file.ExtendedInfo.MoveTo(fileToMoveNewPath);
                    _logger.Info("Moved file to: " + fileToMoveNewPath);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message + " - " + file.Location);
                }
            }

            string logMessage = "Process finished... " + FilesWithInfoExtended.Count + " items moved.";
            MessageBox.Show(logMessage, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _logger.Info(logMessage);
        }
    }
}
