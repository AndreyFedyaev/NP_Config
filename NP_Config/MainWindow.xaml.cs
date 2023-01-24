using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                }
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
            for (int i = 0; i < Menu_Button_Count; i++)
            {
                if (i == ButtonIndex) page_struct[i].WP.Timer_Start();
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
        }

        private void Menu_Down_Click(object sender, RoutedEventArgs e)
        {
            Menu_Buttons_Add();
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

    }
}
