﻿using HelixToolkit.Wpf;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Core;
using NartControl;
using NartControl.Control;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;


namespace Nart
{
    using Model;
    using SharpDX.Direct3D11;
    using System.Windows;
    using System.Windows.Data;
    using Color = System.Windows.Media.Color;
    /// <summary>
    /// 模型設置清單中Listview中的項目
    /// </summary>
    public class ModelSettingItem : ObservableObject
    {
        /// <summary>
        /// 設置列當中的OSP模型
        /// </summary>
        public OSPModel OSP = new OSPModel();
        /// <summary>
        /// 設置列當中的Bone模型
        /// </summary>
        public BoneModel Bone = new BoneModel();
        /// <summary>
        /// 模型名稱
        /// </summary>
        private String boneFilePath;
        /// <summary>
        /// OSP名稱
        /// </summary>
        private String ospFilePath;
        /// <summary>
        /// Model 顏色
        /// </summary>
        private Color boneDiffuseColor;
        /// <summary>
        /// OSP 顏色
        /// </summary>
        private Color ospDiffuseColor;
        /// <summary>
        /// MarkerID 的值
        /// </summary>
        private String markerID;
        /// <summary>
        /// combobox選項的內容
        /// </summary>
        private List<String> comboboxList = MarkerDatabase.MarkerIDList;

                               
        //public ModelSettingItem()
        //{                       
            
        //}
        ///// <summary>
        ///// 設定模型材質
        ///// </summary>        
        //private void SetModelMaterial()
        //{
        //    if (Model.ModelMaterial == null)
        //    {
        //        Model.ModelMaterial = new HelixToolkit.Wpf.SharpDX.PhongMaterial();
        //        Model.ModelMaterial.ReflectiveColor = SharpDX.Color.Black;
        //        float ambient = 0.0f;
        //        Model.ModelMaterial.AmbientColor = new SharpDX.Color(ambient, ambient, ambient, 1.0f);
        //        Model.ModelMaterial.EmissiveColor = SharpDX.Color.Black; //這是自己發光的顏色
        //        int Specular = 90;
        //        Model.ModelMaterial.SpecularColor = new SharpDX.Color(Specular, Specular, Specular, 255);
        //        Model.ModelMaterial.SpecularShininess = 60;
        //    }

        //    Model.ModelMaterial.DiffuseColor = ModelDiffuseColor.ToColor4();
        //}
        ///// <summary>
        ///// 設定OSP材質
        ///// </summary>        
        //private void SetOSPMaterial()
        //{
        //    //if (OSP.ModelMaterial == null)
        //    //{
        //    //    OSP.ModelMaterial = new HelixToolkit.Wpf.SharpDX.PhongMaterial();
        //    //    OSP.ModelMaterial.ReflectiveColor = SharpDX.Color.Black;
        //    //    float ambient = 0.0f;
        //    //    OSP.ModelMaterial.AmbientColor = new SharpDX.Color(ambient, ambient, ambient, 1.0f);
        //    //    OSP.ModelMaterial.EmissiveColor = SharpDX.Color.Black; //這是自己發光的顏色
        //    //    int Specular = 90;
        //    //    OSP.ModelMaterial.SpecularColor = new SharpDX.Color(Specular, Specular, Specular, 255);
        //    //    OSP.ModelMaterial.SpecularShininess = 60;
        //    //}

        //    //OSP.ModelMaterial.DiffuseColor = OSPDiffuseColor.ToColor4();

        //    HelixToolkit.Wpf.SharpDX.PhongMaterial  material= new PhongMaterial();

        //    material.ReflectiveColor = SharpDX.Color.Black;
        //    float ambient = 0.0f;
        //    material.AmbientColor = new SharpDX.Color(ambient, ambient, ambient, 1.0f);
        //    material.EmissiveColor = SharpDX.Color.Black; //這是自己發光的顏色
        //    int Specular = 90;
        //    material.SpecularColor = new SharpDX.Color(Specular, Specular, Specular, 255);
        //    material.SpecularShininess = 60;
        //    material.DiffuseColor = OSPDiffuseColor.ToColor4();

        //    OSP2.Material = material;

        //}
        /// <summary>
        /// 設定好模型之後Load進去模型資料所用
        /// </summary>
        public void Load()
        {
            
            Bone.FilePath = BoneFilePath;
            Bone.MarkerID = MarkerID;
            Bone.DiffuseColor = BoneDiffuseColor;

            OSP.FilePath = OSPFilePath;
            OSP.MarkerID = MarkerID;
            OSP.DiffuseColor = OSPDiffuseColor;
            if (!OSP.IsLoaded)
            {
                OSP.LoadOSP();
            }

            if (!Bone.IsLoaded)
            {
                Bone.LoadModel();
            }

        }

        public String BoneFilePath
        {
            get
            {
                return boneFilePath;
            }
            set
            {
                SetValue(ref boneFilePath, value);
                Bone.IsLoaded = false;                
            }
        }
        public String OSPFilePath
        {
            get
            {
                return ospFilePath;
            }
            set
            {
                SetValue(ref ospFilePath, value);
                OSP.IsLoaded = false;
            }
        }
        public Color BoneDiffuseColor
        {
            get
            {
                return boneDiffuseColor;
            }
            set
            {
                SetValue(ref boneDiffuseColor, value);
                if (Bone.IsLoaded == true)
                {
                    Bone.DiffuseColor = boneDiffuseColor;
                    Bone.SetBoneMaterial();
                }
            }
        }
        public Color OSPDiffuseColor
        {
            get
            {
                return ospDiffuseColor;
            }
            set
            {
                //如果透明度太低則降低到50
                if (value.A>150)
                {
                    value.A = 50;
                }
                SetValue(ref ospDiffuseColor, value);
                if (OSP.IsLoaded == true)
                {
                    OSP.DiffuseColor = ospDiffuseColor;
                    OSP.SetOSPMaterial();
                }
            }
        }
        public String MarkerID
        {
            get
            {
                return markerID;
            }
            set
            {
                SetValue(ref markerID, value);
            }
        }
        public List<String> ComboBoxList
        {
            get
            {
                return comboboxList;
            }
            set
            {
                SetValue(ref comboboxList, value);
            }
        }
    }
}
