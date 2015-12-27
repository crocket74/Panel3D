using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace TestPanel3D
{
    public partial class Form2 : Form
    {
        Panel3D panel1;
        float[, , ] land;

        public Form2()
        {
            InitializeComponent();
            InitializePanel1();

            //InitializeLand(); Si on veut tracer une courbe
            //panel1.setLand(land); //methode pour envoyer le terrain au panel
            //panel1.setName("dil", "dis", "Profondeur");

            /*Methode n°1 pour ajouter les mesures en utilisant des vecteurs (il faut ajouter 
             * using Microsoft.DirectX;
                using Microsoft.DirectX.Direct3D;)
             * /
            Vector3 direction = new Vector3(0f,0f,1f);
            Vector3 d2 = new Vector3(0f, 0f, 1f);
            Vector3 d3 = new Vector3(0f, 0.714f, 0.714f);
            Vector3 p1 = new Vector3(0f,0.2f,1f);
            Vector3 p2 = new Vector3(0f,0.5f,0.5f);
            Vector3 p3 = new Vector3(0.6f,0f,1f);
            panel1.addMesure(p1, direction, 0.05f, 0.1f);
            panel1.addMesure(p2, d2, 0.05f, 0.1f);
            panel1.addMesure(p3, d3, 0.05f, 0.1f);

            //Methode n°2 pour ajouter les mesures sans utiliser les vecteurs
            panel1.addMesure(0,0.1f,0.5f,0.714f,0.714f,0f, 0.02f, 0.05f);


            panel1.CameraPositioning(); //Il faut repositionner la camera car il faut surement l'éloigner ou la rapprocher
            panel1.Refresh();//On demande au controle de se redessiner */

        }

        public void InitializePanel1()
        {
            this.panel1 = new Panel3D();
            /*this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(20, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(460, 294);
            this.panel1.TabIndex = 0;
            this.Controls.Add(this.panel1);*/
        }

         void InitializeLand()//initialise une courbe à afficher
         {
             int nW = 10;
             int nH = 10;

             land = new float[nW, nH, 3];
             for (int i = 0; i < nW; i++)
             {
                 for (int j = 0; j < nH; j++)
                 {
                     land[i, j, 0] = -(float)(i);
                     land[i, j, 1] = -(float)(400*j);
                     land[i, j, 2] = -((float)((i-5)*(i-5) + (j-5)*(j-5)) / (nH * nW) * 20+50);
                 }
             }
         }

         private void checkBox1_CheckedChanged(object sender, EventArgs e)
         {
             panel1.changeTexture(checkBox1.Checked);
             panel1.Refresh();
         }
    }
}