using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDFReader;
using PDFTextReader;

namespace PDFExtractor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void txOutFolder_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PDF files (*.pdf)|*.pdf";
            if ( ofd.ShowDialog() == DialogResult.OK)
            {
                txPDFFileName.Text = ofd.FileName;
                if (txOutFolder.Text.Trim() == "")
                    txOutFolder.Text = Path.GetDirectoryName(txPDFFileName.Text);

            }
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if ( fbd.ShowDialog()== DialogResult.OK)
            {
                txOutFolder.Text = fbd.SelectedPath;
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (txPDFFileName.Text.Trim() != "" &&
                File.Exists(txPDFFileName.Text))
            {
                try
                {
                    Cursor = Cursors.WaitCursor;

                    var textExtractor = new PDFTextReader.PDFTextReader();
                    var list = textExtractor.GetText(txPDFFileName.Text);

                    var textFolder = $"{txOutFolder.Text}\\{Path.GetFileNameWithoutExtension(txPDFFileName.Text)}\\Text";
                    if (!Directory.Exists(textFolder))
                        Directory.CreateDirectory(textFolder);

                    
                    for (int i = 0; i < list.Count; i++)
                    {
                        string fileName = $"{textFolder}\\Text_Page_{i.ToString().PadLeft(4, '0')}.txt";
                        File.WriteAllText( fileName, list[i]);
                    };


                    var imageExtractor = new PDFProcessor();
                    imageExtractor.exportImages(txPDFFileName.Text, txOutFolder.Text);

                    MessageBox.Show("process finished SUCCESFULLY. Check Folder");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Processing file", ex.Message);
                }
                Cursor = Cursors.Default;

            }
            else MessageBox.Show("Select an existing PDF File, and a output folder");
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            if (txOutFolder.Text.Trim() != "" &&
                Directory.Exists(txOutFolder.Text)
                )
                System.Diagnostics.Process.Start(txOutFolder.Text);
            else MessageBox.Show("Invalid Folder");

        }
    }
}
