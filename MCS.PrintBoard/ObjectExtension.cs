using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MCS.PrintBoard;

public static class ObjectExtension
{
	public static bool IsNullOrEmpty(this object item)
	{
		try
		{
			if (item != null)
			{
				return item.ToString().Trim() == "";
			}
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static void Each<T>(this IEnumerable<T> list, Action<T> action)
	{
		foreach (T elem in list)
		{
			action(elem);
		}
	}

	public static bool ContainsAll<T>(this IList<T> list, IEnumerable<T> subList)
	{
		foreach (T t in subList)
		{
			if (!list.Contains(t))
			{
				return false;
			}
		}
		return true;
	}

	public static T[] EmptyArray<T>()
	{
		return new T[0];
	}

	public static T ChangeType<T>(this object value, IFormatProvider provider) where T : IConvertible
	{
		return (T)Convert.ChangeType(value, typeof(T), provider);
	}

	public static T ChangeType<T>(this object value) where T : IConvertible
	{
		return value.ChangeType<T>(CultureInfo.CurrentCulture);
	}

	public static bool SafeToBoolean(this object obj)
	{
		if (obj == null || obj == DBNull.Value || obj.ToString().Trim().Length == 0)
		{
			return false;
		}
		string s = obj.ToString().ToUpper();
		if (s.Equals("TRUE") || s.Equals("Y") || s.Equals("1"))
		{
			return true;
		}
		return false;
	}

	public static double SafeToDouble(this object obj)
	{
		if (obj == null || obj == DBNull.Value || obj.ToString().Trim().Length == 0)
		{
			return 0.0;
		}
		return Convert.ToDouble(obj, CultureInfo.CurrentCulture);
	}

	public static int SafeToInt32(this object obj)
	{
		if (obj == null || obj == DBNull.Value || obj.ToString().Trim().Length == 0)
		{
			return 0;
		}
		return Convert.ToInt32(obj, CultureInfo.CurrentCulture);
	}

	public static decimal SafeToDecimal(this object obj)
	{
		if (obj == null || obj == DBNull.Value || obj.ToString().Trim().Length == 0)
		{
			return 0m;
		}
		if (decimal.TryParse(obj.ToString(), out var val))
		{
			return val;
		}
		return 0m;
	}

	public static string SafeToString(this object obj, bool trim = false, bool removeSpecial = false)
	{
		string result = string.Empty;
		if (obj == null || obj == DBNull.Value)
		{
			return result;
		}
		result = obj.ToString();
		if (removeSpecial)
		{
			result = Regex.Replace(result, "[^a-zA-Z0-9%._]", string.Empty);
		}
		if (trim)
		{
			result = result.Trim();
		}
		return result;
	}

	public static string SafeFormat(string message, params object[] args)
	{
		if (args == null || args.Length == 0)
		{
			return message;
		}
		try
		{
			return string.Format(CultureInfo.InvariantCulture, message, args);
		}
		catch (Exception ex)
		{
			return message + " (System error: failed to format message. " + ex.Message + ")";
		}
	}
}
