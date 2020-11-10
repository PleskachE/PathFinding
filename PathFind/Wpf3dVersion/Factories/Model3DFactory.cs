﻿using System.Windows.Media.Media3D;

namespace Wpf3dVersion.Factories
{
    internal static class Model3DFactory
    {
        internal static Model3D CreateCubicModel3D(double size, Material material)
        {
            var model = new Model3DGroup();

            var p0 = new Point3D(0, 0, 0);
            var p1 = new Point3D(size, 0, 0);
            var p2 = new Point3D(size, 0, size);
            var p3 = new Point3D(0, 0, size);
            var p4 = new Point3D(0, size, size);
            var p5 = new Point3D(size, size, size);
            var p6 = new Point3D(size, size, 0);
            var p7 = new Point3D(0, size, 0);
            
            model.Children.Add(CreateRectangleModel(p4, p3, p2, p5, material));
            model.Children.Add(CreateRectangleModel(p5, p2, p1, p6, material));
            model.Children.Add(CreateRectangleModel(p7, p6, p1, p0, material));
            model.Children.Add(CreateRectangleModel(p7, p0, p3, p4, material));
            model.Children.Add(CreateRectangleModel(p7, p4, p5, p6, material));
            model.Children.Add(CreateRectangleModel(p0, p1, p2, p3, material));

            return model;
        }

        private static GeometryModel3D CreateRectangleModel(
            Point3D p0, Point3D p1, 
            Point3D p2, Point3D p3, 
            Material material)
        {
            var mesh = new MeshGeometry3D();

            mesh.Positions.Add(p0);
            mesh.Positions.Add(p1);
            mesh.Positions.Add(p2);
            mesh.Positions.Add(p3);

            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(3);

            return new GeometryModel3D(mesh, material);
        }        
    }
}
