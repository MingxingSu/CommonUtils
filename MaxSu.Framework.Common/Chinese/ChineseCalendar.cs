using System;

namespace MaxSu.Framework.Common.Chinese
{
    /// <summary>
    ///     �й�����
    /// </summary>
//-------------------------------------------------------------------------------
//����:
//ChineseCalendar c = new ChineseCalendar(new DateTime(1990, 01, 15));
//StringBuilder dayInfo = new StringBuilder();
//dayInfo.Append("������" + c.DateString + "\r\n");
//dayInfo.Append("ũ����" + c.ChineseDateString + "\r\n");
//dayInfo.Append("���ڣ�" + c.WeekDayStr);
//dayInfo.Append("ʱ����" + c.ChineseHour + "\r\n");
//dayInfo.Append("���ࣺ" + c.AnimalString + "\r\n");
//dayInfo.Append("������" + c.ChineseTwentyFourDay + "\r\n");
//dayInfo.Append("ǰһ��������" + c.ChineseTwentyFourPrevDay + "\r\n");
//dayInfo.Append("��һ��������" + c.ChineseTwentyFourNextDay + "\r\n");
//dayInfo.Append("���գ�" + c.DateHoliday + "\r\n");
//dayInfo.Append("��֧��" + c.GanZhiDateString + "\r\n");
//dayInfo.Append("���ޣ�" + c.ChineseConstellation + "\r\n");
//dayInfo.Append("������" + c.Constellation + "\r\n");
//-------------------------------------------------------------------------------
    public class ChineseCalendar
    {
        #region �ڲ��ṹ

        /// <summary>
        ///     ũ��
        /// </summary>
        private struct LunarHolidayStruct
        {
            public readonly int Day;
            public readonly string HolidayName;
            public readonly int Month;
            public int Recess;

            public LunarHolidayStruct(int month, int day, int recess, string name)
            {
                Month = month;
                Day = day;
                Recess = recess;
                HolidayName = name;
            }
        }

        /// <summary>
        ///     ����
        /// </summary>
        private struct SolarHolidayStruct
        {
            public readonly int Day;
            public readonly string HolidayName;
            public readonly int Month;
            public int Recess; //���ڳ���

            public SolarHolidayStruct(int month, int day, int recess, string name)
            {
                Month = month;
                Day = day;
                Recess = recess;
                HolidayName = name;
            }
        }

        private struct WeekHolidayStruct
        {
            public readonly string HolidayName;
            public readonly int Month;
            public readonly int WeekAtMonth;
            public readonly int WeekDay;

            public WeekHolidayStruct(int month, int weekAtMonth, int weekDay, string name)
            {
                Month = month;
                WeekAtMonth = weekAtMonth;
                WeekDay = weekDay;
                HolidayName = name;
            }
        }

        #endregion

        #region �ڲ�����

        private readonly int _cDay;
        private readonly bool _cIsLeapMonth; //�����Ƿ�����
        private readonly bool _cIsLeapYear; //�����Ƿ�������
        private readonly int _cMonth;
        private readonly int _cYear;
        private readonly DateTime _datetime;
        private DateTime _date;

        #endregion

        #region ��������

        #region ��������

        private const int MinYear = 1900;
        private const int MaxYear = 2050;
        private const int GanZhiStartYear = 1864; //��֧������ʼ��
        private const string HZNum = "��һ�����������߰˾�";
        private const int AnimalStartYear = 1900; //1900��Ϊ����
        private static DateTime MinDay = new DateTime(1900, 1, 30);
        private static readonly DateTime MaxDay = new DateTime(2049, 12, 31);
        private static readonly DateTime GanZhiStartDay = new DateTime(1899, 12, 22); //��ʼ��
        private static readonly DateTime ChineseConstellationReferDay = new DateTime(2007, 9, 13); //28���޲ο�ֵ,����Ϊ��

        #endregion

        #region ��������

