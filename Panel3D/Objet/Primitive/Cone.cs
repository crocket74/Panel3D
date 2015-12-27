using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


class Cone : Object3D
{
    public float radius;
    public float height;
    
    public Vector3 position;
    public Vector3 direction;
    private Material mat;
    private Mesh mesh;
    private Vector3 axisRotation;
    private float angle;

    public Cone(Panel3D _panel3D, float _radius, float _height, Vector3 _position, Vector3 _direction)
        : base(_panel3D)
    {
        direction = _direction;
        radius = _radius;
        height = _height;
        position = _position;
        mesh = Mesh.Cylinder(device,0.0f,radius,height,10,10);
        CreateMaterial();
        CalculateAngle();
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
        angle = (float)Math.Acos(productSca(dir,direction));
    }

    Vector3 productVect(Vector3 v1, Vector3 v2)
    {
        float X = v1.Y*v2.Z-v1.Z*v2.Y;
        float Y = v1.Z*v2.X-v1.X*v2.Z;
        float Z = v1.X*v2.Y-v1.Y*v2.X;
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

        device.Transform.World = Matrix.Multiply(Matrix.RotationAxis(axisRotation,angle),Matrix.Translation(position));

        mesh.DrawSubset(0);

        panel3D.activeLight(false);
    }
}
