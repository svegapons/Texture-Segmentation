using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioApplication.Interfaces;
using TxEstudioKernel;
using System.Drawing.Drawing2D;
using System.IO;

namespace TxEstudioApplication
{
    public partial class ImageHolderForm : TxAppForm, IImageExposer, ISaveManager, IClipboardManager, IZoomManager, IPropertiesExposer, IPrintManager
    {
        public ImageHolderForm()
        {
            InitializeComponent();
        }


        #region ISaveManager Members

        bool isImageSaved = false;

        /// <summary>
        /// Contendra en todo momento la ruta a la carpeta.
        /// </summary>
        string path = "";

        public void Save()
        {
            if (!isImageSaved)
            {
                if (path == "")//No se ha salvado nunca
                {
                    path = env.SaveImage(image, imageName);//Garantizar que nunca sea ""
                    if (path != "")
                    {
                        imageName = Path.GetFileNameWithoutExtension(path);
                        path = Path.GetDirectoryName(path);
                        isImageSaved = true;
                        this.Text = imageName;
                    }
                }
                else
                {
                    //TODO:Tratamiento de excepciones
                    //Siempre salvamos la imagen como bmp
                    string realPath = path + "\\" + imageName + ".bmp";
                    image.Save(realPath);
                    isImageSaved = true;
                    this.Text = imageName;
                }

            }

        }

        public void SaveAs()
        {
            string savedPath = env.SaveImage(image, imageName);
            if (savedPath != "")//No problem on saving
            {
                path = Path.GetDirectoryName(savedPath);
                //TODO: Si uno salva la imagen con determinada extension
                //seria deseable salvar despues siempre con esta extension 
                // a menos que se diga lo contrario o siempre salvamos por defecto con bmp?
                imageName = Path.GetFileNameWithoutExtension(savedPath);
                isImageSaved = true;
                this.Text = imageName;
            }
         
        }

        #endregion

        #region IClipboardManager Members

        public bool CanCopyToClipboard()
        {
            return true;
        }

        public void CopyToClipboard()
        {
            Clipboard.SetImage(pictureControl.Bitmap);
        }

        public bool CanGetFromClipboard()
        {
            return true;
        }

        public void GetFromClipboard()
        {
            //pass
            if (Clipboard.ContainsImage())
            {
                this.Bitmap = (Bitmap)Clipboard.GetImage();
            }
        }

        #endregion

        #region IImageExposer Members

