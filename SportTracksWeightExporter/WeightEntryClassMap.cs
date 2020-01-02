using CsvHelper.Configuration;

namespace SportTracksWeightExporter
{
    public sealed class WeightEntryClassMap : ClassMap<WeightEntry>
    {
        public WeightEntryClassMap()
        {
            Map(m => m.Date);
            Map(m => m.Weight);
        }
    }
}
