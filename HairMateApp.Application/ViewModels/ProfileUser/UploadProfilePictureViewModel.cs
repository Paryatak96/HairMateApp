﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairMateApp.Application.ViewModels.ProfileUser
{
    public class UploadProfilePictureViewModel
    {
        public IFormFile File { get; set; }
    }
}
