using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Input;

namespace Projects.Commons
{
    public class ProjectsHelper
    {

        #region "Properties"

        private static ResourceDictionary _mainDictionary;
        public static ResourceDictionary mainDictionary
        {
            get
            {
                if (_mainDictionary == null)
                    _mainDictionary = Application.Current.Resources.MergedDictionaries.First();
                return _mainDictionary;
            }
            set
            {
                _mainDictionary = value;
            }
        }

        #endregion

        #region "Fade_Events"

        public static void BeginFadeOut(FrameworkElement containingObject)
        {
            containingObject.Cursor = Cursors.Wait;
            ((Storyboard)ProjectsHelper.mainDictionary["StoryboardFadeOut"]).Begin(containingObject);
        }

        public static void BeginFadeIn(FrameworkElement containingObject)
        {
            containingObject.Cursor = Cursors.Arrow;
            ((Storyboard)ProjectsHelper.mainDictionary["StoryboardFadeIn"]).Begin(containingObject);
        }

        #endregion

    }
}