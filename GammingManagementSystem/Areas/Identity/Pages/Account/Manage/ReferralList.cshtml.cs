// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using GammingManagementSystem.Data;
using GammingManagementSystem.Dtos;
using GammingManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace GammingManagementSystem.Areas.Identity.Pages.Account.Manage
{
    public class ReferralList : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IEnumerable<Player> applicationUsers { get; set; }
        public ReferralList(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirst("UserId");
            var role = User.FindFirst("UserRole");
            var gameList = _context.Games.ToList();
            if (role.Value == "Admin")
            {
                applicationUsers = _context.Players.Where(x => x.ReferralCode != null).ToList();
            }
            else
            {
                var agent = _context.Users.Where(x => x.Id == userId.Value).FirstOrDefault();
                applicationUsers = _context.Players.Where(x => x.ReferralCode == agent.ReferalCode).ToList();
            }
            foreach (var dtRow in applicationUsers)
            {
                string games = string.Empty;

                if (dtRow.GameInterested != null)
                {
                    var arrY = dtRow.GameInterested.Split(",");
                    ApplicationUserDto customer = new ApplicationUserDto();
                    int index = 1;
                    for (int i = 0; i < arrY.Length; i++)
                    {
                        if (games == null || games == "")
                        {
                            games = $"({index})  " + gameList.Where(x => x.Id == Convert.ToInt32(arrY[i])).Select(x => x.GameName).FirstOrDefault();
                        }
                        else
                        {
                            games += $"({index})" + gameList.Where(x => x.Id == Convert.ToInt32(arrY[i])).Select(x => x.GameName).FirstOrDefault();
                        }
                        index++;
                    }
                    dtRow.GameInterested = games;
                }
            }



            return Page();
        }
    }
}
