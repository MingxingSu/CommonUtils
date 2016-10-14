using System;

namespace MaxSu.Framework.Common.Chinese
{
    /// <summary>
    ///     ����תũ��
    /// </summary>
    public class ChinaDate
    {
        #region ˽�з���

        private static readonly long[] lunarInfo = new long[]
            {
                0x04bd8, 0x04ae0, 0x0a570, 0x054d5, 0x0d260, 0x0d950, 0x16554,
                0x056a0, 0x09ad0, 0x055d2, 0x04ae0, 0x0a5b6, 0x0a4d0, 0x0d250, 0x1d255, 0x0b540, 0x0d6a0, 0x0ada2, 0x095b0,
                0x14977, 0x04970, 0x0a4b0, 0x0b4b5, 0x06a50, 0x06d40, 0x1ab54, 0x02b60, 0x09570, 0x052f2, 0x04970, 0x06566,
                0x0d4a0, 0x0ea50, 0x06e95, 0x05ad0, 0x02b60, 0x186e3, 0x092e0, 0x1c8d7, 0x0c950, 0x0d4a0, 0x1d8a6, 0x0b550,
                0x056a0, 0x1a5b4, 0x025d0, 0x092d0, 0x0d2b2, 0x0a950, 0x0b557, 0x06ca0, 0x0b550, 0x15355, 0x04da0, 0x0a5d0,
                0x14573, 0x052d0, 0x0a9a8, 0x0e950, 0x06aa0, 0x0aea6, 0x0ab50, 0x04b60, 0x0aae4, 0x0a570, 0x05260, 0x0f263,
                0x0d950, 0x05b57, 0x056a0, 0x096d0, 0x04dd5, 0x04ad0, 0x0a4d0, 0x0d4d4, 0x0d250, 0x0d558, 0x0b540, 0x0b5a0,
                0x195a6, 0x095b0, 0x049b0, 0x0a974, 0x0a4b0, 0x0b27a, 0x06a50, 0x06d40, 0x0af46, 0x0ab60, 0x09570, 0x04af5,
                0x04970, 0x064b0, 0x074a3, 0x0ea50, 0x06b58, 0x055c0, 0x0ab60, 0x096d5, 0x092e0, 0x0c960, 0x0d954, 0x0d4a0,
                0x0da50, 0x07552, 0x056a0, 0x0abb7, 0x025d0, 0x092d0, 0x0cab5, 0x0a950, 0x0b4a0, 0x0baa4, 0x0ad50, 0x055d9,
                0x04ba0, 0x0a5b0, 0x15176, 0x052b0, 0x0a930, 0x07954, 0x06aa0, 0x0ad50, 0x05b52, 0x04b60, 0x0a6e6, 0x0a4e0,
                0x0d260, 0x0ea65, 0x0d530, 0x05aa0, 0x076a3, 0x096d0, 0x04bd7, 0x04ad0, 0x0a4d0, 0x1d0b6, 0x0d250, 0x0d520,
                0x0dd45, 0x0b5a0, 0x056d0, 0x055b2, 0x049b0, 0x0a577, 0x0a4b0, 0x0aa50, 0x1b255, 0x06d20, 0x0ada0
            };

