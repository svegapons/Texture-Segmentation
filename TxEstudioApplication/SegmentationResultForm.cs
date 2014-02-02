using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioApplication.Interfaces;
using TxEstudioKernel;
using System.Drawing.Imaging;

namespace TxEstudioApplication
{

    public struct ClassDisplaySetting
    {
        public Color color;
        public bool showOriginal;
        public bool visible;
    }

    public partial class SegmentationResultForm : TxAppForm , IImageExposer, IZoomManager, ISaveManager, IPropertiesExposer, IClipboardManager, IPrintManager
    {
        public SegmentationResultForm()
        {
            InitializeComponent();
        }

        public void SetData(Bitmap original, TxSegmentationResult segmentation)
        {
            this.original = original;
            this.segmentation = segmentation;
            pictureControl.Bitmap = new Bitmap(segmentation.Width, segmentation.Height, PixelFormat.Format24bppRgb);
            displaySettigns = new ClassDisplaySetting[segmentation.Classes];
            Color[] colors = TxSegmentationResult.GetDefaultMapping(segmentation.Classes);
            for (int i = 0; i < segmentation.Classes; i++)
            {
                displaySettigns[i] = new ClassDisplaySetting();
                displaySettigns[i].color = colors[i];
                displaySettigns[i].visible = true;
            }
            RefereshSettings();
            Size = new Size(segmentation.Width + 35, segmentation.Height + 35);//Para que la ventana tome el tamaño de la imagen
        }

        Bitmap original;

        public Bitmap Original
        {
            get { return original; }
        }
        TxSegmentationResult segmentation;

        public TxSegmentationResult Segmentation
        {
            get { return segmentation; }
        }
        ClassDisplaySetting[] displaySettigns;
        Color backGround = Color.White;

        public Color BackGround
        {
            get { return backGround; }
            set
            {
                backGround = value;
                RefereshSettings();//Quizas poner un metodo para cambiar el backGround
                pictureControl.Refresh();
            }
        } 

        public ClassDisplaySetting[] DisplaySettigns
        {
            get { return displaySettigns; }
            set 
            { 
                displaySettigns = value; 
                RefereshSettings(); 
                pictureControl.Refresh(); 
            }
        }

        public void ChangeSetting(int classIndex, ClassDisplaySetting setting)
        {
            displaySettigns[classIndex] = setting;
            if (setting.visible)
            {
                if (setting.showOriginal)
                    SetToOriginal(classIndex);
                else
                    SetToColor(classIndex, setting.color);
            }
            else
                SetToColor(classIndex, backGround);
            pictureControl.Refresh();
        }

