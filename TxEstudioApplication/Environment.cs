using System;
using System.Windows.Forms;
using TxEstudioKernel;
using System.IO;
using System.Collections.Generic;

namespace TxEstudioApplication
{
    /// <summary>
    /// Esta clase es la intermediaria entre la aplicacion y su  ventana principal 
    /// con las ventanas hijas y en un futuro(cercano :-)) los add-ons.
    /// </summary>
    public partial class Environment
    {

        string[][] filters;

        public Environment(MainForm mainForm)
        {
            this.mainForm = mainForm;

            filters = new string[6][];
            filters[0] = new string[] { ".bmp", " .jpg", ".jpeg", ".gif", ".png", ".dcm" };
            filters[1] = new string[] { ".bmp" };
            filters[2] = new string[] { ".jpg", ".jpeg" };
            filters[3] = new string[] { ".png" };
            filters[4] = new string[] { ".gif" };
            filters[5] = new string[] { ".dcm" };

            //Initializing dialogs
            openImageDialog.Filter = "Image files|*.bmp;*.jpg;*.jpeg;*.gif;*.png;*dcm|Bitmap|*.bmp|JPEG|*.jpeg;*.jpg|" +
            "Portable Network Graphics|*.png|GIF|*.gif|DICOM(Not supported yet)|*.dcm|All files|*.*";
            openImageDialog.Title = "Open image";
            saveImageDialog.Title = "Save image as";
            saveImageDialog.Filter = openImageDialog.Filter;
            saveImageDialog.SupportMultiDottedExtensions = true;
            saveImageDialog.AddExtension = false;
            saveImageDialog.FileOk += new System.ComponentModel.CancelEventHandler(saveImageDialog_FileOk);
            saveImageDialog.OverwritePrompt = false;
            openImageDialog.RestoreDirectory = saveImageDialog.RestoreDirectory = true;

            if (Properties.Settings.Default.UseDatabase)
                dbManager = new DataBaseManager.DBManager();
        }

        public List<AlgorithmHolder> GetAllSegmentationOperators()
        {
            List<AlgorithmHolder> result = new List<AlgorithmHolder>(segmentersCollection.Count);
            foreach (AlgorithmHolder alg in segmentersCollection.Values)
                result.Add(alg);
            return result;
        }

        public List<AlgorithmHolder> GetAllTextureEdgeOperators()
        {
            List<AlgorithmHolder> result = new List<AlgorithmHolder>(textureEdgeCollection.Count);
            foreach (AlgorithmHolder alg in textureEdgeCollection.Values)
                result.Add(alg);
            return result;
        }

