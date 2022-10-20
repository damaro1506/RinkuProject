using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cover.Backend.BL;
using Cover.Backend.Entities;
using System.Windows.Media.Animation;
using Cover.POS;
using Cover.POS.Controls;
using WBM.Common.Windows;
using WBM.Common;

namespace Cover.POS.ConfigurationUC
{
    /// <summary>
    /// Interaction logic for Areas.xaml
    /// </summary>
    public partial class Areas : UserControl
    {

        #region "Properties"

        private AreaService _areaService;
        private AreaService areaService
        {
            get
            {
                if (_areaService == null)
                    _areaService = new AreaService();
                return _areaService;
            }
            set
            {
                _areaService = value;
            }
        }

        private Window _parent;
        private Window parent
        {
            get
            {
                if (_parent == null)
                    _parent = Window.GetWindow(this);
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        #endregion

        #region "Constructor"

        public Areas()
        {
            InitializeComponent();
        }

        #endregion

        #region "Loaded"

        private void Areas_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAreas();
        }

        private void LoadAreas()
        {
            dgAreas.Items.Clear();
            var areas = areaService.GetActiveAreas();
            foreach (var area in areas)
                dgAreas.Items.Add(area);
        }

        #endregion

        #region "Button_Events"

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            StoryBoardHelper.BeginFadeOut(parent);
            var frm = new Area_Window();
            frm.OnSuccess += frm_OnSuccess;
            frm.ShowDialog();
            frm = null;
            StoryBoardHelper.BeginFadeIn(parent);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            StoryBoardHelper.BeginFadeOut(parent);
            var frm = new Area_Window();
            frm.areaId = ((Area)dgAreas.SelectedItem).Id;
            frm.OnSuccess += frm_OnSuccess;
            frm.ShowDialog();
            StoryBoardHelper.BeginFadeIn(parent);
        }

        void frm_OnSuccess()
        {
            LoadAreas();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            StoryBoardHelper.BeginFadeOut(parent);
            var area = ((Area)dgAreas.SelectedItem);
            var messageUC = new Message_Window(String.Format("¿Está seguro que desea eliminar el área {0}?", area.Name), MessageType.confirmation);
            messageUC.ShowDialog();
            if (messageUC.DialogResult == true)
            {
                areaService.UpdateArea(area.Name, false, area.Id);
                //if (area.IsVisible)
                //    dgAreas.Visibility = Visibility.Visible;
                LoadAreas();
            }
            StoryBoardHelper.BeginFadeIn(parent);
        }

        #endregion

    }
}
