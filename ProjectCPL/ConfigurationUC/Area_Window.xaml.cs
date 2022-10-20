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
using System.Windows.Shapes;
using Cover.Backend.BL;
using Cover.Backend.Entities;
using Cover.POS.Controls;
using WBM.Common.Windows;
using WBM.Common;

namespace Cover.POS.ConfigurationUC
{
    public partial class Area_Window : Window
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


        public Int32 areaId { get; set; }
        public delegate void OnSuccessEventHandler();
        public event OnSuccessEventHandler OnSuccess;

        #endregion

        #region "Constructor"

        public Area_Window()
        {
            InitializeComponent();
        }

        #endregion

        #region "Loaded"

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (areaId == 0)
            {
                this.lbTitle.Content = "Alta de área";
            }
            else
            {
                this.lbTitle.Content = "Edición de área";
                var user = areaService.GetById(areaId);
                this.txtName.Text = user.Name;
            }
        }

        #endregion

        #region "Button_Events"

        private void ExitButtonw_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (areaId == 0)
                {
                    var name = this.txtName.Text;
                    areaService.InsertArea(name);
                }
                else
                {
                    var area = areaService.GetById(areaId);
                    area.Name = this.txtName.Text;
                    areaService.UpdateArea(area.Name, area.Active, area.Id);
                }
                OnSuccess();
                this.Close();
            }
            catch (Exception ex)
            {
                StoryBoardHelper.BeginFadeOut(parent);
                var messageUC = new Message_Window(String.Format(ex.Message), MessageType.message);
                messageUC.ShowDialog();
                StoryBoardHelper.BeginFadeIn(parent);
            }

        }

        #endregion

    }
}
