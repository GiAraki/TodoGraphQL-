using TodoGraphQL.Application.UseCases.Auth;
using TodoGraphQL.Application.UseCases.Todos;

namespace TodoGraphQL.API.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<GetTodosUseCase>();
        services.AddScoped<AddTodoUseCase>();
        services.AddScoped<CompleteTodoUseCase>();
        services.AddScoped<DeleteTodoUseCase>();
        services.AddScoped<RegisterUseCase>();
        services.AddScoped<LoginUseCase>();
        return services;
    }
}