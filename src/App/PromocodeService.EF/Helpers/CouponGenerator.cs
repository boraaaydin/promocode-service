using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PromocodeService.EF.Helpers
{
    public class CouponGenerator
    {
        public List<string> GenerateCode(int quantity, int length)
        {
            int couponCount = 0;
            var random = new Random();
            var list = new List<string>();
            for (int j = 0; j < quantity; j++)
            {
                const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
                var builder = new StringBuilder();

                for (var i = 0; i < length; i++)
                {
                    var c = pool[random.Next(0, pool.Length)];
                    builder.Append(c);
                }

                list.Add(builder.ToString());

                couponCount++;
                if (couponCount % 10000 == 0)
                {
                    Debug.WriteLine("Oluşturulan kupon adedi: " + couponCount);
                }
            }

            return list;
        }

        public List<string> RemoveDublicateCode(List<string> list, out int deletedCount)
        {
            var initialCount = list.Count;
            deletedCount = 0;
            var duplicateKeys = list.GroupBy(x => x)
                        .Where(group => group.Count() > 1)
                        .Select(group => group.Key).ToList();

            foreach (var dublicateKey in duplicateKeys)
            {
                list.Remove(dublicateKey);
            }
            deletedCount = initialCount - list.Count;
            return list;
        }
    }
}
