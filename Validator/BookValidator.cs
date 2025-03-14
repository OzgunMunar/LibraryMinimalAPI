using FluentValidation;
using LibraryMinimalAPI.Models;

namespace LibraryMinimalAPI.Validator;
public sealed class BookValidator:AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(p => p.Isbn).MinimumLength(13).MaximumLength(13);
        RuleFor(p => p.Title).MinimumLength(4);
        RuleFor(p => p.ShortDescription).MinimumLength(3);
        RuleFor(p => p.PageCount).GreaterThan(100);
    }
}