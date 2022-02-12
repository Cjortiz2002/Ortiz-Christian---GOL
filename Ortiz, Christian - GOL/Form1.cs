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

namespace Ortiz__Christian___GOL
{
    public partial class Form1 : Form
    {
        #region Member Fields
        // The universe array
        bool[,] universe = new bool[Properties.Settings.Default.GridWidth, Properties.Settings.Default.GridHeight];
        bool[,] scratchPad = new bool[Properties.Settings.Default.GridWidth, Properties.Settings.Default.GridHeight];

        // grid dimensions
        int gridWidth = Properties.Settings.Default.GridWidth;
        int gridHeight = Properties.Settings.Default.GridHeight;

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;
        // livng cells
        int livingCells = 0;

        // Boundary Type
        string boundary = "";
        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        // Current Seed
        int Seed = 0;


        #endregion

        #region Constructor
        public Form1()
        {
            InitializeComponent();

            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            gridColor = Properties.Settings.Default.GridColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridHeight = Properties.Settings.Default.GridHeight;
            gridWidth = Properties.Settings.Default.GridWidth;

            // Setup the timer
            timer.Interval = Properties.Settings.Default.TimerInterval;
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
        }
        #endregion

        #region NextGen
        // Calculate the next generation of cells
        private void NextGeneration()
        {
            int neighborCount = 0;
            // iterate through universe y axis
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < gridHeight; y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < gridWidth; x++)
                {

                    // get neighbor count for each cell
                    if (toroidalToolStripMenuItem.Checked)
                    {
                        neighborCount = CountNeighborsToroidal(x, y);
                    }
                    else if (finiteToolStripMenuItem.Checked)
                    {
                        neighborCount = CountNeighborsFinite(x, y);
                    }
                    // Applying rules to cells
                    // Underpopulation/Overpopulation, cell dies
                    if (universe[x, y] == true && neighborCount < 2 || neighborCount > 3)
                    {
                        scratchPad[x, y] = false;
                    }
                    // living cell lives into next generation
                    else if (universe[x, y] == true && neighborCount == 2 || neighborCount == 3)
                    {
                        scratchPad[x, y] = true;
                    }
                    // reproduction, dead cell comes back to live
                    else if (universe[x, y] == false && neighborCount == 3)
                    {
                        scratchPad[x, y] = true;

                    }
                    else
                    {
                        scratchPad[x, y] = false;
                    }

                }
            }

            // copy from scratchpad to the universe
            // Code for this in misc how to 
            bool[,] temp = universe;
            universe = scratchPad;
            scratchPad = temp;

            // clear out scratchpad
            Array.Clear(scratchPad, 0, scratchPad.Length);

            // Increment generation count
            generations++;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            GetLivingCells(ref universe);


