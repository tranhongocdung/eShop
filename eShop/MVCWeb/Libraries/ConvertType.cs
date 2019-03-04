﻿using System;
using System.Globalization;

namespace MVCWeb.Libraries
{
    public class ConvertType
    {
        public static Int16 ToInt16(object obj, Int16 defValue = -1)
        {
            try
            {
                return Convert.ToInt16(obj);
            }
            catch (Exception)
            {
                return defValue;
            }
        }
        public static int ToInt32(object obj, int defValue = -1)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch (Exception)
            {
                return defValue;
            }
        }
        public static Int64 ToInt64(object obj, Int64 defValue = -1)
        {
            try
            {
                return Convert.ToInt64(obj);
            }
            catch (Exception)
            {
                return defValue;
            }
        }
        public static int ToInt(object obj)
        {
            return ToInt32(obj);
        }
        public static bool ToBool(object obj)
        {
            try
            {
                return Convert.ToBoolean(obj);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static DateTime ToDateTime(object obj)
        {
            try
            {
                return Convert.ToDateTime(obj);
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }
        public static DateTime ToDateTime(string obj, string format = "dd/MM/yyyy")
        {
            try
            {
                return DateTime.ParseExact(obj, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }
    }
}
