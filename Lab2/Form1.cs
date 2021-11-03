using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Lab2
{
    public partial class Form1 : Form
    {
        bool drawing;
        int tempCount;
        List<Image> tempImages = new List<Image>();
        GraphicsPath currentPath;
        Point oldLocation;
        Pen currentPen;
        Color tempColor;
        public Form1()
        {
            InitializeComponent();
            drawing = false;
            currentPen = new Pen(Color.Black);
            currentPen.Width = trackBar1.Value;
            textBox1.BackColor = currentPen.Color;
        }
        private void picDrawingSurface_MouseDown(object sender, MouseEventArgs e)
        {
            if (picDrawingSurface.Image == null)
            {
                MessageBox.Show("Сначала создайте новый файл!");
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                drawing = true;
                oldLocation = e.Location;
                currentPath = new GraphicsPath();
            }
            if (e.Button == MouseButtons.Right)
            {
                drawing = true;
                oldLocation = e.Location;
                currentPath = new GraphicsPath();
                tempColor = currentPen.Color;
                currentPen.Color = Color.White;
            }
            }
        private void picDrawingSurface_MouseUp(object sender, MouseEventArgs e)
        {
            tempImages.RemoveRange(tempCount + 1, tempImages.Count - tempCount - 1);
            tempImages.Add(new Bitmap(picDrawingSurface.Image));
            if (tempCount + 1 < 20) tempCount++;
            if (tempImages.Count - 1 == 20) tempImages.RemoveAt(0);
            if (e.Button == MouseButtons.Right)
            {
                currentPen.Color = tempColor;
            }
            drawing = false;
            try
            {
                currentPath.Dispose();
            }
            catch { };
        }
        private void picDrawingSurface_MouseMove(object sender, MouseEventArgs e)
        {
            label_XY.Text = e.X.ToString() + ", " + e.Y.ToString();
            if (drawing)
            {
                Graphics g = Graphics.FromImage(picDrawingSurface.Image);
                currentPath.AddLine(oldLocation, e.Location);
                g.DrawPath(currentPen, currentPath);
                oldLocation = e.Location;
                g.Dispose();
                picDrawingSurface.Invalidate();
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
            tempImages.Clear();
            tempCount = 0;
            Bitmap pic = new Bitmap(700, 500);
            picDrawingSurface.Image = pic;
            tempImages.Add(new Bitmap(picDrawingSurface.Image));
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
            OP.FilterIndex = 4;
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

        private void trackBar1_Scroll(object sender, System.EventArgs e)
        {
            currentPen.Width = trackBar1.Value;
        }

        private void undoToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (tempImages.Count != 0 && tempCount != 0)
            {
                picDrawingSurface.Image = new Bitmap(tempImages[--tempCount]);
            }
            else MessageBox.Show("Nothing to change.");
        }

        private void renoToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (tempCount < tempImages.Count - 1)
            {
                picDrawingSurface.Image = new Bitmap(tempImages[++tempCount]);
            }
            else MessageBox.Show("Nothing to change.");
        }

        private void solidToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            currentPen.DashStyle = DashStyle.Solid;

            solidToolStripMenuItem.Checked = true;
            dotToolStripMenuItem.Checked = false;
            dashDotDotToolStripMenuItem.Checked = false;
        }

        private void dotToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            currentPen.DashStyle = DashStyle.Dot;

            solidToolStripMenuItem.Checked = false;
            dotToolStripMenuItem.Checked = true;
            dashDotDotToolStripMenuItem.Checked = false;
        }

        private void dashDotDotToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            currentPen.DashStyle = DashStyle.DashDotDot;

            solidToolStripMenuItem.Checked = false;
            dotToolStripMenuItem.Checked = false;
            dashDotDotToolStripMenuItem.Checked = true;
        }

        private void colorToolStripButton_Click(object sender, System.EventArgs e)
        {
            colorDialog1.ShowDialog();
            currentPen.Color = colorDialog1.Color;
            textBox1.BackColor = colorDialog1.Color;
        }

        private void colorToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            colorDialog1.ShowDialog();
            currentPen.Color = colorDialog1.Color;
            textBox1.BackColor = colorDialog1.Color;
        }
    }

}
