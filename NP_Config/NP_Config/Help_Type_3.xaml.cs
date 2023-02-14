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
    /// Логика взаимодействия для Help_Type_3.xaml
    /// </summary>
    public partial class Help_Type_3 : Page
    {
        public Help_Type_3()
        {
            InitializeComponent();
        }
        private static readonly Regex IP_Setting_RangeNumbers = new Regex("[0-9]");    //набор допустимых символов для ввода в TextBox

        public void TB_5_Write(string TB51, string TB52, string TB53, string TB54, string TB55, string TB56)
        {
            this.TB51.Text = TB51;
            this.TB52.Text = TB52;
            this.TB53.Text = TB53;
            this.TB54.Text = TB54;
            this.TB55.Text = TB55;
            this.TB56.Text = TB56;
        }
        public void TB_6_Write(string TB61, string TB62, string TB63, string TB64, string TB65, string TB66)
        {
            this.TB61.Text = TB61;
            this.TB62.Text = TB62;
            this.TB63.Text = TB63;
            this.TB64.Text = TB64;
            this.TB65.Text = TB65;
            this.TB66.Text = TB66;
        }
        private void Cheking_for_numbers(object sender, TextCompositionEventArgs e)   //без ограничений по количеству символов (весь перечень цифр)
        {
            e.Handled = !IP_Setting_RangeNumbers.IsMatch(e.Text);
        }
    }
}
