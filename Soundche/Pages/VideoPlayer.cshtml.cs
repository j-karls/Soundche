using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Soundche.Pages.Shared
{
    public class VideoPlayerModel : PageModel
    {
        public readonly RoomController RoomController;

        public VideoPlayerModel(RoomController roomController)
        {
            RoomController = roomController; // RoomController is our singleton - ergo only one room is active at a time
        }

        public void OnGet()
        {

        }

/*        public async Task OnGetAsync()
        {
            // await 
        }
*/    }
}