using ParadosLib;
using System.Configuration;

namespace ParadosTranslator
{
    public partial class FormUI : Form
    {
        private Verifier verificatore;

        public FormUI()
        {
            this.InitializeComponent();
            this.cbOriginLanguage.DataSource = LanguageValues.Values;
            this.cbTraducedLanguage.DataSource = LanguageValues.Values;
            this.cbWorkingLanguage.DataSource = LanguageValues.Values;

            cbOriginLanguage.SelectedIndex = 0;
            cbTraducedLanguage.SelectedIndex = 1;
            cbWorkingLanguage.SelectedIndex = 2;
            this.verificatore = new Verifier();

            LoadUserSettings();
        }

        internal void LoadUserSettings()
        {
            string originalFolder = ConfigurationManager.AppSettings["OriginalFolder"];
            string translatedFolder = ConfigurationManager.AppSettings["TranslatedFolder"];
            string workingFolder = ConfigurationManager.AppSettings["WorkingFolder"];

            this.tbPathOrigin.Text = originalFolder;
            this.tbPathTranslated.Text = translatedFolder;
            this.tbPathWorking.Text = workingFolder;
        }

        public void SaveUserSettings()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["OriginalFolder"].Value = tbPathOrigin.Text;
            config.AppSettings.Settings["TranslatedFolder"].Value = tbPathTranslated.Text;
            config.AppSettings.Settings["WorkingFolder"].Value = tbPathWorking.Text;
            config.Save(ConfigurationSaveMode.Modified);

            // Ricarica le impostazioni di configurazione per applicarle
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void btnSelectOriginal_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialogFra.ShowDialog((IWin32Window)this) != DialogResult.OK)
                return;
            try
            {
                this.tbPathOrigin.Text = this.folderBrowserDialogFra.SelectedPath;
            }
            catch (Exception ex)
            {
                this.txtOutput.AppendText(ex.ToString());
            }
        }

        private void btnSelectTranslated_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialogIta.ShowDialog((IWin32Window)this) != DialogResult.OK)
                return;
            try
            {
                this.tbPathTranslated.Text = this.folderBrowserDialogIta.SelectedPath;
            }
            catch (Exception ex)
            {
                this.txtOutput.AppendText(ex.ToString());
            }
        }

        private void btnSelectWorking_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialogIta.ShowDialog((IWin32Window)this) != DialogResult.OK)
                return;
            try
            {
                this.tbPathWorking.Text = this.folderBrowserDialogIta.SelectedPath;
            }
            catch (Exception ex)
            {
                this.txtOutput.AppendText(ex.ToString());
            }
        }

        private void btnVerifyOriTra_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtOutput.Clear();
                this.txtOutput.AppendText("Call verify Original Vs Translated " + this.tbPathOrigin.Text + " " + this.tbPathWorking.Text + Environment.NewLine);
                int filesToBeDeleted;
                int folderToBeDeleted;
                List<string> stringList = verificatore.MatchOriginalVsTranslated(tbPathOrigin.Text, (string)cbOriginLanguage.SelectedItem, tbPathTranslated.Text, (string)cbTraducedLanguage.SelectedItem, tbPathWorking.Text, (string)this.cbWorkingLanguage.SelectedItem, false);
                foreach (string str in stringList)
                    this.txtOutput.AppendText(str + Environment.NewLine);
                this.txtOutput.AppendText("End call verify Original Vs Translated " + this.tbPathOrigin.Text + " " + this.tbPathWorking.Text + Environment.NewLine);
            }
            catch (Exception ex)
            {
                this.txtOutput.AppendText(ex.ToString());
            }
        }

        private void btnCreateDb_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtOutput.Clear();
                this.txtOutput.AppendText("Call create db Original Vs Translated " + this.tbPathOrigin.Text + " " + this.tbPathWorking.Text + Environment.NewLine);
                int filesToBeDeleted;
                int folderToBeDeleted;
                List<string> stringList = verificatore.MatchOriginalVsTranslated(tbPathOrigin.Text, (string)cbOriginLanguage.SelectedItem, tbPathTranslated.Text, (string)cbTraducedLanguage.SelectedItem, tbPathWorking.Text, (string)this.cbWorkingLanguage.SelectedItem, true);
                foreach (string str in stringList)
                    this.txtOutput.AppendText(str + Environment.NewLine);
                this.txtOutput.AppendText("End call create db Original Vs Translated " + this.tbPathOrigin.Text + " " + this.tbPathWorking.Text + Environment.NewLine);
            }
            catch (Exception ex)
            {
                this.txtOutput.AppendText(ex.ToString());
            }
        }

        private void btnCreateFilesFromDb_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtOutput.Clear();
                this.txtOutput.AppendText("Call create files from db and Original " + this.tbPathOrigin.Text + " " + this.tbPathWorking.Text + Environment.NewLine);
                int filesToBeDeleted;
                int folderToBeDeleted;
                List<string> stringList = verificatore.CreateFromDb(tbPathOrigin.Text, (string)cbOriginLanguage.SelectedItem, tbPathWorking.Text, (string)this.cbWorkingLanguage.SelectedItem);
                foreach (string str in stringList)
                    this.txtOutput.AppendText(str + Environment.NewLine);
                this.txtOutput.AppendText("End call create files from db and Original " + this.tbPathOrigin.Text + " " + this.tbPathWorking.Text + Environment.NewLine);
            }
            catch (Exception ex)
            {
                this.txtOutput.AppendText(ex.ToString());
            }
        }
    }
}
