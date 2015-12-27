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
    
    private Material mat;
    private Mesh mesh;

    public Cube(Panel3D _panel3D, float _R, Vector3 _position)
        : base(_panel3D)
    {
        R = _R;
        position = _position;
        mesh = Mesh.Box(device,R,R,R);
        CreateMaterial();
    }

    void CreateMaterial()
    {
        mat = new Material();
        mat.Ambient = Color.Black; //on affecte au material qui va recouviri la sphere la couleur de la boules concernée
        mat.Diffuse = Color.Black;// pareil pour la couleur diffuse

    }

    public override void draw()
    {
        panel3D.activeLight(true);

        device.RenderState.FillMode = FillMode.Solid;
 
        device.Material = mat;

        device.Transform.World = Matrix.Translation(position);

        mesh.DrawSubset(0);

        panel3D.activeLight(false);
    }
}
