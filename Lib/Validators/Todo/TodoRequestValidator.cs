using FluentValidation;
using Lib.Model.Todo;

namespace Lib.Validators.Todo;

public class TodoRequestValidator : AbstractValidator<TodoRequest>
{
  public TodoRequestValidator()
  {
    RuleFor(t => t.Name)
      .NotEmpty()
      .MinimumLength(1)
      .MaximumLength(250);

    RuleFor(t => t.Status)
      .NotEmpty();
  }
}