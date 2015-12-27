using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

class Cube : Object3D
{
    //Permet d'afficher un cube dans l'espace

    public float R;//Dimension du carré
    public Vector3 position; //Position dans l'espaces
    
    public Cube(Panel3D _panel3D, float _R, Vector3 _position) : base(_panel3D)
    {
        R = _R;
        position = _position;
        VertexDeclaration();
    }

    public void VertexDeclaration()
    {
        //Trace tous les triangles necessaires au carré
        numTriangle = 0;
        vertices = new CustomVertex.PositionColored[36];
        Vector3 X = new Vector3(R/2,0,0);
        Vector3 Y = new Vector3(0, R/2, 0);
        Vector3 Z = new Vector3(0, 0, R/2);

        vertices[0].Position = position - X + Y + Z;
        vertices[1].Position = position + X + Y + Z;
        vertices[2].Position = position - X - Y + Z;

        vertices[3].Position = position + X - Y + Z;
        vertices[4].Position = position + X + Y + Z;
        vertices[5].Position = position - X - Y + Z;

        vertices[6].Position = position - X + Y - Z;
        vertices[7].Position = position + X + Y - Z;
        vertices[8].Position = position - X - Y - Z;

        vertices[9].Position = position + X - Y - Z;
        vertices[10].Position = position + X + Y - Z;
        vertices[11].Position = position - X - Y - Z;

        vertices[12].Position = position - Z + Y + X;
        vertices[13].Position = position + Z + Y + X;
        vertices[14].Position = position - Z - Y + X;

        vertices[15].Position = position + Z - Y + X;
        vertices[16].Position = position + Z + Y + X;
        vertices[17].Position = position - Z - Y + X;

        vertices[18].Position = position - Z + Y - X;
        vertices[19].Position = position + Z + Y - X;
        vertices[20].Position = position - Z - Y - X;

        vertices[21].Position = position + Z - Y - X;
        vertices[22].Position = position + Z + Y - X;
        vertices[23].Position = position - Z - Y - X;

        vertices[24].Position = position - X + Z + Y;
        vertices[25].Position = position + X + Z + Y;
        vertices[26].Position = position - X - Z + Y;

        vertices[27].Position = position + X - Z + Y;
        vertices[28].Position = position + X + Z + Y;
        vertices[29].Position = position - X - Z + Y;

        vertices[30].Position = position - X + Z - Y;
        vertices[31].Position = position + X + Z - Y;
        vertices[32].Position = position - X - Z - Y;

        vertices[33].Position = position + X - Z - Y;
        vertices[34].Position = position + X + Z - Y;
        vertices[35].Position = position - X - Z - Y;

        for(int i = 0; i < 36; i++)//Colorie les cotés du carré en rouge
        {
            vertices[i].Color = Color.Red.ToArgb();
        }

        numTriangle = 12;
    }
}
