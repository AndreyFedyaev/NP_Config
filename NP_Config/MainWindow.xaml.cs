﻿using System;
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
        struct Menu_Struct
        {
            public Button Menu_Button;
        }
        struct Pages_Struct
        {
            public WorkPage WP;
        }

        Menu_Struct[] menu_struct = new Menu_Struct[16];
        Pages_Struct[] page_struct = new Pages_Struct[16];

        private int Menu_Button_Count = 1;

        public MainWindow()
        {
            InitializeComponent();

            InitializeButtonMenu(); //инициализируем кнопки меню
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

            //убираем кнопки "добавить" и "удалить"
            StackPanelMenu.Children.Remove(Menu_Add_Delete_Button);
            //добавляем первую кнопку при запуске
            StackPanelMenu.Children.Add(menu_struct[0].Menu_Button);
            //добавляем кнопки "добавить" и "удалить"
            StackPanelMenu.Children.Add(Menu_Add_Delete_Button);

            //инициализируем страницу для первой кнопки
            page_struct[0].WP = new WorkPage();

            Menu_Visibility();
        }
        private void NP_Click(object sender, RoutedEventArgs e)
        {
            int ButtonIndex = 0; //индекс нажатой кнопки меню

            //определяем индекс нажатой кнопки (написано через жопу, ну что поделать)
            try
            {
                string ButtonName = ((System.Windows.FrameworkElement)sender).Name;
                string[] spl = ButtonName.Split('_');
                ButtonIndex = Convert.ToInt32(spl[1]);
            }
            catch { }

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
            if (Menu_Button_Count > 1)
            {
                //убираем кнопки "добавить" и "удалить"
                StackPanelMenu.Children.Remove(Menu_Add_Delete_Button);
                //добавляем следующую кнопку при запуске
                StackPanelMenu.Children.Remove(menu_struct[Menu_Button_Count - 1].Menu_Button);
                //добавляем кнопки "добавить" и "удалить"
                StackPanelMenu.Children.Add(Menu_Add_Delete_Button);

                page_struct[Menu_Button_Count - 1].WP = null;

                Menu_Button_Count--;
            }
            Menu_Visibility();
        }

        private void Menu_Down_Click(object sender, RoutedEventArgs e)
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