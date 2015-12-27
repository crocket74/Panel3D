using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using D3D = Microsoft.DirectX.Direct3D;


class Text3D : Object3D
{
    private const int NUM_TRIANGLES = 12;

    String text;
    Vector3 position;


    D3D.Font police;

    // Constructeur.

    public Text3D(Panel3D _Panel3D, Vector3 _position, String _text, D3D.Font _police)
        : base(_Panel3D)
    {
        police = _police;


        position = _position;
        text = _text;
        device = panel3D.getDevice();     
    }


    public override void draw()
    {
        if (device != null)
        {
            device.Transform.World = Matrix.Identity;

            Vector3 ab = position;
            ab.Project(device.Viewport, device.Transform.Projection, device.Transform.View, device.Transform.World);
            int x = (int)ab.X;
            int y = (int)ab.Y;
            police.DrawText(null, text, new Point(x, y), Color.Black);

        }
    }
}
