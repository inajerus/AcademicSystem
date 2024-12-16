namespace AcademicSystem
{
    partial class LecturerDashboard
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
            this.cmbCourses = new System.Windows.Forms.ComboBox();
            this.dgvStudentsGrades = new System.Windows.Forms.DataGridView();
            this.btnSaveGrades = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudentsGrades)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Course";
            // 
            // cmbCourses
            // 
            this.cmbCourses.FormattingEnabled = true;
            this.cmbCourses.Location = new System.Drawing.Point(162, 63);
            this.cmbCourses.Name = "cmbCourses";
            this.cmbCourses.Size = new System.Drawing.Size(121, 24);
            this.cmbCourses.TabIndex = 1;
            // 
            // dgvStudentsGrades
            // 
            this.dgvStudentsGrades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStudentsGrades.Location = new System.Drawing.Point(307, 12);
            this.dgvStudentsGrades.Name = "dgvStudentsGrades";
            this.dgvStudentsGrades.RowHeadersWidth = 51;
            this.dgvStudentsGrades.RowTemplate.Height = 24;
            this.dgvStudentsGrades.Size = new System.Drawing.Size(240, 150);
            this.dgvStudentsGrades.TabIndex = 2;
            // 
            // btnSaveGrades
            // 
            this.btnSaveGrades.Location = new System.Drawing.Point(74, 102);
            this.btnSaveGrades.Name = "btnSaveGrades";
            this.btnSaveGrades.Size = new System.Drawing.Size(144, 23);
            this.btnSaveGrades.TabIndex = 3;
            this.btnSaveGrades.Text = "Save Grades";
            this.btnSaveGrades.UseVisualStyleBackColor = true;
            this.btnSaveGrades.Click += new System.EventHandler(this.btnSaveGrades_Click);
            // 
            // LecturerDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnSaveGrades);
            this.Controls.Add(this.dgvStudentsGrades);
            this.Controls.Add(this.cmbCourses);
            this.Controls.Add(this.label1);
            this.Name = "LecturerDashboard";
            this.Text = "LecturerDashboard";
            this.Load += new System.EventHandler(this.LecturerDashboard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudentsGrades)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbCourses;
        private System.Windows.Forms.DataGridView dgvStudentsGrades;
        private System.Windows.Forms.Button btnSaveGrades;
    }
}