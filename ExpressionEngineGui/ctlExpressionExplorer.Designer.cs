﻿namespace PropertyGui
{
    partial class ctlExpressionExplorer
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
            this.cboMode = new System.Windows.Forms.ComboBox();
            this.lblMode = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboPropertyTypeFilter = new System.Windows.Forms.ComboBox();
            this.cboEntityTypeFilter = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panFunction = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.cboCategory = new System.Windows.Forms.ComboBox();
            this.panGeneral = new System.Windows.Forms.Panel();
            this.treExplorer = new PropertyGui.ctlExpressionTree();
            this.mnuEnumValue = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuInsertValue = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInsertPropertyAndValue = new System.Windows.Forms.ToolStripMenuItem();
            this.panFunction.SuspendLayout();
            this.panGeneral.SuspendLayout();
            this.mnuEnumValue.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboMode
            // 
            this.cboMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMode.FormattingEnabled = true;
            this.cboMode.Location = new System.Drawing.Point(54, 7);
            this.cboMode.Name = "cboMode";
            this.cboMode.Size = new System.Drawing.Size(253, 21);
            this.cboMode.TabIndex = 9;
            this.cboMode.SelectedIndexChanged += new System.EventHandler(this.cbo_SelectedIndexChanged);
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Location = new System.Drawing.Point(3, 10);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(37, 13);
            this.lblMode.TabIndex = 8;
            this.lblMode.Text = "Mode:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Entity:";
            // 
            // cboPropertyTypeFilter
            // 
            this.cboPropertyTypeFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboPropertyTypeFilter.FormattingEnabled = true;
            this.cboPropertyTypeFilter.Location = new System.Drawing.Point(54, 30);
            this.cboPropertyTypeFilter.Name = "cboPropertyTypeFilter";
            this.cboPropertyTypeFilter.Size = new System.Drawing.Size(250, 21);
            this.cboPropertyTypeFilter.TabIndex = 7;
            this.cboPropertyTypeFilter.SelectedIndexChanged += new System.EventHandler(this.cbo_SelectedIndexChanged);
            // 
            // cboEntityTypeFilter
            // 
            this.cboEntityTypeFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboEntityTypeFilter.FormattingEnabled = true;
            this.cboEntityTypeFilter.Location = new System.Drawing.Point(54, 0);
            this.cboEntityTypeFilter.Name = "cboEntityTypeFilter";
            this.cboEntityTypeFilter.Size = new System.Drawing.Size(250, 21);
            this.cboEntityTypeFilter.TabIndex = 10;
            this.cboEntityTypeFilter.SelectedIndexChanged += new System.EventHandler(this.cbo_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Property:";
            // 
            // panFunction
            // 
            this.panFunction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panFunction.Controls.Add(this.label3);
            this.panFunction.Controls.Add(this.cboCategory);
            this.panFunction.Location = new System.Drawing.Point(6, 99);
            this.panFunction.Name = "panFunction";
            this.panFunction.Size = new System.Drawing.Size(304, 26);
            this.panFunction.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Category:";
            // 
            // cboCategory
            // 
            this.cboCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboCategory.FormattingEnabled = true;
            this.cboCategory.Location = new System.Drawing.Point(54, 0);
            this.cboCategory.Name = "cboCategory";
            this.cboCategory.Size = new System.Drawing.Size(250, 21);
            this.cboCategory.TabIndex = 13;
            this.cboCategory.SelectedIndexChanged += new System.EventHandler(this.cbo_SelectedIndexChanged);
            // 
            // panGeneral
            // 
            this.panGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panGeneral.Controls.Add(this.cboPropertyTypeFilter);
            this.panGeneral.Controls.Add(this.label1);
            this.panGeneral.Controls.Add(this.label2);
            this.panGeneral.Controls.Add(this.cboEntityTypeFilter);
            this.panGeneral.Location = new System.Drawing.Point(3, 32);
            this.panGeneral.Name = "panGeneral";
            this.panGeneral.Size = new System.Drawing.Size(307, 53);
            this.panGeneral.TabIndex = 13;
            // 
            // treExplorer
            // 
            this.treExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treExplorer.EntityTypeFilter = MetraTech.ExpressionEngine.Entity.EntityTypeEnum.ServiceDefinition;
            this.treExplorer.FunctionFilter = null;
            this.treExplorer.ImageIndex = 0;
            this.treExplorer.Location = new System.Drawing.Point(6, 86);
            this.treExplorer.Name = "treExplorer";
            this.treExplorer.PropertyTypeFilter = null;
            this.treExplorer.SelectedImageIndex = 0;
            this.treExplorer.ShowNodeToolTips = true;
            this.treExplorer.Size = new System.Drawing.Size(301, 283);
            this.treExplorer.TabIndex = 5;
            this.treExplorer.ViewMode = PropertyGui.ctlExpressionTree.ViewModeType.Properties;
            this.treExplorer.DoubleClick += new System.EventHandler(this.treExplorer_DoubleClick);
            // 
            // mnuEnumValue
            // 
            this.mnuEnumValue.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuInsertValue,
            this.mnuInsertPropertyAndValue});
            this.mnuEnumValue.Name = "mnuEnumValue";
            this.mnuEnumValue.Size = new System.Drawing.Size(207, 48);
            // 
            // mnuInsertValue
            // 
            this.mnuInsertValue.Name = "mnuInsertValue";
            this.mnuInsertValue.Size = new System.Drawing.Size(206, 22);
            this.mnuInsertValue.Text = "Insert Value";
            // 
            // mnuInsertPropertyAndValue
            // 
            this.mnuInsertPropertyAndValue.Name = "mnuInsertPropertyAndValue";
            this.mnuInsertPropertyAndValue.Size = new System.Drawing.Size(206, 22);
            this.mnuInsertPropertyAndValue.Text = "Insert Property and Value";
            // 
            // ctlExpressionExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panGeneral);
            this.Controls.Add(this.panFunction);
            this.Controls.Add(this.cboMode);
            this.Controls.Add(this.lblMode);
            this.Controls.Add(this.treExplorer);
            this.Name = "ctlExpressionExplorer";
            this.Size = new System.Drawing.Size(313, 372);
            this.panFunction.ResumeLayout(false);
            this.panFunction.PerformLayout();
            this.panGeneral.ResumeLayout(false);
            this.panGeneral.PerformLayout();
            this.mnuEnumValue.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboMode;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboPropertyTypeFilter;
        private ctlExpressionTree treExplorer;
        private System.Windows.Forms.ComboBox cboEntityTypeFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panFunction;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboCategory;
        private System.Windows.Forms.Panel panGeneral;
        private System.Windows.Forms.ContextMenuStrip mnuEnumValue;
        private System.Windows.Forms.ToolStripMenuItem mnuInsertValue;
        private System.Windows.Forms.ToolStripMenuItem mnuInsertPropertyAndValue;
    }
}
