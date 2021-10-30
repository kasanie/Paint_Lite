using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Lab2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripContainer1_ContentPanel_Load(object sender, System.EventArgs e)
        {

        }
        private void picDrawingSurface_MouseDown(object sender, MouseEventArgs e)
        {
            if (picDrawingSurface.Image == null)
            {
                MessageBox.Show("Сначала создайте новый файл!");
                return;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("Picture editor lite\ndeveloped by Sergey Vashchenko, 2021\nver 1.0");
        }

        private void newToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            NewImage(sender, e);
        }

        private void newToolStripButton_Click(object sender, System.EventArgs e)
        {
            NewImage(sender, e);
        }
        private void saveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SaveImage();
        }

        private void saveToolStripButton_Click(object sender, System.EventArgs e)
        {
            SaveImage();
        }

        private void openToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            OpenImage();
        }

        private void openToolStripButton_Click(object sender, System.EventArgs e)
        {
            OpenImage();
        }
        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            CloseApp(sender, e);
        }
        private void exitToolStripButton_Click(object sender, System.EventArgs e)
        {
            CloseApp(sender, e);
        }

        void NewImage(object sender, System.EventArgs e)
        {
            if (picDrawingSurface.Image != null)
            {
                var result = MessageBox.Show("Save the current image before creating a new one?", "Warning", MessageBoxButtons.YesNoCancel);

                switch (result)
                {
                    case DialogResult.No: break;
                    case DialogResult.Yes: saveToolStripMenuItem_Click(sender, e); break;
                    case DialogResult.Cancel: return;
                }
            }
            Bitmap pic = new Bitmap(700, 500);
            picDrawingSurface.Image = pic;
        }
        void SaveImage()
        {
            SaveFileDialog SaveDlg = new SaveFileDialog();
            SaveDlg.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif|PNG Image|*.png";
            SaveDlg.Title = "Save an Image File";
            SaveDlg.FilterIndex = 4;    //По умолчанию будет выбрано последнее расширение *.png

            SaveDlg.ShowDialog();
            if (SaveDlg.FileName != "")     //Если введено не пустое имя
            {
                System.IO.FileStream fs =
                   (System.IO.FileStream)SaveDlg.OpenFile();

                switch (SaveDlg.FilterIndex)
                {
                    case 1:
                        this.picDrawingSurface.Image.Save(fs, ImageFormat.Jpeg);
                        break;

                    case 2:
                        this.picDrawingSurface.Image.Save(fs, ImageFormat.Bmp);
                        break;

                    case 3:
                        this.picDrawingSurface.Image.Save(fs, ImageFormat.Gif);
                        break;

                    case 4:
                        this.picDrawingSurface.Image.Save(fs, ImageFormat.Png);
                        break;
                }

                fs.Close();
            }
        }
        void OpenImage()
        {
            OpenFileDialog OP = new OpenFileDialog();
            OP.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif|PNG Image|*.png";
            OP.Title = "Open an Image File";
            OP.FilterIndex = 1; //По умолчанию будет выбрано первое расширение *.jpg
            if (OP.ShowDialog() != DialogResult.Cancel)
                picDrawingSurface.Load(OP.FileName);

            picDrawingSurface.AutoSize = true;
        }
        void CloseApp(object sender, System.EventArgs e)
        {
            if (picDrawingSurface.Image != null)
            {
                var result = MessageBox.Show("Save the current image before closing the application?", "Warning", MessageBoxButtons.YesNoCancel);

                switch (result)
                {
                    case DialogResult.No: break;
                    case DialogResult.Yes: saveToolStripMenuItem_Click(sender, e); break;
                    case DialogResult.Cancel: return;
                }
            }
            Application.Exit();
        }
    }

}
