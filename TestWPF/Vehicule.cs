using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace TestWPF
{
    public class Vehicule : Forme
    {
        public int id;
        public int xInit;
        public int yInit;
        public int angleCumu;
        public int angleMvt;
        public bool firstTurn = true;

        public Vehicule(int id, int x, int y, int rayon, int epaisseur, int angle)
        {
            this.type = "VEHICULE";
            this.id = id;
            this.x = x;
            this.y = y;
            this.xInit = x;
            this.yInit = y;
            this.angleCumu = angle;
            this.angleMvt = angle;
            this.rayon = rayon;
            this.epaisseur = epaisseur;

            if (id == 0)
                this.color = Brushes.Red;
            else
            {
                Random r = new Random();
                this.color = new SolidColorBrush(Color.FromRgb((byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255)));
            }
        }

        private void Avance(int routeRayon)
        {
            double angleRad = Math.PI * this.angleCumu / 180.0;

            double cos = routeRayon * Math.Cos(angleRad);
            double sin = routeRayon * Math.Sin(angleRad);

            this.x = this.xInit + (int)cos;
            this.y = this.yInit + (int)sin;
            this.angleCumu = (this.angleCumu + this.angleMvt);
        }

        public void InteractOrdre(List<Vehicule> allVehicule, List<int> ordreVehicule, int rayonRoute)
        {
            int margeSecurite = this.rayon + 5;
            int i = ordreVehicule.IndexOf(this.id);
            if (i > 0)
            {
                Vehicule vehicu_devant = allVehicule.Find(a => a.id == ordreVehicule[i - 1]);

                // Cas des rajouts de voitures en retard: on les remet au même niveau (id en dernière position)
                if (vehicu_devant.angleCumu > 360 && this.angleCumu < 360 && this.id == ordreVehicule.Count())
                    this.angleCumu = vehicu_devant.angleCumu - 10 - margeSecurite;

                int nextPos = (this.angleCumu + this.angleMvt);

                if (nextPos + margeSecurite > vehicu_devant.angleCumu)
                    this.Freine();
                else
                    this.Accelere();
            }

            this.Avance(rayonRoute);
        }

        private int CalcDist(int x1, int y1, int x2, int y2)
        {
            return (int)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }

        public void Freine(int dist)
        {
            if (this.angleMvt > 0)
                this.angleMvt /= (dist * this.angleMvt);
        }

        public void Freine()
        {
            if (this.angleMvt > 0)
                this.angleMvt /= 2;
        }

        public void Accelere()
        {
            this.angleMvt++;
        }
    }
}
