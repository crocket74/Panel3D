using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using D3D = Microsoft.DirectX.Direct3D;


class Text3Dxy : Object3D
{
    public Vector3 position;
    public Vector3 direction;

    public String text;
    
    private float scale;

    private Material mat;
    private Mesh mesh;
    private Vector3 axisRotation;
    private float angle;

    private Matrix transform;


    public Text3Dxy(Panel3D _Panel3D, Vector3 _position, Vector3 _direction, float _scale, String _text)
        : base(_Panel3D)
    {
        position = _position;
        direction = _direction;
        scale = _scale;

        text = _text;
        System.Drawing.Font p = new System.Drawing.Font("Arial", 10f);
        mesh = Mesh.TextFromFont(panel3D.device, p, text, 0.00001f, 0.05f);
        CreateMaterial();
        CalculateAngle();
        CalculateTransform();
    }

    void CreateMaterial()
    {
        mat = new Material();
        mat.Ambient = Color.Black; //on affecte au material qui va recouviri la sphere la couleur de la boules concernée
        mat.Diffuse = Color.Black;// pareil pour la couleur diffuse

    }

    void CalculateAngle()
    {
        Vector3 dir = new Vector3(0, 0, -1f);
        axisRotation = productVect(dir, direction);
        angle = (float)Math.Acos(productSca(dir, direction));
    }

    void CalculateTransform()
    {
        float angle2 = (float)Math.Acos(productSca(new Vector3(1f, 0f, 0f), direction));
        Vector3 sens = productVect(new Vector3(1f, 0f, 0f), direction);
        if (sens.Z < 0) angle2 = -angle2;

        transform = Matrix.Multiply(Matrix.Scaling(scale, scale, scale), Matrix.RotationAxis(new Vector3(1f,0f,0f), (float)Math.Acos(-1)));
        transform = Matrix.Multiply(transform, Matrix.RotationAxis(new Vector3(0f, 0f, 1f), angle2));
        transform = Matrix.Multiply(transform, Matrix.Translation(position));
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

        device.Material = mat;

        device.Transform.World = transform;
        mesh.DrawSubset(0);

        panel3D.activeLight(false);
    }


}

