using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;

namespace TestWPF
{
    public partial class MainPage : Window
    {
        public Route route;
        private int routeX = 0;
        private int routeY = 0;
        private int routeRayon = 0;
        private int epaisseurRoute = 2;

        private Vehicule premierVehicule;
        private int vehiculeStartX = 0;
        private int vehiculeStartY = 0;
        private int vehiculeRayon = 5;
        private int epaisseurVehicule = 2;

        private List<Vehicule> allVehicule = new List<Vehicule>();
        private List<int> ordreVehicule = new List<int>();

        public volatile int i = 0;
        //public int angle = 16;        
        public Window1 w_;
        private volatile bool shouldStop = false;
        private Thread mainThread;        

        public MainPage()
        {
            this.Init();
        }

        public MainPage(Window1 w)
        {
            w_ = w;
            this.Init();
            this.Affiche();
        }

        public void Init()
        {
            InitializeComponent();
            this.routeX = (int)this.MainCanvas.Width / 2;
            this.routeY = (int)this.MainCanvas.Height / 2;
            this.routeRayon = (int)this.MainCanvas.Height / 2 - 30;

            this.vehiculeStartX = this.routeX;
            this.vehiculeStartY = this.routeY;
            this.route = new Route(this.routeX, this.routeY, this.routeRayon, this.epaisseurRoute);
            this.allVehicule = new List<Vehicule>();
            this.ordreVehicule = new List<int>();

            this.premierVehicule = new Vehicule(0, this.vehiculeStartX, this.vehiculeStartY, this.vehiculeRayon, this.epaisseurVehicule, 10);
            this.allVehicule.Add(premierVehicule);
            this.ordreVehicule.Add(0);
        }

        private void UpdateText()
        {
            this.tourTextBox.Text = string.Format("Nombre de tour: {0}.", this.i.ToString());
            this.vitesseTextBox.Text = string.Format("Vitesse du Véhicule: {0}.", this.premierVehicule.angleMvt);
            this.nbVehiculeTextBox.Text = string.Format("Nb de Véhicule: {0}/{1}.", this.allVehicule.Count, this.ordreVehicule.Count());
        }

        /// <summary>
        /// Lance les voitures sur la route
        /// </summary>
        public void LancementRoute()
        {
            this.shouldStop = false;
            while (!shouldStop)
            {
                this.LancementUnitaire();
            }
        }

        /// <summary>
        /// lancement unitaire: Mise à jour du nb de tour + update canva
        /// </summary>
        public void LancementUnitaire()
        {
            this.UpdateTextCB();
            this.UpdateCanvaCB();

            foreach (Vehicule v in this.allVehicule)
                v.InteractOrdre(this.allVehicule, this.ordreVehicule, this.routeRayon);

            /*
            bool spawnVehicule = (new Random()).Next(3) == 0; // 1 chance sur 3 de spawn un véhicule
            if (spawnVehicule && this.allVehicule.Count() < nbMaxVehicule)
                this.AddNewVehicule();
            */

            System.Threading.Thread.Sleep(200);

            this.i++;
        }

        private void AddNewVehicule()
        {
            int vitesse = 0;
            Random r = new Random();
            int id = this.allVehicule.Count() + 1; // id unique
            int rayon = r.Next(10) + 3;
            int epaisseur = r.Next(3) + 1;

            vitesse = r.Next(30) + 5;

            Vehicule v = new Vehicule(id, this.vehiculeStartX, this.vehiculeStartY, rayon, this.epaisseurVehicule, vitesse);
            this.allVehicule.Add(v);
            this.ordreVehicule.Add(id);
        }

        /// <summary>
        /// Met à jour le nb de tour via delegate
        /// </summary>
        private void UpdateTextCB()
        {
            Delegate myDelegate = (Action)UpdateText;
            this.tourTextBox.Dispatcher.Invoke(myDelegate);
        }

        /// <summary>
        /// Met à jour le canva via delegate
        /// </summary>
        private void UpdateCanvaCB()
        {
            Delegate myDelegate2 = (Action)Affiche;
            this.MainCanvas.Dispatcher.Invoke(myDelegate2);
        }

        /// <summary>
        /// ferme les thread et repasser sur l'écran d'accueil
        /// </summary>
        private void Button_exit(object sender, RoutedEventArgs e)
        {
            if (this.mainThread != null)
                this.mainThread.Abort();

            this.w_.Show();
            this.Close();
        }

        private void Button_clear(object sender, RoutedEventArgs e)
        {
            this.Init();
            this.MainCanvas.Children.Clear();
        }

        private void Button_next(object sender, RoutedEventArgs e)
        {
            this.LancementUnitaire();
        }

        public void Affiche()
        {
            MainCanvas.Children.Clear();
            this.Draw(route);

            foreach (Vehicule v in allVehicule)
            {
                if (v.x == this.vehiculeStartX && v.y == this.vehiculeStartY)
                    continue;

                this.Draw(v);
            }
        }

        private void Draw(Forme o)
        {
            if (o.type == "ROUTE")
            {
                MainCanvas.Children.Add(o.GetEllipse());
            }
            else
            {
                Forme f1 = new Forme(o.x - 1, o.y - 1, 1, 1);
                Forme f2 = new Forme(o.x + 1, o.y - 1, 1, 1);
                Forme f3 = new Forme(o.x - 1, o.y + 1, 1, 1);
                Forme f4 = new Forme(o.x + 1, o.y + 1, 1, 1);
                MainCanvas.Children.Add(f1.GetRectangle());
                MainCanvas.Children.Add(f2.GetRectangle());
                MainCanvas.Children.Add(f3.GetRectangle());
                MainCanvas.Children.Add(f4.GetRectangle());

                MainCanvas.Children.Add(o.GetRectangle());
            }
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            mainThread = new Thread(LancementRoute);
            mainThread.Start();
            this.buttonLaunch.IsEnabled = false;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            this.buttonLaunch.IsEnabled = true;
            this.shouldStop = true;
        }

        private void buttonAddVoiture_Click(object sender, RoutedEventArgs e)
        {
            this.AddNewVehicule();
        }

        private void buttonSpeedUp_Click(object sender, RoutedEventArgs e)
        {
            this.allVehicule[0].Accelere();
        }

        private void buttonSpeedDown_Click(object sender, RoutedEventArgs e)
        {
            this.allVehicule[0].Freine();
        }
    }
}
