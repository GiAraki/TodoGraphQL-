namespace TodoGraphQL.Application.DTOs;

public record TodoDto(
    string Id,
    string Title,
    bool IsCompleted,
    DateTime CreatedAt
);