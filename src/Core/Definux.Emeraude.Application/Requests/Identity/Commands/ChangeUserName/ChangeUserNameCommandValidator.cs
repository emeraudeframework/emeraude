﻿using Definux.Emeraude.Resources;
using FluentValidation;

namespace Definux.Emeraude.Application.Requests.Identity.Commands.ChangeUserName
{
    /// <summary>
    /// Validator for <see cref="ChangeUserNameCommand"/>.
    /// </summary>
    public class ChangeUserNameCommandValidator : AbstractValidator<ChangeUserNameCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeUserNameCommandValidator"/> class.
        /// </summary>
        public ChangeUserNameCommandValidator()
        {
            this.RuleFor(x => x.NewName)
                .NotEmpty()
                .WithMessage(Messages.NameIsARequiedField);
        }
    }
}