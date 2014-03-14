using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyEagle
{
    public class Constants
    {
        #region Defaults
        /// <summary>
        /// Ruta por default donde se almagenan las imagenes
        /// </summary>
        public const string DEFAULT_PATH = @"C:\Ingress\Screenshots";
        /// <summary>
        /// Tiempo que tarda la interfaz de Intel en hacer Human Timeout.
        /// </summary>
        public const int HUMAN_TIMEOUT_SECONDS = 1200;
#if DEBUG
        /// <summary>
        /// Tiempo que tarda en actualizar la pagina y realizar el screenshot.
        /// </summary>
        public const int DELAY_SCREENSHOT_SECONDS = 5;
#else
        /// <summary>
        /// Tiempo que tarda en actualizar la pagina y realizar el screenshot.
        /// </summary>
        public const int DELAY_SCREENSHOT_SECONDS = 60;
#endif
#if DEBUG
        /// <summary>
        /// Tiempo que tarda entre un screenshot y el otro.
        /// </summary>
        public const int DELAY_BETWEEN_SCREENSHOTS_SECONDS = 10;
#else
        /// <summary>
        /// Tiempo que tarda entre un screenshot y el otro.
        /// </summary>
        public const int DELAY_BETWEEN_SCREENSHOTS_SECONDS = 3000;
#endif
        #endregion

        #region Text Formats
        public const string SCREENSHOT_FILE_FORMAT = "imagen_{0}.png";
        public const string SCREENSHOT_TIMESTAMP_FORMAT = "_yyMMdd_HHmmss"; 
        #endregion

        #region Text messages
        public const string TEXT_START = "Start screenshots timer!";
        public const string TEXT_STOP = "Stop screenshots timer!";
        public const string TEXT_IE_REQUIRED = "Microsoft Internet Explorer is required!"; 
        #endregion

        #region URLS
        public const string INGRESS_INTEL_URL = "http://ingress.com/intel"; 
        #endregion

        #region Registry
        public const string REGISTRY_BROWSER_FEATURE_CONTROL_KEY = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\";
        public const string REGISTRY_BROWSER_FEATURE_SUB_KEY = @"SOFTWARE\Microsoft\Internet Explorer"; 
        #endregion
    }
}