        /// <summary>
        ///     ��Դ�����ϵ�ũ������
        /// </summary>
        /// <remarks>
        ///     ���ݽṹ���£���ʹ��17λ����
        ///     ��17λ����ʾ����������0��ʾ29��   1��ʾ30��
        ///     ��16λ-��5λ����12λ����ʾ12���£����е�16λ��ʾ��һ�£��������Ϊ30����Ϊ1��29��Ϊ0
        ///     ��4λ-��1λ����4λ����ʾ�������ĸ��£��������û�����£�����0
        /// </remarks>
        private static readonly int[] LunarDateArray = new[]
            {
                0x04BD8, 0x04AE0, 0x0A570, 0x054D5, 0x0D260, 0x0D950, 0x16554, 0x056A0, 0x09AD0, 0x055D2,
                0x04AE0, 0x0A5B6, 0x0A4D0, 0x0D250, 0x1D255, 0x0B540, 0x0D6A0, 0x0ADA2, 0x095B0, 0x14977,
                0x04970, 0x0A4B0, 0x0B4B5, 0x06A50, 0x06D40, 0x1AB54, 0x02B60, 0x09570, 0x052F2, 0x04970,
                0x06566, 0x0D4A0, 0x0EA50, 0x06E95, 0x05AD0, 0x02B60, 0x186E3, 0x092E0, 0x1C8D7, 0x0C950,
                0x0D4A0, 0x1D8A6, 0x0B550, 0x056A0, 0x1A5B4, 0x025D0, 0x092D0, 0x0D2B2, 0x0A950, 0x0B557,
                0x06CA0, 0x0B550, 0x15355, 0x04DA0, 0x0A5B0, 0x14573, 0x052B0, 0x0A9A8, 0x0E950, 0x06AA0,
                0x0AEA6, 0x0AB50, 0x04B60, 0x0AAE4, 0x0A570, 0x05260, 0x0F263, 0x0D950, 0x05B57, 0x056A0,
                0x096D0, 0x04DD5, 0x04AD0, 0x0A4D0, 0x0D4D4, 0x0D250, 0x0D558, 0x0B540, 0x0B6A0, 0x195A6,
                0x095B0, 0x049B0, 0x0A974, 0x0A4B0, 0x0B27A, 0x06A50, 0x06D40, 0x0AF46, 0x0AB60, 0x09570,
                0x04AF5, 0x04970, 0x064B0, 0x074A3, 0x0EA50, 0x06B58, 0x055C0, 0x0AB60, 0x096D5, 0x092E0,
                0x0C960, 0x0D954, 0x0D4A0, 0x0DA50, 0x07552, 0x056A0, 0x0ABB7, 0x025D0, 0x092D0, 0x0CAB5,
                0x0A950, 0x0B4A0, 0x0BAA4, 0x0AD50, 0x055D9, 0x04BA0, 0x0A5B0, 0x15176, 0x052B0, 0x0A930,
                0x07954, 0x06AA0, 0x0AD50, 0x05B52, 0x04B60, 0x0A6E6, 0x0A4E0, 0x0D260, 0x0EA65, 0x0D530,
                0x05AA0, 0x076A3, 0x096D0, 0x04BD7, 0x04AD0, 0x0A4D0, 0x1D0B6, 0x0D250, 0x0D520, 0x0DD45,
                0x0B5A0, 0x056D0, 0x055B2, 0x049B0, 0x0A577, 0x0A4B0, 0x0AA50, 0x1B255, 0x06D20, 0x0ADA0,
                0x14B63
            };

        #endregion

        #region ��������

        private static readonly string[] _constellationName =
            {
                "������", "��ţ��", "˫����",
                "��з��", "ʨ����", "��Ů��",
                "�����", "��Ы��", "������",
                "Ħ����", "ˮƿ��", "˫����"
            };

        #endregion

        #region ��ʮ�Ľ���

        private static string[] _lunarHolidayName =
            {
                "С��", "��", "����", "��ˮ",
                "����", "����", "����", "����",
                "����", "С��", "â��", "����",
                "С��", "����", "����", "����",
                "��¶", "���", "��¶", "˪��",
                "����", "Сѩ", "��ѩ", "����"
            };

        #endregion

        #region ��ʮ������

        private static readonly string[] _chineseConstellationName =
            {
                //��        ��      ��         ��        һ      ��      ��  
                "��ľ��", "������", "Ů����", "������", "���º�", "β��", "��ˮ��",
                "��ľ�", "ţ��ţ", "ص����", "������", "Σ����", "�һ���", "��ˮ��",
                "��ľ��", "¦��", "θ����", "���ռ�", "������", "�����", "��ˮԳ",
                "��ľ��", "������", "�����", "������", "����¹", "������", "��ˮ�"
            };

        #endregion

        #region ��������

        private static readonly string[] SolarTerm = new[]
            {
                "С��", "��", "����", "��ˮ", "����", "����", "����", "����", "����", "С��", "â��", "����", "С��", "����", "����", "����", "��¶", "���",
                "��¶", "˪��", "����", "Сѩ", "��ѩ", "����"
            };

        private static readonly int[] sTermInfo = new[]
            {
                0, 21208, 42467, 63836, 85337, 107014, 128867, 150921, 173149, 195551, 218072, 240693, 263343, 285989, 308563,
                331033, 353350, 375494, 397447, 419210, 440795, 462224, 483532, 504758
            };

        #endregion

        #region ũ���������

        private static string ganStr = "���ұ����켺�����ɹ�";
        private static string zhiStr = "�ӳ���î������δ�����纥";
        private static string animalStr = "��ţ������������Ｆ����";
        private static string nStr1 = "��һ�����������߰˾�";
        private static string nStr2 = "��ʮإئ";

        private static readonly string[] _monthString =
            {
                "����", "����", "����", "����", "����", "����", "����", "����", "����", "����", "ʮ��", "ʮһ��", "����"
            };

        #endregion

        #region ����������Ľ���

