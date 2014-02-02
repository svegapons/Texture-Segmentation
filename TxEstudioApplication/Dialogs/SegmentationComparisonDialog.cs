using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioApplication.Interfaces;
using TxEstudioKernel.SegmentationSimilarity;

namespace TxEstudioApplication.Dialogs
{
    public partial class SegmentationComparisonDialog : TxAppForm 
    {
        public SegmentationComparisonDialog()
        {
            InitializeComponent();
            segmentationBox1.ExposerFilter = new TxEstudioApplication.Custom_Controls.FilterExposerDelegate(SegmentationFilter);
            segmentationBox2.ExposerFilter = new TxEstudioApplication.Custom_Controls.FilterExposerDelegate(SegmentationAndSizeFilter);
        }

        public override void SetEnvironment(Environment env)
        {
            base.SetEnvironment(env);
            segmentationBox1.LoadEnvironment(env);
            segmentationBox2.LoadEnvironment(env);
        }

        private bool SegmentationFilter(IImageExposer imageExposer)
        {
            return imageExposer is SegmentationResultForm;
        }

        private bool SegmentationAndSizeFilter(IImageExposer imageExposer)
        {
            SegmentationResultForm first = segmentationBox1.SelectedExposer as SegmentationResultForm;
            SegmentationResultForm second = imageExposer as SegmentationResultForm;
            return second != null && second.Segmentation.Height == first.Segmentation.Height && second.Segmentation.Width == first.Segmentation.Width;
        }

        private TxSegmentationSimilarityMeasure measure = new HungarianBasedSimilarity();

        public TxSegmentationSimilarityMeasure Measure
        {
            get { return measure; }
            set 
            {
                if (value == null)
                    throw new ArgumentNullException();
                measure = value; 
            }
        }

        private void calculateButton_Click(object sender, EventArgs e)
        {
            if (segmentationBox1.SelectedExposer == null || segmentationBox2.SelectedExposer == null)
                return;
            TxEstudioKernel.TxSegmentationResult segmentationOne = (segmentationBox1.SelectedExposer as SegmentationResultForm).Segmentation;
            TxEstudioKernel.TxSegmentationResult segmentationTwo = (segmentationBox2.SelectedExposer as SegmentationResultForm).Segmentation;
            coincidenceMasureLabel.Text = measure.GetSimilarity(segmentationOne, segmentationTwo).ToString();
            totalCoincidenceLabel.Text = measure.LastTotalCoincidencePercent.ToString();

            totalCoincidenceMapBox.Image = measure.LastCoincidenceMap.ToBitamp();
            coincidenceByClassList.Items.Clear();
            if (segmentationOne.Classes <= segmentationTwo.Classes)
            {

                for (int i = 0; i < segmentationOne.Classes; i++)
                    coincidenceByClassList.Items.Add(new ClassCoincidence((segmentationBox1.SelectedExposer as SegmentationResultForm).DisplaySettigns[i].color,
                                                                            (segmentationBox2.SelectedExposer as SegmentationResultForm).DisplaySettigns[measure.LastMatch[i]].color,
                                                                            measure.GetLastCoincidenceOnClass(i)));
            }
            else
            {
                for (int i = 0; i < segmentationTwo.Classes; i++)
                    coincidenceByClassList.Items.Add(new ClassCoincidence((segmentationBox1.SelectedExposer as SegmentationResultForm).DisplaySettigns[measure.LastMatch[i]].color,
                                                                            (segmentationBox2.SelectedExposer as SegmentationResultForm).DisplaySettigns[i].color,
                                                                            measure.GetLastCoincidenceOnClass(i)));
            }
            
            
            

            



        }

        struct ClassCoincidence
        {
            public Color first;
            public Color second;
            public double coincidence;

            public ClassCoincidence(Color first, Color second, double coincidence)
            {
                this.first = first;
                this.second = second;
                this.coincidence = coincidence;
            }
        }

        private void coincidenceByClassList_DrawItem(object sender, DrawItemEventArgs e)
        {
            ClassCoincidence current =  (ClassCoincidence) coincidenceByClassList.Items[e.Index];
            e.Graphics.FillEllipse(new SolidBrush(current.first), e.Bounds.Left + 3, e.Bounds.Top + 3, e.Bounds.Height - 6, e.Bounds.Height - 6);
            e.Graphics.FillEllipse(new SolidBrush(current.second), e.Bounds.Left + e.Bounds.Height, e.Bounds.Top + 3, e.Bounds.Height - 6, e.Bounds.Height - 6);
            e.Graphics.DrawString(current.coincidence.ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds.Left + 2 * e.Bounds.Height,e.Bounds.Top + 3);
        }
    }
}