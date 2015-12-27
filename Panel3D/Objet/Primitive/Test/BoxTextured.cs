using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


class BoxTextured : Object3D
{
    private const int NUM_TRIANGLES = 12;

	// Variable privées.
	Texture boxTexture;
	VertexBuffer VBuf = null;
	Material mtrl;
	float scale = 0.5f;

    System.Drawing.Font drawFont;
    System.Drawing.SolidBrush drawBrush;
    System.Drawing.SolidBrush whiteBrush;
    System.Drawing.Pen pen;

	// Constructeur.
	public BoxTextured(Panel3D _Panel3D) : base(_Panel3D)
	{
        drawFont = new System.Drawing.Font("Arial", 30);

        drawBrush = new System.Drawing.SolidBrush(Color.Black);
        whiteBrush = new System.Drawing.SolidBrush(Color.White);
        pen = new System.Drawing.Pen(Color.Black);

		createBox();
		createLight();
		createMaterial();
        device = panel3D.getDevice();
	}

	#region Méthode pour créer la boîte
	private void createBox()
	{
        //Bitmap bmp = new Bitmap(Application.StartupPath + "\\box.bmp");
        Bitmap bmp = new Bitmap(100, 100);
        Graphics g = Graphics.FromImage(bmp);

        Point pt = new Point(0, 0);
        Rectangle rect = new Rectangle(pt, new Size(100, 100));
        g.FillRectangle(whiteBrush, rect);
        g.DrawString("12", drawFont, drawBrush, pt);

        System.IO.MemoryStream stream = new System.IO.MemoryStream();
        bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
        stream.Seek(0, 0); // give the texture loader a "clear run" at the stream

        boxTexture = TextureLoader.FromStream(device, stream);


		VBuf = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured),
			36,
			device,
			Usage.WriteOnly,
			CustomVertex.PositionNormalTextured.Format,
			Pool.Managed);

		CustomVertex.PositionNormalTextured[] verts =
			(CustomVertex.PositionNormalTextured[])VBuf.Lock(0, 0);


		/*
		 * Création des vertices en spécifiant leurs coordonnées x, y, et z et
		 * deux autres coordonnées, Tu et Tv. Ils sont comme le x et le y des
		 * textures, respectivement. Ils spécifient les coordonnées des
		 * coins des textures.
		 * 
		 * Note :
		 * Certains vertices sont assignés par d'autres vertices car ils sont exactement les mêmes
		 * coordonnées (p.e. les coins des faces adjacentes).
		 */

		// 1ere face de la boîte (face avant) -------------------------------
		// 1er triangle
		verts[0] = createVertex(0, 1, 1, 0, 0);
		verts[1] = createVertex(1, 1, 1, 1, 0);
		verts[2] = createVertex(0, 0, 1, 0, 1);

		// 2eme triangle
		verts[3] = verts[2];
		verts[4] = verts[1];
		verts[5] = createVertex(1, 0, 1, 1, 1);
		VBuf.Unlock();
	}
	#endregion

	private void createMaterial()
	{
		// Crée le matériel qui supporte les textures.
		mtrl.Diffuse = Color.White;
		mtrl.Ambient = Color.White;
		device.Material = mtrl;
	}

	private void createLight()
	{
		// Pas besoin d'explications supplémentaires :
		// les noms des propriétés modifiés sont assez explicites.
		device.Lights[0].Type = LightType.Directional;
		device.Lights[0].Position = new Vector3(1.0f, 1.0f, -1.0f);
		device.Lights[0].Diffuse = Color.White;
		device.Lights[0].Direction = new Vector3(-0.5f, -0.5f, 0.5f);
		device.Lights[0].Range = 0.5f;

		device.Lights[0].Update(); // Fait connaître la lumière au device Direct3D
		device.Lights[0].Enabled = true; // Allume la lumière.

		device.RenderState.Ambient = Color.Black; // Les zones non-éclairés seront noires.
	}

	private CustomVertex.PositionNormalTextured createVertex(float x, float y, float z, float tu, float tv)
	{
		// La normale est le deuxième paramètre
		// Si vous vous rappelez pas de vos cours de physique,
		// la normale est une droite qui indique l'angle que prendra la lumière
		// lorsqu'elle se réfléchira sur la surface après l'avoir frappée.
		// Bref, pour une réflexion "réelle", la normale est la même que la position.
		return new CustomVertex.PositionNormalTextured(new Vector3(x, y, z),
			new Vector3(x, y, z), tu, tv);
	}

	public override void draw()
	{
		if (device != null)
		{
            device.RenderState.CullMode = Cull.None;
			device.SetStreamSource(0, VBuf, 0);
			// On redonne le format de vertex au device.
			device.VertexFormat = CustomVertex.PositionNormalTextured.Format;

			setTexture(boxTexture);
			// On dessine les triangles qui forment la boîte.
			// Le premier paramètre indique qu'on les dessine tous, à
			// partir du premier (index 0).
			//device.DrawPrimitives(PrimitiveType.TriangleList, 0, NUM_TRIANGLES);
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            setTexture(null);
		}
	}

	private void setViewPoint()
	{
		/* Ajuste la caméra :
		 *   - Le 1er paramètre est la position de la caméra
		 *   - Le 2eme paramètre est la position de la cible (le point regardé par la caméra)
		 *   - Le 3eme paramètre (je ne suis pas sûr) est le vecteur directeur du regard
		 *     ou l'angle avec lequel la caméra regarde sa cible.
		 */
		device.Transform.View = Matrix.LookAtLH(new Vector3(0.0F, 0.0F, -5.0F), new Vector3(0.0F, 0.0F, 0.0F), new Vector3(0.0F, 1.0F, 0.0F));
	}

	private void rotateBox()
	{
		double dblAngle = Environment.TickCount * (2 * Math.PI) / 8000;
		// Applique une matrice de rotation au monde qui tourne en X de dblAngle
		// radians, en Y aussi de dblAngle radians, et en X du même angle
		device.Transform.World = Matrix.RotationX((float)dblAngle) *
			Matrix.RotationY((float)dblAngle) *
			Matrix.RotationZ((float)dblAngle);
	}

	private void scaleBox()
	{
		// On applique une matrice de redimensionnement en multipliant la matrice
		// actuelle par celle de redimensionnement.
		// Si on faisait juste assigner la matrice, on écraserait celle de rotation.
		device.Transform.World = Matrix.Multiply(device.Transform.World, Matrix.Scaling(scale, scale, scale));
	}

	private void setTexture(Texture aTexture)
	{
		// Ajuste la texture. Ces paramètres ne sont pas explicables, ils sont
		// là parce qu'ils le sont.
		device.TextureState[0].ColorOperation = TextureOperation.Modulate;
		device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;
		device.TextureState[0].ColorArgument2 = TextureArgument.Diffuse;
		device.TextureState[0].AlphaOperation = TextureOperation.Disable;

		device.SetTexture(0, aTexture);
	}
}
