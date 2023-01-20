using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public partial class WorkPage : Page
    {
        public WorkPage()
        {
            Initialize_NP_ZR();         //Создаем динамические текстовые боксы для настройки датчиков NP
            Initialize_UCH_ZR_set();     //Создаем динамические текстовые боксы для настройки датчиков на участке
            InitializeComponent();
        }
        struct UCH_ZR_setting
        {
            public Grid G;
            public TextBox UCH_ZR_Set_Address;
            public TextBox UCH_ZR_Set_NP;
            public TextBox UCH_ZR_Set_Channel;
        }
        struct NP_ZR_Channel
        {
            public Grid G;
            public TextBox NP_ZR_Address;
        }

        UCH_ZR_setting[] UCH_ZR_set_left = new UCH_ZR_setting[7];
        UCH_ZR_setting[] UCH_ZR_set_right = new UCH_ZR_setting[7];
        NP_ZR_Channel[] NP_ZR_channel1 = new NP_ZR_Channel[10];
        NP_ZR_Channel[] NP_ZR_channel2 = new NP_ZR_Channel[10];

        private void Initialize_NP_ZR()    //Создаем динамические текстовые боксы для настройки датчиков NP
        {
            // 1 канал
            for (int index = 0; index < NP_ZR_channel1.Length; index++)
            {
                NP_ZR_channel1[index].G = new Grid();

                NP_ZR_channel1[index].NP_ZR_Address = new TextBox();
                NP_ZR_channel1[index].NP_ZR_Address.Text = "";
                NP_ZR_channel1[index].NP_ZR_Address.HorizontalAlignment = HorizontalAlignment.Center;
                NP_ZR_channel1[index].NP_ZR_Address.VerticalAlignment = VerticalAlignment.Center;
                NP_ZR_channel1[index].NP_ZR_Address.Name = "NP_ZR_Channel1_Address_Index_" + index.ToString();
                NP_ZR_channel1[index].NP_ZR_Address.Margin = new Thickness(0, 0, 0, 5);
                NP_ZR_channel1[index].NP_ZR_Address.Width = 40;
                NP_ZR_channel1[index].NP_ZR_Address.Height = 18;
                NP_ZR_channel1[index].NP_ZR_Address.Style = (Style)NP_ZR_channel1[index].NP_ZR_Address.FindResource("TextBoxStyle_Type1");
                NP_ZR_channel1[index].NP_ZR_Address.PreviewTextInput += new TextCompositionEventHandler(Cheking_for_numbers);

                NP_ZR_channel1[index].G.Children.Add(NP_ZR_channel1[index].NP_ZR_Address);
            }
            // 2 канал
            for (int index = 0; index < NP_ZR_channel2.Length; index++)
            {
                NP_ZR_channel2[index].G = new Grid();

                NP_ZR_channel2[index].NP_ZR_Address = new TextBox();
                NP_ZR_channel2[index].NP_ZR_Address.Text = "";
                NP_ZR_channel2[index].NP_ZR_Address.HorizontalAlignment = HorizontalAlignment.Center;
                NP_ZR_channel2[index].NP_ZR_Address.VerticalAlignment = VerticalAlignment.Center;
                NP_ZR_channel2[index].NP_ZR_Address.Name = "NP_ZR_Channel1_Address_Index_" + index.ToString();
                NP_ZR_channel2[index].NP_ZR_Address.Margin = new Thickness(0, 0, 0, 5);
                NP_ZR_channel2[index].NP_ZR_Address.Width = 40;
                NP_ZR_channel2[index].NP_ZR_Address.Height = 18;
                NP_ZR_channel2[index].NP_ZR_Address.Style = (Style)NP_ZR_channel2[index].NP_ZR_Address.FindResource("TextBoxStyle_Type1");
                NP_ZR_channel2[index].NP_ZR_Address.PreviewTextInput += new TextCompositionEventHandler(Cheking_for_numbers);

                NP_ZR_channel2[index].G.Children.Add(NP_ZR_channel2[index].NP_ZR_Address);
            }
        }
            private void Initialize_UCH_ZR_set()    //Создаем динамические текстовые боксы для настройки датчиков на участке
        {
            //для датчиков слева
            for (int index = 0; index < UCH_ZR_set_left.Length; index++)
            {
                UCH_ZR_set_left[index].G = new Grid();

                UCH_ZR_set_left[index].UCH_ZR_Set_Address = new TextBox();
                UCH_ZR_set_left[index].UCH_ZR_Set_Address.Text = "";
                UCH_ZR_set_left[index].UCH_ZR_Set_Address.HorizontalAlignment= HorizontalAlignment.Center;
                UCH_ZR_set_left[index].UCH_ZR_Set_Address.VerticalAlignment= VerticalAlignment.Center;
                UCH_ZR_set_left[index].UCH_ZR_Set_Address.Name = "UCH_ZR_Set_Address_Index_" + index.ToString();
                UCH_ZR_set_left[index].UCH_ZR_Set_Address.Margin= new Thickness(0,0,100,5);
                UCH_ZR_set_left[index].UCH_ZR_Set_Address.Width = 40;
                UCH_ZR_set_left[index].UCH_ZR_Set_Address.Height = 18;
                UCH_ZR_set_left[index].UCH_ZR_Set_Address.Style = (Style)UCH_ZR_set_left[index].UCH_ZR_Set_Address.FindResource("TextBoxStyle_Type1");
                UCH_ZR_set_left[index].UCH_ZR_Set_Address.PreviewTextInput += new TextCompositionEventHandler(Cheking_for_numbers);

                UCH_ZR_set_left[index].UCH_ZR_Set_NP = new TextBox();
                UCH_ZR_set_left[index].UCH_ZR_Set_NP.Text = "";
                UCH_ZR_set_left[index].UCH_ZR_Set_NP.HorizontalAlignment = HorizontalAlignment.Center;
                UCH_ZR_set_left[index].UCH_ZR_Set_NP.VerticalAlignment = VerticalAlignment.Center;
                UCH_ZR_set_left[index].UCH_ZR_Set_NP.Name = "UCH_ZR_Set_NP_Index_" + index.ToString();
                UCH_ZR_set_left[index].UCH_ZR_Set_NP.Margin = new Thickness(0, 0, 0, 5);
                UCH_ZR_set_left[index].UCH_ZR_Set_NP.Width = 40;
                UCH_ZR_set_left[index].UCH_ZR_Set_NP.Height = 18;
                UCH_ZR_set_left[index].UCH_ZR_Set_NP.Style = (Style)UCH_ZR_set_left[index].UCH_ZR_Set_NP.FindResource("TextBoxStyle_Type1");
                UCH_ZR_set_left[index].UCH_ZR_Set_NP.PreviewTextInput += new TextCompositionEventHandler(Cheking_for_numbers);

                UCH_ZR_set_left[index].UCH_ZR_Set_Channel = new TextBox();
                UCH_ZR_set_left[index].UCH_ZR_Set_Channel.Text = "";
                UCH_ZR_set_left[index].UCH_ZR_Set_Channel.HorizontalAlignment = HorizontalAlignment.Center;
                UCH_ZR_set_left[index].UCH_ZR_Set_Channel.VerticalAlignment = VerticalAlignment.Center;
                UCH_ZR_set_left[index].UCH_ZR_Set_Channel.Name = "UCH_ZR_Set_Channe_Index_" + index.ToString();
                UCH_ZR_set_left[index].UCH_ZR_Set_Channel.Margin = new Thickness(100, 0, 0, 5);
                UCH_ZR_set_left[index].UCH_ZR_Set_Channel.Width = 40;
                UCH_ZR_set_left[index].UCH_ZR_Set_Channel.Height = 18;
                UCH_ZR_set_left[index].UCH_ZR_Set_Channel.Style = (Style)UCH_ZR_set_left[index].UCH_ZR_Set_Channel.FindResource("TextBoxStyle_Type1");
                UCH_ZR_set_left[index].UCH_ZR_Set_Channel.PreviewTextInput += new TextCompositionEventHandler(Cheking_for_numbers);

                UCH_ZR_set_left[index].G.Children.Add(UCH_ZR_set_left[index].UCH_ZR_Set_Address);
                UCH_ZR_set_left[index].G.Children.Add(UCH_ZR_set_left[index].UCH_ZR_Set_NP);
                UCH_ZR_set_left[index].G.Children.Add(UCH_ZR_set_left[index].UCH_ZR_Set_Channel);
            }
            //для датчиков справа
            for (int index = 0; index < UCH_ZR_set_right.Length; index++)
            {
                UCH_ZR_set_right[index].G = new Grid();

                UCH_ZR_set_right[index].UCH_ZR_Set_Address = new TextBox();
                UCH_ZR_set_right[index].UCH_ZR_Set_Address.Text = "";
                UCH_ZR_set_right[index].UCH_ZR_Set_Address.HorizontalAlignment = HorizontalAlignment.Center;
                UCH_ZR_set_right[index].UCH_ZR_Set_Address.VerticalAlignment = VerticalAlignment.Center;
                UCH_ZR_set_right[index].UCH_ZR_Set_Address.Name = "UCH_ZR_Set_Address_Index_" + index.ToString();
                UCH_ZR_set_right[index].UCH_ZR_Set_Address.Margin = new Thickness(0, 0, 100, 5);
                UCH_ZR_set_right[index].UCH_ZR_Set_Address.Width = 40;
                UCH_ZR_set_right[index].UCH_ZR_Set_Address.Height = 18;
                UCH_ZR_set_right[index].UCH_ZR_Set_Address.Style = (Style)UCH_ZR_set_right[index].UCH_ZR_Set_Address.FindResource("TextBoxStyle_Type1");
                UCH_ZR_set_right[index].UCH_ZR_Set_Address.PreviewTextInput += new TextCompositionEventHandler(Cheking_for_numbers);

                UCH_ZR_set_right[index].UCH_ZR_Set_NP = new TextBox();
                UCH_ZR_set_right[index].UCH_ZR_Set_NP.Text = "";
                UCH_ZR_set_right[index].UCH_ZR_Set_NP.HorizontalAlignment = HorizontalAlignment.Center;
                UCH_ZR_set_right[index].UCH_ZR_Set_NP.VerticalAlignment = VerticalAlignment.Center;
                UCH_ZR_set_right[index].UCH_ZR_Set_NP.Name = "UCH_ZR_Set_NP_Index_" + index.ToString();
                UCH_ZR_set_right[index].UCH_ZR_Set_NP.Margin = new Thickness(0, 0, 0, 5);
                UCH_ZR_set_right[index].UCH_ZR_Set_NP.Width = 40;
                UCH_ZR_set_right[index].UCH_ZR_Set_NP.Height = 18;
                UCH_ZR_set_right[index].UCH_ZR_Set_NP.Style = (Style)UCH_ZR_set_right[index].UCH_ZR_Set_NP.FindResource("TextBoxStyle_Type1");
                UCH_ZR_set_right[index].UCH_ZR_Set_NP.PreviewTextInput += new TextCompositionEventHandler(Cheking_for_numbers);

                UCH_ZR_set_right[index].UCH_ZR_Set_Channel = new TextBox();
                UCH_ZR_set_right[index].UCH_ZR_Set_Channel.Text = "";
                UCH_ZR_set_right[index].UCH_ZR_Set_Channel.HorizontalAlignment = HorizontalAlignment.Center;
                UCH_ZR_set_right[index].UCH_ZR_Set_Channel.VerticalAlignment = VerticalAlignment.Center;
                UCH_ZR_set_right[index].UCH_ZR_Set_Channel.Name = "UCH_ZR_Set_Channe_Index_" + index.ToString();
                UCH_ZR_set_right[index].UCH_ZR_Set_Channel.Margin = new Thickness(100, 0, 0, 5);
                UCH_ZR_set_right[index].UCH_ZR_Set_Channel.Width = 40;
                UCH_ZR_set_right[index].UCH_ZR_Set_Channel.Height = 18;
                UCH_ZR_set_right[index].UCH_ZR_Set_Channel.Style = (Style)UCH_ZR_set_right[index].UCH_ZR_Set_Channel.FindResource("TextBoxStyle_Type1");
                UCH_ZR_set_right[index].UCH_ZR_Set_Channel.PreviewTextInput += new TextCompositionEventHandler(Cheking_for_numbers);

                UCH_ZR_set_right[index].G.Children.Add(UCH_ZR_set_right[index].UCH_ZR_Set_Address);
                UCH_ZR_set_right[index].G.Children.Add(UCH_ZR_set_right[index].UCH_ZR_Set_NP);
                UCH_ZR_set_right[index].G.Children.Add(UCH_ZR_set_right[index].UCH_ZR_Set_Channel);
            }
        }
        private void Cheking_for_numbers(object sender, TextCompositionEventArgs e)
        {
        }


        private static readonly Regex UCH_ZR_RangeNumbers = new Regex("[01234567]");    //набор допустимых символов для ввода в TextBox
        private static readonly Regex NP_ZR_RangeNumbers = new Regex("[0-9]");    //набор допустимых символов для ввода в TextBox

        private void Cheking_for_numbers_type1(object sender, TextCompositionEventArgs e)   //ограничение 1 символ (цифры от 0 до 7)
        {
            e.Handled = !UCH_ZR_RangeNumbers.IsMatch(e.Text);

            if (e.Handled == false)
            {
                //ограничиваем ввод данных по количеству символов
                int strlength = ((System.Windows.Controls.TextBox)sender).Text.Length + 1;
                if (strlength > 1)
                {
                    e.Handled = true;
                }
            }
        }
        private void Cheking_for_numbers_type2(object sender, TextCompositionEventArgs e)   //ограничение 2 символа (цифры от 0 до 10)
        {
            e.Handled = !NP_ZR_RangeNumbers.IsMatch(e.Text);

            if (e.Handled == false)
            {
                //ограничиваем ввод данных по количеству символов
                int strlength = ((System.Windows.Controls.TextBox)sender).Text.Length + 1;
                
                if (strlength > 2)
                {
                    e.Handled = true;
                }
            }
        }

        private void UCH_CountZR_Left_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (UCH_Left_StPanel != null) UCH_Left_StPanel.Children.Clear();

            if (UCH_CountZR_Left.Text != "")
            {
                for (int i = 0; i < Convert.ToInt32(UCH_CountZR_Left.Text); i++)
                {
                    UCH_Left_StPanel.Children.Add(UCH_ZR_set_left[i].G);
                }
                //очищаем неактуальные (удаленные) поля
                for (int i = Convert.ToInt32(UCH_CountZR_Left.Text); i < UCH_ZR_set_left.Length; i++)
                {
                    UCH_ZR_set_left[i].UCH_ZR_Set_Address.Text = "";
                    UCH_ZR_set_left[i].UCH_ZR_Set_NP.Text = "";
                    UCH_ZR_set_left[i].UCH_ZR_Set_Channel.Text = "";
                }
            }
        }

        private void UCH_AddZR_Left_Click(object sender, RoutedEventArgs e) //добавить датчики на участке слева
        {
            int TB_Value = 0;

            if (UCH_CountZR_Left.Text != "") TB_Value = Convert.ToInt32(UCH_CountZR_Left.Text);

            TB_Value++;

            if (UCH_ZR_RangeNumbers.IsMatch(TB_Value.ToString()))
            {
                UCH_CountZR_Left.Text = TB_Value.ToString();
            }
        }

        private void UCH_RemoveZR_Left_Click(object sender, RoutedEventArgs e)  //убрать датчики на участке слева
        {
            int TB_Value = 0;

            if (UCH_CountZR_Left.Text != "") TB_Value = Convert.ToInt32(UCH_CountZR_Left.Text);

            if (TB_Value > 0)
            {
                TB_Value--;
                UCH_CountZR_Left.Text = TB_Value.ToString();
            }
        }

        private void UCH_CountZR_Right_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (UCH_Right_StPanel != null) UCH_Right_StPanel.Children.Clear();

            if (UCH_CountZR_Right.Text != "")
            {
                for (int i = 0; i < Convert.ToInt32(UCH_CountZR_Right.Text); i++)
                {
                    UCH_Right_StPanel.Children.Add(UCH_ZR_set_right[i].G);
                }
                //очищаем неактуальные (удаленные) поля
                for (int i = Convert.ToInt32(UCH_CountZR_Right.Text); i < UCH_ZR_set_right.Length; i++)
                {
                    UCH_ZR_set_right[i].UCH_ZR_Set_Address.Text = "";
                    UCH_ZR_set_right[i].UCH_ZR_Set_NP.Text = "";
                    UCH_ZR_set_right[i].UCH_ZR_Set_Channel.Text = "";
                }
            }
        }

        private void UCH_AddZR_Right_Click(object sender, RoutedEventArgs e)
        {
            int TB_Value = 0;

            if (UCH_CountZR_Right.Text != "") TB_Value = Convert.ToInt32(UCH_CountZR_Right.Text);

            TB_Value++;

            if (UCH_ZR_RangeNumbers.IsMatch(TB_Value.ToString()))
            {
                UCH_CountZR_Right.Text = TB_Value.ToString();
            }
        }

        private void UCH_RemoveZR_Right_Click(object sender, RoutedEventArgs e)
        {
            int TB_Value = 0;

            if (UCH_CountZR_Right.Text != "") TB_Value = Convert.ToInt32(UCH_CountZR_Right.Text);

            if (TB_Value > 0)
            {
                TB_Value--;
                UCH_CountZR_Right.Text = TB_Value.ToString();
            }
        }

        private void NP_Channel1_count_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NP_Channel1_StPanel != null) NP_Channel1_StPanel.Children.Clear();

            if (NP_Channel1_count.Text != "")
            {
                //делаем ограничение не больше 10
                if (Convert.ToInt32(NP_Channel1_count.Text) > 10)
                {
                    NP_Channel1_count.Text = "10";
                }
                else
                {
                    //отображаем поля
                    for (int i = 0; i < Convert.ToInt32(NP_Channel1_count.Text); i++)
                    {
                        NP_Channel1_StPanel.Children.Add(NP_ZR_channel1[i].G);
                    }
                    //очищаем неактуальные (удаленные) поля
                    for (int i = Convert.ToInt32(NP_Channel1_count.Text); i < NP_ZR_channel1.Length; i++)
                    {
                        NP_ZR_channel1[i].NP_ZR_Address.Text = "";
                    }
                }
            }
        }

        private void NP_Channel1_Remove_Click(object sender, RoutedEventArgs e)
        {
            int TB_Value = 0;

            if (NP_Channel1_count.Text != "") TB_Value = Convert.ToInt32(NP_Channel1_count.Text);

            if (TB_Value > 0)
            {
                TB_Value--;
                NP_Channel1_count.Text = TB_Value.ToString();
            }
        }

        private void NP_Channel1_Add_Click(object sender, RoutedEventArgs e)
        {
            int TB_Value = 0;

            if (NP_Channel1_count.Text != "") TB_Value = Convert.ToInt32(NP_Channel1_count.Text);

            TB_Value++;

            if (NP_ZR_RangeNumbers.IsMatch(TB_Value.ToString()))
            {
                NP_Channel1_count.Text = TB_Value.ToString();
            }
        }

        private void NP_Channel2_count_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NP_Channel2_StPanel != null) NP_Channel2_StPanel.Children.Clear();

            if (NP_Channel2_count.Text != "")
            {
                //делаем ограничение не больше 10
                if (Convert.ToInt32(NP_Channel2_count.Text) > 10)
                {
                    NP_Channel2_count.Text = "10";
                }
                else
                {
                    //отображаем поля
                    for (int i = 0; i < Convert.ToInt32(NP_Channel2_count.Text); i++)
                    {
                        NP_Channel2_StPanel.Children.Add(NP_ZR_channel2[i].G);
                    }
                    //очищаем неактуальные (удаленные) поля
                    for (int i = Convert.ToInt32(NP_Channel2_count.Text); i < NP_ZR_channel2.Length; i++)
                    {
                        NP_ZR_channel2[i].NP_ZR_Address.Text = "";
                    }
                }
            }
        }

        private void NP_Channel2_Remove_Click(object sender, RoutedEventArgs e)
        {
            int TB_Value = 0;

            if (NP_Channel2_count.Text != "") TB_Value = Convert.ToInt32(NP_Channel2_count.Text);

            if (TB_Value > 0)
            {
                TB_Value--;
                NP_Channel2_count.Text = TB_Value.ToString();
            }
        }

        private void NP_Channel2_Add_Click(object sender, RoutedEventArgs e)
        {
            int TB_Value = 0;

            if (NP_Channel2_count.Text != "") TB_Value = Convert.ToInt32(NP_Channel2_count.Text);

            TB_Value++;

            if (NP_ZR_RangeNumbers.IsMatch(TB_Value.ToString()))
            {
                NP_Channel2_count.Text = TB_Value.ToString();
            }
        }
    }
}
