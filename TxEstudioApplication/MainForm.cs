using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioKernel;
using System.IO;
using System.Reflection;
using TxEstudioKernel.CustomAttributes;
using TxEstudioApplication.Interfaces;
using Janus.Windows.UI.Dock;
using TxEstudioApplication.Custom_Controls;
using TxEstudioApplication.Dialogs;
using System.Drawing.Imaging;
using TxEstudioKernel.Operators;

namespace TxEstudioApplication
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            //Zoom related
            zoom20.Tag = 20;
            zoom50.Tag = 50;
            zoom100.Tag = 100;
            zoom200.Tag = 200;

            //Transform related 
            rotateClockwiseToolStripMenuItem.Tag = RotateFlipType.Rotate90FlipNone;
            rotateCounterclockwiseToolStripMenuItem.Tag = RotateFlipType.Rotate270FlipNone;
            flipHorizontalToolStripMenuItem.Tag = RotateFlipType.RotateNoneFlipX;
            flipVerticalToolStripMenuItem.Tag = RotateFlipType.RotateNoneFlipY;

            rotateC_toolStripButton.Tag = RotateFlipType.Rotate90FlipNone;
            rotateCC_toolStripButton.Tag = RotateFlipType.Rotate270FlipNone;
            flipH_toolStripButton.Tag = RotateFlipType.RotateNoneFlipX;
            flipV_toolStripButton.Tag = RotateFlipType.RotateNoneFlipY;

            ToolStripControlHost toolStrip = new ToolStripControlHost(opacity_TrackBar);
            toolStrip.Padding = new Padding(1);
            zoomTrackStrip.DropDown.Items.Add(toolStrip);
            this.Controls.Remove(opacity_TrackBar);

            InitSegmentationPanel();
            this.openToolStripMenuItem.DropDown.ItemClicked += new ToolStripItemClickedEventHandler(OpenDropDown_ItemClicked);
            //toolStrip.Overflow = ToolStripItemOverflow.Always;
            //toolStrip1.Items.Add(toolStrip);

        }

        private MenuStrip main_menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem printToolStripMenuItem;
        private ToolStripMenuItem printPreviewToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem contentsToolStripMenuItem;
        private ToolStripMenuItem indexToolStripMenuItem;
        private ToolStripMenuItem searchToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStrip main_toolStrip;
        private ToolStripButton newToolStripButton;
        private ToolStripButton openToolStripButton;
        private ToolStripButton saveToolStripButton;
        private ToolStripButton printToolStripButton;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripButton copyToolStripButton;
        private ToolStripButton pasteToolStripButton;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripButton helpToolStripButton;
        private StatusStrip statusStrip;
        private ToolStripMenuItem imageToolStripMenuItem;
        private ToolStripMenuItem transformToolStripMenuItem;
        private ToolStripMenuItem rotateClockwiseToolStripMenuItem;
        private ToolStripMenuItem rotateCounterclockwiseToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem flipHorizontalToolStripMenuItem;
        private ToolStripMenuItem flipVerticalToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripMenuItem grayScaleToolStripMenuItem;
        private ToolStripMenuItem splitToolStripMenuItem;
        private ToolStripMenuItem composeToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripMenuItem histogramToolStripMenuItem;
        private ToolStripMenuItem propertiesToolStripMenuItem;
        private ToolStripPanel top_toolStripPanel;
        private ToolStripMenuItem windowToolStripMenuItem;
        private ToolStripMenuItem cascadeToolStripMenuItem;
        private ToolStripMenuItem tileHorizontalToolStripMenuItem;
        private ToolStripMenuItem tileVerticalToolStripMenuItem;
        private ToolStripMenuItem organizeIconsToolStripMenuItem;
        private ToolStripMenuItem closeAllToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem zoomToolStripMenuItem;
        private ToolStripMenuItem zoomInToolStripMenuItem;
        private ToolStripMenuItem zoomOutToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem zoom20;
        private ToolStripMenuItem zoom50;
        private ToolStripMenuItem zoom100;
        private ToolStripMenuItem zoom200;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripMenuItem negativeToolStripMenuItem;
        private ToolStrip toolStrip1;
        private ToolStripButton rotateCC_toolStripButton;
        private ToolStripButton rotateC_toolStripButton;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripButton flipH_toolStripButton;
        private ToolStripButton flipV_toolStripButton;
        private ToolStripSeparator toolStripSeparator13;
        private ToolStripButton zoomIn_toolStripButton;
        private ToolStripButton zoomOut_toolStripButton;
        private Janus.Windows.UI.Dock.UIPanelManager panelManager;
        private IContainer components;
        private Janus.Windows.UI.Dock.UIPanel propertiesPanel;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer propertiesPanelContainer;
        private ImageList panelImageLists;
        private Janus.Windows.UI.Dock.UIPanel histogramPanel;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer histogramPanelContainer;
        private TxEstudioApplication.Custom_Controls.HistogramViewer histogramViewer1;
        private PropertyGrid propertyGrid1;
        private UIPanel segmentationPanel;
        private UIPanelInnerContainer segmentationPanelContainer;
        private Janus.Windows.EditControls.UIColorButton backGround_ButtonColor;
        private Panel panel1;
        private Label label1;
        private FlowLayoutPanel segmentation_FlowLayout;
        private Label label2;
        private Panel panel2;
        private Label label4;
        private Label label3;
        private ToolStripStatusLabel x_StatusLabel;
        private ToolStripStatusLabel y_StatusLabel;
        private ToolStripStatusLabel red_StatusLabel;
        private ToolStripStatusLabel green_StatusLabel;
        private ToolStripStatusLabel blue_StatusLabel;
        private ToolStripSeparator toolStripSeparator14;
        private TrackBar opacity_TrackBar;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripStatusLabel toolStripStatusLabel3;
        private ToolStripStatusLabel toolStripStatusLabel4;
        private ToolStripStatusLabel toolStripStatusLabel5;
        private ToolStripButton totalBordersButton;
        internal ToolStripDropDownButton zoomTrackStrip;
        private ToolStripContainer toolStripContainer1;
        private ToolStrip segmentationToolStrip;
        private ToolStripDropDownButton simplificationDropDownButton;
        private Panel byCountPanel;
        private Label byCountLabel;
        private NumericUpDown byCountUpDown;
        private Button byCountButton;
        private Panel bySizePanel;
        private Label bySizeLabel;
        private NumericUpDown bySizeUpDown;
        private Button bySizeButton;
        private ToolStripDropDownButton compareSegmentation;
        private ToolStripMenuItem byHungarianToolStripMenuItem;
        private ToolStripMenuItem byStableMarriageToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator15;
        private ToolStripMenuItem externalFiltersMenuItem;
        private ToolStripButton externalFilterStripButton;
        private PrintDialog printDialog;
        internal ToolStripMenuItem addOnsMenuItem;
        private ToolStripDropDownButton I;
        private ToolStripMenuItem universalImageQualityIndexToolStripMenuItem;
        private ToolStripMenuItem contentBasedImageQualityMetricToolStripMenuItem;
        private ToolStripMenuItem structuralSimilarityBasedImageQualityAssesmentToolStripMenuItem;
        private ToolStripMenuItem brihgtContrastToolStripMenuItem;
        private ToolStripMenuItem histogramEqualizationToolStripMenuItem;
        private ToolStripMenuItem recentFilesToolStripMenuItem;
        private ToolStripDropDownButton featureSelectionButton;
        private ToolStripMenuItem principalComponentAnalysisToolStripMenuItem;
        private ToolStripMenuItem sToolStripMenuItem;
        private ToolStripMenuItem theoreticalQualityMeasureToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator16;

        #region Vars
        Environment env;
        #endregion

        #region Actions

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageHolderForm holder = new ImageHolderForm();
            holder.Bitmap = new Bitmap(5, 5);
            holder.MdiParent = this;
            holder.Show();
        }

        protected void open_Image_Method(object sender, EventArgs e)
        {
            //ImageHolderForm.OpenImage(env);
            fileToolStripMenuItem.DropDown.Close();

            string[] files = env.OpenMultipleImages();
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    ImageHolderForm.OpenImage(files[i], env);
                }
                catch (Exception exc)
                {
                    env.ShowError(string.Format("Error while loading {0}. Message:{1}", files[i], exc.Message));
                }
            }
        }
        protected void save_Image_Method(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null && ActiveMdiChild is ISaveManager)
            {
                (ActiveMdiChild as ISaveManager).Save();
            }
        }
        protected void saveAs_Image_Method(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null && ActiveMdiChild is ISaveManager)
            {
                (ActiveMdiChild as ISaveManager).SaveAs();
            }
        }
        protected void copy_Method(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null && ActiveMdiChild is IClipboardManager)
            {
                IClipboardManager cm = ActiveMdiChild as IClipboardManager;
                if (cm.CanCopyToClipboard())
                    cm.CopyToClipboard();
            }
        }

        int newImagesCount = 0;
        protected void paste_Method(object sender, EventArgs e)
        {
            try
            {
                if (ActiveMdiChild != null && ActiveMdiChild is IClipboardManager && (ActiveMdiChild as IClipboardManager).CanGetFromClipboard())
                    (ActiveMdiChild as IClipboardManager).GetFromClipboard();
                else
                    if (Clipboard.ContainsImage())
                    {
                        ImageHolderForm newForm = new ImageHolderForm();
                        newForm.Image = new TxImage(Clipboard.GetImage());
                        newForm.ImageName = String.Format("new{0}", newImagesCount++);
                        env.OpenWindow(newForm);
                    }
            }
            catch (Exception exc)
            {
                env.ShowError("Some error ocurred while pasting image from clipboard\nMessage: " + exc.Message);
            }

        }

        private void externalFilter_Apply(object sender, EventArgs e)
        {
            env.ShowDialog(new ExternalFiltersDialog());
        } 
        
        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            if(ActiveMdiChild != null && ActiveMdiChild is IPrintManager)
            {
                IPrintManager pManager = ActiveMdiChild as IPrintManager;
                System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
                printDocument.BeginPrint+= new System.Drawing.Printing.PrintEventHandler(pManager.BeginPrint);
                printDocument.EndPrint += new System.Drawing.Printing.PrintEventHandler(pManager.EndPrint);
                printDocument.QueryPageSettings += new System.Drawing.Printing.QueryPageSettingsEventHandler(pManager.QueryPageSettings);
                printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pManager.PrintPage);
                printDialog.Document = printDocument;
                if (printDialog.ShowDialog() == DialogResult.OK)
                    printDocument.Print();
                printDialog.Document = null;
            }
        }
        #endregion

        #region MDI behavior
        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void tileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void arrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void MainForm_MdiChildActivate(object sender, EventArgs e)
        {
            UpdatePanels();
        }
        #endregion

        #region Zoom related code
        private void zoomItem_Click_Action(object sender, EventArgs e)
        {
            if (ActiveMdiChild is IZoomManager)
                (ActiveMdiChild as IZoomManager).ZoomLevel = (int)((ToolStripMenuItem)sender).Tag;
            //(ActiveMdiChild as ImageHolderForm).SetZoom((int)((ToolStripMenuItem)sender).Tag);
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IZoomManager zm = (ActiveMdiChild as IZoomManager);
            if (zm != null)
                zm.ZoomLevel += zm.ZoomLevel / 2;///zm.ZoomLevel + 1;

        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IZoomManager zm = (ActiveMdiChild as IZoomManager);
            if (zm != null && zm.ZoomLevel > 10)
                zm.ZoomLevel -= zm.ZoomLevel / 2;//zm.ZoomLevel - 1;
        }
        #endregion

       
        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool enabled = ActiveMdiChild is ImageHolderForm;
            zoomToolStripMenuItem.Enabled = enabled;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            env = new Environment(this);
            env.LoadOperators();
            env.LoadAddOns();
            this.LoadRecentFiles();

        }

       

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.main_menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.zoom20 = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom50 = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom100 = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom200 = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transformToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotateClockwiseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotateCounterclockwiseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.flipHorizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flipVerticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.negativeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.grayScaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.composeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.externalFiltersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.brihgtContrastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.histogramEqualizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.histogramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cascadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileHorizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileVerticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.organizeIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addOnsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.main_toolStrip = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.x_StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.y_StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.red_StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.green_StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.blue_StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.top_toolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.rotateCC_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.rotateC_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.flipH_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.flipV_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.zoomIn_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.zoomTrackStrip = new System.Windows.Forms.ToolStripDropDownButton();
            this.zoomOut_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.externalFilterStripButton = new System.Windows.Forms.ToolStripButton();
            this.totalBordersButton = new System.Windows.Forms.ToolStripButton();
            this.compareSegmentation = new System.Windows.Forms.ToolStripDropDownButton();
            this.byHungarianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.theoreticalQualityMeasureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byStableMarriageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.I = new System.Windows.Forms.ToolStripDropDownButton();
            this.universalImageQualityIndexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentBasedImageQualityMetricToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.structuralSimilarityBasedImageQualityAssesmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.featureSelectionButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.principalComponentAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelManager = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.panelImageLists = new System.Windows.Forms.ImageList(this.components);
            this.propertiesPanel = new Janus.Windows.UI.Dock.UIPanel();
            this.propertiesPanelContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.histogramPanel = new Janus.Windows.UI.Dock.UIPanel();
            this.histogramPanelContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.histogramViewer1 = new TxEstudioApplication.Custom_Controls.HistogramViewer();
            this.segmentationPanel = new Janus.Windows.UI.Dock.UIPanel();
            this.segmentationPanelContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.segmentation_FlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.backGround_ButtonColor = new Janus.Windows.EditControls.UIColorButton();
            this.segmentationToolStrip = new System.Windows.Forms.ToolStrip();
            this.simplificationDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.opacity_TrackBar = new System.Windows.Forms.TrackBar();
            this.bySizePanel = new System.Windows.Forms.Panel();
            this.bySizeLabel = new System.Windows.Forms.Label();
            this.bySizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.bySizeButton = new System.Windows.Forms.Button();
            this.byCountPanel = new System.Windows.Forms.Panel();
            this.byCountUpDown = new System.Windows.Forms.NumericUpDown();
            this.byCountLabel = new System.Windows.Forms.Label();
            this.byCountButton = new System.Windows.Forms.Button();
            this.printDialog = new System.Windows.Forms.PrintDialog();
            this.main_menuStrip.SuspendLayout();
            this.main_toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.top_toolStripPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesPanel)).BeginInit();
            this.propertiesPanel.SuspendLayout();
            this.propertiesPanelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.histogramPanel)).BeginInit();
            this.histogramPanel.SuspendLayout();
            this.histogramPanelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.segmentationPanel)).BeginInit();
            this.segmentationPanel.SuspendLayout();
            this.segmentationPanelContainer.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.segmentationToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opacity_TrackBar)).BeginInit();
            this.bySizePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bySizeUpDown)).BeginInit();
            this.byCountPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.byCountUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // main_menuStrip
            // 
            this.main_menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.imageToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.addOnsMenuItem,
            this.helpToolStripMenuItem});
            this.main_menuStrip.Location = new System.Drawing.Point(0, 0);
            this.main_menuStrip.MdiWindowListItem = this.windowToolStripMenuItem;
            this.main_menuStrip.Name = "main_menuStrip";
            this.main_menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.main_menuStrip.Size = new System.Drawing.Size(819, 24);
            this.main_menuStrip.TabIndex = 0;
            this.main_menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.printToolStripMenuItem,
            this.printPreviewToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recentFilesToolStripMenuItem,
            this.toolStripSeparator16});
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.open_Image_Method);
            // 
            // recentFilesToolStripMenuItem
            // 
            this.recentFilesToolStripMenuItem.Name = "recentFilesToolStripMenuItem";
            this.recentFilesToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.recentFilesToolStripMenuItem.Text = "Recent Files";
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(133, 6);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(143, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.save_Image_Method);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAs_Image_Method);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripMenuItem.Image")));
            this.printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.printToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.printToolStripMenuItem.Text = "&Print";
            // 
            // printPreviewToolStripMenuItem
            // 
            this.printPreviewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printPreviewToolStripMenuItem.Image")));
            this.printPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.printPreviewToolStripMenuItem.Text = "Print Pre&view";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.selectAllToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
            this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copy_Method);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
            this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.pasteToolStripMenuItem.Text = "&Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.paste_Method);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(141, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.selectAllToolStripMenuItem.Text = "Select &All";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // zoomToolStripMenuItem
            // 
            this.zoomToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomInToolStripMenuItem,
            this.zoomOutToolStripMenuItem,
            this.toolStripSeparator3,
            this.zoom20,
            this.zoom50,
            this.zoom100,
            this.zoom200});
            this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            this.zoomToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.zoomToolStripMenuItem.Text = "Zoom";
            // 
            // zoomInToolStripMenuItem
            // 
            this.zoomInToolStripMenuItem.Image = global::TxEstudioApplication.Properties.Resources.zoomIn;
            this.zoomInToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Lime;
            this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
            this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.zoomInToolStripMenuItem.Text = "Zoom In";
            this.zoomInToolStripMenuItem.Click += new System.EventHandler(this.zoomInToolStripMenuItem_Click);
            // 
            // zoomOutToolStripMenuItem
            // 
            this.zoomOutToolStripMenuItem.Image = global::TxEstudioApplication.Properties.Resources.zoomOut;
            this.zoomOutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Lime;
            this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
            this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.zoomOutToolStripMenuItem.Text = "Zoom Out";
            this.zoomOutToolStripMenuItem.Click += new System.EventHandler(this.zoomOutToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(126, 6);
            // 
            // zoom20
            // 
            this.zoom20.Name = "zoom20";
            this.zoom20.Size = new System.Drawing.Size(129, 22);
            this.zoom20.Text = "20%";
            this.zoom20.Click += new System.EventHandler(this.zoomItem_Click_Action);
            // 
            // zoom50
            // 
            this.zoom50.Name = "zoom50";
            this.zoom50.Size = new System.Drawing.Size(129, 22);
            this.zoom50.Text = "50%";
            this.zoom50.Click += new System.EventHandler(this.zoomItem_Click_Action);
            // 
            // zoom100
            // 
            this.zoom100.Name = "zoom100";
            this.zoom100.Size = new System.Drawing.Size(129, 22);
            this.zoom100.Text = "100%";
            this.zoom100.Click += new System.EventHandler(this.zoomItem_Click_Action);
            // 
            // zoom200
            // 
            this.zoom200.Name = "zoom200";
            this.zoom200.Size = new System.Drawing.Size(129, 22);
            this.zoom200.Text = "200%";
            this.zoom200.Click += new System.EventHandler(this.zoomItem_Click_Action);
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.transformToolStripMenuItem,
            this.toolStripSeparator9,
            this.grayScaleToolStripMenuItem,
            this.splitToolStripMenuItem,
            this.composeToolStripMenuItem,
            this.toolStripSeparator15,
            this.externalFiltersMenuItem,
            this.toolStripSeparator10,
            this.brihgtContrastToolStripMenuItem,
            this.histogramEqualizationToolStripMenuItem,
            this.histogramToolStripMenuItem,
            this.propertiesToolStripMenuItem});
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.imageToolStripMenuItem.Text = "Image";
            // 
            // transformToolStripMenuItem
            // 
            this.transformToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rotateClockwiseToolStripMenuItem,
            this.rotateCounterclockwiseToolStripMenuItem,
            this.toolStripSeparator8,
            this.flipHorizontalToolStripMenuItem,
            this.flipVerticalToolStripMenuItem,
            this.toolStripSeparator11,
            this.negativeToolStripMenuItem});
            this.transformToolStripMenuItem.Name = "transformToolStripMenuItem";
            this.transformToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.transformToolStripMenuItem.Text = "Transform";
            // 
            // rotateClockwiseToolStripMenuItem
            // 
            this.rotateClockwiseToolStripMenuItem.Image = global::TxEstudioApplication.Properties.Resources.clock1;
            this.rotateClockwiseToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Lime;
            this.rotateClockwiseToolStripMenuItem.Name = "rotateClockwiseToolStripMenuItem";
            this.rotateClockwiseToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.rotateClockwiseToolStripMenuItem.Text = "Rotate clockwise";
            this.rotateClockwiseToolStripMenuItem.Click += new System.EventHandler(this.transform_Action);
            // 
            // rotateCounterclockwiseToolStripMenuItem
            // 
            this.rotateCounterclockwiseToolStripMenuItem.Image = global::TxEstudioApplication.Properties.Resources.counterclock1;
            this.rotateCounterclockwiseToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Lime;
            this.rotateCounterclockwiseToolStripMenuItem.Name = "rotateCounterclockwiseToolStripMenuItem";
            this.rotateCounterclockwiseToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.rotateCounterclockwiseToolStripMenuItem.Text = "Rotate counterclockwise";
            this.rotateCounterclockwiseToolStripMenuItem.Click += new System.EventHandler(this.transform_Action);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(200, 6);
            // 
            // flipHorizontalToolStripMenuItem
            // 
            this.flipHorizontalToolStripMenuItem.Image = global::TxEstudioApplication.Properties.Resources.FlipHorizontalHS;
            this.flipHorizontalToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.flipHorizontalToolStripMenuItem.Name = "flipHorizontalToolStripMenuItem";
            this.flipHorizontalToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.flipHorizontalToolStripMenuItem.Text = "Flip horizontal";
            this.flipHorizontalToolStripMenuItem.Click += new System.EventHandler(this.transform_Action);
            // 
            // flipVerticalToolStripMenuItem
            // 
            this.flipVerticalToolStripMenuItem.Image = global::TxEstudioApplication.Properties.Resources.FlipVerticalHS;
            this.flipVerticalToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.flipVerticalToolStripMenuItem.Name = "flipVerticalToolStripMenuItem";
            this.flipVerticalToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.flipVerticalToolStripMenuItem.Text = "Flip vertical";
            this.flipVerticalToolStripMenuItem.Click += new System.EventHandler(this.transform_Action);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(200, 6);
            // 
            // negativeToolStripMenuItem
            // 
            this.negativeToolStripMenuItem.Name = "negativeToolStripMenuItem";
            this.negativeToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.negativeToolStripMenuItem.Text = "Negative";
            this.negativeToolStripMenuItem.Click += new System.EventHandler(this.negativeToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(194, 6);
            // 
            // grayScaleToolStripMenuItem
            // 
            this.grayScaleToolStripMenuItem.Name = "grayScaleToolStripMenuItem";
            this.grayScaleToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.grayScaleToolStripMenuItem.Text = "Gray scale";
            this.grayScaleToolStripMenuItem.Click += new System.EventHandler(this.grayScaleToolStripMenuItem_Click);
            // 
            // splitToolStripMenuItem
            // 
            this.splitToolStripMenuItem.Name = "splitToolStripMenuItem";
            this.splitToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.splitToolStripMenuItem.Text = "Color decomposition";
            this.splitToolStripMenuItem.Click += new System.EventHandler(this.splitToolStripMenuItem_Click);
            // 
            // composeToolStripMenuItem
            // 
            this.composeToolStripMenuItem.Name = "composeToolStripMenuItem";
            this.composeToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.composeToolStripMenuItem.Text = "Color composition";
            this.composeToolStripMenuItem.Click += new System.EventHandler(this.composeToolStripMenuItem_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(194, 6);
            // 
            // externalFiltersMenuItem
            // 
            this.externalFiltersMenuItem.Image = global::TxEstudioApplication.Properties.Resources.Filter2HS;
            this.externalFiltersMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.externalFiltersMenuItem.Name = "externalFiltersMenuItem";
            this.externalFiltersMenuItem.Size = new System.Drawing.Size(197, 22);
            this.externalFiltersMenuItem.Text = "External filters";
            this.externalFiltersMenuItem.Click += new System.EventHandler(this.externalFilter_Apply);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(194, 6);
            // 
            // brihgtContrastToolStripMenuItem
            // 
            this.brihgtContrastToolStripMenuItem.Name = "brihgtContrastToolStripMenuItem";
            this.brihgtContrastToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.brihgtContrastToolStripMenuItem.Text = "Brihgt-Contrast";
            this.brihgtContrastToolStripMenuItem.Click += new System.EventHandler(this.brihgtContrastToolStripMenuItem_Click);
            // 
            // histogramEqualizationToolStripMenuItem
            // 
            this.histogramEqualizationToolStripMenuItem.Name = "histogramEqualizationToolStripMenuItem";
            this.histogramEqualizationToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.histogramEqualizationToolStripMenuItem.Text = "Histogram Equalization";
            this.histogramEqualizationToolStripMenuItem.Click += new System.EventHandler(this.histogramEqualizationToolStripMenuItem_Click);
            // 
            // histogramToolStripMenuItem
            // 
            this.histogramToolStripMenuItem.Image = global::TxEstudioApplication.Properties.Resources.histogram;
            this.histogramToolStripMenuItem.Name = "histogramToolStripMenuItem";
            this.histogramToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.histogramToolStripMenuItem.Text = "Histogram";
            this.histogramToolStripMenuItem.Click += new System.EventHandler(this.histogramToolStripMenuItem_Click);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Image = global::TxEstudioApplication.Properties.Resources.PropertiesHS;
            this.propertiesToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cascadeToolStripMenuItem,
            this.tileHorizontalToolStripMenuItem,
            this.tileVerticalToolStripMenuItem,
            this.organizeIconsToolStripMenuItem,
            this.closeAllToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.windowToolStripMenuItem.Text = "Window";
            // 
            // cascadeToolStripMenuItem
            // 
            this.cascadeToolStripMenuItem.Name = "cascadeToolStripMenuItem";
            this.cascadeToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.cascadeToolStripMenuItem.Text = "Cascade";
            this.cascadeToolStripMenuItem.Click += new System.EventHandler(this.cascadeToolStripMenuItem_Click);
            // 
            // tileHorizontalToolStripMenuItem
            // 
            this.tileHorizontalToolStripMenuItem.Name = "tileHorizontalToolStripMenuItem";
            this.tileHorizontalToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.tileHorizontalToolStripMenuItem.Text = "Tile horizontal";
            this.tileHorizontalToolStripMenuItem.Click += new System.EventHandler(this.tileHorizontalToolStripMenuItem_Click);
            // 
            // tileVerticalToolStripMenuItem
            // 
            this.tileVerticalToolStripMenuItem.Name = "tileVerticalToolStripMenuItem";
            this.tileVerticalToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.tileVerticalToolStripMenuItem.Text = "Tile vertical";
            this.tileVerticalToolStripMenuItem.Click += new System.EventHandler(this.tileVerticalToolStripMenuItem_Click);
            // 
            // organizeIconsToolStripMenuItem
            // 
            this.organizeIconsToolStripMenuItem.Name = "organizeIconsToolStripMenuItem";
            this.organizeIconsToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.organizeIconsToolStripMenuItem.Text = "Arrange icons";
            this.organizeIconsToolStripMenuItem.Click += new System.EventHandler(this.arrangeIconsToolStripMenuItem_Click);
            // 
            // closeAllToolStripMenuItem
            // 
            this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            this.closeAllToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.closeAllToolStripMenuItem.Text = "Close all";
            this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.closeAllToolStripMenuItem_Click);
            // 
            // addOnsMenuItem
            // 
            this.addOnsMenuItem.Name = "addOnsMenuItem";
            this.addOnsMenuItem.Size = new System.Drawing.Size(65, 20);
            this.addOnsMenuItem.Text = "Add Ons";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.contentsToolStripMenuItem.Text = "&Contents";
            this.contentsToolStripMenuItem.Click += new System.EventHandler(this.contentsToolStripMenuItem_Click);
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.indexToolStripMenuItem.Text = "&Index";
            this.indexToolStripMenuItem.Click += new System.EventHandler(this.indexToolStripMenuItem_Click);
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.searchToolStripMenuItem.Text = "&Search";
            this.searchToolStripMenuItem.Click += new System.EventHandler(this.searchToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(119, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            // 
            // main_toolStrip
            // 
            this.main_toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.main_toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton,
            this.printToolStripButton,
            this.toolStripSeparator6,
            this.copyToolStripButton,
            this.pasteToolStripButton,
            this.toolStripSeparator7,
            this.helpToolStripButton});
            this.main_toolStrip.Location = new System.Drawing.Point(3, 0);
            this.main_toolStrip.Name = "main_toolStrip";
            this.main_toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.main_toolStrip.Size = new System.Drawing.Size(185, 25);
            this.main_toolStrip.TabIndex = 1;
            this.main_toolStrip.Text = "toolStrip1";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.newToolStripButton.Text = "&New";
            this.newToolStripButton.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openToolStripButton.Text = "&Open";
            this.openToolStripButton.Click += new System.EventHandler(this.open_Image_Method);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "&Save";
            this.saveToolStripButton.Click += new System.EventHandler(this.save_Image_Method);
            // 
            // printToolStripButton
            // 
            this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripButton.Image")));
            this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolStripButton.Name = "printToolStripButton";
            this.printToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.printToolStripButton.Text = "&Print";
            this.printToolStripButton.Click += new System.EventHandler(this.printToolStripButton_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // copyToolStripButton
            // 
            this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripButton.Image")));
            this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripButton.Name = "copyToolStripButton";
            this.copyToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.copyToolStripButton.Text = "&Copy";
            this.copyToolStripButton.Click += new System.EventHandler(this.copy_Method);
            // 
            // pasteToolStripButton
            // 
            this.pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pasteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripButton.Image")));
            this.pasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolStripButton.Name = "pasteToolStripButton";
            this.pasteToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.pasteToolStripButton.Text = "&Paste";
            this.pasteToolStripButton.Click += new System.EventHandler(this.paste_Method);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // helpToolStripButton
            // 
            this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripButton.Image")));
            this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpToolStripButton.Name = "helpToolStripButton";
            this.helpToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.helpToolStripButton.Text = "He&lp";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.x_StatusLabel,
            this.toolStripStatusLabel2,
            this.y_StatusLabel,
            this.toolStripStatusLabel3,
            this.red_StatusLabel,
            this.toolStripStatusLabel4,
            this.green_StatusLabel,
            this.toolStripStatusLabel5,
            this.blue_StatusLabel});
            this.statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip.Location = new System.Drawing.Point(0, 437);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip.Size = new System.Drawing.Size(819, 22);
            this.statusStrip.Stretch = false;
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(17, 17);
            this.toolStripStatusLabel1.Text = "X:";
            // 
            // x_StatusLabel
            // 
            this.x_StatusLabel.AutoSize = false;
            this.x_StatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.x_StatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.x_StatusLabel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.x_StatusLabel.Name = "x_StatusLabel";
            this.x_StatusLabel.Size = new System.Drawing.Size(34, 14);
            this.x_StatusLabel.ToolTipText = "X coordinate";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(17, 17);
            this.toolStripStatusLabel2.Text = "Y:";
            // 
            // y_StatusLabel
            // 
            this.y_StatusLabel.AutoSize = false;
            this.y_StatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.y_StatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.y_StatusLabel.Margin = new System.Windows.Forms.Padding(2, 3, 0, 2);
            this.y_StatusLabel.Name = "y_StatusLabel";
            this.y_StatusLabel.Size = new System.Drawing.Size(34, 14);
            this.y_StatusLabel.ToolTipText = "Y coordinate";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(30, 17);
            this.toolStripStatusLabel3.Text = "Red:";
            // 
            // red_StatusLabel
            // 
            this.red_StatusLabel.AutoSize = false;
            this.red_StatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.red_StatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.red_StatusLabel.Margin = new System.Windows.Forms.Padding(5, 3, 0, 2);
            this.red_StatusLabel.Name = "red_StatusLabel";
            this.red_StatusLabel.Size = new System.Drawing.Size(34, 14);
            this.red_StatusLabel.ToolTipText = "Red";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(41, 17);
            this.toolStripStatusLabel4.Text = "Green:";
            // 
            // green_StatusLabel
            // 
            this.green_StatusLabel.AutoSize = false;
            this.green_StatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.green_StatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.green_StatusLabel.Margin = new System.Windows.Forms.Padding(2, 3, 0, 2);
            this.green_StatusLabel.Name = "green_StatusLabel";
            this.green_StatusLabel.Size = new System.Drawing.Size(34, 14);
            this.green_StatusLabel.ToolTipText = "Green";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(33, 17);
            this.toolStripStatusLabel5.Text = "Blue:";
            // 
            // blue_StatusLabel
            // 
            this.blue_StatusLabel.AutoSize = false;
            this.blue_StatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.blue_StatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.blue_StatusLabel.Margin = new System.Windows.Forms.Padding(2, 3, 0, 2);
            this.blue_StatusLabel.Name = "blue_StatusLabel";
            this.blue_StatusLabel.Size = new System.Drawing.Size(34, 14);
            this.blue_StatusLabel.ToolTipText = "Blue";
            // 
            // top_toolStripPanel
            // 
            this.top_toolStripPanel.Controls.Add(this.main_toolStrip);
            this.top_toolStripPanel.Controls.Add(this.toolStrip1);
            this.top_toolStripPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.top_toolStripPanel.Location = new System.Drawing.Point(0, 24);
            this.top_toolStripPanel.Name = "top_toolStripPanel";
            this.top_toolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.top_toolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.top_toolStripPanel.Size = new System.Drawing.Size(819, 25);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rotateCC_toolStripButton,
            this.rotateC_toolStripButton,
            this.toolStripSeparator12,
            this.flipH_toolStripButton,
            this.flipV_toolStripButton,
            this.toolStripSeparator13,
            this.zoomIn_toolStripButton,
            this.zoomTrackStrip,
            this.zoomOut_toolStripButton,
            this.toolStripSeparator14,
            this.externalFilterStripButton,
            this.totalBordersButton,
            this.compareSegmentation,
            this.I,
            this.featureSelectionButton});
            this.toolStrip1.Location = new System.Drawing.Point(235, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(569, 25);
            this.toolStrip1.TabIndex = 2;
            // 
            // rotateCC_toolStripButton
            // 
            this.rotateCC_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.rotateCC_toolStripButton.Image = global::TxEstudioApplication.Properties.Resources.counterclock1;
            this.rotateCC_toolStripButton.ImageTransparentColor = System.Drawing.Color.Lime;
            this.rotateCC_toolStripButton.Name = "rotateCC_toolStripButton";
            this.rotateCC_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.rotateCC_toolStripButton.Text = "toolStripButton1";
            this.rotateCC_toolStripButton.ToolTipText = "Rotate counterclockwise";
            this.rotateCC_toolStripButton.Click += new System.EventHandler(this.transform_Action);
            // 
            // rotateC_toolStripButton
            // 
            this.rotateC_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.rotateC_toolStripButton.Image = global::TxEstudioApplication.Properties.Resources.clock1;
            this.rotateC_toolStripButton.ImageTransparentColor = System.Drawing.Color.Lime;
            this.rotateC_toolStripButton.Name = "rotateC_toolStripButton";
            this.rotateC_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.rotateC_toolStripButton.Text = "toolStripButton2";
            this.rotateC_toolStripButton.ToolTipText = "Rotate clockwise";
            this.rotateC_toolStripButton.Click += new System.EventHandler(this.transform_Action);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(6, 25);
            // 
            // flipH_toolStripButton
            // 
            this.flipH_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.flipH_toolStripButton.Image = global::TxEstudioApplication.Properties.Resources.FlipHorizontalHS;
            this.flipH_toolStripButton.ImageTransparentColor = System.Drawing.Color.Black;
            this.flipH_toolStripButton.Name = "flipH_toolStripButton";
            this.flipH_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.flipH_toolStripButton.Text = "toolStripButton3";
            this.flipH_toolStripButton.ToolTipText = "Flip horizontal";
            this.flipH_toolStripButton.Click += new System.EventHandler(this.transform_Action);
            // 
            // flipV_toolStripButton
            // 
            this.flipV_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.flipV_toolStripButton.Image = global::TxEstudioApplication.Properties.Resources.FlipVerticalHS;
            this.flipV_toolStripButton.ImageTransparentColor = System.Drawing.Color.Black;
            this.flipV_toolStripButton.Name = "flipV_toolStripButton";
            this.flipV_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.flipV_toolStripButton.Text = "toolStripButton4";
            this.flipV_toolStripButton.ToolTipText = "Flip vertical";
            this.flipV_toolStripButton.Click += new System.EventHandler(this.transform_Action);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(6, 25);
            // 
            // zoomIn_toolStripButton
            // 
            this.zoomIn_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.zoomIn_toolStripButton.Image = global::TxEstudioApplication.Properties.Resources.zoomIn;
            this.zoomIn_toolStripButton.ImageTransparentColor = System.Drawing.Color.Lime;
            this.zoomIn_toolStripButton.Name = "zoomIn_toolStripButton";
            this.zoomIn_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.zoomIn_toolStripButton.Text = "toolStripButton5";
            this.zoomIn_toolStripButton.ToolTipText = "Zoom In";
            this.zoomIn_toolStripButton.Click += new System.EventHandler(this.zoomInToolStripMenuItem_Click);
            // 
            // zoomTrackStrip
            // 
            this.zoomTrackStrip.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.zoomTrackStrip.Image = global::TxEstudioApplication.Properties.Resources.zoomTrack;
            this.zoomTrackStrip.ImageTransparentColor = System.Drawing.Color.Lime;
            this.zoomTrackStrip.Name = "zoomTrackStrip";
            this.zoomTrackStrip.Size = new System.Drawing.Size(29, 22);
            this.zoomTrackStrip.Text = "Zoom";
            this.zoomTrackStrip.ToolTipText = "Zoom";
            // 
            // zoomOut_toolStripButton
            // 
            this.zoomOut_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.zoomOut_toolStripButton.Image = global::TxEstudioApplication.Properties.Resources.zoomOut;
            this.zoomOut_toolStripButton.ImageTransparentColor = System.Drawing.Color.Lime;
            this.zoomOut_toolStripButton.Name = "zoomOut_toolStripButton";
            this.zoomOut_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.zoomOut_toolStripButton.Text = "toolStripButton6";
            this.zoomOut_toolStripButton.ToolTipText = "Zoom Out";
            this.zoomOut_toolStripButton.Click += new System.EventHandler(this.zoomOutToolStripMenuItem_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(6, 25);
            // 
            // externalFilterStripButton
            // 
            this.externalFilterStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.externalFilterStripButton.Image = global::TxEstudioApplication.Properties.Resources.Filter2HS;
            this.externalFilterStripButton.ImageTransparentColor = System.Drawing.Color.Black;
            this.externalFilterStripButton.Name = "externalFilterStripButton";
            this.externalFilterStripButton.Size = new System.Drawing.Size(23, 22);
            this.externalFilterStripButton.Click += new System.EventHandler(this.externalFilter_Apply);
            // 
            // totalBordersButton
            // 
            this.totalBordersButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.totalBordersButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.totalBordersButton.Name = "totalBordersButton";
            this.totalBordersButton.Size = new System.Drawing.Size(81, 22);
            this.totalBordersButton.Text = "Total borders";
            this.totalBordersButton.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // compareSegmentation
            // 
            this.compareSegmentation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.compareSegmentation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.byHungarianToolStripMenuItem,
            this.theoreticalQualityMeasureToolStripMenuItem,
            this.byStableMarriageToolStripMenuItem});
            this.compareSegmentation.Image = ((System.Drawing.Image)(resources.GetObject("compareSegmentation.Image")));
            this.compareSegmentation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.compareSegmentation.Name = "compareSegmentation";
            this.compareSegmentation.Size = new System.Drawing.Size(69, 22);
            this.compareSegmentation.Text = "Compare";
            // 
            // byHungarianToolStripMenuItem
            // 
            this.byHungarianToolStripMenuItem.Name = "byHungarianToolStripMenuItem";
            this.byHungarianToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.byHungarianToolStripMenuItem.Text = "RQM: Real Quality Measure";
            this.byHungarianToolStripMenuItem.Click += new System.EventHandler(this.byHungarianToolStripMenuItem_Click);
            // 
            // theoreticalQualityMeasureToolStripMenuItem
            // 
            this.theoreticalQualityMeasureToolStripMenuItem.Name = "theoreticalQualityMeasureToolStripMenuItem";
            this.theoreticalQualityMeasureToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.theoreticalQualityMeasureToolStripMenuItem.Text = "TQM: Theoretical Quality Measure";
            this.theoreticalQualityMeasureToolStripMenuItem.Click += new System.EventHandler(this.theoreticalQualityMeasureToolStripMenuItem_Click);
            // 
            // byStableMarriageToolStripMenuItem
            // 
            this.byStableMarriageToolStripMenuItem.Name = "byStableMarriageToolStripMenuItem";
            this.byStableMarriageToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.byStableMarriageToolStripMenuItem.Text = "By Stable Marriage";
            this.byStableMarriageToolStripMenuItem.Click += new System.EventHandler(this.byStableMarriageToolStripMenuItem_Click);
            // 
            // I
            // 
            this.I.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.I.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.universalImageQualityIndexToolStripMenuItem,
            this.contentBasedImageQualityMetricToolStripMenuItem,
            this.structuralSimilarityBasedImageQualityAssesmentToolStripMenuItem});
            this.I.Image = ((System.Drawing.Image)(resources.GetObject("I.Image")));
            this.I.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.I.Name = "I";
            this.I.Size = new System.Drawing.Size(58, 22);
            this.I.Text = "Quality";
            // 
            // universalImageQualityIndexToolStripMenuItem
            // 
            this.universalImageQualityIndexToolStripMenuItem.Name = "universalImageQualityIndexToolStripMenuItem";
            this.universalImageQualityIndexToolStripMenuItem.Size = new System.Drawing.Size(352, 22);
            this.universalImageQualityIndexToolStripMenuItem.Text = "Universal Image Quality Index";
            this.universalImageQualityIndexToolStripMenuItem.Click += new System.EventHandler(this.universalImageQualityIndexToolStripMenuItem_Click);
            // 
            // contentBasedImageQualityMetricToolStripMenuItem
            // 
            this.contentBasedImageQualityMetricToolStripMenuItem.Name = "contentBasedImageQualityMetricToolStripMenuItem";
            this.contentBasedImageQualityMetricToolStripMenuItem.Size = new System.Drawing.Size(352, 22);
            this.contentBasedImageQualityMetricToolStripMenuItem.Text = "Content-Based Image Quality Metric";
            this.contentBasedImageQualityMetricToolStripMenuItem.Click += new System.EventHandler(this.contentBasedImageQualityMetricToolStripMenuItem_Click);
            // 
            // structuralSimilarityBasedImageQualityAssesmentToolStripMenuItem
            // 
            this.structuralSimilarityBasedImageQualityAssesmentToolStripMenuItem.Name = "structuralSimilarityBasedImageQualityAssesmentToolStripMenuItem";
            this.structuralSimilarityBasedImageQualityAssesmentToolStripMenuItem.Size = new System.Drawing.Size(352, 22);
            this.structuralSimilarityBasedImageQualityAssesmentToolStripMenuItem.Text = "Structural-Similarity-Based Image Quality Assesment";
            this.structuralSimilarityBasedImageQualityAssesmentToolStripMenuItem.Click += new System.EventHandler(this.structuralSimilarityBasedImageQualityAssesmentToolStripMenuItem_Click);
            // 
            // featureSelectionButton
            // 
            this.featureSelectionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.featureSelectionButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.principalComponentAnalysisToolStripMenuItem,
            this.sToolStripMenuItem});
            this.featureSelectionButton.Image = ((System.Drawing.Image)(resources.GetObject("featureSelectionButton.Image")));
            this.featureSelectionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.featureSelectionButton.Name = "featureSelectionButton";
            this.featureSelectionButton.Size = new System.Drawing.Size(110, 22);
            this.featureSelectionButton.Text = "Feature Selection";
            // 
            // principalComponentAnalysisToolStripMenuItem
            // 
            this.principalComponentAnalysisToolStripMenuItem.Name = "principalComponentAnalysisToolStripMenuItem";
            this.principalComponentAnalysisToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.principalComponentAnalysisToolStripMenuItem.Text = "Principal Component Analysis";
            this.principalComponentAnalysisToolStripMenuItem.Click += new System.EventHandler(this.principalComponentAnalysisToolStripMenuItem_Click);
            // 
            // sToolStripMenuItem
            // 
            this.sToolStripMenuItem.Name = "sToolStripMenuItem";
            this.sToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.sToolStripMenuItem.Text = "SFFS";
            this.sToolStripMenuItem.Click += new System.EventHandler(this.sToolStripMenuItem_Click);
            // 
            // panelManager
            // 
            this.panelManager.ContainerControl = this;
            this.panelManager.DefaultPanelSettings.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.panelManager.ImageList = this.panelImageLists;
            this.propertiesPanel.Id = new System.Guid("7e1b24c0-065c-476b-90dd-101b4c213a06");
            this.panelManager.Panels.Add(this.propertiesPanel);
            this.histogramPanel.Id = new System.Guid("baa32940-e645-4337-8a33-ef30755f0814");
            this.panelManager.Panels.Add(this.histogramPanel);
            this.segmentationPanel.Id = new System.Guid("812d73bf-93ee-4330-bb6d-b6497e65da12");
            this.panelManager.Panels.Add(this.segmentationPanel);
            // 
            // Design Time Panel Info:
            // 
            this.panelManager.BeginPanelInfo();
            this.panelManager.AddDockPanelInfo(new System.Guid("7e1b24c0-065c-476b-90dd-101b4c213a06"), Janus.Windows.UI.Dock.PanelDockStyle.Right, new System.Drawing.Size(200, 382), true);
            this.panelManager.AddDockPanelInfo(new System.Guid("baa32940-e645-4337-8a33-ef30755f0814"), Janus.Windows.UI.Dock.PanelDockStyle.Right, new System.Drawing.Size(275, 382), true);
            this.panelManager.AddDockPanelInfo(new System.Guid("812d73bf-93ee-4330-bb6d-b6497e65da12"), Janus.Windows.UI.Dock.PanelDockStyle.Right, new System.Drawing.Size(200, 382), true);
            this.panelManager.AddFloatingPanelInfo(new System.Guid("7e1b24c0-065c-476b-90dd-101b4c213a06"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.panelManager.AddFloatingPanelInfo(new System.Guid("baa32940-e645-4337-8a33-ef30755f0814"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.panelManager.AddFloatingPanelInfo(new System.Guid("812d73bf-93ee-4330-bb6d-b6497e65da12"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.panelManager.EndPanelInfo();
            // 
            // panelImageLists
            // 
            this.panelImageLists.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("panelImageLists.ImageStream")));
            this.panelImageLists.TransparentColor = System.Drawing.Color.Black;
            this.panelImageLists.Images.SetKeyName(0, "PropertiesHS.BMP");
            // 
            // propertiesPanel
            // 
            this.propertiesPanel.AutoHide = true;
            this.propertiesPanel.ImageIndex = 0;
            this.propertiesPanel.InnerContainer = this.propertiesPanelContainer;
            this.propertiesPanel.InnerContainerFormatStyle.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.propertiesPanel.InnerContainerFormatStyle.BackColorGradient = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertiesPanel.InnerContainerFormatStyle.BackgroundGradientMode = Janus.Windows.UI.BackgroundGradientMode.Horizontal;
            this.propertiesPanel.Location = new System.Drawing.Point(400, 52);
            this.propertiesPanel.Name = "propertiesPanel";
            this.propertiesPanel.Size = new System.Drawing.Size(200, 382);
            this.propertiesPanel.TabIndex = 4;
            this.propertiesPanel.Text = "Properties";
            this.propertiesPanel.Visible = false;
            // 
            // propertiesPanelContainer
            // 
            this.propertiesPanelContainer.Controls.Add(this.propertyGrid1);
            this.propertiesPanelContainer.Location = new System.Drawing.Point(5, 23);
            this.propertiesPanelContainer.Name = "propertiesPanelContainer";
            this.propertiesPanelContainer.Size = new System.Drawing.Size(194, 358);
            this.propertiesPanelContainer.TabIndex = 0;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertyGrid1.Size = new System.Drawing.Size(194, 358);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // histogramPanel
            // 
            this.histogramPanel.AutoHide = true;
            this.histogramPanel.Image = ((System.Drawing.Image)(resources.GetObject("histogramPanel.Image")));
            this.histogramPanel.InnerContainer = this.histogramPanelContainer;
            this.histogramPanel.InnerContainerFormatStyle.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.histogramPanel.InnerContainerFormatStyle.BackColorGradient = System.Drawing.SystemColors.InactiveCaptionText;
            this.histogramPanel.InnerContainerFormatStyle.BackgroundGradientMode = Janus.Windows.UI.BackgroundGradientMode.Horizontal;
            this.histogramPanel.Location = new System.Drawing.Point(325, 52);
            this.histogramPanel.Name = "histogramPanel";
            this.histogramPanel.Size = new System.Drawing.Size(275, 382);
            this.histogramPanel.TabIndex = 4;
            this.histogramPanel.Text = "Histogram";
            // 
            // histogramPanelContainer
            // 
            this.histogramPanelContainer.Controls.Add(this.histogramViewer1);
            this.histogramPanelContainer.Location = new System.Drawing.Point(5, 23);
            this.histogramPanelContainer.Name = "histogramPanelContainer";
            this.histogramPanelContainer.Size = new System.Drawing.Size(269, 358);
            this.histogramPanelContainer.TabIndex = 0;
            // 
            // histogramViewer1
            // 
            this.histogramViewer1.BackColor = System.Drawing.Color.Transparent;
            this.histogramViewer1.Histogram = null;
            this.histogramViewer1.Location = new System.Drawing.Point(4, 4);
            this.histogramViewer1.MaximumSize = new System.Drawing.Size(263, 277);
            this.histogramViewer1.MinimumSize = new System.Drawing.Size(263, 277);
            this.histogramViewer1.Name = "histogramViewer1";
            this.histogramViewer1.Size = new System.Drawing.Size(263, 277);
            this.histogramViewer1.TabIndex = 0;
            // 
            // segmentationPanel
            // 
            this.segmentationPanel.AutoHide = true;
            this.segmentationPanel.Image = global::TxEstudioApplication.Properties.Resources.segmentResIco;
            this.segmentationPanel.InnerContainer = this.segmentationPanelContainer;
            this.segmentationPanel.InnerContainerFormatStyle.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.segmentationPanel.InnerContainerFormatStyle.BackColorGradient = System.Drawing.SystemColors.InactiveCaptionText;
            this.segmentationPanel.InnerContainerFormatStyle.BackgroundGradientMode = Janus.Windows.UI.BackgroundGradientMode.Horizontal;
            this.segmentationPanel.Location = new System.Drawing.Point(596, 52);
            this.segmentationPanel.Name = "segmentationPanel";
            this.segmentationPanel.Size = new System.Drawing.Size(200, 382);
            this.segmentationPanel.TabIndex = 4;
            this.segmentationPanel.Text = "Segmentation Result";
            // 
            // segmentationPanelContainer
            // 
            this.segmentationPanelContainer.Controls.Add(this.toolStripContainer1);
            this.segmentationPanelContainer.Location = new System.Drawing.Point(5, 23);
            this.segmentationPanelContainer.Name = "segmentationPanelContainer";
            this.segmentationPanelContainer.Size = new System.Drawing.Size(194, 358);
            this.segmentationPanelContainer.TabIndex = 0;
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AutoScroll = true;
            this.toolStripContainer1.ContentPanel.BackColor = System.Drawing.Color.Transparent;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.segmentation_FlowLayout);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel2);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(194, 333);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(194, 358);
            this.toolStripContainer1.TabIndex = 10;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.segmentationToolStrip);
            // 
            // segmentation_FlowLayout
            // 
            this.segmentation_FlowLayout.BackColor = System.Drawing.Color.Transparent;
            this.segmentation_FlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.segmentation_FlowLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.segmentation_FlowLayout.Location = new System.Drawing.Point(0, 47);
            this.segmentation_FlowLayout.Name = "segmentation_FlowLayout";
            this.segmentation_FlowLayout.Size = new System.Drawing.Size(194, 286);
            this.segmentation_FlowLayout.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(194, 14);
            this.panel2.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(103, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Show Original";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(63, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Color";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "View";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.backGround_ButtonColor);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(194, 33);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Background";
            // 
            // backGround_ButtonColor
            // 
            // 
            // 
            // 
            this.backGround_ButtonColor.ColorPicker.Location = new System.Drawing.Point(0, 0);
            this.backGround_ButtonColor.ColorPicker.Name = "";
            this.backGround_ButtonColor.ColorPicker.Size = new System.Drawing.Size(100, 100);
            this.backGround_ButtonColor.ColorPicker.TabIndex = 0;
            this.backGround_ButtonColor.ImageReplaceableColor = System.Drawing.Color.Empty;
            this.backGround_ButtonColor.Location = new System.Drawing.Point(92, 4);
            this.backGround_ButtonColor.Name = "backGround_ButtonColor";
            this.backGround_ButtonColor.Size = new System.Drawing.Size(43, 23);
            this.backGround_ButtonColor.TabIndex = 0;
            this.backGround_ButtonColor.UseColorName = false;
            this.backGround_ButtonColor.SelectedColorChanged += new System.EventHandler(this.backGround_ButtonColor_SelectedColorChanged);
            // 
            // segmentationToolStrip
            // 
            this.segmentationToolStrip.AutoSize = false;
            this.segmentationToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.segmentationToolStrip.GripMargin = new System.Windows.Forms.Padding(1);
            this.segmentationToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.segmentationToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.simplificationDropDownButton});
            this.segmentationToolStrip.Location = new System.Drawing.Point(0, 0);
            this.segmentationToolStrip.Name = "segmentationToolStrip";
            this.segmentationToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.segmentationToolStrip.Size = new System.Drawing.Size(194, 25);
            this.segmentationToolStrip.Stretch = true;
            this.segmentationToolStrip.TabIndex = 0;
            // 
            // simplificationDropDownButton
            // 
            this.simplificationDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.simplificationDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("simplificationDropDownButton.Image")));
            this.simplificationDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.simplificationDropDownButton.Name = "simplificationDropDownButton";
            this.simplificationDropDownButton.Size = new System.Drawing.Size(63, 22);
            this.simplificationDropDownButton.Text = "Simplify";
            // 
            // opacity_TrackBar
            // 
            this.opacity_TrackBar.AutoSize = false;
            this.opacity_TrackBar.Location = new System.Drawing.Point(481, 139);
            this.opacity_TrackBar.Margin = new System.Windows.Forms.Padding(10);
            this.opacity_TrackBar.Maximum = 200;
            this.opacity_TrackBar.Minimum = 1;
            this.opacity_TrackBar.Name = "opacity_TrackBar";
            this.opacity_TrackBar.Size = new System.Drawing.Size(92, 19);
            this.opacity_TrackBar.TabIndex = 6;
            this.opacity_TrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.opacity_TrackBar.Value = 1;
            this.opacity_TrackBar.Scroll += new System.EventHandler(this.opacity_TrackBar_Scroll);
            // 
            // bySizePanel
            // 
            this.bySizePanel.BackColor = System.Drawing.Color.Transparent;
            this.bySizePanel.Controls.Add(this.bySizeLabel);
            this.bySizePanel.Controls.Add(this.bySizeUpDown);
            this.bySizePanel.Controls.Add(this.bySizeButton);
            this.bySizePanel.Location = new System.Drawing.Point(481, 67);
            this.bySizePanel.Name = "bySizePanel";
            this.bySizePanel.Size = new System.Drawing.Size(140, 25);
            this.bySizePanel.TabIndex = 9;
            // 
            // bySizeLabel
            // 
            this.bySizeLabel.AutoSize = true;
            this.bySizeLabel.Location = new System.Drawing.Point(3, 7);
            this.bySizeLabel.Name = "bySizeLabel";
            this.bySizeLabel.Size = new System.Drawing.Size(42, 13);
            this.bySizeLabel.TabIndex = 2;
            this.bySizeLabel.Text = "By Size";
            // 
            // bySizeUpDown
            // 
            this.bySizeUpDown.Location = new System.Drawing.Point(51, 3);
            this.bySizeUpDown.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.bySizeUpDown.Name = "bySizeUpDown";
            this.bySizeUpDown.Size = new System.Drawing.Size(55, 20);
            this.bySizeUpDown.TabIndex = 1;
            this.bySizeUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // bySizeButton
            // 
            this.bySizeButton.Image = global::TxEstudioApplication.Properties.Resources.right_arrow;
            this.bySizeButton.Location = new System.Drawing.Point(112, 2);
            this.bySizeButton.Name = "bySizeButton";
            this.bySizeButton.Size = new System.Drawing.Size(24, 23);
            this.bySizeButton.TabIndex = 0;
            this.bySizeButton.UseVisualStyleBackColor = true;
            this.bySizeButton.Click += new System.EventHandler(this.bySizeButton_Click);
            // 
            // byCountPanel
            // 
            this.byCountPanel.BackColor = System.Drawing.Color.Transparent;
            this.byCountPanel.Controls.Add(this.byCountUpDown);
            this.byCountPanel.Controls.Add(this.byCountLabel);
            this.byCountPanel.Controls.Add(this.byCountButton);
            this.byCountPanel.Location = new System.Drawing.Point(481, 101);
            this.byCountPanel.Name = "byCountPanel";
            this.byCountPanel.Size = new System.Drawing.Size(140, 25);
            this.byCountPanel.TabIndex = 10;
            // 
            // byCountUpDown
            // 
            this.byCountUpDown.Location = new System.Drawing.Point(51, 3);
            this.byCountUpDown.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.byCountUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.byCountUpDown.Name = "byCountUpDown";
            this.byCountUpDown.Size = new System.Drawing.Size(55, 20);
            this.byCountUpDown.TabIndex = 1;
            this.byCountUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // byCountLabel
            // 
            this.byCountLabel.AutoSize = true;
            this.byCountLabel.Location = new System.Drawing.Point(3, 7);
            this.byCountLabel.Name = "byCountLabel";
            this.byCountLabel.Size = new System.Drawing.Size(50, 13);
            this.byCountLabel.TabIndex = 2;
            this.byCountLabel.Text = "By Count";
            // 
            // byCountButton
            // 
            this.byCountButton.Image = global::TxEstudioApplication.Properties.Resources.right_arrow;
            this.byCountButton.Location = new System.Drawing.Point(112, 2);
            this.byCountButton.Name = "byCountButton";
            this.byCountButton.Size = new System.Drawing.Size(24, 23);
            this.byCountButton.TabIndex = 0;
            this.byCountButton.UseVisualStyleBackColor = true;
            this.byCountButton.Click += new System.EventHandler(this.byCountButton_Click);
            // 
            // printDialog
            // 
            this.printDialog.UseEXDialog = true;
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.ClientSize = new System.Drawing.Size(819, 459);
            this.Controls.Add(this.byCountPanel);
            this.Controls.Add(this.bySizePanel);
            this.Controls.Add(this.opacity_TrackBar);
            this.Controls.Add(this.top_toolStripPanel);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.main_menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.main_menuStrip;
            this.Name = "MainForm";
            this.Text = "TxEstudio ver 2.0";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.MdiChildActivate += new System.EventHandler(this.MainForm_MdiChildActivate);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.main_menuStrip.ResumeLayout(false);
            this.main_menuStrip.PerformLayout();
            this.main_toolStrip.ResumeLayout(false);
            this.main_toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.top_toolStripPanel.ResumeLayout(false);
            this.top_toolStripPanel.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertiesPanel)).EndInit();
            this.propertiesPanel.ResumeLayout(false);
            this.propertiesPanelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.histogramPanel)).EndInit();
            this.histogramPanel.ResumeLayout(false);
            this.histogramPanelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.segmentationPanel)).EndInit();
            this.segmentationPanel.ResumeLayout(false);
            this.segmentationPanelContainer.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.segmentationToolStrip.ResumeLayout(false);
            this.segmentationToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opacity_TrackBar)).EndInit();
            this.bySizePanel.ResumeLayout(false);
            this.bySizePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bySizeUpDown)).EndInit();
            this.byCountPanel.ResumeLayout(false);
            this.byCountPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.byCountUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #region Image operations
        private void transform_Action(object sender, EventArgs e)
        {
            //Flip and rotate operations are implemented using .NET image class methods
            IImageExposer ie = ActiveMdiChild as IImageExposer;
            if (ie != null)
            {
                ie.Bitmap.RotateFlip((RotateFlipType)((ToolStripItem)sender).Tag);
                ie.Bitmap = ie.Bitmap; // Updating purposes
            }
        }

        private void negativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IImageExposer ie = ActiveMdiChild as IImageExposer;
            if (ie != null)
            {
                ie.Image = new TxNegative().Process(ie.Image);
                UpdatePanels(); //Panels update purposes


            }
        }

        private void grayScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IImageExposer ie = ActiveMdiChild as IImageExposer;
            if (ie != null && ie.Image.ImageFormat == TxImageFormat.RGB)
            {
                ImageHolderForm newForm = new ImageHolderForm();
                newForm.ImageName = String.Format("new{0}", newImagesCount++);
                newForm.Image = ie.Image.ToGrayScale();
                env.OpenWindow(newForm);
            }
        }

        private void splitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] color = new string[3] { "_Red", "_Green", "_Blue" };
            IImageExposer ie = ActiveMdiChild as IImageExposer;
            if (ie != null && ie.Image.ImageFormat == TxImageFormat.RGB)
            {
                List<TxImage> result = new TxColorDecomposition().Process(ie.Image);
                for (int i = 0; i < 3; i++)
                {
                    this.Update();
                    ImageHolderForm newForm = new ImageHolderForm();
                    newForm.Image = result[i];
                    newForm.ImageName = (ie as IImageExposer).ImageName + color[i];
                    env.OpenWindow(newForm);
                }
            }
        }

        private void composeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComposeDialogBox box = new ComposeDialogBox();
            box.SetEnvironment(env);
            box.ShowDialog();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //propertiesPanel.Activate();
            propertiesPanel.AutoHideActive = true;


        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //histogramPanel.Activate();
            histogramPanel.AutoHideActive = true;
        }
        #endregion

        #region Segmentation Panel

        private void InitSegmentationPanel()
        {
            simplificationDropDownButton.DropDown.Items.Add(new ToolStripControlHost(bySizePanel));
            Controls.Remove(bySizePanel);
            simplificationDropDownButton.DropDown.Items.Add(new ToolStripSeparator());
            simplificationDropDownButton.DropDown.Items.Add(new ToolStripControlHost(byCountPanel));
            Controls.Remove(byCountPanel);
        }

        private void bySizeButton_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is SegmentationResultForm)
            {
                SegmentationResultForm initial = (SegmentationResultForm)ActiveMdiChild;
                TxEstudioKernel.Operators.ConnectedComponents.SegmentationSimplificationBySize bySize
                    = new TxEstudioKernel.Operators.ConnectedComponents.SegmentationSimplificationBySize();
                bySize.MinSize = (int)bySizeUpDown.Value;
                SegmentationResultForm newForm = new SegmentationResultForm();
                newForm.SetData(initial.Original, bySize.Process(initial.Segmentation));
                newForm.ImageName = initial.ImageName + "+" + AbbreviationAttribute.GetFullAbbreviation(bySize);
                env.OpenWindow(newForm);
            }
            simplificationDropDownButton.DropDown.Hide();
        }

        private void byCountButton_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is SegmentationResultForm)
            {
                SegmentationResultForm initial = (SegmentationResultForm)ActiveMdiChild;
                TxEstudioKernel.Operators.ConnectedComponents.SegmentationSimplificationByCount byCount
                    = new TxEstudioKernel.Operators.ConnectedComponents.SegmentationSimplificationByCount();
                byCount.MaxCount = (int)byCountUpDown.Value;
                SegmentationResultForm newForm = new SegmentationResultForm();
                newForm.SetData(initial.Original, byCount.Process(initial.Segmentation));
                newForm.ImageName = initial.ImageName + "+" + AbbreviationAttribute.GetFullAbbreviation(byCount);
                env.OpenWindow(newForm);
            }
            simplificationDropDownButton.DropDown.Hide();
        }

        private void FillSegmentationPanel(SegmentationResultForm form)
        {
            if (form.Segmentation.Classes < 30)
            {
                backGround_ButtonColor.SelectedColor = form.BackGround;
                segmentation_FlowLayout.Controls.Clear();
                for (int i = 0; i < form.Segmentation.Classes; i++)
                {
                    ClassSettingViewer settingViewer = new ClassSettingViewer();
                    settingViewer.Setting = form.DisplaySettigns[i];
                    settingViewer.Tag = i;
                    settingViewer.SettingChanged += new EventHandler(settingViewer_SettingChanged);
                    segmentation_FlowLayout.Controls.Add(settingViewer);
                }
            }
            else
                ClearSegmentationPanel();
        }

        void settingViewer_SettingChanged(object sender, EventArgs e)
        {
            SegmentationResultForm form = ActiveMdiChild as SegmentationResultForm;
            if (form != null)
            {
                ClassSettingViewer viewer = sender as ClassSettingViewer;
                form.ChangeSetting((int)viewer.Tag, viewer.Setting);
            }
        }

        private void backGround_ButtonColor_SelectedColorChanged(object sender, EventArgs e)
        {
            SegmentationResultForm form = ActiveMdiChild as SegmentationResultForm;
            if (form != null)
                form.BackGround = backGround_ButtonColor.SelectedColor;
        }

        private void ClearSegmentationPanel()
        {
            backGround_ButtonColor.SelectedColor = Color.Empty;
            segmentation_FlowLayout.Controls.Clear();
        }



        #endregion

        #region RecentFiles
        int maxcount = 9;
        string rfpath = "RecentFiles.sv";
        private void SaveRecentFiles()
        {
            if (!File.Exists(rfpath))
            {
                File.Create(rfpath).Close();
            }
            FileStream stream = File.OpenWrite(rfpath);

            StreamWriter writer = new StreamWriter(stream);

            ToolStripItemCollection list = openToolStripMenuItem.DropDown.Items;

            for (int i = 2; i < list.Count; i++)
            {
                //Directory 
                writer.WriteLine(list[i].ToolTipText);
                //FileName
                writer.WriteLine(list[i].Text);
            }
            writer.Close();
        }
        private void LoadRecentFiles()
        {

            if (File.Exists(rfpath))
            {
                FileStream stream = File.OpenRead(rfpath);
                StreamReader reader = new StreamReader(stream);

                int cont = 0;
                while (!reader.EndOfStream && cont < maxcount)
                {
                    string dir = reader.ReadLine();
                    string filename = reader.ReadLine();
                    AddRecentFileMenuItem(filename, dir);
                    cont++;
                }
                reader.Close();
            }
        }
        public void PushRecentFileMenuItem(string filename, string dir)
        {
            ToolStripItemCollection list = openToolStripMenuItem.DropDown.Items;

            ToolStripItem item = new ToolStripButton(filename);
            item.ToolTipText = dir;

            string path = dir + filename;

            for (int i = 2; i < list.Count; i++)
            {
                if (list[i].ToolTipText + list[i].Text == path)
                {
                    list.Remove(list[i]);
                    list.Insert(2, item);
                    return;
                }
            }

            list.Insert(2, item);

            // -2 porque ya xisten dos items de encabezado 
            if (list.Count - 2 > maxcount)
            {
                list.RemoveAt(maxcount + 2);
            }
        }
        private void AddRecentFileMenuItem(string filename, string dir)
        {
            ToolStripItem item = new ToolStripButton(filename);
            item.ToolTipText = dir;
            openToolStripMenuItem.DropDown.Items.Add(item);
        }

        void OpenDropDown_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            int pos = openToolStripMenuItem.DropDown.Items.IndexOf(item);

            if (pos > 1)
            {
                string path = item.ToolTipText + item.Text;
                ImageHolderForm.OpenImage(path, env);
            }

        }
        #endregion


        /// <summary>
        /// Para refrescar los paneles y demas en el momento que cambie la ventana
        /// activa o cuando cambie la imagen en la ventana activa.
        /// </summary>
        private void UpdatePanels()
        {
            IImageExposer ie = ActiveMdiChild as IImageExposer;
            if (ie != null)
                histogramViewer1.Histogram = new TxHistogram(ie.Image);
            else
                histogramViewer1.Histogram = null;
            IPropertiesExposer ip = ActiveMdiChild as IPropertiesExposer;
            if (ip != null)
                propertyGrid1.SelectedObject = ip.Properties;
            else
                propertyGrid1.SelectedObject = null;
            SegmentationResultForm form = ActiveMdiChild as SegmentationResultForm;
            if (form != null)
            {
                FillSegmentationPanel(form);//segmentation_PictureBox.Image = form.Original;
            }
            else
            {
                ClearSegmentationPanel();
                //segmentation_PictureBox.Image = null;
            }
            IZoomManager zoom = ActiveMdiChild as IZoomManager;
            if (zoom != null)
            {
                opacity_TrackBar.Value = Math.Min( zoom.ZoomLevel, 200);
            }
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //TxMatrix matrix = new TxMatrix(new float[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } });
            //TxMatrix kernel = new TxMatrix(new float[,] { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } });
            ////TxMatrix matrix = TxMatrix.FromImage(env.OpenImage());

            //TxMatrix result = matrix.Convolve(kernel);

            //for (int i = 0; i < result.Height; i++)
            //{
            //    for (int i = 0; i < result.Width; i++)
            //    {
            //       result[j, i] = 0;
            //    }
            //}
            //IImageExposer ie = ActiveMdiChild as IImageExposer;
            //if (ie != null)
            //{
            //    TxImage image = ie.Image;
            //    ImageHolderForm newForm = new ImageHolderForm();
            //    newForm.Image = /*TxMatrix.FromImage(image).ToImage();*/new AC().Process(image);
            //    env.OpenWindow(newForm);
            //}
            //TxEstudioKernel.Operators.GaborEnergyTextureDescriptor gabor = new TxEstudioKernel.Operators.GaborEnergyTextureDescriptor();
            //gabor.Orientations = 1;
            //gabor.Frequencies = new float[] { 1f };
            //TxImage image = (ActiveMdiChild as IImageExposer).Image;
            //gabor.Reset();
            //while (gabor.MoveNext())
            //{
            //    ImageHolderForm form = new ImageHolderForm();
            //    form.Image = gabor.Current.GetDescription(image).ToImage();
            //    env.OpenWindow(form);
            //}

            Dialogs.TotalEdgesDialog totalEdges = new TxEstudioApplication.Dialogs.TotalEdgesDialog();
           // totalEdges.ShowDialog(this);
            env.ShowDialog(totalEdges);
        }

        internal void AddPanel(UIPanel panel)
        {
            panelManager.Panels.Add(panel);
            panel.AutoHide = true;
            panel.InnerContainerFormatStyle.BackColor = SystemColors.GradientActiveCaption;
            panel.InnerContainerFormatStyle.BackColorGradient = SystemColors.InactiveCaptionText;
            panel.InnerContainerFormatStyle.BackgroundGradientMode = Janus.Windows.UI.BackgroundGradientMode.Horizontal;
        }

        public void DisplayCoordinates(object sender, MouseOverImageEventArgs e)
        {
            x_StatusLabel.Text = e.X.ToString();
            y_StatusLabel.Text = e.Y.ToString();
            red_StatusLabel.Text = e.Color.R.ToString();
            green_StatusLabel.Text = e.Color.G.ToString();
            blue_StatusLabel.Text = e.Color.B.ToString();

        }

        private void opacity_TrackBar_Scroll(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null && ActiveMdiChild is IZoomManager)
            {
                //ActiveMdiChild.Opacity = opacity_TrackBar.Value;
                //this.Text = opacity_TrackBar.Value.ToString();
                (ActiveMdiChild as IZoomManager).ZoomLevel = opacity_TrackBar.Value;
            }
        }
        #region Drag drop
        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
            
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileDrop = (System.String[])e.Data.GetData(DataFormats.FileDrop, true);
                for (int i = 0; i < fileDrop.Length; i++)
                {
                    try
                    {
                        ImageHolderForm.OpenImage(fileDrop[i], env);
                    }
                    catch (Exception exc)
                    {
                        env.ShowError(string.Format("Error while loading {0}. Message: {1}", fileDrop[i], exc.Message));
                    }
                   
                }
            }
        }
        #endregion

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SegmentationComparisonDialog dialog = new SegmentationComparisonDialog();
            env.ShowDialog(dialog);
            
        }

        private void byHungarianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SegmentationComparisonDialog dialog = new SegmentationComparisonDialog();
            env.ShowDialog(dialog);
        }

        private void byStableMarriageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SegmentationComparisonDialog dialog = new SegmentationComparisonDialog();
            dialog.Measure = new TxEstudioKernel.SegmentationSimilarity.StableMarriageBaseSimilarity();
            env.ShowDialog(dialog);
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form child in MdiChildren)
                child.Close();
        }

        private void universalImageQualityIndexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QualityDialog dialog = new QualityDialog();
            dialog.QualityAssessment = new TxEstudioKernel.Operators.ImageQualityAssessment.UIQ();
            env.ShowDialog(dialog);
        }

        private void contentBasedImageQualityMetricToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QualityDialog dialog = new QualityDialog();
            dialog.QualityAssessment = new TxEstudioKernel.Operators.ImageQualityAssessment.CBM();
            env.ShowDialog(dialog);
        }

        private void structuralSimilarityBasedImageQualityAssesmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QualityDialog dialog = new QualityDialog();
            dialog.QualityAssessment = new TxEstudioKernel.Operators.ImageQualityAssessment.MSSIM();
            env.ShowDialog(dialog);
        }

      
        #region ContourPaint

        private void tsb_cp_Click(object sender, EventArgs e)
        {
            ImageHolderForm reference=(this.ActiveMdiChild as ImageHolderForm);
            if (reference != null)
            {
               //// reference.Size = new Size((this.Size.Width - 80) / 2, (this.Size.Height - 120));
               //// reference.Location = new Point(0, 0);
               
               // ContourPaintForm cpf = new ContourPaintForm();               
               //// cpf.ZoomLevel = reference.ZoomLevel;
               //// cpf.Bitmap = new Bitmap(reference.Bitmap.Width, reference.Bitmap.Height);             
               // //cpf.Image = new TxImage(new Bitmap(reference.Bitmap.Width, reference.Bitmap.Height));
               //// cpf.Bitmap = reference.Bitmap;
               // cpf.BackImage = reference.Image.ToGrayScale().ToBitamp();
               // env.OpenWindow(cpf);

                Combination_Methods.CombinationMethodDialog dialog = new TxEstudioApplication.Combination_Methods.CombinationMethodDialog();
                env.ShowDialog(dialog);
                
                
               // cpf.Size = new Size((this.Size.Width - 80) / 2, (this.Size.Height - 120));
               // cpf.Location = new Point((this.Size.Width - 80) / 2 + 5, 0);
            }
        }

        #endregion

        #region Help

        string helpPath = "HelpTxEstudioVers2.chm";
        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, helpPath, HelpNavigator.TableOfContents);         
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, helpPath, HelpNavigator.Find);

        }
        private void indexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, helpPath, HelpNavigator.Index);
           
        }

        #endregion

        private void b_yadi_Click(object sender, EventArgs e)
        {
            DialogoYadi yadi = new DialogoYadi();
            yadi.Owner = this;
            yadi.ShowDialog();
        }

        private void brihgtContrastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageHolderForm target = (this.ActiveMdiChild as ImageHolderForm);
            if (target != null)
            {
                BrightContrastDialog bc = new BrightContrastDialog();
                bc.Image = target.Image;
                env.ShowDialog(bc);
            }
        }

        private void histogramEqualizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TxHistogramEqualization target = new TxHistogramEqualization();
            OneBandDialog dialog = new OneBandDialog();
            dialog.SetEnvironment(env);
            dialog.ShowDialogWith(this, target);
        }

        private void principalComponentAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TxPCA target = new TxPCA();

            ShowRes res = new ShowRes(ShowPCAResult);
            target.ResultViewer = res;
            MultiBandDialog dialog = new MultiBandDialog();
            dialog.SetEnvironment(env);
            dialog.ShowDialogWith(this, target);
        }

        private void sToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Feature_Selection fs = new Feature_Selection();
            env.ShowDialog(fs);
        }


        private void ShowPCAResult(TxPCA pca)
        {
            PCAResult res = new PCAResult(pca);
            env.OpenWindow(res);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveRecentFiles();
        }

        private void theoreticalQualityMeasureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SegmentationComparisonDialog dialog = new SegmentationComparisonDialog();
            dialog.Measure = new TxEstudioKernel.SegmentationSimilarity.TQM();
            env.ShowDialog(dialog);
        }

       
       

       







    }
}
