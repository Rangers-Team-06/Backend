using Team_06.Data.DTOs;

namespace Team_06.Tests.Helpers
{
    public static class TestDataHelper
    {
        public static List<int> CreateProductIdList(int count, int startId = 1)
        {
            return Enumerable.Range(startId, count).ToList();
        }
    }
}
