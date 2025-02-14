using api.Core.Entities;
using api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace api.Tests.Infrastructure.Data;

public class CommentRepositoryTests
{
    private static Task<ApplicationDbContext> GetDatabaseContext() => TestDatabaseHelper.GetDatabaseContext();
    
    [Fact]
    public async Task AddComment_ShouldAddComment()
    {
        var context = await GetDatabaseContext();
        var comment = new Comment { Title = "Test Comment", Content = "Test Content", StockId = 1, AppUserId = "6c0b1d17-0fa8-491c-92ed-029ea7c96889" };
        context.Comments.Add(comment);
        await context.SaveChangesAsync();

        var addedComment = await context.Comments.FirstOrDefaultAsync(c => c.Title == "Test Comment");
        Assert.NotNull(addedComment);
        Assert.Equal(1, addedComment.StockId);
    }
    
    [Fact]
    public async Task GetCommentById_ShouldReturnComment()
    {
        var context = await GetDatabaseContext();
        var comment = new Comment { Title = "Test Comment", Content = "Test Content", StockId = 1, AppUserId = "6c0b1d17-0fa8-491c-92ed-029ea7c96889" };
        context.Comments.Add(comment);
        await context.SaveChangesAsync();

        var retrievedComment = await context.Comments.FindAsync(comment.Id);
        Assert.NotNull(retrievedComment);
        Assert.Equal("Test Comment", retrievedComment.Title);
    }

    [Fact]
    public async Task UpdateComment_ShouldUpdateComment()
    {
        var context = await GetDatabaseContext();
        var comment = new Comment { Title = "Test Comment", Content = "Test Content", StockId = 1, AppUserId = "6c0b1d17-0fa8-491c-92ed-029ea7c96889" };
        context.Comments.Add(comment);
        await context.SaveChangesAsync();
    
        comment.Title = "Updated Comment";
        context.Comments.Update(comment);
        await context.SaveChangesAsync();
    
        var updatedComment = await context.Comments.FindAsync(comment.Id);
        Assert.NotNull(updatedComment);
        Assert.Equal("Updated Comment", updatedComment.Title);
    }
    
    [Fact]
    public async Task DeleteComment_ShouldRemoveComment()
    {
        var context = await GetDatabaseContext();
        var comment = new Comment { Title = "Test Comment", Content = "Test Content", StockId = 1, AppUserId = "6c0b1d17-0fa8-491c-92ed-029ea7c96889" };
        context.Comments.Add(comment);
        await context.SaveChangesAsync();
    
        context.Comments.Remove(comment);
        await context.SaveChangesAsync();
    
        var deletedComment = await context.Comments.FindAsync(comment.Id);
        Assert.Null(deletedComment);
    }
}
