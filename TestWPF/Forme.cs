using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace TestWPF
{
    public class Forme
    {
        public string type;
        public int x;
        public int y;
        public int rayon;
        public int epaisseur;
        public Brush color;

        public Forme()
        {
        }

        public Forme(int x, int y, int rayon, int epaisseur)
        {
            this.x = x;
            this.y = y;
            this.rayon = rayon;
            this.epaisseur = epaisseur;
        }

        public Ellipse GetEllipse()
        {
            Ellipse el = new Ellipse();
            el.Width = this.rayon * 2;
            el.Height = this.rayon * 2;

            el.SetValue(Canvas.LeftProperty, (Double)(this.x - this.rayon));
            el.SetValue(Canvas.TopProperty, (Double)(this.y - this.rayon));

            el.Fill = Brushes.Transparent;
            el.Stroke = color;
            el.StrokeThickness = this.epaisseur;

            return el;
        }

        public Rectangle GetRectangle()
        {
            Rectangle rect = new Rectangle();            

            rect.Width = this.rayon * 2;
            rect.Height = this.rayon * 2;

            rect.SetValue(Canvas.LeftProperty, (Double)(this.x - this.rayon));
            rect.SetValue(Canvas.TopProperty, (Double)(this.y - this.rayon));

            rect.Fill = color;
            rect.Stroke = Brushes.Black;
            rect.StrokeThickness = this.epaisseur;

            return rect;
        }
    }
}
