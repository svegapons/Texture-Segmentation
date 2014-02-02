using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using TxEstudioKernel;
using TxEstudioKernel.Exceptions;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.VisualElements
{
    public partial class AlgorithmViewer : UserControl
    {
        public AlgorithmViewer()
        {
            InitializeComponent();
        }

        #region Algorithm handling


        TxAlgorithm instance;
        public TxAlgorithm AlgorithmInstance
        {
            get
            {
                if (instance != null)
                    GatherValues();
                return instance;
            }
            set
            {
                if (instance != value)
                {
                    ClearAll();
                    LoadInstance(value);
                }
            }
        }

        protected virtual void LoadInstance(TxAlgorithm algorithmInstance)
        {
            Type algType = algorithmInstance.GetType();
            object[] attributes = algType.GetCustomAttributes(typeof(AlgorithmAttribute), false);
            //Debe haber uno solo
            if (attributes.Length > 0)
            {
                AlgorithmAttribute algAtt = attributes[0] as AlgorithmAttribute;
                name_Label.Text = algAtt.Name;
                description_TextBox.Text = algAtt.Description;
            }
            //Si no tiene este atributo, no mostramos sus datos
            //Cargar cada uno de los parametros
            int row = 0;
            foreach (PropertyInfo property in algType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.CanRead && property.CanWrite)
                {
                    object[] paramAtt = property.GetCustomAttributes(typeof(ParameterAttribute),false);
                    //Verificar que está especificada como parámetro
                    if (paramAtt.Length > 0)//Tiene puesto el atributo
                    {
                        AddParameter(row, property, algorithmInstance,(ParameterAttribute)paramAtt[0]);
                        row++;
                    }
                }
            }
            instance = algorithmInstance;
        }

        private void AddParameter(int row, PropertyInfo property, TxAlgorithm algorithmInstance, ParameterAttribute description)
        {
            Label label = new Label();
            label.Text = description.Name;
            label.Tag = description.Description;
            label.Click+=new EventHandler(label_Click);
            parameters_Table.Controls.Add(label, 0, row);
            Control parameterEditor = (Control)ParameterEditorProvider.GetParameterEditor(property);
            parameterEditor.Tag = property;
            
            //Tomando los valores por defecto
            //TODO:Esto no debe dar palo pero hay que prevenir
            ((IParameterEditor)parameterEditor).ParameterValue = property.GetValue(algorithmInstance, new object[] { });
            parameters_Table.Controls.Add(parameterEditor, 1, row);
            (parameterEditor as IParameterEditor).ParameterValueChanged += new EventHandler(AlgorithmViewer_ParameterValueChanged);
        }

        void AlgorithmViewer_ParameterValueChanged(object sender, EventArgs e)
        {
            //TODO: No se si mejor manejo aqui la exception que dejar lo a la aplicacion
            //lo segundo pudiera ser traumatico
            Control control = (Control)sender;
            PropertyInfo property = (PropertyInfo)control.Tag;
            try
            {
                property.SetValue(instance, (control as IParameterEditor).ParameterValue, new object[] { });
            }
            catch (Exception exc)
            {
                int row = parameters_Table.GetCellPosition(control).Row;
                throw new TxIncorrectValueForParameterException(parameters_Table.GetControlFromPosition(0, row).Text);
            }
        }

        private void GatherValues()
        {
            foreach (Control control in parameters_Table.Controls)
            {
                if (control is IParameterEditor)
                {

                    PropertyInfo property = (PropertyInfo)control.Tag;
                    try
                    {
                        property.SetValue(instance, (control as IParameterEditor).ParameterValue, new object[] { });
                    }
                    catch (Exception) 
                    {
                        int row = parameters_Table.GetCellPosition(control).Row;
                        throw new TxIncorrectValueForParameterException(parameters_Table.GetControlFromPosition(0,row).Text); 
                    }
                }
            }
        }
    

        public void ClearAll()
        {
            name_Label.Text = "";
            description_TextBox.Text = "";
            paramDsc_TextBox.Text = "";
            parameters_Table.Controls.Clear();
            instance = null;
        }


        #endregion

        #region Event handling
        void text_GotFocus(object sender, EventArgs e)
        {
            int row = parameters_Table.GetPositionFromControl((Control)sender).Row;
            label_Click(parameters_Table.GetControlFromPosition(0, row), EventArgs.Empty);
        }

        Label prior = null;
        void label_Click(object sender, EventArgs e)
        {
            if (prior != null)
            {
                prior.BackColor = SystemColors.Control;
                prior.ForeColor = SystemColors.ControlText;
            }
            Label label = (Label)sender;
            label.BackColor = SystemColors.ActiveCaption;
            label.ForeColor = SystemColors.ActiveCaptionText;
            paramDsc_TextBox.Text = (string)label.Tag;
            prior = label;
            
        }
        #endregion

        #region Browsable properties
        [Browsable(true)]
        [Category("Algorithm")]
        [Description("Gets or sets a value indicating whether the algorithm's description should be visible")]
        public bool ShowDescription
        {
            get
            {
                return description_Label.Visible;
            }
            set
            {
                description_TextBox.Visible = value;
                description_Label.Visible = value;
                splitter1.Visible = value;
            }
        }

        [Browsable(true)]
        [Category("Algorithm")]
        [Description("Gets or sets a value indicating whether the parameters' description should be visible")]
        public bool ViewParameterDescription
        {
            get
            {
                return paramDsc_TextBox.Visible;
            }
            set
            {
                paramDsc_TextBox.Visible = value;
                splitter2.Visible = value;
            }
        }
        #endregion


    }
}
