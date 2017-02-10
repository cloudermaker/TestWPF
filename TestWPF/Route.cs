using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace TestWPF
{
    public class Route : Forme
    {
        public Route(int x, int y, int rayon, int epaisseur)
        {
            this.type = "ROUTE";
            this.x = x;
            this.y = y;
            this.rayon = rayon;
            this.epaisseur = epaisseur;
            this.color = Brushes.Maroon;
        }
    }
}
