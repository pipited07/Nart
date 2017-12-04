﻿using HelixToolkit.Wpf.SharpDX;
using Nart.Model_Object;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nart
{
    public class Projectata : ObservableObject
    {
        private string name = "蔡慧君";
        private string id = "123456";
        private string institution = "成大";
        private bool canSelectPoints = false;
        private string selectPointState = "OFF";
        private  ObservableCollection<BallModel> ballCollection=  new ObservableCollection<BallModel>();

        public Projectata()
        {

            Random crandom = new Random();
            for (int i = 0; i < 5; i++)
            {
                BallModel ball = new BallModel();
                ball.BallName = i.ToString() /*+ "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"*/;
                ball.BallInfo = "!!!!!!!!!!!!!!!!!!!!!!!!!!";

                var b1 = new MeshBuilder();

                int a =crandom.Next() % 100;
                int b = crandom.Next() % 100;
                int c = crandom.Next() % 100;
                ball.ModelCenter = new Vector3(a, b, c);
                b1.AddSphere(new Vector3(a, b, c), 5);
                ball.Geometry = b1.ToMeshGeometry3D();
                ball.Material =PhongMaterials.White;
                ball.Transform = new System.Windows.Media.Media3D.MatrixTransform3D();
                

                //MultiAngleViewModel.NormalModelCollection.Add(ball);

                BallCollection.Add(ball);
            }
        }


        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                SetValue(ref name, value);
            }
        }
        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                SetValue(ref id, value);
            }
        }
        public string Institution
        {
            get
            {
                return institution;
            }
            set
            {
                SetValue(ref institution, value);
            }
        }
        public bool CanSelectPoints
        {
            get
            {
                return canSelectPoints;
            }
            set
            {
                SetValue(ref canSelectPoints, value);
                if (canSelectPoints)
                {
                    SelectPointState = "ON";
                }
                else
                {
                    SelectPointState = "OFF";
                }
            }
        }
        public string SelectPointState
        {
            get
            {
                return selectPointState;
            }
            set
            {
                SetValue(ref selectPointState, value);
            }
        }
        public  ObservableCollection<BallModel> BallCollection
        {
            get
            {
                return ballCollection;
            }
            set
            {
                SetValue(ref ballCollection, value);
            }
        } 

    }
}