        protected void SetToOriginal(int classIndex)
        {
            BitmapData data = pictureControl.Bitmap.LockBits(new Rectangle(0, 0, pictureControl.Bitmap.Width, pictureControl.Bitmap.Height),
                                                             ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData originalData = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            
            int offset = data.Stride - 3*data.Width;
            int width = data.Width;
            int height = data.Height;
            unsafe
            {
                byte* current = (byte*)data.Scan0;
                byte* origCurrent = (byte*)originalData.Scan0;

                for (int i = 0; i < height; i++, current += offset, origCurrent+=offset)
                {
                    for (int j = 0; j < width; j++, current+=3, origCurrent+=3)
                    {
                        if (segmentation[i,j] == classIndex)
                        {
                            current[0] = origCurrent[0];
                            current[1] = origCurrent[1];
                            current[2] = origCurrent[2];
                        }
                    }
                }
            }
            pictureControl.Bitmap.UnlockBits(data);
            original.UnlockBits(originalData);
        }

        protected void RefereshSettings()
        {
            BitmapData data = pictureControl.Bitmap.LockBits(new Rectangle(0, 0, pictureControl.Bitmap.Width, pictureControl.Bitmap.Height),
                                                             ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData originalData = original.LockBits(new Rectangle(0, 0, original.Width, original.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            int offset = data.Stride - 3*data.Width;
            int width = data.Width;
            int height = data.Height;
            int classIndex = 0;
            Color color;
            unsafe
            {
                byte* current = (byte*)data.Scan0;
                byte* origCurrent = (byte*)originalData.Scan0;

                for (int i = 0; i < height; i++, current += offset, origCurrent += offset)
                {
                    for (int j = 0; j < width; j++, current += 3, origCurrent += 3)
                    {
                        classIndex = segmentation[i,j];
                        if (!displaySettigns[classIndex].visible)
                        {
                            current[0] = backGround.B;
                            current[1] = backGround.G;
                            current[2] = backGround.R;
                        }
                        else if (displaySettigns[classIndex].showOriginal)
                        {
                            current[0] = origCurrent[0];
                            current[1] = origCurrent[1];
                            current[2] = origCurrent[2];
                        }
                        else
                        {
                            color = displaySettigns[classIndex].color;
                            current[0] = color.B;
                            current[1] = color.G;
                            current[2] = color.R;
                        }
                    }
                }
            }
            pictureControl.Bitmap.UnlockBits(data);
            original.UnlockBits(originalData);
        }

        protected void SetToColor(int classIndex, Color color)
        {
            BitmapData data = pictureControl.Bitmap.LockBits(new Rectangle(0, 0, pictureControl.Bitmap.Width, pictureControl.Bitmap.Height),
                                                             ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int offset = data.Stride - 3*data.Width;
            int width = data.Width;
            int height = data.Height;
            unsafe
            {
                byte* current = (byte*)data.Scan0;
                for (int i = 0; i < height; i++, current += offset)
                {
                    for (int j = 0; j < width; j++, current += 3)
                    {
                        if (segmentation[i,j] == classIndex)
                        {
                            current[0] = color.B;
                            current[1] = color.G;
                            current[2] = color.R;
                        }
                    }
                }
            }
            pictureControl.Bitmap.UnlockBits(data);
        }


        #region IImageExposer Members

        public TxEstudioKernel.TxImage Image
        {
            get
            {
                return new TxImage(pictureControl.Bitmap);
            }
            set
            {
                throw new InvalidOperationException("Can't asing a TxImage here");
            }
        }

        public Bitmap Bitmap
        {
            get
            {
                return pictureControl.Bitmap;
            }
            set
            {
                pictureControl.Bitmap = value;
            }
        }

        public string ImageName
        {
            
            get
            {
                return Text;
            }
            set
            {
                Text = value;
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

        public int ZoomLevel
        {
            get
            {
                return (int)(pictureControl.Zoom * 100f);
            }
            set
            {
                pictureControl.Zoom = value / 100f;
            }
        }

        #endregion

        #region ISaveManager Members

        public void Save()
        {
            SaveAs();
        }

        public void SaveAs()
        {
            env.SaveImage(new TxImage(pictureControl.Bitmap), ImageName);
        }

        #endregion

        #region IPropertiesExposer Members

        public object Properties
        {
            get { return new SegmentationProperties(ImageName, segmentation.Classes, segmentation.Width, segmentation.Height); }
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
            return false;
        }

        public void GetFromClipboard()
        {
            
        }

        #endregion


        class SegmentationProperties
        {
            string imageName;

            public string ImageName
            {
                get 
                {
                    return imageName; 
                }
            }
            int numberOfClasses;

            public int Classes
            {
                get { return numberOfClasses; }
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
            
            public SegmentationProperties(string name, int classes, int width, int height)
            {
                imageName = name;
                numberOfClasses = classes;
                this.width = width;
                this.height = height;
                
            }
        }

        private void pictureControl_MouseOverImage(object sender, MouseOverImageEventArgs e)
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
            int destinationWidth = Math.Min(e.MarginBounds.Width, pictureControl.Bitmap .Width);
            int destinationHeight = Math.Min(e.MarginBounds.Height, pictureControl.Bitmap.Height);
            e.Graphics.DrawImage(pictureControl.Bitmap, new Rectangle(e.MarginBounds.Left, e.MarginBounds.Top, destinationWidth, destinationHeight));
        }

        #endregion
    }
}