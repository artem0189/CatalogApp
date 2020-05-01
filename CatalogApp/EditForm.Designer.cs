namespace CatalogApp
{
    partial class EditForm
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
            this.formPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.updateButton = new System.Windows.Forms.Button();
            this.createButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.allObjectsComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // formPanel
            // 
            this.formPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.formPanel.Location = new System.Drawing.Point(25, 53);
            this.formPanel.Name = "formPanel";
            this.formPanel.Size = new System.Drawing.Size(175, 473);
            this.formPanel.TabIndex = 0;
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(25, 564);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(84, 25);
            this.updateButton.TabIndex = 1;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(25, 532);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(175, 25);
            this.createButton.TabIndex = 2;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(116, 564);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(84, 25);
            this.deleteButton.TabIndex = 3;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Все объекты:";
            // 
            // allObjectsComboBox
            // 
            this.allObjectsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.allObjectsComboBox.FormattingEnabled = true;
            this.allObjectsComboBox.Location = new System.Drawing.Point(25, 29);
            this.allObjectsComboBox.Name = "allObjectsComboBox";
            this.allObjectsComboBox.Size = new System.Drawing.Size(175, 24);
            this.allObjectsComboBox.TabIndex = 5;
            // 
            // EditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 601);
            this.Controls.Add(this.allObjectsComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.formPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditForm";
            this.Text = "EditForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EditForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel formPanel;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox allObjectsComboBox;
    }
}