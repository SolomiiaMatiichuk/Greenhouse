using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoZoneBooking.BLL.Dtos;
using PhotoZoneBooking.BLL.Interfaces;

namespace PhotoZoneBooking.Presentation.Controllers;

public class PhotoZoneController : BaseController
{
    private readonly IPhotoZoneService _photoZoneService;
    private readonly IReservationService _reservationService;

    public PhotoZoneController(IPhotoZoneService photoZoneService, IReservationService reservationService)
    {
        _photoZoneService = photoZoneService;
        _reservationService = reservationService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> PhotoZoneWithDetail(int id, string? query)
    {
        var field = await _photoZoneService.GetPhotoZoneWithDetailsAsync(id);
        var reservations = await _reservationService.GetPhotoZoneReservationsByTitleAsync(query, id);
        ViewBag.SearchedReservations = reservations;
        return View(field);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var photoZones = await _photoZoneService.GetPhotoZonesWithDetailsAsync();
        return View(photoZones);
    }
    
    public IActionResult PostPhotoZone()
    {
        return View();
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> PostPhotoZone(PhotoZoneDto model)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await _photoZoneService.CreatePhotoZone(model);
        if (result.StatusCode != HttpStatusCode.BadRequest) return RedirectToAction("Index");
        ViewBag.ErrorMessage = result.Error;
        return View(model);
    }
    
    public async Task<IActionResult> DeletePhotoZone(int id)
    {
        await _photoZoneService.DeletePhotoZone(id);
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> UpdatePhotoZone(int id)
    {
        var photoZone = await _photoZoneService.GetPhotoZoneWithDetailsAsync(id);
        return View(photoZone);
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdatePhotoZone(PhotoZoneDto model)
    {
        if (!ModelState.IsValid) return View(model);
        await _photoZoneService.UpdatePhotoZone(model);
        return RedirectToAction("Index");
    }
}