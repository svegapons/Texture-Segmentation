using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [DigitalFilter]
    [Algorithm("Isotropic Filter", "Isotropic Filter to obtain the image invariant illumination.")]
    [Abbreviation("norm_isot", "Constante")]
    public class IsotropicFilter : TxOneBand
    {
        TxMatrix mascara = new TxMatrix(3, 3);


        float constante = 1;
        [Parameter("Smooth", "This parameter controls the relative importance of smoothness")]
        [RealInRange(0, float.MaxValue)]
        public float Constante
        {
            get
            {
                return constante;
            }
            set
            {
                constante = value;
                //Inicializando la mascara
                mascara[0, 0] = mascara[2, 0] = mascara[0, 2] = mascara[2, 2] = 0;
                mascara[1, 0] = mascara[0, 1] = mascara[2, 1] = mascara[1, 2] = -1*constante;
                mascara[1, 1] = 1 + 4 * constante;
            }
        }

        public override TxImage Process(TxImage input)
        {
            TxImage imagen = input.ToGrayScale();
            TxImage result = imagen.Convolve(mascara);

            return NormalizaImagen(result);

        }

        protected TxImage NormalizaImagen(TxImage imagen)
        {
            float min = 10000000000; 
            float max = -1000000000;
            float k;
            float color;


            for (int j = 5; j < imagen.Height - 5; j++)
                for (int i = 5; i < imagen.Width - 5; i++)
                {
                    color = imagen[i, j, ColorChannel.Red];
                    if (color < min) { min = color; }
                    if (color > max) { max = color; }
                }

            TxImage result = new TxImage(imagen.Width, imagen.Height, TxImageFormat.GrayScale);

            // Normalizando
            k = (255) / (max - min);

            for (int j = 0; j < imagen.Height; j++)
            {
                for (int i = 0; i < imagen.Width; i++)
                {
                    color = k * (imagen[i, j, ColorChannel.Red] - min);
                    result[i, j] = (byte)color;
                }
            }
            

            return result;

        }
    }

    
        
    }

