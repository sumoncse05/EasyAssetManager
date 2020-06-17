using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EasyAssetManagerCore.Model.CommonModel
{
    public static class App
    {

        public static DateTime StringToDateTime(string stringDate)
        {
            try
            {

                stringDate = Regex.Replace(stringDate, @"\s+", " ");
                var dateTime = stringDate.Split(' ');
                var dates = dateTime[0].Split('-');
                var times = dateTime[1].Split(':');

                var date = new DateTime(Convert.ToInt32(dates[0]), Convert.ToInt32(dates[1]), Convert.ToInt32(dates[2]), Convert.ToInt32(times[0]), Convert.ToInt32(times[1]), 0);

                return date;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DateTime StringToDate(string stringDate)
        {
            try
            {

                stringDate = Regex.Replace(stringDate, @"\s+", " ");
                var dateTime = stringDate.Split(' ');
                var dates = dateTime[0].Split('-');


                var date = new DateTime(Convert.ToInt32(dates[0]), Convert.ToInt32(dates[1]), Convert.ToInt32(dates[2]));

                return date;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string DateTimeToString(this DateTime dateTime, bool returnEmptyonDefault = false)
        {
            try
            {
                if (returnEmptyonDefault && dateTime.Year < 1900)
                {
                    return string.Empty;
                }
                else
                {
                    return dateTime.Year < 1900 ? string.Format("{0:yyyy-MM-dd   H:mm}", DateTime.Now) : string.Format("{0:yyyy-MM-dd   H:mm}", dateTime);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string DateToString(this DateTime dateTime, bool returnEmptyonDefault = false)
        {
            if (returnEmptyonDefault && dateTime.Year < 1900)
            {
                return string.Empty;
            }

            if (dateTime == DateTime.MinValue)
            {
                dateTime = DateTime.Now;
            }

            try
            {
                return string.Format("{0:yyyy-MM-dd}", dateTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IEnumerable<SelectListItem> SexSelectList(string sex = "")
        {
            if (string.IsNullOrWhiteSpace(sex)) { sex = ""; }
            sex = sex.ToLower();
            var items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Select Sex", Value = "" });
            items.Add(new SelectListItem { Text = "Male", Value = "M", Selected = sex.Equals("m") ? true : false });
            items.Add(new SelectListItem { Text = "Female", Value = "F", Selected = sex.Equals("f") ? true : false });
            return items;
        }

        public static IEnumerable<SelectListItem> MonthSelectList(string value = "")
        {

            var items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Select Month", Value = "" });
            items.Add(new SelectListItem { Text = "January", Value = "1" });
            items.Add(new SelectListItem { Text = "February", Value = "2" });
            items.Add(new SelectListItem { Text = "March", Value = "3" });
            items.Add(new SelectListItem { Text = "April", Value = "4" });
            items.Add(new SelectListItem { Text = "May", Value = "5" });
            items.Add(new SelectListItem { Text = "June", Value = "6" });
            items.Add(new SelectListItem { Text = "July", Value = "7" });
            items.Add(new SelectListItem { Text = "August", Value = "8" });
            items.Add(new SelectListItem { Text = "September", Value = "9" });
            items.Add(new SelectListItem { Text = "October", Value = "10" });
            items.Add(new SelectListItem { Text = "November", Value = "11" });
            items.Add(new SelectListItem { Text = "December", Value = "12" });

            if (value.IsNotNullOrEmpty())
            {
                items.Where(o => o.Value == value).FirstOrDefault().Selected = true;
            }

            return items;
        }

       
        public static DateTime GetMinValueOfADay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public static DateTime GetMaxValueOfADay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 998);

        }

        public static string ToFullDateTime(this DateTime date)
        {
            return $"{date.Year}-{date.Month}-{date.Day} {date.Hour:00}:{date.Minute:00}:{date.Second:00}.{date.Millisecond:000}";
        }

      
        public static DateTime FirstDayOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime FirstDayOfMonth(int month, int year)
        {
            return new DateTime(year, month, 1);
        }

        public static DateTime LastDayOfMonth(DateTime date)
        {
            int year = 0, month = 0;
            if (date.Month == 12)
            {
                month = 1;
                year = date.Year + 1;
            }
            else
            {
                month = date.Month + 1;
                year = date.Year;
            }
            return new DateTime(year, month, 1).AddDays(-1);
        }

        public static DateTime LastDayOfMonth(int month, int year)
        {
            if (month == 12)
            {
                month = 1;
                year = year + 1;
            }
            else
            {
                month = month + 1;
            }
            return new DateTime(year, month, 1).AddDays(-1);
        }

        public static T Success<T>(this T entity, string message = null)
        {
            var type = entity.GetType();

            PropertyInfo messageTypePI = type.GetProperty("MessageType");
            if (messageTypePI != null)
            {
                messageTypePI.SetValue(entity, Convert.ChangeType(MessageTypes.Success, messageTypePI.PropertyType), null);
            }
            PropertyInfo messageStringPI = type.GetProperty("MessageString");
            if (messageStringPI != null)
            {
                messageStringPI.SetValue(entity, Convert.ChangeType(message != null ? message : ApplicationConstant.GlobalSuccessMessage, messageStringPI.PropertyType), null);
            }

            return entity;
        }

        public static T Error<T>(this T entity, string message = null)
        {
            var type = entity.GetType();

            PropertyInfo messageTypePI = type.GetProperty("MessageType");
            if (messageTypePI != null)
            {
                messageTypePI.SetValue(entity, Convert.ChangeType(MessageTypes.Error, messageTypePI.PropertyType), null);
            }
            PropertyInfo messageStringPI = type.GetProperty("MessageString");
            if (messageStringPI != null)
            {
                messageStringPI.SetValue(entity, Convert.ChangeType(message != null ? message : ApplicationConstant.GlobalErrorMessage, messageStringPI.PropertyType), null);
            }

            return entity;
        }

        public static string GetColor(int index)
        {
            if (index > 4)
            {
                index = index % 4 == 0 ? 4 : index % 4;
            }

            var color = "purple";

            if (index == 1)
            {
                color = "blue";
            }
            else if (index == 2)
            {

                color = "red";
            }
            else if (index == 3)
            {
                color = "green";
            }

            return color;
        }
    }

    public static class StringExtension
    {

        public static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }
    }
}

