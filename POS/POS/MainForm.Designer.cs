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
            this.radPageViewItemPage1 = new Telerik.WinControls.UI.RadPageViewItemPage();
            this.BonDesignerPage = new Telerik.WinControls.UI.RadPageViewPage();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.propertyGrid = new Telerik.WinControls.UI.RadPropertyGrid();
            this.AddImageBtn = new Telerik.WinControls.UI.RadCommandBar();
            this.commandBarRowElement1 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.commandBarStripElement1 = new Telerik.WinControls.UI.CommandBarStripElement();
            this.ImageBtn = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator1 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.PropertiesBtn = new Telerik.WinControls.UI.CommandBarButton();
            this.radDesktopAlert1 = new Telerik.WinControls.UI.RadDesktopAlert(this.components);
            this.ProductsView = new Telerik.WinControls.UI.RadPanorama();
            this.tileGroupElement1 = new Telerik.WinControls.UI.TileGroupElement();
            this.radTileElement1 = new Telerik.WinControls.UI.RadTileElement();
            this.radTileElement2 = new Telerik.WinControls.UI.RadTileElement();
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).BeginInit();
            this.radPageView1.SuspendLayout();
            this.HomePage.SuspendLayout();
            this.BonDesignerPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddImageBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductsView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPageView1
            // 
            this.radPageView1.Controls.Add(this.HomePage);
            this.radPageView1.Controls.Add(this.ProductsPage);
            this.radPageView1.Controls.Add(this.SettingsPage);
            this.radPageView1.Controls.Add(this.radPageViewItemPage1);
            this.radPageView1.Controls.Add(this.BonDesignerPage);
            this.radPageView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPageView1.Location = new System.Drawing.Point(0, 0);
            this.radPageView1.Name = "radPageView1";
            this.radPageView1.SelectedPage = this.HomePage;
            this.radPageView1.Size = new System.Drawing.Size(926, 409);
            this.radPageView1.TabIndex = 0;
            this.radPageView1.Text = "radPageView1";
            this.radPageView1.ThemeName = "TelerikMetroTouch";
            this.radPageView1.ViewMode = Telerik.WinControls.UI.PageViewMode.Backstage;
            // 
            // HomePage
            // 
            this.HomePage.Controls.Add(this.ProductsView);
            this.HomePage.Image = global::POS.Properties.Resources.home;
            this.HomePage.Location = new System.Drawing.Point(205, 4);
            this.HomePage.Name = "HomePage";
            this.HomePage.Size = new System.Drawing.Size(717, 401);
            this.HomePage.Text = "Home";
            this.HomePage.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ProductsPage
            // 
            this.ProductsPage.Image = global::POS.Properties.Resources.box;
            this.ProductsPage.Location = new System.Drawing.Point(205, 4);
            this.ProductsPage.Name = "ProductsPage";
            this.ProductsPage.Size = new System.Drawing.Size(717, 401);
            this.ProductsPage.Text = "Produkte";
            this.ProductsPage.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SettingsPage
            // 
            this.SettingsPage.Image = global::POS.Properties.Resources.application_x_desktop;
            this.SettingsPage.Location = new System.Drawing.Point(205, 4);
            this.SettingsPage.Name = "SettingsPage";
            this.SettingsPage.Size = new System.Drawing.Size(717, 401);
            this.SettingsPage.Text = "Einstellungen";
            this.SettingsPage.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radPageViewItemPage1
            // 
            this.radPageViewItemPage1.Description = null;
            this.radPageViewItemPage1.ItemType = Telerik.WinControls.UI.PageViewItemType.GroupHeaderItem;
            this.radPageViewItemPage1.Location = new System.Drawing.Point(0, 0);
            this.radPageViewItemPage1.Name = "radPageViewItemPage1";
            this.radPageViewItemPage1.Size = new System.Drawing.Size(0, 0);
            this.radPageViewItemPage1.Text = "Extras";
            this.radPageViewItemPage1.Title = "Extras";
            // 
            // BonDesignerPage
            // 
            this.BonDesignerPage.Controls.Add(this.radPanel1);
            this.BonDesignerPage.Controls.Add(this.propertyGrid);
            this.BonDesignerPage.Controls.Add(this.AddImageBtn);
            this.BonDesignerPage.Image = global::POS.Properties.Resources.designer;
            this.BonDesignerPage.Location = new System.Drawing.Point(205, 4);
            this.BonDesignerPage.Name = "BonDesignerPage";
            this.BonDesignerPage.Size = new System.Drawing.Size(717, 401);
            this.BonDesignerPage.Text = "BonDesigner";
            // 
            // radPanel1
            // 
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(0, 73);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(456, 328);
            this.radPanel1.TabIndex = 3;
            this.radPanel1.ThemeName = "TelerikMetroTouch";
            // 
            // propertyGrid
            // 
            this.propertyGrid.AutoExpandGroups = false;
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGrid.EnableGrouping = false;
            this.propertyGrid.HelpVisible = false;
            this.propertyGrid.ItemHeight = 40;
            this.propertyGrid.ItemIndent = 40;
            this.propertyGrid.Location = new System.Drawing.Point(456, 73);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGrid.Size = new System.Drawing.Size(261, 328);
            this.propertyGrid.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.propertyGrid.TabIndex = 2;
            this.propertyGrid.Text = "radPropertyGrid1";
            this.propertyGrid.ThemeName = "TelerikMetroTouch";
            this.propertyGrid.ToolbarVisible = true;
            this.propertyGrid.Visible = false;
            // 
            // AddImageBtn
            // 
            this.AddImageBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.AddImageBtn.Location = new System.Drawing.Point(0, 0);
            this.AddImageBtn.Name = "AddImageBtn";
            this.AddImageBtn.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.commandBarRowElement1});
            this.AddImageBtn.Size = new System.Drawing.Size(717, 73);
            this.AddImageBtn.TabIndex = 1;
            this.AddImageBtn.Text = "Bild";
            this.AddImageBtn.ThemeName = "TelerikMetroTouch";
            // 
            // commandBarRowElement1
            // 
            this.commandBarRowElement1.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElement1.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.commandBarStripElement1});
            // 
            // commandBarStripElement1
            // 
            this.commandBarStripElement1.DisplayName = "commandBarStripElement1";
            this.commandBarStripElement1.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.ImageBtn,
            this.commandBarSeparator1,
            this.PropertiesBtn});
            this.commandBarStripElement1.Name = "commandBarStripElement1";
            // 
            // ImageBtn
            // 
            this.ImageBtn.AccessibleDescription = "commandBarButton1";
            this.ImageBtn.AccessibleName = "commandBarButton1";
            this.ImageBtn.DisplayName = "commandBarButton1";
            this.ImageBtn.Image = global::POS.Properties.Resources.images;
            this.ImageBtn.Name = "ImageBtn";
            this.ImageBtn.Text = "commandBarButton1";
            this.ImageBtn.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.ImageBtn.Click += new System.EventHandler(this.ImageBtn_Click);
            // 
            // commandBarSeparator1
            // 
            this.commandBarSeparator1.AccessibleDescription = "commandBarSeparator1";
            this.commandBarSeparator1.AccessibleName = "commandBarSeparator1";
            this.commandBarSeparator1.DisplayName = "commandBarSeparator1";
            this.commandBarSeparator1.Name = "commandBarSeparator1";
            this.commandBarSeparator1.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarSeparator1.VisibleInOverflowMenu = false;
            // 
            // PropertiesBtn
            // 
            this.PropertiesBtn.AccessibleDescription = "Eigenschaften";
            this.PropertiesBtn.AccessibleName = "Eigenschaften";
            this.PropertiesBtn.DisplayName = "commandBarButton2";
            this.PropertiesBtn.Image = global::POS.Properties.Resources.stock_folder_properties;
            this.PropertiesBtn.Name = "PropertiesBtn";
            this.PropertiesBtn.Text = "Eigenschaften";
            this.PropertiesBtn.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.PropertiesBtn.Click += new System.EventHandler(this.PropertiesBtn_Click);
            // 
            // radDesktopAlert1
            // 
            this.radDesktopAlert1.ShowOptionsButton = false;
            this.radDesktopAlert1.ShowPinButton = false;
            this.radDesktopAlert1.ThemeName = "TelerikMetroTouch";
            // 
            // ProductsView
            // 
            this.ProductsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProductsView.Groups.AddRange(new Telerik.WinControls.RadItem[] {
            this.tileGroupElement1});
            this.ProductsView.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radTileElement2});
            this.ProductsView.Location = new System.Drawing.Point(0, 0);
            this.ProductsView.Name = "ProductsView";
            this.ProductsView.ScrollBarThickness = 36;
            this.ProductsView.ShowGroups = true;
            this.ProductsView.Size = new System.Drawing.Size(717, 401);
            this.ProductsView.TabIndex = 0;
            this.ProductsView.ThemeName = "TelerikMetroTouch";
            // 
            // tileGroupElement1
            // 
            this.tileGroupElement1.AccessibleDescription = "Category";
            this.tileGroupElement1.AccessibleName = "Category";
            this.tileGroupElement1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radTileElement1});
            this.tileGroupElement1.Name = "tileGroupElement1";
            this.tileGroupElement1.Text = "Category";
            this.tileGroupElement1.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // radTileElement1
            // 
            this.radTileElement1.AccessibleDescription = "Product";
            this.radTileElement1.AccessibleName = "Product";
            this.radTileElement1.Name = "radTileElement1";
            this.radTileElement1.Text = "Product";
            this.radTileElement1.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // radTileElement2
            // 
            this.radTileElement2.AccessibleDescription = "Product";
            this.radTileElement2.AccessibleName = "Product";
            this.radTileElement2.Name = "radTileElement2";
            this.radTileElement2.Text = "Product";
            this.radTileElement2.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 409);
            this.Controls.Add(this.radPageView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).EndInit();
            this.radPageView1.ResumeLayout(false);
            this.HomePage.ResumeLayout(false);
            this.BonDesignerPage.ResumeLayout(false);
            this.BonDesignerPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddImageBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProductsView)).EndInit();
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
        private Telerik.WinControls.UI.RadPageViewItemPage radPageViewItemPage1;
        private Telerik.WinControls.UI.RadPageViewPage BonDesignerPage;
        private Telerik.WinControls.UI.RadPropertyGrid propertyGrid;
        private Telerik.WinControls.UI.RadCommandBar AddImageBtn;
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement1;
        private Telerik.WinControls.UI.CommandBarStripElement commandBarStripElement1;
        private Telerik.WinControls.UI.CommandBarButton ImageBtn;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator1;
        private Telerik.WinControls.UI.CommandBarButton PropertiesBtn;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadPanorama ProductsView;
        private Telerik.WinControls.UI.TileGroupElement tileGroupElement1;
        private Telerik.WinControls.UI.RadTileElement radTileElement1;
        private Telerik.WinControls.UI.RadTileElement radTileElement2;
    }
}
