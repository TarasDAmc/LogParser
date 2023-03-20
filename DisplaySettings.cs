namespace LogParser
{
    public enum DisplayType
    {
        onlyErrors,
        onlyWarnings,
        onlyInfo,
        onlyVerb,
        all,
        atLeastInfo,
        atLeastWarning
    };
    public class DisplaySettings
    {
        public bool showErrors;
        public bool showWarning;
        public bool showInfo;
        public bool showVerbose;
        public bool showAll;
        public DisplaySettings()
        {
            showErrors = true;
            showWarning = true;
            showInfo = true;
            showVerbose = true;
            showAll = true;
        }
        public DisplaySettings(DisplayType dt)
        {
            showErrors = false;
            showWarning = false;
            showInfo = false;
            showVerbose = false;
            showAll = false;
            switch (dt)
            {
                case DisplayType.onlyErrors:
                    showErrors = true;
                    showWarning = false;
                    showInfo = false;
                    showVerbose = false;
                    showAll = false;
                    break;
                case DisplayType.onlyWarnings:
                    showErrors = false;
                    showWarning = true;
                    showInfo = false;
                    showVerbose = false;
                    showAll = false;
                    break;
                case DisplayType.onlyInfo:
                    showErrors = false;
                    showWarning = false;
                    showInfo = true;
                    showVerbose = false;
                    showAll = false;
                    break;
                case DisplayType.onlyVerb:
                    showErrors = false;
                    showWarning = false;
                    showInfo = false;
                    showVerbose = true;
                    showAll = false;
                    break;
                case DisplayType.all:
                    showErrors = true;
                    showWarning = true;
                    showInfo = true;
                    showVerbose = true;
                    showAll = true;
                    break;
                case DisplayType.atLeastInfo:
                    showErrors = true;
                    showWarning = true;
                    showInfo = true;
                    showVerbose = false;
                    showAll = false;
                    break;
                case DisplayType.atLeastWarning:
                    showErrors = true;
                    showWarning = true;
                    showInfo = false;
                    showVerbose = false;
                    showAll = false;
                    break;
            }
        }
    }
}
