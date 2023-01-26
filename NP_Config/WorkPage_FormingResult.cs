using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NP_Config
{
    public partial class WorkPage
    {
        //для хранения значений сетевых настроек
        int[] IP_Setting_1 = new int[6];
        int[] IP_Setting_2 = new int[6];
        int[] IP_Setting_3 = new int[6];
        int[] IP_Setting_4 = new int[6];
        int[] IP_Setting_5 = new int[6];
        int[] IP_Setting_6 = new int[6];
        //для хранения информации о номерах внешних NP
        List<int> External_NP = new List<int>();
        //для хранения значений адресов датчиков первого канала (десятичное представление)
        List<int> ZR_Address_Channel1_DEC = new List<int>();
        //для хранения значений адресов датчиков первого канала (шестнадцатиричное представление)
        List<string> ZR_Address_Channel1_HEX = new List<string>();
        //для хранения значений адресов датчиков второго канала (десятичное представление)
        List<int> ZR_Address_Channel2_DEC = new List<int>();
        //для хранения значений адресов датчиков второго канала (шестнадцатиричное представление)
        List<string> ZR_Address_Channel2_HEX = new List<string>();
        //для хранения конфигурации внешних датчиков (шестнадцатиричное представление)
        List<int> External_ZR_ = new List<int>();

        public struct UCH_IndexZR
        {
            public string UCH_Name;
            public List<int> ZR_Left;
            public List<int> ZR_Right;
        }
        public UCH_IndexZR[] UCH_list_index = new UCH_IndexZR[30];  //информацию о индексах датчиков в участках
        public void Write_data()    //записываем данные в массивы
        {
            IP_Setting_1[0] = Convert.ToInt32(TB11.Text);
            IP_Setting_1[1] = Convert.ToInt32(TB12.Text);
            IP_Setting_1[2] = Convert.ToInt32(TB13.Text);
            IP_Setting_1[3] = Convert.ToInt32(TB14.Text);
            IP_Setting_1[4] = Convert.ToInt32(TB15.Text);
            IP_Setting_1[5] = Convert.ToInt32(TB16.Text);

            IP_Setting_2[0] = Convert.ToInt32(TB21.Text);
            IP_Setting_2[1] = Convert.ToInt32(TB22.Text);
            IP_Setting_2[2] = Convert.ToInt32(TB23.Text);
            IP_Setting_2[3] = Convert.ToInt32(TB24.Text);
            IP_Setting_2[4] = Convert.ToInt32(TB25.Text);
            IP_Setting_2[5] = Convert.ToInt32(TB26.Text);

            IP_Setting_3[0] = Convert.ToInt32(TB31.Text);
            IP_Setting_3[1] = Convert.ToInt32(TB32.Text);
            IP_Setting_3[2] = Convert.ToInt32(TB33.Text);
            IP_Setting_3[3] = Convert.ToInt32(TB34.Text);
            IP_Setting_3[4] = Convert.ToInt32(TB35.Text);
            IP_Setting_3[5] = Convert.ToInt32(TB36.Text);

            IP_Setting_4[0] = Convert.ToInt32(TB41.Text);
            IP_Setting_4[1] = Convert.ToInt32(TB42.Text);
            IP_Setting_4[2] = Convert.ToInt32(TB43.Text);
            IP_Setting_4[3] = Convert.ToInt32(TB44.Text);
            IP_Setting_4[4] = Convert.ToInt32(TB45.Text);
            IP_Setting_4[5] = Convert.ToInt32(TB46.Text);

            IP_Setting_5[0] = Convert.ToInt32(TB51.Text);
            IP_Setting_5[1] = Convert.ToInt32(TB52.Text);
            IP_Setting_5[2] = Convert.ToInt32(TB53.Text);
            IP_Setting_5[3] = Convert.ToInt32(TB54.Text);
            IP_Setting_5[4] = Convert.ToInt32(TB55.Text);
            IP_Setting_5[5] = Convert.ToInt32(TB56.Text);

            IP_Setting_6[0] = Convert.ToInt32(TB61.Text);
            IP_Setting_6[1] = Convert.ToInt32(TB62.Text);
            IP_Setting_6[2] = Convert.ToInt32(TB63.Text);
            IP_Setting_6[3] = Convert.ToInt32(TB64.Text);
            IP_Setting_6[4] = Convert.ToInt32(TB65.Text);
            IP_Setting_6[5] = Convert.ToInt32(TB66.Text);

            //адреса датчиков в десятичном формате
            for (int i = 0; i < NP_ZR_channel1.Length; i++)
            {
                if (NP_ZR_channel1[i].NP_ZR_Address.Text != "")
                {
                    ZR_Address_Channel1_DEC.Add(Convert.ToInt32(NP_ZR_channel1[i].NP_ZR_Address.Text));
                }
            }
            for (int i = 0; i < NP_ZR_channel2.Length; i++)
            {
                if (NP_ZR_channel2[i].NP_ZR_Address.Text != "")
                {
                    ZR_Address_Channel2_DEC.Add(Convert.ToInt32(NP_ZR_channel2[i].NP_ZR_Address.Text));
                }
            }
            //адреса датчиков в шестнадцатиричном формате
            for (int i = 0; i < ZR_Address_Channel1_DEC.Count; i++)
            {
                ZR_Address_Channel1_HEX.Add(ZR_Address_Channel1_DEC[i].ToString("X2"));
            }
            for (int i = 0; i < ZR_Address_Channel2_DEC.Count; i++)
            {
                ZR_Address_Channel2_HEX.Add(ZR_Address_Channel2_DEC[i].ToString("X2"));
            }

            //инициализируем поля структуры участков
            for (int i = 0; i < UCH_list_index.Length; i++)
            {
                UCH_list_index[i].ZR_Left = new List<int>();
                UCH_list_index[i].ZR_Right = new List<int>();
            }
            //записываем информацию по индексам датчиков в участках
            for (int a = 0; a < UCH_list_index.Length; a++)
            {
                UCH_list_index[a].UCH_Name = UCH_list[a].UCH_Name;

                //перебор всех датчиков на участке слева
                for (int b = 0; b < UCH_list[a].ZR_Left.Count; b++)
                {
                    int Addres = UCH_list[a].ZR_Left[b].ZR_Address;
                    int NP = UCH_list[a].ZR_Left[b].ZR_NP;
                    int Channel = UCH_list[a].ZR_Left[b].ZR_Channel;
                    //проверка что датчик принадлежит текущему NP
                    if (NP == This_NP_number) //датчик в текущем NP
                    {
                        int index_zr = Search_IndexZR_ThisNP(Addres, Channel);
                        UCH_list_index[a].ZR_Left.Add(index_zr);
                    }
                    else //датчик во внешнем NP
                    {

                    }
                }

                //перебор всех датчиков на участке справа
                for (int b = 0; b < UCH_list[a].ZR_Right.Count; b++)
                {
                    int Addres = UCH_list[a].ZR_Right[b].ZR_Address;
                    int NP = UCH_list[a].ZR_Right[b].ZR_NP;
                    int Channel = UCH_list[a].ZR_Right[b].ZR_Channel;
                    //проверка что датчик принадлежит текущему NP
                    if (NP == This_NP_number) //датчик в текущем NP
                    {
                        int index_zr = Search_IndexZR_ThisNP(Addres, Channel);
                        UCH_list_index[a].ZR_Right.Add(index_zr);
                    }
                    else //датчик во внешнем NP
                    {

                    }
                }
            }
        }
        private int Search_IndexZR_ThisNP(int ZR, int channel)   //определяем индекс датчика в текущем NP
        {
            int result = 0;

            if (channel == 1)
            {
                result = ZR_Address_Channel1_DEC.BinarySearch(ZR) + 1;
            }
            if (channel == 2)
            {
                result = ZR_Address_Channel1_DEC.Count + ZR_Address_Channel2_DEC.BinarySearch(ZR) + 1;
            }
            return result;
        }
        private int Search_IndexZR_ExternalNP(int ZR, int NP, int Channel)   //определяем индекс датчика во внешнем NP
        {
            int result = 0;
            //добавляем NP в список внешних NP, если его там нет
            int Index_External_NP = 0;
            bool Search_External_NP = false;
            for (int i = 0; i < External_NP.Count; i++)
            {
                if (External_NP[i] == NP)
                {
                    Index_External_NP = i;
                    Search_External_NP = true;
                    break;
                }
            }
            //если не нашли - добавляем
            if (Search_External_NP == false)
            {
                External_NP.Add(NP);
                Index_External_NP = External_NP.Count - 1;
            }




            return result;
        }
        public string Str_1()
        {
            string result = "";

            result = string.Format("{0}.{1}.{2}.{3}.{4}.{5}'",TB11.Text,TB12.Text,TB13.Text,TB14.Text,TB15.Text,TB16.Text);
            result = result.PadRight(50, ' ');
            result += "\tIP адрес разъема T2 первого канала\r\n";

            return result;
        }
        public string Str_2()
        {
            string result = "";

            result = string.Format("{0}.{1}.{2}.{3}.{4}.{5}'", TB21.Text, TB22.Text, TB23.Text, TB24.Text, TB25.Text, TB26.Text);
            result = result.PadRight(50, ' ');
            result += "\tIP адрес разъема T2 второго канала\r\n";

            return result;
        }
        public string Str_3()
        {
            string result = "";

            result = string.Format("{0}.{1}.{2}.{3}.{4}.{5}'", TB31.Text, TB32.Text, TB33.Text, TB34.Text, TB35.Text, TB36.Text);
            result = result.PadRight(50, ' ');
            result += "\tIP адрес, UDP порт разъема T1\r\n";

            return result;
        }
        public string Str_4()
        {
            string result = "";

            result = string.Format("{0}.{1}.{2}.{3}.{4}.{5}'", TB41.Text, TB42.Text, TB43.Text, TB44.Text, TB45.Text, TB46.Text);
            result = result.PadRight(50, ' ');
            result += "\tIP адрес, UDP порт разъема T2\r\n";

            return result;
        }
        public string Str_5()
        {
            string result = "";

            result = string.Format("{0}.{1}.{2}.{3}.{4}.{5}'", TB51.Text, TB52.Text, TB53.Text, TB54.Text, TB55.Text, TB56.Text);
            result = result.PadRight(50, ' ');
            result += "\tIP адрес системы ЖАТ, UDP порт разъема T1\r\n";

            return result;
        }
        public string Str_6()
        {
            string result = "";

            result = string.Format("{0}.{1}.{2}.{3}.{4}.{5}'", TB61.Text, TB62.Text, TB63.Text, TB64.Text, TB65.Text, TB66.Text);
            result = result.PadRight(50, ' ');
            result += "\tIP адрес системы ЖАТ, UDP порт разъема T2\r\n";

            return result;
        }
        public string Str_7()
        {
            string result = "";

            result = "0.0'";
            result = result.PadRight(60, ' ');
            result += "\tСкорость обмена данными 1 и 2 канала по RS-485\r\n";

            return result;
        }
        public string NP_InputZR_STR()
        {
            string result = "";
            int CountZR_Channel1 = Convert.ToInt32(NP_Channel1_count.Text);
            int CountZR_Channel2 = Convert.ToInt32(NP_Channel2_count.Text);

            result = CountZR_Channel1.ToString("X2") + " " + CountZR_Channel2.ToString("X2") + " FF" + " 20";

            for (int i = 0; i < CountZR_Channel1; i++)
            {
                // добавляем адреса датчиков первого канала
                int ZR_Address = Convert.ToInt32(NP_ZR_channel1[i].NP_ZR_Address.Text);
                result += " " + ZR_Address.ToString("X2");                           
            }
            for (int i = 0; i < CountZR_Channel2; i++)
            {
                // добавляем адреса датчиков первого канала
                int ZR_Address = Convert.ToInt32(NP_ZR_channel2[i].NP_ZR_Address.Text);
                result += " " + ZR_Address.ToString("X2");
            }
            result += "'";
            result = result.PadRight(50, ' ');
            result += "\tКонфигурация датчиков нпорта \r\n";
            return result;
        }
    }
}
