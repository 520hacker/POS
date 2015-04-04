namespace POS
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.telerikMetroTouchTheme1 = new Telerik.WinControls.Themes.TelerikMetroTouchTheme();
            this.radPageView1 = new Telerik.WinControls.UI.RadPageView();
            this.HomePage = new Telerik.WinControls.UI.RadPageViewPage();
            this.ProductsPage = new Telerik.WinControls.UI.RadPageViewPage();
            this.SettingsPage = new Telerik.WinControls.UI.RadPageViewPage();
            this.radDesktopAlert1 = new Telerik.WinControls.UI.RadDesktopAlert(this.components);
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).BeginInit();
            this.radPageView1.SuspendLayout();
            this.HomePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPageView1
            // 
            this.radPageView1.Controls.Add(this.HomePage);
            this.radPageView1.Controls.Add(this.ProductsPage);
            this.radPageView1.Controls.Add(this.SettingsPage);
            this.radPageView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPageView1.Location = new System.Drawing.Point(0, 0);
            this.radPageView1.Name = "radPageView1";
            this.radPageView1.SelectedPage = this.HomePage;
            this.radPageView1.Size = new System.Drawing.Size(926, 409);
            this.radPageView1.TabIndex = 0;
            this.radPageView1.Text = "radPageView1";
            this.radPageView1.ThemeName = "TelerikMetroTouch";
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.radPageView1.GetChildAt(0))).StripButtons = Telerik.WinControls.UI.StripViewButtons.None;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.radPageView1.GetChildAt(0))).ItemAlignment = Telerik.WinControls.UI.StripViewItemAlignment.Center;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.radPageView1.GetChildAt(0))).ItemFitMode = Telerik.WinControls.UI.StripViewItemFitMode.Fill;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.radPageView1.GetChildAt(0))).StripAlignment = Telerik.WinControls.UI.StripViewAlignment.Top;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.radPageView1.GetChildAt(0))).ItemDragMode = Telerik.WinControls.UI.PageViewItemDragMode.Preview;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.radPageView1.GetChildAt(0))).ItemSizeMode = Telerik.WinControls.UI.PageViewItemSizeMode.EqualWidth;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.radPageView1.GetChildAt(0))).ItemContentOrientation = Telerik.WinControls.UI.PageViewContentOrientation.Auto;
            // 
            // HomePage
            // 
            this.HomePage.Controls.Add(this.button1);
            this.HomePage.Image = global::POS.Properties.Resources.home;
            this.HomePage.Location = new System.Drawing.Point(5, 46);
            this.HomePage.Name = "HomePage";
            this.HomePage.Size = new System.Drawing.Size(916, 358);
            this.HomePage.Text = "Home";
            this.HomePage.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ProductsPage
            // 
            this.ProductsPage.Image = global::POS.Properties.Resources.box;
            this.ProductsPage.Location = new System.Drawing.Point(5, 46);
            this.ProductsPage.Name = "ProductsPage";
            this.ProductsPage.Size = new System.Drawing.Size(916, 358);
            this.ProductsPage.Text = "Produkte";
            this.ProductsPage.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SettingsPage
            // 
            this.SettingsPage.Image = global::POS.Properties.Resources.application_x_desktop;
            this.SettingsPage.Location = new System.Drawing.Point(5, 46);
            this.SettingsPage.Name = "SettingsPage";
            this.SettingsPage.Size = new System.Drawing.Size(916, 358);
            this.SettingsPage.Text = "Einstellungen";
            this.SettingsPage.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radDesktopAlert1
            // 
            this.radDesktopAlert1.ShowOptionsButton = false;
            this.radDesktopAlert1.ShowPinButton = false;
            this.radDesktopAlert1.ThemeName = "TelerikMetroTouch";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(137, 37);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 45);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 409);
            this.Controls.Add(this.radPageView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IconScaling = Telerik.WinControls.Enumerations.ImageScaling.None;
            this.Name = "MainForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Furesoft POS";
            this.ThemeName = "TelerikMetroTouch";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).EndInit();
            this.radPageView1.ResumeLayout(false);
            this.HomePage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.Themes.TelerikMetroTouchTheme telerikMetroTouchTheme1;
        private Telerik.WinControls.UI.RadPageView radPageView1;
        private Telerik.WinControls.UI.RadPageViewPage HomePage;
        private Telerik.WinControls.UI.RadDesktopAlert radDesktopAlert1;
        private Telerik.WinControls.UI.RadPageViewPage ProductsPage;
        private Telerik.WinControls.UI.RadPageViewPage SettingsPage;
        private System.Windows.Forms.Button button1;
    }
}
