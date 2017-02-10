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

namespace TestWPF
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private MainPage m;
        public Window1()
        {
            InitializeComponent();
        }

        private void Launcher_MouseRightButtonDown(object sender, EventArgs e)
        {
            m = new MainPage(this);
            this.Hide();
            m.Show();
        }

        private void Quit_MouseRightButtonDown(object sender, EventArgs e)
        {
            if (m != null)
                m.Close();
            this.Close();
        }
    }
}