        private static readonly SolarHolidayStruct[] sHolidayInfo = new[]
            {
                new SolarHolidayStruct(1, 1, 1, "Ԫ��"),
                new SolarHolidayStruct(2, 2, 0, "����ʪ����"),
                new SolarHolidayStruct(2, 10, 0, "���������"),
                new SolarHolidayStruct(2, 14, 0, "���˽�"),
                new SolarHolidayStruct(3, 1, 0, "���ʺ�����"),
                new SolarHolidayStruct(3, 5, 0, "ѧ�׷������"),
                new SolarHolidayStruct(3, 8, 0, "��Ů��"),
                new SolarHolidayStruct(3, 12, 0, "ֲ���� ����ɽ����������"),
                new SolarHolidayStruct(3, 14, 0, "���ʾ�����"),
                new SolarHolidayStruct(3, 15, 0, "������Ȩ����"),
                new SolarHolidayStruct(3, 17, 0, "�й���ҽ�� ���ʺ�����"),
                new SolarHolidayStruct(3, 21, 0, "����ɭ���� �����������ӹ����� ���������"),
                new SolarHolidayStruct(3, 22, 0, "����ˮ��"),
                new SolarHolidayStruct(3, 24, 0, "������ν�˲���"),
                new SolarHolidayStruct(4, 1, 0, "���˽�"),
                new SolarHolidayStruct(4, 7, 0, "����������"),
                new SolarHolidayStruct(4, 22, 0, "���������"),
                new SolarHolidayStruct(5, 1, 1, "�Ͷ���"),
                new SolarHolidayStruct(5, 2, 1, "�Ͷ��ڼ���"),
                new SolarHolidayStruct(5, 3, 1, "�Ͷ��ڼ���"),
                new SolarHolidayStruct(5, 4, 0, "�����"),
                new SolarHolidayStruct(5, 8, 0, "�����ʮ����"),
                new SolarHolidayStruct(5, 12, 0, "���ʻ�ʿ��"),
                new SolarHolidayStruct(5, 31, 0, "����������"),
                new SolarHolidayStruct(6, 1, 0, "���ʶ�ͯ��"),
                new SolarHolidayStruct(6, 5, 0, "���绷��������"),
                new SolarHolidayStruct(6, 26, 0, "���ʽ�����"),
                new SolarHolidayStruct(7, 1, 0, "������ ��ۻع���� ���罨����"),
                new SolarHolidayStruct(7, 11, 0, "�����˿���"),
                new SolarHolidayStruct(8, 1, 0, "������"),
                new SolarHolidayStruct(8, 8, 0, "�й����ӽ� ���׽�"),
                new SolarHolidayStruct(8, 15, 0, "����ս��ʤ������"),
                new SolarHolidayStruct(9, 9, 0, "ë��ϯ��������"),
                new SolarHolidayStruct(9, 10, 0, "��ʦ��"),
                new SolarHolidayStruct(9, 18, 0, "�š�һ���±������"),
                new SolarHolidayStruct(9, 20, 0, "���ʰ�����"),
                new SolarHolidayStruct(9, 27, 0, "����������"),
                new SolarHolidayStruct(9, 28, 0, "���ӵ���"),
                new SolarHolidayStruct(10, 1, 1, "����� ����������"),
                new SolarHolidayStruct(10, 2, 1, "����ڼ���"),
                new SolarHolidayStruct(10, 3, 1, "����ڼ���"),
                new SolarHolidayStruct(10, 6, 0, "���˽�"),
                new SolarHolidayStruct(10, 24, 0, "���Ϲ���"),
                new SolarHolidayStruct(11, 10, 0, "���������"),
                new SolarHolidayStruct(11, 12, 0, "����ɽ��������"),
                new SolarHolidayStruct(12, 1, 0, "���簬�̲���"),
                new SolarHolidayStruct(12, 3, 0, "����м�����"),
                new SolarHolidayStruct(12, 20, 0, "���Żع����"),
                new SolarHolidayStruct(12, 24, 0, "ƽ��ҹ"),
                new SolarHolidayStruct(12, 25, 0, "ʥ����"),
                new SolarHolidayStruct(12, 26, 0, "ë��ϯ��������")
            };

        #endregion

        #region ��ũ������Ľ���

        private static readonly LunarHolidayStruct[] lHolidayInfo = new[]
            {
                new LunarHolidayStruct(1, 1, 1, "����"),
                new LunarHolidayStruct(1, 15, 0, "Ԫ����"),
                new LunarHolidayStruct(5, 5, 0, "�����"),
                new LunarHolidayStruct(7, 7, 0, "��Ϧ���˽�"),
                new LunarHolidayStruct(7, 15, 0, "��Ԫ�� �������"),
                new LunarHolidayStruct(8, 15, 0, "�����"),
                new LunarHolidayStruct(9, 9, 0, "������"),
                new LunarHolidayStruct(12, 8, 0, "���˽�"),
                new LunarHolidayStruct(12, 23, 0, "����С��(ɨ��)"),
                new LunarHolidayStruct(12, 24, 0, "�Ϸ�С��(����)"),
                //new LunarHolidayStruct(12, 30, 0, "��Ϧ")  //ע���Ϧ��Ҫ�����������м���
            };

        #endregion

        #region ��ĳ�µڼ������ڼ�

        private static readonly WeekHolidayStruct[] wHolidayInfo = new[]
            {
                new WeekHolidayStruct(5, 2, 1, "ĸ�׽�"),
                new WeekHolidayStruct(5, 3, 1, "ȫ��������"),
                new WeekHolidayStruct(6, 3, 1, "���׽�"),
                new WeekHolidayStruct(9, 3, 3, "���ʺ�ƽ��"),
                new WeekHolidayStruct(9, 4, 1, "�������˽�"),
                new WeekHolidayStruct(10, 1, 2, "����ס����"),
                new WeekHolidayStruct(10, 1, 4, "���ʼ�����Ȼ�ֺ���"),
                new WeekHolidayStruct(11, 4, 5, "�ж���")
            };

        #endregion

        #endregion

        #region ���캯��

        #region �������ڳ�ʼ��

