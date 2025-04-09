namespace AdventureGame.AdventureGame.UI
{
    partial class InventoryForm
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
            this.lstInventory = new DevExpress.XtraEditors.ListBoxControl();
            this.btnDrop = new DevExpress.XtraEditors.SimpleButton();
            this.btnUse = new DevExpress.XtraEditors.SimpleButton();
            this.lblInventory = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.lstInventory)).BeginInit();
            this.SuspendLayout();
            // 
            // lstInventory
            // 
            this.lstInventory.Location = new System.Drawing.Point(108, 113);
            this.lstInventory.Name = "lstInventory";
            this.lstInventory.Size = new System.Drawing.Size(291, 184);
            this.lstInventory.TabIndex = 0;
            // 
            // btnDrop
            // 
            this.btnDrop.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDrop.Appearance.Options.UseFont = true;
            this.btnDrop.Location = new System.Drawing.Point(160, 320);
            this.btnDrop.Name = "btnDrop";
            this.btnDrop.Size = new System.Drawing.Size(77, 35);
            this.btnDrop.TabIndex = 1;
            this.btnDrop.Text = "Drop";
            this.btnDrop.Click += new System.EventHandler(this.btnDrop_Click);
            // 
            // btnUse
            // 
            this.btnUse.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUse.Appearance.Options.UseFont = true;
            this.btnUse.Location = new System.Drawing.Point(265, 318);
            this.btnUse.Name = "btnUse";
            this.btnUse.Size = new System.Drawing.Size(77, 37);
            this.btnUse.TabIndex = 2;
            this.btnUse.Text = "Use";
            // 
            // lblInventory
            // 
            this.lblInventory.Appearance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInventory.Appearance.Options.UseFont = true;
            this.lblInventory.Location = new System.Drawing.Point(216, 44);
            this.lblInventory.Name = "lblInventory";
            this.lblInventory.Size = new System.Drawing.Size(102, 29);
            this.lblInventory.TabIndex = 4;
            this.lblInventory.Text = "Inventory";
            // 
            // InventoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 479);
            this.Controls.Add(this.lblInventory);
            this.Controls.Add(this.btnUse);
            this.Controls.Add(this.btnDrop);
            this.Controls.Add(this.lstInventory);
            this.Name = "InventoryForm";
            this.Text = "InventoryForm";
            this.Load += new System.EventHandler(this.InventoryForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lstInventory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ListBoxControl lstInventory;
        private DevExpress.XtraEditors.SimpleButton btnDrop;
        private DevExpress.XtraEditors.SimpleButton btnUse;
        private DevExpress.XtraEditors.LabelControl lblInventory;
    }
}