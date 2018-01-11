﻿using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media.Animation;
using Nart.Model_Object;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Soap;

namespace Nart
{
    /// <summary>
    /// 此類別用來記錄各種MainView的屬性
    /// </summary>
    public class MainViewModel : ObservableObject
    {
        public static ProjectData Data = new ProjectData();
        public CameraControl CamCtrl;
        public NartServer Server= new NartServer();
        private readonly MainView _mainWindow;
        private static string _pointNumber;
        private static int _tabIndex = 1; //預設tab頁面索引值        
        private ModelSettingView _modelSettingdlg;
        private NavigateView _navigatedlg;
        



        public MainViewModel(MainView mainWindow)
        {
            _mainWindow = mainWindow;
            SetModelCommand = new RelayCommand(SetModel);
            RegisterCommand = new RelayCommand(Register);
            SetNavigationCommand = new RelayCommand(SetNavigation);
            TrackCommand = new RelayCommand(Track);
            CloseWindowCommand = new RelayCommand(this.OnClosed, null);
            DeleteBallCommad = new RelayCommand(DeleteBallItem);
            DeleteBoneCommad = new RelayCommand(DeleteBoneItem);
            SaveProjectCommand = new RelayCommand(SaveProject);
            LoadProjectCommand = new RelayCommand(LoadProject);

            BindPatientData();
            BindBallData();
            BindBoneData();

            //mainWindow.ExpanderInfo.BindPatientInfo(Data);
            //mainWindow.ExpanderNavigationBalls .BindBallCollection(Data);
            //mainWindow.ExpanderTargetModel.BindBoneCollection(Data);

        }
                
        public static int TabIndex
        {
            get
            {
                return _tabIndex;
            }
            set
            {
                SetStaticValue(ref _tabIndex, value);
            }
        }
        public static string PointNumber
        {
            get { return _pointNumber; }
            set
            {
                SetStaticValue(ref _pointNumber, value);
            }
        }

        public ICommand SaveProjectCommand { private set; get; }
        public ICommand LoadProjectCommand { private set; get; }
        public ICommand SetModelCommand { private set; get; }
        public ICommand SetNavigationCommand { private set; get; }
        /// <summary>
        /// 註冊按鈕
        /// </summary>
        public ICommand RegisterCommand { private set; get; }
        /// <summary>
        /// 追蹤按鈕
        /// </summary>
        public ICommand TrackCommand { private set; get; }
        /// <summary>
        /// 關閉程式
        /// </summary>
        public ICommand CloseWindowCommand { private set; get; }
        /// <summary>
        /// 刪除球Item的Command        
        /// </summary>
        public ICommand DeleteBallCommad { private set; get; }
        /// <summary>
        /// 刪除骨骼模型的Command        
        /// </summary>
        public ICommand DeleteBoneCommad { private set; get; }

