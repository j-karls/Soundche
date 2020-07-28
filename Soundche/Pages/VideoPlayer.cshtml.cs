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
        private readonly RoomController _roomController;

        public VideoPlayerModel(RoomController roomController)
        {
            _roomController = roomController;
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