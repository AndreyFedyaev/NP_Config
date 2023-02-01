using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using static NP_Config.WorkPage;

namespace NP_Config
{
    public partial class WorkPage
    {
        //для хранения информации о номерах внешних NP
        public List<int> External_NP = new List<int>();
        //для хранения значений адресов датчиков первого канала (десятичное представление)
        public List<int> ZR_Address_Channel1_DEC = new List<int>();
        //для хранения значений адресов датчиков первого канала (шестнадцатиричное представление)
        List<string> ZR_Address_Channel1_HEX = new List<string>();
        //для хранения значений адресов датчиков второго канала (десятичное представление)
        public List<int> ZR_Address_Channel2_DEC = new List<int>();
        //для хранения значений адресов датчиков второго канала (шестнадцатиричное представление)
        List<string> ZR_Address_Channel2_HEX = new List<string>();
        //для хранения конфигурации внешних датчиков (шестнадцатиричное представление)
        public struct External_ZR
        {
            public int Index;           //от 21 до 28
            public string Result_hex;    
            public int Address;      
            public int NP;             
            public int Chanel;
        }
        //для хранения списка внешних датчиков
        public External_ZR[] Ext_ZR= new External_ZR[8];
        public struct UCH_IndexZR
        {
            public string UCH_Name;
            public List<int> ZR_Left;
            public List<int> ZR_Right;
        }
        //для хранения индексов датчиков в участках
        public UCH_IndexZR[] UCH_list_index = new UCH_IndexZR[24];  //информацию о индексах датчиков в участках
        private void Warning_Dialog_Show(string Warning_Text_str1, string Warning_Text_str2, string Warning_Text_str3)   //отображение диалогового окна
        {
            Warning warning = new Warning();
            warning.Warning_Text_str1.Text = Warning_Text_str1;
            warning.Warning_Text_str2.Text = Warning_Text_str2;
            warning.Warning_Text_str3.Text = Warning_Text_str3;
            warning.ShowDialog();
        }
        private void ExternalZR_Initializate()
        {
            //инициализируем поля структуры внешних датчиков
            for (int i = 0; i < Ext_ZR.Length; i++)
            {
                Ext_ZR[i].Index = 21 + i;
                Ext_ZR[i].Result_hex = "00";
                Ext_ZR[i].Address = 0;
                Ext_ZR[i].NP = 0;
                Ext_ZR[i].Chanel = 0;
            }
        }
        private void Write_ZP_data()
        {
            //адреса датчиков в десятичном формате
            ZR_Address_Channel1_DEC.Clear();
            for (int i = 0; i < NP_Channel1_CountZR; i++)
            {
                ZR_Address_Channel1_DEC.Add(Convert.ToInt32(NP_ZR_channel1[i].NP_ZR_Address.Text));
            }
            ZR_Address_Channel2_DEC.Clear();
            for (int i = 0; i < NP_Channel2_CountZR; i++)
            {
                ZR_Address_Channel2_DEC.Add(Convert.ToInt32(NP_ZR_channel2[i].NP_ZR_Address.Text));
            }
            //адреса датчиков в шестнадцатиричном формате
            ZR_Address_Channel1_HEX.Clear();
            for (int i = 0; i < ZR_Address_Channel1_DEC.Count; i++)
            {
                ZR_Address_Channel1_HEX.Add(ZR_Address_Channel1_DEC[i].ToString("X2"));
            }
            ZR_Address_Channel2_HEX.Clear();
            for (int i = 0; i < ZR_Address_Channel2_DEC.Count; i++)
            {
                ZR_Address_Channel2_HEX.Add(ZR_Address_Channel2_DEC[i].ToString("X2"));
            }
        }   //записываем данные датчиков в массивы
        public bool Write_UCH_data()    //записываем данные участков в массивы
        {
            bool result = true;
            //инициализируем поля структуры участков
            for (int i = 0; i < UCH_list_index.Length; i++)
            {
                UCH_list_index[i].UCH_Name = "";
                UCH_list_index[i].ZR_Left = new List<int>();
                UCH_list_index[i].ZR_Right = new List<int>();
            }
            //инициализируем поля структуры внешних датчиков
            ExternalZR_Initializate();
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
                        int index_zr = Search_IndexZR_ExternalNP(Addres, NP, Channel);
                        if (index_zr != 0)
                        {
                            UCH_list_index[a].ZR_Left.Add(index_zr);
                        }
                        else
                        {
                            Warning_Dialog_Show("Превышено максимальное количество", "внешних датчиков для текущего NP!", "");
                            result = false;
                            break;
                        }
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
                        int index_zr = Search_IndexZR_ExternalNP(Addres, NP, Channel);
                        if (index_zr != 0)
                        {
                            UCH_list_index[a].ZR_Right.Add(index_zr);
                        }
                        else
                        {
                            Warning_Dialog_Show("Превышено максимальное количество", "внешних датчиков для текущего NP!", "");
                            result = false;
                            break;
                        }
                    }
                }
            }
            if (result)
            {
                //определяем количество использованных внешних датчиков
                int count = 0;
                for (int i = 0; i < Ext_ZR.Length; i++)
                {
                    if (Ext_ZR[i].Result_hex != "00") count++;
                }
                ExternalZR_Count_Txt.Text = string.Format("Использовано внешних датчиков - {0} из 8", count);
            }
            return result;
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

            //смотрим список внешних датчиков и ищем текущий датчик
            for (int i = 0; i < Ext_ZR.Length; i++)
            {
                if (Ext_ZR[i].Address == ZR && Ext_ZR[i].NP == NP && Ext_ZR[i].Chanel == Channel)
                {
                    result = Ext_ZR[i].Index; 
                    break;
                }
            }
            //если такого датчика еще в списке нет - добавляем
            if (result == 0)   
            {
                int Index_External_NP = 0;  //индекс внешнего NP
                int IndexZR_in_ExternalNP = 0;  //индекс датчика во внешнем NP

                // 1 - Определяем индекс внешнего NP
                {
                    //добавляем NP в список внешних NP, если его там нет
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
                }
                // 2 - определяем индекс датчика во внешнем NP
                {
                    if (Channel == 1)
                    {
                        for (int i = 0; i < ZR_List[NP - 1].channel_1.Count; i++)
                        {
                            if (ZR_List[NP - 1].channel_1[i] == ZR)
                            {
                                IndexZR_in_ExternalNP = i + 1;
                                break;
                            }
                        }
                    }
                    if (Channel == 2)
                    {
                        for (int i = 0; i < ZR_List[NP - 1].channel_2.Count; i++)
                        {
                            if (ZR_List[NP - 1].channel_2[i] == ZR)
                            {
                                IndexZR_in_ExternalNP = ZR_List[NP - 1].channel_1.Count + i + 1;
                                break;
                            }
                        }
                    }
                }
                // 3 - формируем результат
                {
                    //определяем индекс в массиве внешних датчиков для записи
                    int index_external_zr = -1;
                    for (int i = 0; i < Ext_ZR.Length; i++)
                    {
                        if (Ext_ZR[i].Result_hex == "00")
                        {
                            index_external_zr = i;
                            break;
                        }
                    }
                    if (index_external_zr == -1)    //если заполнен весь список внешних датчиков
                    {
                        return 0;
                    }
                    //записываем
                    Ext_ZR[index_external_zr].Result_hex = ((Index_External_NP << 5) | IndexZR_in_ExternalNP).ToString("X2");
                    Ext_ZR[index_external_zr].Address = ZR;
                    Ext_ZR[index_external_zr].NP = NP;
                    Ext_ZR[index_external_zr].Chanel = Channel;

                    result = Ext_ZR[index_external_zr].Index;
                }
            }

            return result;
        }
        public string Forming_Result()
        {
            string result = "";
            result += Str_1();
            result += Str_2();
            result += Str_3();
            result += Str_4();
            result += Str_5();
            result += Str_6();
            result += Str_7();
            result += Str_8_Str_15();
            result += NP_InputZR_STR();
            result += NP_ExternalZR_STR();
            result += Str_18();
            result += Str_UCH();
            return result;
        }
        private string Str_1()
        {
            string result = "";

            result = string.Format("{0}.{1}.{2}.{3}.{4}.{5}'",TB11.Text,TB12.Text,TB13.Text,TB14.Text,TB15.Text,TB16.Text);
            result = result.PadRight(50, ' ');
            result += "\tIP адрес разъема T2 первого канала\r\n";

            return result;
        }
        private string Str_2()
        {
            string result = "";

            result = string.Format("{0}.{1}.{2}.{3}.{4}.{5}'", TB21.Text, TB22.Text, TB23.Text, TB24.Text, TB25.Text, TB26.Text);
            result = result.PadRight(50, ' ');
            result += "\tIP адрес разъема T2 второго канала\r\n";

            return result;
        }
        private string Str_3()
        {
            string result = "";

            result = string.Format("{0}.{1}.{2}.{3}:{4}:{5}'", TB31.Text, TB32.Text, TB33.Text, TB34.Text, TB35.Text, TB36.Text);
            result = result.PadRight(50, ' ');
            result += "\tIP адрес, UDP порт разъема T1\r\n";

            return result;
        }
        private string Str_4()
        {
            string result = "";

            result = string.Format("{0}.{1}.{2}.{3}:{4}:{5}'", TB41.Text, TB42.Text, TB43.Text, TB44.Text, TB45.Text, TB46.Text);
            result = result.PadRight(50, ' ');
            result += "\tIP адрес, UDP порт разъема T2\r\n";

            return result;
        }
        private string Str_5()
        {
            string result = "";

            result = string.Format("{0}.{1}.{2}.{3}:{4}:{5}'", TB51.Text, TB52.Text, TB53.Text, TB54.Text, TB55.Text, TB56.Text);
            result = result.PadRight(50, ' ');
            result += "\tIP адрес системы ЖАТ, UDP порт разъема T1\r\n";

            return result;
        }
        private string Str_6()
        {
            string result = "";

            result = string.Format("{0}.{1}.{2}.{3}:{4}:{5}'", TB61.Text, TB62.Text, TB63.Text, TB64.Text, TB65.Text, TB66.Text);
            result = result.PadRight(50, ' ');
            result += "\tIP адрес системы ЖАТ, UDP порт разъема T2\r\n";

            return result;
        }
        private string Str_7()
        {
            string result = "";

            result = "0.0'";
            result = result.PadRight(60, ' ');
            result += "\tСкорость обмена данными 1 и 2 канала по RS-485\r\n";

            return result;
        }
        private string Str_8_Str_15()
        {
            string result = "";
            string[] str = new string[8];
            for (int i = 0; i < 8; i++) //максимум 8 внешних NP
            {
                if (External_NP.Count > i)
                {
                    string st1 = IP_All_Np_List[External_NP[i] - 1].TB34.ToString();
                    string st2 = IP_All_Np_List[External_NP[i] - 1].TB56.ToString();
                    string st3 = IP_All_Np_List[External_NP[i] - 1].TB44.ToString();
                    string st4 = IP_All_Np_List[External_NP[i] - 1].TB66.ToString();
                    str[i] = string.Format("{0}:{1}.{2}:{3}'", st1, st2, st3, st4);
                    str[i] = str[i].PadRight(50, ' ');
                    str[i] += "\tКонфигурация внешнего нпорта с индексом " + i + "\r\n";
                }
                else
                {
                    str[i] = "0'";
                    str[i] = str[i].PadRight(60, ' ');
                    str[i] += "\tКонфигурация внешнего нпорта с индексом " + i + "\r\n";
                }
            }
            for (int i = 0; i < str.Length; i++)
            {
                result = result + str[i];
            }
            return result;
        }
        private string NP_InputZR_STR()
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
        private string NP_ExternalZR_STR()
        {
            string result = "";
            for (int i = 0; i < Ext_ZR.Length; i++)
            {
                result += Ext_ZR[i].Result_hex + " ";
            }
            result = result.Trim();
            result += "'";
            result = result.PadRight(50, ' ');
            result += "\tКонфигурация датчиков, не подключенных к нпорту\r\n";
            return result;
        }
        private string Str_18()
        {
            string result = "";

            result = UCH_Count.ToString("X2");
            result += "'";
            result = result.PadRight(60, ' ');
            result += "\tКоличество конфигурируемых участков\r\n";

            return result;
        }
        private string Str_UCH()
        {
            string result = "";

            for (int i = 0; i < UCH_Count; i++)
            {
                result += UCH_forming_result(i);
            }

            return result;
        }
        private string UCH_forming_result(int UCH_Index)
        {
            string result = "";

            int zr_count = 0;
            string zr_left = "";
            string zr_right = "";

            for (int a = 0; a < UCH_list_index[UCH_Index].ZR_Left.Count; a++)
            {
                zr_count++;
                zr_left += UCH_list_index[UCH_Index].ZR_Left[a].ToString("X2") + " ";
            }
            for (int b = 0; b < UCH_list_index[UCH_Index].ZR_Right.Count; b++)
            {
                zr_count++;
                zr_right += ((1 << 7) | UCH_list_index[UCH_Index].ZR_Right[b]).ToString("X2") + " ";
            }

            result += zr_count.ToString("X2") + " 00";
            if (zr_left != "")
            {
                result += " " + zr_left.Trim();
            }
            if (zr_right != "")
            {
                result += " " + zr_right.Trim();
            }
            result += "'";
            result = result.PadRight(50, ' ');
            result += "\tКонфигурирование участка " + UCH_list_index[UCH_Index].UCH_Name + "\r\n";

            return result;
        }
    }
}
