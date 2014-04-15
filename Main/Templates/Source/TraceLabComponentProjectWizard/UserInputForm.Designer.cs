﻿namespace TraceLabComponentProjectWizard
{
    partial class UserInputForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TBTraceLabSDK = new System.Windows.Forms.TextBox();
            this.TLSDKBtn = new System.Windows.Forms.Button();
            this.openTraceLabSDK = new System.Windows.Forms.OpenFileDialog();
            this.AcceptBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.TBTLTypes = new System.Windows.Forms.TextBox();
            this.TLTypesBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.TBOutputDirectory = new System.Windows.Forms.TextBox();
            this.outputDirectoryBtn = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(183, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Project Setup";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "TraceLabSDK\r\nLocation";
            // 
            // TBTraceLabSDK
            // 
            this.TBTraceLabSDK.Location = new System.Drawing.Point(138, 74);
            this.TBTraceLabSDK.Name = "TBTraceLabSDK";
            this.TBTraceLabSDK.Size = new System.Drawing.Size(258, 20);
            this.TBTraceLabSDK.TabIndex = 2;
            // 
            // TLSDKBtn
            // 
            this.TLSDKBtn.Location = new System.Drawing.Point(393, 72);
            this.TLSDKBtn.Name = "TLSDKBtn";
            this.TLSDKBtn.Size = new System.Drawing.Size(75, 23);
            this.TLSDKBtn.TabIndex = 3;
            this.TLSDKBtn.Text = "Browse...";
            this.TLSDKBtn.UseVisualStyleBackColor = true;
            this.TLSDKBtn.Click += new System.EventHandler(this.TLSDKBtn_Click);
            // 
            // openTraceLabSDK
            // 
            this.openTraceLabSDK.Filter = "dll (*.dll)|*.dll";
            // 
            // AcceptBtn
            // 
            this.AcceptBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.AcceptBtn.Location = new System.Drawing.Point(392, 249);
            this.AcceptBtn.Name = "AcceptBtn";
            this.AcceptBtn.Size = new System.Drawing.Size(88, 30);
            this.AcceptBtn.TabIndex = 4;
            this.AcceptBtn.Text = "Ok";
            this.AcceptBtn.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 26);
            this.label3.TabIndex = 5;
            this.label3.Text = "TraceLabSDK.Types\r\nLocation";
            // 
            // TBTLTypes
            // 
            this.TBTLTypes.Location = new System.Drawing.Point(138, 136);
            this.TBTLTypes.Name = "TBTLTypes";
            this.TBTLTypes.Size = new System.Drawing.Size(258, 20);
            this.TBTLTypes.TabIndex = 6;
            // 
            // TLTypesBtn
            // 
            this.TLTypesBtn.Location = new System.Drawing.Point(392, 134);
            this.TLTypesBtn.Name = "TLTypesBtn";
            this.TLTypesBtn.Size = new System.Drawing.Size(75, 23);
            this.TLTypesBtn.TabIndex = 7;
            this.TLTypesBtn.Text = "Browse...";
            this.TLTypesBtn.UseVisualStyleBackColor = true;
            this.TLTypesBtn.Click += new System.EventHandler(this.TLSDKTypesBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 194);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Output Directory";
            // 
            // TBOutputDirectory
            // 
            this.TBOutputDirectory.Location = new System.Drawing.Point(138, 194);
            this.TBOutputDirectory.Name = "TBOutputDirectory";
            this.TBOutputDirectory.Size = new System.Drawing.Size(258, 20);
            this.TBOutputDirectory.TabIndex = 9;
            // 
            // outputDirectoryBtn
            // 
            this.outputDirectoryBtn.Location = new System.Drawing.Point(392, 194);
            this.outputDirectoryBtn.Name = "outputDirectoryBtn";
            this.outputDirectoryBtn.Size = new System.Drawing.Size(75, 23);
            this.outputDirectoryBtn.TabIndex = 10;
            this.outputDirectoryBtn.Text = "Browse...";
            this.outputDirectoryBtn.UseVisualStyleBackColor = true;
            this.outputDirectoryBtn.Click += new System.EventHandler(this.OutputDirectoryBtn_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.Personal;
            // 
            // UserInputForm
            // 
            this.AcceptButton = this.AcceptBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 303);
            this.Controls.Add(this.outputDirectoryBtn);
            this.Controls.Add(this.TBOutputDirectory);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TLTypesBtn);
            this.Controls.Add(this.TBTLTypes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AcceptBtn);
            this.Controls.Add(this.TLSDKBtn);
            this.Controls.Add(this.TBTraceLabSDK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "UserInputForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBTraceLabSDK;
        private System.Windows.Forms.Button TLSDKBtn;
        private System.Windows.Forms.OpenFileDialog openTraceLabSDK;
        private System.Windows.Forms.Button AcceptBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TBTLTypes;
        private System.Windows.Forms.Button TLTypesBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TBOutputDirectory;
        private System.Windows.Forms.Button outputDirectoryBtn;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;

        public string TraceLabSDKPath
        {
            get { return TBTraceLabSDK.Text; }
        }
        public string TraceLabTypesPath
        {
            get { return TBTLTypes.Text; }
        }
        public string OutputDirectoryPath
        {
            get { return TBOutputDirectory.Text; }
        }
    }
}