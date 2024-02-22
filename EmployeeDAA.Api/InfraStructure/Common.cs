using LinqToDB.Common.Internal.Cache;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace EmployeeDAA.Api.InfraStructure
{
    public static class Common
    {
        public static List<SelectListItem> DropDownBindWithEnum(Type enumType)
        {
            return enumType.ToSelectListItems();
        }

        /// <summary>
        /// From enum type convert to SelectListItems
        /// </summary>
        /// <param name="enumType">Type of enum</param>
        /// <returns></returns>
        public static List<SelectListItem> ToSelectListItems(this Type enumType)
        {
            List<SelectListItem> items = new();
            foreach (Enum cur in Enum.GetValues(enumType))
            {
                items.Add(new SelectListItem()
                {
                    Text = cur.ToString(),
                    Value = GetEnumValue(cur)
                });
            }
            return items;
        }

        #region :: Get Enum value ::
        public static string GetEnumValue(this Enum EnumType)
        {
            return Convert.ToString((int)(object)EnumType);
        }
        #endregion

        #region  :: To Int & Decimal ::
        public static int ToInt(this object a)
        {
            try
            {
                return Convert.ToInt32(a);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static decimal ToDecimal(this object a)
        {
            try
            {
                return Convert.ToDecimal(a);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion
        #region  :: Cache Api for Get list of All message ::

        public static bool GetCache(IMemoryCache cache, string cacheKey, out object resObject)
        {
            return cache.TryGetValue(cacheKey, out resObject);
        }
        public static void SetCache(IMemoryCache cache, string cacheKey, object resObject)
        {
            MemoryCacheEntryOptions cacheExpirationOptions = new()
            {
                AbsoluteExpiration = DateTime.Now.AddHours(8),
                Priority = Microsoft.Extensions.Caching.Memory.CacheItemPriority.Normal
            };
            cache.Set(cacheKey, resObject, cacheExpirationOptions);
        }
        public static void RemoveCache(IMemoryCache cache, string cacheKey)
        {
            cache.Remove(cacheKey);
        }
        #endregion

        #region  :: Get Enum value with description ::
        public static List<SelectListItem> DropDownBindWithEnumDescription(Type enumType)
        {
            return enumType.ToSelectListItemsWithDesription();
        }
        public static List<SelectListItem> ToSelectListItemsWithDesription(this Type enumType)
        {
            List<SelectListItem> items = new();
            foreach (Enum cur in Enum.GetValues(enumType))
            {
                items.Add(new SelectListItem()
                {
                    Value = GetEnumValue(cur)
                });
            }
            return items;
        }
        #endregion
        #region  :: Get Enum value same text value ::
        public static List<SelectListItem> DropDownBindWithSameTextValue(Type enumType)
        {
            return enumType.ToSelectListItemsWithSameTextValue();
        }
        public static List<SelectListItem> ToSelectListItemsWithSameTextValue(this Type enumType)
        {
            List<SelectListItem> items = new();
            foreach (Enum cur in Enum.GetValues(enumType))
            {
                items.Add(new SelectListItem()
                {
                    Text = cur.ToString(),
                    Value = cur.ToString()
                });
            }
            return items;
        }
        #endregion

        public static string GetVal(string val, bool isSkipFirst = false)
        {
            return ((val?.Length ?? 0) == 1 && val.ToLower() == "x") || ((val?.Length ?? 0) > 1 && val.ToLower()[1..].Trim('x') == "") ? "" : val;
        }
    }
}
