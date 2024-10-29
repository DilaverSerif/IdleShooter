using System.Collections.Generic;
using System.IO;
using UnityEditor;
namespace _BASE_.Scripts.Extensions
{
    public static class EnumCreator
    {
#if UNITY_EDITOR
        public static void GenerateEnumFile(this string[] values, string enumName, string enumPath, string tag)
        {
            // Eğer dosya yoksa, yeni bir dosya oluştur.
            if (!File.Exists(enumPath))
            {
                // Boş bir dosya oluştur
                using (FileStream fs = File.Create(enumPath))
                {
                    // FileStream kapatılacak ve boş bir dosya oluşturulacak
                }
            }
            File.WriteAllText(enumPath, string.Empty); 

            var filePath = Path.Combine(enumPath);

            for (int index = 0; index < values.Length; index++)
            {
                string value = values[index];
                string replace = value.Replace(" ", "_");
                values[index] = replace;
            }

            using (var writer = new StreamWriter(filePath, true))
            {
                //clear the file
                
                writer.WriteLine($"public enum {tag}{enumName}");
                writer.WriteLine("{ None = -1,");

                for (var i = 0; i < values.Length; i++)
                    writer.WriteLine($"    {values[i]} = {i},");

                writer.WriteLine("}");
            }

            AssetDatabase.Refresh();
        }
    
        public static void GenerateEnumFile(this List<string> values, string enumName)
        {
            GenerateEnumFile(values,enumName);
        }

#endif
    }
}