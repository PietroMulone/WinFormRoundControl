using System.Drawing;
using System.IO;

namespace RoundComboboxTest
{
    partial class RoundCombobox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.TimerItem = new System.Windows.Forms.Timer(this.components);
            this.PnlItem = new RoundComboboxTest.RoundPanel();
            this.FLPItem = new System.Windows.Forms.FlowLayoutPanel();
            this.PnlTitle = new RoundComboboxTest.RoundPanel();
            this.txtCombobox = new System.Windows.Forms.TextBox();
            this.PicCombobox = new System.Windows.Forms.PictureBox();
            this.PnlItem.SuspendLayout();
            this.PnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicCombobox)).BeginInit();
            this.SuspendLayout();
            // 
            // TimerItem
            // 
            this.TimerItem.Interval = 1;
            this.TimerItem.Tick += new System.EventHandler(this.TimerItem_Tick);
            // 
            // PnlItem
            // 
            this.PnlItem.BackColor = System.Drawing.Color.White;
            this.PnlItem.BorderColor = System.Drawing.Color.Black;
            this.PnlItem.BorderRadius = 0;
            this.PnlItem.BorderSize = 0;
            this.PnlItem.Controls.Add(this.FLPItem);
            this.PnlItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlItem.ForeColor = System.Drawing.Color.Black;
            this.PnlItem.GradientAngle = 270F;
            this.PnlItem.GradientBottomColor = System.Drawing.Color.Transparent;
            this.PnlItem.GradientTopColor = System.Drawing.Color.Transparent;
            this.PnlItem.Location = new System.Drawing.Point(0, 35);
            this.PnlItem.Margin = new System.Windows.Forms.Padding(0);
            this.PnlItem.Name = "PnlItem";
            this.PnlItem.Padding = new System.Windows.Forms.Padding(8);
            this.PnlItem.Size = new System.Drawing.Size(300, 0);
            this.PnlItem.TabIndex = 1;
            // 
            // FLPItem
            // 
            this.FLPItem.AutoScroll = true;
            this.FLPItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FLPItem.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.FLPItem.Location = new System.Drawing.Point(8, 8);
            this.FLPItem.Margin = new System.Windows.Forms.Padding(10, 10, 10, 0);
            this.FLPItem.Name = "FLPItem";
            this.FLPItem.Size = new System.Drawing.Size(284, 0);
            this.FLPItem.TabIndex = 0;
            this.FLPItem.WrapContents = false;
            // 
            // PnlTitle
            // 
            this.PnlTitle.BackColor = System.Drawing.Color.White;
            this.PnlTitle.BorderColor = System.Drawing.Color.Black;
            this.PnlTitle.BorderRadius = 0;
            this.PnlTitle.BorderSize = 0;
            this.PnlTitle.Controls.Add(this.txtCombobox);
            this.PnlTitle.Controls.Add(this.PicCombobox);
            this.PnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.PnlTitle.ForeColor = System.Drawing.Color.Black;
            this.PnlTitle.GradientAngle = 270F;
            this.PnlTitle.GradientBottomColor = System.Drawing.Color.Transparent;
            this.PnlTitle.GradientTopColor = System.Drawing.Color.Transparent;
            this.PnlTitle.Location = new System.Drawing.Point(0, 0);
            this.PnlTitle.Margin = new System.Windows.Forms.Padding(0);
            this.PnlTitle.Name = "PnlTitle";
            this.PnlTitle.Size = new System.Drawing.Size(300, 35);
            this.PnlTitle.TabIndex = 0;
            // 
            // txtCombobox
            // 
            this.txtCombobox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCombobox.BackColor = System.Drawing.Color.White;
            this.txtCombobox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCombobox.Location = new System.Drawing.Point(14, 6);
            this.txtCombobox.Margin = new System.Windows.Forms.Padding(0);
            this.txtCombobox.Name = "txtCombobox";
            this.txtCombobox.Size = new System.Drawing.Size(237, 23);
            this.txtCombobox.TabIndex = 2;
            this.txtCombobox.Text = "Choose An Option";
            this.txtCombobox.Click += new System.EventHandler(this.txtCombobox_Click);
            this.txtCombobox.TextChanged += new System.EventHandler(this.txtCombobox_TextChanged);
            // 
            // PicCombobox
            // 
            this.PicCombobox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PicCombobox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PicCombobox.Location = new System.Drawing.Point(259, -1);
            this.PicCombobox.Name = "PicCombobox";
            this.PicCombobox.Size = new System.Drawing.Size(30, 30);
            this.PicCombobox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicCombobox.TabIndex = 0;
            this.PicCombobox.TabStop = false;
            this.PicCombobox.Click += new System.EventHandler(this.PicCombobox_Click);
            // 
            // RoundCombobox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.PnlItem);
            this.Controls.Add(this.PnlTitle);
            this.Font = new System.Drawing.Font("Century Gothic", 11.25F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "RoundCombobox";
            this.Size = new System.Drawing.Size(300, 35);
            this.Load += new System.EventHandler(this.RoundCombobox_Load);
            this.FontChanged += new System.EventHandler(this.RoundCombobox_FontChanged);
            this.PnlItem.ResumeLayout(false);
            this.PnlTitle.ResumeLayout(false);
            this.PnlTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicCombobox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private RoundPanel PnlTitle;
        private RoundPanel PnlItem;
        private System.Windows.Forms.FlowLayoutPanel FLPItem;
        private System.Windows.Forms.PictureBox PicCombobox;
        private System.Windows.Forms.Timer TimerItem;
        private System.Windows.Forms.TextBox txtCombobox;
    }
}
