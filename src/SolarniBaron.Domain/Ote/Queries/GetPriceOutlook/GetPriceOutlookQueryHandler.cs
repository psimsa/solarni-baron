using DotnetDispatcher.Core;
using SolarniBaron.Domain.Clustering;
using SolarniBaron.Domain.Contracts;
using SolarniBaron.Domain.Ote.Models;
using SolarniBaron.Domain.Ote.Queries.GetPricelist;

namespace SolarniBaron.Domain.Ote.Queries.GetPriceOutlook;

public class GetPriceOutlookQueryHandler : IQueryHandler<GetPriceOutlookQuery, Result<GetPriceOutlookQueryResponse>>
{
    private readonly ISolarniBaronDispatcher _dispatcher;
    private readonly IPriceClusteringWorker _priceClusteringWorker;

    public GetPriceOutlookQueryHandler(ISolarniBaronDispatcher dispatcher, IPriceClusteringWorker priceClusteringWorker)
    {
        _dispatcher = dispatcher;
        _priceClusteringWorker = priceClusteringWorker;
    }

    public async Task<Result<GetPriceOutlookQueryResponse>> Query(GetPriceOutlookQuery getPriceOutlookQuery,
        CancellationToken cancellationToken = default)
    {
        if (getPriceOutlookQuery == null)
        {
            throw new ArgumentNullException(nameof(getPriceOutlookQuery));
        }

        var pragueDateTimeNow = getPriceOutlookQuery.DateTime;
        var today = await _dispatcher.Dispatch(new GetPricelistQuery(DateOnly.FromDateTime(pragueDateTimeNow)), cancellationToken);
        var tomorrow = await _dispatcher.Dispatch(new GetPricelistQuery(DateOnly.FromDateTime(pragueDateTimeNow.AddDays(1))),
            cancellationToken);

        if (today?.HourlyRateBreakdown == null)
        {
            return new Result<GetPriceOutlookQueryResponse>("No pricelist for today");
        }

        if (tomorrow?.HourlyRateBreakdown == null)
        {
            return new Result<GetPriceOutlookQueryResponse>(new GetPriceOutlookQueryResponse(today.HourlyRateBreakdown));
        }

        var lookAheadList = today?.HourlyRateBreakdown?.Where(_ => _.HourIndex >= pragueDateTimeNow.Hour)
            .Concat(tomorrow.HourlyRateBreakdown)
            .Take(24).ToList()!;

        var sortedItemsWithOriginalIndex = lookAheadList.Select((item, index) => new KeyValuePair<int, PriceListItem>(index, item))
            .OrderBy(_ => _.Value.BasePriceEur).ToList();

        var clusters = _priceClusteringWorker.GetClusters(sortedItemsWithOriginalIndex.Select(_ => _.Value.BasePriceEur).ToArray(), 4);

        for (int i = 0; i < 24; i++)
        {
            var indexInOriginalList = sortedItemsWithOriginalIndex[i].Key;
            lookAheadList[indexInOriginalList] = lookAheadList[indexInOriginalList] with {PriceCluster = clusters[i], PriceScore = i};
        }

        return new Result<GetPriceOutlookQueryResponse>(new GetPriceOutlookQueryResponse(lookAheadList));
    }
}