        /// <summary>
        ///     ��һ����׼�Ĺ�����������ʹ��
        /// </summary>
        public ChineseCalendar(DateTime dt)
        {
            int i;
            int leap;
            int temp;
            int offset;

            CheckDateLimit(dt);

            _date = dt.Date;
            _datetime = dt;

            //ũ�����ڼ��㲿��
            leap = 0;
            temp = 0;

            //��������Ļ������
            TimeSpan ts = _date - MinDay;
            offset = ts.Days;

            for (i = MinYear; i <= MaxYear; i++)
            {
                //����ũ��������
                temp = GetChineseYearDays(i);
                if (offset - temp < 1)
                    break;
                else
                {
                    offset = offset - temp;
                }
            }
            _cYear = i;

            //����������ĸ���
            leap = GetChineseLeapMonth(_cYear);

            //�趨�����Ƿ�������
            if (leap > 0)
            {
                _cIsLeapYear = true;
            }
            else
            {
                _cIsLeapYear = false;
            }

            _cIsLeapMonth = false;
            for (i = 1; i <= 12; i++)
            {
                //����
                if ((leap > 0) && (i == leap + 1) && (_cIsLeapMonth == false))
                {
                    _cIsLeapMonth = true;
                    i = i - 1;
                    temp = GetChineseLeapMonthDays(_cYear); //������������
                }
                else
                {
                    _cIsLeapMonth = false;
                    temp = GetChineseMonthDays(_cYear, i); //�������������
                }

                offset = offset - temp;
                if (offset <= 0) break;
            }

            offset = offset + temp;
            _cMonth = i;
            _cDay = offset;
        }

        #endregion

        #region ũ�����ڳ�ʼ��

        /// <summary>
        ///     ��ũ������������ʹ��
        /// </summary>
        /// <param name="cy">ũ����</param>
        /// <param name="cm">ũ����</param>
        /// <param name="cd">ũ����</param>
        /// <param name="LeapFlag">���±�־</param>
        public ChineseCalendar(int cy, int cm, int cd, bool leapMonthFlag)
        {
            int i, leap, Temp, offset;

            CheckChineseDateLimit(cy, cm, cd, leapMonthFlag);

            _cYear = cy;
            _cMonth = cm;
            _cDay = cd;

            offset = 0;

            for (i = MinYear; i < cy; i++)
            {
                //����ũ��������
                Temp = GetChineseYearDays(i);
                offset = offset + Temp;
            }

            //�������Ӧ�����ĸ���
            leap = GetChineseLeapMonth(cy);
            if (leap != 0)
            {
                _cIsLeapYear = true;
            }
            else
            {
                _cIsLeapYear = false;
            }

            if (cm != leap)
            {
                //��ǰ���ڲ�������
                _cIsLeapMonth = false;
            }
            else
            {
                //ʹ���û�������Ƿ������·�
                _cIsLeapMonth = leapMonthFlag;
            }

            //����û������||�����·�С������
            if ((_cIsLeapYear == false) || (cm < leap))
            {
                for (i = 1; i < cm; i++)
                {
                    Temp = GetChineseMonthDays(cy, i); //�������������
                    offset = offset + Temp;
                }

                //��������Ƿ���������
                if (cd > GetChineseMonthDays(cy, cm))
                {
                    throw new Exception("���Ϸ���ũ������");
                }
                //���ϵ��µ�����
                offset = offset + cd;
            }

                //�����꣬�Ҽ����·ݴ��ڻ��������
            else
            {
                for (i = 1; i < cm; i++)
                {
                    //�������������
                    Temp = GetChineseMonthDays(cy, i);
                    offset = offset + Temp;
                }

                //�����´�������
                if (cm > leap)
                {
                    Temp = GetChineseLeapMonthDays(cy); //������������
                    offset = offset + Temp; //������������

                    if (cd > GetChineseMonthDays(cy, cm))
                    {
                        throw new Exception("���Ϸ���ũ������");
                    }
                    offset = offset + cd;
                }

                    //�����µ�������
                else
                {
                    //�����Ҫ����������£���Ӧ���ȼ��������¶�Ӧ����ͨ�µ�����
                    if (_cIsLeapMonth) //������Ϊ����
                    {
                        Temp = GetChineseMonthDays(cy, cm); //�������������
                        offset = offset + Temp;
                    }

                    if (cd > GetChineseLeapMonthDays(cy))
                    {
                        throw new Exception("���Ϸ���ũ������");
                    }
                    offset = offset + cd;
                }
            }
            _date = MinDay.AddDays(offset);
        }

        #endregion

        #endregion

        #region ˽�к���

        #region GetChineseMonthDays

        /// <summary>
        ///     //����ũ��y��m�µ�������
        /// </summary>
        private int GetChineseMonthDays(int year, int month)
        {
            if (BitTest32((LunarDateArray[year - MinYear] & 0x0000FFFF), (16 - month)))
            {
                return 30;
            }
            else
            {
                return 29;
            }
        }

        #endregion

        #region GetChineseLeapMonth

        /// <summary>
        ///     ����ũ�� y�����ĸ��� 1-12 , û�򴫻� 0
        /// </summary>
        private int GetChineseLeapMonth(int year)
        {
            return LunarDateArray[year - MinYear] & 0xF;
        }

        #endregion

        #region GetChineseLeapMonthDays

