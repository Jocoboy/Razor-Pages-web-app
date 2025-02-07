﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;

        public IndexModel(RazorPagesMovie.Data.RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        //Genres allows the user to select a genre from the list. 
        public SelectList Genres { get; set; }
        [BindProperty(SupportsGet = true)]
        public string MovieGenre { get; set; }

        //When a request is made for the page, the OnGetAsync method returns a list of movies to the Razor Page.
        public async Task OnGetAsync()
        {

            //The query is only defined at this point, it has not been run against the database

            
            IQueryable<string> genreQuery = from m in _context.Movie
                                                                    orderby m.Genre
                                                                    select m.Genre;

            
            var movies = from m in _context.Movie
                         select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                //Console.WriteLine("SearchString:  " + SearchString);
                movies = movies.Where(s => s.Title.Contains(SearchString));
                //Console.WriteLine("SearchResult:  " + movies.Count());
            }

            if (!String.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(x => x.Genre == MovieGenre);
            }
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            //Movie = await _context.Movie.ToListAsync();
            Movie = await movies.ToListAsync();
        }
    }
}
