using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


class Curve3D : Object3D
{
    public float maxX, maxY, maxZ;
    public float minX, minY, minZ;

    float scaleX, scaleY, scaleZ;
    float[, ,] land;

    public bool solid;

    public float maxColor = 10, minColor = 0;

    public Curve3D(Panel3D _panel3D)  : base(_panel3D)
    {
        VertexDeclaration();
        solid = true;
    }

    public void VertexDeclaration()
    {
        if (land == null) return;
        if (land.GetLongLength(2) != 3) return;

        int nW = (int)land.GetLongLength(0);
        int nH = (int)land.GetLongLength(1);

        numTriangle = 0;
        vertices = new CustomVertex.PositionColored[2*6 * (nW - 1) * (nH - 1)];
        for (int jj = 0; jj < 2; jj++)
        {
            for (int i = 0; i < nW - 1; i++)
            {
                for (int j = 0; j < nH - 1; j++)
                {
                    int k = numTriangle;
                    vertices[3 * k].Position = new Vector3((land[i, j, 0] - minX) * scaleX, (land[i, j, 1] - minY) * scaleY, (land[i, j, 2] - minZ) * scaleZ);
                    vertices[3 * k + 1].Position = new Vector3((land[i + 1, j, 0] - minX) * scaleX, (land[i + 1, j, 1] - minY) * scaleY, (land[i + 1, j, 2] - minZ) * scaleZ);
                    vertices[3 * k + 2].Position = new Vector3((land[i + 1, j + 1, 0] - minX) * scaleX, (land[i + 1, j + 1, 1] - minY) * scaleY, (land[i + 1, j + 1, 2] - minZ) * scaleZ);
                    numTriangle++;

                    k = numTriangle;
                    vertices[3 * k].Position = new Vector3((land[i, j + 1, 0] - minX) * scaleX, (land[i, j + 1, 1] - minY) * scaleY, (land[i, j + 1, 2] - minZ) * scaleZ);
                    vertices[3 * k + 1].Position = new Vector3((land[i + 1, j + 1, 0] - minX) * scaleX, (land[i + 1, j + 1, 1] - minY) * scaleY, (land[i + 1, j + 1, 2] - minZ) * scaleZ);
                    vertices[3 * k + 2].Position = new Vector3((land[i, j, 0] - minX) * scaleX, (land[i, j, 1] - minY) * scaleY, (land[i, j, 2] - minZ) * scaleZ);
                    numTriangle++;
                }
            }
        }
        AssociateColor();
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
        for (int i = numTriangle / 2; i < numTriangle; i++)
        {
            for (int k = 0; k < 3; k++)
            {
                vertices[3 * i + k].Color = Color.Black.ToArgb();
            }
        }
        panel3D.Refresh();
    }

    public void setLand(float[,,] _land)
    {
        if (_land.GetLongLength(2) != 3) return;
        land = _land;
        maxX = 0; maxY = 0; maxZ = 0;

        int nW = (int)land.GetLongLength(0);
        int nH = (int)land.GetLongLength(1);

        maxX = land[1, 1, 0];
        maxY = land[1, 1, 1];
        maxZ = land[1, 1, 2];
        minX = land[1, 1, 0];
        minY = land[1, 1, 1];
        minZ = land[1, 1, 2];

        for (int i = 0; i < nW; i++)
        {
            for (int j = 0; j < nH; j++)
            {
                if (land[i, j, 0] > maxX) maxX = land[i, j, 0];
                if (land[i, j, 1] > maxY) maxY = land[i, j, 1];
                if (land[i, j, 2] > maxZ) maxZ = land[i, j, 2];
                if (land[i, j, 0] < minX) minX = land[i, j, 0];
                if (land[i, j, 1] < minY) minY = land[i, j, 1];
                if (land[i, j, 2] < minZ) minZ = land[i, j, 2];
            }
        }
        scaleX = 1 / (maxX - minX);
        scaleY = 1 / (maxY - minY);
        scaleZ = 1 / (maxZ - minZ);

        VertexDeclaration();
    }

    public override void draw()
    {
        device.VertexFormat = CustomVertex.PositionColored.Format;
        if (solid) device.RenderState.FillMode = FillMode.Solid;
        else device.RenderState.FillMode = FillMode.WireFrame;
        device.Transform.World = Matrix.Identity;

        if (vertices != null)
        {
            device.DrawUserPrimitives(PrimitiveType.TriangleList, numTriangle / 2, vertices);
            device.RenderState.FillMode = FillMode.WireFrame;
            device.DrawUserPrimitives(PrimitiveType.TriangleList, numTriangle, vertices);

        }

        foreach (Object3D obj in objectList)//On dessine tous les objet de la liste
        {
            obj.draw();
        }
    }
}
