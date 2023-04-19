// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using GammingManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using QRCoder;
using System.Drawing;

namespace GammingManagementSystem.Areas.Identity.Pages.Account.Manage
{
    public class ReferralDetailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ReferralDetailsModel(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string URL { get; set; }
            public byte[] ReferralQRImage { get; set; }
        }

        private static Random random = new Random();


        public async Task<IActionResult> OnGetAsync()
        {
            string referralCode = "";
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (string.IsNullOrEmpty(user.ReferalCode))
            {
                referralCode = ReferralCode(6);
                user.ReferalCode = referralCode;
                await _userManager.UpdateAsync(user);
            }

            //var URL = "https://localhost:7029/Identity/Account/Register?referralCode=" + user.ReferalCode;
            var URL = "https://localhost:7029/Home/PlayerRegistration?referralCode=" + user.ReferalCode;

        
            Input = new InputModel { URL = URL };

            QRCodeGenerator _qrCode = new QRCodeGenerator();
            QRCodeData _qrCodeData = _qrCode.CreateQrCode(URL, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            Input.ReferralQRImage = BitmapToBytesCode(qrCodeImage);

            return Page();
        }

        [NonAction]
        private static Byte[] BitmapToBytesCode(Bitmap image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        private string ReferralCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
