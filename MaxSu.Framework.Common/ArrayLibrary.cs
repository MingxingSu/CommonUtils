using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace MaxSu.Framework.Common
{
    public static class ArrayLibrary
    {
        public static Array AddItemToArray(Array arrOriginal, object objNewItem, int intIndex = 0)
        {
            Array array;
            try
            {
                int num;
                if (intIndex > arrOriginal.Length)
                {
                    intIndex = arrOriginal.Length;
                }
                if (intIndex < 0)
                {
                    intIndex = 0;
                }
                Array instance = Array.CreateInstance(typeof (object), (arrOriginal.Length + 1));
                int num2 = intIndex - 1;
                for (num = 0; num <= num2; num++)
                {
                    NewLateBinding.LateIndexSet(instance,
                                                new[]
                                                    {
                                                        num,
                                                        RuntimeHelpers.GetObjectValue(
                                                            NewLateBinding.LateIndexGet(arrOriginal, new object[] {num},
                                                                                        null))
                                                    }, null);
                }
                NewLateBinding.LateIndexSet(instance, new[] {intIndex, RuntimeHelpers.GetObjectValue(objNewItem)}, null);
                int num3 = arrOriginal.Length - 1;
                for (num = intIndex; num <= num3; num++)
                {
                    NewLateBinding.LateIndexSet(instance,
                                                new[]
                                                    {
                                                        num + 1,
                                                        RuntimeHelpers.GetObjectValue(
                                                            NewLateBinding.LateIndexGet(arrOriginal, new object[] {num},
                                                                                        null))
                                                    }, null);
                }
                array = instance;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return array;
        }

        public static Array AppendItemToArray(Array arrOriginal, object objNewItem)
        {
            Array array;
            try
            {
                Array instance = Array.CreateInstance(typeof (object), (arrOriginal.Length + 1));
                int num2 = arrOriginal.Length - 1;
                for (int i = 0; i <= num2; i++)
                {
                    NewLateBinding.LateIndexSet(instance,
                                                new[]
                                                    {
                                                        i,
                                                        RuntimeHelpers.GetObjectValue(
                                                            NewLateBinding.LateIndexGet(arrOriginal, new object[] {i},
                                                                                        null))
                                                    }, null);
                }
                NewLateBinding.LateIndexSet(instance,
                                            new[] {instance.Length - 1, RuntimeHelpers.GetObjectValue(objNewItem)}, null);
                array = instance;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return array;
        }

        public static Array CreateArray(params object[] objObjects)
        {
            return objObjects;
        }

        public static object Get2DArrayValue(Array arr, object objKey, object objDefaultValue = null)
        {
            object obj2;
            try
            {
                long upperBound = arr.GetUpperBound(0);
                for (long i = arr.GetLowerBound(0); i <= upperBound; i += 1L)
                {
                    if (
                        Operators.ConditionalCompareObjectEqual(
                            NewLateBinding.LateIndexGet(arr, new object[] {i, 0}, null), objKey, false))
                    {
                        return NewLateBinding.LateIndexGet(arr, new object[] {i, 1}, null);
                    }
                }
                obj2 = objDefaultValue;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return obj2;
        }

        public static object GetArrayValue(Array objArray, int intIndex, object objDefaultValue = null)
        {
            object obj2;
            try
            {
                obj2 = NewLateBinding.LateIndexGet(objArray, new object[] {intIndex}, null);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                obj2 = objDefaultValue;
                ProjectData.ClearProjectError();
            }
            return obj2;
        }

        public static bool IsIn(object objValue, object[] objValues)
        {
            bool flag;
            try
            {
                IEnumerator enumerator = null;
                try
                {
                    enumerator = ((IEnumerable) objValue).GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        object objectValue = RuntimeHelpers.GetObjectValue(enumerator.Current);
                        if (Operators.ConditionalCompareObjectEqual(objValue, objectValue, false))
                        {
                            return true;
                        }
                    }
                }
                finally
                {
                    if (enumerator is IDisposable)
                    {
                        (enumerator as IDisposable).Dispose();
                    }
                }
                flag = false;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return flag;
        }

        public static bool IsInArray(object objValue, Array arrArray)
        {
            bool flag;
            try
            {
                int num2 = arrArray.Length - 1;
                for (int i = 0; i <= num2; i++)
                {
                    if (Operators.ConditionalCompareObjectEqual(arrArray.GetValue(i), objValue, false))
                    {
                        return true;
                    }
                }
                flag = false;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return flag;
        }

        public static Array JsArray2SystemArray(string strJavaScriptArray, long lngLevel = 1L)
        {
            Array array;
            try
            {
                int num;
                Array array2;
                string str = "#" + new string('@', (int) lngLevel) + "#";
                string str2 = "#" + new string('@', (int) (lngLevel + 1L)) + "#";
                if (Strings.InStr(strJavaScriptArray, str, CompareMethod.Text) > 0)
                {
                    string[] strArray = Strings.Split(strJavaScriptArray, str, -1, CompareMethod.Binary);
                    array2 = Array.CreateInstance(typeof (object), strArray.Length);
                    int num2 = strArray.Length - 1;
                    for (num = 0; num <= num2; num++)
                    {
                        if (strArray[num].Length != 0)
                        {
                            if (Strings.InStr(strArray[num], str2, CompareMethod.Text) > 0)
                            {
                                array2.SetValue(JsArray2SystemArray(strArray[num], lngLevel + 1L), num);
                            }
                            else
                            {
                                array2.SetValue(strArray[num], num);
                            }
                        }
                        else
                        {
                            array2.SetValue(DBNull.Value, num);
                        }
                    }
                }
                else
                {
                    Array instance = Array.CreateInstance(typeof (object), 1);
                    if (!Information.IsDBNull(strJavaScriptArray) && (strJavaScriptArray != ""))
                    {
                        instance.SetValue(strJavaScriptArray, 0);
                    }
                    else
                    {
                        instance.SetValue(DBNull.Value, 0);
                    }
                    array2 = Array.CreateInstance(typeof (object), instance.Length);
                    int num3 = instance.Length - 1;
                    for (num = 0; num <= num3; num++)
                    {
                        if (
                            !Information.IsDBNull(
                                RuntimeHelpers.GetObjectValue(NewLateBinding.LateIndexGet(instance, new object[] {num},
                                                                                          null))) &&
                            Operators.ConditionalCompareObjectNotEqual(
                                NewLateBinding.LateIndexGet(instance, new object[] {num}, null), "", false))
                        {
                            if (
                                Strings.InStr(
                                    Conversions.ToString(NewLateBinding.LateIndexGet(instance, new object[] {num}, null)),
                                    str2, CompareMethod.Text) > 0)
                            {
                                array2.SetValue(
                                    JsArray2SystemArray(
                                        Conversions.ToString(NewLateBinding.LateIndexGet(instance, new object[] {num},
                                                                                         null)), lngLevel + 1L), num);
                            }
                            else
                            {
                                array2.SetValue(
                                    RuntimeHelpers.GetObjectValue(NewLateBinding.LateIndexGet(instance,
                                                                                              new object[] {num}, null)),
                                    num);
                            }
                        }
                        else
                        {
                            array2.SetValue(DBNull.Value, num);
                        }
                    }
                }
                array = array2;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return array;
        }

        public static void Nullify(ref Array arrParameters, ref Array arrNullValues)
        {
            try
            {
                int num2 = arrParameters.Length - 1;
                for (int i = 0; i <= num2; i++)
                {
                    if (arrParameters.GetValue(i) == arrNullValues.GetValue(i))
                    {
                        arrParameters.SetValue(DBNull.Value, i);
                    }
                }
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
        }

        public static void Set2DArrayValue(ref Array arr, object objKey, object objValue)
        {
            try
            {
                bool flag = false;
                long num;
                long upperBound = arr.GetUpperBound(0);
                for (num = arr.GetLowerBound(0); num <= upperBound; num += 1L)
                {
                    if (
                        Operators.ConditionalCompareObjectEqual(
                            NewLateBinding.LateIndexGet(arr, new object[] {num, 0}, null), objKey, false))
                    {
                        NewLateBinding.LateIndexSet(arr, new[] {num, 1, RuntimeHelpers.GetObjectValue(objValue)}, null);
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    long num2;
                    if (arr.GetUpperBound(0) == -1)
                    {
                        num2 = 1L;
                    }
                    else
                    {
                        num2 = arr.GetUpperBound(0) + 1;
                    }
                    Array array = Array.CreateInstance(typeof (object), new[] {num2, 2L});
                    long num4 = arr.GetUpperBound(0);
                    for (num = arr.GetLowerBound(0); num <= num4; num += 1L)
                    {
                        array.SetValue(RuntimeHelpers.GetObjectValue(arr.GetValue(num, 0L)), num, 0L);
                        array.SetValue(RuntimeHelpers.GetObjectValue(arr.GetValue(num, 1L)), num, 1L);
                    }
                    array.SetValue(RuntimeHelpers.GetObjectValue(objKey), array.GetUpperBound(0), 0);
                    array.SetValue(RuntimeHelpers.GetObjectValue(objValue), array.GetUpperBound(0), 1);
                }
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
        }

        public static string SystemArray2JsArray(ref Array arSystemArray)
        {
            string str2;
            try
            {
                string str;
                if (Information.IsNothing(arSystemArray) || (arSystemArray.GetLength(0) == 0))
                {
                    str = "";
                }
                else
                {
                    str = arSystemArray.GetValue(0).ToString();
                    int num2 = arSystemArray.GetLength(0) - 1;
                    for (int i = 1; i <= num2; i++)
                    {
                        str = str + "#@#" + arSystemArray.GetValue(i);
                    }
                }
                str2 = str;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return str2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicParameters"></param>
        /// <returns></returns>
        public static Array CreateArrayFromDictionary(Dictionary<string, object> dicParameters)
        {
            Array arrResults = null;

            if (dicParameters == null)
                return ArrayLibrary.CreateArray();

            foreach (object objValue in dicParameters.Values)
            {
                object objToAdd = objValue.ToString();

                if (objValue == DBNull.Value)
                    objToAdd = DBNull.Value;

                if (arrResults == null)
                {
                    arrResults = ArrayLibrary.CreateArray(objToAdd);
                }
                else
                {
                    arrResults = ArrayLibrary.AppendItemToArray(arrResults, objToAdd);
                }
            }

            return arrResults;
        }
    }
}