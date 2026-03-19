using TodoGraphQL.Models;

namespace TodoGraphQL.Types;

public class Mutation
{
    // Quando alguém chamar "addTodo", cria e retorna a nova tarefa
    public Todo AddTodo(string title, [Service] TodoRepository repo)
        => repo.Add(title);
}