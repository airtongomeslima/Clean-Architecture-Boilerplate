using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Test.MSTest.Extensions
{
    public static class AssertExtensions
    {
        public static void AreEqualByValue<T>(T expected, T actual)
        {
            if (expected == null && actual == null) return;
            if (expected == null || actual == null)
                throw new Exception($"Expected: {expected}, Actual: {actual}");

            var type = typeof(T);

            if (type.IsValueType || type == typeof(string))
            {
                Assert.AreEqual(expected, actual);
            }
            else if (type.IsArray)
            {
                var expectedArray = (Array)(object)expected;
                var actualArray = (Array)(object)actual;

                Assert.AreEqual(expectedArray.Length, actualArray.Length, "Array length mismatch");

                for (int i = 0; i < expectedArray.Length; i++)
                {
                    AreEqualByValue(expectedArray.GetValue(i), actualArray.GetValue(i));
                }
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var expectedList = (IList)(object)expected;
                var actualList = (IList)(object)actual;

                Assert.AreEqual(expectedList.Count, actualList.Count, "List count mismatch");

                for (int i = 0; i < expectedList.Count; i++)
                {
                    AreEqualByValue(expectedList[i], actualList[i]);
                }
            }
            else
            {
                var properties = type.GetProperties();

                foreach (var prop in properties)
                {
                    var expectedValue = prop.GetValue(expected);
                    var actualValue = prop.GetValue(actual);

                    AreEqualByValue(expectedValue, actualValue);
                }
            }
        }

    }
}