        public void InitCamCtrl()
        {

            CamCtrl = new CameraControl(new TIS.Imaging.ICImagingControl[2] { _mainWindow.CamHost1.IcImagingControl, _mainWindow.CamHost2.IcImagingControl });
            CamCtrl.CameraStart();
        }
        /// <summary>
        /// 顯示設置好的各項模型資訊，按下Set Model 之後並且按ok後會走到這
        /// </summary>       
        private void SetModel(object o)
        {
            if (_modelSettingdlg == null)
            {
                _modelSettingdlg = new ModelSettingView();
            }
            _modelSettingdlg.Owner = _mainWindow;

            _modelSettingdlg.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            _modelSettingdlg.ShowDialog();

            RestoreGridLength();
        }
        private void SetNavigation(object o)
        {
            if (_navigatedlg == null)
            {
                _navigatedlg = new NavigateView();
            }
            _navigatedlg.Owner = _mainWindow;

            _navigatedlg.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            _navigatedlg.ShowDialog();

            RestoreGridLength();
        }
        private void Register(object o)
        {
            CameraControl.RegToggle = !CameraControl.RegToggle;
            RestoreGridLength();
        }
        private void Track(object o)
        {
            CameraControl.TrackToggle = !CameraControl.TrackToggle;
            RestoreGridLength();
        }
        private void OnClosed(object o)
        {
            if (CamCtrl!=null)
            {
                CamCtrl.CameraClose();
            }
            System.Windows.Application.Current.Shutdown();
        }
        /// <summary>
        /// DeleteBallCommad 的實作內容，刪除球項目
        /// </summary>
        private void DeleteBallItem(object o)
        {
            ListView ballListView =  _mainWindow.BallListView;

            if (ballListView.SelectedItem != null)
            {
                //選擇的BallModel
                BallModel selectedModelItem = (BallModel)ballListView.SelectedItem;

                int temp = ballListView.SelectedIndex;

                ObservableCollection<BallModel> ballCollection = MainViewModel.Data.BallCollection;

                ballCollection.Remove(selectedModelItem);

                //刪減之後數量若跟舊的索引值一樣，代表選項在最後一個
                if (ballCollection.Count == temp)
                {
                    ballListView.SelectedIndex = ballCollection.Count - 1;
                }
                else//不是的話則維持原索引值
                {
                    ballListView.SelectedIndex = temp;
                }

                ListViewItem item = ballListView.ItemContainerGenerator.ContainerFromIndex(ballListView.SelectedIndex) as ListViewItem;
                if (item != null)
                {
                    item.Focus();
                }

            }
        }
        /// <summary>
        /// DeleteBoneCommad 的實作內容，刪除Bone模型項目
        /// </summary>
        private void DeleteBoneItem(object o)
        {
            ListView boneListView = _mainWindow.BoneListView;

            if (boneListView.SelectedItem != null)
            {
                //選擇的BoneModel
                BoneModel selectedModelItem = (BoneModel)boneListView.SelectedItem;

                int temp = boneListView.SelectedIndex;

                var ballCollection = MainViewModel.Data.BoneCollection;

                ballCollection.Remove(selectedModelItem);

                //刪減之後數量若跟舊的索引值一樣，代表選項在最後一個
                if (ballCollection.Count == temp)
                {
                    boneListView.SelectedIndex = ballCollection.Count - 1;
                }
                else//不是的話則維持原索引值
                {
                    boneListView.SelectedIndex = temp;
                }
                ListViewItem item = boneListView.ItemContainerGenerator.ContainerFromIndex(boneListView.SelectedIndex) as ListViewItem;
                item?.Focus();
            }
        }
        private void SaveProject(object o)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "Nart_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".nart",
                DefaultExt = ".nart",
                Filter = "Nart Project Files (.nart)|*.nart"
            };
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                // 建立暫存目錄
                string fullFilePath = dlg.FileName;//完整路徑+副檔名
                if (File.Exists(fullFilePath))
                {
                    System.IO.File.Delete(fullFilePath);
                }

                string projectName = System.IO.Path.GetFileNameWithoutExtension(fullFilePath);//檔名不包含副檔名
                string filePath = System.IO.Path.GetDirectoryName(fullFilePath);//路徑
                string tempDirectory = System.IO.Path.Combine(filePath, projectName);//路徑+檔名(不包含副檔名)

                if (File.Exists(filePath))
                {
                    System.IO.Directory.Delete(filePath);
                }

                //先創建一個資料夾
                if (System.IO.Directory.Exists(tempDirectory) == true)
                {
                    System.IO.Directory.Delete(tempDirectory);
                }
                else
                {
                    System.IO.Directory.CreateDirectory(tempDirectory);
                }

                // 專案檔輸出                             
                string xmlFilePah = System.IO.Path.Combine(tempDirectory, projectName) + ".xml";

                using (FileStream myFileStream = new FileStream(xmlFilePah, FileMode.Create))
                {
                    SoapFormatter soapFormatter = new SoapFormatter();
                    soapFormatter.Serialize(myFileStream, MainViewModel.Data);
                    myFileStream.Close();
                }

                foreach (BoneModel boneModel in MainViewModel.Data.BoneCollection)
                {
                    boneModel.SaveModel(tempDirectory, false);
                }

                


