using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Help_Type_2.xaml
    /// </summary>
    public partial class Help_Type_2 : Page
    {
        public Help_Type_2()
        {
            InitializeComponent();
        }
        private static readonly Regex IP_Setting_RangeNumbers = new Regex("[0-9]");    //набор допустимых символов для ввода в TextBox

        public void TB_3_Write(string TB31, string TB32, string TB33, string TB34, string TB35, string TB36)
        {
            this.TB31.Text = TB31;
            this.TB32.Text = TB32;
            this.TB33.Text = TB33;
            this.TB34.Text = TB34;
            this.TB35.Text = TB35;
            this.TB36.Text = TB36;
        }
        public void TB_4_Write(string TB41, string TB42, string TB43, string TB44, string TB45, string TB46)
        {
            this.TB41.Text = TB41;
            this.TB42.Text = TB42;
            this.TB43.Text = TB43;
            this.TB44.Text = TB44;
            this.TB45.Text = TB45;
            this.TB46.Text = TB46;
        }
        private void Cheking_for_numbers(object sender, TextCompositionEventArgs e)   //без ограничений по количеству символов (весь перечень цифр)
        {
            e.Handled = !IP_Setting_RangeNumbers.IsMatch(e.Text);
        }
    }
}
