using BlogApp.Core.DTOs;
using BlogApp.Core.IRepository;
using BlogApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlogApp.EF.Repository
{
    public class CommentRepository : ICommentRepository
    {
        AppDbContext _context;
        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async void AddComment(AddCommentDTO commentDto,string UserId , int postId)
        {
            var comment = new Comment
            {
                PostId = postId,
                UserId = UserId,
                content = commentDto.content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();


        }
        public List<ShowCommentDTO> ShowCommentsOfPost(int postId,string currentUserId) 
        {
            var comments = _context.Comments
        .Where(p => p.PostId == postId)
        .Include(c => c.User)
        .ToList();               

                return comments.Select(c => {
                var isEdited = (c.UpdatedAt - c.CreatedAt).TotalMinutes > 1;
                    return new ShowCommentDTO
                    {
                        Id = c.Id,
                        content = c.content,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt,
                        UserId = c.UserId,
                        FirstName = c.User.FirstName,
                        username = c.User.UserName,
                        isAuthor = c.UserId == currentUserId,
                        isEdited = isEdited,
                    };
            }).ToList();


        }
        public async Task<Dictionary<int, int>> GetCommentCountsForPostsAsync(List<int> postIds)
        {
            return await _context.Comments
                .Where(pl => postIds.Contains(pl.PostId))
                .GroupBy(pl => pl.PostId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<Comment> GetComment(int id)
        {
            return await _context.Comments.FirstOrDefaultAsync(p => p.Id == id);

        }
        public async Task<bool> UpdateComment(Comment comment)
        {

            try
            {
                _context.Comments.Update(comment);
                return await _context.SaveChangesAsync() > 0;

            }
            catch
            {

                return false;
            }
        }

    }
}
