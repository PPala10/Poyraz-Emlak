namespace AirBnb.Models.Template;

public class TemplateDashboardViewModel
{
    public IReadOnlyList<TemplateKpiItem> Kpis { get; init; } = [];

    public IReadOnlyList<TemplateListingSummary> RecentListings { get; init; } = [];

    public IReadOnlyList<TemplateReservationSummary> RecentReservations { get; init; } = [];
}

public class TemplateKpiItem
{
    public required string Label { get; init; }

    public required string Value { get; init; }
}

public class TemplateListingSummary
{
    public required string Title { get; init; }

    public required string City { get; init; }

    public required string PricePerNight { get; init; }

    public required string Status { get; init; }
}

public class TemplateReservationSummary
{
    public required string GuestName { get; init; }

    public required string ListingTitle { get; init; }

    public required string DateRange { get; init; }

    public required string TotalPrice { get; init; }

    public required string Status { get; init; }
}
