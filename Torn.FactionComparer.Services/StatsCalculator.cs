namespace Torn.FactionComparer.Services
{
    public interface IStatsCalculator
    {
        Stats CalculateStats(FactionCompareData first, FactionCompareData seccond);
    }

    public class StatsCalculator : IStatsCalculator
    {
        private string[] propSkip = new string[] { "ID", "Name" };

        public Stats CalculateStats(FactionCompareData first, FactionCompareData seccond)
        {
            var res = new Stats();

            var weightStatValue = new StatValue() { First = 0, Seccond = 0 };

            var properties = typeof(FactionCompareData).GetProperties();

            foreach (var prop in properties)
            {
                if (propSkip.Contains(prop.Name))
                    continue;

                var valueFirst = Convert.ToInt64(prop.GetValue(first));
                var valueSeccond = Convert.ToInt64(prop.GetValue(seccond));

                var statsProp = typeof(Stats).GetProperty(prop.Name);

                if (valueFirst > valueSeccond)
                {
                    statsProp.SetValue(res, new StatValue() { First = 100, Seccond = GetPercentageForSeccond(valueFirst, valueSeccond) });
                    weightStatValue.First++;
                }
                else
                {
                    statsProp.SetValue(res, new StatValue() { First = GetPercentageForFirst(valueFirst, valueSeccond), Seccond = 100 });
                    weightStatValue.Seccond++;
                }
            }

            res.Weight = weightStatValue;

            return res;
        }
        private int GetPercentageForSeccond(long first, long sec)
        {
            return (int)(100 * sec / first);
        }
        private int GetPercentageForFirst(long first, long sec)
        {
            return (int)(100 * first / sec);
        }
    }
}
