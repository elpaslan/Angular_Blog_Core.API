﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularBlog.Core.Api.Models;
using AngularBlog.Core.Api.Responses;

namespace AngularBlog.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly AngularBlogDbContext _context;

        public ArticlesController(AngularBlogDbContext context)
        {
            _context = context;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            return await _context.Articles.ToListAsync();
        }

        // GET: api/Articles/1/5
        [HttpGet]
        public IActionResult GetArticle()
        {
            var articles = _context.Articles.Include(a => a.Category).Include(b => b.Comments).OrderByDescending(x => x.PublishDate).ToList().Select(y => new ArticleResponse()
            {
                Id = y.Id,
                Title = y.Title,
                Picture = y.Picture,
                Category = new CategoryResponse() { Id = y.Category.Id, Name = y.Category.CategoryName },
                CommentCount = y.Comments.Count,

                ViewCount = y.ViewCount,
                PublishDate = y.PublishDate
            });
            return Ok(articles);
        }


        // GET: api/Articles/5
        [HttpGet("{id}")]
        public IActionResult GetArticle(int id)
        {
            System.Threading.Thread.Sleep(2000);

            var article = _context.Articles.Include(x => x.Category).Include(y => y.Comments).FirstOrDefault(z => z.Id == id);

            if (article == null)
            {
                return NotFound();
            }
            ArticleResponse articleResponse = new ArticleResponse()
            {
                Id = article.Id,
                Title = article.Title,
                ContentMain = article.ContentMain,
                ContentSummary = article.ContentSummary,
                Picture = article.Picture,
                PublishDate = article.PublishDate,
                ViewCount = article.ViewCount,
                Category = new CategoryResponse() { Id = article.Category.Id, Name = article.Category.CategoryName },
                CommentCount = article.Comments.Count
            };

            return Ok(articleResponse);
        }


      

        [HttpGet("{page}/{pageSize}")]
        public IActionResult GetArticle(int page = 1, int pageSize = 5)
        {
            System.Threading.Thread.Sleep(3000);

            try
            {
                IQueryable<Article> query;

                query = _context.Articles.Include(x => x.Category).Include(y => y.Comments).OrderByDescending(z => z.PublishDate);

                int totalCount = query.Count();

                // 5*(1-1) => 0
                //5*(2-1)=>5
                var articlesResponse = query.Skip((pageSize * (page - 1))).Take(5).ToList().Select(x => new ArticleResponse()
                {
                    Id = x.Id,
                    Title = x.Title,
                    ContentMain = x.ContentMain,
                    ContentSummary = x.ContentSummary,
                    Picture = x.Picture,
                    ViewCount = x.ViewCount,
                    CommentCount = x.Comments.Count,
                    Category = new CategoryResponse() { Id = x.Category.Id, Name = x.Category.CategoryName }
                });

                var result = new
                {
                    TotalCount = totalCount,
                    Articles = articlesResponse
                };
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            if (id != article.Id)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Articles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticle", new { id = article.Id }, article);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}