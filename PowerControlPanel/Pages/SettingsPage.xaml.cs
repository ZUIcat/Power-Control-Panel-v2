﻿using ControlzEx.Theming;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Power_Control_Panel.PowerControlPanel.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            loadSettings();

            ThemeManager.Current.ChangeTheme(this, Properties.Settings.Default.systemTheme);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //Save Settings

            //General settings
            Properties.Settings.Default.systemTheme = cboLightDarkTheme.Text + "." + cboAccentTheme.Text;

            if (Properties.Settings.Default.systemAutoStart != cboAutoStart.Text)
            {
                Properties.Settings.Default.systemAutoStart = cboAutoStart.Text;
                changeTaskService(cboAutoStart.Text);
            }
            
            Properties.Settings.Default.maxTDP = (int)TDPMAX.Value;

            Properties.Settings.Default.sizeQAM = cboQAMSize.Text;


            //Save
            Properties.Settings.Default.Save();

            //Reapply theme
            ThemeManager.Current.ChangeTheme(this, Properties.Settings.Default.systemTheme);

            MessageBox.Show("Settings saved!");
        }

        private void loadSettings()
        {
            //Load general settings

            //handle theme
            int intPeriodLocation = Properties.Settings.Default.systemTheme.IndexOf(".");
            int intLengthTheme = Properties.Settings.Default.systemTheme.Length;
            cboAccentTheme.Text = Properties.Settings.Default.systemTheme.Substring(intPeriodLocation+1,intLengthTheme-(intPeriodLocation+1) );
            cboLightDarkTheme.Text = Properties.Settings.Default.systemTheme.Substring(0, intPeriodLocation);
            TDPMAX.Value = Properties.Settings.Default.maxTDP;

            cboQAMSize.Text = Properties.Settings.Default.sizeQAM;

            cboAutoStart.Text = Properties.Settings.Default.systemAutoStart;
        }
        private void changeTaskService(string systemAutoStart)
        {
            Microsoft.Win32.TaskScheduler.TaskService ts = new Microsoft.Win32.TaskScheduler.TaskService();
            Microsoft.Win32.TaskScheduler.Task task = ts.GetTask("Power_Control_Panel");
            string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
            if (task == null)
            {
                if (systemAutoStart == "Enable")
                {
                    TaskDefinition td = ts.NewTask();

                    td.RegistrationInfo.Description = "Power Control Panel";
                    td.Triggers.AddNew(TaskTriggerType.Logon);
                    td.Principal.RunLevel = TaskRunLevel.Highest;
                    td.Settings.DisallowStartIfOnBatteries = false;
                    td.Settings.StopIfGoingOnBatteries = false;
                    td.Settings.RunOnlyIfIdle = false;

                    td.Actions.Add(new ExecAction(BaseDir + "\\Power Control Panel.exe"));

                    Microsoft.Win32.TaskScheduler.TaskService.Instance.RootFolder.RegisterTaskDefinition("Power_Control_Panel", td);

                }
            }

            else
            {
                if (systemAutoStart == "Disable")
                {
                    task.RegisterChanges();
                    ts.RootFolder.DeleteTask("Power_Control_Panel");
                }
            }














        }


        private DependencyObject GetElementFromParent(DependencyObject parent, string childname)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is FrameworkElement childframeworkelement && childframeworkelement.Name == childname)
                    return child;

                var FindRes = GetElementFromParent(child, childname);
                if (FindRes != null)
                    return FindRes;
            }
            return null;
        }

        private void Slider_Loaded(object sender, RoutedEventArgs e)
        {
            var SliderThumb = GetElementFromParent(sender as DependencyObject, "HorizontalThumb"); //Make sure to put the right name for your slider layout options are: ("VerticalThumb", "HorizontalThumb")
            if (SliderThumb != null)
            {
                if (SliderThumb is Thumb thumb)
                {


                    thumb.Width = 20;
                    thumb.Height = 25;
                }
                else
                {
                    //SliderThumb is not an object of type Thumb
                }
            }
            else
            {
                //SliderThumb is null
            }
        }

        private void TDPMAX_Loaded(object sender, RoutedEventArgs e)
        {
            Slider_Loaded(sender, e);
        }
    }
}
