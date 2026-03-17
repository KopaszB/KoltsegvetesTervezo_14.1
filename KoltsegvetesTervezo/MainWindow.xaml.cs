using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NetworkHelper;

namespace KoltsegvetesTervezo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Kiadas> koltsegek = new List<Kiadas>();
        public MainWindow()
        {
            InitializeComponent();
            osszbevetelKiir();  
            DataGridFeltoltes();
        }
        public void DataGridFeltoltes()
        {
            string url = "http://localhost:3000/kiadasok";
            koltsegek = Backend.GET(url).Send().As<List<Kiadas>>();
            dg_kiadasok.ItemsSource = koltsegek;
        }
        public void osszbevetelKiir() 
        {
            string url = "http://localhost:3000/osszbevetel";
            decimal osszbevetel = Backend.GET(url).Send().As<decimal>();
            tbl_bevetel.Text = $"Összbevétel: {osszbevetel} Ft";
        }

        private void btn_bevetelHozzaad_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://localhost:3000/bevetel";
            if (string.IsNullOrEmpty(tbx_bevetelForras.Text) || string.IsNullOrEmpty(tbx_bevetelOsszeg.Text))
            {
                MessageBox.Show("Kérem töltse ki az összes mezőt!");
                return;
            }

            var ujBevetel = new Bevetel()
            {
                forras = tbx_bevetelForras.Text.Trim(),
                osszeg = decimal.Parse(tbx_bevetelOsszeg.Text.Trim())
            };

            try
            {
                Backend.POST(url).Body(ujBevetel).Send();
                MessageBox.Show("Sikeresen hozzáadva a bevétel!");
                osszbevetelKiir();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt a bevétel hozzáadásakor!"+ex.Message);
            }
        }

        private void btn_bevetelTorol_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://localhost:3000/beveteltorol";
            try
            {
                Backend.DELETE(url).Send();
                MessageBox.Show("Sikeresen törölve az összes bevétel!");
                osszbevetelKiir();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt a bevételek törlésekor!"+ex.Message);
            }

        }
    }
}
