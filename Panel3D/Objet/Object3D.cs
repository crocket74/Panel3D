using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using D3D = Microsoft.DirectX.Direct3D;

public class Object3D
{
    static int num = 0;

    public readonly int ID;
    protected List<Object3D> objectList;

    protected Microsoft.DirectX.Direct3D.Device device;
    public CustomVertex.PositionColored[] vertices;
    protected VertexBuffer VBuf = null;

    protected FillMode fillMode = FillMode.Solid;
    protected int numTriangle = 0;
    protected Panel3D panel3D;
    
    public Object3D(Panel3D _panel3D)
    {
        panel3D = _panel3D;
        device = panel3D.getDevice();
        objectList = new List<Object3D>();
        ID = num;
        num++;

        VBuf = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured),
            36,
            device,
            Usage.WriteOnly,
            CustomVertex.PositionNormalTextured.Format,
            Pool.Managed);

    }

    public virtual void createVertices()
    {
        vertices = new CustomVertex.PositionColored[0];
    }

    public void setFillMode(FillMode _fillMode)
    {
        fillMode = _fillMode;
    }

    public FillMode getFillMode()
    {
        return fillMode;
    }

    public virtual void draw()
    {
        device.Transform.World = Matrix.Identity;
        device.VertexFormat = CustomVertex.PositionColored.Format;
        device.RenderState.FillMode = fillMode;
        if(vertices != null)
            device.DrawUserPrimitives(PrimitiveType.TriangleList, numTriangle, vertices);

        foreach (Object3D obj in objectList)//On dessine tous les objet de la liste
        {
            obj.draw();
        }
    }
}
