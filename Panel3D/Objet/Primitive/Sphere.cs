using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


class Sphere : Object3D
{
    public float size;

    public Vector3 position;

    private Material matC;

    private Mesh meshC;

    private Matrix transform;

    public Sphere(Panel3D _panel3D, Vector3 _position, float _size)
        : base(_panel3D)
    {


        position = _position;
        size = _size;

        meshC = Mesh.Sphere(device, size, 10, 10);

        CreateMaterial();
        CalculateAngle();
        CalculateTransform();
    }

    void CreateMaterial()
    {
        matC = new Material();
        matC.Ambient = Color.Blue; //on affecte au material qui va recouviri la sphere la couleur de la boules concernée
        matC.Diffuse = Color.Blue;// pareil pour la couleur diffuse
    }

    void CalculateTransform()
    {
        transform = Matrix.Multiply(Matrix.Identity, Matrix.Translation(position));
    }

    void CalculateAngle()
    {
        //Vector3 dir = new Vector3(0, 0, -1f);
        //axisRotation = productVect(dir, direction);
        //angle = (float)Math.Acos(productSca(dir, direction));
    }

    Vector3 productVect(Vector3 v1, Vector3 v2)
    {
        float X = v1.Y * v2.Z - v1.Z * v2.Y;
        float Y = v1.Z * v2.X - v1.X * v2.Z;
        float Z = v1.X * v2.Y - v1.Y * v2.X;
        return new Vector3(X, Y, Z);
    }

    float productSca(Vector3 v1, Vector3 v2)
    {
        return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
    }

    public override void draw()
    {
        panel3D.activeLight(true);

        device.RenderState.FillMode = FillMode.Solid;

        device.Material = matC;
        device.Transform.World = transform;
        meshC.DrawSubset(0);

        panel3D.activeLight(false);
    }
}
