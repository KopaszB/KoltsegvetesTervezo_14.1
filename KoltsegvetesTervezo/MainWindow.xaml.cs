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
            //DataGridFeltoltes();
        }
        public void DataGridFeltoltes()
        {
            string url = "http://localhost:3000/kiadasok";
            koltsegek = Backend.GET(url).Send().As<List<Kiadas>>();
            dg_kiadasok.ItemsSource = koltsegek;
        }
    }
}
