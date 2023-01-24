﻿using System;
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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NP_Config
{
    public partial class WorkPage : Page
    {
        System.Windows.Threading.DispatcherTimer timer_page = new System.Windows.Threading.DispatcherTimer();
        public WorkPage()
        {
            Initialize_NP_ZR();         //Создаем динамические текстовые боксы для настройки датчиков NP
            Initialize_UCH_ZR_set();     //Создаем динамические текстовые боксы для настройки датчиков на участке
            InitializeComponent();
            Initialize_ZR_List();
            Initialize_UCH_List();

            timer_page.Tick += new EventHandler(tm_page);
            timer_page.Interval = new TimeSpan(0, 0, 0, 0, 50);

            UCH_Button_Visibility(true, false, false);
        }
        struct UCH_ZR_setting
        {
            public Grid G;
            public TextBox UCH_ZR_Set_Address;
            public TextBox UCH_ZR_Set_NP;
            public TextBox UCH_ZR_Set_Channel;
            public bool Address_err;
            public bool NP_err;
            public bool Channel_err;
        }
        public struct NP_ZR_Channel
        {
            public Grid G;
            public TextBox NP_ZR_Address;
        }
        public struct NP_ZR_List   //перечень датчиков во всех NP
        {
            public List<int> channel_1;
            public List<int> channel_2;
        }
        public struct UCH
        {
            public string UCH_Name;
            public List<UCH_ZR> ZR_Left;
            public List<UCH_ZR> ZR_Right;
            public Button UCH_Button;
        }

        //количество датчиков в первом и втором каналах NP
        private int NP_Channel1_CountZR = 0;
        private int NP_Channel2_CountZR = 0;

        public int All_Count_NP = 1;
        public int UCH_Count { get; set; }

        UCH_ZR_setting[] UCH_ZR_set_left = new UCH_ZR_setting[7];
        UCH_ZR_setting[] UCH_ZR_set_right = new UCH_ZR_setting[7];
        public NP_ZR_Channel[] NP_ZR_channel1 = new NP_ZR_Channel[10];
        public NP_ZR_Channel[] NP_ZR_channel2 = new NP_ZR_Channel[10];
        public NP_ZR_List[] ZR_List = new NP_ZR_List[16];
        public UCH[] UCH_list = new UCH[30];

        public void Timer_Start()
        {
            timer_page.Start();
        }
        public void Timer_Stop()
        {
            timer_page.Stop();
        }
        private void tm_page(object sender, EventArgs e)
        {
            //проверка адресов датчиков (добавленных в участок)
            for (int i = 0; i < UCH_ZR_set_left.Length; i++)
            {
                var value = UCH_ZR_set_left[i].UCH_ZR_Set_Address.Text;
                if (value == "")
                {
                    UCH_ZR_set_left[i].UCH_ZR_Set_Address.Style = (Style)UCH_ZR_set_left[i].UCH_ZR_Set_Address.FindResource("TextBoxStyle_Type1");
                    UCH_ZR_set_left[i].Address_err = false;
                }
                else
                {
                    bool result = Search_ZR_Adress(Convert.ToInt32(value));

                    if (result)
                    {
                        UCH_ZR_set_left[i].UCH_ZR_Set_Address.Style = (Style)UCH_ZR_set_left[i].UCH_ZR_Set_Address.FindResource("TextBoxStyle_Type1");
                        UCH_ZR_set_left[i].Address_err = false;
                    }
                    else
                    {
                        UCH_ZR_set_left[i].UCH_ZR_Set_Address.Style = (Style)UCH_ZR_set_left[i].UCH_ZR_Set_Address.FindResource("TextBoxStyle_Type2");
                        UCH_ZR_set_left[i].Address_err = true;
                    }
                }
            }
            for (int i = 0; i < UCH_ZR_set_right.Length; i++)
            {
                var value = UCH_ZR_set_right[i].UCH_ZR_Set_Address.Text;
                if (value == "")
                {
                    UCH_ZR_set_right[i].UCH_ZR_Set_Address.Style = (Style)UCH_ZR_set_right[i].UCH_ZR_Set_Address.FindResource("TextBoxStyle_Type1");
                    UCH_ZR_set_right[i].Address_err = false;
                }
                else
                {
                    bool result = Search_ZR_Adress(Convert.ToInt32(value));

                    if (result)
                    {
                        UCH_ZR_set_right[i].UCH_ZR_Set_Address.Style = (Style)UCH_ZR_set_right[i].UCH_ZR_Set_Address.FindResource("TextBoxStyle_Type1");
                        UCH_ZR_set_right[i].Address_err = false;
                    }
                    else
                    {
                        UCH_ZR_set_right[i].UCH_ZR_Set_Address.Style = (Style)UCH_ZR_set_right[i].UCH_ZR_Set_Address.FindResource("TextBoxStyle_Type2");
                        UCH_ZR_set_right[i].Address_err = true;
                    }
                }
            }
            //проверка принадлежности к NP датчиков (добавленных в участок) слева
            for (int i = 0; i < UCH_ZR_set_left.Length; i++)
            {
                var value = UCH_ZR_set_left[i].UCH_ZR_Set_Address.Text;
                if (value == "" || UCH_ZR_set_left[i].Address_err)
                {
                    UCH_ZR_set_left[i].UCH_ZR_Set_NP.Style = (Style)UCH_ZR_set_left[i].UCH_ZR_Set_NP.FindResource("TextBoxStyle_Type1");
                    UCH_ZR_set_left[i].UCH_ZR_Set_NP.Text = "";
                    UCH_ZR_set_left[i].NP_err = false;
                }
                else
                {
                    List<int> result = Search_ZR_numbersNP(Convert.ToInt32(value));
                    if (result.Count == 1)
                    {
                        UCH_ZR_set_left[i].UCH_ZR_Set_NP.Text = (result[0] + 1).ToString();
                        UCH_ZR_set_left[i].UCH_ZR_Set_NP.Style = (Style)UCH_ZR_set_left[i].UCH_ZR_Set_NP.FindResource("TextBoxStyle_Type1");
                        UCH_ZR_set_left[i].NP_err = false;
                    }
                    else
                    {
                        if (UCH_ZR_set_left[i].UCH_ZR_Set_NP.Text == "")
                        {
                            UCH_ZR_set_left[i].UCH_ZR_Set_NP.Style = (Style)UCH_ZR_set_left[i].UCH_ZR_Set_NP.FindResource("TextBoxStyle_Type2");
                            UCH_ZR_set_left[i].UCH_ZR_Set_NP.Text = "";
                            UCH_ZR_set_left[i].NP_err = true;
                        }
                    }
                }
            }
            //проверка принадлежности к NP датчиков (добавленных в участок) справа
            for (int i = 0; i < UCH_ZR_set_right.Length; i++)
            {
                var value = UCH_ZR_set_right[i].UCH_ZR_Set_Address.Text;
                if (value == "" || UCH_ZR_set_right[i].Address_err)
                {
                    UCH_ZR_set_right[i].UCH_ZR_Set_NP.Style = (Style)UCH_ZR_set_right[i].UCH_ZR_Set_NP.FindResource("TextBoxStyle_Type1");
                    UCH_ZR_set_right[i].UCH_ZR_Set_NP.Text = "";
                    UCH_ZR_set_right[i].NP_err = false;
                }
                else
                {
                    List<int> result = Search_ZR_numbersNP(Convert.ToInt32(value));
                    if (result.Count == 1)
                    {
                        UCH_ZR_set_right[i].UCH_ZR_Set_NP.Text = (result[0] + 1).ToString();
                        UCH_ZR_set_right[i].UCH_ZR_Set_NP.Style = (Style)UCH_ZR_set_right[i].UCH_ZR_Set_NP.FindResource("TextBoxStyle_Type1");
                        UCH_ZR_set_right[i].NP_err = false;
                    }
                    else
                    {
                        if (UCH_ZR_set_right[i].UCH_ZR_Set_NP.Text == "")
                        {
                            UCH_ZR_set_right[i].UCH_ZR_Set_NP.Style = (Style)UCH_ZR_set_right[i].UCH_ZR_Set_NP.FindResource("TextBoxStyle_Type2");
                            UCH_ZR_set_right[i].UCH_ZR_Set_NP.Text = "";
                            UCH_ZR_set_right[i].NP_err = true;
                        }
                    }
                }
            }
            //проверка на соответствие введенного номера NP слева
            for (int i = 0; i < UCH_ZR_set_left.Length; i++)
            {
                if (UCH_ZR_set_left[i].UCH_ZR_Set_NP.Text != "")
                {
                    var value = UCH_ZR_set_left[i].UCH_ZR_Set_Address.Text;
                    int number_NP = Convert.ToInt32(UCH_ZR_set_left[i].UCH_ZR_Set_NP.Text);
                    List<int> result = Search_ZR_numbersNP(Convert.ToInt32(value));

                    bool search_result = false;
                    //поиск
                    for (int a = 0; a < result.Count; a++) 
                    {
                        if ((result[a] + 1) == number_NP) search_result = true;
                    }
                    //результат
                    if (search_result)
                    {
                        UCH_ZR_set_left[i].UCH_ZR_Set_NP.Style = (Style)UCH_ZR_set_left[i].UCH_ZR_Set_NP.FindResource("TextBoxStyle_Type1");
                        UCH_ZR_set_left[i].NP_err = false;
                    }
                    else
                    {
                        UCH_ZR_set_left[i].UCH_ZR_Set_NP.Style = (Style)UCH_ZR_set_left[i].UCH_ZR_Set_NP.FindResource("TextBoxStyle_Type2");
                        UCH_ZR_set_left[i].NP_err = true;
                    }
                }
            }
            //проверка на соответствие введенного номера NP справа
            for (int i = 0; i < UCH_ZR_set_right.Length; i++)
            {
                if (UCH_ZR_set_right[i].UCH_ZR_Set_NP.Text != "")
                {
                    var value = UCH_ZR_set_right[i].UCH_ZR_Set_Address.Text;
                    int number_NP = Convert.ToInt32(UCH_ZR_set_right[i].UCH_ZR_Set_NP.Text);
                    List<int> result = Search_ZR_numbersNP(Convert.ToInt32(value));

                    bool search_result = false;
                    //поиск
                    for (int a = 0; a < result.Count; a++)
                    {
                        if ((result[a] + 1) == number_NP) search_result = true;
                    }
                    //результат
                    if (search_result)
                    {
                        UCH_ZR_set_right[i].UCH_ZR_Set_NP.Style = (Style)UCH_ZR_set_right[i].UCH_ZR_Set_NP.FindResource("TextBoxStyle_Type1");
                        UCH_ZR_set_right[i].NP_err = false;
                    }
                    else
                    {
                        UCH_ZR_set_right[i].UCH_ZR_Set_NP.Style = (Style)UCH_ZR_set_right[i].UCH_ZR_Set_NP.FindResource("TextBoxStyle_Type2");
                        UCH_ZR_set_right[i].NP_err = true;
                    }
                }
            }
            //автозаполнение принадлежности датчика к каналу NP (слева)
            for (int i = 0; i < UCH_ZR_set_left.Length; i++)
            {
                var value = UCH_ZR_set_left[i].UCH_ZR_Set_NP.Text;
                if (value == "" || UCH_ZR_set_left[i].NP_err)
                {
                    UCH_ZR_set_left[i].UCH_ZR_Set_Channel.Style = (Style)UCH_ZR_set_left[i].UCH_ZR_Set_Channel.FindResource("TextBoxStyle_Type1");
                    UCH_ZR_set_left[i].UCH_ZR_Set_Channel.Text = "";
                    UCH_ZR_set_left[i].Channel_err = false;
                }
                else
                {
                    int number_ZR = Convert.ToInt32(UCH_ZR_set_left[i].UCH_ZR_Set_Address.Text);
                    int number_NP = Convert.ToInt32(UCH_ZR_set_left[i].UCH_ZR_Set_NP.Text);

                    int result = Search_NP_count_channel(number_ZR, number_NP);
                    if (result != 0)
                    {
                        UCH_ZR_set_left[i].UCH_ZR_Set_Channel.Style = (Style)UCH_ZR_set_left[i].UCH_ZR_Set_Channel.FindResource("TextBoxStyle_Type1");
                        UCH_ZR_set_left[i].Channel_err = false;
                        UCH_ZR_set_left[i].UCH_ZR_Set_Channel.Text = result.ToString();
                    }
                    else
                    {
                        if (UCH_ZR_set_left[i].UCH_ZR_Set_Channel.Text == "")
                        {
                            UCH_ZR_set_left[i].UCH_ZR_Set_Channel.Style = (Style)UCH_ZR_set_left[i].UCH_ZR_Set_Channel.FindResource("TextBoxStyle_Type2");
                            UCH_ZR_set_left[i].Channel_err = true;
                        }
                    }
                }
            }
            //проверка на соответствие введенного вручную номера канала NP (слева)
            for (int i = 0; i < UCH_ZR_set_left.Length; i++)
            {
                if (UCH_ZR_set_left[i].UCH_ZR_Set_Channel.Text != "")
                {
                    int number_ZR = Convert.ToInt32(UCH_ZR_set_left[i].UCH_ZR_Set_Address.Text);
                    int number_NP = Convert.ToInt32(UCH_ZR_set_left[i].UCH_ZR_Set_NP.Text);
                    int number_Channel = Convert.ToInt32(UCH_ZR_set_left[i].UCH_ZR_Set_Channel.Text);

                    bool search_result = false;
                    //поиск
                    if (number_Channel == 1)
                    {
                        for (int a = 0; a < ZR_List[number_NP - 1].channel_1.Count; a++)
                        {
                            if (ZR_List[number_NP - 1].channel_1[a] == number_ZR) search_result = true;
                        }
                    }
                    if (number_Channel == 2)
                    {
                        for (int a = 0; a < ZR_List[number_NP - 1].channel_2.Count; a++)
                        {
                            if (ZR_List[number_NP - 1].channel_2[a] == number_ZR) search_result = true;
                        }
                    }
                    //результат
                    if (search_result)
                    {
                        UCH_ZR_set_left[i].UCH_ZR_Set_Channel.Style = (Style)UCH_ZR_set_left[i].UCH_ZR_Set_Channel.FindResource("TextBoxStyle_Type1");
                        UCH_ZR_set_left[i].Channel_err = false;
                    }
                    else
                    {
                        UCH_ZR_set_left[i].UCH_ZR_Set_Channel.Style = (Style)UCH_ZR_set_left[i].UCH_ZR_Set_Channel.FindResource("TextBoxStyle_Type2");
                        UCH_ZR_set_left[i].Channel_err = true;
                    }
                }
            }
            //автозаполнение принадлежности датчика к каналу NP (справа)
            for (int i = 0; i < UCH_ZR_set_right.Length; i++)
            {
                var value = UCH_ZR_set_right[i].UCH_ZR_Set_NP.Text;
                if (value == "" || UCH_ZR_set_right[i].NP_err)
                {
                    UCH_ZR_set_right[i].UCH_ZR_Set_Channel.Style = (Style)UCH_ZR_set_right[i].UCH_ZR_Set_Channel.FindResource("TextBoxStyle_Type1");
                    UCH_ZR_set_right[i].UCH_ZR_Set_Channel.Text = "";
                    UCH_ZR_set_right[i].Channel_err = false;
                }
                else
                {
                    int number_ZR = Convert.ToInt32(UCH_ZR_set_right[i].UCH_ZR_Set_Address.Text);
                    int number_NP = Convert.ToInt32(UCH_ZR_set_right[i].UCH_ZR_Set_NP.Text);

                    int result = Search_NP_count_channel(number_ZR, number_NP);
                    if (result != 0)
                    {
                        UCH_ZR_set_right[i].UCH_ZR_Set_Channel.Style = (Style)UCH_ZR_set_right[i].UCH_ZR_Set_Channel.FindResource("TextBoxStyle_Type1");
                        UCH_ZR_set_right[i].Channel_err = false;
                        UCH_ZR_set_right[i].UCH_ZR_Set_Channel.Text = result.ToString();
                    }
                    else
                    {
                        if (UCH_ZR_set_right[i].UCH_ZR_Set_Channel.Text == "")
                        {
                            UCH_ZR_set_right[i].UCH_ZR_Set_Channel.Style = (Style)UCH_ZR_set_right[i].UCH_ZR_Set_Channel.FindResource("TextBoxStyle_Type2");
                            UCH_ZR_set_right[i].Channel_err = true;
                        }
                    }
                }
            }
            //проверка на соответствие введенного вручную номера канала NP (справа)
            for (int i = 0; i < UCH_ZR_set_right.Length; i++)
            {
                if (UCH_ZR_set_right[i].UCH_ZR_Set_Channel.Text != "")
                {
                    int number_ZR = Convert.ToInt32(UCH_ZR_set_right[i].UCH_ZR_Set_Address.Text);
                    int number_NP = Convert.ToInt32(UCH_ZR_set_right[i].UCH_ZR_Set_NP.Text);
                    int number_Channel = Convert.ToInt32(UCH_ZR_set_right[i].UCH_ZR_Set_Channel.Text);

                    bool search_result = false;
                    //поиск
                    if (number_Channel == 1)
                    {
                        for (int a = 0; a < ZR_List[number_NP - 1].channel_1.Count; a++)
                        {
                            if (ZR_List[number_NP - 1].channel_1[a] == number_ZR) search_result = true;
                        }
                    }
                    if (number_Channel == 2)
                    {
                        for (int a = 0; a < ZR_List[number_NP - 1].channel_2.Count; a++)
                        {
                            if (ZR_List[number_NP - 1].channel_2[a] == number_ZR) search_result = true;
                        }
                    }
                    //результат
                    if (search_result)
                    {
                        UCH_ZR_set_right[i].UCH_ZR_Set_Channel.Style = (Style)UCH_ZR_set_right[i].UCH_ZR_Set_Channel.FindResource("TextBoxStyle_Type1");
                        UCH_ZR_set_right[i].Channel_err = false;
                    }
                    else
                    {
                        UCH_ZR_set_right[i].UCH_ZR_Set_Channel.Style = (Style)UCH_ZR_set_right[i].UCH_ZR_Set_Channel.FindResource("TextBoxStyle_Type2");
                        UCH_ZR_set_right[i].Channel_err = true;
                    }
                }
            }
            //ошибка полей если они не заполнены
            for (int i = 0; i < Convert.ToInt32(UCH_CountZR_Left.Text); i++)
            {
                if (UCH_ZR_set_left[i].UCH_ZR_Set_Address.Text == "") UCH_ZR_set_left[i].Address_err = true;
                if (UCH_ZR_set_left[i].UCH_ZR_Set_NP.Text == "") UCH_ZR_set_left[i].NP_err = true;
                if (UCH_ZR_set_left[i].UCH_ZR_Set_Channel.Text == "") UCH_ZR_set_left[i].Channel_err = true;
            }
            for (int i = 0; i < Convert.ToInt32(UCH_CountZR_Right.Text); i++)
            {
                if (UCH_ZR_set_right[i].UCH_ZR_Set_Address.Text == "") UCH_ZR_set_right[i].Address_err = true;
                if (UCH_ZR_set_right[i].UCH_ZR_Set_NP.Text == "") UCH_ZR_set_right[i].NP_err = true;
                if (UCH_ZR_set_right[i].UCH_ZR_Set_Channel.Text == "") UCH_ZR_set_right[i].Channel_err = true;
            }
            //настраиваем видимость элементов страницы
            bool name_result = false;
            if (UCH_Name.Text != "") name_result = true;
            bool ZR_left_result = true;
            for (int i = 0; i < Convert.ToInt32(UCH_CountZR_Left.Text); i++)
            {
                if (UCH_ZR_set_left[i].Address_err) ZR_left_result = false;
                if (UCH_ZR_set_left[i].NP_err) ZR_left_result = false;
                if (UCH_ZR_set_left[i].Channel_err) ZR_left_result = false;
            }
            bool ZR_right_result = true;
            for (int i = 0; i < Convert.ToInt32(UCH_CountZR_Right.Text); i++)
            {
                if (UCH_ZR_set_right[i].Address_err) ZR_right_result = false;
                if (UCH_ZR_set_right[i].NP_err) ZR_right_result = false;
                if (UCH_ZR_set_right[i].Channel_err) ZR_right_result = false;
            }
            if ((UCH_CountZR_Left.Text == "" || UCH_CountZR_Left.Text == "0") && (UCH_CountZR_Right.Text == "" || UCH_CountZR_Right.Text == "0"))
            {
                ZR_left_result = false;
                ZR_right_result = false;
            }
            if (name_result && ZR_left_result && ZR_right_result)   //если ошибок нет
            {
                UCH_Add.IsEnabled = true;
                if(UCH_Save.Visibility == Visibility.Visible) UCH_Save.IsEnabled= true;
            }
            else 
            { 
                UCH_Add.IsEnabled = false;
                if (UCH_Save.Visibility == Visibility.Visible) UCH_Save.IsEnabled = false;
            }
        }
        private void Initialize_ZR_List()
        {
            for (int i = 0; i < ZR_List.Length; i++)
            {
                ZR_List[i].channel_1 = new List<int>();
                ZR_List[i].channel_2 = new List<int>();
            }
        }
        private void Initialize_UCH_List()
        {
            for (int i = 0; i < UCH_list.Length; i++)
            {
                UCH_list[i].UCH_Name = "";
                UCH_list[i].ZR_Left = new List<UCH_ZR>();
                UCH_list[i].ZR_Right = new List<UCH_ZR>();

                UCH_list[i].UCH_Button = new Button();
                UCH_list[i].UCH_Button.Content = "";
                UCH_list[i].UCH_Button.Style = (Style)UCH_list[i].UCH_Button.FindResource("ButtonStyle_UCH_Name");
                UCH_list[i].UCH_Button.Click += new RoutedEventHandler(UCH_list_Click);
            }
        }
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
                NP_ZR_channel1[index].NP_ZR_Address.PreviewTextInput += new TextCompositionEventHandler(Cheking_for_numbers_type3);
                NP_ZR_channel1[index].NP_ZR_Address.TextChanged += new TextChangedEventHandler(NP_Channel_TextChanged);

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
                NP_ZR_channel2[index].NP_ZR_Address.PreviewTextInput += new TextCompositionEventHandler(Cheking_for_numbers_type3);
                NP_ZR_channel2[index].NP_ZR_Address.TextChanged += new TextChangedEventHandler(NP_Channel_TextChanged);

                NP_ZR_channel2[index].G.Children.Add(NP_ZR_channel2[index].NP_ZR_Address);
            }
        }
        private void Initialize_UCH_ZR_set()    //Создаем динамические текстовые боксы для настройки датчиков на участке
        {
            //для датчиков слева
            for (int index = 0; index < UCH_ZR_set_left.Length; index++)
            {
                UCH_ZR_set_left[index].G = new Grid();
                UCH_ZR_set_left[index].Address_err = false;
                UCH_ZR_set_left[index].NP_err = false;
                UCH_ZR_set_left[index].Channel_err = false;

                UCH_ZR_set_left[index].UCH_ZR_Set_Address = new TextBox();
                UCH_ZR_set_left[index].UCH_ZR_Set_Address.Text = "";
                UCH_ZR_set_left[index].UCH_ZR_Set_Address.HorizontalAlignment = HorizontalAlignment.Center;
                UCH_ZR_set_left[index].UCH_ZR_Set_Address.VerticalAlignment = VerticalAlignment.Center;
                UCH_ZR_set_left[index].UCH_ZR_Set_Address.Name = "UCH_ZR_Set_Address_Index_" + index.ToString();
                UCH_ZR_set_left[index].UCH_ZR_Set_Address.Margin = new Thickness(0, 0, 100, 5);
                UCH_ZR_set_left[index].UCH_ZR_Set_Address.Width = 40;
                UCH_ZR_set_left[index].UCH_ZR_Set_Address.Height = 18;
                UCH_ZR_set_left[index].UCH_ZR_Set_Address.Style = (Style)UCH_ZR_set_left[index].UCH_ZR_Set_Address.FindResource("TextBoxStyle_Type1");
                UCH_ZR_set_left[index].UCH_ZR_Set_Address.PreviewTextInput += new TextCompositionEventHandler(Cheking_for_numbers_type3);

                UCH_ZR_set_left[index].UCH_ZR_Set_NP = new TextBox();
                UCH_ZR_set_left[index].UCH_ZR_Set_NP.Text = "";
                UCH_ZR_set_left[index].UCH_ZR_Set_NP.HorizontalAlignment = HorizontalAlignment.Center;
                UCH_ZR_set_left[index].UCH_ZR_Set_NP.VerticalAlignment = VerticalAlignment.Center;
                UCH_ZR_set_left[index].UCH_ZR_Set_NP.Name = "UCH_ZR_Set_NP_Index_" + index.ToString();
                UCH_ZR_set_left[index].UCH_ZR_Set_NP.Margin = new Thickness(0, 0, 0, 5);
                UCH_ZR_set_left[index].UCH_ZR_Set_NP.Width = 40;
                UCH_ZR_set_left[index].UCH_ZR_Set_NP.Height = 18;
                UCH_ZR_set_left[index].UCH_ZR_Set_NP.Style = (Style)UCH_ZR_set_left[index].UCH_ZR_Set_NP.FindResource("TextBoxStyle_Type1");
                UCH_ZR_set_left[index].UCH_ZR_Set_NP.PreviewTextInput += new TextCompositionEventHandler(Cheking_for_numbers_type2);

                UCH_ZR_set_left[index].UCH_ZR_Set_Channel = new TextBox();
                UCH_ZR_set_left[index].UCH_ZR_Set_Channel.Text = "";
                UCH_ZR_set_left[index].UCH_ZR_Set_Channel.HorizontalAlignment = HorizontalAlignment.Center;
                UCH_ZR_set_left[index].UCH_ZR_Set_Channel.VerticalAlignment = VerticalAlignment.Center;
                UCH_ZR_set_left[index].UCH_ZR_Set_Channel.Name = "UCH_ZR_Set_Channe_Index_" + index.ToString();
                UCH_ZR_set_left[index].UCH_ZR_Set_Channel.Margin = new Thickness(100, 0, 0, 5);
                UCH_ZR_set_left[index].UCH_ZR_Set_Channel.Width = 40;
                UCH_ZR_set_left[index].UCH_ZR_Set_Channel.Height = 18;
                UCH_ZR_set_left[index].UCH_ZR_Set_Channel.Style = (Style)UCH_ZR_set_left[index].UCH_ZR_Set_Channel.FindResource("TextBoxStyle_Type1");
                UCH_ZR_set_left[index].UCH_ZR_Set_Channel.PreviewTextInput += new TextCompositionEventHandler(Cheking_for_numbers_type4);

                UCH_ZR_set_left[index].G.Children.Add(UCH_ZR_set_left[index].UCH_ZR_Set_Address);
                UCH_ZR_set_left[index].G.Children.Add(UCH_ZR_set_left[index].UCH_ZR_Set_NP);
                UCH_ZR_set_left[index].G.Children.Add(UCH_ZR_set_left[index].UCH_ZR_Set_Channel);
            }
            //для датчиков справа
            for (int index = 0; index < UCH_ZR_set_right.Length; index++)
            {
                UCH_ZR_set_right[index].G = new Grid();
                UCH_ZR_set_right[index].Address_err = false;
                UCH_ZR_set_right[index].NP_err = false;
                UCH_ZR_set_right[index].Channel_err = false;

                UCH_ZR_set_right[index].UCH_ZR_Set_Address = new TextBox();
                UCH_ZR_set_right[index].UCH_ZR_Set_Address.Text = "";
                UCH_ZR_set_right[index].UCH_ZR_Set_Address.HorizontalAlignment = HorizontalAlignment.Center;
                UCH_ZR_set_right[index].UCH_ZR_Set_Address.VerticalAlignment = VerticalAlignment.Center;
                UCH_ZR_set_right[index].UCH_ZR_Set_Address.Name = "UCH_ZR_Set_Address_Index_" + index.ToString();
                UCH_ZR_set_right[index].UCH_ZR_Set_Address.Margin = new Thickness(0, 0, 100, 5);
                UCH_ZR_set_right[index].UCH_ZR_Set_Address.Width = 40;
                UCH_ZR_set_right[index].UCH_ZR_Set_Address.Height = 18;
                UCH_ZR_set_right[index].UCH_ZR_Set_Address.Style = (Style)UCH_ZR_set_right[index].UCH_ZR_Set_Address.FindResource("TextBoxStyle_Type1");
                UCH_ZR_set_right[index].UCH_ZR_Set_Address.PreviewTextInput += new TextCompositionEventHandler(Cheking_for_numbers_type3);

                UCH_ZR_set_right[index].UCH_ZR_Set_NP = new TextBox();
                UCH_ZR_set_right[index].UCH_ZR_Set_NP.Text = "";
                UCH_ZR_set_right[index].UCH_ZR_Set_NP.HorizontalAlignment = HorizontalAlignment.Center;
                UCH_ZR_set_right[index].UCH_ZR_Set_NP.VerticalAlignment = VerticalAlignment.Center;
                UCH_ZR_set_right[index].UCH_ZR_Set_NP.Name = "UCH_ZR_Set_NP_Index_" + index.ToString();
                UCH_ZR_set_right[index].UCH_ZR_Set_NP.Margin = new Thickness(0, 0, 0, 5);
                UCH_ZR_set_right[index].UCH_ZR_Set_NP.Width = 40;
                UCH_ZR_set_right[index].UCH_ZR_Set_NP.Height = 18;
                UCH_ZR_set_right[index].UCH_ZR_Set_NP.Style = (Style)UCH_ZR_set_right[index].UCH_ZR_Set_NP.FindResource("TextBoxStyle_Type1");
                UCH_ZR_set_right[index].UCH_ZR_Set_NP.PreviewTextInput += new TextCompositionEventHandler(Cheking_for_numbers_type2);

                UCH_ZR_set_right[index].UCH_ZR_Set_Channel = new TextBox();
                UCH_ZR_set_right[index].UCH_ZR_Set_Channel.Text = "";
                UCH_ZR_set_right[index].UCH_ZR_Set_Channel.HorizontalAlignment = HorizontalAlignment.Center;
                UCH_ZR_set_right[index].UCH_ZR_Set_Channel.VerticalAlignment = VerticalAlignment.Center;
                UCH_ZR_set_right[index].UCH_ZR_Set_Channel.Name = "UCH_ZR_Set_Channe_Index_" + index.ToString();
                UCH_ZR_set_right[index].UCH_ZR_Set_Channel.Margin = new Thickness(100, 0, 0, 5);
                UCH_ZR_set_right[index].UCH_ZR_Set_Channel.Width = 40;
                UCH_ZR_set_right[index].UCH_ZR_Set_Channel.Height = 18;
                UCH_ZR_set_right[index].UCH_ZR_Set_Channel.Style = (Style)UCH_ZR_set_right[index].UCH_ZR_Set_Channel.FindResource("TextBoxStyle_Type1");
                UCH_ZR_set_right[index].UCH_ZR_Set_Channel.PreviewTextInput += new TextCompositionEventHandler(Cheking_for_numbers_type4);

                UCH_ZR_set_right[index].G.Children.Add(UCH_ZR_set_right[index].UCH_ZR_Set_Address);
                UCH_ZR_set_right[index].G.Children.Add(UCH_ZR_set_right[index].UCH_ZR_Set_NP);
                UCH_ZR_set_right[index].G.Children.Add(UCH_ZR_set_right[index].UCH_ZR_Set_Channel);
            }
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
        private void Cheking_for_numbers_type2(object sender, TextCompositionEventArgs e)   //ограничение 2 символа (весь перечень цифр)
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
        private void Cheking_for_numbers_type3(object sender, TextCompositionEventArgs e)   //ограничение 3 символа (весь перечень цифр)
        {
            e.Handled = !NP_ZR_RangeNumbers.IsMatch(e.Text);

            if (e.Handled == false)
            {
                //ограничиваем ввод данных по количеству символов
                int strlength = ((System.Windows.Controls.TextBox)sender).Text.Length + 1;

                if (strlength > 3)
                {
                    e.Handled = true;
                }
            }
        }
        private void Cheking_for_numbers_type4(object sender, TextCompositionEventArgs e)   //ограничение 1 символ (цифры 1 или 2)
        {
            bool result = true;
            if (e.Text == "1" || e.Text == "2")
            {
                result  = false;

                //ограничиваем ввод данных по количеству символов
                int strlength = ((System.Windows.Controls.TextBox)sender).Text.Length + 1;

                if (strlength > 1)
                {
                    result = true;
                }
            }

            e.Handled = result;
        }

        private void NP_Channel_TextChanged(object sender, TextChangedEventArgs e)  //ограничение адресов датчиков - 127
        {
            var value = ((System.Windows.Controls.TextBox)e.OriginalSource).Text;
            if (value != "" && (Convert.ToInt32(value) > 127))
            {
                ((System.Windows.Controls.TextBox)e.OriginalSource).Text = "127";
            }
        }


        private bool Search_ZR_Adress(int ZR)   //поиск датчика по всем NP (возврат - результат поиска)
        {
            bool result = false;

            for (int a = 0; a < ZR_List.Length; a++)
            {
                for (int b = 0; b < ZR_List[a].channel_1.Count; b++)
                {
                    if (ZR == ZR_List[a].channel_1[b]) result = true;
                }
                for (int b = 0; b < ZR_List[a].channel_2.Count; b++)
                {
                    if (ZR == ZR_List[a].channel_2[b]) result = true;
                }
            }

            return result;
        }
        private List<int> Search_ZR_numbersNP(int ZR)   //поиск датчика по всем NP (возврат - индексы NP в которых есть этот датчик)
        {
            List<int> NP_Index = new List<int>();

            for (int a = 0; a < ZR_List.Length; a++)
            {
                for (int b = 0; b < ZR_List[a].channel_1.Count; b++)
                {
                    if (ZR == ZR_List[a].channel_1[b]) NP_Index.Add(a);
                }
                for (int b = 0; b < ZR_List[a].channel_2.Count; b++)
                {
                    if (ZR == ZR_List[a].channel_2[b]) NP_Index.Add(a);
                }
            }
            var List_Distinct = NP_Index.Distinct();

            List<int> result= new List<int>();
            foreach (var item in List_Distinct)
            {
                result.Add(item);
            }
            return result;
        }
        private int Search_NP_count_channel(int ZR, int NP) //поиск датчика в NP на соответствие каналу NP (возврат 1 - датчик в первом канале, 2 - во втором, 0 - не определено)
        {
            int result = 0;
            bool channel_1 = false; 
            bool channel_2 = false;

            //проверка первого канала
            for (int a = 0; a < ZR_List[NP - 1].channel_1.Count; a++)
            {
                if (ZR == ZR_List[NP - 1].channel_1[a])
                {
                    channel_1 = true;
                    break;
                }
            }
            //проверка второго канала
            for (int a = 0; a < ZR_List[NP - 1].channel_2.Count; a++)
            {
                if (ZR == ZR_List[NP - 1].channel_2[a])
                {
                    channel_2 = true;
                    break;
                }
            }
            //формируем результат
            if (channel_1 && channel_2 == false) result = 1;
            if (channel_1 == false && channel_2) result = 2;
            return result;
        }

        private void UCH_NP_TextChanged(object sender, TextChangedEventArgs e)  //
        {
            var value = ((System.Windows.Controls.TextBox)e.OriginalSource).Text;
            if (value != "")
            {
                if (Convert.ToInt32(value) > All_Count_NP)
                {
                    ((System.Windows.Controls.TextBox)e.OriginalSource).Style = (Style)((System.Windows.Controls.TextBox)e.OriginalSource).FindResource("TextBoxStyle_Type2");
                }
                else
                {
                    ((System.Windows.Controls.TextBox)e.OriginalSource).Style = (Style)((System.Windows.Controls.TextBox)e.OriginalSource).FindResource("TextBoxStyle_Type1");
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
                    UCH_ZR_set_left[i].UCH_ZR_Set_Address.Style = (Style)UCH_ZR_set_left[i].UCH_ZR_Set_Address.FindResource("TextBoxStyle_Type1");
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
                    UCH_ZR_set_right[i].UCH_ZR_Set_Address.Style = (Style)UCH_ZR_set_right[i].UCH_ZR_Set_Address.FindResource("TextBoxStyle_Type1");
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
                NP_Channel1_CountZR = Convert.ToInt32(NP_Channel1_count.Text);
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
                NP_Channel2_CountZR = Convert.ToInt32(NP_Channel2_count.Text);
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

        private void UCH_Add_Click(object sender, RoutedEventArgs e)
        {
            //добавляем участок
            UCH_list[UCH_Count].UCH_Name = UCH_Name.Text;

            for (int i = 0; i < Convert.ToInt32(UCH_CountZR_Left.Text); i++)
            {
                UCH_list[UCH_Count].ZR_Left.Add(new UCH_ZR
                {
                    ZR_Address = Convert.ToInt16(UCH_ZR_set_left[i].UCH_ZR_Set_Address.Text),
                    ZR_NP = Convert.ToInt16(UCH_ZR_set_left[i].UCH_ZR_Set_NP.Text),
                    ZR_Channel = Convert.ToInt16(UCH_ZR_set_left[i].UCH_ZR_Set_Channel.Text)
                });
            }
            for (int i = 0; i < Convert.ToInt32(UCH_CountZR_Right.Text); i++)
            {
                UCH_list[UCH_Count].ZR_Right.Add(new UCH_ZR
                {
                    ZR_Address = Convert.ToInt16(UCH_ZR_set_right[i].UCH_ZR_Set_Address.Text),
                    ZR_NP = Convert.ToInt16(UCH_ZR_set_right[i].UCH_ZR_Set_NP.Text),
                    ZR_Channel = Convert.ToInt16(UCH_ZR_set_right[i].UCH_ZR_Set_Channel.Text)
                });
            }

            UCH_list[UCH_Count].UCH_Button.Content = UCH_Name.Text;
            WrapPanel_UCH.Children.Add(UCH_list[UCH_Count].UCH_Button);

            UCH_Count++;
            //очищаем поля
            UCH_CountZR_Left.Text = "0";
            UCH_CountZR_Right.Text = "0";
            UCH_Name.Text = "";
        }
        private void UCH_list_Click(object sender, RoutedEventArgs e)
        {
            //определяем название нажатого участка
            string Button_Name = ((System.Windows.Controls.ContentControl)e.Source).Content.ToString();

            //отображаем информацию по участку
            UCH_Open(Button_Name);

            UCH_Button_Visibility(false, true, true);
        }
        private void UCH_Button_Visibility(bool add, bool save, bool delete)
        {
            if (add) UCH_Add.Visibility = Visibility.Visible;
            else UCH_Add.Visibility = Visibility.Hidden;

            if (save) UCH_Save.Visibility = Visibility.Visible;
            else UCH_Save.Visibility = Visibility.Hidden;

            if (delete) UCH_Delete.Visibility = Visibility.Visible;
            else UCH_Delete.Visibility = Visibility.Hidden;
        }
        private void UCH_Open(string Name)
        {
            //определяем индекс
            int UCH_Index = 0;
            for (int i = 0; i < UCH_list.Length; i++)
            {
                if (UCH_list[i].UCH_Name == Name) UCH_Index = i;
            }

            //очищаем поля
            UCH_CountZR_Left.Text = "0";
            UCH_CountZR_Right.Text = "0";
            UCH_Name.Text = "";
            //открываем поля участка
            UCH_Name.Text = UCH_list[UCH_Index].UCH_Name;
            UCH_CountZR_Left.Text = UCH_list[UCH_Index].ZR_Left.Count.ToString();
            UCH_CountZR_Right.Text = UCH_list[UCH_Index].ZR_Right.Count.ToString();
            for (int i = 0; i < UCH_list[UCH_Index].ZR_Left.Count; i++)
            {
                UCH_ZR_set_left[i].UCH_ZR_Set_Address.Text = UCH_list[UCH_Index].ZR_Left[i].ZR_Address.ToString();
                UCH_ZR_set_left[i].UCH_ZR_Set_NP.Text = UCH_list[UCH_Index].ZR_Left[i].ZR_NP.ToString();
                UCH_ZR_set_left[i].UCH_ZR_Set_Channel.Text = UCH_list[UCH_Index].ZR_Left[i].ZR_Channel.ToString();
            }
            for (int i = 0; i < UCH_list[UCH_Index].ZR_Right.Count; i++)
            {
                UCH_ZR_set_right[i].UCH_ZR_Set_Address.Text = UCH_list[UCH_Index].ZR_Right[i].ZR_Address.ToString();
                UCH_ZR_set_right[i].UCH_ZR_Set_NP.Text = UCH_list[UCH_Index].ZR_Right[i].ZR_NP.ToString();
                UCH_ZR_set_right[i].UCH_ZR_Set_Channel.Text = UCH_list[UCH_Index].ZR_Right[i].ZR_Channel.ToString();
            }

            UCH_Open_Index = UCH_Index;
        }
        private int UCH_Open_Index = 0; //индекс открытого участка
        private void UCH_Save_Click(object sender, RoutedEventArgs e)
        {
            //удаляем старые данные
            UCH_list[UCH_Open_Index].UCH_Name = "";
            UCH_list[UCH_Open_Index].ZR_Left.Clear();
            UCH_list[UCH_Open_Index].ZR_Right.Clear();
            //сохраняем данные
            UCH_list[UCH_Open_Index].UCH_Name = UCH_Name.Text;

            for (int i = 0; i < Convert.ToInt32(UCH_CountZR_Left.Text); i++)
            {
                UCH_list[UCH_Open_Index].ZR_Left.Add(new UCH_ZR
                {
                    ZR_Address = Convert.ToInt16(UCH_ZR_set_left[i].UCH_ZR_Set_Address.Text),
                    ZR_NP = Convert.ToInt16(UCH_ZR_set_left[i].UCH_ZR_Set_NP.Text),
                    ZR_Channel = Convert.ToInt16(UCH_ZR_set_left[i].UCH_ZR_Set_Channel.Text)
                });
            }
            for (int i = 0; i < Convert.ToInt32(UCH_CountZR_Right.Text); i++)
            {
                UCH_list[UCH_Open_Index].ZR_Right.Add(new UCH_ZR
                {
                    ZR_Address = Convert.ToInt16(UCH_ZR_set_right[i].UCH_ZR_Set_Address.Text),
                    ZR_NP = Convert.ToInt16(UCH_ZR_set_right[i].UCH_ZR_Set_NP.Text),
                    ZR_Channel = Convert.ToInt16(UCH_ZR_set_right[i].UCH_ZR_Set_Channel.Text)
                });
            }

            UCH_list[UCH_Open_Index].UCH_Button.Content = UCH_Name.Text;

            //очищаем поля
            UCH_CountZR_Left.Text = "0";
            UCH_CountZR_Right.Text = "0";
            UCH_Name.Text = "";

            UCH_Button_Visibility(true, false, false);
        }

        private void UCH_Delete_Click(object sender, RoutedEventArgs e)
        {
            //удаляем старые данные
            UCH_list[UCH_Open_Index].UCH_Name = "";
            UCH_list[UCH_Open_Index].ZR_Left.Clear();
            UCH_list[UCH_Open_Index].ZR_Right.Clear();
            UCH_list[UCH_Open_Index].UCH_Button.Content = "";
            WrapPanel_UCH.Children.RemoveAt(UCH_Open_Index);

            //перестраиваем массив участков
            //var selectedmassive = from p in UCH_list // передаем каждый элемент из people в переменную p
            //                     where p.UCH_Name != "" //фильтрация по критерию
            //                     select p; // выбираем объект в создаваемую коллекцию

            //for (int i = 0; i < UCH_list.Length; i++)
            //{
            //    UCH_list[i].UCH_Name = "";
            //    UCH_list[i].UCH_Button.Content = "";
            //    UCH_list[i].ZR_Left.Clear();
            //    UCH_list[i].ZR_Right.Clear();
            //}
            //for (int i = 0; i < selectedmassive.; i++)
            //{

            //}


            //очищаем поля
            UCH_CountZR_Left.Text = "0";
            UCH_CountZR_Right.Text = "0";
            UCH_Name.Text = "";

            UCH_Button_Visibility(true, false, false);
        }
    }
}
