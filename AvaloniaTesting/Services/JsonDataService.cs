using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using static AvaloniaTestingProgramm.Entities;

namespace AvaloniaTestingProgramm
{
    //Чтение и запись json файла
    public static class JsonDataService
    {
        private static string ResultsfilePath = "Results.json";
        //
        internal static ResultsDatabase LoadData()
        {
            if (!File.Exists(ResultsfilePath))
                return new ResultsDatabase();

            string json = File.ReadAllText(ResultsfilePath);
            return JsonSerializer.Deserialize<ResultsDatabase>(json) ?? new ResultsDatabase();
        }
        internal static void SaveData(ResultsDatabase data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(ResultsfilePath, json);
        }
        // - все результаты - 1 Json

        //
        internal static TestsDatabase LoadData(string TestFilepath) //Не каждая загрузка - новый тест
        {
            if (!File.Exists(TestFilepath))
                return new TestsDatabase();

            string json = File.ReadAllText(TestFilepath);
            return JsonSerializer.Deserialize<TestsDatabase>(json) ?? new TestsDatabase();
        }
        internal static void SaveData(TestsDatabase data, string newTestFilepath) //Но каждый новый тест - новый JSON
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(newTestFilepath, json);
        }
        //
    }
}
