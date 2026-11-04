using System.IO;
using System.Windows;


namespace SymbolicLinkCreater
{
    /// <summary>
    /// LicenseWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class LicenseWindow : Window
    {
        public LicenseWindow(bool FPLorTPL)
        {
            InitializeComponent();

            if (FPLorTPL)
            {
                LicenceText();
            }
            else
            {
                ThirdPartyText();
            }
        }

        private void LicenceText()
        {
            try
            {
                var resource = Application.GetResourceStream(
                new Uri("pack://application:,,,/LICENSE.txt"));

                using var reader = new StreamReader(resource!.Stream);
                LicenseTextBox.Text = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                LicenseTextBox.Text = "ライセンスファイルが見つかりませんでした\n" + ex.Message;
            }
        }

        private void ThirdPartyText()
        {
            try
            {
                var resource = Application.GetResourceStream(
                new Uri("pack://application:,,,/ThirdPartyNotices.txt"));

                using var reader = new StreamReader(resource!.Stream);
                LicenseTextBox.Text = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                LicenseTextBox.Text = "サードパーティ通知ファイルが見つかりませんでした\n" + ex.Message;
            }
        }
    }
}
