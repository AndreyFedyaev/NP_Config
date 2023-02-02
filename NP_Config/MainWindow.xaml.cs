using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
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
using static NP_Config.WorkPage;

namespace NP_Config
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer1 = new System.Windows.Threading.DispatcherTimer();

        struct Menu_Struct
        {
            public Button Menu_Button;
        }
        struct Pages_Struct
        {
            public WorkPage WP;
        }
        struct NP_ZR_List   //перечень датчиков во всех NP
        {
            public List<int> channel_1;
            public List<int> channel_2;
        }

        Menu_Struct[] menu_struct = new Menu_Struct[16];
        Pages_Struct[] page_struct = new Pages_Struct[16];
        NP_ZR_List[] ZR_List = new NP_ZR_List[16];

        private int Menu_Button_Count = 0;  //количество добавленных кнопок
        private int Menu_Button_IsActive = 0; //активная (нажатая) кнопка в текущий момент

        public MainWindow()
        {
            InitializeComponent();
            InitializeButtonMenu(); //инициализируем кнопки меню
            Initialize_ZR_List();

            timer1.Tick += new EventHandler(timer_1);
            timer1.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer1.Start();


        }

        private void Initialize_ZR_List()
        {
            for (int i = 0; i < ZR_List.Length; i++)
            {
                ZR_List[i].channel_1 = new List<int>();
                ZR_List[i].channel_2 = new List<int>();
            }
        }

        private void timer_1(object sender, EventArgs e)
        {

            ZR_List_Write();

            //количество NP
            All_NP_txt.Text = string.Format("Добавлено модулей NP: {0}", Menu_Button_Count);
            //количество датчиков
            int ZR_Count_txt = 0;
            for (int i = 0; i < Menu_Button_Count; i++)
            {
                ZR_Count_txt += ZR_List[i].channel_1.Count;
                ZR_Count_txt += ZR_List[i].channel_2.Count;
            }
            All_ZR_txt.Text = string.Format("Добавлено датчиков: {0}", ZR_Count_txt);
            //количество участков
            int UCH_Count_txt = 0;
            for (int i = 0; i < Menu_Button_Count; i++)
            {
                UCH_Count_txt += page_struct[i].WP.UCH_Count;
            }
            All_UCH_txt.Text = string.Format("Добавлено участков: {0}", UCH_Count_txt);
        }

        private void ZR_List_Write()
        {
            for (int i = 0; i < ZR_List.Length; i++)
            {
                ZR_List[i].channel_1.Clear();
                ZR_List[i].channel_2.Clear();
            }

            for (int a = 0; a < Menu_Button_Count; a++)
            {
                page_struct[a].WP.All_Count_NP = Menu_Button_Count;

                //считываем адреса датчиков всех NP первого канала
                for (int b = 0; b < page_struct[a].WP.NP_ZR_channel1.Length; b++)
                {
                    if (page_struct[a].WP.NP_ZR_channel1[b].NP_ZR_Address.Text != "")
                    {
                        ZR_List[a].channel_1.Add(Convert.ToInt32(page_struct[a].WP.NP_ZR_channel1[b].NP_ZR_Address.Text));
                    }
                }
                //считываем адреса датчиков всех NP второго канала
                for (int b = 0; b < page_struct[a].WP.NP_ZR_channel2.Length; b++)
                {
                    if (page_struct[a].WP.NP_ZR_channel2[b].NP_ZR_Address.Text != "")
                    {
                        ZR_List[a].channel_2.Add(Convert.ToInt32(page_struct[a].WP.NP_ZR_channel2[b].NP_ZR_Address.Text));
                    }
                }
            }

            for (int a = 0; a < Menu_Button_Count; a++)
            {
                for (int b = 0; b < page_struct[a].WP.ZR_List.Length; b++)
                {
                    page_struct[a].WP.ZR_List[b].channel_1 = ZR_List[b].channel_1;
                    page_struct[a].WP.ZR_List[b].channel_2 = ZR_List[b].channel_2;

                    if (page_struct[b].WP != null)
                    {
                        page_struct[a].WP.ZR_List[b].channel_1_Err = page_struct[b].WP.NP_Channel1_Errors;
                        page_struct[a].WP.ZR_List[b].channel_2_Err = page_struct[b].WP.NP_Channel2_Errors;
                    }
                }
                page_struct[a].WP.This_NP_number = a + 1;
            }
        }

        private void InitializeButtonMenu ()    //инициализируем кнопки меню
        {
            //Создаем динамически кнопки
            for (int index = 0; index < menu_struct.Length; index++)
            {
                menu_struct[index].Menu_Button = new Button();
                menu_struct[index].Menu_Button.Name = "MenuButtonIndex_" + index.ToString();
                menu_struct[index].Menu_Button.Click += new RoutedEventHandler(NP_Click);
                menu_struct[index].Menu_Button.Content = "NP" + (index + 1).ToString();
                menu_struct[index].Menu_Button.Style = (Style)menu_struct[index].Menu_Button.FindResource("ButtonStyle_Menu1");
            }

            Menu_Buttons_Add(); //добавляем первую (стартовую) кнопку
            Page_Display(0);    //делаем первую кнопку нажатой
        }
        private void NP_Click(object sender, RoutedEventArgs e)
        {
            int ButtonIndex = 0; //индекс нажатой кнопки меню

            //определяем индекс нажатой кнопки (написано немного криво)
            try
            {
                string ButtonName = ((System.Windows.FrameworkElement)sender).Name;
                string[] spl = ButtonName.Split('_');
                ButtonIndex = Convert.ToInt32(spl[1]);
            }
            catch { }

            Page_Display(ButtonIndex);

            //запускаем таймер нажатой страницы, остальные выключаем
            Page_Timer();
        }

        private void Page_Timer()
        {
            //запускаем таймер нажатой страницы, остальные выключаем
            for (int i = 0; i < Menu_Button_Count; i++)
            {
                if (i == (Menu_Button_IsActive - 1)) page_struct[i].WP.Timer_Start();
                else page_struct[i].WP.Timer_Stop();
            }
        }

        private void Page_Display(int ButtonIndex)
        {
            Menu_Button_IsActive = ButtonIndex + 1;

            //настройка стилей кнопок меню
            for (int index = 0; index < menu_struct.Length; index++)
            {
                menu_struct[index].Menu_Button.Style = (Style)menu_struct[index].Menu_Button.FindResource("ButtonStyle_Menu1");
            }
            menu_struct[ButtonIndex].Menu_Button.Style = (Style)menu_struct[ButtonIndex].Menu_Button.FindResource("ButtonStyle_Menu2");

            //отображение станицы
            WorkMainPage_Frame.NavigationService.Navigate(page_struct[ButtonIndex].WP);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();
        }

        private void Window_Minimizate_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_All_Size_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void Window_Close_Click(object sender, RoutedEventArgs e)
        {
            //закрываем основное окно
            this.Close();
            
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Menu_Up_Click(object sender, RoutedEventArgs e)
        {
            Menu_Buttons_Remove();
            //запускаем таймер нажатой страницы, остальные выключаем
            Page_Timer();
        }

        private void Menu_Down_Click(object sender, RoutedEventArgs e)
        {
            Menu_Buttons_Add();
            //запускаем таймер нажатой страницы, остальные выключаем
            Page_Timer();
        }

        private void Menu_Buttons_Remove()
        {
            if (Menu_Button_Count > 1)
            {
                //убираем кнопки "добавить" и "удалить"
                StackPanelMenu.Children.Remove(Menu_Add_Delete_Button);
                //добавляем следующую кнопку при запуске
                StackPanelMenu.Children.Remove(menu_struct[Menu_Button_Count - 1].Menu_Button);
                //добавляем кнопки "добавить" и "удалить"
                StackPanelMenu.Children.Add(Menu_Add_Delete_Button);

                page_struct[Menu_Button_Count - 1].WP = null;

                if (Menu_Button_IsActive == Menu_Button_Count)  
                {
                    menu_struct[Menu_Button_Count - 1].Menu_Button.Style = (Style)menu_struct[Menu_Button_Count - 1].Menu_Button.FindResource("ButtonStyle_Menu1");
                    menu_struct[Menu_Button_Count - 2].Menu_Button.Style = (Style)menu_struct[Menu_Button_Count - 2].Menu_Button.FindResource("ButtonStyle_Menu2");
                    
                    //отображение станицы
                    WorkMainPage_Frame.NavigationService.Navigate(page_struct[Menu_Button_Count - 2].WP);
                    WorkMainPage_Frame.NavigationService.RemoveBackEntry();

                    Menu_Button_IsActive--;
                }


                Menu_Button_Count--;
            }
            Menu_Visibility();
        }
        private void Menu_Buttons_Add()
        {
            NP_Add();
        }
        private void NP_Add()
        {
            if (Menu_Button_Count < menu_struct.Length)
            {
                //убираем кнопки "добавить" и "удалить"
                StackPanelMenu.Children.Remove(Menu_Add_Delete_Button);
                //добавляем следующую кнопку при запуске
                StackPanelMenu.Children.Add(menu_struct[Menu_Button_Count].Menu_Button);
                //добавляем кнопки "добавить" и "удалить"
                StackPanelMenu.Children.Add(Menu_Add_Delete_Button);

                //инициализируем страницу для добавленной кнопки
                page_struct[Menu_Button_Count].WP = new WorkPage();


                page_struct[0].WP.Timer_Start();

                Menu_Button_Count++;
            }
            Menu_Visibility();
        }

        private void Menu_Visibility()
        {
            if (Menu_Button_Count == 1) Menu_Up.Visibility = Visibility.Hidden;
            else Menu_Up.Visibility = Visibility.Visible;

            if (Menu_Button_Count == 16) Menu_Down.Visibility = Visibility.Hidden;
            else Menu_Down.Visibility = Visibility.Visible;
        }

        private void Chenging_Theme_Click(object sender, RoutedEventArgs e)
        {
            if (Chenging_Theme.Content.ToString() == "Тёмная тема")
            {
                var uri = new Uri(@"Dictionary_DarkStyle.xaml", UriKind.Relative);
                ResourceDictionary ResourseDict1 = Application.LoadComponent(uri) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(ResourseDict1);
                Style_Setting();
            }
            if (Chenging_Theme.Content.ToString() == "Светлая тема")
            {
                var uri2 = new Uri(@"Dictionary_WhiteStyle.xaml", UriKind.Relative);
                ResourceDictionary ResourseDict2 = Application.LoadComponent(uri2) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(ResourseDict2);
                Style_Setting();
            }

            if (Chenging_Theme.Content.ToString() == "Тёмная тема") Chenging_Theme.Content = "Светлая тема";
            else Chenging_Theme.Content = "Тёмная тема";
        }

        private void Style_Setting()
        {
            for (int a = 0; a < page_struct.Length; a++)
            {
                if (page_struct[a].WP != null)
                {
                    for (int b = 0; b < page_struct[a].WP.NP_ZR_channel1.Length; b++)
                    {
                        page_struct[a].WP.NP_ZR_channel1[b].NP_ZR_Address.Style = (Style)page_struct[a].WP.NP_ZR_channel1[b].NP_ZR_Address.FindResource("TextBoxStyle_Type1");
                    }
                    for (int b = 0; b < page_struct[a].WP.NP_ZR_channel2.Length; b++)
                    {
                        page_struct[a].WP.NP_ZR_channel2[b].NP_ZR_Address.Style = (Style)page_struct[a].WP.NP_ZR_channel2[b].NP_ZR_Address.FindResource("TextBoxStyle_Type1");
                    }
                    for (int b = 0; b < page_struct[a].WP.UCH_list.Length; b++)
                    {
                        page_struct[a].WP.UCH_list[b].UCH_Button.Style = (Style)page_struct[a].WP.UCH_list[b].UCH_Button.FindResource("ButtonStyle_UCH_Name");
                    }
                }
            }
        }
        private void Warning_Dialog_Show(string Warning_Text_str1, string Warning_Text_str2, string Warning_Text_str3)   //отображение диалогового окна
        {
            Warning warning = new Warning();
            warning.Warning_Text_str1.Text = Warning_Text_str1;
            warning.Warning_Text_str2.Text = Warning_Text_str2;
            warning.Warning_Text_str3.Text = Warning_Text_str3;
            warning.ShowDialog();
        }
        private void View_button_Click(object sender, RoutedEventArgs e)
        {
            bool Search_Result = IP_Setting_Search_Errors();

            if (Search_Result == false)
            {
                string result = page_struct[Menu_Button_IsActive - 1].WP.Forming_Result();

                //Создаем файл++++
                StreamWriter TEXT = new StreamWriter("ViewingConfig.ini", false, System.Text.Encoding.UTF8, 512);
                TEXT.WriteLine(result);
                TEXT.Close();

                //Открываем файл
                if (File.Exists("ViewingConfig.ini"))       //проверяем существует ли заданный файл
                {
                    Process.Start("ViewingConfig.ini");     // если да, то открываем его
                }
            }
        }

        private bool IP_Setting_Search_Errors()
        {
            //выполняем проверки заполнения всех полей сетевых настроек
            for (int i = 0; i < Menu_Button_Count; i++)
            {
                page_struct[i].WP.IP_Settings_Search_Errors();
            }
            //анализируем результаты
            for (int i = 0; i < Menu_Button_Count; i++)
            {
                if (page_struct[i].WP.IP_Setting_1_Errors)
                {
                    string NP_Name = "NP" + (i + 1);
                    Warning_Dialog_Show(NP_Name, "IP адрес разъема Т2 первого канала", "Заполнено с ошибками");
                    return true;
                }
                if (page_struct[i].WP.IP_Setting_2_Errors)
                {
                    string NP_Name = "NP" + (i + 1);
                    Warning_Dialog_Show(NP_Name, "IP адрес разъема Т2 второго канала", "Заполнено с ошибками");
                    return true;
                }
                if (page_struct[i].WP.IP_Setting_3_Errors)
                {
                    string NP_Name = "NP" + (i + 1);
                    Warning_Dialog_Show(NP_Name, "IP адрес, UDP порт разъема T1", "Заполнено с ошибками");
                    return true;
                }
                if (page_struct[i].WP.IP_Setting_4_Errors)
                {
                    string NP_Name = "NP" + (i + 1);
                    Warning_Dialog_Show(NP_Name, "IP адрес, UDP порт разъема T2", "Заполнено с ошибками");
                    return true;
                }
                if (page_struct[i].WP.IP_Setting_5_Errors)
                {
                    string NP_Name = "NP" + (i + 1);
                    Warning_Dialog_Show(NP_Name, "IP адрес контроллера ЖАТ разъема Т1", "Заполнено с ошибками");
                    return true;
                }
                if (page_struct[i].WP.IP_Setting_6_Errors)
                {
                    string NP_Name = "NP" + (i + 1);
                    Warning_Dialog_Show(NP_Name, "IP адрес контроллера ЖАТ разъема Т2", "Заполнено с ошибками");
                    return true;
                }
            }
            //записываем информацию необходимых сетевых настроек всех NP для формирования записи о внешних NP
            for (int i = 0; i < Menu_Button_Count; i++)
            {
                for (int a = 0; a < page_struct[i].WP.IP_All_Np_List.Length; a++)
                {
                    if (page_struct[a].WP != null)
                    {
                        page_struct[i].WP.IP_All_Np_List[a].TB34 = Convert.ToInt32(page_struct[a].WP.TB34.Text);
                        page_struct[i].WP.IP_All_Np_List[a].TB44 = Convert.ToInt32(page_struct[a].WP.TB44.Text);
                        page_struct[i].WP.IP_All_Np_List[a].TB56 = Convert.ToInt32(page_struct[a].WP.TB56.Text);
                        page_struct[i].WP.IP_All_Np_List[a].TB66 = Convert.ToInt32(page_struct[a].WP.TB66.Text);
                    }
                }
            }
            return false;
        }

        private void Open_button_Click(object sender, RoutedEventArgs e)
        {
            Alarm alarm = new Alarm();
            string alarm_text = "Перед открытием все поля будут очищены!";
            alarm.Alarm_Show(alarm_text);
            if (alarm.Alarm_Result)
            {
                //очищаем все поля
                Clear_All_Page();

                //открываем конфигурационные файлы
                Open_Configs();
            }
        }
        private void Open_Error_Show_DialogWindow(string File_Name, int Number_Errror)
        {
            Warning_Dialog_Show(File_Name, "Файл был открыт с ошибками", "Номер ошибки - " + Number_Errror);
        }
        private void Open_Configs()
        {
            timer1.Stop();

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = "Config Files|*.ini";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                string[] result = openFileDialog1.FileNames;

                string[] FileName = new string[result.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    FileName[i] = System.IO.Path.GetFileNameWithoutExtension(result[i]);
                }

                //создаем нужное количество NP
                for (int i = 1; i < result.Length; i++)
                {
                    NP_Add();
                }
                //перебираем все открытые файлы по порядку и заполняем поля IP и адресов датчиков
                for (int i = 0; i < result.Length; i++)
                {
                    StreamReader sr = new StreamReader(result[i], System.Text.Encoding.Default);

                    try
                    {
                        string[] str1;
                        str1 = sr.ReadLine().Split('\'');
                        str1 = str1[0].Split('.');
                        page_struct[i].WP.TB11.Text = str1[0];
                        page_struct[i].WP.TB12.Text = str1[1];
                        page_struct[i].WP.TB13.Text = str1[2];
                        page_struct[i].WP.TB14.Text = str1[3];
                        page_struct[i].WP.TB15.Text = str1[4];
                        page_struct[i].WP.TB16.Text = str1[5];
                    }
                    catch 
                    {
                        Open_Error_Show_DialogWindow(FileName[i], 1);
                    }

                    try
                    {
                        string[] str2;
                        str2 = sr.ReadLine().Split('\'');
                        str2 = str2[0].Split('.');
                        page_struct[i].WP.TB21.Text = str2[0];
                        page_struct[i].WP.TB22.Text = str2[1];
                        page_struct[i].WP.TB23.Text = str2[2];
                        page_struct[i].WP.TB24.Text = str2[3];
                        page_struct[i].WP.TB25.Text = str2[4];
                        page_struct[i].WP.TB26.Text = str2[5];
                    }
                    catch 
                    {
                        Open_Error_Show_DialogWindow(FileName[i], 2);
                    }

                    try
                    {
                        char[] delimiterChars = { '.', ':' };
                        string[] str3;
                        str3 = sr.ReadLine().Split('\'');
                        str3 = str3[0].Split(delimiterChars);
                        page_struct[i].WP.TB31.Text = str3[0];
                        page_struct[i].WP.TB32.Text = str3[1];
                        page_struct[i].WP.TB33.Text = str3[2];
                        page_struct[i].WP.TB34.Text = str3[3];
                        page_struct[i].WP.TB35.Text = str3[4];
                        page_struct[i].WP.TB36.Text = str3[5];
                    }
                    catch 
                    {
                        Open_Error_Show_DialogWindow(FileName[i], 3);
                    }

                    try
                    {
                        char[] delimiterChars = { '.', ':' };
                        string[] str4;
                        str4 = sr.ReadLine().Split('\'');
                        str4 = str4[0].Split(delimiterChars);
                        page_struct[i].WP.TB41.Text = str4[0];
                        page_struct[i].WP.TB42.Text = str4[1];
                        page_struct[i].WP.TB43.Text = str4[2];
                        page_struct[i].WP.TB44.Text = str4[3];
                        page_struct[i].WP.TB45.Text = str4[4];
                        page_struct[i].WP.TB46.Text = str4[5];
                    }
                    catch 
                    {
                        Open_Error_Show_DialogWindow(FileName[i], 4);
                    }

                    try
                    {
                        char[] delimiterChars = { '.', ':' };
                        string[] str5;
                        str5 = sr.ReadLine().Split('\'');
                        str5 = str5[0].Split(delimiterChars);
                        page_struct[i].WP.TB51.Text = str5[0];
                        page_struct[i].WP.TB52.Text = str5[1];
                        page_struct[i].WP.TB53.Text = str5[2];
                        page_struct[i].WP.TB54.Text = str5[3];
                        page_struct[i].WP.TB55.Text = str5[4];
                        page_struct[i].WP.TB56.Text = str5[5];
                    }
                    catch 
                    {
                        Open_Error_Show_DialogWindow(FileName[i], 5);
                    }

                    try
                    {
                        char[] delimiterChars = { '.', ':' };
                        string[] str6;
                        str6 = sr.ReadLine().Split('\'');
                        str6 = str6[0].Split(delimiterChars);
                        page_struct[i].WP.TB61.Text = str6[0];
                        page_struct[i].WP.TB62.Text = str6[1];
                        page_struct[i].WP.TB63.Text = str6[2];
                        page_struct[i].WP.TB64.Text = str6[3];
                        page_struct[i].WP.TB65.Text = str6[4];
                        page_struct[i].WP.TB66.Text = str6[5];
                    }
                    catch 
                    {
                        Open_Error_Show_DialogWindow(FileName[i], 6);
                    }

                    //прокускаем строку "Скорость обмена данными 1 и 2 канала по RS-485"
                    sr.ReadLine();

                    //пропускаем строки "Конфигурация внешнего нпорта с индексом 0-7"
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();

                    //заполняем датчики NP
                    try
                    {
                        string[] str;
                        str = sr.ReadLine().Split('\'');
                        str = str[0].Split(' ');
                        //первый канал
                        int Channel1_ZR_Count = Int32.Parse(str[0], System.Globalization.NumberStyles.HexNumber);
                        page_struct[i].WP.NP_Channel1_count.Text = Channel1_ZR_Count.ToString();
                        for (int a = 0; a < Channel1_ZR_Count; a++)
                        {
                            page_struct[i].WP.NP_ZR_channel1[a].NP_ZR_Address.Text = (Int32.Parse(str[a + 4], System.Globalization.NumberStyles.HexNumber)).ToString();
                        }

                        //второй канал
                        int Channel2_ZR_Count = Int32.Parse(str[1], System.Globalization.NumberStyles.HexNumber);
                        page_struct[i].WP.NP_Channel2_count.Text = Channel2_ZR_Count.ToString();
                        for (int a = 0; a < Channel2_ZR_Count; a++)
                        {
                            page_struct[i].WP.NP_ZR_channel2[a].NP_ZR_Address.Text = (Int32.Parse(str[a + 4 + Channel1_ZR_Count], System.Globalization.NumberStyles.HexNumber)).ToString();
                        }
                    }
                    catch 
                    {
                        Open_Error_Show_DialogWindow(FileName[i], 7);
                    }
                }

                //Заполняем ZR_List
                ZR_List_Write();

                //перебираем все файлы по порядку и заполняем список внешних NP
                for (int i = 0; i < result.Length; i++)
                {
                    StreamReader sr = new StreamReader(result[i], System.Text.Encoding.Default);

                    //пропускаем 7 строк
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();

                    try
                    {
                        for (int a = 0; a < 8; a++) //перебираем список внешних NP
                        {
                            string[] str1;
                            str1 = sr.ReadLine().Split('\'');
                            str1 = str1[0].Split(':');

                            int External_NP = 0;

                            if (str1[0] == "0")
                            {
                                page_struct[i].WP.External_NP.Add(0);
                            }
                            else
                            {
                                for (int b = 0; b < Menu_Button_Count; b++)
                                {
                                    if (page_struct[b].WP.TB34.Text == str1[0])
                                    {
                                        External_NP = b + 1;
                                        page_struct[i].WP.External_NP.Add(External_NP);
                                        break;
                                    }
                                }
                            }
                        }

                        for (int a = page_struct[i].WP.External_NP.Count - 1; a >= 0; a--)
                        {
                            if (page_struct[i].WP.External_NP[a] == 0)
                            {
                                page_struct[i].WP.External_NP.RemoveAt(a);
                            }
                            else break;
                        }
                    }
                    catch 
                    {
                        Open_Error_Show_DialogWindow(FileName[i], 8);
                    }
                }
                //перебираем все файлы по порядку и заполняем список внешних датчиков
                for (int i = 0; i < result.Length; i++)
                {
                    StreamReader sr = new StreamReader(result[i], System.Text.Encoding.Default);

                    //пропускаем 16 строк
                    for (int a = 0; a < 16; a++)
                    {
                        sr.ReadLine();
                    }

                    try
                    {
                        string[] str1;
                        str1 = sr.ReadLine().Split('\'');
                        str1 = str1[0].Split(' ');

                        for (int b = 0; b < str1.Length; b++)
                        {
                            if (str1[b] != "00")
                            {
                                int index_External_NP = 0;
                                int index_External_ZR = 0;

                                index_External_NP = Int32.Parse(str1[b], System.Globalization.NumberStyles.HexNumber) >> 5;
                                index_External_ZR = (Int32.Parse(str1[b], System.Globalization.NumberStyles.HexNumber) & 31);

                                page_struct[i].WP.Ext_ZR[b].Result_hex = str1[b];

                                int Number_NP = page_struct[i].WP.External_NP[index_External_NP];
                                page_struct[i].WP.Ext_ZR[b].NP = Number_NP;

                                int channel1_count = page_struct[Number_NP - 1].WP.ZR_Address_Channel1_DEC.Count;
                                if (index_External_ZR <= channel1_count)
                                {
                                    page_struct[i].WP.Ext_ZR[b].Address = page_struct[Number_NP - 1].WP.ZR_Address_Channel1_DEC[index_External_ZR - 1];
                                    page_struct[i].WP.Ext_ZR[b].Chanel = 1;
                                }
                                else
                                {
                                    page_struct[i].WP.Ext_ZR[b].Address = page_struct[Number_NP - 1].WP.ZR_Address_Channel2_DEC[index_External_ZR - channel1_count - 1];
                                    page_struct[i].WP.Ext_ZR[b].Chanel = 2;
                                }
                            }
                        }
                    }
                    catch
                    {
                        Open_Error_Show_DialogWindow(FileName[i], 9);
                    }
                }
                //перебираем все файлы по порядку и заполняем список участков
                for (int i = 0; i < result.Length; i++)
                {
                    StreamReader sr = new StreamReader(result[i], System.Text.Encoding.Default);

                    //пропускаем 17 строк
                    for (int a = 0; a < 17; a++)
                    {
                        sr.ReadLine();
                    }

                    //определяем количество участков
                    int UCH_Count_read = 0;
                    try
                    {
                        string[] str;
                        str = sr.ReadLine().Split('\'');
                        UCH_Count_read = Int32.Parse(str[0], System.Globalization.NumberStyles.HexNumber);
                    }
                    catch 
                    {
                        Open_Error_Show_DialogWindow(FileName[i], 10);
                    }

                    //перебираем участки
                    try
                    {
                        for (int a = 0; a < UCH_Count_read; a++)
                        {
                            string[] str1;
                            string[] str2;
                            str1 = sr.ReadLine().Split('\'');
                            str2 = str1[0].Split(' ');

                            //определяем название участка
                            string UCH_Name = "";
                            UCH_Name = str1[1].Replace("Конфигурирование участка","");
                            UCH_Name = UCH_Name.Trim();

                            //перебираем все датчики участка и заполняем массивы
                            for (int b = 2; b < str2.Length; b++)
                            {
                                //определяем направление счета датчика
                                bool left = false;      //датчик слева
                                bool right = false;     //датчик справа
                                if ((Int32.Parse(str2[b], System.Globalization.NumberStyles.HexNumber) >> 7) == 0) left = true;
                                else right = true;

                                //определяем индекс датчика 
                                int zr_index = (Int32.Parse(str2[b], System.Globalization.NumberStyles.HexNumber) & 127);

                                //определяем Address, NP, Channel
                                int Address = 0;
                                int NP = 0;
                                int Channel = 0;

                                if (zr_index < 21)
                                {
                                    if (zr_index <= page_struct[i].WP.ZR_Address_Channel1_DEC.Count)
                                    {
                                        Address = page_struct[i].WP.ZR_Address_Channel1_DEC[zr_index - 1];
                                        Channel = 1;
                                    }
                                    else
                                    {
                                        Address = page_struct[i].WP.ZR_Address_Channel2_DEC[zr_index - page_struct[i].WP.ZR_Address_Channel1_DEC.Count - 1];
                                        Channel = 2;
                                    }
                                    NP = i + 1;
                                }
                                else
                                {
                                    for (int c = 0; c < page_struct[i].WP.Ext_ZR.Length; c++)
                                    {
                                        if (page_struct[i].WP.Ext_ZR[c].Index == zr_index)
                                        {
                                            Address = page_struct[i].WP.Ext_ZR[c].Address;
                                            NP = page_struct[i].WP.Ext_ZR[c].NP;
                                            Channel = page_struct[i].WP.Ext_ZR[c].Chanel;
                                        }
                                    }
                                }

                                //добавляем датчик в участок
                                if (left)
                                {
                                    page_struct[i].WP.UCH_list[page_struct[i].WP.UCH_Count].ZR_Left.Add(new UCH_ZR
                                    {
                                        ZR_Address = Address,
                                        ZR_NP = NP,
                                        ZR_Channel = Channel
                                    });
                                }
                                if (right)
                                {
                                    page_struct[i].WP.UCH_list[page_struct[i].WP.UCH_Count].ZR_Right.Add(new UCH_ZR
                                    {
                                        ZR_Address = Address,
                                        ZR_NP = NP,
                                        ZR_Channel = Channel
                                    });
                                }
                            }
                            //добавляем имя участока
                            page_struct[i].WP.UCH_list[page_struct[i].WP.UCH_Count].UCH_Name = UCH_Name;


                            page_struct[i].WP.UCH_list[page_struct[i].WP.UCH_Count].UCH_Button.Content = UCH_Name;
                            page_struct[i].WP.WrapPanel_UCH.Children.Add(page_struct[i].WP.UCH_list[page_struct[i].WP.UCH_Count].UCH_Button);

                            page_struct[i].WP.UCH_Count++;
                        }
                        
                    }
                    catch 
                    {
                        Open_Error_Show_DialogWindow(FileName[i], 11);
                    }
                    
                }
            }

            //после добавление всех данных сбрасываем список внешних NP, для их перестроения на случай если были пропуски или ошибки
            for (int i = 0; i < Menu_Button_Count; i++)
            {
                page_struct[i].WP.External_NP.Clear();
            }
            //определяем индексы датчиков участков и перестраиваем список внешних NP
            for (int i = 0; i < Menu_Button_Count; i++)
            {
                page_struct[i].WP.Write_UCH_data();
            }

            timer1.Start(); 
        }

        private void Clear_page_Button_Click(object sender, RoutedEventArgs e)
        {
            Alarm alarm = new Alarm();
            string alarm_text = string.Format("Все данные NP{0} будут удалены!", Menu_Button_IsActive);
            alarm.Alarm_Show(alarm_text);
            if (alarm.Alarm_Result)
            {
                Clear_One_Page(Menu_Button_IsActive - 1);
            }
        }

        private void Clear_One_Page(int Page_Index)
        {
            page_struct[Page_Index].WP = null;
            page_struct[Page_Index].WP = new WorkPage();

            WorkMainPage_Frame.NavigationService.Navigate(page_struct[Page_Index].WP);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();

            Page_Timer();
        }
        private void Clear_All_Page()
        {
            timer1.Stop();

            //убираем кнопки "добавить" и "удалить"
            StackPanelMenu.Children.Remove(Menu_Add_Delete_Button);

            //удаляем все NP кроме первого
            for (int i = Menu_Button_Count - 1; i > 0; i--)
            {
                page_struct[i].WP = null;
                StackPanelMenu.Children.Remove(menu_struct[i].Menu_Button);

                menu_struct[i].Menu_Button.Style = (Style)menu_struct[Menu_Button_Count - 1].Menu_Button.FindResource("ButtonStyle_Menu1");

                Menu_Button_Count--;
            }
            //добавляем кнопки "добавить" и "удалить"
            StackPanelMenu.Children.Add(Menu_Add_Delete_Button);
            Menu_Visibility();

            //делаем активным первый NP
            Menu_Button_IsActive = 1;
            menu_struct[0].Menu_Button.Style = (Style)menu_struct[0].Menu_Button.FindResource("ButtonStyle_Menu2");

            //очищаем первый NP
            page_struct[0].WP = null;
            page_struct[0].WP = new WorkPage();

            //отображаем первый NP
            WorkMainPage_Frame.NavigationService.Navigate(page_struct[0].WP);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();

            //запускаем таймер нажатой страницы, остальные выключаем
            Page_Timer();

            timer1.Start();
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            bool Search_Result = IP_Setting_Search_Errors();

            if (Search_Result == false)
            {
                string result = page_struct[Menu_Button_IsActive - 1].WP.Forming_Result();

                Stream myStream;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.FileName = "Config";
                saveFileDialog1.Filter = "txt files (*.ini)|*.ini";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == true)
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        StreamWriter TEXT = new StreamWriter(myStream, System.Text.Encoding.UTF8, 512);
                        TEXT.Write(result);
                        TEXT.Close();
                    }
                }

            }
        }

        private void Save_all_button_Click(object sender, RoutedEventArgs e)
        {
            bool Search_Result = IP_Setting_Search_Errors();

            if (Search_Result == false)
            {
                for (int i = 0; i < Menu_Button_Count; i++)
                {
                    string result = page_struct[i].WP.Forming_Result();

                    Stream mySaveStream;
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.RestoreDirectory = true;
                    saveFileDialog1.Filter = "txt files (*.ini)|*.ini";
                    saveFileDialog1.FilterIndex = 2;
                    saveFileDialog1.FileName = "NP" + (i + 1) + "_Config";

                    if (saveFileDialog1.ShowDialog() == true)
                    {
                        if ((mySaveStream = saveFileDialog1.OpenFile()) != null)
                        {
                            StreamWriter TEXT = new StreamWriter(mySaveStream, System.Text.Encoding.UTF8, 512);
                            TEXT.Write(result);
                            TEXT.Close();
                        }
                    }
                    else return;
                }
            }
        }
    }
}
