using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using TxEstudioKernel;
using TxEstudioKernel.CustomAttributes;
using System.Collections.Generic;
using Janus.Windows.ButtonBar;
using TxEstudioApplication.Dialogs;
using TxEstudioKernel.Operators;


namespace TxEstudioApplication
{
    /// <summary>
    /// Esta es la parte encargada de la carga de los operadores.
    /// </summary>
    public partial class Environment
    {
        /// <summary>
        /// La coleccion completa de operadores.
        /// </summary>
        Dictionary<string, AlgorithmHolder> operatorsCollection = new Dictionary<string, AlgorithmHolder>(30);
        Dictionary<string, AlgorithmHolder> descriptorsCollection = new Dictionary<string, AlgorithmHolder>(30);
        Dictionary<string, AlgorithmHolder> segmentersCollection = new Dictionary<string, AlgorithmHolder>(10);
        Dictionary<string, AlgorithmHolder> textureEdgeCollection = new Dictionary<string, AlgorithmHolder>(10);
        Dictionary<string, CategoryHolder> descriptorsCategories = new Dictionary<string, CategoryHolder>(5);

        #region Loading Operators

        /// <summary>
        /// Es el metodo encargado de realizar la carga de los 
        /// distintos operadores, por ahora no se tiene en cuenta la categoria.
        /// </summary>
        public void LoadOperators()
        {
            //Cargando el kernel
            LoadAssembly(Assembly.GetAssembly(typeof(TxDIPAlgorithm)));//Los operadores que estan en el kernel
            string operatorsDirectory = Application.StartupPath + Properties.Settings.Default.OperatorsPath;
            if (Directory.Exists(operatorsDirectory))
            {
                //Cargando del "repositorio"
                foreach (string file in Directory.GetFiles(operatorsDirectory, "*.dll", SearchOption.AllDirectories))
                {
                    try
                    {
                        LoadAssembly(Assembly.LoadFile(file));
                    }
                    catch (Exception exc)
                    {
                        //Pudiera construirse un log para los errores
                        //pass por el momento retorno silencioso
                    }
                }
            }
            //Construir paneles
            OrderDescriptorsCategories();
            BuildOperatorsPanel();
            BuildTexturePanel();
        }
        public void LoadAssembly(Assembly assembly)
        {
            foreach (Type type in assembly.GetExportedTypes())
            {
                if (type.IsSubclassOf(typeof(TxAlgorithm)) && !type.IsAbstract)//es un posible operador
                {
                    object[] atts = type.GetCustomAttributes(typeof(AlgorithmAttribute), false);
                    AlgorithmHolder holder = null;
                    AlgorithmAttribute attribute = null;
                    //debe haber uno solo
                    if (atts.Length > 0)//Si no tiene el atributo colocado, no lo cargamos
                    {
                        attribute = atts[0] as AlgorithmAttribute;
                        holder = new AlgorithmHolder(type);
                        //Depende de lo que sea lo ponemos o no en un lugar o en otro
                        if (type.IsSubclassOf(typeof(TextureDescriptor)))//Es un descriptor de textura
                        {
                            if (!descriptorsCollection.ContainsKey(attribute.Name))
                                descriptorsCollection.Add(attribute.Name, holder);
                        }
                        else if (type.IsSubclassOf(typeof(TxTextureEdgeDetector)))//es un operador de  procesamiento de detecion de bordes de textura
                        {
                            if (!textureEdgeCollection.ContainsKey(attribute.Name))
                                textureEdgeCollection.Add(attribute.Name, holder);

                        }
                        else if (type.IsSubclassOf(typeof(TxSegmentationAlgorithm)))//Es un operador de segmentacion
                        {
                            if (!segmentersCollection.ContainsKey(attribute.Name))
                                segmentersCollection.Add(attribute.Name, holder);
                        }
                        else if (type.IsSubclassOf(typeof(TxDIPAlgorithm)))//es un operador de  procesamiento de imagenes
                        {
                            if (!operatorsCollection.ContainsKey(attribute.Name))
                                operatorsCollection.Add(attribute.Name, holder);
                        }
                       
                    }
                }
            }
        }
        private void OrderDescriptorsCategories()
        {
            //Por ahora, para quitarnos complicaciones de arriba,  vamos a manejar las categorias conocidas
            //Si despues se quiere ampliar, adicionar codigo al final de este metodo
            List<AlgorithmHolder> allDescriptors = new List<AlgorithmHolder>(descriptorsCollection.Count);
            foreach (AlgorithmHolder holder in descriptorsCollection.Values)
                allDescriptors.Add(holder);
            FeatureDescriptorAtribute[] stdDescCats = new FeatureDescriptorAtribute[]{  new FirstOrderStatisticDescriptorAttribute(),
                                                                                    new CoocurrenceMatrixDescriptorAttribute(),
                                                                                    new RunLenthDescriptorAttribute(),
                                                                                    new TextureSpectrumDescriptorAttribute(),
                                                                                    new GaborDescriptorAttribute(),
                                                                                    new MomentsDescriptorAttribute()
                                                                                   };
            for (int i = 0; i < stdDescCats.Length; i++)
                descriptorsCategories.Add(stdDescCats[i].Name, new CategoryHolder(stdDescCats[i], allDescriptors));
        }

