﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ortiz__Christian___GOL
{
    public partial class Form1 : Form
    {
        // The universe array
        bool[,] universe = new bool[20, 20];
        bool[,] scratchPad = new bool[20, 20];

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        #region Constructor
        public Form1()
        {
            InitializeComponent();

            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
        }
        #endregion

        #region NextGen
        // Calculate the next generation of cells
        private void NextGeneration()
        {
            // iterate through universe y axis
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // iterate through universe x axis
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // get neighbor count for each cell
                    int finiteCount = CountNeighborsToroidal(x, y);

                    // Applying rules to cells
                    // Underpopulation/Overpopulation, cell dies
                    if (universe[x, y] == true && finiteCount < 2 || finiteCount > 3)
                    {
                        scratchPad[x, y] = false;
                    }
                    // living cell lives into next generation
                    else if (universe[x, y] == true && finiteCount == 2 || finiteCount == 3)
                    {
                        scratchPad[x, y] = true;
                    }
                    // reproduction, dead cell comes back to live
                    else if (universe[x, y] == false && finiteCount == 3)
                    {
                        scratchPad[x, y] = true;
                    }

                }
            }

            // copy from scratchpad to the universe
            // Code for this in misc how to 
            bool[,] temp = universe;
            universe = scratchPad;
            scratchPad = temp;

            // clear out scratchpad
            for (int y = 0; y < scratchPad.GetLength(1); y++)
            {
                for (int x = 0; x < scratchPad.GetLength(0); x++)
                {
                    scratchPad[x, y] = false;
                }
            }

            // Increment generation count
            generations++;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();

            // Invalidate the graphics panel
            graphicsPanel1.Invalidate();
        }
        #endregion

        #region Timer Tick
        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
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

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // printing number of neighbors in the middle of cells
                    int neighbors = CountNeighborsFinite(x, y);
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

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Black, cellRect, stringFormat);

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

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }
        #endregion

        #region Exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Run/Pause/Next
        private void runtoolStripButton1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void pausetoolStripButton_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void NextToolStripButton_Click(object sender, EventArgs e)
        {
            NextGeneration();
        }

        #endregion

        #region File new
       

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            // reset generations
            generations = 0;
            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            // pause timer
            timer.Enabled = false;

            // iterate through universe y axis
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // iterate through universe x axis
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // reset universe
                    universe[x, y] = false;
                }
            }
            // invalidate graphics
            graphicsPanel1.Invalidate();
        }
        #endregion

        #region Random Seed
        private void randomSeedToolStripMenuItem_Click(object sender, EventArgs e)
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
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
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
            graphicsPanel1.Invalidate();
        }
        #endregion

        #region Color Dialog
        private void backColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = graphicsPanel1.BackColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                graphicsPanel1.BackColor = dlg.Color;
                graphicsPanel1.Invalidate();
            }

        }

        private void gridColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = gridColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                gridColor = dlg.Color;
                graphicsPanel1.Invalidate();
            }
        }

        private void cellColorToolStripMenuItem_Click(object sender, EventArgs e)
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

    }
}
