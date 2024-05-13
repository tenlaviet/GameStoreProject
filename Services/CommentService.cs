using AspMVC.Data;
using AspMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace AspMVC.Services
{
    public class CommentService
    {
        private readonly AppDbContext _context;

        public CommentService(AppDbContext db)
        {
            _context = db;
        }
        public async Task AddCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetComments(int PageID)
        {
            var commentList = await _context.Comments.Where(c => c.ProjectPageId == PageID).Include(c=> c.User).ToListAsync();
            return commentList;
        }

        //public List<ReplyComment> GetReplyComments(int entityID, int recordID)
        //{
        //    return db.ReplyComments.Where(x => x.Comment.EntityID == entityID && x.Comment.RecordID == recordID).Include(x => x.User).Include(x => x.Comment).ToList();
        //}

        //public ReplyComment GetAllReplyCommentByID(int ID)
        //{
        //    return db.ReplyComments.Find(ID);
        //}

        //public bool UpdateReplyComment(ReplyComment replycomment)
        //{
        //    db.Entry(replycomment).State = EntityState.Modified;
        //    return db.SaveChanges() > 0;
        //}

        //public bool SaveReplyComment(ReplyComment replycomment)
        //{
        //    db.ReplyComments.Add(replycomment);
        //    return db.SaveChanges() > 0;
        //}

        //public bool DeleteAccomodationType(ReplyComment replycomment)
        //{
        //    db.Entry(replycomment).State = EntityState.Deleted;

        //    return db.SaveChanges() > 0;
        //}

        //public Comment GetCommentByID(int ID)
        //{
        //    return db.Comments.Find(ID);
        //}

        //public List<ReplyCommentLove> GetAllReplyCommentLoveByID(int ID, string userID)
        //{
        //    return db.ReplyCommentLoves.Where(x => x.ReplyCommentID == ID && x.UserID == userID).ToList();
        //}

        //public bool SaveReplyCommentLike(ReplyCommentLove replycommentlove)
        //{
        //    db.ReplyCommentLoves.Add(replycommentlove);
        //    return db.SaveChanges() > 0;
        //}

        //public bool UpdateComment(Comment comment)
        //{
        //    db.Entry(comment).State = EntityState.Modified;

        //    return db.SaveChanges() > 0;
        //}
    }
}