        #endregion

        #region Retrieving operators
        public CategoryHolder GetDescriptorsCategory(string categoryName)
        {
           return descriptorsCategories[categoryName];
        }

        public List<CategoryHolder> GetAllDescriptorsCategories()
        {
            List<CategoryHolder> result = new List<CategoryHolder>(5);
            foreach (CategoryHolder holder in descriptorsCategories.Values)
                result.Add(holder);
            return result;
        }

        public TxAlgorithm GetOperator(string operatorName)
        {
            return operatorsCollection[operatorName].Algorithm;
        }

        public TextureDescriptor GetDescriptor(string descriptorName)
        {
            return (TextureDescriptor)descriptorsCollection[descriptorName].Algorithm;
        }

        public TxSegmentationAlgorithm GetSegmentationOperator(string operatorName)
        {
            return (TxSegmentationAlgorithm)segmentersCollection[operatorName].Algorithm;
        }

        public TxTextureEdgeDetector GetTextureEdgeOperator(string operatorName)
        {
            return (TxTextureEdgeDetector)textureEdgeCollection[operatorName].Algorithm;
        }

        public List<AlgorithmHolder> GetOperators(TxAlgorithmCondition condition)
        {
            List<AlgorithmHolder> result = new List<AlgorithmHolder>(operatorsCollection.Count);
            foreach (AlgorithmHolder holder in operatorsCollection.Values)
            {
                if (condition(holder))
                    result.Add(holder);
            }
            return result;
        }
        #endregion

        #region Click Handling

        void operators_ItemClick(object sender, ItemEventArgs e)
        {
            TxDIPAlgorithm target = (TxDIPAlgorithm)operatorsCollection[e.Item.Text].Algorithm;
            if (TxDIPAlgorithm.IsOneBandInput(target))
            {
                OneBandDialog dialog = new OneBandDialog();
                dialog.SetEnvironment(this);
                dialog.ShowDialogWith(mainForm, target);
            }
            else
            {
                MultiBandDialog dialog = new MultiBandDialog();
                dialog.SetEnvironment(this);
                dialog.ShowDialogWith(mainForm, target);
            }
        }

        void textures_ItemClick(object sender, ItemEventArgs e)
        {
            if (e.Item.Group.Text == "Texture Region Segmentation")
            {
                SegmentationDialog dialog = new SegmentationDialog();
                dialog.SetEnvironment(this);
                dialog.ShowDialogWith(mainForm, this.GetSegmentationOperator(e.Item.Text));
            }
            else if (e.Item.Group.Text == "Texture Edge Segmentation")
            {
                SegmentationDialog dialog = new SegmentationDialog();
                dialog.SetEnvironment(this);
                dialog.ShowDialogWith(mainForm, this.GetTextureEdgeOperator(e.Item.Text));
            }
            else if (e.Item.Group.Text == "Texture Combined Segmentation")
            {

                Combination_Methods.CombinationMethodDialog dialog = new TxEstudioApplication.Combination_Methods.CombinationMethodDialog();
                dialog.SetEnvironment(this);
                //dialog.ShowDialog(mainForm,
                this.ShowDialog(dialog);
            }
            else
            {
                DescriptorDialog dialog = new DescriptorDialog();
                dialog.SetEnvironment(this);
                dialog.ShowDialogWith(mainForm, e.Item.Text);
            }
        }

        #endregion

        #region Visual Components

        private ButtonBar BuildButtonBar()
        {
            ButtonBar bar = new ButtonBar();
            bar.SelectionArea = SelectionArea.FullItem;
            bar.ItemAppearance = ItemAppearance.Flat;
            bar.KeepSelection = false;
            bar.VisualStyle = VisualStyle.Office2003;
            bar.Dock = DockStyle.Fill;
            return bar;
        }

