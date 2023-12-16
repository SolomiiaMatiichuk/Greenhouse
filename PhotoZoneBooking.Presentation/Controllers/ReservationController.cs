using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhotoZoneBooking.BLL.Dtos;
using PhotoZoneBooking.BLL.Interfaces;
using PhotoZoneBooking.DAL.Entities;

namespace PhotoZoneBooking.Presentation.Controllers;

public class ReservationController : BaseController
{
    private readonly IReservationService _reservationService;
    private readonly IPhotoZoneService _photoZoneService;
    private readonly IMailService _mailService;
    private readonly UserManager<User> _userManager;

    public ReservationController(IReservationService reservationService, IPhotoZoneService photoZoneService, IMailService mailService, UserManager<User> userManager)
    {
        _reservationService = reservationService;
        _photoZoneService = photoZoneService;
        _mailService = mailService;
        _userManager = userManager;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var userId = User?.Claims.SingleOrDefault(t => t.Type.Equals("id"))?.Value;
        var result = await _reservationService.GetUserReservationsAsync(userId);
        var user = await _userManager.FindByIdAsync(userId);
        ViewBag.User = user;
        return View(result);
    }
    
    [AllowAnonymous]
    public async Task<IEnumerable<ReservationDto>> GetPhotoZoneReservations(int photoZoneId)
    {
        return await _reservationService.GetPhotoZoneReservationsAsync(photoZoneId);
    }

    [AllowAnonymous]
    public async Task<IActionResult> PostReservation(int photoZoneId)
    {
        var viewModel = new ReservationDto();
        viewModel.Start = DateTime.Today;
        viewModel.End = DateTime.Today.AddDays(1);
        viewModel.PhotoZoneId = photoZoneId;
        return View(viewModel);
    }

    public IActionResult InvoiceSent()
    {
        return View();
    }
    
    public async Task<IActionResult> PayReservation(int id)
    {
        var result = await _reservationService.PayReservationAsync(id);
        var userEmail = User.Claims.SingleOrDefault(t => t.Type == ClaimTypes.Email).Value;
        await _mailService.SendInvoiceAsync(userEmail, result);
        return RedirectToAction("InvoiceSent");
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IEnumerable<ReservationDto>> GetPhotoZoneReservationsByTitleAsync(string title, int photoZoneId)
    {
        return await _reservationService.GetPhotoZoneReservationsByTitleAsync(title, photoZoneId);
    }

    public async Task<IActionResult> DeleteReservation(int id)
    {
        await _reservationService.DeleteReservationAsync(id);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> UpdateReservation(int id)
    {
        var reservation = await _reservationService.GetReservationAsync(id);
        return View(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateReservation(int id, int photoZoneId, ReservationDto model)
    {
        if (!ModelState.IsValid) return View(model);
        model.Id = id; 
        model.PhotoZoneId = photoZoneId;
        model.UserId = User?.Claims.SingleOrDefault(t => t.Type.Equals("id"))?.Value;
        var callback = await _reservationService.UpdateReservationAsync(model);
        if (callback.StatusCode != HttpStatusCode.BadRequest) return RedirectToAction("Index");
        ViewBag.ErrorMessage = callback.Error;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> PostReservation(int photoZoneId, ReservationDto model)
    {
        if (!ModelState.IsValid) return View(model);
        model.PhotoZoneId = photoZoneId;
        model.UserId = User?.Claims.SingleOrDefault(t => t.Type.Equals("id"))?.Value;
        var callback = await _reservationService.CreateReservationAsync(model);
        
        if (callback.StatusCode == HttpStatusCode.OK)
        {
            return RedirectToAction("Index");
        }
        
        ViewBag.ErrorMessage = callback.Error;
        return View(model);
    }
    
    public async Task<IActionResult> CancelReservation(int reservationId)
    {
        var userEmail = User?.Claims.SingleOrDefault(t => t.Type.Equals(ClaimTypes.Email)).Value;
        await _reservationService.CancelReservationAsync(reservationId);
        await _mailService.SendReservationCancelMailAsync(userEmail, reservationId);
        return RedirectToAction("Index");
    }
}