        void saveImageDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string extension = Path.GetExtension(saveImageDialog.FileName);
            int index = saveImageDialog.FilterIndex;
            if (index <= filters.Length)
            {
                if(!Array.Exists<string>(filters[index], new Predicate<string>(delegate(string target){return target==extension;})))
                {
                    saveImageDialog.FileName = saveImageDialog.FileName + "." +filters[index-1][0];
                }
            }
            if (File.Exists(saveImageDialog.FileName))
                if (DialogResult.No == MessageBox.Show(mainForm, "File " + saveImageDialog.FileName + " already exists. Do you want to overwrite it?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    e.Cancel = true;
        }

        private MainForm mainForm;
        /// <summary>
        /// Retorna la instancia de la ventana prioncipal de TxEstudio.
        /// </summary>
        public MainForm MainForm
        {
            get { return mainForm; }
        }

        #region Images IO related methods

        OpenFileDialog openImageDialog = new OpenFileDialog();
        SaveFileDialog saveImageDialog = new SaveFileDialog();

        /// <summary>
        /// Despliega el dialogo de abrir el archivo de la imagen y se encarga de su semantica.
        /// </summary>
        /// <param name="path">Se almacenara la ruta del archivo abierto.</param>
        /// <returns>La imagen almacenada en el archivo seleccionado o null en caso de error.</returns>
        public TxImage OpenImage(out string path)
        {
            path = "";
            try
            {
                TxImage result = null;
                if (openImageDialog.ShowDialog(mainForm) == DialogResult.OK)
                {
                    result = TxImage.LoadImageFrom(openImageDialog.FileName);
                    path = openImageDialog.FileName; 
                    return result;
                }
                return null;
            }
            catch (Exception exc)
            {
                ShowError("An exception has ocurred while loading the image.\n Message: " + exc.Message);
                return null;
            }
        }

        /// <summary>
        /// Despliega el dialogo de abrir el archivo de la imagen y se encarga de su semantica.
        /// </summary>
        /// <returns>La imagen almacenada en el archivo seleccionado o null en caso de error.</returns>
        public TxImage OpenImage()
        {
            string path;
            return OpenImage(out path);
        }

        public string[] OpenMultipleImages()
        {
            string[] result = new string[] { };
            openImageDialog.Multiselect = true;
            if (openImageDialog.ShowDialog(mainForm) == DialogResult.OK)
                result = openImageDialog.FileNames;
            openImageDialog.Multiselect = false;
            return result;
        }

        /// <summary>
        /// Despliega el dialogo correspondiente a la accion de salvar la imagen.
        /// </summary>
        /// <param name="image">La imagen a salvar.</param>
        /// <returns>La ruta del archivo en que se salvo la imagen o la cadena vacia en caso de error.</returns>
        public string SaveImage(TxImage image)
        {

            try
            {
                if (saveImageDialog.ShowDialog(mainForm) == DialogResult.OK)
                {
                    image.Save(saveImageDialog.FileName);
                    return saveImageDialog.FileName;
                }
                return "";
            }
            catch (Exception exc)
            {
                ShowError("An exception has ocurred while saving the image.\n Message: " + exc.Message);
                return "";
            }
        }

        public string SaveImage(TxImage image, string proposedFileName)
        {
            saveImageDialog.FileName = proposedFileName;
            return SaveImage(image);
        }

        #endregion

        #region MessageBox related code
        public void ShowError(string errorMessage)
        {
            MessageBox.Show(mainForm, errorMessage, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowInfo(string infoMessage)
        {
            MessageBox.Show(mainForm, infoMessage, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public DialogResult ShowWarning(string warningMessage)
        {
            return MessageBox.Show(mainForm, warningMessage, "Warning!",MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
        }

        public DialogResult ShowQuestion(string caption, string questionText, MessageBoxButtons buttons)
        {
           return  MessageBox.Show(mainForm, questionText, caption, buttons, MessageBoxIcon.Question);
        }
        #endregion

        #region Windows and dialogs related code

        
        /// <summary>
        /// Asigna el environment actual y muestra la forma dada como hija mdi de la ventana principal.
        /// </summary>
        /// <param name="form"></param>
        public void OpenWindow(TxAppForm form)
        {
            if (form != null && !form.IsMdiContainer)
            {
                if(form is TxEstudioApplication.Interfaces.IImageExposer)
                    ((TxEstudioApplication.Interfaces.IImageExposer)form).MouseOverImage+=new MoveOverImageEventHandler(mainForm.DisplayCoordinates);
                form.SetEnvironment(this);
                form.MdiParent = mainForm;
                form.Show();
            }
            else
                //Debug purpose
                ShowError("Window can't be null or MdiContainer.");
        }

        /// <summary>
        /// Muestra todos los hijos mdi de la ventana principal.
        /// </summary>
        public Form[] OpenedForms
        {
            get
            {
                return mainForm.MdiChildren;
            }
        }

        /// <summary>
        /// Asigna el environment y muestra el dialogo con la ventana principal como owner.
        /// </summary>
        public DialogResult ShowDialog(TxAppForm dialog)
        {
            if (dialog != null)
            {
                dialog.SetEnvironment(this);
                return dialog.ShowDialog(mainForm);
            }
            //Debug purpose
            ShowError("Dialog can't be null");
            return DialogResult.Abort;
        }

        /// <summary>
        /// Muestra el dialogo con la ventana principal como owner.
        /// </summary>
        public DialogResult ShowDialog(Form dialog)
        {
            if (dialog != null)
                return dialog.ShowDialog();
            //Debug purpose
            ShowError("Dialog can't be null");
            return DialogResult.Abort;
        }


        #endregion

    }
}