            // Invalidate the graphics panel
            graphicsPanel1.Invalidate();
        }
        #endregion

        #region Timer Tick
        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
            GetLivingCells(ref universe);
        }
        #endregion

        #region Count Neighbors

        #region Finite
        private int CountNeighborsFinite(int x, int y)
        {

            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then continue
                    // if yCheck is less than 0 then continue
                    else if (xCheck < 0)
                    {
                        continue;
                    }
                    else if (yCheck < 0)
                    {
                        continue;
                    }
                    // if xCheck is greater than or equal too xLen then continue
                    // if yCheck is greater than or equal too yLen then continue
                    else if (xCheck >= xLen)
                    {
                        continue;
                    }
                    else if (yCheck >= yLen)
                    {
                        continue;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }
        #endregion

        #region Toroidal
        private int CountNeighborsToroidal(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = (x + xOffset + xLen) % xLen;
                    int yCheck = (y + yOffset + yLen) % yLen;

                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then set to xLen - 1
                    else if (xCheck < 0)
                    {
                        xCheck = xLen - 1;
                    }
                    // if yCheck is less than 0 then set to yLen - 1
                    else if (yCheck < 0)
                    {
                        yCheck = yLen - 1;
                    }
                    // if xCheck is greater than or equal too xLen then set to 0
                    else if (xCheck >= xLen)
                    {
                        xCheck = 0;
                    }
                    // if yCheck is greater than or equal too yLen then set to 0
                    else if (yCheck >= yLen)
                    {
                        yCheck = 0;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }

        #endregion

        #endregion

        #region Paint/Mouse Click
        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {

            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = (float)graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            int neighbors = 0;
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < gridHeight; y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < gridWidth; x++)
                {
                    // printing number of neighbors in the middle of cells
                    if (toroidalToolStripMenuItem.Checked)
                    {
                        neighbors = CountNeighborsToroidal(x, y);
                        boundary = "Toroidal";
                    }
                    else if (finiteToolStripMenuItem.Checked)
                    {
                        neighbors = CountNeighborsFinite(x, y);
                        boundary = "Finite";
                    }
                    Font font = new Font("Arial", 10f);

                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    // A rectangle to represent each cell in pixels
                    RectangleF cellRect = RectangleF.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }
                    // if check for toggalable options
                    // Outline the cell with a pen
                    if (toggleGridToolStripMenuItem.Checked)
                    {
                        e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    }

                    if (neighbors > 0 && toggleNeighborCountToolStripMenuItem.Checked)
                    {
                        e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Black, cellRect, stringFormat);

                    }

                    if (toggleHUDToolStripMenuItem.Checked)
                    {
                        font = new Font("Arial", 13f);
                        string hud = "Generations: " + generations + "\nLiving Cell: " + livingCells + "\nBoundary Type: " + boundary + "\nUniverse Size: Width = " + gridWidth + " Height = " + gridHeight;
                        stringFormat.Alignment = StringAlignment.Near;
                        stringFormat.LineAlignment = StringAlignment.Near;
                        e.Graphics.DrawString(hud.ToString(), font, Brushes.BlueViolet, ClientRectangle, stringFormat);
                    }

                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                float cellWidth = (float)graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                float cellHeight = (float)graphicsPanel1.ClientSize.Height / universe.GetLength(1);
                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = (int)(e.X / cellWidth);
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = (int)(e.Y / cellHeight);

                // Toggle the cell's state
                universe[x, y] = !universe[x, y];
                GetLivingCells(ref universe);
                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }
        #endregion

        #region Exit / Save Settings
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // updates properties
            Properties.Settings.Default.BackColor = graphicsPanel1.BackColor;
            Properties.Settings.Default.GridColor = gridColor;
            Properties.Settings.Default.CellColor = cellColor;
            Properties.Settings.Default.GridHeight = gridHeight;
            Properties.Settings.Default.GridWidth = gridWidth;
            Properties.Settings.Default.TimerInterval = timer.Interval;
            Properties.Settings.Default.Save();
        }
        #endregion

        #region Run/Pause/Next
        private void RuntoolStripButton1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void PausetoolStripButton_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void NextToolStripButton_Click(object sender, EventArgs e)
        {
            NextGeneration();
        }

        #endregion

        #region File new
        private void NewToolStripButton_Click(object sender, EventArgs e)
        {
            // reset generations
            generations = 0;
            livingCells = 0;
            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            toolStripStatusLabelLivingCells.Text = "Living Cells = " + livingCells.ToString();
            // pause timer
            timer.Enabled = false;

            // iterate through universe y axis
            Array.Clear(universe, 0, universe.Length);

            // invalidate graphics
            graphicsPanel1.Invalidate();
        }
        #endregion

        #region Randomize

        #region Randomize from Time
        private void randomFromTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RandomizeTime();
            graphicsPanel1.Invalidate();
        }
        private void RandomizeTime()
        {
            // reset generations
            generations = 0;
            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            // pause timer
            timer.Enabled = false;
            // create instance of random
            Random rnd = new Random();
            // iterate through universe x and y axis
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    // create random number between 0 and 2
                    int randomNum = rnd.Next(0, 3);
                    if (randomNum == 0)
                    {
                        // if random number is 0 cell is alive (on)
                        universe[x, y] = true;
                    }
                    else
                    {
                        // if any other number cell is dead (off)
                        universe[x, y] = false;
                    }
                }
            }
            GetLivingCells(ref universe);
            graphicsPanel1.Invalidate();
        }
        #endregion

        #region Randomize from Seed
        private void RandomSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RandomDialog dlg = new RandomDialog();
            dlg.Seed = Seed;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                Seed = dlg.Seed;
                RandomizeSeed(dlg.Seed);
            }
            GetLivingCells(ref universe);
            graphicsPanel1.Invalidate();



        }
        private void RandomizeSeed(int seed)
        {

            Random rnd = new Random(seed);
            // iterate through universe x and y axis
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    // create random number between 0 and 2
                    int randomNum = rnd.Next(0, 3);
                    if (randomNum == 0)
                    {
                        // if random number is 0 cell is alive (on)
                        universe[x, y] = true;
                    }
                    else
                    {
                        // if any other number cell is dead (off)
                        universe[x, y] = false;
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Color Dialog
        private void BackColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = graphicsPanel1.BackColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                graphicsPanel1.BackColor = dlg.Color;
                graphicsPanel1.Invalidate();
            }

        }

        private void GridColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = gridColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                gridColor = dlg.Color;
                graphicsPanel1.Invalidate();
            }
        }

        private void CellColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = cellColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                cellColor = dlg.Color;
                graphicsPanel1.Invalidate();
            }
        }
        #endregion

        #region Options
        private void OptionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OptionsDialog dlg = new OptionsDialog();
            dlg.UniHeight = gridHeight;
            dlg.UniWidth = gridWidth;
            dlg.GenInterval = timer.Interval;
            if (DialogResult.OK == dlg.ShowDialog())
            {

                if (dlg.UniHeight != gridHeight || dlg.UniWidth != gridWidth)
                {
                    ResizeUniverse(dlg.UniHeight, dlg.UniWidth, ref universe, ref scratchPad);

                }
                timer.Interval = dlg.GenInterval;

            }
        }

        private void ResizeUniverse(int height, int width, ref bool[,] tempunivers, ref bool[,] tempScratchpad)
        {
            gridHeight = height;
            gridWidth = width;
            universe = new bool[gridWidth, gridHeight];
            scratchPad = new bool[gridWidth, gridHeight];
            graphicsPanel1.Invalidate();
        }
        #endregion

        #region Reset/Reload Universe
        private void resetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // reset toggles to true
            toggleGridToolStripMenuItem.Checked = true;
            toggleHUDToolStripMenuItem.Checked = true;
            toggleNeighborCountToolStripMenuItem.Checked = true;

            // reset generations and living cells
            generations = 0;
            livingCells = 0;
            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            toolStripStatusLabelLivingCells.Text = "Living Cells = " + livingCells.ToString();
            // pause and reset timer 
            timer.Enabled = false;
            timer.Interval = 100;

            // reset dimensions
            gridHeight = 20;
            gridWidth = 20;

            // reset colors
            gridColor = Color.Black;
            cellColor = Color.Gray;
            graphicsPanel1.BackColor = Color.White;

            // reset universe to default 20 x 20 size
            ResizeUniverse(gridHeight, gridWidth, ref universe, ref scratchPad);
            // clear universe
            Array.Clear(universe, 0, universe.Length);

            // invalidate graphics
            graphicsPanel1.Invalidate();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // reset toggles to true
            toggleGridToolStripMenuItem.Checked = true;
            toggleHUDToolStripMenuItem.Checked = true;
            toggleNeighborCountToolStripMenuItem.Checked = true;

            // reset generations and living cells
            generations = 0;
            livingCells = 0;
            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            toolStripStatusLabelLivingCells.Text = "Living Cells = " + livingCells.ToString();
            // pause timer
            timer.Enabled = false;

            // reload properties
            Properties.Settings.Default.Reload();
            timer.Interval = Properties.Settings.Default.TimerInterval;
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            gridColor = Properties.Settings.Default.GridColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridHeight = Properties.Settings.Default.GridHeight;
            gridWidth = Properties.Settings.Default.GridWidth;
            // resize universe to reloaded size
            ResizeUniverse(gridHeight, gridWidth, ref universe, ref scratchPad);
            // clear universe
            Array.Clear(universe, 0, universe.Length);
            graphicsPanel1.Invalidate();
        }


        #endregion

        #region Get Living cells
        private void GetLivingCells(ref bool[,] tempuniverse)
        {
            livingCells = 0;
            for (int y = 0; y < gridHeight; y++)
            {
                // iterate through universe x axis
                for (int x = 0; x < gridWidth; x++)
                {

                    if (universe[x, y] == true)
                    {
                        livingCells++;
                    }
                }
            }
            toolStripStatusLabelLivingCells.Text = "Living Cells = " + livingCells.ToString();
        }


        #endregion

        #region Save as Functionality
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);

                // Write any comments you want to include first.
                // Prefix all comment strings with an exclamation point.
                // Use WriteLine to write the strings to the file. 
                // It appends a CRLF for you.
                writer.WriteLine("!Your Universe");

                // Iterate through the universe one row at a time.
                for (int y = 0; y < gridHeight; y++)
                {
                    // Iterate through the universe in the x, left to right
                    string currentRow = string.Empty;
                    for (int x = 0; x < gridWidth; x++)
                    {
                        if (universe[x, y])
                        {
                            // appends an O for alive cells
                            currentRow += "O";
                        }
                        else
                        {
                            // appends a . for dead cells
                            currentRow += ".";
                        }
                    }
                    // writes current row to file
                    writer.WriteLine(currentRow);

                }

                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }
        #endregion

        #region Open File Functionality
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Create a couple variables to calculate the width and height
                // of the data in the file.
                int maxWidth = 0;
                int maxHeight = 0;
                int currentRow = 0;
                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then it is a comment
                    // and should be ignored.
                    if (row.StartsWith("!"))
                    {
                        continue;
                    }
                    else
                    {
                        maxHeight++;
                        // Get the length of the current row string
                        // and adjust the maxWidth variable if necessary.
                        maxWidth = row.Length;
                    }

                }
                // Resize the current universe and scratchPad
                // to the width and height of the file calculated above.
                ResizeUniverse(maxHeight, maxWidth, ref universe, ref scratchPad);

                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // ignores row if it begins with a '!'
                    if (row.StartsWith("!"))
                    {
                        continue;
                    }
                    else
                    {
                        // itterates through current row
                        for (int xPos = 0; xPos < row.Length; xPos++)
                        {
                            // sets current cell to alive if a O is found at row[xPos]
                            if (row[xPos] == 'O')
                            {
                                universe[xPos, currentRow] = true;
                            }
                            // sets current cell to dead if a period is found at row[xPos]
                            else if (row[xPos] == '.')
                            {
                                universe[xPos, currentRow] = false;
                            }
                        }
                        currentRow++;
                    }

                }

                // Close the file.
                reader.Close();
                GetLivingCells(ref universe);
            }
        }
        #endregion

        #region Toggle click events

        #region View Menu Toggles
        private void toggleGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toggleGridToolStripMenuItem.Checked)
            {
                ContextToggleGridToolStripMenuItem.Checked = true;
            }
            else
            {
                ContextToggleGridToolStripMenuItem.Checked = false;
            }
            graphicsPanel1.Invalidate();
        }

        private void toggleHUDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toggleHUDToolStripMenuItem.Checked)
            {
                ContextToggleHUDToolStripMenuItem.Checked = true;
            }
            else
            {
                ContextToggleHUDToolStripMenuItem.Checked = false;
            }
            graphicsPanel1.Invalidate();
        }

        private void toggleNeighborCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toggleNeighborCountToolStripMenuItem.Checked)
            {
                ContextToggleNeighborCountToolStripMenuItem.Checked = true;
            }
            else
            {
                ContextToggleNeighborCountToolStripMenuItem.Checked = false;
            }
            graphicsPanel1.Invalidate();
        }
        #endregion

        #region Context Menu Toggles
        private void ContextToggleGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ContextToggleGridToolStripMenuItem.Checked)
            {
                toggleGridToolStripMenuItem.Checked = true;
            }
            else
            {
                toggleGridToolStripMenuItem.Checked = false;
            }
            graphicsPanel1.Invalidate();
        }

        private void ContextToggleHUDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ContextToggleHUDToolStripMenuItem.Checked)
            {
                toggleHUDToolStripMenuItem.Checked = true;
            }
            else
            {
                toggleHUDToolStripMenuItem.Checked = false;
            }
            graphicsPanel1.Invalidate();
        }

        private void ContextToggleNeighborCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ContextToggleNeighborCountToolStripMenuItem.Checked)
            {
                toggleNeighborCountToolStripMenuItem.Checked = true;
            }
            else
            {
                toggleNeighborCountToolStripMenuItem.Checked = false;
            }
            graphicsPanel1.Invalidate();
        }

        #region boundary type click events
        private void toroidalToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // catch in case user trys to uncheck the current state they are in
            // this is to prevent count neighbor from breaking
            if (toroidalToolStripMenuItem.Checked == false && finiteToolStripMenuItem.Checked == false)
            {
                toroidalToolStripMenuItem.Checked = true;
            }
            else
            {
                finiteToolStripMenuItem.Checked = false;
                boundary = "Toroidal";
            }
            graphicsPanel1.Invalidate();
        }

        private void finiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // catch in case user trys to uncheck the current state they are in
            // this is to prevent count neighbor from breaking
            if (finiteToolStripMenuItem.Checked == false && toroidalToolStripMenuItem.Checked == false)
            {
                finiteToolStripMenuItem.Checked = true;
            }
            else
            {
                toroidalToolStripMenuItem.Checked = false;
                boundary = "Finite";

            }
            graphicsPanel1.Invalidate();
        }


        #endregion

        #endregion

    }
}
