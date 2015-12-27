using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Collections.Generic;

using D3D = Microsoft.DirectX.Direct3D;

public class Panel3D : System.Windows.Forms.Panel
{
    public Device device; //Objet de directx permettant d'afficher les objets
    List<Object3D> objectList; //Liste contenant l'ensemble des objets à dessiner
    static float PI = 3.14159f;

    public float phi = PI/4, theta = PI/4; //Angle de positionnement de la camera
    public float R = 3.6f; //Distance de la camera au point qu'elle regarde
    public Vector3 cameraLook; //Point que regarde la camera

    int mouseX = 0, mouseY = 0; // Position de la souris

    Curve3D curve; //Objet3D contenant la courbe à afficher
    Axes3D axes; //Objet 3D contenant les axes
    ACP3D acp;
    D3D.Font police;

     public Panel3D()
     {
         InitializeComponent(); //Initialise le controle de la souris
         this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true); //On dessine tout dans la fonction refresh (enleve le scintillement)
         InitializeDevice(); //Crée l'objet directx permettant de dessiner les courbes

         cameraLook = new Vector3(0, 1/2, 1/2); //Initialise cameraLook
         CameraPositioning(); //On positionne une première fois la camera

         objectList = new List<Object3D>(); //Initialise la liste d'objet

         curve = new Curve3D(this);
         axes = new Axes3D(this);
         objectList.Add(axes); //On ajoute un système d'axe à la liste d'objets
         objectList.Add(new Cube(this, 0.02f, new Vector3(0,0,0))); //On ajoute un cube à l'origine
         //objectList.Add(new BoxTextured(this));
         objectList.Add(curve); //On ajoute une courbe à la liste d'objets