        /// <summary>
        ///     ����ũ��y�����µ�����
        /// </summary>
        private int GetChineseLeapMonthDays(int year)
        {
            if (GetChineseLeapMonth(year) != 0)
            {
                if ((LunarDateArray[year - MinYear] & 0x10000) != 0)
                {
                    return 30;
                }
                else
                {
                    return 29;
                }
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region GetChineseYearDays

        /// <summary>
        ///     ȡũ����һ�������
        /// </summary>
        private int GetChineseYearDays(int year)
        {
            int i, f, sumDay, info;

            sumDay = 348; //29��*12����
            i = 0x8000;
            info = LunarDateArray[year - MinYear] & 0x0FFFF;

            //����12�������ж�����Ϊ30��
            for (int m = 0; m < 12; m++)
            {
                f = info & i;
                if (f != 0)
                {
                    sumDay++;
                }
                i = i >> 1;
            }
            return sumDay + GetChineseLeapMonthDays(year);
        }

        #endregion

        #region GetChineseHour

        /// <summary>
        ///     ��õ�ǰʱ���ʱ��
        /// </summary>
        private string GetChineseHour(DateTime dt)
        {
            int _hour, _minute, offset, i;
            int indexGan;
            string ganHour, zhiHour;
            string tmpGan;

            //����ʱ���ĵ�֧
            _hour = dt.Hour; //��õ�ǰʱ��Сʱ
            _minute = dt.Minute; //��õ�ǰʱ�����

            if (_minute != 0) _hour += 1;
            offset = _hour/2;
            if (offset >= 12) offset = 0;
            //zhiHour = zhiStr[offset].ToString();

            //�������
            TimeSpan ts = _date - GanZhiStartDay;
            i = ts.Days%60;

            //ganStr[i % 10] Ϊ�յ����,(n*2-1) %10�ó���֧��Ӧ,n��1��ʼ
            indexGan = ((i%10 + 1)*2 - 1)%10 - 1;

            tmpGan = ganStr.Substring(indexGan) + ganStr.Substring(0, indexGan + 2); //����12λ
            //ganHour = ganStr[((i % 10 + 1) * 2 - 1) % 10 - 1].ToString();

            return tmpGan[offset].ToString() + zhiStr[offset].ToString();
        }

        #endregion

        #region CheckDateLimit

        /// <summary>
        ///     ��鹫�������Ƿ����Ҫ��
        /// </summary>
        private void CheckDateLimit(DateTime dt)
        {
            if ((dt < MinDay) || (dt > MaxDay))
            {
                throw new Exception("������ת��������");
            }
        }

        #endregion

        #region CheckChineseDateLimit

        /// <summary>
        ///     ���ũ�������Ƿ����
        /// </summary>
        private void CheckChineseDateLimit(int year, int month, int day, bool leapMonth)
        {
            if ((year < MinYear) || (year > MaxYear))
            {
                throw new Exception("�Ƿ�ũ������");
            }
            if ((month < 1) || (month > 12))
            {
                throw new Exception("�Ƿ�ũ������");
            }
            if ((day < 1) || (day > 30)) //�й��������30��
            {
                throw new Exception("�Ƿ�ũ������");
            }
            int leap = GetChineseLeapMonth(year); // �������Ӧ�����ĸ���
            if (leapMonth && (month != leap))
            {
                throw new Exception("�Ƿ�ũ������");
            }
        }

        #endregion

        #region ConvertNumToChineseNum

        /// <summary>
        ///     ��0-9ת�ɺ�����ʽ
        /// </summary>
        private string ConvertNumToChineseNum(char n)
        {
            if ((n < '0') || (n > '9')) return "";
            switch (n)
            {
                case '0':
                    return HZNum[0].ToString();
                case '1':
                    return HZNum[1].ToString();
                case '2':
                    return HZNum[2].ToString();
                case '3':
                    return HZNum[3].ToString();
                case '4':
                    return HZNum[4].ToString();
                case '5':
                    return HZNum[5].ToString();
                case '6':
                    return HZNum[6].ToString();
                case '7':
                    return HZNum[7].ToString();
                case '8':
                    return HZNum[8].ToString();
                case '9':
                    return HZNum[9].ToString();
                default:
                    return "";
            }
        }

        #endregion

        #region BitTest32

        /// <summary>
        ///     ����ĳλ�Ƿ�Ϊ��
        /// </summary>
        private bool BitTest32(int num, int bitpostion)
        {
            if ((bitpostion > 31) || (bitpostion < 0))
                throw new Exception("Error Param: bitpostion[0-31]:" + bitpostion.ToString());

            int bit = 1 << bitpostion;

            if ((num & bit) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region ConvertDayOfWeek

        /// <summary>
        ///     �����ڼ�ת�����ֱ�ʾ
        /// </summary>
        private int ConvertDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return 1;
                case DayOfWeek.Monday:
                    return 2;
                case DayOfWeek.Tuesday:
                    return 3;
                case DayOfWeek.Wednesday:
                    return 4;
                case DayOfWeek.Thursday:
                    return 5;
                case DayOfWeek.Friday:
                    return 6;
                case DayOfWeek.Saturday:
                    return 7;
                default:
                    return 0;
            }
        }

        #endregion

        #region CompareWeekDayHoliday

        /// <summary>
        ///     �Ƚϵ����ǲ���ָ���ĵ��ܼ�
        /// </summary>
        private bool CompareWeekDayHoliday(DateTime date, int month, int week, int day)
        {
            bool ret = false;

            if (date.Month == month) //�·���ͬ
            {
                if (ConvertDayOfWeek(date.DayOfWeek) == day) //���ڼ���ͬ
                {
                    var firstDay = new DateTime(date.Year, date.Month, 1); //���ɵ��µ�һ��
                    int i = ConvertDayOfWeek(firstDay.DayOfWeek);
                    int firWeekDays = 7 - ConvertDayOfWeek(firstDay.DayOfWeek) + 1; //�����һ��ʣ������

                    if (i > day)
                    {
                        if ((week - 1)*7 + day + firWeekDays == date.Day)
                        {
                            ret = true;
                        }
                    }
                    else
                    {
                        if (day + firWeekDays + (week - 2)*7 == date.Day)
                        {
                            ret = true;
                        }
                    }
                }
            }

            return ret;
        }

        #endregion

        #endregion

        #region  ����

        #region ����

        #region newCalendarHoliday

        /// <summary>
        ///     �����й�ũ������
        /// </summary>
        public string newCalendarHoliday
        {
            get
            {
                string tempStr = "";
                if (_cIsLeapMonth == false) //���²��������
                {
                    foreach (LunarHolidayStruct lh in lHolidayInfo)
                    {
                        if ((lh.Month == _cMonth) && (lh.Day == _cDay))
                        {
                            tempStr = lh.HolidayName;
                            break;
                        }
                    }

                    //�Գ�Ϧ�����ر���
                    if (_cMonth == 12)
                    {
                        int i = GetChineseMonthDays(_cYear, 12); //���㵱��ũ��12�µ�������
                        if (_cDay == i) //���Ϊ���һ��
                        {
                            tempStr = "��Ϧ";
                        }
                    }
                }
                return tempStr;
            }
        }

        #endregion

        #region WeekDayHoliday

        /// <summary>
        ///     ��ĳ�µڼ��ܵڼ��ռ���Ľ���
        /// </summary>
        public string WeekDayHoliday
        {
            get
            {
                string tempStr = "";
                foreach (WeekHolidayStruct wh in wHolidayInfo)
                {
                    if (CompareWeekDayHoliday(_date, wh.Month, wh.WeekAtMonth, wh.WeekDay))
                    {
                        tempStr = wh.HolidayName;
                        break;
                    }
                }
                return tempStr;
            }
        }

        #endregion

        #region DateHoliday

        /// <summary>
        ///     �������ռ���Ľ���
        /// </summary>
        public string DateHoliday
        {
            get
            {
                string tempStr = "";

                foreach (SolarHolidayStruct sh in sHolidayInfo)
                {
                    if ((sh.Month == _date.Month) && (sh.Day == _date.Day))
                    {
                        tempStr = sh.HolidayName;
                        break;
                    }
                }
                return tempStr;
            }
        }

        #endregion

        #endregion

        #region ��������

        #region Date

        /// <summary>
        ///     ȡ��Ӧ�Ĺ�������
        /// </summary>
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        #endregion

        #region WeekDay

        /// <summary>
        ///     ȡ���ڼ�
        /// </summary>
        public DayOfWeek WeekDay
        {
            get { return _date.DayOfWeek; }
        }

        #endregion

        #region WeekDayStr

        /// <summary>
        ///     �ܼ����ַ�
        /// </summary>
        public string WeekDayStr
        {
            get
            {
                switch (_date.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        return "������";
                    case DayOfWeek.Monday:
                        return "����һ";
                    case DayOfWeek.Tuesday:
                        return "���ڶ�";
                    case DayOfWeek.Wednesday:
                        return "������";
                    case DayOfWeek.Thursday:
                        return "������";
                    case DayOfWeek.Friday:
                        return "������";
                    default:
                        return "������";
                }
            }
        }

        #endregion

        #region DateString

        /// <summary>
        ///     �����������ı�ʾ�� ��һ�ž���������һ��
        /// </summary>
        public string DateString
        {
            get { return "��Ԫ" + _date.ToLongDateString(); }
        }

        #endregion

        #region IsLeapYear

        /// <summary>
        ///     ��ǰ�Ƿ�������
        /// </summary>
        public bool IsLeapYear
        {
            get { return DateTime.IsLeapYear(_date.Year); }
        }

        #endregion

        #region ChineseConstellation

        /// <summary>
        ///     28���޼���
        /// </summary>
        public string ChineseConstellation
        {
            get
            {
                int offset = 0;
                int modStarDay = 0;

                TimeSpan ts = _date - ChineseConstellationReferDay;
                offset = ts.Days;
                modStarDay = offset%28;
                return (modStarDay >= 0 ? _chineseConstellationName[modStarDay] : _chineseConstellationName[27 + modStarDay]);
            }
        }

        #endregion

        #region ChineseHour

        /// <summary>
        ///     ʱ��
        /// </summary>
        public string ChineseHour
        {
            get { return GetChineseHour(_datetime); }
        }

        #endregion

        #endregion

        #region ũ������

        #region IsChineseLeapMonth

        /// <summary>
        ///     �Ƿ�����
        /// </summary>
        public bool IsChineseLeapMonth
        {
            get { return _cIsLeapMonth; }
        }

        #endregion

        #region IsChineseLeapYear

        /// <summary>
        ///     �����Ƿ�������
        /// </summary>
        public bool IsChineseLeapYear
        {
            get { return _cIsLeapYear; }
        }

        #endregion

        #region ChineseDay

        /// <summary>
        ///     ũ����
        /// </summary>
        public int ChineseDay
        {
            get { return _cDay; }
        }

        #endregion

        #region ChineseDayString

        /// <summary>
        ///     ũ�������ı�ʾ
        /// </summary>
        public string ChineseDayString
        {
            get
            {
                switch (_cDay)
                {
                    case 0:
                        return "";
                    case 10:
                        return "��ʮ";
                    case 20:
                        return "��ʮ";
                    case 30:
                        return "��ʮ";
                    default:
                        return nStr2[(_cDay/10)].ToString() + nStr1[_cDay%10].ToString();
                }
            }
        }

        #endregion

        #region ChineseMonth

        /// <summary>
        ///     ũ�����·�
        /// </summary>
        public int ChineseMonth
        {
            get { return _cMonth; }
        }

        #endregion

        #region ChineseMonthString

        /// <summary>
        ///     ũ���·��ַ���
        /// </summary>
        public string ChineseMonthString
        {
            get { return _monthString[_cMonth]; }
        }

        #endregion

        #region ChineseYear

        /// <summary>
        ///     ȡũ�����
        /// </summary>
        public int ChineseYear
        {
            get { return _cYear; }
        }

        #endregion

        #region ChineseYearString

        /// <summary>
        ///     ȡũ�����ַ����磬һ�ž�����
        /// </summary>
        public string ChineseYearString
        {
            get
            {
                string tempStr = "";
                string num = _cYear.ToString();
                for (int i = 0; i < 4; i++)
                {
                    tempStr += ConvertNumToChineseNum(num[i]);
                }
                return tempStr + "��";
            }
        }

        #endregion

        #region ChineseDateString

        /// <summary>
        ///     ȡũ�����ڱ�ʾ����ũ��һ�ž��������³���
        /// </summary>
        public string ChineseDateString
        {
            get
            {
                if (_cIsLeapMonth)
                {
                    return "ũ��" + ChineseYearString + "��" + ChineseMonthString + ChineseDayString;
                }
                else
                {
                    return "ũ��" + ChineseYearString + ChineseMonthString + ChineseDayString;
                }
            }
        }

        #endregion

        #region ChineseTwentyFourDay

        /// <summary>
        ///     �����������ʮ�Ľ���,��ʮ�Ľ����ǰ�����ת������ģ����������������
        /// </summary>
        /// <remarks>
        ///     �����Ķ��������֡��Ŵ��������õĳ�Ϊ"����"������ʱ���һ��ȷ�Ϊ24�ݣ�
        ///     ÿһ����ƽ����15�����࣬�����ֳ�"ƽ��"���ִ�ũ�����õĳ�Ϊ"����"����
        ///     �������ڹ���ϵ�λ��Ϊ��׼��һ��360�㣬������֮�����15�㡣���ڶ���ʱ��
        ///     ��λ�ڽ��յ㸽�����˶��ٶȽϿ죬���̫���ڻƵ����ƶ�15���ʱ�䲻��15�졣
        ///     ����ǰ�����������෴��̫���ڻƵ����ƶ�������һ��������16��֮�ࡣ����
        ///     ����ʱ���Ա�֤���������ֱ�Ȼ����ҹƽ�ֵ������졣
        /// </remarks>
        public string ChineseTwentyFourDay
        {
            get
            {
                var baseDateAndTime = new DateTime(1900, 1, 6, 2, 5, 0); //#1/6/1900 2:05:00 AM#
                DateTime newDate;
                double num;
                int y;
                string tempStr = "";

                y = _date.Year;

                for (int i = 1; i <= 24; i++)
                {
                    num = 525948.76*(y - 1900) + sTermInfo[i - 1];

                    newDate = baseDateAndTime.AddMinutes(num); //�����Ӽ���
                    if (newDate.DayOfYear == _date.DayOfYear)
                    {
                        tempStr = SolarTerm[i - 1];
                        break;
                    }
                }
                return tempStr;
            }
        }

        //��ǰ����ǰһ���������
        public string ChineseTwentyFourPrevDay
        {
            get
            {
                var baseDateAndTime = new DateTime(1900, 1, 6, 2, 5, 0); //#1/6/1900 2:05:00 AM#
                DateTime newDate;
                double num;
                int y;
                string tempStr = "";

                y = _date.Year;

                for (int i = 24; i >= 1; i--)
                {
                    num = 525948.76*(y - 1900) + sTermInfo[i - 1];

                    newDate = baseDateAndTime.AddMinutes(num); //�����Ӽ���

                    if (newDate.DayOfYear < _date.DayOfYear)
                    {
                        tempStr = string.Format("{0}[{1}]", SolarTerm[i - 1], newDate.ToString("yyyy-MM-dd"));
                        break;
                    }
                }

                return tempStr;
            }
        }

        //��ǰ���ں�һ���������
        public string ChineseTwentyFourNextDay
        {
            get
            {
                var baseDateAndTime = new DateTime(1900, 1, 6, 2, 5, 0); //#1/6/1900 2:05:00 AM#
                DateTime newDate;
                double num;
                int y;
                string tempStr = "";

                y = _date.Year;

                for (int i = 1; i <= 24; i++)
                {
                    num = 525948.76*(y - 1900) + sTermInfo[i - 1];

                    newDate = baseDateAndTime.AddMinutes(num); //�����Ӽ���

                    if (newDate.DayOfYear > _date.DayOfYear)
                    {
                        tempStr = string.Format("{0}[{1}]", SolarTerm[i - 1], newDate.ToString("yyyy-MM-dd"));
                        break;
                    }
                }
                return tempStr;
            }
        }

        #endregion

        #endregion

        #region ����

        /// <summary>
        ///     ����ָ�����ڵ��������
        /// </summary>
        public string Constellation
        {
            get
            {
                int index = 0;
                int y, m, d;
                y = _date.Year;
                m = _date.Month;
                d = _date.Day;
                y = m*100 + d;

                if (((y >= 321) && (y <= 419)))
                {
                    index = 0;
                }
                else if ((y >= 420) && (y <= 520))
                {
                    index = 1;
                }
                else if ((y >= 521) && (y <= 620))
                {
                    index = 2;
                }
                else if ((y >= 621) && (y <= 722))
                {
                    index = 3;
                }
                else if ((y >= 723) && (y <= 822))
                {
                    index = 4;
                }
                else if ((y >= 823) && (y <= 922))
                {
                    index = 5;
                }
                else if ((y >= 923) && (y <= 1022))
                {
                    index = 6;
                }
                else if ((y >= 1023) && (y <= 1121))
                {
                    index = 7;
                }
                else if ((y >= 1122) && (y <= 1221))
                {
                    index = 8;
                }
                else if ((y >= 1222) || (y <= 119))
                {
                    index = 9;
                }
                else if ((y >= 120) && (y <= 218))
                {
                    index = 10;
                }
                else if ((y >= 219) && (y <= 320))
                {
                    index = 11;
                }
                else
                {
                    index = 0;
                }

                return _constellationName[index];
            }
        }

        #endregion

        #region ����

        #region Animal

        /// <summary>
        ///     ���������������ע����Ȼ��������ũ����������ģ�����Ŀǰ��ʵ��ʹ�����ǰ������������
        ///     ����Ϊ1,��������
        /// </summary>
        public int Animal
        {
            get
            {
                int offset = _date.Year - AnimalStartYear;
                return (offset%12) + 1;
            }
        }

        #endregion

        #region AnimalString

        /// <summary>
        ///     ȡ�����ַ���
        /// </summary>
        public string AnimalString
        {
            get
            {
                int offset = _date.Year - AnimalStartYear; //��������
                //int offset = this._cYear - AnimalStartYear;��ũ������
                return animalStr[offset%12].ToString();
            }
        }

        #endregion

        #endregion

        #region ��ɵ�֧

        #region GanZhiYearString

        /// <summary>
        ///     ȡũ����ĸ�֧��ʾ���� �ҳ���
        /// </summary>
        public string GanZhiYearString
        {
            get
            {
                string tempStr;
                int i = (_cYear - GanZhiStartYear)%60; //�����֧
                tempStr = ganStr[i%10].ToString() + zhiStr[i%12].ToString() + "��";
                return tempStr;
            }
        }

        #endregion

        #region GanZhiMonthString

        /// <summary>
        ///     ȡ��֧���±�ʾ�ַ�����ע��ũ�������²��Ǹ�֧
        /// </summary>
        public string GanZhiMonthString
        {
            get
            {
                //ÿ���µĵ�֧���ǹ̶���,�������Ǵ����¿�ʼ
                int zhiIndex;
                string zhi;
                if (_cMonth > 10)
                {
                    zhiIndex = _cMonth - 10;
                }
                else
                {
                    zhiIndex = _cMonth + 2;
                }
                zhi = zhiStr[zhiIndex - 1].ToString();

                //���ݵ���ĸ�֧��ĸ��������¸ɵĵ�һ��
                int ganIndex = 1;
                string gan;
                int i = (_cYear - GanZhiStartYear)%60; //�����֧
                switch (i%10)
                {
                        #region ...

                    case 0: //��
                        ganIndex = 3;
                        break;
                    case 1: //��
                        ganIndex = 5;
                        break;
                    case 2: //��
                        ganIndex = 7;
                        break;
                    case 3: //��
                        ganIndex = 9;
                        break;
                    case 4: //��
                        ganIndex = 1;
                        break;
                    case 5: //��
                        ganIndex = 3;
                        break;
                    case 6: //��
                        ganIndex = 5;
                        break;
                    case 7: //��
                        ganIndex = 7;
                        break;
                    case 8: //��
                        ganIndex = 9;
                        break;
                    case 9: //��
                        ganIndex = 1;
                        break;

                        #endregion
                }
                gan = ganStr[(ganIndex + _cMonth - 2)%10].ToString();

                return gan + zhi + "��";
            }
        }

        #endregion

        #region GanZhiDayString

        /// <summary>
        ///     ȡ��֧�ձ�ʾ��
        /// </summary>
        public string GanZhiDayString
        {
            get
            {
                int i, offset;
                TimeSpan ts = _date - GanZhiStartDay;
                offset = ts.Days;
                i = offset%60;
                return ganStr[i%10].ToString() + zhiStr[i%12].ToString() + "��";
            }
        }

        #endregion

        #region GanZhiDateString

        /// <summary>
        ///     ȡ��ǰ���ڵĸ�֧��ʾ���� �������ҳ��±�����
        /// </summary>
        public string GanZhiDateString
        {
            get { return GanZhiYearString + GanZhiMonthString + GanZhiDayString; }
        }

        #endregion

        #endregion

        #endregion
    }
}