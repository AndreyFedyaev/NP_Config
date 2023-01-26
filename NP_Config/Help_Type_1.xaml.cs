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

namespace NP_Config
{
    /// <summary>
    /// Логика взаимодействия для Help_Type_1.xaml
    /// </summary>
    public partial class Help_Type_1 : Page
    {
        public Help_Type_1()
        {
            InitializeComponent();
        }

        public void TB_1_Write(string TB11, string TB12, string TB13, string TB14, string TB15, string TB16)
        {
            this.TB11.Text = TB11;
            this.TB12.Text = TB12;
            this.TB13.Text = TB13;
            this.TB14.Text = TB14;
            this.TB15.Text = TB15;
            this.TB16.Text = TB16;
        }
        public void TB_2_Write(string TB21, string TB22, string TB23, string TB24, string TB25, string TB26)
        {
            this.TB21.Text = TB21;
            this.TB22.Text = TB22;
            this.TB23.Text = TB23;
            this.TB24.Text = TB24;
            this.TB25.Text = TB25;
            this.TB26.Text = TB26;
        }
    }
}
