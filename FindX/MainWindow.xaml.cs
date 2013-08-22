using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
namespace FindX
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            tbpath.Text = AppDomain.CurrentDomain.BaseDirectory;
            Paths = new List<string>();
        }
        public List<string> Paths { get; set; }
        private void ButtonFind_Click(object sender, RoutedEventArgs e)
        {
            if(Paths == null)
                Paths = new List<string>();
            else
            {
                Paths.Clear();
            }
            var path = tbpath.Text.Trim();
            Debug.Assert(path.Length > 0);
            lst.Items.Clear();
            if (Directory.Exists(path))
            {
                foreach (var item in Directory.GetDirectories(path))
                {
                    Console.WriteLine(item);
                    if (CheckDirectoryEmpty(item))
                    {
                        Paths.Add(item);
                        lst.Items.Add(item);
                    }
                }
                if (lst.Items.Count == 0)
                    lst.Items.Add("没有找到空目录");
                   
            }
            else
            {
                lst.Items.Add("不存在此目录");
            }
        }

        private bool CheckDirectoryEmpty(string path)
        {
            try
            {
                if (Directory.GetDirectories(path).Length > 0)
                    return false;
                else if (Directory.GetFiles(path).Length > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {                
                return false;
            }
           
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if (Paths.Count > 0)
            {
                try
                {
                    foreach (var item in Paths)
                    {
                        lst.Items.Remove(item);
                        Directory.Delete(item);                        
                    }
                }
                finally
                {
                    System.Windows.MessageBox.Show("删除完毕");
                }       
            }
            else
            {
                System.Windows.MessageBox.Show("没有找到空目录");
            }
        }

        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbpath.Text = fbd.SelectedPath;
                lst.Items.Clear();
            }
               
        }

        private void Window_DragLeave(object sender, System.Windows.DragEventArgs e)
        {
            string[] files = e.Data.GetData(System.Windows.DataFormats.FileDrop) as string[];
            foreach (var item in files)
            {
                if (Directory.Exists(item))
                {
                    tbpath.Text = item;
                    break;
                }
            }
        }
    }
}
