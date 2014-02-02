using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators.ImageQualityAssessment
{
    [Algorithm("CBM", "Calculates the CBM index for image quality assesment. ")]
    [Abbreviation("cbm", "PesoBordes", "PesoTexturas", "PesoPlano", "XSize", "YSize", "K1", "K2", "Alfa", "Beta", "Gamma")]
    public class CBM:ImageQuality
    {
        //Para particionar la imagen
        ArrayList SSIMbordes;
        ArrayList SSIMplano ;
        ArrayList SSIMtexturas ;
        #region Parametros del SSIM
        float k1 = 0.1f;
        [Parameter("Constante 1", "Constante para evitar problemas de inestabilidad ")]
        [RealInRange(0, 1)]
        public float K1
        {
            get { return k1; }
            set { k1 = value; }
        }
        float k2 = 0.01f;

        [Parameter("Constante 2", "Constante para evitar problemas de inestabilidad ")]
        [RealInRange(0, 1)]
        public float K2
        {
            get { return k2; }
            set { k2 = value; }
        }

        float alfa = 1;

        [Parameter("Alfa", "Constante para evitar problemas de inestabilidad ")]
        [RealInRange(0, 1)]
        public float Alfa
        {
            get { return alfa; }
            set { alfa = value; }
        }
        float beta = 1;
        [Parameter("Beta", "Constante para evitar problemas de inestabilidad ")]
        [RealInRange(0, 1)]
        public float Beta
        {
            get { return beta; }
            set { beta = value; }
        }
        float gamma = 1;
        [Parameter("Gamma", "Constante para evitar problemas de inestabilidad ")]
        [RealInRange(0, 1)]
        public float Gamma
        {
            get { return gamma; }
            set { gamma = value; }
        }
        #endregion

        float pesoBordes = 0.5f;

        [Parameter("w1", "Edge's weight")]
        [RealInRange(0, 1)]
        public float PesoBordes
        {
            get { return pesoBordes; }
            set { pesoBordes = value; }
        }
        float pesoTexturas = 0.25f;

        [Parameter("w2", "Texture's weigth")]
        [RealInRange(0, 1)]
        public float PesoTexturas
        {
            get { return pesoTexturas; }
            set { pesoTexturas = value; }
        }
        float pesoPlano = 0.25f;

        [Parameter("w3", "Flat region's weight")]
        [RealInRange(0, 1)]
        public float PesoPlano
        {
            get { return pesoPlano; }
            set { pesoPlano = value; }
        }

        public TxImage Plano
        {
            get { return plano.ToImage(); }
        }

        public TxImage Textura
        {
            get { return textura.ToImage(); }
        }

        public TxImage Bordes
        {
            get { return bordes.ToImage(); }
        }

        TxMatrix SSIM;

        TxMatrix bordes;
        TxMatrix plano;
        TxMatrix textura;

        protected void RegionClasification(TxImage imagen1, TxImage imagen2)
        {
            float max=0;
            float t1 = 0;
            float t2 = 0;

            mapa = new TxMatrix(imagen1.Height, imagen1.Width);
            bordes = new TxMatrix(imagen1.Height, imagen1.Width);
            plano = new TxMatrix(imagen1.Height, imagen1.Width);
            textura = new TxMatrix(imagen1.Height, imagen1.Width);

            

            #region Hallando SSIM de las imagenes

            MSSIM ssim = new MSSIM();
            ssim.Error(imagen1, imagen2);
            SSIM = ssim.Mapa;

            #endregion

            #region Computar Gradiente de las dos imagenes y buscando los umbrales 
            Sobel1 sobel = new Sobel1();
            sobel.Umbral = 0;

            TxImage sobel1 = sobel.Process(imagen1);
            TxImage sobel2 = sobel.Process(imagen2);

            for(int i=0;i<sobel1.Width;i++)
                for (int j = 0; j < sobel1.Height; j++)
                {
                    if (max < sobel1[i, j]) max = sobel1[i, j];
                }

            t1 = (float)0.12 * max;
            t2 = (float)0.06 * max;
            #endregion

            #region Particionando las regiones

            for (int i = 0; i < sobel1.Width; i++)
                for (int j = 0; j < sobel1.Height; j++)
                {
                    if ((sobel1[i, j] > t1) || (sobel2[i, j] > t1))
                    {
                        mapa[j, i] = 255;//borde
                        bordes[j, i] = 255;
                        SSIMbordes.Add(SSIM[j, i]);

                    }
                        
                    else 
                        if ((sobel1[i, j] < t2) && (sobel2[i,j] <= t1))
                        {
                            mapa[j, i] = 100;//plana
                            plano[j, i] = 255;
                            SSIMplano.Add(SSIM[j, i]);
                        }
                       
                        else 
                            if (((t1 >= sobel1[i, j]) && (sobel1[i, j] >= t2)) && (sobel2[i, j] <= t1)) 
                            {
                                mapa[j, i] = 3;//textura
                                textura[j, i] = 255;
                                SSIMtexturas.Add(SSIM[j, i]);
                            }
                    
                            else 
                                mapa[j, i] = 0;//sin clasificacion
                }

          

            #endregion

        }

        protected float HallandoSSIM(ArrayList region)
        {
           double max = 0;
           double valor = 0;

            region.Sort();

            //Integral de Sugeno
            for (int i = 0; i < region.Count; i++)
            {
                valor = Math.Min((float)region[i],((float)(region.Count-1 - i)) /(float) region.Count);
                max = Math.Max(max, valor);
                
            }
            return (float)max;
            
        }

        public override double Error(TxImage imagen1, TxImage imagen2)
        {

            SSIMbordes = new ArrayList();
            SSIMplano = new ArrayList();
            SSIMtexturas = new ArrayList();

            RegionClasification(imagen1, imagen2);

            return pesoBordes * HallandoSSIM(SSIMbordes) + pesoPlano * HallandoSSIM(SSIMplano) + PesoTexturas * HallandoSSIM(SSIMtexturas);

        }
    }
}
