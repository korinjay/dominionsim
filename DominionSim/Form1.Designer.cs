namespace DominionSim
{
    partial class Form1
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
            this.playerCombo0 = new System.Windows.Forms.ComboBox();
            this.playButton = new System.Windows.Forms.Button();
            this.playerCombo1 = new System.Windows.Forms.ComboBox();
            this.playerCombo2 = new System.Windows.Forms.ComboBox();
            this.playerCombo3 = new System.Windows.Forms.ComboBox();
            this.playerCombo4 = new System.Windows.Forms.ComboBox();
            this.playerCombo5 = new System.Windows.Forms.ComboBox();
            this.outputBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // playerCombo0
            // 
            this.playerCombo0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playerCombo0.FormattingEnabled = true;
            this.playerCombo0.Location = new System.Drawing.Point(12, 12);
            this.playerCombo0.Name = "playerCombo0";
            this.playerCombo0.Size = new System.Drawing.Size(216, 21);
            this.playerCombo0.TabIndex = 0;
            // 
            // playButton
            // 
            this.playButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playButton.Location = new System.Drawing.Point(12, 93);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(438, 52);
            this.playButton.TabIndex = 6;
            this.playButton.Text = "Play!";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // playerCombo1
            // 
            this.playerCombo1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playerCombo1.FormattingEnabled = true;
            this.playerCombo1.Location = new System.Drawing.Point(234, 12);
            this.playerCombo1.Name = "playerCombo1";
            this.playerCombo1.Size = new System.Drawing.Size(216, 21);
            this.playerCombo1.TabIndex = 1;
            // 
            // playerCombo2
            // 
            this.playerCombo2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playerCombo2.FormattingEnabled = true;
            this.playerCombo2.Location = new System.Drawing.Point(12, 39);
            this.playerCombo2.Name = "playerCombo2";
            this.playerCombo2.Size = new System.Drawing.Size(216, 21);
            this.playerCombo2.TabIndex = 2;
            // 
            // playerCombo3
            // 
            this.playerCombo3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playerCombo3.FormattingEnabled = true;
            this.playerCombo3.Location = new System.Drawing.Point(234, 39);
            this.playerCombo3.Name = "playerCombo3";
            this.playerCombo3.Size = new System.Drawing.Size(216, 21);
            this.playerCombo3.TabIndex = 3;
            // 
            // playerCombo4
            // 
            this.playerCombo4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playerCombo4.FormattingEnabled = true;
            this.playerCombo4.Location = new System.Drawing.Point(12, 66);
            this.playerCombo4.Name = "playerCombo4";
            this.playerCombo4.Size = new System.Drawing.Size(216, 21);
            this.playerCombo4.TabIndex = 4;
            // 
            // playerCombo5
            // 
            this.playerCombo5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playerCombo5.FormattingEnabled = true;
            this.playerCombo5.Location = new System.Drawing.Point(234, 66);
            this.playerCombo5.Name = "playerCombo5";
            this.playerCombo5.Size = new System.Drawing.Size(216, 21);
            this.playerCombo5.TabIndex = 5;
            // 
            // outputBox
            // 
            this.outputBox.AcceptsReturn = true;
            this.outputBox.Location = new System.Drawing.Point(13, 152);
            this.outputBox.Multiline = true;
            this.outputBox.Name = "outputBox";
            this.outputBox.ReadOnly = true;
            this.outputBox.Size = new System.Drawing.Size(437, 212);
            this.outputBox.TabIndex = 7;
            this.outputBox.WordWrap = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 376);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.playerCombo1);
            this.Controls.Add(this.playerCombo5);
            this.Controls.Add(this.playerCombo4);
            this.Controls.Add(this.playerCombo3);
            this.Controls.Add(this.playerCombo2);
            this.Controls.Add(this.playerCombo0);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox playerCombo0;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.ComboBox playerCombo1;
        private System.Windows.Forms.ComboBox playerCombo2;
        private System.Windows.Forms.ComboBox playerCombo3;
        private System.Windows.Forms.ComboBox playerCombo4;
        private System.Windows.Forms.ComboBox playerCombo5;
        private System.Windows.Forms.TextBox outputBox;
    }
}

