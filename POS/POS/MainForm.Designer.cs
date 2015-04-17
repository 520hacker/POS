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
            this.ProductsView = new Telerik.WinControls.UI.RadPanorama();
            this.tileGroupElement1 = new Telerik.WinControls.UI.TileGroupElement();
            this.radTileElement1 = new Telerik.WinControls.UI.RadTileElement();
            this.radTileElement2 = new Telerik.WinControls.UI.RadTileElement();
            this.radPanel2 = new Telerik.WinControls.UI.RadPanel();
            this.historyView1 = new POS.UI.HistoryView();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.currencyLbl = new Telerik.WinControls.UI.RadLabel();
            this.priceLbl = new POS.UI.DigitalDisplayControl();
            this.payFooter = new Telerik.WinControls.UI.RadPanel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.paywithBTCBtn = new Telerik.WinControls.UI.RadButton();
            this.paywithEuroBtn = new Telerik.WinControls.UI.RadButton();
            this.ProductsPage = new Telerik.WinControls.UI.RadPageViewPage();
            this.productsView1 = new Telerik.WinControls.UI.RadGridView();
            this.SettingsPage = new Telerik.WinControls.UI.RadPageViewPage();
            this.btcddressTb = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
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
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.radDropDownButton1 = new Telerik.WinControls.UI.RadDropDownButton();
            this.radMenuItem1 = new Telerik.WinControls.UI.RadMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).BeginInit();
            this.radPageView1.SuspendLayout();
            this.HomePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProductsView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel2)).BeginInit();
            this.radPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.payFooter)).BeginInit();
            this.payFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paywithBTCBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paywithEuroBtn)).BeginInit();
            this.ProductsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.productsView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productsView1.MasterTemplate)).BeginInit();
            this.SettingsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btcddressTb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            this.BonDesignerPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddImageBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownButton1)).BeginInit();
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
            this.HomePage.Controls.Add(this.radPanel2);
            this.HomePage.Controls.Add(this.payFooter);
            this.HomePage.Image = global::POS.Properties.Resources.home;
            this.HomePage.Location = new System.Drawing.Point(205, 4);
            this.HomePage.Name = "HomePage";
            this.HomePage.Size = new System.Drawing.Size(717, 401);
            this.HomePage.Text = "Home";
            this.HomePage.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ProductsView
            // 
            this.ProductsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProductsView.Groups.AddRange(new Telerik.WinControls.RadItem[] {
            this.tileGroupElement1});
            this.ProductsView.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radTileElement2});
            this.ProductsView.Location = new System.Drawing.Point(0, 58);
            this.ProductsView.Name = "ProductsView";
            this.ProductsView.ScrollBarThickness = 36;
            this.ProductsView.ShowGroups = true;
            this.ProductsView.Size = new System.Drawing.Size(717, 307);
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
            // radPanel2
            // 
            this.radPanel2.Controls.Add(this.historyView1);
            this.radPanel2.Controls.Add(this.radButton1);
            this.radPanel2.Controls.Add(this.currencyLbl);
            this.radPanel2.Controls.Add(this.priceLbl);
            this.radPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanel2.Location = new System.Drawing.Point(0, 0);
            this.radPanel2.Name = "radPanel2";
            this.radPanel2.Size = new System.Drawing.Size(717, 58);
            this.radPanel2.TabIndex = 1;
            this.radPanel2.ThemeName = "TelerikMetroTouch";
            // 
            // historyView1
            // 
            this.historyView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.historyView1.Location = new System.Drawing.Point(54, 8);
            this.historyView1.Name = "historyView1";
            this.historyView1.Size = new System.Drawing.Size(392, 44);
            this.historyView1.TabIndex = 4;
            // 
            // radButton1
            // 
            this.radButton1.Dock = System.Windows.Forms.DockStyle.Left;
            this.radButton1.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radButton1.Location = new System.Drawing.Point(0, 0);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(48, 58);
            this.radButton1.TabIndex = 3;
            this.radButton1.Text = "-";
            this.radButton1.ThemeName = "TelerikMetroTouch";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // currencyLbl
            // 
            this.currencyLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.currencyLbl.AutoSize = false;
            this.currencyLbl.Font = new System.Drawing.Font("Segoe UI Light", 27F);
            this.currencyLbl.Location = new System.Drawing.Point(593, 3);
            this.currencyLbl.Name = "currencyLbl";
            this.currencyLbl.Size = new System.Drawing.Size(25, 51);
            this.currencyLbl.TabIndex = 2;
            this.currencyLbl.Text = "€";
            this.currencyLbl.ThemeName = "TelerikMetroTouch";
            // 
            // priceLbl
            // 
            this.priceLbl.BackColor = System.Drawing.Color.Transparent;
            this.priceLbl.DigitColor = System.Drawing.Color.Black;
            this.priceLbl.DigitText = "0.00";
            this.priceLbl.Dock = System.Windows.Forms.DockStyle.Right;
            this.priceLbl.Location = new System.Drawing.Point(624, 0);
            this.priceLbl.Name = "priceLbl";
            this.priceLbl.Size = new System.Drawing.Size(93, 58);
            this.priceLbl.TabIndex = 1;
            // 
            // payFooter
            // 
            this.payFooter.Controls.Add(this.radLabel2);
            this.payFooter.Controls.Add(this.paywithBTCBtn);
            this.payFooter.Controls.Add(this.paywithEuroBtn);
            this.payFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.payFooter.Location = new System.Drawing.Point(0, 365);
            this.payFooter.Name = "payFooter";
            this.payFooter.Size = new System.Drawing.Size(717, 36);
            this.payFooter.TabIndex = 5;
            this.payFooter.ThemeName = "TelerikMetroTouch";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(3, 6);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(93, 23);
            this.radLabel2.TabIndex = 7;
            this.radLabel2.Text = "Bezahlen mit";
            this.radLabel2.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.radLabel2.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.radLabel2.ThemeName = "TelerikMetroTouch";
            // 
            // paywithBTCBtn
            // 
            this.paywithBTCBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.paywithBTCBtn.Location = new System.Drawing.Point(599, 0);
            this.paywithBTCBtn.Name = "paywithBTCBtn";
            this.paywithBTCBtn.Size = new System.Drawing.Size(59, 36);
            this.paywithBTCBtn.TabIndex = 6;
            this.paywithBTCBtn.Text = "BTC";
            this.paywithBTCBtn.ThemeName = "TelerikMetroTouch";
            this.paywithBTCBtn.Click += new System.EventHandler(this.paywithBTCBtn_Click);
            // 
            // paywithEuroBtn
            // 
            this.paywithEuroBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.paywithEuroBtn.Location = new System.Drawing.Point(658, 0);
            this.paywithEuroBtn.Name = "paywithEuroBtn";
            this.paywithEuroBtn.Size = new System.Drawing.Size(59, 36);
            this.paywithEuroBtn.TabIndex = 5;
            this.paywithEuroBtn.Text = "€";
            this.paywithEuroBtn.ThemeName = "TelerikMetroTouch";
            this.paywithEuroBtn.Click += new System.EventHandler(this.paywithEuroBtn_Click);
            // 
            // ProductsPage
            // 
            this.ProductsPage.Controls.Add(this.productsView1);
            this.ProductsPage.Image = global::POS.Properties.Resources.box;
            this.ProductsPage.Location = new System.Drawing.Point(205, 4);
            this.ProductsPage.Name = "ProductsPage";
            this.ProductsPage.Size = new System.Drawing.Size(717, 401);
            this.ProductsPage.Text = "Produkte";
            this.ProductsPage.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // productsView1
            // 
            this.productsView1.BackColor = System.Drawing.Color.Transparent;
            this.productsView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.productsView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productsView1.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.productsView1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.productsView1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.productsView1.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.productsView1.MasterTemplate.AllowColumnReorder = false;
            this.productsView1.Name = "productsView1";
            this.productsView1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.productsView1.ShowGroupPanel = false;
            this.productsView1.Size = new System.Drawing.Size(717, 401);
            this.productsView1.TabIndex = 0;
            this.productsView1.ThemeName = "TelerikMetroTouch";
            // 
            // SettingsPage
            // 
            this.SettingsPage.Controls.Add(this.radDropDownButton1);
            this.SettingsPage.Controls.Add(this.btcddressTb);
            this.SettingsPage.Controls.Add(this.radLabel3);
            this.SettingsPage.Controls.Add(this.radLabel1);
            this.SettingsPage.Image = global::POS.Properties.Resources.application_x_desktop;
            this.SettingsPage.Location = new System.Drawing.Point(205, 4);
            this.SettingsPage.Name = "SettingsPage";
            this.SettingsPage.Size = new System.Drawing.Size(717, 401);
            this.SettingsPage.Text = "Einstellungen";
            this.SettingsPage.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btcddressTb
            // 
            this.btcddressTb.Location = new System.Drawing.Point(121, 9);
            this.btcddressTb.Name = "btcddressTb";
            this.btcddressTb.Size = new System.Drawing.Size(578, 30);
            this.btcddressTb.TabIndex = 1;
            this.btcddressTb.ThemeName = "TelerikMetroTouch";
            this.btcddressTb.TextChanged += new System.EventHandler(this.btcddressTb_TextChanged);
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(4, 9);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(111, 23);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "Bitcoin-Adresse";
            this.radLabel1.ThemeName = "TelerikMetroTouch";
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
            this.radPanel1.Location = new System.Drawing.Point(0, 1);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(456, 400);
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
            this.propertyGrid.Location = new System.Drawing.Point(456, 1);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGrid.Size = new System.Drawing.Size(261, 400);
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
            this.AddImageBtn.Size = new System.Drawing.Size(717, 1);
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
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "POS";
            this.notifyIcon1.Visible = true;
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(4, 45);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(61, 23);
            this.radLabel3.TabIndex = 2;
            this.radLabel3.Text = "Sprache";
            this.radLabel3.ThemeName = "TelerikMetroTouch";
            // 
            // radDropDownButton1
            // 
            this.radDropDownButton1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radMenuItem1});
            this.radDropDownButton1.Location = new System.Drawing.Point(121, 43);
            this.radDropDownButton1.Name = "radDropDownButton1";
            this.radDropDownButton1.Size = new System.Drawing.Size(212, 32);
            this.radDropDownButton1.TabIndex = 3;
            this.radDropDownButton1.ThemeName = "TelerikMetroTouch";
            // 
            // radMenuItem1
            // 
            this.radMenuItem1.AccessibleDescription = "Deutsch";
            this.radMenuItem1.AccessibleName = "Deutsch";
            this.radMenuItem1.Name = "radMenuItem1";
            this.radMenuItem1.Text = "Deutsch";
            this.radMenuItem1.Visibility = Telerik.WinControls.ElementVisibility.Visible;
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
            ((System.ComponentModel.ISupportInitialize)(this.ProductsView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel2)).EndInit();
            this.radPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.payFooter)).EndInit();
            this.payFooter.ResumeLayout(false);
            this.payFooter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paywithBTCBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paywithEuroBtn)).EndInit();
            this.ProductsPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.productsView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productsView1)).EndInit();
            this.SettingsPage.ResumeLayout(false);
            this.SettingsPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btcddressTb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            this.BonDesignerPage.ResumeLayout(false);
            this.BonDesignerPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddImageBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public Telerik.WinControls.Themes.TelerikMetroTouchTheme telerikMetroTouchTheme1;
        public Telerik.WinControls.UI.RadPageView radPageView1;
        public Telerik.WinControls.UI.RadPageViewPage HomePage;
        public Telerik.WinControls.UI.RadDesktopAlert radDesktopAlert1;
        public Telerik.WinControls.UI.RadPageViewPage ProductsPage;
        public Telerik.WinControls.UI.RadPageViewPage SettingsPage;
        public Telerik.WinControls.UI.RadPageViewItemPage radPageViewItemPage1;
        public Telerik.WinControls.UI.RadPageViewPage BonDesignerPage;
        public Telerik.WinControls.UI.RadPropertyGrid propertyGrid;
        public Telerik.WinControls.UI.RadCommandBar AddImageBtn;
        public Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement1;
        public Telerik.WinControls.UI.CommandBarStripElement commandBarStripElement1;
        public Telerik.WinControls.UI.CommandBarButton ImageBtn;
        public Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator1;
        public Telerik.WinControls.UI.CommandBarButton PropertiesBtn;
        public Telerik.WinControls.UI.RadPanel radPanel1;
        public Telerik.WinControls.UI.RadPanorama ProductsView;
        public Telerik.WinControls.UI.TileGroupElement tileGroupElement1;
        public Telerik.WinControls.UI.RadTileElement radTileElement1;
        public Telerik.WinControls.UI.RadTileElement radTileElement2;
        public Telerik.WinControls.UI.RadPanel radPanel2;
        public Telerik.WinControls.UI.RadLabel currencyLbl;
        public POS.UI.DigitalDisplayControl priceLbl;
        public Telerik.WinControls.UI.RadButton radButton1;
        public POS.UI.HistoryView historyView1;
        public Telerik.WinControls.UI.RadGridView productsView1;
        public Telerik.WinControls.UI.RadTextBox btcddressTb;
        public Telerik.WinControls.UI.RadLabel radLabel1;
        public Telerik.WinControls.UI.RadPanel payFooter;
        public Telerik.WinControls.UI.RadLabel radLabel2;
        public Telerik.WinControls.UI.RadButton paywithBTCBtn;
        public Telerik.WinControls.UI.RadButton paywithEuroBtn;
        public System.Windows.Forms.NotifyIcon notifyIcon1;
        private Telerik.WinControls.UI.RadDropDownButton radDropDownButton1;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem1;
        public Telerik.WinControls.UI.RadLabel radLabel3;
    }
}