        private static readonly int[] year20 = new[] {1, 4, 1, 2, 1, 2, 1, 1, 2, 1, 2, 1};
        private static readonly int[] year19 = new[] {0, 3, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0};
        private static readonly int[] year2000 = new[] {0, 3, 1, 2, 1, 2, 1, 1, 2, 1, 2, 1};
        private static readonly String[] nStr1 = new[] {"", "��", "��", "��", "��", "��", "��", "��", "��", "��", "ʮ", "ʮһ", "ʮ��"};
        private static readonly String[] Gan = new[] {"��", "��", "��", "��", "��", "��", "��", "��", "��", "��"};
        private static readonly String[] Zhi = new[] {"��", "��", "��", "î", "��", "��", "��", "δ", "��", "��", "��", "��"};
        private static readonly String[] Animals = new[] {"��", "ţ", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��"};

        private static readonly String[] solarTerm = new[]
            {
                "С��", "��", "����", "��ˮ", "����", "����", "����", "����", "����", "С��", "â��", "����", "С��", "����", "����", "����", "��¶", "���",
                "��¶", "˪��", "����", "Сѩ", "��ѩ", "����"
            };

        private static readonly int[] sTermInfo =
            {
                0, 21208, 42467, 63836, 85337, 107014, 128867, 150921, 173149, 195551,
                218072, 240693, 263343, 285989, 308563, 331033, 353350, 375494, 397447, 419210, 440795, 462224, 483532, 504758
            };

        private static readonly String[] lFtv = new[]
            {
                "0101ũ������", "0202 ��̧ͷ��", "0115 Ԫ����", "0505 �����", "0707 ��Ϧ���˽�", "0815 �����", "0909 ������", "1208 ���˽�",
                "1114 �����������", "1224 С��", "0100��Ϧ"
            };

        private static readonly String[] sFtv = new[]
            {
                "0101 ����Ԫ��",
                "0202 ����ʪ����",
                "0207 ������Ԯ�Ϸ���",
                "0210 ���������",
                "0214 ���˽�",
                "0301 ���ʺ�����",
                "0303 ȫ��������",
                "0308 ���ʸ�Ů��",
                "0312 ֲ���� ����ɽ����������",
                "0314 ���ʾ�����",
                "0315 ����������Ȩ����",
                "0317 �й���ҽ�� ���ʺ�����",
                "0321 ����ɭ���� �����������ӹ�����",
                "0321 ���������",
                "0322 ����ˮ��",
                "0323 ����������",
                "0324 ������ν�˲���",
                "0325 ȫ����Сѧ����ȫ������",
                "0330 ����˹̹������",
                "0401 ���˽� ȫ�����������˶���(����) ˰��������(����)",
                "0407 ����������",
                "0422 ���������",
                "0423 ����ͼ��Ͱ�Ȩ��",
                "0424 �Ƿ����Ź�������",
                "0501 �����Ͷ���",
                "0504 �й����������",
                "0505 ��ȱ����������",
                "0508 �����ʮ����",
                "0512 ���ʻ�ʿ��",
                "0515 ���ʼ�ͥ��",
                "0517 ���������",
                "0518 ���ʲ������",
                "0520 ȫ��ѧ��Ӫ����",
                "0523 ����ţ����",
                "0531 ����������",
                "0601 ���ʶ�ͯ��",
                "0605 ���绷����",
                "0606 ȫ��������",
                "0617 ���λ�Į���͸ɺ���",
                "0623 ���ʰ���ƥ����",
                "0625 ȫ��������",
                "0626 ���ʷ���Ʒ��",
                "0701 �й������������� ���罨����",
                "0702 ��������������",
                "0707 �й�������ս��������",
                "0711 �����˿���",
                "0730 ���޸�Ů��",
                "0801 �й�������",
                "0808 �й����ӽ�(�ְֽ�)",
                "0815 �ձ���ʽ����������Ͷ����",
                "0908 ����ɨä�� �������Ź�������",
                "0910 ��ʦ��",
                "0914 ������������",
                "0916 ���ʳ����㱣����",
                "0918 �š�һ���±������",
                "0920 ȫ��������",
                "0927 ����������",
                "1001 ����� ���������� �������˽�",
                "1001 ����������",
                "1002 ���ʺ�ƽ���������ɶ�����",
                "1004 ���綯����",
                "1008 ȫ����Ѫѹ��",
                "1008 �����Ӿ���",
                "1009 ���������� ���������",
                "1010 �������������� ���羫��������",
                "1013 ���籣���� ���ʽ�ʦ��",
                "1014 �����׼��",
                "1015 ����ä�˽�(�����Ƚ�)",
                "1016 ������ʳ��",
                "1017 ��������ƶ����",
                "1022 ���紫ͳҽҩ��",
                "1024 ���Ϲ��� ���緢չ��Ϣ��",
                "1031 �����ڼ���",
                "1107 ʮ������������������",
                "1108 �й�������",
                "1109 ȫ��������ȫ����������",
                "1110 ���������",
                "1111 ���ʿ�ѧ���ƽ��(����������һ��)",
                "1112 ����ɽ����������",
                "1114 ����������",
                "1117 ���ʴ�ѧ���� ����ѧ����",
                "1121 �����ʺ��� ���������",
                "1129 ������Ԯ����˹̹���������",
                "1201 ���簬�̲���",
                "1203 ����м�����",
                "1205 ���ʾ��ú���ᷢչ־Ը��Ա��",
                "1208 ���ʶ�ͯ������",
                "1209 ����������",
                "1210 ������Ȩ��",
                "1212 �����±������",
                "1213 �Ͼ�����ɱ(1937��)�����գ�����Ѫ��ʷ��",
                "1221 ����������",
                "1224 ƽ��ҹ",
                "1225 ʥ����",
                "1226 ë��ϯ����",
                "1229 ���������������"
            };


        /// <summary>
        ///     ����ũ��y���������
        /// </summary>
        private static int lYearDays(int y)
        {
            int i, sum = 348;
            for (i = 0x8000; i > 0x8; i >>= 1)
            {
                if ((lunarInfo[y - 1900] & i) != 0)
                    sum += 1;
            }
            return (sum + leapDays(y));
        }

        /// <summary>
        ///     ����ũ��y�����µ�����
        /// </summary>
        private static int leapDays(int y)
        {
            if (leapMonth(y) != 0)
            {
                if ((lunarInfo[y - 1900] & 0x10000) != 0)
                    return 30;
                else
                    return 29;
            }
            else
                return 0;
        }

        /// <summary>
        ///     ����ũ��y�����ĸ��� 1-12 , û�򴫻� 0
        /// </summary>
        private static int leapMonth(int y)
        {
            return (int) (lunarInfo[y - 1900] & 0xf);
        }

        /// <summary>
        ///     ����ũ��y��m�µ�������
        /// </summary>
        private static int monthDays(int y, int m)
        {
            if ((lunarInfo[y - 1900] & (0x10000 >> m)) == 0)
                return 29;
            else
                return 30;
        }

        /// <summary>
        ///     ����ũ��y�����Ф
        /// </summary>
        private static String AnimalsYear(int y)
        {
            return Animals[(y - 4)%12];
        }

        /// <summary>
        ///     �������յ�offset ���ظ�֧,0=����
        /// </summary>
        private static String cyclicalm(int num)
        {
            return (Gan[num%10] + Zhi[num%12]);
        }

        /// <summary>
        ///     ����offset ���ظ�֧, 0=����
        /// </summary>
        private static String cyclical(int y)
        {
            int num = y - 1900 + 36;
            return (cyclicalm(num));
        }

        /// <summary>
        ///     ����ũ��.year0 .month1 .day2 .yearCyl3 .monCyl4 .dayCyl5 .isLeap6
        /// </summary>
        private long[] Lunar(int y, int m)
        {
            var nongDate = new long[7];
            int i = 0, temp = 0, leap = 0;
            var baseDate = new DateTime(1900 + 1900, 2, 31);
            var objDate = new DateTime(y + 1900, m + 1, 1);
            TimeSpan ts = objDate - baseDate;
            var offset = (long) ts.TotalDays;
            if (y < 2000)
                offset += year19[m - 1];
            if (y > 2000)
                offset += year20[m - 1];
            if (y == 2000)
                offset += year2000[m - 1];
            nongDate[5] = offset + 40;
            nongDate[4] = 14;

            for (i = 1900; i < 2050 && offset > 0; i++)
            {
                temp = lYearDays(i);
                offset -= temp;
                nongDate[4] += 12;
            }
            if (offset < 0)
            {
                offset += temp;
                i--;
                nongDate[4] -= 12;
            }
            nongDate[0] = i;
            nongDate[3] = i - 1864;
            leap = leapMonth(i); // ���ĸ���
            nongDate[6] = 0;

            for (i = 1; i < 13 && offset > 0; i++)
            {
                // ����
                if (leap > 0 && i == (leap + 1) && nongDate[6] == 0)
                {
                    --i;
                    nongDate[6] = 1;
                    temp = leapDays((int) nongDate[0]);
                }
                else
                {
                    temp = monthDays((int) nongDate[0], i);
                }

                // �������
                if (nongDate[6] == 1 && i == (leap + 1))
                    nongDate[6] = 0;
                offset -= temp;
                if (nongDate[6] == 0)
                    nongDate[4]++;
            }

            if (offset == 0 && leap > 0 && i == leap + 1)
            {
                if (nongDate[6] == 1)
                {
                    nongDate[6] = 0;
                }
                else
                {
                    nongDate[6] = 1;
                    --i;
                    --nongDate[4];
                }
            }
            if (offset < 0)
            {
                offset += temp;
                --i;
                --nongDate[4];
            }
            nongDate[1] = i;
            nongDate[2] = offset + 1;
            return nongDate;
        }

        /// <summary>
        ///     ����y��m��d�ն�Ӧ��ũ��.year0 .month1 .day2 .yearCyl3 .monCyl4 .dayCyl5 .isLeap6
        /// </summary>
        private static long[] calElement(int y, int m, int d)
        {
            var nongDate = new long[7];
            int i = 0, temp = 0, leap = 0;

            var baseDate = new DateTime(1900, 1, 31);

            var objDate = new DateTime(y, m, d);
            TimeSpan ts = objDate - baseDate;

            var offset = (long) ts.TotalDays;

            nongDate[5] = offset + 40;
            nongDate[4] = 14;

            for (i = 1900; i < 2050 && offset > 0; i++)
            {
                temp = lYearDays(i);
                offset -= temp;
                nongDate[4] += 12;
            }
            if (offset < 0)
            {
                offset += temp;
                i--;
                nongDate[4] -= 12;
            }
            nongDate[0] = i;
            nongDate[3] = i - 1864;
            leap = leapMonth(i); // ���ĸ���
            nongDate[6] = 0;

            for (i = 1; i < 13 && offset > 0; i++)
            {
                // ����
                if (leap > 0 && i == (leap + 1) && nongDate[6] == 0)
                {
                    --i;
                    nongDate[6] = 1;
                    temp = leapDays((int) nongDate[0]);
                }
                else
                {
                    temp = monthDays((int) nongDate[0], i);
                }

                // �������
                if (nongDate[6] == 1 && i == (leap + 1))
                    nongDate[6] = 0;
                offset -= temp;
                if (nongDate[6] == 0)
                    nongDate[4]++;
            }

            if (offset == 0 && leap > 0 && i == leap + 1)
            {
                if (nongDate[6] == 1)
                {
                    nongDate[6] = 0;
                }
                else
                {
                    nongDate[6] = 1;
                    --i;
                    --nongDate[4];
                }
            }
            if (offset < 0)
            {
                offset += temp;
                --i;
                --nongDate[4];
            }
            nongDate[1] = i;
            nongDate[2] = offset + 1;
            return nongDate;
        }

        private static String getChinaDate(int day)
        {
            String a = "";
            if (day == 10)
                return "��ʮ";
            if (day == 20)
                return "��ʮ";
            if (day == 30)
                return "��ʮ";
            int two = ((day)/10);
            if (two == 0)
                a = "��";
            if (two == 1)
                a = "ʮ";
            if (two == 2)
                a = "إ";
            if (two == 3)
                a = "��";
            int one = (day%10);
            switch (one)
            {
                case 1:
                    a += "һ";
                    break;
                case 2:
                    a += "��";
                    break;
                case 3:
                    a += "��";
                    break;
                case 4:
                    a += "��";
                    break;
                case 5:
                    a += "��";
                    break;
                case 6:
                    a += "��";
                    break;
                case 7:
                    a += "��";
                    break;
                case 8:
                    a += "��";
                    break;
                case 9:
                    a += "��";
                    break;
            }
            return a;
        }

        private static DateTime sTerm(int y, int n)
        {
            double ms = 31556925974.7*(y - 1900);
            double ms1 = sTermInfo[n];
            var offDate = new DateTime(1900, 1, 6, 2, 5, 0);
            offDate = offDate.AddMilliseconds(ms);
            offDate = offDate.AddMinutes(ms1);
            return offDate;
        }

        private static string FormatDate(int m, int d)
        {
            return string.Format("{0:00}{1:00}", m, d);
        }

        #endregion

        #region ���з���

        /// <summary>
        ///     ���ع���y��m�µ�������
        /// </summary>
        public static int GetDaysByMonth(int y, int m)
        {
            var days = new[] {31, DateTime.IsLeapYear(y) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
            return days[m - 1];
        }

        /// <summary>
        ///     ��������ֵ�����һ������
        /// </summary>
        /// <param name="dt">��������</param>
        /// <returns>��һ������</returns>
        public static DateTime GetMondayDateByDate(DateTime dt)
        {
            double d = 0;
            switch ((int) dt.DayOfWeek)
            {
                    //case 1: d = 0; break;
                case 2:
                    d = -1;
                    break;
                case 3:
                    d = -2;
                    break;
                case 4:
                    d = -3;
                    break;
                case 5:
                    d = -4;
                    break;
                case 6:
                    d = -5;
                    break;
                case 0:
                    d = -6;
                    break;
            }
            return dt.AddDays(d);
        }

        /// <summary>
        ///     ��ȡũ��
        /// </summary>
        public static ChineseCalendarDate getChinaDate(DateTime dt)
        {
            var cd = new ChineseCalendarDate();
            int year = dt.Year;
            int month = dt.Month;
            int date = dt.Day;
            long[] l = calElement(year, month, date);
            cd.cnIntYear = (int) l[0];
            cd.cnIntMonth = (int) l[1];
            cd.cnIntDay = (int) l[2];
            cd.cnStrYear = cyclical(year);
            cd.Animal = AnimalsYear(year);
            cd.cnStrMonth = nStr1[(int) l[1]];
            cd.cnStrDay = getChinaDate((int) (l[2]));
            string smd = dt.ToString("MMdd");

            string lmd = FormatDate(cd.cnIntMonth, cd.cnIntDay);
            for (int i = 0; i < solarTerm.Length; i++)
            {
                string s1 = sTerm(dt.Year, i).ToString("MMdd");
                if (s1.Equals(dt.ToString("MMdd")))
                {
                    cd.cnSolarTerm = solarTerm[i];
                    break;
                }
            }
            foreach (string s in sFtv)
            {
                string s1 = s.Substring(0, 4);
                if (s1.Equals(smd))
                {
                    cd.WesternFestival = s.Substring(4, s.Length - 4);
                    break;
                }
            }
            foreach (string s in lFtv)
            {
                string s1 = s.Substring(0, 4);
                if (s1.Equals(lmd))
                {
                    cd.ChineseFestival = s.Substring(4, s.Length - 4);
                    break;
                }
            }
            dt = dt.AddDays(1);
            year = dt.Year;
            month = dt.Month;
            date = dt.Day;
            l = calElement(year, month, date);
            lmd = FormatDate((int) l[1], (int) l[2]);
            if (lmd.Equals("0101")) cd.ChineseFestival = "��Ϧ";
            return cd;
        }

        #endregion
    }
}