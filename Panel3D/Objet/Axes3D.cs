using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using D3D = Microsoft.DirectX.Direct3D;

class Axes3D : Object3D
{
    float lX = 1f, lY = 1f, lZ = 1f;
    public float minX = 0, maxX =1, minY = 0, maxY =1, minZ = 0, maxZ = 1;
    public String nameX = "X", nameY = "Y", nameZ = "Z";
    D3D.Font police;
 
    public Axes3D(Panel3D _panel3D)  : base(_panel3D)
    {
        fillMode = FillMode.WireFrame;
        buildAxes();
        InitializeFont();
        addText();
        //objectList.Add(new Text3D(this.panel3D, new Vector3(-0.049f, 0.05f, 0.95f), new Vector3(0f, 0f, 1f), new Vector3(0f, -1f, 0f), 0.05f, "Z"));
        //objectList.Add(new Text3D(this.panel3D, new Vector3(0f, 0.2f, 0.8f), new Vector3(0f, 0f, 1f), new Vector3(0f, -1f, 0f), 0.1f, "hdddo"));
    }

    private void InitializeFont()
    {
        System.Drawing.Font systemfont = new System.Drawing.Font("Verdana", 9f, FontStyle.Regular);
        police = new D3D.Font(device, systemfont);
    }

    public void redraw()
    {
        buildAxes();
        addText();
    }

    void addText()
    {
        objectList = new List<Object3D>();
        for (float i = 0; i <= 1.1f; i += 0.25f)
        {
            objectList.Add(new Text3D(this.panel3D, new Vector3(-0.15f, 1f, i), (minZ+(maxZ-minZ)*i).ToString(),police));
            objectList.Add(new Text3D(this.panel3D, new Vector3(1.15f, i, 0), (minY + (maxY - minY) * i).ToString(), police));
            objectList.Add(new Text3D(this.panel3D, new Vector3(i, 1.15f, 0), (minX + (maxX - minX) * i).ToString(), police));
            //objectList.Add(new Text3Dxy(this.panel3D, new Vector3(1.05f, i, 0), new Vector3(1f, 0f, 0f), 0.1f, (minY + (maxY - minY) * i).ToString()));
            objectList.Add(new Cube(this.panel3D,0.01f,new Vector3(1f, i, 0)));
            //objectList.Add(new Text3Dxy(this.panel3D, new Vector3(i, 1.25f, 0), new Vector3(0f, -1f, 0f), 0.1f, (minX + (maxX - minX) * i).ToString()));
            objectList.Add(new Cube(this.panel3D, 0.01f, new Vector3(i, 1f, 0)));
            //objectList.Add(new Text3Dyz(this.panel3D, new Vector3(0f, 1.25f, i), new Vector3(0f, -1f, 0f), 0.1f, (minZ+(maxZ-minZ)*i).ToString()));
            objectList.Add(new Cube(this.panel3D, 0.01f, new Vector3(0f, 1f, i)));
        }

        objectList.Add(new Text3D(this.panel3D, new Vector3(-0.15f, 1f, 1.15f), nameZ, police));
        objectList.Add(new Text3D(this.panel3D, new Vector3(1.40f, 0.5f, 0), nameY, police));
        objectList.Add(new Text3D(this.panel3D, new Vector3(0.5f, 1.40f, 0), nameX, police));
    }


    void buildAxes()
    {
        numTriangle = 0;
        int length = 50;
        vertices = new CustomVertex.PositionColored[length];
        vertices[0].Position = new Vector3(0f, 0f, 0f);
        vertices[1].Position = new Vector3(0f, 0f, 0f);
        vertices[2].Position = new Vector3(lX, 0f, 0f);
        numTriangle++;

        vertices[3].Position = new Vector3(0f, 0f, 0f);
        vertices[4].Position = new Vector3(0f, 0f, 0f);
        vertices[5].Position = new Vector3(0f, lY, 0f);
        numTriangle++;

        vertices[6].Position = new Vector3(0f, 0f, 0f);
        vertices[7].Position = new Vector3(0f, 0f, 0f);
        vertices[8].Position = new Vector3(0f, 0f, lZ);
        numTriangle++;

        vertices[9].Position = new Vector3(lX, 0f, 0f);
        vertices[10].Position = new Vector3(lX, 0f, 0f);
        vertices[11].Position = new Vector3(lX, 0f, lZ);
        numTriangle++;

        vertices[12].Position = new Vector3(lX, 0f, 0f);
        vertices[13].Position = new Vector3(lX, 0f, 0f);
        vertices[14].Position = new Vector3(lX, lY, 0f);
        numTriangle++;

        vertices[15].Position = new Vector3(0f, lY, 0f);
        vertices[16].Position = new Vector3(0f, lY, 0f);
        vertices[17].Position = new Vector3(0f, lY, lZ);
        numTriangle++;

        vertices[18].Position = new Vector3(0f, lY, 0f);
        vertices[19].Position = new Vector3(0f, lY, 0f);
        vertices[20].Position = new Vector3(lX, lY, 0f);
        numTriangle++;

        vertices[18].Position = new Vector3(0f, lY, 0f);
        vertices[19].Position = new Vector3(0f, lY, 0f);
        vertices[20].Position = new Vector3(lX, lY, 0f);
        numTriangle++;

        vertices[21].Position = new Vector3(lX, lY, 0f);
        vertices[22].Position = new Vector3(lX, lY, 0f);
        vertices[23].Position = new Vector3(lX, lY, lZ);
        numTriangle++;

        vertices[24].Position = new Vector3(lX, 0f, lZ);
        vertices[25].Position = new Vector3(lX, 0f, lZ);
        vertices[26].Position = new Vector3(lX, lY, lZ);
        numTriangle++;

        vertices[27].Position = new Vector3(0f, lY, lZ);
        vertices[28].Position = new Vector3(0f, lY, lZ);
        vertices[29].Position = new Vector3(lX, lY, lZ);
        numTriangle++;

        vertices[30].Position = new Vector3(0f, lY, lZ);
        vertices[31].Position = new Vector3(0f, lY, lZ);
        vertices[32].Position = new Vector3(lX, lY, lZ);
        numTriangle++;

        vertices[33].Position = new Vector3(0f, 0f, lZ);
        vertices[34].Position = new Vector3(0f, 0f, lZ);
        vertices[35].Position = new Vector3(lX, 0f, lZ);
        numTriangle++;

        vertices[36].Position = new Vector3(0f, 0f, lZ);
        vertices[37].Position = new Vector3(0f, 0f, lZ);
        vertices[38].Position = new Vector3(0f, lY, lZ);
        numTriangle++;

        for(int i = 0; i < length; i++)
        {
            vertices[i].Color = Color.Black.ToArgb();
        }
    }

    public void setAxes(float maxX, float maxY, float maxZ)
    {
        lX = maxX;
        lY = maxY;
        lZ = maxZ;
        buildAxes();

    }

    public override void draw()
    {
        device.VertexFormat = CustomVertex.PositionColored.Format;
        device.RenderState.FillMode = FillMode.WireFrame;
        device.Transform.World = Matrix.Identity;

        if (vertices != null)
            device.DrawUserPrimitives(PrimitiveType.TriangleList, numTriangle, vertices);

        foreach (Object3D obj in objectList)//On dessine tous les objets de la liste
        {
            obj.draw();
        }
    }

}
