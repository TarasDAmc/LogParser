using System.Collections.Generic;
using System.Windows.Controls;

namespace LogParser
{
    public enum DisplayType
    {
        all,
        onlyErrors,
        onlyWarnings,
        onlyInfo,
        onlyVerb,
        atLeastInfo,
        atLeastWarning,
        warningErrorsInfo,
        warningInfoVerb,
        warningErrorsVerb,

    };
    public class DisplaySettings
    {
        public bool showErrors = true;
        public bool showWarning = true;
        public bool showInfo = true;
        public bool showEcho = true;
        public bool showBold = true;
        public bool showSimple = true;
        public DisplaySettings() { }
        private readonly List<CheckBox> _displayConfigurationsList;
        public DisplaySettings(List<CheckBox> displayConfigurationsList)
        {
            _displayConfigurationsList = displayConfigurationsList;
            foreach (CheckBox c in _displayConfigurationsList)
            {
                if (c.Name == "cbInfo" && c.IsChecked == false) showInfo = false;
                else if (c.Name == "cbWarnings" && c.IsChecked == false) showWarning = false;
                else if (c.Name == "cbErrors" && c.IsChecked == false) showErrors = false;
                else if (c.Name == "cbEcho" && c.IsChecked == false) showEcho = false;
                else if (c.Name == "cbBold" && c.IsChecked == false) showBold = false;
                else if (c.Name == "cbSimple" && c.IsChecked == false) showSimple = false;
            }
        }

        public DisplaySettings(DisplayType dt)
        {
            switch (dt)
            {
                case DisplayType.all:
                    showErrors = true;
                    showWarning = true;
                    showInfo = true;
                    showEcho = true;
                    showBold = true;
                    showSimple = true;
                    break;
                case DisplayType.atLeastWarning:
                    showErrors = true;
                    showWarning = true;
                    showInfo = false;
                    showEcho = false;
                    break;
            }
        }
    }
}
