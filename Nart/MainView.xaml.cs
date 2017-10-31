﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TIS.Imaging;
using UseCVLibrary;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Threading;
using System.ComponentModel;
using HelixToolkit.Wpf.SharpDX;
using System.Diagnostics;
using NartControl.Control;
using Nart.ExtensionMethods;
using Nart.Model_Object;

namespace Nart
{
    /// <summary>
    /// MainView主視窗介面
    /// </summary>
    public partial class MainView : Window
    {
        
        private MainViewModel _mainViewModel = null;
        public MainView()
        {
            InitializeComponent();
          
            _mainViewModel = new MainViewModel(this);

            this.DataContext = _mainViewModel;

            AllocConsole();
        }        
        
        private void Translate_Click(object sender, RoutedEventArgs e)
        {


            //Matrix3D TEST = new Matrix3D(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, -100, -100, 100, 1);
    

            //BoneModel test = MultiAngleViewModel.BoneModelCollection[0] as BoneModel;

            //test.Transform= new MatrixTransform3D(TEST);
            //MainViewModel.ModelDataCollection = multiAngleView._multiAngleViewModel.ModelDataCollection;

            //SharpDX.Vector3 temp = MainViewModel.ModelDataCollection[0].ModelGeometry.Positions[0];

            //Point3D oldPoint = new Point3D(temp.X,temp.Y,temp.Z);

            //Console.WriteLine("oldPoint:" + oldPoint);

            //for (int i = 0; i < MainViewModel.ModelDataCollection.Count; i++)
            //{
            //    MainViewModel.ModelDataCollection[i].ModelTransform = new MatrixTransform3D(TEST);
            //}

            //SharpDX.Vector3 temp2 = MainViewModel.ModelDataCollection[0].ModelGeometry.Positions[0];

            //Point3D newPoint = new Point3D(temp2.X, temp2.Y, temp2.Z);


            //Console.WriteLine("newPoint:" + newPoint);

            //multiAngleView._multiAngleViewModel.ModelDataCollection = MainViewModel.ModelDataCollection;


            // Console.WriteLine(MainViewModel.ModelDataCollection[0].ModelGeometry.Positions[0].X);

        }
        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            CameraControl.RegToggle = !CameraControl.RegToggle;
        }
        private void TrackingButton_Click(object sender, RoutedEventArgs e)
        {
            CameraControl.TrackToggle = !CameraControl.TrackToggle;
        }
        private void load_Closed(object sender, EventArgs e)
        {
            //CamCtrl.CameraClose();            
            System.Windows.Application.Current.Shutdown();
        }
        private void OpenCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Title = "New Project";
            dlg.DefaultExt = ".stl";
            dlg.Multiselect = true;
            dlg.Filter = "STL File (.stl)|*.stl";


            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                foreach (String filename in dlg.FileNames)
                {
                    
                }

            }
        }
        private void NewCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }       
        private void CamHost1_Loaded(object sender, RoutedEventArgs e)
        {
            if (!CamHost1.IsActivated)
            {
                CamHost1.InitializeCamSetting(CamHost1.ActualWidth, CamHost1.ActualHeight);
            }
        }
        private void CamHost2_Loaded(object sender, RoutedEventArgs e)
        {
            if (!CamHost2.IsActivated)
            {
                CamHost2.InitializeCamSetting(CamHost2.ActualWidth, CamHost2.ActualHeight);
                _mainViewModel.InitCamCtrl();
                CamHost1.IsActivated = true;
                CamHost2.IsActivated = true;
            }
        }       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
        

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        [DllImport("Kernel32")]
        public static extern void FreeConsole();

      
    }
}
