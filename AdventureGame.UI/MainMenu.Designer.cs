namespace AdventureGame
{
    partial class MainMenu
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
            this.lblGame = new DevExpress.XtraEditors.LabelControl();
            this.btnStart = new DevExpress.XtraEditors.SimpleButton();
            this.btnResume = new DevExpress.XtraEditors.SimpleButton();
            this.btnExit = new DevExpress.XtraEditors.SimpleButton();
            this.btnInfo = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // lblGame
            // 
            this.lblGame.Appearance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGame.Appearance.Options.UseFont = true;
            this.lblGame.Location = new System.Drawing.Point(176, 34);
            this.lblGame.Name = "lblGame";
            this.lblGame.Size = new System.Drawing.Size(433, 39);
            this.lblGame.TabIndex = 0;
            this.lblGame.Text = "Welcome to Rusty\'s Adventure";
            // 
            // btnStart
            // 
            this.btnStart.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Appearance.Options.UseFont = true;
            this.btnStart.Location = new System.Drawing.Point(310, 117);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(103, 39);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnResume
            // 
            this.btnResume.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResume.Appearance.Options.UseFont = true;
            this.btnResume.Location = new System.Drawing.Point(310, 185);
            this.btnResume.Name = "btnResume";
            this.btnResume.Size = new System.Drawing.Size(103, 39);
            this.btnResume.TabIndex = 2;
            this.btnResume.Text = "Resume";
            // 
            // btnExit
            // 
            this.btnExit.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Appearance.Options.UseFont = true;
            this.btnExit.Location = new System.Drawing.Point(310, 259);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(103, 39);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnInfo
            // 
            this.btnInfo.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInfo.Appearance.Options.UseFont = true;
            this.btnInfo.Location = new System.Drawing.Point(310, 333);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(103, 39);
            this.btnInfo.TabIndex = 4;
            this.btnInfo.Text = "Info";
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::AdventureGame.Properties.Resources.main_menu_pic;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnInfo);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnResume);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblGame);
            this.Name = "MainMenu";
            this.Text = "Main Menu";
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblGame;
        private DevExpress.XtraEditors.SimpleButton btnStart;
        private DevExpress.XtraEditors.SimpleButton btnResume;
        private DevExpress.XtraEditors.SimpleButton btnExit;
        private DevExpress.XtraEditors.SimpleButton btnInfo;
    }
}

