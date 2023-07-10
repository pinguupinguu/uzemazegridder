using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace GridToggler
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    public class MainForm : Form
    {
        private const int GRID_SIZE = 20;
        private const int BOX_SIZE = 20;

        private int[,] boxes;
        private Rectangle[,] rectangles;
        private bool isFilling;
        private bool isClearing;

        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveImageToolStripMenuItem;
        private ToolStripMenuItem clearGridToolStripMenuItem;

        public MainForm()
        {
            InitializeComponent();

            // Initialize grid of boxes
            boxes = new int[GRID_SIZE, GRID_SIZE];
            rectangles = new Rectangle[GRID_SIZE, GRID_SIZE];

            for (int x = 0; x < GRID_SIZE; x++)
            {
                for (int y = 0; y < GRID_SIZE; y++)
                {
                    boxes[x, y] = 0;
                    rectangles[x, y] = new Rectangle(x * BOX_SIZE, y * BOX_SIZE, BOX_SIZE, BOX_SIZE);
                }
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int x = 0; x < GRID_SIZE; x++)
            {
                for (int y = 0; y < GRID_SIZE; y++)
                {
                    Rectangle rect = rectangles[x, y];

                    if (boxes[x, y] == 1)
                        g.FillRectangle(Brushes.Black, rect);
                    else
                        g.FillRectangle(Brushes.White, rect);

                    g.DrawRectangle(Pens.Black, rect);
                }
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                isFilling = true;
            else if (e.Button == MouseButtons.Right)
                isClearing = true;

            ToggleBoxState(e);
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            isFilling = false;
            isClearing = false;
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isFilling || isClearing)
                ToggleBoxState(e);
        }

        private void ToggleBoxState(MouseEventArgs e)
        {
            int x = e.X / BOX_SIZE;
            int y = e.Y / BOX_SIZE;

            if (x >= 0 && x < GRID_SIZE && y >= 0 && y < GRID_SIZE)
            {
                if (isFilling)
                    boxes[x, y] = 1;
                else if (isClearing)
                    boxes[x, y] = 0;

                Invalidate(rectangles[x, y]);
            }
        }

        private void SaveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Bitmap bitmap = new Bitmap(GRID_SIZE * BOX_SIZE, GRID_SIZE * BOX_SIZE))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    for (int x = 0; x < GRID_SIZE; x++)
                    {
                        for (int y = 0; y < GRID_SIZE; y++)
                        {
                            Rectangle rect = rectangles[x, y];

                            if (boxes[x, y] == 1)
                                g.FillRectangle(Brushes.Black, rect);
                            else
                                g.FillRectangle(Brushes.White, rect);

                            g.DrawRectangle(Pens.Black, rect);
                        }
                    }
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PNG Image|*.png";
                saveFileDialog.Title = "Save Image";
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "")
                    bitmap.Save(saveFileDialog.FileName, ImageFormat.Png);
            }
        }

        private void ClearGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < GRID_SIZE; x++)
            {
                for (int y = 0; y < GRID_SIZE; y++)
                {
                    boxes[x, y] = 0;
                    Invalidate(rectangles[x, y]);
                }
            }
        }

        private void InitializeComponent()
        {
            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            saveImageToolStripMenuItem = new ToolStripMenuItem();
            clearGridToolStripMenuItem = new ToolStripMenuItem();

            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { clearGridToolStripMenuItem, saveImageToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";

            clearGridToolStripMenuItem.Name = "clearGridToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            this.SuspendLayout();

            // Create menu strip
            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            saveImageToolStripMenuItem = new ToolStripMenuItem();
            clearGridToolStripMenuItem = new ToolStripMenuItem();

            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { clearGridToolStripMenuItem, saveImageToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";

            saveImageToolStripMenuItem.Name = "saveImageToolStripMenuItem";
            saveImageToolStripMenuItem.Size = new Size(133, 22);
            saveImageToolStripMenuItem.Text = "Save Image";
            saveImageToolStripMenuItem.Click += SaveImageToolStripMenuItem_Click;

            clearGridToolStripMenuItem.Name = "clearGridToolStripMenuItem";
            clearGridToolStripMenuItem.Size = new Size(133, 22);
            clearGridToolStripMenuItem.Text = "Clear Grid";
            clearGridToolStripMenuItem.Click += ClearGridToolStripMenuItem_Click;

            Controls.Add(menuStrip);

            // Configure form properties
            ClientSize = new Size(GRID_SIZE * BOX_SIZE, GRID_SIZE * BOX_SIZE);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Grid Toggler";
            Paint += MainForm_Paint;
            MouseDown += MainForm_MouseDown;
            MouseUp += MainForm_MouseUp;
            MouseMove += MainForm_MouseMove;
            ResumeLayout(false);
            PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
