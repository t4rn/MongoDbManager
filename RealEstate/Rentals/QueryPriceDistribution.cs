using MongoDB.Driver;
using System.Collections;
using System.Linq;

namespace RealEstate.Rentals
{
    internal class QueryPriceDistribution
    {
        public IEnumerable RunAggregation(IMongoCollection<Rental> rentals)
        {
            var distributions = rentals.Aggregate()
                .Project(r => new { PriceRange = r.Price - (r.Price % 500) })
                .Group(r => r.PriceRange, g => new { GroupPriceRange = g.Key, Count = g.Count() })
                .SortBy(r => r.GroupPriceRange)
                .ToList();

            return distributions;
        }

        public IEnumerable RunLinq(IMongoCollection<Rental> rentals)
        {
            var distributions = rentals.AsQueryable()
                .Select(r => new { PriceRange = r.Price - (r.Price % 500) })
                .GroupBy(r => r.PriceRange)
                .Select(g => new { GroupPriceRange = g.Key, Count = g.Count() })
                .OrderBy(r => r.GroupPriceRange)
                .ToList();
            return distributions;
        }
    }
}