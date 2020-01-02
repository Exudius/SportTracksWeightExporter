using CsvHelper;
using CsvHelper.TypeConversion;
using FlaUI.Core;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SportTracksWeightExporter
{
    class Program
    {
        public static object TypeConverterOptionsFactory { get; private set; }

        static void Main(string[] args)
        {
            var app = Application.Attach("SportTracks.exe");
            using (var automation = new UIA3Automation())
            {
                var window = app.GetMainWindow(automation);
                var historyList = window.FindFirstDescendant(e => e.ByAutomationId("historyList"));
                var dateBanner = window.FindFirstDescendant(e => e.ByAutomationId("leftSideBanner")).Patterns.LegacyIAccessible.Pattern;
                var weightInput = window.FindFirstDescendant(e => e.ByAutomationId("weightTextBox")).Patterns.LegacyIAccessible.Pattern;
                historyList.Focus();
                Keyboard.Type(VirtualKeyShort.HOME);
                DateTime? previousDate = null;
                var weightEntries = new List<WeightEntry>();
                while (true)
                {
                    Keyboard.Type(VirtualKeyShort.DOWN);
                    historyList.Focus();
                    var dateTimeString = dateBanner.Name.Value.Split(':')[0];
                    var weightString = weightInput.Name.Value.Split(' ')[0];
                    if (!weightString.Contains(":"))
                    {
                        var weightEntry = new WeightEntry()
                        {
                            Date = DateTime.Parse(dateTimeString),
                            Weight = decimal.Parse(weightString)
                        };
                        if (weightEntry.Date == previousDate)
                        {
                            break;
                        }
                        weightEntries.Add(weightEntry);
                        previousDate = weightEntry.Date;
                    }
                }

                using (var writer = new StreamWriter("weight.csv"))
                {
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.Configuration.CultureInfo = CultureInfo.InvariantCulture;
                        csv.Configuration.HasHeaderRecord = true;
                        csv.Configuration.Delimiter = ";";
                        csv.Configuration.RegisterClassMap<WeightEntryClassMap>();
                        var options = new TypeConverterOptions { Formats = new[] { "yyyy-MM-dd" } };
                        csv.Configuration.TypeConverterOptionsCache.AddOptions<DateTime>(options);
                        csv.WriteRecords(weightEntries);
                    }
                }
            }
        }
    }
}
