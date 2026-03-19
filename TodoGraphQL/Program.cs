using TodoGraphQL.Models;
using TodoGraphQL.Types;

var builder = WebApplication.CreateBuilder(args);

// Registra o repositório como Singleton (uma instância pra toda a app)
builder.Services.AddSingleton<TodoRepository>();

// Configura o servidor GraphQL com nossa Query e Mutation
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

var app = builder.Build();

// Expõe o endpoint GraphQL em /graphql
app.MapGraphQL();

app.Run();