        TxImage image;
        public TxEstudioKernel.TxImage Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                //bitmap = image.ToBitamp();
                pictureControl.Bitmap = image.ToBitamp();
                //AdjustScrolls();
                isImageSaved = false;
                // this.ClientSize = image.Size;//?
                if (!Text.EndsWith("*"))
                    this.Text += '*';
            }
        }



        //Bitmap bitmap;
        public Bitmap Bitmap
        {
            get
            {
                //return bitmap;
                return pictureControl.Bitmap;
            }
            set
            {
                pictureControl.Bitmap = value;
                //bitmap = value;
                image = new TxImage(value);
                //AdjustScrolls();
                isImageSaved = false;
                //this.ClientSize = image.Size;//?
                if (!Text.EndsWith("*"))
                    this.Text += '*';
            }
        }
        string imageName;
        public string ImageName
        {
            get { return imageName; }
            set
            {
                imageName = value;
                this.Text = value;
                if (!isImageSaved)
                    this.Text += "*";
            }
        }
        public event MoveOverImageEventHandler MouseOverImage;
        protected void OnMouseOverImage(object sender, MouseOverImageEventArgs e)
        {
            if (MouseOverImage != null)
            {
                MouseOverImage(this, e);
            }
        }


        #endregion

        #region IZoomManager Members
        //float zoom = 1;
        public int ZoomLevel
        {
            get
            {
                return (int)(pictureControl.Zoom * 100.0f);
            }
            set
            {
                pictureControl.Zoom = value / 100.0f;
                // AdjustScrolls();
            }
        }

        #endregion

        #region IPropertiesExposer Members

        class ImageProperties
        {
            string path, name, bpp;

            public ImageProperties(ImageHolderForm form)
            {
                name = form.ImageName;
                bpp = (form.image.ImageFormat == TxImageFormat.GrayScale) ? "8bpp" : "24bpp";
                path = form.path;
                width = form.image.Width;
                height = form.image.Height;
            }

            public string BPP
            {
                get { return bpp; }
            }

            public string Name
            {
                get { return name; }
            }

            public string Path
            {
                get { return path; }
            }
            int width, height;

            public int Height
            {
                get { return height; }
            }

            public int Width
            {
                get { return width; }
            }
        }

        public object Properties
        {
            get { return new ImageProperties(this); }
        }

        #endregion

        public static void OpenImage(string path, Environment env)
        {
            TxImage image = TxImage.LoadImageFrom(path);
            if (image == null)
            {
                env.ShowError(string.Format("{0} could not be loaded.", path));
                return;
            }
            ImageHolderForm newForm = new ImageHolderForm();
            newForm.Image = image;
            newForm.path = Path.GetDirectoryName(path);
            newForm.isImageSaved = true;
            //newForm.ImageName = Path.GetFileName(path);
            //Quedarse con la extension aqui provoca que despues, el dialogo para salvar la imagen elimine todo lo que quede detras
            newForm.ImageName = Path.GetFileNameWithoutExtension(path);
            newForm.Size = new Size(image.Size.Width + 35, image.Size.Height + 35);//image.Size;

            env.MainForm.PushRecentFileMenuItem(Path.GetFileName(path), Path.GetDirectoryName(path) + "\\");

            env.OpenWindow(newForm);
        }

        public static void OpenImage(Environment env)
        {
            string path = "";
            TxImage image = env.OpenImage(out path);
            if (image != null)
            {
                ImageHolderForm newForm = new ImageHolderForm();
                newForm.Image = image;
                newForm.path = Path.GetDirectoryName(path);
                newForm.isImageSaved = true;
                newForm.ImageName = Path.GetFileName(path);//Aqui sim me interesa quedarme con la extension para saber de donde vino
                newForm.Size = new Size(image.Size.Width + 35, image.Size.Height + 35);//image.Size;
                env.OpenWindow(newForm);
            }


        }

        

        protected void pictureControl_MouseOverImage(object sender, MouseOverImageEventArgs e)
        {
            OnMouseOverImage(sender, e);
        }

        #region IPrintManager Members

        public void BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //pass
        }

        public void EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //pass
        }

        public void QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            //pass
        }

        public void PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //Esto no es realmente lo que quiero pero es aceptable
            int destinationWidth = Math.Min(e.MarginBounds.Width, image.Width);
            int destinationHeight = Math.Min(e.MarginBounds.Height, image.Height);
            e.Graphics.DrawImage(pictureControl.Bitmap, new Rectangle(e.MarginBounds.Left, e.MarginBounds.Top, destinationWidth, destinationHeight));
        }

        #endregion

       
        private void ImageHolderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && Text.EndsWith("*"))
            {
                DialogResult dg = MessageBox.Show("Save changes to "+this.Text, "TxEstudio Texture Image Analysis", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (dg == DialogResult.Yes)
                {
                    string savedPath = env.SaveImage(image, imageName);
                    if (savedPath != "")//No problem on saving
                    {
                        path = Path.GetDirectoryName(savedPath);
                        //TODO: Si uno salva la imagen con determinada extension
                        //seria deseable salvar despues siempre con esta extension 
                        // a menos que se diga lo contrario o siempre salvamos por defecto con bmp?
                        imageName = Path.GetFileNameWithoutExtension(savedPath);
                        isImageSaved = true;
                        this.Text = imageName;
                    }
                    else
                        e.Cancel = true;
                }
                else if (dg == DialogResult.No)
                    return;
                else
                    e.Cancel = true;
            }
        }

    }
}