﻿namespace Leads.Domain.Users.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions;
    using Exceptions;
    using Infrastructure.Commands.Builders.Abstractions;
    using Infrastructure.Commands.Contexts.Common.Extensions;
    using Infrastructure.Queries.Builders.Abstractions;
    using Objects.Entities;
    using Queries.Criteria;


    public class UserService : IUserService
    {
        private readonly IAsyncQueryBuilder _asyncQueryBuilder;
        private readonly IAsyncCommandBuilder _asyncCommandBuilder;


        public UserService(IAsyncQueryBuilder asyncQueryBuilder, IAsyncCommandBuilder asyncCommandBuilder)
        {
            _asyncQueryBuilder = asyncQueryBuilder ?? throw new ArgumentNullException(nameof(asyncQueryBuilder));
            _asyncCommandBuilder = asyncCommandBuilder ?? throw new ArgumentNullException(nameof(asyncCommandBuilder));
        }


        public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            var existingUser = await _asyncQueryBuilder
                .For<User>()
                .WithAsync(new FindByEmail(user.Email), cancellationToken);
            
            if (existingUser != null)
                throw new UserAlreadyExistsException(); // TODO : message

            await _asyncCommandBuilder.CreateAsync(user, cancellationToken);
        }
    }
}