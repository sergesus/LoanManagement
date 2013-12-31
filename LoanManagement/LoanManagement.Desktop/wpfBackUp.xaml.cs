using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.IO;

using System.Windows.Forms;

using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

namespace LoanManagement.Desktop
{
    /// <summary>
    /// Interaction logic for wpfBackUp.xaml
    /// </summary>
    public partial class wpfBackUp : Window
    {
        public wpfBackUp()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Backup bkp = new Backup();
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string PathtobackUp = fbd.SelectedPath.ToString();
                    string fileName = PathtobackUp + "myDB.bak";
                    string databaseName = "LoanManagement.Domain.newerContext";

                bkp.Action = BackupActionType.Database;
                bkp.Database = databaseName;
                bkp.Devices.AddDevice(fileName, DeviceType.File);

                //bkp.Incremental = chkIncremental.Checked;
                //this.progressBar1.Value = 0;
                //this.progressBar1.Maximum = 100;
                //this.progressBar1.Value = 10;

                //bkp.PercentCompleteNotification = 10;
                //bkp.PercentComplete += new PercentCompleteEventHandler(ProgressEventHandler);
                //bkp.Incremental = true;
                Server srv = new Server("(localdb)\\v11.0");
                bkp.SqlBackup(srv);
                System.Windows.MessageBox.Show("Database was successfully backed up to: " + fileName, "Info");
                }

                
            }

            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                //this.Cursor = Cursors.Default;
                //this.progressBar1.Value = 0;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Restore rest = new Restore();
            string PathtobackUp ="";
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Backup Files (*.bak)|*.bak|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathtobackUp = dlg.FileName.ToString();
            }
            else
            {
                return;
            }
            string fileName = PathtobackUp;

            //this.Cursor = Cursors.WaitCursor;
            //this.dataGridView1.DataSource = string.Empty;

            //try
            //{
                rest.Devices.AddDevice(fileName, DeviceType.File);
                Server srv = new Server("(localdb)\\v11.0");
                bool verifySuccessful = rest.SqlVerify(srv);
                string databaseName = "LoanManagement.Domain.newerContext";
                if (verifySuccessful)
                {
                    System.Windows.MessageBox.Show("Backup Verified!", "Info");
                    System.Windows.MessageBoxResult dr = System.Windows.MessageBox.Show("Do you want to restore?","Question",MessageBoxButton.YesNo);
                    if (dr == MessageBoxResult.Yes)
                    {
                        //fileName = dlg.FileName.Replace(Directory.GetCurrentDirectory(), "");
                        System.Windows.MessageBox.Show(fileName);
                        rest.Database = databaseName;
                        rest.Action = RestoreActionType.Database;
                        BackupDeviceItem bdi = default(BackupDeviceItem);
                        bdi = new BackupDeviceItem(fileName, DeviceType.File);
                        rest.Devices.Add(bdi);
                        //rest.Devices.Add(bdi);
                        rest.ReplaceDatabase = true;
                        srv = new Server("(localdb)\\v11.0");
                        rest.SqlRestore(srv);
                        srv.Refresh();
                        System.Windows.MessageBox.Show("Restore of " + databaseName +" Complete!");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("ERROR: Backup not verified!", "Error");
                }
            /*}
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                //this.Cursor = Cursors.Default;
            }*/
        }
    }
}
