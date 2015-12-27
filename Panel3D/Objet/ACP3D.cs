using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using D3D = Microsoft.DirectX.Direct3D;

public class ACP3D : Object3D
{
    public bool solid;
    public int vect;

    public float maxColor = 10, minColor = 0;

    D3D.Font police;

    public ACP3D(Panel3D _panel3D)
        : base(_panel3D)
    {
        solid = true;

        vect = 0;
        numTriangle = 0;
        int length = 200;
        vertices = new CustomVertex.PositionColored[length];

        panel3D = _panel3D;
        device = panel3D.getDevice();
        objectList = new List<Object3D>();

        System.Drawing.Font systemfont = new System.Drawing.Font("Verdana", 9f, FontStyle.Regular);
        police = new D3D.Font(device, systemfont);

        VBuf = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured),
            36,
            device,
            Usage.WriteOnly,
            CustomVertex.PositionNormalTextured.Format,
            Pool.Managed);

        addObjects();
    }

    public void addPoints(Vector3 _vect, String label)
    {
        objectList.Add(new Sphere(panel3D, new Vector3(_vect.X / 2f + 0.5f, _vect.Y / 2f + 0.5f, _vect.Z / 2f + 0.5f), 0.015f));
        objectList.Add(new Text3D(panel3D, new Vector3(_vect.X / 2f + 0.5f, _vect.Y / 2f + 0.5f, _vect.Z / 2f + 0.5f + 0.1f), label, police));
    }

    public void addVector(Vector3 _vect, String label)
    {
         if (numTriangle < 50) {
            vertices[0+3*vect].Position = new Vector3(0.5f, 0.5f, 0.5f);
            vertices[1 + 3 * vect].Position = new Vector3(0.5f, 0.5f, 0.5f);
            vertices[2 + 3 * vect].Position = new Vector3(_vect.X / 2f + 0.5f, _vect.Y / 2f + 0.5f, _vect.Z / 2f + 0.5f);
            numTriangle++;
            vect++;

            objectList.Add(new Text3D(panel3D, new Vector3(_vect.X / 2f + 0.5f, _vect.Y / 2f + 0.5f, _vect.Z / 2f + 0.5f + 0.06f), label, police));

            for (int i = 0; i < 3*numTriangle; i++)
            {
                vertices[i].Color = Color.Red.ToArgb();
            }
        }
    }

    private void addObjects()
    {
        
    }

   
    public void AssociateColor()
    {
        for (int i = 0; i < numTriangle/2; i++)
        {
            for (int k = 0; k < 3; k++)
            {
                float h = vertices[3 * i + k].Position.Z;
                int R = 0, G = 0, B = 0;
                int alt = (int)((1530+255)*h);
                if(alt >= 1530)
                {
                    R = 255;
                    G = alt - 1530;
                    B = alt - 1530;
                }
                if(alt >= 1275 && alt < 1530)
                {
                    R = 255;
                    G = 1530 - alt;
                    B = 0;
                }
                if(alt >= 1020 && alt < 1275)
                {
                    R = alt - 1020;
                    G = 255;
                    B = 0;
                }
                if( alt >= 765 && alt < 1020)
                {
                    R = 0;
                    G = 255;
                    B = 1020 - alt;
                }
                if( alt >= 510 && alt < 765)
                {
                    R = 0;
                    G = alt - 510;
                    B = 255;
                }
                if( alt >= 255 && alt < 510)
                {
                    R = 510 - alt;
                    G = 0;
                    B = 255;
                }
                if (alt >= 0 && alt < 255)
                {
                    R = alt;
                    G = 0;
                    B = alt;
                }
                vertices[3 * i + k].Color = Color.FromArgb(0, R, G, B).ToArgb();
            }
        }
        for (int i = 0; i < numTriangle; i++)
        {
            for (int k = 0; k < 3; k++)
            {
                vertices[3 * i + k].Color = Color.Black.ToArgb();
            }
        }
        panel3D.Refresh();
    }

   

    public override void draw()
    {
        foreach (Object3D obj in objectList)//On dessine tous les objet de la liste
        {
            obj.draw();
        }

        device.VertexFormat = CustomVertex.PositionColored.Format;
        device.RenderState.FillMode = FillMode.WireFrame;
        device.Transform.World = Matrix.Identity;

        if (vertices != null)
            device.DrawUserPrimitives(PrimitiveType.TriangleList, numTriangle, vertices);
    }
}
