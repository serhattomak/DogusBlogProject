using DogusProject.Application.Common;
using DogusProject.Application.Features.Categories.Dtos;
using MediatR;

namespace DogusProject.Application.Features.Categories.Queries;

public class GetPopularCategoriesQuery : IRequest<Result<List<PopularCategoryDto>>> { }