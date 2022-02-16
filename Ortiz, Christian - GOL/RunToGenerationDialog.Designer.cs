
namespace Ortiz__Christian___GOL
{
    partial class RunToGenerationDialog
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
            this.IntervalLabel = new System.Windows.Forms.Label();
            this.RunToGenNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.CancelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.RunToGenNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // IntervalLabel
            // 
            this.IntervalLabel.AutoSize = true;
            this.IntervalLabel.Location = new System.Drawing.Point(4, 22);
            this.IntervalLabel.Name = "IntervalLabel";
            this.IntervalLabel.Size = new System.Drawing.Size(94, 13);
            this.IntervalLabel.TabIndex = 11;
            this.IntervalLabel.Text = "Generation to Run";
            // 
            // RunToGenNumericUpDown
            // 
            this.RunToGenNumericUpDown.Location = new System.Drawing.Point(114, 15);
            this.RunToGenNumericUpDown.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.RunToGenNumericUpDown.Name = "RunToGenNumericUpDown";
            this.RunToGenNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.RunToGenNumericUpDown.TabIndex = 10;
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(159, 85);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 9;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // OKButton
            // 
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(240, 85);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 8;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            // 
            // RunToGenerationDialog
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton;
            this.ClientSize = new System.Drawing.Size(325, 114);
            this.Controls.Add(this.IntervalLabel);
            this.Controls.Add(this.RunToGenNumericUpDown);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RunToGenerationDialog";
            this.Text = "RunToGenerationDialog";
            ((System.ComponentModel.ISupportInitialize)(this.RunToGenNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label IntervalLabel;
        private System.Windows.Forms.NumericUpDown RunToGenNumericUpDown;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OKButton;
    }
}