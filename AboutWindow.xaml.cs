// Copyright (C) 2026 cinm-25h2
// SPDX-License-Identifier: GPL-3.0-only

using System.Diagnostics;
using System.Windows;

namespace SymbolicLinkCreater
{
    /// <summary>
    /// Window1.xaml の相互作用ロジック
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }
        private void btn_OK(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = e.Uri.AbsoluteUri,
                UseShellExecute = true,
                WorkingDirectory = Environment.CurrentDirectory,
            };
            Process.Start(psi);
        }

        private void Hyperlink_LookLICENSE(object sender, RoutedEventArgs e)
        {
            LicenseWindow window = new LicenseWindow(true);
            window.Owner = this;
            window.ShowDialog();
        }

        private void Hyperlink_LookTPL(object sender, RoutedEventArgs e)
        {
            LicenseWindow window = new LicenseWindow(false);
            window.Owner = this;
            window.ShowDialog();
        }
    }
}
