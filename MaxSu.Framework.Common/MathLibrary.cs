using System;
using Microsoft.VisualBasic.CompilerServices;

namespace MaxSu.Framework.Common
{
    public static class MathLibrary
    {
        public static decimal Round(decimal decNumber, int intDecimals)
        {
            decimal num;
            try
            {
                if (decimal.Compare(decNumber, decimal.Zero) < 0)
                {
                    return
                        new decimal(
                            -Math.Floor(((Convert.ToDouble(Math.Abs(decNumber))*Math.Pow(10.0, intDecimals)) + 0.5))/
                            Math.Pow(10.0, intDecimals));
                }
                num =
                    new decimal(Math.Floor(((Convert.ToDouble(decNumber)*Math.Pow(10.0, intDecimals)) + 0.5))/
                                Math.Pow(10.0, intDecimals));
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return num;
        }

        public static double Round(double dblNumber, int intDecimals)
        {
            double num;
            try
            {
                if (dblNumber < 0.0)
                {
                    return (-Math.Floor(((Math.Abs(dblNumber)*Math.Pow(10.0, intDecimals)) + 0.5))/
                            Math.Pow(10.0, intDecimals));
                }
                num = Math.Floor(((dblNumber*Math.Pow(10.0, intDecimals)) + 0.5))/Math.Pow(10.0, intDecimals);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return num;
        }

        public static decimal RoundUp(decimal d, int decimals)
        {
            decimal num;
            try
            {
                if (decimal.Compare(d, decimal.Zero) < 0)
                {
                    return
                        new decimal(((0L -
                                      Convert.ToInt64(((Convert.ToDouble(Math.Abs(d))*Math.Pow(10.0, decimals)) + 0.5))))/
                                    Math.Pow(10.0, decimals));
                }
                num =
                    new decimal((Convert.ToInt64(((Convert.ToDouble(d)*Math.Pow(10.0, decimals)) + 0.5)))/
                                Math.Pow(10.0, decimals));
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return num;
        }

        public static double RoundUp(double d, int decimals)
        {
            double num;
            try
            {
                if (d < 0.0)
                {
                    return (((0L - Convert.ToInt64(((Math.Abs(d)*Math.Pow(10.0, decimals)) + 0.5))))/
                            Math.Pow(10.0, decimals));
                }
                num = (Convert.ToInt64(((d*Math.Pow(10.0, decimals)) + 0.5)))/Math.Pow(10.0, decimals);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return num;
        }
    }
}