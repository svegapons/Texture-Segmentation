using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TxEstudioKernel.Operators;

namespace TxEstudioKernel.VisualElements
{
    public partial class DirectionEditor : UserControl, IParameterEditor
    {
        public DirectionEditor()
        {
            InitializeComponent();

            north_Button.Tag = Directions.North;
            nEast_Button.Tag = Directions.NEast;
            east_Button.Tag = Directions.East;
            sEast_Button.Tag = Directions.SEast;
            south_Button.Tag = Directions.South;
            sWest_Button.Tag = Directions.SWest;
            west_Button.Tag = Directions.West;
            nWest_Button.Tag = Directions.NWest;

            pressed = north_Button;
        }

        Button pressed;

        private void button_Click(object sender, EventArgs e)
        {
            Button senderButt = sender as Button;
            if (senderButt != pressed)
            {
                pressed.FlatStyle = FlatStyle.Standard;
                senderButt.FlatStyle = FlatStyle.Flat;
                pressed = senderButt;
            }
        }

        #region IParameterEditor Members

        public object ParameterValue
        {
            get
            {
                return pressed.Tag;
            }
            set
            {
                Directions direction = (Directions)value;
                pressed.FlatStyle = FlatStyle.Standard;
                switch (direction)
                {

                    case Directions.North:
                        pressed = north_Button;
                        break;
                    case Directions.NEast:
                        pressed = nEast_Button;
                        break;
                    case Directions.East:
                        pressed = east_Button;
                        break;
                    case Directions.SEast:
                        pressed = sEast_Button;
                        break;
                    case Directions.South:
                        pressed = south_Button;
                        break;
                    case Directions.SWest:
                        pressed = sWest_Button;
                        break;
                    case Directions.West:
                        pressed = west_Button;
                        break;
                    case Directions.NWest:
                        pressed = nWest_Button;
                        break;
                }
                pressed.FlatStyle = FlatStyle.Flat;
            }
        }

        public string GetStringParameterRepresentation()
        {
            return pressed.Tag.ToString();
        }

        public event EventHandler ParameterValueChanged;

        #endregion
    }
}
