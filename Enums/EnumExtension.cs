using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VueMvc.Enums
{
    public static class EnumExtension
    {
        public static Nullable<T> Parse<T> (string value) where T : struct
        {
            try 
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch
            {
                return null;
            }
        }
    }
}