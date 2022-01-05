﻿using Blog.Data;
using Blog.Data.Models;
using Blog.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Service
{
    public class BlogService : IBlogService {
        private readonly ApplicationDbContext applicationDbContext;

        public BlogService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public Blok GetBlok(int blokId)
        {
            return applicationDbContext.Bloks.FirstOrDefault(blok => blok.Id == blokId);
        }
        public IEnumerable<Blok> GetBloks(ApplicationUser applicationUser) {
            return applicationDbContext.Bloks
                  .Include(blok => blok.Creator)
                  .Include(blok => blok.Approver)
                  .Include(blok => blok.Posts)
                  .Where(blok => blok.Creator == applicationUser);
        }
        public async Task<Blok> Add(Blok blok)
        {
            applicationDbContext.Add(blok);
            await applicationDbContext.SaveChangesAsync();
            return blok;
        }

        public async Task<Blok> Update(Blok blok)
        {
            applicationDbContext.Update(blok);
            await applicationDbContext.SaveChangesAsync();
            return blok;
        }
    }
 }
