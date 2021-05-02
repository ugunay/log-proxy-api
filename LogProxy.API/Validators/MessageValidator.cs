using FluentValidation;
using LogProxy.API.Entities;

namespace LogProxy.API.Validators
{
    public class MessageValidator : AbstractValidator<Message>
    {
        public MessageValidator()
        {
            RuleFor(c => c.Title).NotNull();
            RuleFor(c => c.Text).NotNull();
        }
    }
}