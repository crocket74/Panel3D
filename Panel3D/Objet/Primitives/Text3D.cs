using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using System.ComponentModel;
using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;

class Text3D : Object3D
{
    public String text;//Texte à ecrire
    public Vector3 position; //Position dans l'espaces
    CustomVertex.PositionTextured[] quad;
    VertexBuffer vb;
    Texture boxTexture;

    public Text3D(Panel3D _panel3D, String _text, Vector3 _position) : base(_panel3D)
    {
        text = _text;
        position = _position;
        VertexDeclaration();
    }

    public void VertexDeclaration()
    {
        System.Drawing.Font drawFont;
        System.Drawing.SolidBrush drawBrush;
        Pen pen;
        drawFont = new System.Drawing.Font("Arial", 10);
        drawBrush = new SolidBrush(Color.Red);
        pen = new Pen(Color.Black);
        
        Bitmap b = new Bitmap(1,1);
        Graphics g = Graphics.FromImage(b);
        g.DrawString("ab", drawFont, drawBrush, new PointF(0, 0));

        boxTexture = new Texture(device, new Bitmap(20, 20, g), Usage.None, Pool.Managed);

        quad = new CustomVertex.PositionTextured[4];
        quad[0] = new CustomVertex.PositionTextured(-1.0f, 0.0f, 0.0f, 0.0f, 1.0f);
        quad[1] = new CustomVertex.PositionTextured(-1.0f, 2.0f, 0.0f, 0.0f, 0.0f);
        quad[2] = new CustomVertex.PositionTextured(1.0f, 0.0f, 0.0f, 1.0f, 1.0f);
        quad[3] = new CustomVertex.PositionTextured(1.0f, 2.0f, 0.0f, 1.0f, 0.0f);

        vb = new VertexBuffer(typeof(CustomVertex.PositionTextured), 4, device, Usage.WriteOnly, CustomVertex.PositionTextured.Format, Pool.Managed);
        GraphicsStream stm = vb.Lock(0, 0, 0);
        stm.Write(quad);
        vb.Unlock();
    }

    public override void draw()
    {
        device.SetTexture(0, boxTexture); //Here we tell direct3d that anything after this method should be rendered with this      texture
        device.SetStreamSource(0, vb, 0);
        device.VertexFormat = CustomVertex.PositionTextured.Format;
        device.RenderState.FillMode = FillMode.Solid;
        device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
    }

}