        private ButtonBarGroup BuildButtonBarGroup(string name, string toolTipText)
        {
            ButtonBarGroup group = new ButtonBarGroup(name);
            group.View = ButtonBarView.SmallIcons;
            group.ToolTipText = toolTipText;
            return group;
        }

        private void BuildOperatorsPanel()
        {
            ButtonBar bar = BuildButtonBar();
            Dictionary<Type, ButtonBarGroup> categories = new Dictionary<Type, ButtonBarGroup>();
            ButtonBarGroup miscGroup = BuildButtonBarGroup("Misc", "");
            categories.Add(typeof(Misc), miscGroup);
            foreach (AlgorithmHolder holder in operatorsCollection.Values)
            {
                object[] categoriesList = holder.AlgorithmType.GetCustomAttributes(typeof(TxCategoryAttribute), false);
                if (categoriesList.Length == 0)
                {
                    ButtonBarItem item = new ButtonBarItem(holder.AlgorithmName);
                    item.ToolTipText = holder.AlgorithmDescription;
                    miscGroup.Items.Add(item);
                }
                else
                {
                    for (int i = 0; i < categoriesList.Length; i++)
                    {
                        Type categoryType = categoriesList[i].GetType();
                        ButtonBarGroup group = null;
                        if (!categories.ContainsKey(categoryType))
                        {
                            group = BuildButtonBarGroup((categoriesList[i] as TxCategoryAttribute).Name, (categoriesList[i] as TxCategoryAttribute).Description);
                            categories.Add(categoryType, group);
                            bar.Groups.Add(group);
                        }
                        else
                            group = categories[categoryType];
                        ButtonBarItem item = new ButtonBarItem(holder.AlgorithmName);
                        item.ToolTipText = holder.AlgorithmDescription;
                        group.Items.Add(item);
                    }
                }
            }
            bar.Groups.Add(miscGroup);
            bar.ItemClick += new ItemEventHandler(operators_ItemClick);
            this.PublishPanel("Digital Image Processing", bar);
        }

        private void BuildTexturePanel()
        {
            ButtonBar bar = BuildButtonBar();
            ButtonBar segmentationBar = BuildButtonBar();
            ButtonBarGroup descriptorsGroup = BuildButtonBarGroup("Texture Descriptors", "");
            ButtonBarGroup segmentationGroup = BuildButtonBarGroup("Texture Region Segmentation", "");

            Dictionary<Type, ButtonBarGroup> categories = new Dictionary<Type, ButtonBarGroup>();
            ButtonBarGroup texEdgeGroup = BuildButtonBarGroup("Texture Edge Segmentation", "");
           // ButtonBarGroup texCombinedGroup = BuildButtonBarGroup("Texture Combined Segmentation", "");
            categories.Add(typeof(TxTextureEdgeDetector), texEdgeGroup);
          //  categories.Add(typeof(AC_BiClass_BorderRegion), texCombinedGroup);

            foreach (CategoryHolder holder in descriptorsCategories.Values)
            {
                ButtonBarItem item = new ButtonBarItem(holder.CategoryName);
                item.ToolTipText = holder.CategoryDescription;
                descriptorsGroup.Items.Add(item);
            }

            foreach (AlgorithmHolder holder in textureEdgeCollection.Values)
            {
                ButtonBarItem item = new ButtonBarItem(holder.AlgorithmName);
                item.ToolTipText = holder.AlgorithmDescription;
                texEdgeGroup.Items.Add(item);
            }

            foreach (AlgorithmHolder holder in segmentersCollection.Values)
            {
                ButtonBarItem item = new ButtonBarItem(holder.AlgorithmName);
                item.ToolTipText = holder.AlgorithmDescription;
                segmentationGroup.Items.Add(item);
            }

           // texCombinedGroup.Items.Add("acBi-EDGE Method");



            bar.Groups.Add(descriptorsGroup);
            segmentationBar.Groups.Add(segmentationGroup);
            segmentationBar.Groups.Add(texEdgeGroup);
          //  segmentationBar.Groups.Add(texCombinedGroup);
            bar.ItemClick += new ItemEventHandler(textures_ItemClick);
            segmentationBar.ItemClick += new ItemEventHandler(textures_ItemClick);
            this.PublishPanel("Texture descriptors", bar);
            this.PublishPanel("Segmentation methods", segmentationBar);
        }

        #endregion

    }

    public delegate bool TxAlgorithmCondition(AlgorithmHolder holder);
}