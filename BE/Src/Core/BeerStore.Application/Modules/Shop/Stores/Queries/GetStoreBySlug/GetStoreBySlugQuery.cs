using BeerStore.Application.DTOs.Shop.Store.Responses;
using MediatR;

namespace BeerStore.Application.Modules.Shop.Stores.Queries.GetStoreBySlug
{
    public record GetStoreBySlugQuery(string Slug) : IRequest<StorePublicResponse?>;
}
