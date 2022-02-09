﻿using System;
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
        bool[,] universe = new bool[20, 20];
        bool[,] scratchPad = new bool[20, 20];

        // grid dimensions
        int gridWidth = 20;
        int gridHeight = 20;

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;

        // livng cells
        int livingCells = 0;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        #endregion

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
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < gridHeight; y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < gridWidth; x++)
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

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < gridHeight; y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < gridWidth; x++)
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
            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            // pause timer
            timer.Enabled = false;

            // iterate through universe y axis
            Array.Clear(universe, 0, universe.Length);

            // invalidate graphics
            graphicsPanel1.Invalidate();
        }
        #endregion

        #region Random Seed
        private void RandomSeedToolStripMenuItem_Click(object sender, EventArgs e)
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

        #region Reset Universe
        private void resetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // reset toggles to true
            toggleGridToolStripMenuItem.Checked = true;
            toggleHUDToolStripMenuItem.Checked = true;
            toggleNeighborCountToolStripMenuItem.Checked = true;

            // reset generations
            generations = 0;
            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();

            // pause and reset timer 
            timer.Enabled = false;
            timer.Interval = 100;

            // reset colors
            gridColor = Color.Black;
            cellColor = Color.Gray;
            graphicsPanel1.BackColor = Color.White;

            // reset universe to default 20 x 20 size
            ResizeUniverse(20, 20, ref universe, ref scratchPad);
            // iterate through universe y axis
            Array.Clear(universe, 0, universe.Length);

            // invalidate graphics
            graphicsPanel1.Invalidate();
        }
        #endregion

        #region Get Living cells
        private void GetLivingCells(ref bool[,] tempuniverse)
        {
            livingCells = 0;
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // iterate through universe x axis
                for (int x = 0; x < universe.GetLength(0); x++)
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
                writer.WriteLine("!Universe");

                // Iterate through the universe one row at a time.
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Iterate through the universe in the x, left to right
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        if (universe[x, y])
                        {
                            // appends an O for alive cells
                            writer.Write('O');
                        }
                        else
                        {
                            // appends a . for dead cells
                            writer.Write('.');
                        }
                    }
                    // goes to next row when complete with row
                    writer.Write('\n');
                }

                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }
        #endregion

    }
}
