using ChineseRaffleApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChineseRaffleApi.Controllers.DI
{
    public interface IDonorController
    {
        Task<ActionResult<Donor>> GetDonor(int id);
        Task<ActionResult<IEnumerable<Donor>>> GetAllDonors();
        Task<ActionResult<Donor>> AddDonor(Donor donor);
        Task<IActionResult> UpdateDonor(int id, Donor donor);
        Task<IActionResult> DeleteDonor(int id);
        Task<ActionResult<bool>> DonorExists(string username);

    }
}
