using FluentValidation;
using Lib.Model.Todo;

namespace Lib.Validators.Todo;

public class TodoRequestValidator : AbstractValidator<TodoRequest>
{
  public TodoRequestValidator()
  {
    RuleFor(t => t.Name)
      .MinimumLength(1)
      .MaximumLength(250);
  }
}