         System.Drawing.Font systemfont = new System.Drawing.Font("Comic Sans MS", 9f, FontStyle.Regular);
         police = new D3D.Font(device, systemfont);
     }

     public void setLand(float[, ,] _land) //Permet de modifier la courbe tracée
     {
         curve.setLand(_land); //On modifie la courbe
         axes.minX = curve.minX;
         axes.minY = curve.minY;
         axes.minZ = curve.minZ;
         axes.maxX = curve.maxX;
         axes.maxY = curve.maxY;
         axes.maxZ = curve.maxZ;
         axes.redraw();
     }

     public void setName(String nameX, String nameY, String nameZ)
     {
         axes.nameX = nameX;
         axes.nameY = nameY;
         axes.nameZ = nameZ;
         axes.redraw();
     }

     public void setVector(float[, ,] _vector, int lx, int ly) //Permet de modifier la courbe tracée
     {
         float[, ,] land = new float[lx, ly, 3];
         int k = 0;
         //if(lx*ly != _vector.Length) MessageBox("Le tableau ne contient pas le nombre d'éléments indiqué");

         for (int i = 0; i < lx; i++)
         {
             for (int j = 0; j < ly; j++)
             {
                 //land[i, j, 0] = _vector[k, 0];
                 //land[i, j, 1] = _vector[k, 0];
                 //land[i, j, 2] = _vector[k, 0];
                 k++;
             }
         }

         curve.setLand(land); //On modifie la courbe
     }


     public Device getDevice() { return device; }

     public void InitializeDevice()
     {
         //Initialisation de directx
         PresentParameters presentParams = new PresentParameters();
         presentParams.Windowed = true; //Permet de l'afficher dans une fenetre
         presentParams.SwapEffect = SwapEffect.Discard;
         device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, presentParams);

         //Active la profondeur
         presentParams.AutoDepthStencilFormat = DepthFormat.D16; // on defini le format du stencil
         presentParams.EnableAutoDepthStencil = true;

         device.RenderState.Lighting = false; // On active le système de lumière pour le rendu 3D
         device.RenderState.CullMode = Cull.None;
         device.RenderState.ZBufferEnable = true; // On active la profondeur (généralement nécessaire)

         InitializeLight();
     }

     public void InitializeLight()
    {
        device.Lights[0].Type = LightType.Directional;// on cré la première lumière de type directionnel
        device.Lights[0].Diffuse = System.Drawing.Color.White;// on definit la couleur de la lumière
        device.Lights[0].Direction = new Vector3(1, 0, 0);// on definit sa direction qui sera selon l'axe des X

        device.Lights[1].Type = LightType.Directional;// on cré la première lumière de type directionnel
        device.Lights[1].Diffuse = System.Drawing.Color.White;// on definit la couleur de la lumière
        device.Lights[1].Direction = new Vector3(0, 1, 0);// on definit sa direction qui sera selon l'axe des X

        device.Lights[2].Type = LightType.Directional;// on cré la première lumière de type directionnel
        device.Lights[2].Diffuse = System.Drawing.Color.White;// on definit la couleur de la lumière
        device.Lights[2].Direction = new Vector3(0, 0, 1);// on definit sa direction qui sera selon l'axe des X

        device.Lights[3].Type = LightType.Directional;// on cré la première lumière de type directionnel
        device.Lights[3].Diffuse = System.Drawing.Color.White;// on definit la couleur de la lumière
        device.Lights[3].Direction = new Vector3(-1, 0, 0);// on definit sa direction qui sera selon l'axe des X

        device.Lights[4].Type = LightType.Directional;// on cré la première lumière de type directionnel
        device.Lights[4].Diffuse = System.Drawing.Color.White;// on definit la couleur de la lumière
        device.Lights[4].Direction = new Vector3(0, -1, 0);// on definit sa direction qui sera selon l'axe des X

        device.Lights[5].Type = LightType.Directional;// on cré la première lumière de type directionnel
        device.Lights[5].Diffuse = System.Drawing.Color.White;// on definit la couleur de la lumière
        device.Lights[5].Direction = new Vector3(0, 0, -1);// on definit sa direction qui sera selon l'axe des X

    }

     public void activeLight(bool active)
     {
         device.RenderState.Lighting = active;
         device.Lights[0].Enabled = active;// on active la lumière 0
         device.Lights[1].Enabled = active;// on active la lumière 0
         device.Lights[2].Enabled = active;// on active la lumière 0
         device.Lights[3].Enabled = active;// on active la lumière 0
         device.Lights[4].Enabled = active;// on active la lumière 0
         device.Lights[5].Enabled = active;// on active la lumière 0

     }

     public void CameraPositioning()
     {
         device.RenderState.Lighting = false; // On active le système de lumière pour le rendu 3D
         device.RenderState.CullMode = Cull.None;
         device.RenderState.ZBufferEnable = true; // On active la profondeur (généralement nécessaire)

         //Définition du type de projection
         device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, this.Width / this.Height, 0.01f, 2 * R);
         //Positionnement de la caméra dans l'espaces (cordonnées sphérique) + translation de caméraLook
         Vector3 cameraPosition = new Vector3(
             (float)(R * Math.Sin(phi) * Math.Cos(theta)),
             (float)(R * Math.Sin(phi) * Math.Sin(theta)),
             (float)(R * Math.Cos(phi))) + cameraLook;
         device.Transform.View = Matrix.LookAtLH(cameraPosition, cameraLook, new Vector3(0, 0, 1));
     }

     protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) //Dessine les objets lors de l'appel de Refresh
     {
         //On efface ce qui était déjà dessiné
         device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, Color.White, 1.0f, 0);
         device.BeginScene();//On lui explique qu'à partir de maintenant il faut dessiner
 
         device.VertexFormat = CustomVertex.PositionColored.Format;
         foreach (Object3D obj in objectList)//On dessine tous les objets de la liste
         {
             obj.draw();
         }
         
         device.EndScene();//On arrete de dessiner

  
         device.Present();//On affiche le dessin
 
     }

     private void InitializeComponent()//Associe le mouvement de la souris à ce panel
     {
         this.SuspendLayout();
         this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Panel3D_MouseMove);
         this.ResumeLayout(false);

     }

     private void Panel3D_MouseMove(object sender, MouseEventArgs e)//Lors du mouvement de la souris
     {
         bool mouseClicked = false; //Vérifie que l'on appuie sur un boutton
         if (e.Button == MouseButtons.Left)
         {
             if (mouseX == 0)//Le boutton n'était pas appuyé précédement
             {
                 mouseX = e.X;
                 mouseY = e.Y;
             }
             else //Le boutton était déjà appuyé, il faut detecter le déplacement pour modifier le graphique en conséquences
             {
                 float depX = (float)(e.X - mouseX);
                 float depY = -(float)(e.Y - mouseY);
                 //Modifie la position de la camera
                 theta += depX / 100f;
                 phi += depY / 100f;
                 if (phi < 0) phi = 0.001f;
                 mouseX = e.X;
                 mouseY = e.Y;

                 CameraPositioning();
                 this.Refresh();
             }
             mouseClicked = true;
         }
         if (e.Button == MouseButtons.Middle)
         {
             if (mouseX == 0)
             {
                 mouseX = e.X;
                 mouseY = e.Y;
             }
             else
             {
                 R = R * (1+(e.Y - mouseY) / 100f);
                 mouseX = e.X;
                 mouseY = e.Y;

                 CameraPositioning();
                 this.Refresh();
             }
             mouseClicked = true;
         }
         if (e.Button == MouseButtons.Right)
         {
             if (mouseX == 0)
             {
                 mouseX = e.X;
                 mouseY = e.Y;
             }
             else
             {
                 Vector3 eventPos = new Vector3((float)e.X, (float)e.Y, 0f);
                 Vector3 mousePos = new Vector3((float)mouseX, (float)mouseY, 0f);
                 eventPos.Unproject(device.Viewport, device.Transform.Projection, device.Transform.View, Matrix.Identity);
                 mousePos.Unproject(device.Viewport, device.Transform.Projection, device.Transform.View, Matrix.Identity);

                 cameraLook += 100*R*(mousePos - eventPos);
                 mouseX = e.X;
                 mouseY = e.Y;

                 CameraPositioning();
                 this.Refresh();
             }
             mouseClicked = true;
         }
         if(!mouseClicked) mouseX = 0;//Aucun boutton n'était appuyé, il faudra le signaler au prochain coup
     }

     public void addACP3D()
     {
         acp = new ACP3D(this);
         objectList.Add(acp);
         axes.minX = -1;
         axes.minY = 1;
         axes.minZ = -1;
         axes.maxX = 1;
         axes.maxY = -1;
         axes.maxZ = 1;
         axes.redraw();
     }

     public void addACP3DPoints(Vector3 _AC1, String label)
     {
         acp.addPoints(_AC1, label);
     }

     public void addACP3DVector(Vector3 _vect, String label)
     {
         acp.addVector(_vect, label);
     }

     public void changeTexture(bool solid)
     {
         curve.solid = solid;
     }

     public void addMesure(Vector3 position, Vector3 direction, float erreur, float size)
     {
         objectList.Add(new Mesure(this,position,direction,erreur,size));
     }

     public void addMesure(float posX, float posY, float posZ, float dirX, float dirY, float dirZ, float erreur, float size)
     {
         objectList.Add(new Mesure(this, new Vector3(posX,posY,posZ), new Vector3(dirX, dirY , dirZ), erreur, size));
     }

     /*public void addACP3D(ACP3D _acp)
     {
         objectList.Add(_acp);
     }*/
}
