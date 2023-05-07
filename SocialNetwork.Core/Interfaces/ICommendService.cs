using System;
using SocialNetwork.Core.Entities;
using SocialNetwork.Core;

namespace SocialNetwork.Core.Interfaces
{
    public interface ICommendService
    {
        OperationResult<Comment> Create(Comment comment);

        OperationResult<Comment> GetById(int id, int postId);
    }
}