using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using SkyEagle;

namespace RoboScreenSniperDesktop
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Construccion de la forma
        /// </summary>
        public MainForm()
        {
            SetBrowserFeatureControl();


            InitializeComponent();
        }

        #region Eventos de la Forma
        private void button1_Click(object sender, EventArgs e)
        {
            Toggle();
        }
        /// <summary>
        /// Tic que ocurre cada segundo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Contador >= Constants.DELAY_BETWEEN_SCREENSHOTS_SECONDS)
            {
                Contador = 0;
                if(!backgroundWorker1.IsBusy)
                    backgroundWorker1.RunWorkerAsync();
            }

            decimal progreso = 
                Convert.ToDecimal(Contador) / Convert.ToDecimal(Constants.DELAY_BETWEEN_SCREENSHOTS_SECONDS) * 
                Convert.ToDecimal(100);

            progressBar1.Value = Convert.ToInt32(progreso);
            Contador++;
        }

        private void Screenshot()
        {
            using (Bitmap bitmap = new Bitmap(webBrowser1.Width, webBrowser1.Height))
            {
                webBrowser1.DrawToBitmap(bitmap, new Rectangle(0, 0, webBrowser1.Width, webBrowser1.Height));
                bitmap.Save(string.Format(this.Path + @"\" + Constants.SCREENSHOT_FILE_FORMAT, DateTime.Now.ToString(Constants.SCREENSHOT_TIMESTAMP_FORMAT)));
            }
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            GoToIngress();
            textBox1.Text = Path;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            GoToIngress();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = Path;
            if (folderBrowserDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                Path = folderBrowserDialog1.SelectedPath;
                textBox1.Text = Path;
            }
        }
        #endregion

        #region Campos

        #endregion

        #region Propiedades
        /// <summary>
        /// Contador de segundos
        /// </summary>
        public int Contador { get; set; }
        /// <summary>
        /// Bandera que indica si fue iniciado el proceso de captura de imagenes
        /// </summary>
        public bool Started { get; set; }
        public string Path {
            get 
            {
                return string.IsNullOrEmpty(this.path) ? 
                    Constants.DEFAULT_PATH : 
                    path;
            }
            set 
            {
                path = value;
            }
        }
        #endregion

        #region Campos
        private string path = string.Empty;
        #endregion

        #region Metodos Privados
        private void SetBrowserFeatureControl()
        {
            // http://msdn.microsoft.com/en-us/library/ee330720(v=vs.85).aspx

            // FeatureControl settings are per-process
            var fileName = System.IO.Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

            // make the control is not running inside Visual Studio Designer
            if (String.Compare(fileName, "devenv.exe", true) == 0 || String.Compare(fileName, "XDesProc.exe", true) == 0)
                return;

            SetBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION", fileName, GetBrowserEmulationMode()); // Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode.
            SetBrowserFeatureControlKey("FEATURE_AJAX_CONNECTIONEVENTS", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_MANAGE_SCRIPT_CIRCULAR_REFS", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_DOMSTORAGE ", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_GPU_RENDERING ", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_IVIEWOBJECTDRAW_DMLT9_WITH_GDI  ", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_NINPUT_LEGACYMODE", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_DISABLE_LEGACY_COMPRESSION", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_LOCALMACHINE_LOCKDOWN", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_OBJECT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_SCRIPT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_DISABLE_NAVIGATION_SOUNDS", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_SCRIPTURL_MITIGATION", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_SPELLCHECKING", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_STATUS_BAR_THROTTLING", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_TABBED_BROWSING", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_VALIDATE_NAVIGATE_URL", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_WEBOC_DOCUMENT_ZOOM", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_WEBOC_POPUPMANAGEMENT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_WEBOC_MOVESIZECHILD", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_ADDON_MANAGEMENT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_WEBSOCKET", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_WINDOW_RESTRICTIONS ", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_XMLHTTP", fileName, 1);
        }
        private UInt32 GetBrowserEmulationMode()
        {
            int browserVersion = 7;
            using (var ieKey = Registry.LocalMachine.OpenSubKey(Constants.REGISTRY_BROWSER_FEATURE_SUB_KEY,
                RegistryKeyPermissionCheck.ReadSubTree,
                System.Security.AccessControl.RegistryRights.QueryValues))
            {
                var version = ieKey.GetValue("svcVersion");
                if (null == version)
                {
                    version = ieKey.GetValue("Version");
                    if (null == version)
                        throw new ApplicationException(Constants.TEXT_IE_REQUIRED);
                }
                int.TryParse(version.ToString().Split('.')[0], out browserVersion);
            }

            UInt32 mode = 10000; // Internet Explorer 10. Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode. Default value for Internet Explorer 10.
            switch (browserVersion)
            {
                case 7:
                    mode = 7000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE7 Standards mode. Default value for applications hosting the WebBrowser Control.
                    break;
                case 8:
                    mode = 8000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE8 mode. Default value for Internet Explorer 8
                    break;
                case 9:
                    mode = 9000; // Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode. Default value for Internet Explorer 9.
                    break;
                default:
                    // use IE10 mode by default
                    break;
            }

            return mode;
        }
        private void GoToIngress()
        {
            webBrowser1.Navigate(Constants.INGRESS_INTEL_URL);
        }
        private void SetBrowserFeatureControlKey(string feature, string appName, uint value)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(
                String.Concat(Constants.REGISTRY_BROWSER_FEATURE_CONTROL_KEY, feature),
                RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                key.SetValue(appName, (UInt32)value, RegistryValueKind.DWord);
            }
        }
        private void Toggle()
        {
            button1.Text = Started ?
                Constants.TEXT_START :
                Constants.TEXT_STOP;

            timer1.Enabled = !Started;
            Started = !Started;
            Contador = 0;
        }
        #endregion   

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Screenshot();
        }
    }
}
