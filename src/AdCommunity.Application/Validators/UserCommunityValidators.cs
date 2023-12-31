﻿using AdCommunity.Application.DTOs.UserCommunity;
using AdCommunity.Application.Features.UserCommunity.Commands.CreateUserCommunityCommand;
using AdCommunity.Application.Features.UserCommunity.Commands.UpdateUserCommunityCommand;
using FluentValidation;

namespace AdCommunity.Application.Validators;

public class UserCommunityDtoValidator : AbstractValidator<UserCommunityDto>
{
    public UserCommunityDtoValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty().GreaterThan(0);
    }
}

public class CreateUserCommunityCommandValidator : AbstractValidator<CreateUserCommunityCommand>
{
    public CreateUserCommunityCommandValidator()
    {
        RuleFor(dto => dto.UserId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.CommunityId).NotEmpty().GreaterThan(0);
    }
}

public class UpdateUserCommunityCommandValidator : AbstractValidator<UpdateUserCommunityCommand>
{
    public UpdateUserCommunityCommandValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.UserId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.CommunityId).NotEmpty().GreaterThan(0);
    }
}