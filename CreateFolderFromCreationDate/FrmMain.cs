using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CreateFolderFromCreationDate
{
    public partial class FrmMain : Form
    {
        const string InfoMessage = "CreateFolderFromCreationDate v1.1";

        public ILog _logger = Logger.Create();
        private readonly FileUtils Utils = new FileUtils();
        private BindingSource BindingSource = new BindingSource();
        private List<string> FilesPathList = new List<string>();
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
                txtOriginLocation.Text = path;
                FilesPathList = Utils.SearchFiles(path, ref toolStripLabelInfo, ref toolStripProgressBar).Select(o => o.Value).ToList();
                LoadFilesToDataGrid();
            }
        }

        private void LoadFilesToDataGrid()
        {
            try
            {
                //Initialise FileObj
                FilesWithInfoExtended = FilesPathList.Select(o => new FileInfoExtended(o)).ToList();

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
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the directory that you want to use to generate folders";
            folderBrowserDialog.ShowNewFolderButton = true;

            // Show the FolderBrowserDialog.
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog.SelectedPath;
                txtLocationToGenerate.Text = folderName;
            }
        }

        private void StartMoveFilesProcess()
        {
            foreach (var file in FilesWithInfoExtended)
            {
                int year = Utils.GetMinimunYear(file);

                //If directory not exist we create it
                string folderPath = txtLocationToGenerate.Text + "\\" + year;
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    _logger.Info("Directory " + folderPath + " created.");
                }

                try
                {
                    //Check if we can move this file
                    int counter = 0;
                    var fileToMoveNewPath = folderPath + "\\" + file.Name;
                    while (File.Exists(fileToMoveNewPath))
                    {
                        counter++;
                        fileToMoveNewPath = folderPath + "\\" + file.NameWithoutExtension + "-" + counter + file.ExtendedInfo.Extension;
                    }

                    //Move file
                    file.ExtendedInfo.MoveTo(fileToMoveNewPath);
                    _logger.Info("Moved file to: " + fileToMoveNewPath);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            }

            MessageBox.Show("Process finished... " + FilesWithInfoExtended.Count + " items moved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
