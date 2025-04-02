namespace AdventureGame.AdventureGame.UI
{
    partial class GameForm
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
            this.components = new System.ComponentModel.Container();
            this.lblChapterName = new DevExpress.XtraEditors.LabelControl();
            this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
            this.lblStoryText = new DevExpress.XtraEditors.LabelControl();
            this.pnlChoices = new DevExpress.XtraEditors.PanelControl();
            this.btnNo = new DevExpress.XtraEditors.SimpleButton();
            this.btnYes = new DevExpress.XtraEditors.SimpleButton();
            this.btnInventory = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlChoices)).BeginInit();
            this.pnlChoices.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblChapterName
            // 
            this.lblChapterName.Appearance.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChapterName.Appearance.Options.UseFont = true;
            this.lblChapterName.Location = new System.Drawing.Point(117, 51);
            this.lblChapterName.Name = "lblChapterName";
            this.lblChapterName.Size = new System.Drawing.Size(194, 33);
            this.lblChapterName.TabIndex = 0;
            this.lblChapterName.Text = "Chapter Name";
            // 
            // lblStoryText
            // 
            this.lblStoryText.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStoryText.Appearance.Options.UseFont = true;
            this.lblStoryText.Location = new System.Drawing.Point(12, 143);
            this.lblStoryText.Name = "lblStoryText";
            this.lblStoryText.Size = new System.Drawing.Size(68, 19);
            this.lblStoryText.TabIndex = 7;
            this.lblStoryText.Text = "StoryText";
            // 
            // pnlChoices
            // 
            this.pnlChoices.Controls.Add(this.btnNo);
            this.pnlChoices.Controls.Add(this.btnYes);
            this.pnlChoices.Location = new System.Drawing.Point(12, 202);
            this.pnlChoices.Name = "pnlChoices";
            this.pnlChoices.Size = new System.Drawing.Size(412, 210);
            this.pnlChoices.TabIndex = 8;
            // 
            // btnNo
            // 
            this.btnNo.Appearance.BackColor = System.Drawing.Color.SlateGray;
            this.btnNo.Appearance.BorderColor = System.Drawing.Color.Black;
            this.btnNo.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo.Appearance.Options.UseBackColor = true;
            this.btnNo.Appearance.Options.UseBorderColor = true;
            this.btnNo.Appearance.Options.UseFont = true;
            this.btnNo.Location = new System.Drawing.Point(128, 24);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 31);
            this.btnNo.TabIndex = 1;
            this.btnNo.Text = "No";
            // 
            // btnYes
            // 
            this.btnYes.Appearance.BackColor = System.Drawing.Color.SlateGray;
            this.btnYes.Appearance.BorderColor = System.Drawing.Color.Black;
            this.btnYes.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYes.Appearance.Options.UseBackColor = true;
            this.btnYes.Appearance.Options.UseBorderColor = true;
            this.btnYes.Appearance.Options.UseFont = true;
            this.btnYes.Location = new System.Drawing.Point(16, 23);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 32);
            this.btnYes.TabIndex = 0;
            this.btnYes.Text = "yes";
            // 
            // btnInventory
            // 
            this.btnInventory.Location = new System.Drawing.Point(474, 389);
            this.btnInventory.Name = "btnInventory";
            this.btnInventory.Size = new System.Drawing.Size(75, 23);
            this.btnInventory.TabIndex = 9;
            this.btnInventory.Text = "Inventory";
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnInventory);
            this.Controls.Add(this.pnlChoices);
            this.Controls.Add(this.lblStoryText);
            this.Controls.Add(this.lblChapterName);
            this.Name = "GameForm";
            this.Text = "GameForm";
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlChoices)).EndInit();
            this.pnlChoices.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblChapterName;
        private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
        private DevExpress.XtraEditors.LabelControl lblStoryText;
        private DevExpress.XtraEditors.PanelControl pnlChoices;
        private DevExpress.XtraEditors.SimpleButton btnNo;
        private DevExpress.XtraEditors.SimpleButton btnYes;
        private DevExpress.XtraEditors.SimpleButton btnInventory;
    }
}