                ZipFile.CreateFromDirectory(tempDirectory, fullFilePath);
                System.IO.Directory.Delete(tempDirectory, true);
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }
        private void LoadProject(object o)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".nart",
                Multiselect = false,
                Filter = "Nart Project Files (.nart)|*.nart"
            };

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                string fullFilePath = dlg.FileName;

                switch (System.IO.Path.GetExtension(fullFilePath).ToLower())
                {

                    case ".nart":
                        {

                            string projectName = System.IO.Path.GetFileNameWithoutExtension(fullFilePath);//檔名不包含副檔名
                            string filePath = System.IO.Path.GetDirectoryName(fullFilePath);//路徑
                            string tempDirectory = System.IO.Path.Combine(filePath, projectName);//路徑+檔名(不包含副檔名)

                            Directory.CreateDirectory(tempDirectory);
                            ZipFile.ExtractToDirectory(fullFilePath, tempDirectory);

                            string xmlFilePath = System.IO.Path.Combine(tempDirectory, projectName + ".xml");
                            if (!File.Exists(xmlFilePath))
                            {
                                return;
                            }

                            ProjectData projectData;
                            using (FileStream myFileStream = new FileStream(xmlFilePath, FileMode.Open))
                            {
                                SoapFormatter soapFormatter = new SoapFormatter();
                                projectData = (ProjectData)soapFormatter.Deserialize(myFileStream);
                                myFileStream.Close();
                            }


                            foreach (BoneModel boneModel in projectData.BoneCollection)
                            {
                                //BoneModel boneModel = projectData.BoneCollection[i];
                                boneModel.FilePath = System.IO.Path.Combine(tempDirectory, boneModel.SafeFileName);
                                boneModel.LoadModel();
                                if (boneModel.ModelType == ModelType.MovedMaxilla)
                                {                                   
                                    foreach (BoneModel targetModel in projectData.TargetCollection)
                                    {
                                        if (targetModel.ModelType == ModelType.TargetMaxilla)
                                        {
                                            targetModel.FilePath = boneModel.FilePath;
                                            targetModel.LoadModel();
                                        }
                                    }
                                }
                                else if (boneModel.ModelType == ModelType.MovedMandible)
                                {
                                    foreach (BoneModel targetModel in projectData.TargetCollection)
                                    {
                                        if (targetModel.ModelType == ModelType.TargetMandible)
                                        {
                                            targetModel.FilePath = boneModel.FilePath;
                                            targetModel.LoadModel();
                                        }
                                    }
                                }
                            }

                          

                            MainViewModel.Data.UpdateData(projectData);
                            MultiAngleViewModel.ResetCameraPosition();
                            System.IO.Directory.Delete(tempDirectory, true);
                            break;
                        }

                }
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }
        /// <summary>
        /// 將Grid回復到原始狀態 
        /// </summary>
        private void RestoreGridLength()
        {
            //Col2.Width = new GridLength(Col2.ActualWidth, GridUnitType.Pixel);

            GridLengthAnimation gla =
                new GridLengthAnimation
                {
                    From = new GridLength(_mainWindow.Col0.ActualWidth, GridUnitType.Pixel),
                    To = new GridLength(0, GridUnitType.Pixel),
                    Duration = new TimeSpan(0, 0, 0, 0, 150),
                    FillBehavior = FillBehavior.HoldEnd
                };

            GridLengthAnimation gla2 =
                new GridLengthAnimation
                {
                    From = new GridLength(_mainWindow.Col1.ActualWidth, GridUnitType.Pixel),
                    To = new GridLength(_mainWindow.Col0.ActualWidth, GridUnitType.Pixel),
                    Duration = new TimeSpan(0, 0, 0, 0, 150),
                    FillBehavior = FillBehavior.HoldEnd
                };

            _mainWindow.Col0.BeginAnimation(ColumnDefinition.WidthProperty, gla);
            _mainWindow.Col1.BeginAnimation(ColumnDefinition.WidthProperty, gla2);
            //GridLengthAnimation gla = new GridLengthAnimation
            //{
            //    From = _mainWindow.Col0.Width,
            //    To = new GridLength(0, GridUnitType.Star),
            //    Duration = new TimeSpan(0, 0, 0, 0, 100),
            //    FillBehavior = FillBehavior.Stop
            //};
            //gla.Completed += (s, e) =>
            //{
            //    _mainWindow.Col0.Width = gla.To;
            //};
            //_mainWindow.MainGrid.ColumnDefinitions[0].BeginAnimation(ColumnDefinition.WidthProperty, gla);


            //GridLengthAnimation gla2 = new GridLengthAnimation
            //{
            //    From = _mainWindow.Col1.Width,
            //    To = new GridLength(0, GridUnitType.Auto),
            //    Duration = new TimeSpan(0, 0, 0, 0, 100),
            //    FillBehavior = FillBehavior.Stop
            //};
            //gla2.Completed += (s, e) =>
            //{
            //    _mainWindow.Col1.Width = gla2.To;
            //};
            //_mainWindow.MainGrid.ColumnDefinitions[1].BeginAnimation(ColumnDefinition.WidthProperty, gla2);
        }






        /// <summary>
        /// Bind Patient Information expander裡面的textbox 
        /// </summary>
        private void BindPatientData()
        {
            Binding binding1 = new Binding("Name");
            binding1.Source = Data;
            binding1.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(_mainWindow.NameTB, TextBlock.TextProperty, binding1);

            Binding binding2 = new Binding("ID");
            binding2.Source = Data;
            binding2.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(_mainWindow.IDTB, TextBlock.TextProperty, binding2);

            Binding binding3 = new Binding("Institution");
            binding3.Source = Data;
            binding3.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(_mainWindow.InstitutionTB, TextBlock.TextProperty, binding3);
        }
        /// <summary>
        /// Bind Navigation balls expander裡面的listview跟switch toggle 
        /// </summary>
        private void BindBallData()
        {
            Binding binding = new Binding("BallCollection");
            binding.Source = Data;
            binding.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(_mainWindow.BallListView, ItemsControl.ItemsSourceProperty, binding);

            Binding binding2 = new Binding("CanSelectPoints");
            binding2.Source = Data;
            binding2.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(_mainWindow.SelectTB, ToggleButton.IsCheckedProperty, binding2);

            Binding binding3 = new Binding("SelectPointState");
            binding3.Source = Data;
            binding3.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(_mainWindow.stateTB, TextBlock.TextProperty, binding3);

            Binding binding4 = new Binding("BallCollection");
            binding4.Source = Data;
            binding4.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(_mainWindow.multiAngleView.BallCollection, ItemsModel3D.ItemsSourceProperty, binding4);


        }
        /// <summary>
        /// Bind bone Model expander裡面的listview 
        /// </summary>
        private void BindBoneData()
        {
            //將data中的BoneCollection綁到此控制項的item上面   
            Binding binding = new Binding("BoneCollection");
            binding.Source = Data;
            binding.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(_mainWindow.BoneListView, ItemsControl.ItemsSourceProperty, binding);

            Binding binding2 = new Binding("BoneCollection");
            binding2.Source = Data;
            binding2.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(_mainWindow.multiAngleView.BoneCollection, ItemsModel3D.ItemsSourceProperty, binding2);

            Binding binding3 = new Binding("TargetCollection");
            binding3.Source = Data;
            binding3.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(_mainWindow.TargetModelListView, ItemsControl.ItemsSourceProperty, binding3);

            Binding binding4 = new Binding("TargetCollection");
            binding4.Source = Data;
            binding4.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(_mainWindow.multiAngleView.Targetollection, ItemsModel3D.ItemsSourceProperty, binding4);
        }


        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        protected static void OnStaticPropertyChanged([CallerMemberName]string info = "")
        {
            if (StaticPropertyChanged != null)
            {
                StaticPropertyChanged(null, new PropertyChangedEventArgs(info));
            }
        }
        protected static bool SetStaticValue<T>(ref T oldValue, T newValue, [CallerMemberName]string propertyName = "")//CallerMemberName主要是.net4.5後定義好的caller訊息，能將訊息傳給後者的變數，目的在使用時不用特地傳入"Property"名稱
        {
            if (object.Equals(oldValue, newValue))
            {
                return false;
            }
            oldValue = newValue;
            OnStaticPropertyChanged(propertyName);
            return true;
        }

    }
}
