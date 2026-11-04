// Copyright (C) 2026 cinm-25h2
// SPDX-License-Identifier: GPL-3.0-only

using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace SymbolicLinkCreater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String cmd = "mklink";
        String LinkPath = "";
        String TargetPath = "";
        private bool _isInitializing = true;
        private bool IsAlreadyRan = false;

        public MainWindow()
        {
            InitializeComponent();
            _isInitializing = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("=================================");
            Console.WriteLine("==WELCOME TO SYMBOLIC LINK GUI===");
            Console.WriteLine("=================================");
        }

        private void btn_About(object sender, RoutedEventArgs e)  
        {
            AboutWindow window = new AboutWindow();
            window.Owner = this;
            window.ShowDialog();
        }
        private void btn_Exit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
            Console.WriteLine("Goodbye!");
        }
        private void btn_Start(object sender, RoutedEventArgs e)
        {
            if (!IsAlreadyRan) {
                IsAlreadyRan = true;
                txtbox_CommandLog.Text = ExecuteCommand(txtbox_Command.Text);
            } else {
                txtbox_CommandLog.Text = ExecuteCommand(txtbox_Command.Text) + "\n" + txtbox_CommandLog.Text;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshAll();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            RefreshAll();
        }

        private void RefreshTextBox_cmd(String Command)
        {
            txtbox_Command.Text = Command;
        }

        private void FilePicker_Link(object sender, RoutedEventArgs e)
        {
            txtLinkPath.Text = GetFolderPath(chkboxForDirectory.IsChecked == true || chkboxForDirJunction.IsChecked == true, "L");
            RefreshAll();
        }

        private void FilePicker_Target(object sender, RoutedEventArgs e)
        {
            txtTargetPath.Text = GetFolderPath(chkboxForDirectory.IsChecked == true || chkboxForDirJunction.IsChecked == true, "T");
            RefreshAll();
        }

        private static string ExecuteCommand(string command)
        {
            ProcessStartInfo processStartInfo = new("cmd.exe", "/c " + command)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process process = new()
            {
                StartInfo = processStartInfo
            };
            process.Start();

            Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
            Task<string> errorTask = process.StandardError.ReadToEndAsync();

            process.WaitForExit();

            string output = outputTask.GetAwaiter().GetResult();
            string error = errorTask.GetAwaiter().GetResult();

            // 標準出力と標準エラー出力を結合して返す
            return (output + Environment.NewLine + error).Trim();
        }

        private String GetFolderPath(Boolean IsFolder,string LinkorTarget)
        {
            if (IsFolder)
            {
                using (var dialog = new CommonOpenFileDialog()
                {
                    Title = "フォルダを選択",
                    IsFolderPicker = true,
                })
                {
                    if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        return (dialog.FileName);
                    }
                }
            }
            else
            {
                using (var dialog = new CommonOpenFileDialog()
                {
                    Title = "ファイルを選択",
                    IsFolderPicker = false,
                })
                {
                    if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        return (dialog.FileName);
                    }
                }
            }
            if (LinkorTarget == "L") {
                return (txtLinkPath.Text);
            } else if (LinkorTarget == "T") {
                return (txtTargetPath.Text);
            } else {
                return ("");
            }
        }

        private void RefreshAll()
        {
            if (_isInitializing) return;
            cmd = "mklink";
            LinkPath = "\"" + txtLinkPath.Text + "\"";
            TargetPath = "\"" + txtTargetPath.Text + "\"";

            if (chkboxForDefault.IsChecked == true)
            {
                cmd += "";
            }
            if (chkboxForDirectory.IsChecked == true)
            {
                cmd += " /D";
            }
            if (chkboxForHardlink.IsChecked == true)
            {
                cmd += " /H";
            }
            if (chkboxForDirJunction.IsChecked == true)
            {
                cmd += " /J";
            }
            cmd = cmd + " " + LinkPath + " " + TargetPath;

            RefreshTextBox_cmd(cmd);
        }
    }
}