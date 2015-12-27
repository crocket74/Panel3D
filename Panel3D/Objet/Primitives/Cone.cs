using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

class Cone : Object3D
{
    public float length;
    public float angle;
    public Vector3 position;
    public Vector3 direction;

    public Cone(Panel3D _panel3D, float _length, float _angle, Vector3 _position, Vector3 _direction)
        : base(_panel3D)
    {
        direction = _direction;
        length = _length;
        angle = _angle;
        position = _position;
        VertexDeclaration();
    }

    public override void draw()
    {
        device.VertexFormat = CustomVertex.PositionColored.Format;
        device.RenderState.FillMode = fillMode;
        if (vertices != null)
            device.DrawUserPrimitives(PrimitiveType.TriangleFan, numTriangle, vertices);

    }
    public void VertexDeclaration()
    {
        numTriangle = 0;
        vertices = new CustomVertex.PositionColored[10];

        vertices[0].Position = position;

        for (int i = 1; i < 10; i++)
        {
            vertices[i].Position = position + direction;
        }

        for(int i = 0; i < 10; i++)
        {
            vertices[i].Color = Color.Red.ToArgb();
        }

        numTriangle = 12;
    }

}
