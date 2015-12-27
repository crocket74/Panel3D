using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


class Mesure : Object3D
{
    public float size;
    public float erreur;

    public Vector3 position;
    public Vector3 direction;
    private Material matC;
    private Material matD;
    private Material matU;

    private Mesh meshC;
    private Mesh meshD;
    private Mesh meshU;
    private Vector3 axisRotation;
    private float angle;

    private Matrix transform;

    public Mesure(Panel3D _panel3D, Vector3 _position, Vector3 _direction,float _erreur, float _size)
        : base(_panel3D)
    {
        direction = _direction;
        direction.Scale(1f);

        position = _position;
        erreur = _erreur;
        size = _size;

        meshC = Mesh.Cylinder(device, size, size, 0.005f, 10, 10);
        meshU = Mesh.Cylinder(device, size / 2, size / 2, 0.005f, 10, 10);
        meshD = Mesh.Cylinder(device, size / 2, size / 2, 0.005f, 10, 10);

        CreateMaterial();
        CalculateAngle();
        CalculateTransform();
    }

    void CreateMaterial()
    {
        matC = new Material();
        matC.Ambient = Color.Black; //on affecte au material qui va recouviri la sphere la couleur de la boules concernée
        matC.Diffuse = Color.Black;// pareil pour la couleur diffuse

        matU = new Material();
        matU.Ambient = Color.Green; //on affecte au material qui va recouviri la sphere la couleur de la boules concernée
        matU.Diffuse = Color.Green;// pareil pour la couleur diffuse

        matD = new Material();
        matD.Ambient = Color.Red; //on affecte au material qui va recouviri la sphere la couleur de la boules concernée
        matD.Diffuse = Color.Red;// pareil pour la couleur diffuse
    }

    void CalculateTransform()
    {
        transform = Matrix.Multiply(Matrix.RotationAxis(axisRotation, angle), Matrix.Translation(position));
    }

    void CalculateAngle()
    {
        Vector3 dir = new Vector3(0, 0, -1f);
        axisRotation = productVect(dir, direction);
        angle = (float)Math.Acos(productSca(dir, direction));
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

        device.Material = matU;
        device.Transform.World = Matrix.Multiply(transform,Matrix.Translation(erreur*direction));
        meshU.DrawSubset(0);

        device.Material = matD;
        device.Transform.World = Matrix.Multiply(transform, Matrix.Translation(-erreur * direction));
        meshD.DrawSubset(0);

        panel3D.activeLight(false);
    }
}
