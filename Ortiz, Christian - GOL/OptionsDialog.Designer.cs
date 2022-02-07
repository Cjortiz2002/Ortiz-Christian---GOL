
namespace Ortiz__Christian___GOL
{
    partial class OptionsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.NumericUpDownHeight = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownWidth = new System.Windows.Forms.NumericUpDown();
            this.GenerationInterval = new System.Windows.Forms.NumericUpDown();
            this.UniverseHeight = new System.Windows.Forms.Label();
            this.UniverseWidth = new System.Windows.Forms.Label();
            this.IntervalLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GenerationInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(288, 123);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(207, 123);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // GridHeight
            // 
            this.NumericUpDownHeight.Location = new System.Drawing.Point(162, 0);
            this.NumericUpDownHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumericUpDownHeight.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.NumericUpDownHeight.Name = "GridHeight";
            this.NumericUpDownHeight.Size = new System.Drawing.Size(120, 20);
            this.NumericUpDownHeight.TabIndex = 2;
            this.NumericUpDownHeight.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // GridWidth
            // 
            this.NumericUpDownWidth.Location = new System.Drawing.Point(162, 26);
            this.NumericUpDownWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumericUpDownWidth.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.NumericUpDownWidth.Name = "GridWidth";
            this.NumericUpDownWidth.Size = new System.Drawing.Size(120, 20);
            this.NumericUpDownWidth.TabIndex = 3;
            this.NumericUpDownWidth.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // GenerationInterval
            // 
            this.GenerationInterval.Location = new System.Drawing.Point(162, 51);
            this.GenerationInterval.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.GenerationInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.GenerationInterval.Name = "GenerationInterval";
            this.GenerationInterval.Size = new System.Drawing.Size(120, 20);
            this.GenerationInterval.TabIndex = 4;
            this.GenerationInterval.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // UniverseHeight
            // 
            this.UniverseHeight.AutoSize = true;
            this.UniverseHeight.Location = new System.Drawing.Point(12, 7);
            this.UniverseHeight.Name = "UniverseHeight";
            this.UniverseHeight.Size = new System.Drawing.Size(95, 13);
            this.UniverseHeight.TabIndex = 5;
            this.UniverseHeight.Text = "Height of Universe";
            // 
            // UniverseWidth
            // 
            this.UniverseWidth.AutoSize = true;
            this.UniverseWidth.Location = new System.Drawing.Point(12, 33);
            this.UniverseWidth.Name = "UniverseWidth";
            this.UniverseWidth.Size = new System.Drawing.Size(92, 13);
            this.UniverseWidth.TabIndex = 6;
            this.UniverseWidth.Text = "Width of Universe";
            // 
            // IntervalLabel
            // 
            this.IntervalLabel.AutoSize = true;
            this.IntervalLabel.Location = new System.Drawing.Point(12, 58);
            this.IntervalLabel.Name = "IntervalLabel";
            this.IntervalLabel.Size = new System.Drawing.Size(141, 13);
            this.IntervalLabel.TabIndex = 7;
            this.IntervalLabel.Text = "Timer Interval in milliseconds";
            // 
            // OptionsDialog
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 150);
            this.Controls.Add(this.IntervalLabel);
            this.Controls.Add(this.UniverseWidth);
            this.Controls.Add(this.UniverseHeight);
            this.Controls.Add(this.GenerationInterval);
            this.Controls.Add(this.NumericUpDownWidth);
            this.Controls.Add(this.NumericUpDownHeight);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsDialog";
            this.Text = "Options Dialog";
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GenerationInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.NumericUpDown NumericUpDownHeight;
        private System.Windows.Forms.NumericUpDown NumericUpDownWidth;
        private System.Windows.Forms.NumericUpDown GenerationInterval;
        private System.Windows.Forms.Label UniverseHeight;
        private System.Windows.Forms.Label UniverseWidth;
        private System.Windows.Forms.Label IntervalLabel;
    }
}