using Cover.Backend.BL;
using Cover.Backend.Entities;
using Cover.POS.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WBM.Common.Windows;
using WBM.Common;

namespace Cover.POS.ConfigurationUC
{
    public partial class CoverConfigurations : UserControl
    {

        #region "Properties"

        private CoverConfigurationService _coverconfigurationService;
        private CoverConfigurationService coverconfigurationService
        {
            get
            {
                if (_coverconfigurationService == null)
                    _coverconfigurationService = new CoverConfigurationService();
                return _coverconfigurationService;
            }
            set
            {
                _coverconfigurationService = value;
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

        public CoverConfigurations()
        {
            InitializeComponent();
        }

        #endregion

        #region "Loaded"

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCovers();
        }

        private void LoadCovers()
        {
            dgCover.Items.Clear();
            var coversConfiguration = coverconfigurationService.GetCoversConfiguration();
            foreach (var cover in coversConfiguration)
                dgCover.Items.Add(cover);
        }

        #endregion

        #region "Button_Events"

        public void AddCover_Click(object sender, RoutedEventArgs e)
        {
            StoryBoardHelper.BeginFadeOut(parent);
            var frm = new CoverConfiguration_Window();
            frm.OnSuccess += frm_OnSuccess;
            frm.ShowDialog();
            frm = null;
            StoryBoardHelper.BeginFadeIn(parent);
        }

        void frm_OnSuccess()
        {
            LoadCovers();
        }

        private void btndeleteConfiguration_Click(object sender, RoutedEventArgs e)
        {
            var coverConfiguration = ((CoverConfiguration)dgCover.SelectedItem);
            coverconfigurationService.CoverDelete(coverConfiguration.Id, false);
            LoadCovers();
        }

        private void btnEditConfiguration_Click(object sender, RoutedEventArgs e)
        {
            var coverConfiguration = ((CoverConfiguration)dgCover.SelectedItem);
            StoryBoardHelper.BeginFadeOut(parent);
            var frm = new CoverConfiguration_Window();
            frm.coverConfigurationId = coverConfiguration.Id;
            frm.OnSuccess += frm_OnSuccess;
            frm.ShowDialog();
            frm = null;
            StoryBoardHelper.BeginFadeIn(parent);
        }

        private void btnEditPrices_Click(object sender, RoutedEventArgs e)
        {
            var coverConfiguration = ((CoverConfiguration)dgCover.SelectedItem);
            StoryBoardHelper.BeginFadeOut(parent);
            var frm = new CoverPrices_Window();
            frm.coverConfigurationId = coverConfiguration.Id;
            frm.ShowDialog();
            frm = null;
            StoryBoardHelper.BeginFadeIn(parent);
        }

        #endregion

    }
}


