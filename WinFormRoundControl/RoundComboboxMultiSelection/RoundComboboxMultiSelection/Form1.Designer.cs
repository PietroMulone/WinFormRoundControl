using RoundComboboxMultiSelect;
namespace MyAPP
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
            this.button_clear = new System.Windows.Forms.Button();
            this.button_add = new System.Windows.Forms.Button();
            this.Selected_button = new System.Windows.Forms.Button();
            this.roundComboboxMultiSelect1 = new RoundComboboxMultiSelect.ComboboxMultiSelect();
            this.SuspendLayout();
            // 
            // button_clear
            // 
            this.button_clear.Location = new System.Drawing.Point(170, 284);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(94, 58);
            this.button_clear.TabIndex = 8;
            this.button_clear.Text = "Clear";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // button_add
            // 
            this.button_add.Location = new System.Drawing.Point(311, 284);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(94, 58);
            this.button_add.TabIndex = 7;
            this.button_add.Text = "add";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // Selected_button
            // 
            this.Selected_button.Location = new System.Drawing.Point(452, 284);
            this.Selected_button.Name = "Selected_button";
            this.Selected_button.Size = new System.Drawing.Size(75, 61);
            this.Selected_button.TabIndex = 10;
            this.Selected_button.Text = "selected";
            this.Selected_button.UseVisualStyleBackColor = true;
            this.Selected_button.Click += new System.EventHandler(this.Selected_button_Click);
            // 
            // roundComboboxMultiSelect1
            // 
            this.roundComboboxMultiSelect1.BackColor = System.Drawing.Color.Transparent;
            this.roundComboboxMultiSelect1.BorderColor = System.Drawing.Color.Black;
            this.roundComboboxMultiSelect1.BorderSize = 0;
            this.roundComboboxMultiSelect1.DataSource = new string[0];
            this.roundComboboxMultiSelect1.Font = new System.Drawing.Font("Century Gothic", 11.25F);
            this.roundComboboxMultiSelect1.IconImage = global::RoundComboboxMultiSelection.Properties.Resources.NewFrecciaGiu;
            this.roundComboboxMultiSelect1.LabelFont = new System.Drawing.Font("Century Gothic", 11.25F);
            this.roundComboboxMultiSelect1.LabelOnHoverColor = System.Drawing.Color.Gainsboro;
            this.roundComboboxMultiSelect1.LabelSelectedColor = System.Drawing.Color.Silver;
            this.roundComboboxMultiSelect1.Location = new System.Drawing.Point(43, 32);
            this.roundComboboxMultiSelect1.Margin = new System.Windows.Forms.Padding(0);
            this.roundComboboxMultiSelect1.Name = "roundComboboxMultiSelect1";
            this.roundComboboxMultiSelect1.PanelsBackgroundColor = System.Drawing.Color.White;
            this.roundComboboxMultiSelect1.Radius = 10;
            this.roundComboboxMultiSelect1.Size = new System.Drawing.Size(393, 35);
            this.roundComboboxMultiSelect1.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(600, 366);
            this.Controls.Add(this.roundComboboxMultiSelect1);
            this.Controls.Add(this.Selected_button);
            this.Controls.Add(this.button_clear);
            this.Controls.Add(this.button_add);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.Button Selected_button;
        private RoundComboboxMultiSelect.ComboboxMultiSelect roundComboboxMultiSelect1;
    }
}

